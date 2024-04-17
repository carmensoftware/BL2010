using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.TP
{
    public class TemplateDt : DbHandler
    {
        public TemplateDt()
        {
            SelectCommand = "SELECT * FROM [In].[TemplateDt]";
            TableName = "TemplateDt";
        }

        /// <summary>
        ///     Gets all active TemplateDt data.
        /// </summary>
        /// <param name="dsTemplateDt"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetListByTmpNo(DataSet dsTemplateDt, int tmpNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", tmpNo.ToString());

            var result = DbRetrieve("dbo.TPDt_GetListByTmpNo", dsTemplateDt, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetReqQtyList(DataSet dsTemplateDt, int TmpNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", TmpNo.ToString());

            return DbRetrieve("[IN].TemplateDt_GetReqQtyList_TmpNo", dsTemplateDt, dbParams, TableName, ConnStr);
        }

        public DataTable GetListByTmpNo(string tmpNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", tmpNo);

            return DbRead("dbo.TPDt_GetListByTmpNo", dbParams, connStr);
        }

        /// <summary>
        ///     Get Max TemplateDt No.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewTmpDtNo(string connStr)
        {
            var NewID = DbReadScalar("dbo.TP_GetTmpNewNo", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Lookup TemplateDtCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.TPDt_GetList", null, connStr);
        }

        public bool GetList(DataSet dsTemplateDt, int TmpNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", TmpNo.ToString());

            return DbRetrieve("dbo.TPDt_GetListByTmpNo", dsTemplateDt, dbParams, TableName, ConnStr);
        }

        /// <summary>
        ///     Get template detail schema
        /// </summary>
        /// <param name="dsTemplateDt"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsTemplateDt, string ConnStr)
        {
            return DbRetrieve("dbo.IN_TemplateDt_GetSchema", dsTemplateDt, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsTemplateDt, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsTemplateDt, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public int GetTmpDtNewNo(string connStr)
        {
            var NewID = DbReadScalar("dbo.TP_GetTmpDtNewNo", null, connStr);

            // Return result
            return NewID;
        }

        public int CountByTemplateDt(string TemplateDt, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TemplateDt", TemplateDt);

            return DbReadScalar("dbo.TemplateDt_CountByTemplateDtNo", dbParams, ConnStr);
        }
    }
}