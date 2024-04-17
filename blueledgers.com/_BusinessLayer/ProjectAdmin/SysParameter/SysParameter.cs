using System.Data;
using Blue.DAL;

namespace Blue.BL.ProjectAdmin
{
    public class SysParameter : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public SysParameter()
        {
            SelectCommand = "SELECT * FROM ProjectAdmin.SysParameter";
            TableName = "SysParameter";
        }

        /// <summary>
        ///     Get System Parameter using Section and Text.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public DataTable GetSysParameter(string section, string text, string connStr)
        {
            var result = new DataTable();
            var dbParams = new DbParameter[2];

            // Create parameter
            dbParams[0] = new DbParameter("@Section", section);
            dbParams[1] = new DbParameter("@Text", text);

            // Get data
            result = DbRead("ProjectAdmin.GetSysParameter", dbParams, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get System Parameter List using Section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public DataTable GetSysParameterList(string section, string connStr)
        {
            var dtSysParameter = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@Section", section);

            // Get data
            dtSysParameter = DbRead("ProjectAdmin.GetSysParameterList", dbParams, connStr);

            // Return result
            return dtSysParameter;
        }

        /// <summary>
        ///     Get System Parameter List using Section for DropDownList.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public DataTable GetSysParameterListForDropDownList(string section, string connStr)
        {
            var dtSysParameter = new DataTable();
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@Section", section);

            // Get data
            dtSysParameter = DbRead("ProjectAdmin.GetSysParameterList", dbParams, connStr);
            var drBlank = dtSysParameter.NewRow();
            dtSysParameter.Rows.InsertAt(drBlank, 0);

            // Return result
            return dtSysParameter;
        }

        /// <summary>
        ///     Get value of specified Section and Text
        /// </summary>
        public string GetValue(string section, string text, string connStr)
        {
            var sysParam = new DataTable();

            // Get data
            sysParam = GetSysParameter(section, text, connStr);

            if (sysParam != null)
            {
                // Return result
                return sysParam.Rows[0]["Value"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get all of sysparameters with specified section.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(string section, string connStr)
        {
            var dsResult = new DataSet();

            // Create Parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Section", section);

            // Get data
            var retrieved = DbRetrieve("ProjectAdmin.GetSysParameterList", dsResult, dbParams, TableName, connStr);

            if (retrieved)
            {
                return dsResult;
            }
            return null;
        }

        /// <summary>
        ///     Get all of sysparameter.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSysParameterList(DataSet dsSystemParameter, string connStr)
        {
            // Get all data
            return DbRetrieve("ProjectAdmin.GetSysParameterListAll", dsSystemParameter, null, TableName, connStr);
        }

        /// <summary>
        ///     Save process.
        /// </summary>
        /// <param name="dsRole"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSystemParameter, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSystemParameter, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}