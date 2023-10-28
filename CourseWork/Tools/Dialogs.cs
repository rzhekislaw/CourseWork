namespace CourseWork.Tools
{
    public class Dialogs
    {
        public static object InsertDialog(string field, object comparativeObj = null, string[] additionalMessages = null, bool isSkippable = false)
        {
            object value = null;
            do
            {
                Console.Clear();
                if (additionalMessages != null && additionalMessages.Count() > 0)
                {
                    foreach (var message in additionalMessages)
                    {
                        Console.WriteLine(message);
                    }
                }
                Console.WriteLine($"Enter {field}:");
                var input = Console.ReadLine();
                if (input != string.Empty)
                {
                    if (comparativeObj != null)
                    {
                        if (comparativeObj.GetType() == 0.GetType())
                        {
                            if (int.TryParse(input, out var parsedValue))
                            {
                                value = parsedValue;
                                break;
                            }
                            Console.WriteLine("Wrong input!");
                            Console.ReadLine();
                            continue;
                        }
                        if (comparativeObj.GetType() == 0m.GetType())
                        {
                            if (decimal.TryParse(input, out var parsedValue))
                            {
                                value = parsedValue;
                                break;
                            }
                            Console.WriteLine("Wrong input!");
                            Console.ReadLine();
                            continue;
                        }
                        if (comparativeObj.GetType() == Guid.Empty.GetType())
                        {
                            if (Guid.TryParse(input, out var parsedValue))
                            {
                                value = parsedValue;
                                break;
                            }
                            Console.WriteLine("Wrong input!");
                            Console.ReadLine();
                            continue;
                        }
                        if (comparativeObj.GetType() == DateTime.MinValue.GetType())
                        {
                            if (DateTime.TryParse(input, out var parsedValue))
                            {
                                value = parsedValue;
                                break;
                            }
                            Console.WriteLine("Wrong input!");
                            Console.ReadLine();
                            continue;
                        }
                    }
                    else
                    {
                        return input;
                    }
                }
                else
                {
                    if (isSkippable)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine("Wrong input!");
                        Console.ReadLine();
                        continue;
                    }

                }
            }
            while (true);

            return value;
        }

        public static Dictionary<string, object> SetValuesDialog(List<Schema> schema)
        {
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

            return SetValuesPairs;
        }

        public static Dictionary<string, object> SetConditionsDialog(List<Schema> schema)
        {
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

            return WhereValuesPairs;
        }
    }
}
