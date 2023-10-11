using Npgsql;

namespace CourseWork.Tools
{
    public class GetSchema
    {
		private NpgsqlDataSource DBconnection;
		private string commandText;

		public GetSchema(NpgsqlDataSource DBconnection, string table)
        {
			commandText = $@"
			SELECT 
					ordinal_position,
	 				column_name,
	 				data_type,
					is_nullable,
					is_identity,
	 				character_maximum_length,
	 				numeric_precision,
	 				numeric_precision_radix,
	 				datetime_precision
			 FROM information_schema.columns
			 WHERE table_catalog = '{Caller.DataBase}'
	 			  AND table_schema = '{Caller.Schema}'
	 			  AND table_name = '{table}'
			 ORDER BY ordinal_position";

			this.DBconnection = DBconnection;
        }

        public List<Schema> GetCurrentSchema()
        {
			return new SelectBuilder(DBconnection, commandText).GetObjects<Schema>();
        }
    }
}
