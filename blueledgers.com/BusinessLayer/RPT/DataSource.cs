using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.RPT
{
    public class DataSource : DbHandler
    {
        #region " Attributies "

        #endregion

        #region " Operations "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public DataSource()
        {
            SelectCommand = "SELECT * FROM RPT.DataSource";
            TableName = "DataSource";
        }

        /// <summary>
        ///     Get DataSource data by using ReportID
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="reportID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByReportID(DataSet dsDataSource, int reportID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

            return DbRetrieve("RPT.GetDataSourceByReportID", dsDataSource, dbParams, TableName, connStr);
        }

        public string GetSourceByReportID(int reportID, string connStr)
        {
            var dsTmp = new DataSet();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ReportID", reportID.ToString());

            DbRetrieve("RPT.GetDataSourceByReportID", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows[0]["DataSourceID"].ToString() != string.Empty)
            {
                return (string) dsTmp.Tables[TableName].Rows[0]["SelectCommand"];
            }
            return string.Empty;
        }


        /// <summary>
        ///     Get all of datasource list
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDataSource, string connStr)
        {
            var result = false;
            result = DbRetrieve("RPT.GetDataSourceList", dsDataSource, null, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get all of datasource list by datasourceID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Get(DataSet dsDataSource, int dataSourceID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@dataSourceID", Convert.ToString(dataSourceID));

            result = DbRetrieve("RPT.GetDataSourceListByDataSourceID", dsDataSource, dbParams, TableName, connStr);
            return result;
        }

        /// <summary>
        ///     Get Table ID Related to Specified Data Source ID.
        /// </summary>
        /// <param name="dataSourceID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTableID(int dataSourceID, string connStr)
        {
            var dsTemp = new DataSet();

            var result = Get(dsTemp, dataSourceID, connStr);

            if (result)
            {
                if (dsTemp.Tables[TableName].Rows.Count > 0)
                {
                    if (dsTemp.Tables[TableName].Rows[0]["TableID"] != DBNull.Value)
                    {
                        return dsTemp.Tables[TableName].Rows[0]["TableID"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public string GetTableSchema(int dataSourceID, string connStr)
        {
            var dsTemp = new DataSet();

            var result = Get(dsTemp, dataSourceID, connStr);

            if (result)
            {
                if (dsTemp.Tables[TableName].Rows.Count > 0)
                {
                    if (dsTemp.Tables[TableName].Rows[0]["SchemaName"].ToString() != string.Empty)
                    {
                        return dsTemp.Tables[TableName].Rows[0]["SchemaName"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public string GetTableName(int dataSourceID, string connStr)
        {
            var dsTemp = new DataSet();

            var result = Get(dsTemp, dataSourceID, connStr);

            if (result)
            {
                if (dsTemp.Tables[TableName].Rows.Count > 0)
                {
                    if (dsTemp.Tables[TableName].Rows[0]["TableName"].ToString() != string.Empty)
                    {
                        return dsTemp.Tables[TableName].Rows[0]["TableName"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Read data
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public DataTable GetListByActive(string connStr)
        {
            return DbRead("RPT.GetDataSourceListByActive", null, connStr);
        }

        /// <summary>
        ///     Get max datasource id
        /// </summary>
        /// <returns></returns>
        public int GetDataSourceMaxID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("RPT.GetDataSourceMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get datasource schema
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetDataSourceStructure(DataSet dsDataSource, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("RPT.GetDataSourceList", dsDataSource, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Save changes to database.
        /// </summary>
        /// <param name="dsDataSource"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDataSource, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsDataSource, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}