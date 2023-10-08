namespace CourseWork.Extentions
{
    public static class CastsFromObjects
    {
        public static List<string> ToString(this List<object> objects)
        {
            var data = new List<string>();

            objects.ForEach(pr =>
            {
                data.Add(pr.ToString());
            });

            return data;
        }

        public static string ToString(this object obj)
        {
            return obj.ToString();
        }

        public static List<int> ToInt(this List<object> objects)
        {
            var data = new List<int>();

            objects.ForEach(pr =>
            {
                data.Add(int.Parse(pr.ToString()));
            });

            return data;
        }

        public static int ToInt(this object obj)
        {
            return int.Parse(obj.ToString());
        }

        public static List<decimal> ToDecimal(this List<object> objects)
        {
            var data = new List<decimal>();

            objects.ForEach(pr =>
            {
                data.Add(decimal.Parse(pr.ToString()));
            });

            return data;
        }

        public static decimal ToDecimal(this object obj)
        {
            return decimal.Parse(obj.ToString());
        }

        public static List<DateTime> ToDateTime(this List<object> objects)
        {
            var data = new List<DateTime>();

            objects.ForEach(pr =>
            {
                data.Add(DateTime.Parse(pr.ToString()));
            });

            return data;
        }
        public static DateTime ToDateTime(this object obj)
        {
            return DateTime.Parse(obj.ToString());
        }

        public static List<Guid> ToGuid(this List<object> objects)
        {
            var data = new List<Guid>();

            objects.ForEach(pr =>
            {
                data.Add(Guid.Parse(pr.ToString()));
            });

            return data;
        }

        public static Guid ToGuid(this object obj)
        {
            return Guid.Parse(obj.ToString());
        }

    }

    public static class ShowData
    {
        public static void Show(this List<object> data)
        {
            data.ForEach(obj => 
            {
                Console.WriteLine(obj ?? "null");
            });
        }

        public static void Show<T>(this List<T> data)
        {
            var structure = typeof(T).GetFields().ToList();

            foreach (var row in data)
            {
                foreach (var field in structure)
                {
                    Console.WriteLine(field.Name + ": " + typeof(T).GetField(field.Name).GetValue(row) ?? "null");
                }
                Console.WriteLine();
            }
        }
    }
}
