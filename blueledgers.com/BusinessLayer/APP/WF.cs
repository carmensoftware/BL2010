using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.APP
{
    public class WF : DbHandler
    {
        #region "Attributes"

        private readonly WFDt _wfDt = new WFDt();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public WF()
        {
            SelectCommand = "SELECT * FROM [APP].[WF]";
            TableName = "WF";
        }

        /// <summary>
        ///     Get work-flow config data.
        /// </summary>
        /// <param name="dsWF"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsWF, string connStr)
        {
            return DbRetrieve("APP_WF_GetList", dsWF, null, TableName, connStr);
        }

        /// <summary>
        ///     Get work-flow config data.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsWF = new DataSet();

            var result = GetList(dsWF, connStr);

            if (result)
            {
                return dsWF.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get WF data by Module and SubModule
        /// </summary>
        /// <param name="dsWF"></param>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsWF, string module, string subModule, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@SubModule", subModule);

            return DbRetrieve("[dbo].[APP_WF_Get_Module_SubModule]", dsWF, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get WF's IsActive Data by Module and SubModule
        /// </summary>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetWFId(string module, string subModule, string connStr)
        {
            var dsWF = new DataSet();

            var result = Get(dsWF, module, subModule, connStr);

            if (result)
            {
                if (dsWF.Tables[TableName].Rows.Count > 0)
                {
                    return int.Parse(dsWF.Tables[TableName].Rows[0]["WFId"].ToString());
                }
            }

            return 0;
        }

        /// <summary>
        ///     Get WF's IsActive Data by Module and SubModule
        /// </summary>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetIsActive(string module, string subModule, string connStr)
        {
            var dsWF = new DataSet();

            var result = Get(dsWF, module, subModule, connStr);

            if (result)
            {
                if (dsWF.Tables[TableName].Rows.Count > 0)
                {
                    return bool.Parse(dsWF.Tables[TableName].Rows[0]["IsActive"].ToString());
                }
            }

            return false;
        }

        /// <summary>
        ///     Get new header approve status for new transaction.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetHdrApprStatus(string module, string subModule, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@SubModule", subModule);

            var dtWF = DbRead("APP.WF_GetApprStatus", dbParams, connStr);

            if (dtWF != null)
            {
                if (dtWF.Rows.Count > 0)
                {
                    return dtWF.Rows[0]["ApprStatus"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get new detail approve status for new transaction.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDtApprStatus(string module, string subModule, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@SubModule", subModule);

            var apprStatus = string.Empty;

            var dtWF = DbRead("[APP].[WFDt_GetApprStatus]", dbParams, connStr);

            if (dtWF != null)
            {
                if (dtWF.Rows.Count > 0)
                {
                    for (var i = 0; i < dtWF.Rows.Count; i++)
                    {
                        apprStatus += dtWF.Rows[i][0].ToString();
                    }
                }
            }

            return apprStatus;
        }

        /// <summary>
        ///     Commit changed to database
        /// </summary>
        /// <param name="dsWF"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsWF, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsWF, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsWF, _wfDt.SelectCommand, _wfDt.TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}