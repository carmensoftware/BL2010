using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class SubCategory : DbHandler
    {
        /// <summary>
        ///     Empty contructor
        /// </summary>
        public SubCategory()
        {
            SelectCommand = "SELECT * FROM Application.SubCategory";
            TableName = "SubCategory";
        }

        /// <summary>
        ///     Get SubCategory data by specified SubCategoryCode.
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <param name="subCategoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsSubCategory, string subCategoryCode, string connStr)
        {
            // Create Parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SubCategoryCode", subCategoryCode);

            // Get data and return result
            return DbRetrieve("[Application].GetSubCategory", dsSubCategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get SubCategory data by specified SubCategoryCode.
        /// </summary>
        /// <param name="subCategoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string subCategoryCode, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = Get(dsSubCategory, subCategoryCode, connStr);

            // Return result
            if (result)
            {
                return dsSubCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all SubCategory.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, string connStr)
        {
            // Get data and return result
            return DbRetrieve("[Application].GetSubCategoryList", dsSubCategory, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all SubCategory.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = GetList(dsSubCategory, connStr);

            // Return result
            if (result)
            {
                return dsSubCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all SubCategory depend on specified CategoryCode.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, string categoryCode, string connStr)
        {
            // Create Paramter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);

            // Get data and return result
            return DbRetrieve("[Application].GetSubCategoryListByCategory", dsSubCategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all SubCategory depend on specified CategoryCode.
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string categoryCode, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = GetList(dsSubCategory, categoryCode, connStr);

            // Return result
            if (result)
            {
                return dsSubCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived SubCategory.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, bool isActive, string connStr)
        {
            // Create Paramter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return result
            return DbRetrieve("[Application].GetSubCategoryListActive", dsSubCategory, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived SubCategory.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = GetList(dsSubCategory, isActive, connStr);

            // Return result
            if (result)
            {
                return dsSubCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived SubCategory depend on specified CategoryCode.
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="isActive"></param>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSubCategory, string categoryCode, bool isActive, string connStr)
        {
            // Create Paramter
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@CategoryCode", categoryCode);
            dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return result
            return DbRetrieve("[Application].GetSubCategoryListActiveByCategory", dsSubCategory, dbParams, TableName,
                connStr);
        }

        /// <summary>
        ///     Get all actived or inactived SubCategory depend on specified CategoryCode.
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string categoryCode, bool isActive, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = GetList(dsSubCategory, categoryCode, isActive, connStr);

            // Return result
            if (result)
            {
                return dsSubCategory.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get new SubCategoryCode
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewCode(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("[Application].GetSubCategoryNewCode", null, connStr);

            // If return null valule
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Get SubCategory's Name of sepecified SubCategoryCode
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string subCategoryCode, string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get data
            var result = Get(dsSubCategory, subCategoryCode, connStr);

            // Return result
            if (result)
            {
                if (dsSubCategory.Tables[TableName] != null)
                {
                    if (dsSubCategory.Tables[TableName].Rows.Count > 0)
                    {
                        var drSubCategory = dsSubCategory.Tables[TableName].Rows[0];

                        return (drSubCategory["Name"] == DBNull.Value ? string.Empty : drSubCategory["Name"].ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get sub category code using sub category code
        /// </summary>
        /// <param name="dsSubCategory"></param>
        /// <param name="subCategoryCode"></param>
        /// <returns></returns>
        public bool GetSubCategory(DataSet dsSubCategory, string subCategoryCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@SubCategoryCode", subCategoryCode);

            // Get data
            result = DbRetrieve("Application.GetSubCategory", dsSubCategory, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get sub category name using sub category code
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public string GetSubCategoryName(string subCategoryCode, string connStr)
        {
            var result = string.Empty;
            var dsSubCategory = new DataSet();

            // Get data
            GetSubCategory(dsSubCategory, subCategoryCode, connStr);

            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                if (dsSubCategory.Tables[TableName].Rows.Count > 0)
                {
                    result = dsSubCategory.Tables[TableName].Rows[0]["Name"].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSubCategory, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsSubCategory, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}