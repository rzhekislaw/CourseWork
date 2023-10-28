using Npgsql;
using CourseWork.Tools;
using CourseWork.NorthWind_Tables__Structs;
using CourseWork.Extentions;
using CourseWork.CRUD_on_Tasks;

namespace CourseWork
{
    public class Caller
    {
        public static void Main()
        {
            NpgsqlDataSource DBconnection = null;

            string username = string.Empty;

            while (DBconnection == null || username == string.Empty)
            {
                DBconnection = OnInitialize(out username);
            }

            do
            {
                Console.Clear();
                Console.WriteLine("Enter \'q\' to exit, \'s\' to show all data, \'a\' to add new task \'u\' to update task \'d\' to delete task");
                var input = Console.ReadLine();
                if (input?.ToUpper() == "Q")
                {
                    return;
                }
                if (input?.ToUpper() == "S")
                {
                    new Crud().ShowExistingTasks(DBconnection, username);
                }
                if (input?.ToUpper() == "A")
                {
                    new Crud().AddTask(DBconnection, username);
                }
                if (input?.ToUpper() == "U")
                {
                    PrepareUpdate(DBconnection, username);
                }
                if (input?.ToUpper() == "D")
                {
                    PrepareDelete(DBconnection, username);
                }
            }
            while (true);
        }

        public static NpgsqlDataSource OnInitialize(out string username)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Login: ");
            var login = Console.ReadLine();
            Console.WriteLine("Password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            var password = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            var user = new User() { Login = login, Password = password };

            var connection = $"Host={PublicConstants.Host};Username={user.Login};Password={user.Password};Database={PublicConstants.DataBase}";
            var DBconnection = NpgsqlDataSource.Create(connection);

            try
            {
                var testConnection = new SelectBuilder(DBconnection, "select 1").GetScalar().ToInt();              
            }
            catch
            {
                Console.WriteLine("Authorization failure");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                DBconnection = null;
                username = string.Empty;
                return DBconnection;
            }
            Console.WriteLine($"Connected to {PublicConstants.DataBase} as {user.Login}");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            username = user.Login;
            return DBconnection;
        }

        public static void PrepareUpdate(NpgsqlDataSource DBconnection, string username)
        {
            Console.Clear();

            var schema = new GetSchema(DBconnection, "trello").GetCurrentSchema();

            if (schema.Count < 1 || schema is null)
            {
                Console.WriteLine("Unable to update target table");
                Console.ReadLine();
                return;
            }

            var SetValuesPairs = Dialogs.SetValuesDialog(schema);

            if (SetValuesPairs.Count() < 1)
            {
                Console.WriteLine("No values set");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Values set");
            Console.ReadLine();

            var WhereValuesPairs = Dialogs.SetConditionsDialog(schema);

            Console.WriteLine("Conditions set");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Here are set values:");
            Console.WriteLine();
            foreach (var svp in SetValuesPairs)
            {
                Console.WriteLine($"{svp.Key} = {svp.Value.ToString()}");
            }
            Console.WriteLine();
            Console.WriteLine("Here are where conditions:");
            Console.WriteLine();
            foreach (var wvp in WhereValuesPairs)
            {
                Console.WriteLine($"{wvp.Key} = {wvp.Value.ToString()}");
            }
            Console.WriteLine();
            Console.WriteLine("Please, confirm update: y/n");

            var confirmInput = Console.ReadLine();
            if (confirmInput?.ToUpper() == "Y")
            {
                new Crud().UpdateTask(DBconnection, username, SetValuesPairs, WhereValuesPairs);
            }
            else
            {
                Console.WriteLine("Update canceled");
                Console.ReadLine();
            }
        }

        public static void PrepareDelete(NpgsqlDataSource DBconnection, string username)
        {
            var schema = new GetSchema(DBconnection, "trello").GetCurrentSchema();

            if (schema.Count < 1 || schema is null)
            {
                Console.WriteLine("Unable to update target table");
                Console.ReadLine();
                return;
            }

            var WhereValuesPairs = Dialogs.SetConditionsDialog(schema);

            if (WhereValuesPairs.Count() < 1)
            {
                Console.WriteLine("No conditions set");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Conditions set");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Here are where conditions:");
            Console.WriteLine();
            foreach (var wvp in WhereValuesPairs)
            {
                Console.WriteLine($"{wvp.Key} = {wvp.Value.ToString()}");
            }
            Console.WriteLine();
            Console.WriteLine("Please, confirm delete: y/n");

            var confirmInput = Console.ReadLine();
            if (confirmInput?.ToUpper() == "Y")
            {
                new Crud().DeleteTask(DBconnection, username, WhereValuesPairs);
            }
            else
            {
                Console.WriteLine("Delete canceled");
                Console.ReadLine();
            }
        }
    }
}