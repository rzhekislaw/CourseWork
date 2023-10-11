//using Npgsql;

//namespace CourseWork.Tools
//{
//    public class SelectBuilders
//    {
//        #region GetScalar
//        public static object GetScalar(NpgsqlDataSource dataSource, string commandText, Dictionary<string, object> parameters = null)
//        {
//            object obj;

//            using (var cmd = dataSource.CreateCommand(commandText))
//            {
//                obj = GetScalar(cmd, parameters);
//            }

//            return obj;
//        }

//        private static object GetScalar(NpgsqlCommand cmd, Dictionary<string, object> parameters = null)
//        {
//            object obj = null;

//            if (parameters != null)
//            {
//                foreach (var param in parameters)
//                {
//                    cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
//                }
//            }

//            using (var reader = cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    obj = GetScalar(reader);
//                }
//            }

//            return obj;
//        }
//        private static object GetScalar(NpgsqlDataReader reader)
//        {
//            object obj;

//            obj = reader.GetValue(0);

//            return obj;
//        }
//        #endregion

//        #region GetScalars

//        public static List<object> GetScalars(NpgsqlDataSource dataSource, string commandText, Dictionary<string, object> parameters = null)
//        {
//            var data = new List<object>();

//            using (var cmd = dataSource.CreateCommand(commandText))
//            {
//                data = GetScalars(cmd, parameters);
//            }

//            return data;
//        }

//        private static List<object> GetScalars(NpgsqlCommand cmd, Dictionary<string, object> parameters = null)
//        {
//            var data = new List<object>();

//            if (parameters != null)
//            {
//                foreach (var param in parameters)
//                {
//                    cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
//                }
//            }

//            using (var reader = cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    data.Add(GetScalars(reader));
//                }
//            }

//            return data;
//        }
//        private static object GetScalars(NpgsqlDataReader reader)
//        {
//            object obj;

//            obj = reader.GetValue(0);

//            return obj;
//        }
//        #endregion

//        #region GetObject
//        public static T GetObject<T>(NpgsqlDataSource dataSource, string commandText, Dictionary<string, object> parameters = null) where T : new()
//        {
//            var row = new T();

//            using (var cmd = dataSource.CreateCommand(commandText))
//            {
//                row = GetObject<T>(cmd, parameters);
//            }

//            return row;
//        }
//        private static T GetObject<T>(NpgsqlCommand cmd, Dictionary<string, object> parameters = null) where T : new()
//        {
//            var row = new T();

//            if (parameters != null)
//            {
//                foreach (var param in parameters)
//                {
//                    cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
//                }
//            }

//            using (var reader = cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    row = GetObject<T>(reader);
//                }
//            }

//            return row;
//        }
//        private static T GetObject<T>(NpgsqlDataReader reader) where T : new()
//        {
//            var row = new T();

//            foreach (var field in typeof(T).GetFields())
//            {
//                field.SetValue(row, reader.GetValue(reader.GetOrdinal(field.Name)));
//            }

//            return row;
//        }
//        #endregion

//        #region GetObjects
//        public static List<T> GetObjects<T>(NpgsqlDataSource dataSource, string commandText, Dictionary<string, object> parameters = null) where T : new()
//        {
//            var data = new List<T>();

//            using (var cmd = dataSource.CreateCommand(commandText))
//            {
//                data = GetObjects<T>(cmd, parameters);
//            }

//            return data;
//        }

//        private static List<T> GetObjects<T>(NpgsqlCommand cmd, Dictionary<string, object> parameters = null) where T : new()
//        {
//            var data = new List<T>();

//            if (parameters != null)
//            {
//                foreach (var param in parameters)
//                {
//                    cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
//                }
//            }

//            using (var reader = cmd.ExecuteReader())
//            {
//                while (reader.Read())
//                {
//                    data.Add(GetObjects<T>(reader));
//                }
//            }

//            return data;
//        }

//        private static T GetObjects<T>(NpgsqlDataReader reader) where T : new()
//        {
//            var row = new T();

//            foreach (var field in typeof(T).GetFields())
//            {
//                field.SetValue(row, reader.GetValue(reader.GetOrdinal(field.Name)));
//            }

//            return row;
//        }
//        #endregion
//    }
//}

using Npgsql;

namespace CourseWork.Tools
{
    public class SelectBuilder
    {
        private NpgsqlCommand NpgsqlCommand;

        public SelectBuilder(NpgsqlDataSource DBconnection, string commandText)
        {
            NpgsqlCommand = DBconnection.CreateCommand(commandText);
        }

        public SelectBuilder(NpgsqlDataSource DBconnection, string commandText, Dictionary<string, object> parameters)
        {
            NpgsqlCommand = DBconnection.CreateCommand(commandText);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    NpgsqlCommand.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
                }
            }
        }

        #region GetScalars

        public object GetScalar()
        {
            var obj = GetScalars(true);

            if (obj.Count() > 0 && obj != null)
            {
                return obj.ElementAt(0);
            }
            else
            {
                return null;
            }

        }

        public List<object> GetScalars()
        {
            return GetScalars(false);
        }

        private List<object> GetScalars(bool firstRowOnly)
        {
            var column = new List<object>();

            using (var reader = NpgsqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    column.Add(GetScalar(reader));
                    if (firstRowOnly)
                    {
                        break;
                    }
                }
            }
            return column;
        }

        private object GetScalar(NpgsqlDataReader reader)
        {
            object obj;

            obj = reader.GetValue(0);

            return obj;
        }

        #endregion

        #region GetObjects
        public T GetObject<T>(T obj) where T : class
        {
            var row = GetAnonymousObjects<T>(true);

            if (row.Count() > 0 && row != null)
            {
                return row.ElementAt(0);
            }
            else
            {
                return obj;
            }
        }

        public T GetObject<T>() where T : new()
        {
            var row = GetObjects<T>(true);

            if (row.Count > 0 && row != null)
            {
                return row.ElementAt(0);
            }
            else
            {
                return default(T);
            }
        }
        
        public List<T> GetObjects<T>(T obj) where T : class
        {
            return GetAnonymousObjects<T>(false);
        }

        public List<T> GetObjects<T>() where T : new()
        {
            return GetObjects<T>(false);
        }

        private List<T> GetAnonymousObjects<T>(bool firstRowOnly) where T : class
        {
            var data = new List<T>();

            using (var reader = NpgsqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(GetAnonymousObject<T>(reader));
                    if (firstRowOnly)
                    {
                        break;
                    }
                }
            }
            return data;
        }

        private List<T> GetObjects<T>(bool firstRowOnly) where T : new()
        {
            var data = new List<T>();

            using (var reader = NpgsqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(GetObject<T>(reader));
                    if(firstRowOnly)
                    {
                        break;
                    }
                }
            }
            return data;
        }

        private T GetAnonymousObject<T>(NpgsqlDataReader reader) where T : class
        {
            var constructor = typeof(T).GetConstructors().ElementAt(0);

            var parameters = new object[constructor.GetParameters().Count()];

            for (int i = 0; i < constructor.GetParameters().Count(); i++)
            {
                var defaultValue = GetDefaultValue(constructor.GetParameters().ToArray()[i].ParameterType);

                parameters[i] = reader.GetValue(reader.GetOrdinal(constructor.GetParameters()[i].Name)) == DBNull.Value ? defaultValue : reader.GetValue(reader.GetOrdinal(constructor.GetParameters()[i].Name));
            }

            var row = constructor.Invoke(parameters);

            return (T)row;
        }

        private T GetObject<T>(NpgsqlDataReader reader) where T : new()
        {
            var row = new T();

            foreach (var field in typeof(T).GetFields())
            {
                var defaultFieldValue = GetDefaultValue(field.FieldType);

                field.SetValue(row, reader.GetValue(reader.GetOrdinal(field.Name)) == DBNull.Value ? defaultFieldValue : reader.GetValue(reader.GetOrdinal(field.Name)));
            }

            return row;
        }

        #endregion

        private T GetDefaultValue<T>(T type) where T : Type
        {
            var value = default(T);

            return value;
        }
    }
}