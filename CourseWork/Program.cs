using Npgsql;
using CourseWork.Tools;
using CourseWork.NorthWind_Tables__Structs;
using CourseWork.Extentions;
using CourseWork.CRUD_on_Tasks;

namespace CourseWork
{
    public class Caller
    {
        public const string Host = "localhost";
        public const string DataBase = "northwind";
        public const string Schema = "public";
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
                Console.WriteLine("Enter \'q\' to exit, \'s\' to show all data, \'a\' to add new task");
                var input = Console.ReadLine();
                if (input?.ToUpper() == "Q")
                {
                    return;
                }
                if (input?.ToUpper() == "S")
                {
                    Crud.ShowExistingTasks(DBconnection, username);
                }
                if (input?.ToUpper() == "A")
                {
                    Crud.AddTask(DBconnection, username);
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

            var connection = $"Host={Host};Username={user.Login};Password={user.Password};Database={DataBase}";
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
            Console.WriteLine($"Connected to {DataBase} as {user.Login}");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            username = user.Login;
            return DBconnection;
        }
    }
}