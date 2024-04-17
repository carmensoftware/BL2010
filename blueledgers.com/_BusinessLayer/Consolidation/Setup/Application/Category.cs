using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Category : DbHandler
    {
        /// <summary>
        ///     Empty contructor.
        /// </summary>
        public Category()
        {
            SelectCommand = "SELECT * FROM Application.Category";
            TableName = "Category";
        }

        /// <summary>
        ///     Get Category data by specified CategoryCode.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsCategory, string categoryCode, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

            // Get data and return result
            return DbRetrieve("[Application].GetCategory", dsCategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get Category data by specified CategoryCode.
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string categoryCode, string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            var result = Get(dsCategory, categoryCode, connStr);

            // Return result
            if (result)
            {
                return dsCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all Category data.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsCategory, string connStr)
        {
            // Get data and return result
            return DbRetrieve("[Application].GetCategoryList", dsCategory, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all Category data.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            var result = GetList(dsCategory, connStr);

            // Return result
            if (result)
            {
                return dsCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived and inactived Category.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsCategory, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return result
            return DbRetrieve("[Application].GetCategoryListActive", dsCategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived and inactived Category.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            var result = GetList(dsCategory, isActive, connStr);

            // return result
            if (result)
            {
                return dsCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get new CategoryCode
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewCode(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("[Application].GetCategoryNewCode", null, connStr);

            // If return null valule
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get Category's Name of sepecified CategoryCode
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string categoryCode, string connStr)
        {
            var dsCategory = new DataSet();

            // Get data
            var result = Get(dsCategory, categoryCode, connStr);

            // Return result
            if (result)
            {
                if (dsCategory.Tables[TableName] != null)
                {
                    if (dsCategory.Tables[TableName].Rows.Count > 0)
                    {
                        var drCategory = dsCategory.Tables[TableName].Rows[0];

                        return (drCategory["Name"] == DBNull.Value ? string.Empty : drCategory["Name"].ToString());
                    }
                }
            }

            return string.Empty;
        }


        /// <summary>
        ///     Get category using category code
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public bool GetCategory(DataSet dsCategory, string categoryCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

            // Get data
            result = DbRetrieve("Application.GetCategory", dsCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get category name using category code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public string GetCategoryName(string categoryCode, string connStr)
        {
            var result = string.Empty;
            var dsCategory = new DataSet();

            // Get data
            GetCategory(dsCategory, categoryCode, connStr);

            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                if (dsCategory.Tables[TableName].Rows.Count > 0)
                {
                    result = dsCategory.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="Connstr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsCategory, string Connstr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsCategory, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, Connstr);

            // Return result
            return result;
        }
    }
}