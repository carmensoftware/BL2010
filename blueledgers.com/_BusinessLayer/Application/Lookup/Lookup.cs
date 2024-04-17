using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class Lookup : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Lookup()
        {
            SelectCommand = "SELECT * FROM [Application].Lookup";
            TableName = "Lookup";
        }

        /// <summary>
        ///     Get lookup data by specified ID.
        /// </summary>
        /// <param name="dsLookup"></param>
        /// <param name="TableID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Get(DataSet dsLookup, string ID, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ID", ID);

            // Get data and return result
            return DbRetrieve("[Application].GetLookup", dsLookup, dbParams, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get lookup name by specified ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public string GetName(string ID, string ConnectionString)
        {
            var dsLookup = new DataSet();

            // Get data
            var retrieved = Get(dsLookup, ID, ConnectionString);

            // Return result
            if (retrieved)
            {
                if (dsLookup.Tables[TableName] != null)
                {
                    if (dsLookup.Tables[TableName].Rows.Count > 0)
                    {
                        return dsLookup.Tables[TableName].Rows[0]["Name"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get list of lookup name included list of table name which can be a lookup.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataTable GetListIncludeTable(string ConnectionString)
        {
            var dsLookup = new DataSet();

            // Get data and return result
            var retrieved = DbRetrieve("[Application].GetLookupListIncludeTable", dsLookup, null, TableName,
                ConnectionString);

            // Return result
            if (retrieved)
            {
                return dsLookup.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all Lookup data.
        /// </summary>
        /// <param name="dsLookup"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsLookup, string connStr)
        {
            return DbRetrieve("[Application].[GetLookupList]", dsLookup, null, TableName, connStr);
        }

        /// <summary>
        ///     Get lookup item from standard table by speccified table id, text field id and value field id.
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="textFieldID"></param>
        /// <param name="valueFieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string tableID, string textFieldID, string valueFieldID, string connStr)
        {
            // Prepare table name, text field name and value field name.
            var table = new Table();
            var field = new Field();
            //BL.APP.Field field = new BL.APP.Field();
            var schema = new Schema();

            var schemaName = table.GetSchemaName(tableID, connStr);
            var tableName = table.GetName(tableID, connStr);
            var textFieldName = field.GetFieldName(textFieldID, connStr);
            var valueFieldName = field.GetFieldName(valueFieldID, connStr);

            var cmd = "SELECT " + textFieldName + " AS Text, " + valueFieldName + " AS Value FROM " +
                      "[" + schemaName + "].[" + tableName + "]";

            return DbExecuteQuery(cmd, null, connStr);
        }

        /// <summary>
        ///     Get Lookup Physical Table Structure.
        /// </summary>
        /// <param name="dsLookup"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsLookup, string connStr)
        {
            return DbRetrieveSchema("[Application].[GetLookupList]", dsLookup, null, TableName, connStr);
        }
    }
}