using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class Schema : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Schema()
        {
            SelectCommand = "SELECT * FROM Application.Schema";
            TableName = "Schema";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSchemaList(string connStr)
        {
            var dtSchema = new DataTable();

            dtSchema = DbRead("Application.GetModuleList", null, connStr);

            return dtSchema;
        }

        /// <summary>
        ///     Get schema data which IsPermissionSetup is 'True'.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetListIsPermissionSetup(DataSet dsSchema, string ConnectionString)
        {
            return DbRetrieve("Application.GetSchemaListIsPermissionSetup", dsSchema, null, TableName, ConnectionString);
        }
    }
}