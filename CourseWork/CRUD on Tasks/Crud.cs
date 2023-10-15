using Npgsql;
using CourseWork.Tools;
using CourseWork.Extentions;

namespace CourseWork.CRUD_on_Tasks
{
    public class Crud
    {
        public static void ShowExistingTasks(NpgsqlDataSource DBconnection, string username)
        {
            Console.Clear();

            new SelectBuilder(DBconnection, "select * from trello where board_user = @user", new Dictionary<string, object>() { { "user", username } }).GetObjects(new
            {
                id = Guid.Empty,
                board = string.Empty,
                board_user = string.Empty,
                col_name = string.Empty,
                task_name = string.Empty,
                task_description = string.Empty,
                task_priority = 0,
                task_comment = string.Empty
            }).Show();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        public static void AddTask(NpgsqlDataSource DBconnection, string username)
        {
            string board = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter board name to add task to: ");
                var input = Console.ReadLine();
                if (input != null)
                {
                    board = input;
                    break;
                }
                Console.WriteLine("Wrong input");
                Console.ReadLine();
            }
            while (true);

            string col_name = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter column name to add task to: ");
                var input = Console.ReadLine();
                if (input != null)
                {
                    col_name = input;
                    break;
                }
                Console.WriteLine("Wrong input");
                Console.ReadLine();
            }
            while (true);

            string task_name = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter task name: ");
                var input = Console.ReadLine();
                if (input != null)
                {
                    task_name = input;
                    break;
                }
                Console.WriteLine("Wrong input");
                Console.ReadLine();
            }
            while (true);

            string task_description = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter task description: ");
                var input = Console.ReadLine();
                if (input != null)
                {
                    task_description = input;
                    break;
                }
                Console.WriteLine("Wrong input");
                Console.ReadLine();
            }
            while (true);

            int task_priority = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Task priority has values constraint");
                Console.WriteLine("Only values between 1 and 5 are allowed");
                Console.WriteLine("Otherwise adding will be terminated");
                Console.WriteLine("In case of skipping priority setting priority will be set to 1");
                Console.WriteLine("Enter task priority: ");
                var input = Console.ReadLine();
                if (input != null && int.TryParse(input, out var parsed))
                {
                    task_priority = parsed;
                    break;
                }
                if (input == null)
                {
                    break;
                }
                Console.WriteLine("Wrong input");
                Console.ReadLine();
            }
            while (true);

            string task_comment = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter task comment: ");
                var input = Console.ReadLine();
                if (input != null)
                {
                    task_comment = input;
                    break;
                }
                if (input == null)
                {
                    break;
                }
            }
            while (true);

            var cmdText = $@"insert into trello (board_user, board, col_name, task_name, task_description{(task_priority == 0 ? "" : ", task_priority")}{(task_comment == string.Empty ? "" : ", task_comment")})
            values ('{username}', '{board}', '{col_name}', '{task_name}', '{task_description}'{(task_priority == 0 ? "" : $", '{task_priority}'")}{(task_comment == string.Empty ? "" : $", '{task_comment}'")})";

            try
            {
                Console.Clear();
                var cmd = DBconnection.CreateCommand(cmdText).ExecuteNonQuery();
                Console.WriteLine("Added successfully");
                Console.ReadLine();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Adding terminated");
                Console.ReadLine();
            }
        }
    }
}
