using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class Table : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Table()
        {
            SelectCommand = "SELECT * FROM Application.Table";
            TableName = "Table";
        }

        /// <summary>
        ///     Get table list by tableID.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetTableListByTableID(DataSet dsTable, string tableID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@tableID", tableID);

            return DbRetrieve("Application.GetTableListByTableID", dsTable, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get list of table data which IsPermisisonSetup is True.
        /// </summary>
        /// <param name="dsTable"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetListIsPermissionSetup(DataSet dsTable, string ConnectionString)
        {
            return DbRetrieve("Application.GetTableListIsPermissionSetup", dsTable, null, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get all table data.
        /// </summary>
        /// <param name="dsTable"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTable, string ConnectionString)
        {
            var result = DbRetrieve("Application.GetTableList", dsTable, null, TableName, ConnectionString);

            return result;
        }

        /// <summary>
        ///     Get all table data.
        /// </summary>
        /// <param name="dsTable"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataTable GetList(string ConnectionString)
        {
            var dsTable = new DataSet();

            // Get data.
            GetList(dsTable, ConnectionString);

            // Return result
            if (dsTable.Tables[TableName] != null)
            {
                return dsTable.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get table data which allow to use for lookup.
        /// </summary>
        /// <param name="IsLookup"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool IsLookup, string ConnStr)
        {
            var dsTable = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsLookup", IsLookup.ToString());

            // Get data.
            var result = DbRetrieve("Application.GetTableListByIsLookup", dsTable, dbParams, TableName, ConnStr);

            // Return result
            if (result)
            {
                if (dsTable.Tables[TableName] != null)
                {
                    return dsTable.Tables[TableName];
                }
            }

            return null;
        }

        /// <summary>
        ///     Get table data from specified TableID.
        /// </summary>
        /// <param name="dsTable"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Get(DataSet dsTable, string tableID, string ConnectionString)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TableID", tableID);

            // Get data.
            return DbRetrieve("Application.GetTable", dsTable, dbParams, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get schema name of specified TableID.
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public string GetSchemaName(string tableID, string ConnectionString)
        {
            var dsTable = new DataSet();
            var schemaName = string.Empty;

            // Get data
            var retrieved = Get(dsTable, tableID, ConnectionString);

            // Get table name
            if (retrieved)
            {
                if (dsTable.Tables[TableName] != null)
                {
                    if (dsTable.Tables[TableName].Rows.Count > 0)
                    {
                        schemaName = (dsTable.Tables[TableName].Rows[0]["Schema"] == DBNull.Value
                            ? string.Empty
                            : dsTable.Tables[TableName].Rows[0]["Schema"].ToString());
                    }
                }
            }

            return schemaName;
        }

        /// <summary>
        ///     Get name of specified TableID.
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public string GetName(string tableID, string ConnectionString)
        {
            var dsTable = new DataSet();
            var tableName = string.Empty;

            // Get data
            var retrieved = Get(dsTable, tableID, ConnectionString);

            // Get table name
            if (retrieved)
            {
                if (dsTable.Tables[TableName] != null)
                {
                    if (dsTable.Tables[TableName].Rows.Count > 0)
                    {
                        tableName = (dsTable.Tables[TableName].Rows[0]["TableName"] == DBNull.Value
                            ? string.Empty
                            : dsTable.Tables[TableName].Rows[0]["TableName"].ToString());
                    }
                }
            }

            return tableName;
        }
    }
}