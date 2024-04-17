using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Region : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Region()
        {
            SelectCommand = "SELECT * FROM Reference.Region";
            TableName = "Region";
        }

        /// <summary>
        ///     Get category using region code
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public bool GetRegion(DataSet dsRegion, string regionCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RegionCode", regionCode);

            // Get data
            result = DbRetrieve("Reference.GetRegion", dsRegion, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all data in table
        /// </summary>
        /// <returns></returns>
        public DataTable GetRegionList(string connStr)
        {
            var dtRegion = new DataTable();
            dtRegion = DbRead("Reference.GetRegionList", null, connStr);

            if (dtRegion != null)
            {
                var drBlank = dtRegion.NewRow();
                dtRegion.Rows.InsertAt(drBlank, 0);
            }
            return dtRegion;
        }

        /// <summary>
        ///     Get all category
        /// </summary>
        /// <returns></returns>
        public bool GetRegionList(DataSet dsRegion, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetRegionList", dsRegion, null, TableName, connStr);

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
            GetRegionList(dsRegion, connStr);

            // Return result
            if (dsRegion.Tables[TableName] != null)
            {
                var drBlank = dsRegion.Tables[TableName].NewRow();
                dsRegion.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsRegion.Tables[TableName];
        }

        /// <summary>
        ///     Get region name using region code
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetRegionName(string regionCode, string connStr)
        {
            var result = string.Empty;
            var dsRegion = new DataSet();

            // Get data
            GetRegion(dsRegion, regionCode, connStr);

            // Return result
            if (dsRegion.Tables[TableName] != null)
            {
                if (dsRegion.Tables[TableName].Rows.Count > 0)
                {
                    result = dsRegion.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Get max region Code.
        /// </summary>
        /// <returns></returns>
        public int GetRegionCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetRegionCodeMax", null, connStr);

            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCategory"></param>
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