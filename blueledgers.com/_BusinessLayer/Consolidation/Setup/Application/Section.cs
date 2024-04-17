using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class Section : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Section()
        {
            SelectCommand = "SELECT * FROM Application.Section";
            TableName = "Section";
        }

        /// <summary>
        ///     Get section by section code.
        /// </summary>
        /// <param name="dsSection"></param>
        /// <param name="sectionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsSection, string sectionCode, string connStr)
        {
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SectionCode", sectionCode);

            // Get data
            result = DbRetrieve("[Application].GetSection", dsSection, dbParams, TableName, connStr);

            // Return result        
            return result;
        }

        /// <summary>
        ///     Get section by section code.
        /// </summary>
        /// <param name="sectionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string sectionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = Get(dsSection, sectionCode, connStr);

            // Return result
            if (result)
            {
                return dsSection.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all section.
        /// </summary>
        /// <param name="dsSection"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSection, string connStr)
        {
            // Get data and return result
            return DbRetrieve("[Application].GetSectionList", dsSection, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all section.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = GetList(dsSection, connStr);

            // Return result
            if (result)
            {
                return dsSection.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all section depend on DivisionCode.
        /// </summary>
        /// <param name="dsSection"></param>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSection, string divisionCode, string connStr)
        {
            // Created parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", divisionCode);

            // Get data and return result
            return DbRetrieve("[Application].GetSectionListByDivision", dsSection, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all section depend on DivisionCode.
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string divisionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = GetList(dsSection, divisionCode, connStr);

            // Return result
            if (result)
            {
                return dsSection.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived section.
        /// </summary>
        /// <param name="dsSection"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSection, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return value
            return DbRetrieve("[Application].GetSectionListActive", dsSection, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived section.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = GetList(dsSection, isActive, connStr);

            // Return result
            if (result)
            {
                return dsSection.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived section depend on specified DivisionCode.
        /// </summary>
        /// <param name="dsSection"></param>
        /// <param name="divisionCode"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsSection, string divisionCode, bool isActive, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@DivisionCode", divisionCode);
            dbParams[1] = new DbParameter("@IsActive", isActive.ToString());

            // Get data and return value
            return DbRetrieve("[Application].GetSectionListActiveByDivision", dsSection, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived section depend on specified DivisionCode.
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string divisionCode, bool isActive, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = GetList(dsSection, divisionCode, isActive, connStr);

            // Return result
            if (result)
            {
                return dsSection.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get Section's Description of sepecified SectionCode
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDescription(string sectionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var result = Get(dsSection, sectionCode, connStr);

            // Return result
            if (result)
            {
                if (dsSection.Tables[TableName] != null)
                {
                    if (dsSection.Tables[TableName].Rows.Count > 0)
                    {
                        var drSection = dsSection.Tables[TableName].Rows[0];

                        return (drSection["Description"] == DBNull.Value
                            ? string.Empty
                            : drSection["Description"].ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get max section Code.
        /// </summary>
        /// <returns></returns>
        public int GetNewCode(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("[Application].GetSectionNewCode", null, connStr);

            // If return null value assign 1
            if (result == Convert.ToInt32("0"))
            {
                result = 1;
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsDepartment"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSection, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsSection, SelectCommand, TableName);

            // Commit to database and return result
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}