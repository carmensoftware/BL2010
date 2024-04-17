using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class DataSourceColumn : DbHandler
    {
        #region " Attributes "

        #endregion

        #region " Operation "

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public DataSourceColumn()
        {
            SelectCommand = "SELECT * FROM Report.DataSourceColumn";
            TableName = "DataSourceColumn";
        }

        /// <summary>
        ///     Get all of datasource column.
        /// </summary>
        /// <param name="dsDataSourceColumn"></param>
        /// <param name="datasource_ID"></param>
        /// <param name="userName"></param>
        /// <param name="buID"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDataSourceColumn, int datasourceID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@datasource_ID", Convert.ToString(datasourceID));
            result = DbRetrieve("Report.GetDataSourceColumnListByDataSourceID", dsDataSourceColumn, dbParams, TableName,
                connStr);
            return result;
        }

        /// <summary>
        ///     Get datasourcecolumn schema
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetDataSourceColumnStructure(DataSet dsDataSourceColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Report.GetDataSourceColumnList", dsDataSourceColumn, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get DataSourceColumnList By DataSourceID
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(int dataSourceID, bool isJoinFieldData, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@dataSourceID", Convert.ToString(dataSourceID));
            return DbRead("Report.GetDataSourceColumnListByDataSourceIDJoinField", dbParams, connStr);
        }

        /// <summary>
        ///     Save changes to database.
        /// </summary>
        /// <param name="dsDataSourceColumn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDataSourceColumn, string connStr)
        {
            var dataSource = new DataSource();
            var result = false;
            var dbSaveSource = new DbSaveSource[2];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsDataSourceColumn, dataSource.SelectCommand, dataSource.TableName);
            dbSaveSource[1] = new DbSaveSource(dsDataSourceColumn, SelectCommand, TableName);


            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}