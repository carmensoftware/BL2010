using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
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
            SelectCommand = "SELECT * FROM Reference.Section";
            TableName = "Section";
        }

        /// <summary>
        ///     Get name section
        /// </summary>
        /// <param name="DivisionCode"></param>
        /// <returns></returns>
        public string GetName(string SectionCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SectionCode", SectionCode);

            DbRetrieve("GL.GetSection", dsTmp, dbParams, TableName, connStr);
            return dsTmp.Tables[TableName].Rows[0]["Description"].ToString();
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <returns></returns>
        public DataTable GetSectionList(string connStr)
        {
            return DbRead("GL.GetSectionList", null, connStr);
        }

        /// <summary>
        ///     Get all data to look up
        /// </summary>
        /// <returns></returns>
        public DataTable GetSectionLookupList(string connStr)
        {
            var dsSection = new DataSet();
            var section = new Section();

            // Retrieve data.
            section.DbRetrieve("GL.GetSectionList", dsSection, null, TableName, connStr);

            // Return result
            if (dsSection.Tables[TableName] != null)
            {
                var drBlank = dsSection.Tables[TableName].NewRow();
                dsSection.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSection.Tables[TableName];
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsDivision"></param>
        /// <returns></returns>
        public bool GetSectionList(DataSet dsSection, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetSectionList", dsSection, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get only data from param
        /// </summary>
        /// <returns></returns>
        public DataTable GetSectionLookup(string DivisionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var section = new Section();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", DivisionCode);

            section.DbRetrieve("Reference.GetSectionListByDivision", dsSection, dbParams, TableName, connStr);

            // Return result
            if (dsSection.Tables[TableName] != null)
            {
                var drBlank = dsSection.Tables[TableName].NewRow();
                dsSection.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsSection.Tables[TableName];
        }

        /// <summary>
        ///     Get active section related to sepecified division code.
        /// </summary>
        /// <param name="strDivCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string strDivCode, string strConn)
        {
            var dsSec = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", strDivCode);
            var result = DbRetrieve("Reference.GetSectionActiveList_DivisionCode", dsSec, dbParams, TableName, strConn);

            if (result)
            {
                return dsSec.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get only data by department code
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSectionLookupByDepartmentCode(string DivisionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var section = new Section();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", DivisionCode);

            section.DbRetrieve("GL.GetSectionLookup", dsSection, dbParams, TableName, connStr);

            return dsSection.Tables[TableName];
        }

        /// <summary>
        ///     Get table section by division code
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="dsSection"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetSectionListByDivision(string divisionCode, DataSet dsSection, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", divisionCode);

            //return DbRead("GL.GetBudgetSAcc", dbParams);
            DbRetrieve("Reference.GetSectionListByDivision", dsSection, dbParams, TableName, connStr);

            return dsSection;
        }


        /// <summary>
        ///     Get table section by division code
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetSectionListByDivision(string divisionCode, string connStr)
        {
            var dsSection = new DataSet();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", divisionCode);


            DbRetrieve("Reference.GetSectionListByDivision", dsSection, dbParams, TableName, connStr);

            return dsSection;
        }

        /// <summary>
        ///     Get only data by department code
        /// </summary>
        /// <param name="DepartmentCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSectionListByDiv(string DivisionCode, string connStr)
        {
            var dsSection = new DataSet();

            // Get data
            var section = new Section();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DivisionCode", DivisionCode);

            section.DbRetrieve("Reference.GetSectionListByDivision", dsSection, dbParams, TableName, connStr);

            return dsSection.Tables[TableName];
        }

        /// <summary>
        ///     Get max section Code.
        /// </summary>
        /// <returns></returns>
        public int GetSectionCodeMax(string connStr)
        {
            var result = 0;

            // Get data
            result = DbReadScalar("Reference.GetSectionCodeMax", null, connStr);

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
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsSection, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}