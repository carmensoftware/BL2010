using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Region : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Region()
        {
            SelectCommand = "Select * FROM [Application].Region";
            TableName = "Region";
        }

        /// <summary>
        ///     Get Region data by ID.
        /// </summary>
        /// <param name="dsRegion"></param>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsRegion, int id, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ID", id.ToString());

            // Get data
            result = DbRetrieve("Application.GetRegionByID", dsRegion, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all Region data.
        /// </summary>
        /// <param name="dsRegion"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRegion, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Application.GetRegionList", dsRegion, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all active or inactive Region data.
        /// </summary>
        /// <param name="dsRegion"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRegion, bool isActive, string connStr)
        {
            var result = false;

            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", Convert.ToString(isActive));

            // Get data
            result = DbRetrieve("Application.GetRegionListByIsActive", dsRegion, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Region's name of specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(int id, string connStr)
        {
            var dsRegion = new DataSet();
            var Name = string.Empty;
            var result = false;

            result = Get(dsRegion, id, connStr);

            if (result)
            {
                Name = (string) dsRegion.Tables[TableName].Rows[0]["Name"];
            }

            return Name;
        }

        /// <summary>
        ///     Get max region Code.
        /// </summary>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Application.GetRegionNewID", null, connStr);

            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get region data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetRegionForDropDownList(string connStr)
        {
            var dsRegion = new DataSet();

            // Get data
            GetList(dsRegion, connStr);

            // Return result
            if (dsRegion.Tables[TableName] != null)
            {
                var drBlank = dsRegion.Tables[TableName].NewRow();
                dsRegion.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsRegion.Tables[TableName];
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="dsRegion"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRegion, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsRegion, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}