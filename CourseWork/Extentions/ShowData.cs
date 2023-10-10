namespace CourseWork.Extentions
{
    public static class ShowData
    {
        public static void Show<T>(this List<T> data)
        {
            var structure = typeof(T).GetFields().ToList();

            if (structure.Count() > 0)
            {
                foreach (var row in data)
                {
                    foreach (var field in structure)
                    {
                        Console.WriteLine(field.Name + ": " + typeof(T).GetField(field.Name).GetValue(row) ?? "null");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                var properties = typeof(T).GetProperties();

                foreach (var row in data)
                {
                    foreach (var property in properties)
                    {
                        Console.WriteLine(property.Name + ": " + typeof(T).GetProperty(property.Name)?.GetValue(row) ?? "null");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
