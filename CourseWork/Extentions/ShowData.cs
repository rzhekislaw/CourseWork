namespace CourseWork.Extentions
{
    public static class ShowData
    {
        public static void Show<T>(this List<T> data)
        {
            Console.Clear();

            var structure = typeof(T).GetFields().ToList();

            if (structure.Count() > 0)
            {
                var separatorPositions = new List<int>();

                var widths = new Dictionary<string, int>();

                foreach (var field in structure)
                {
                    Console.Write($"{field.Name.PadLeft(10 + field.Name.Length, ' ').PadRight(20 + field.Name.Length, ' ')}|");
                    separatorPositions.Add(field.Name.Length + 21 + (separatorPositions.Count == 0 ? 0 : separatorPositions.Max()));
                    widths.Add(field.Name, field.Name.Length + 20);
                }
                Console.Write("\n");
                AddSeparatorLine(separatorPositions);

                foreach (var row in data)
                {
                    foreach (var field in structure)
                    {
                        var value = (typeof(T).GetField(field.Name).GetValue(row) ?? "null").ToString();
                        Console.Write($"{value.PadLeft((widths[field.Name] + value.Length) / 2, ' ').PadRight(widths[field.Name], ' ')}|");
                    }
                    Console.Write("\n");
                    AddSeparatorLine(separatorPositions);
                }
            }
            else
            {
                var properties = typeof(T).GetProperties();

                var separatorPositions = new List<int>();

                var widths = new Dictionary<string, int>();

                foreach (var property in properties)
                {
                    Console.Write($"{property.Name.PadLeft(5 + property.Name.Length, ' ').PadRight(10 + property.Name.Length, ' ')}|");
                    separatorPositions.Add(property.Name.Length + 11 + (separatorPositions.Count == 0 ? 0 : separatorPositions.Max()));
                    widths.Add(property.Name, property.Name.Length + 10);
                }

                Console.Write("\n");
                AddSeparatorLine(separatorPositions);

                foreach (var row in data)
                {
                    foreach (var property in properties)
                    {
                        var value = (typeof(T).GetProperty(property.Name).GetValue(row) ?? "null").ToString();
                        Console.Write($"{value.PadLeft((widths[property.Name] + value.Length) / 2, ' ').PadRight(widths[property.Name], ' ')}|");
                    }
                    Console.Write("\n");
                    AddSeparatorLine(separatorPositions);
                }
            }
        }

        private static void AddSeparatorLine(List<int> separators)
        {
            var line = new char[separators.Max()];

            for (int i = 0; i < separators.Max(); i++)
            {
                if (separators.Contains(i + 1))
                {
                    line[i] = '+';
                }
                else
                {
                    line[i] = '-';
                }
            }

            Console.WriteLine(new string(line));
        }
    }
}
