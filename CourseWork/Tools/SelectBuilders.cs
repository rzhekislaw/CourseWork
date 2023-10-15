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