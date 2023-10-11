using Npgsql;
using CourseWork.Tools;
using CourseWork.NorthWind_Tables__Structs;
using CourseWork.Extentions;

namespace CourseWork
{
    public class Caller
    {
        public const string Host = "localhost";
        public const string DataBase = "northwind";
        public const string Schema = "public";
        public static void Main()
        {
            OnInitialize();
        }

        public static void OnInitialize()
        {
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
            using var DBconnection = NpgsqlDataSource.Create(connection);

            try
            {
                //var testConnection = SelectBuilders.GetScalar(DBconnection, "select 1").ToInt();
                var testConnection = new SelectBuilder(DBconnection, "select 1").GetScalar().ToInt();              
            }
            catch
            {
                Console.WriteLine("Authorization failure");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"Connected to {DataBase} as {user.Login}");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}