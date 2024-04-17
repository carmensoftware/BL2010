using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class FieldType : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public FieldType()
        {
            SelectCommand = "SELECT * FROM [Application].FieldType";
            TableName = "FieldType";
        }

        /// <summary>
        ///     Get active or inactive field type.
        /// </summary>
        /// <param name="IsActive"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataTable GetList(bool IsActive, string ConnectionString)
        {
            var dbParam = new DbParameter[1];

            // Create parameter
            dbParam[0] = new DbParameter("@IsActive", IsActive.ToString());

            // Get data
            var dtFieldType = DbRead("[Application].GetFieldTypeListIsActive", dbParam, ConnectionString);

            // Return result
            return dtFieldType;
        }

        /// <summary>
        ///     Get FieldType data using ID.
        /// </summary>
        /// <param name="dsFieldType"></param>
        /// <param name="ID">Field type ID.</param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Get(DataSet dsFieldType, int ID, string ConnectionString)
        {
            var dbParam = new DbParameter[1];

            // Create parameter
            dbParam[0] = new DbParameter("@ID", ID.ToString());

            // Get data and return result.
            return DbRetrieve("[Application].GetFieldTypeByID", dsFieldType, dbParam, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get name of specified FieldType ID.
        /// </summary>
        /// <returns></returns>
        public string GetName(int ID, string ConnectionString)
        {
            var dsField = new DataSet();

            // Get data
            var retrieved = Get(dsField, ID, ConnectionString);

            // Return result
            if (retrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        return dsField.Tables[TableName].Rows[0]["Name"].ToString();
                    }
                }
            }

            return string.Empty;
        }
    }
}