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
                Console.WriteLine("Enter \'q\' to exit, \'s\' to show all data, \'a\' to add new task \'u\' to update task \'d\' to delete task");
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
                if (input?.ToUpper() == "U")
                {
                    Console.Clear();

                    var schema = new GetSchema(DBconnection, "trello").GetCurrentSchema();

                    if (schema.Count < 1 || schema is null)
                    {
                        Console.WriteLine("Unable to update target table");
                        continue;
                    }

                    var SetValuesPairs = new Dictionary<string, object>();

                    while (true)
                    {
                        Console.Clear();

                        Console.WriteLine("Enter 'q' to stop adding or field`s name");
                        var field = Console.ReadLine();
                        if (field == null)
                        {
                            continue;
                        }
                        if (field.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new value stopped");
                            break;
                        }
                        if (!schema.Select(s => s.column_name.ToUpper()).Contains(field.ToUpper()))
                        {
                            Console.WriteLine("This column does not exist");
                            Console.ReadLine();
                            continue;
                        }

                        Console.WriteLine("Enter 'q' to stop adding or value to set");
                        if (field.ToUpper() == "TASK_PRIORITY")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Task priority has values constraint");
                            Console.WriteLine("Only values between 1 and 5 are allowed");
                            Console.WriteLine("Otherwise updating will be terminated");
                        }
                        var value = Console.ReadLine();
                        if (value == null)
                        {
                            continue;
                        }
                        if (value.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new value stopped");
                            break;
                        }
                        if (Guid.TryParse(value, out var guidValue))
                        {
                            SetValuesPairs.Add(field, guidValue);
                            continue;
                        }
                        if (int.TryParse(value, out var intValue))
                        {
                            SetValuesPairs.Add(field, intValue);
                            continue;
                        }
                        SetValuesPairs.Add(field, value);
                    }

                    Console.WriteLine("Values set");
                    Console.ReadLine();

                    var WhereValuesPairs = new Dictionary<string, object>();

                    while (true)
                    {
                        Console.Clear();

                        Console.WriteLine("Enter 'q' to stop adding conditions or field`s name");
                        var field = Console.ReadLine();
                        if (field == null)
                        {
                            continue;
                        }
                        if (field.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new condition stopped");
                            break;
                        }
                        if (!schema.Select(s => s.column_name.ToUpper()).Contains(field.ToUpper()))
                        {
                            Console.WriteLine("This column does not exist");
                            Console.ReadLine();
                            continue;
                        }

                        Console.WriteLine("Enter 'q' to stop adding conditions or value to set");
                        var value = Console.ReadLine();
                        if (value == null)
                        {
                            continue;
                        }
                        if (value.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new condition stopped");
                            break;
                        }
                        if (Guid.TryParse(value, out var guidValue))
                        {
                            WhereValuesPairs.Add(field, guidValue);
                            continue;
                        }
                        if (int.TryParse(value, out var intValue))
                        {
                            WhereValuesPairs.Add(field, intValue);
                            continue;
                        }
                        WhereValuesPairs.Add(field, value);
                    }

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
                        Crud.UpdateTask(DBconnection, username, SetValuesPairs, WhereValuesPairs);
                    }
                    else
                    {
                        Console.WriteLine("Update canceled");
                        Console.ReadLine();
                    }
                }
                if (input?.ToUpper() == "D")
                {
                    var schema = new GetSchema(DBconnection, "trello").GetCurrentSchema();

                    if (schema.Count < 1 || schema is null)
                    {
                        Console.WriteLine("Unable to update target table");
                        continue;
                    }

                    var WhereValuesPairs = new Dictionary<string, object>();

                    while (true)
                    {
                        Console.Clear();

                        Console.WriteLine("Enter 'q' to stop adding conditions or field`s name");
                        var field = Console.ReadLine();
                        if (field == null)
                        {
                            continue;
                        }
                        if (field.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new condition stopped");
                            break;
                        }
                        if (!schema.Select(s => s.column_name.ToUpper()).Contains(field.ToUpper()))
                        {
                            Console.WriteLine("This column does not exist");
                            Console.ReadLine();
                            continue;
                        }

                        Console.WriteLine("Enter 'q' to stop adding conditions or value to set");
                        var value = Console.ReadLine();
                        if (value == null)
                        {
                            continue;
                        }
                        if (value.ToUpper() == "Q")
                        {
                            Console.WriteLine("Adding of new condition stopped");
                            break;
                        }
                        if (Guid.TryParse(value, out var guidValue))
                        {
                            WhereValuesPairs.Add(field, guidValue);
                            continue;
                        }
                        if (int.TryParse(value, out var intValue))
                        {
                            WhereValuesPairs.Add(field, intValue);
                            continue;
                        }
                        WhereValuesPairs.Add(field, value);  
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
                        Crud.DeleteTask(DBconnection, username, WhereValuesPairs);
                    }
                    else
                    {
                        Console.WriteLine("Delete canceled");
                        Console.ReadLine();
                    }
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