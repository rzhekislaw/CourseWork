using Npgsql;
using CourseWork.Tools;
using CourseWork.Extentions;
using CourseWork.NorthWind_Tables__Structs;

namespace CourseWork.CRUD_on_Tasks
{
    public interface ICrud
    {
        void ShowExistingTasks(NpgsqlDataSource DBconnection, string username);
        void AddTask(NpgsqlDataSource DBconnection, string username);

        void UpdateTask(NpgsqlDataSource DBconnection, string username, Dictionary<string, object> SetValuesPairs, Dictionary<string, object> WhereValuesPairs);

        void DeleteTask(NpgsqlDataSource DBconnection, string username, Dictionary<string, object> WhereValuesPairs);
    }

    public class Crud : ICrud
    {
        public void ShowExistingTasks(NpgsqlDataSource DBconnection, string username)
        {
            Console.Clear();

            //new SelectBuilder(DBconnection, "select board, col_name, task_name, task_description, task_priority, task_comment from trello where board_user = @user order by board, col_name, task_priority desc", new Dictionary<string, object>() { { "user", username } }).GetObjects(new
            //{
            //    //id = Guid.Empty,
            //    board = string.Empty,
            //    //board_user = string.Empty,
            //    col_name = string.Empty,
            //    task_name = string.Empty,
            //    task_description = string.Empty,
            //    task_priority = 0,
            //    task_comment = string.Empty
            //}).Show();

            new SelectBuilder(DBconnection, "select * from trello where board_user = @user order by board, col_name, task_priority desc", new Dictionary<string, object>() { { "user", username } }).GetObjects<Trello>().Show();

            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        public void AddTask(NpgsqlDataSource DBconnection, string username)
        {
            string board = Dialogs.InsertDialog("board").ToString();

            string col_name = Dialogs.InsertDialog("col_name").ToString();

            string task_name = Dialogs.InsertDialog("task_name").ToString();

            string task_description = Dialogs.InsertDialog("task_description").ToString();

            int task_priority = Dialogs.InsertDialog("task_priority", 0, new string[]
            {
                "Task priority has values constraint",
                "Only values between 1 and 5 are allowed",
                "Otherwise adding will be terminated",
                "In case of skipping priority setting priority will be set to 1"
            }, true)?.ToInt() ?? 0;

            string task_comment = Dialogs.InsertDialog("task_comment", isSkippable : true)?.ToString();

            var cmdText = $@"
            insert into trello (
            board_user,
            board,
            col_name,
            task_name,
            task_description
            {(task_priority == 0 ? "" : ", task_priority")}
            {(task_comment == string.Empty ? "" : ", task_comment")}
            )
            values (
            '{username}',
            '{board}',
            '{col_name}',
            '{task_name}',
            '{task_description}'
            {(task_priority == 0 ? "" : $", '{task_priority}'")}
            {(task_comment == string.Empty ? "" : $", '{task_comment}'")}
            )";

            DBoperations.ExecuteOperation(operationsType.Insert, DBconnection, cmdText);
        }

        public void UpdateTask(NpgsqlDataSource DBconnection, string username, Dictionary<string, object> SetValuesPairs, Dictionary<string, object> WhereValuesPairs)
        {
            var setValues = "set";
            if (SetValuesPairs.Count > 0)
            {
                foreach (var svp in SetValuesPairs)
                {
                    setValues += $" {svp.Key} = {(svp.Value is string ? $"'{svp.Value}'" : svp.Value)},";
                }
                setValues = setValues.TrimEnd(',');
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No fields to update");
                Console.WriteLine("Update terminated");
                Console.ReadLine();
                return;
            }

            var whereCondition = $"where board_user = '{username}'";
            if (WhereValuesPairs.Count > 0)
            {
                foreach (var wvp in WhereValuesPairs)
                {
                    whereCondition += $" and {wvp.Key} = {(wvp.Value is string ? $"'{wvp.Value}'" : wvp.Value is Guid ? $"'{wvp.Value}'" : wvp.Value)}";
                }
            }

            var cmdText = $@"
            update trello
            {setValues}
            {whereCondition}";

            DBoperations.ExecuteOperation(operationsType.Update, DBconnection, cmdText);
        }

        public void DeleteTask(NpgsqlDataSource DBconnection, string username, Dictionary<string, object> WhereValuesPairs)
        {
            
            if (WhereValuesPairs.Count < 1)
            {
                Console.Clear();
                Console.WriteLine("Unable to delete all tasks");
                Console.WriteLine("Delete terminated");
                Console.ReadLine();
                return;
            }

            var whereCondition = $"where board_user = '{username}'";
            if (WhereValuesPairs.Count > 0)
            {
                foreach (var wvp in WhereValuesPairs)
                {
                    whereCondition += $" and {wvp.Key} = {(wvp.Value is string ? $"'{wvp.Value}'" : wvp.Value is Guid ? $"'{wvp.Value}'" : wvp.Value)}";
                }
            }

            var cmdText = $@"
            delete from trello
            {whereCondition}";

            DBoperations.ExecuteOperation(operationsType.Delete, DBconnection, cmdText);
        }
    }
}
