using Npgsql;

namespace CourseWork.CRUD_on_Tasks
{
    public class DBoperations
    {
        private static Dictionary<int, string> SuccessCaptions = new Dictionary<int, string>()
        {
            { 1, "Added" },
            { 2, "Updated" },
            { 3, "Deleted" }
        };

        private static Dictionary<int, string> FailedCaptions = new Dictionary<int, string>()
        {
            { 1, "Adding" },
            { 2, "Updating" },
            { 3, "Deleting" }
        };

        public static void ExecuteOperation(operationsType operationType, NpgsqlDataSource DBconnection, string cmdText)
        {
            try
            {
                Console.Clear();
                var cmd = DBconnection.CreateCommand(cmdText).ExecuteNonQuery();
                Console.WriteLine($"{SuccessCaptions[(int)operationType]} successfully");
                Console.ReadLine();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine($"{FailedCaptions[(int)operationType]} terminated");
                Console.ReadLine();
            }
        }
    }

    public enum operationsType
    { 
        Insert = 1,
        Update,
        Delete
    }
}
