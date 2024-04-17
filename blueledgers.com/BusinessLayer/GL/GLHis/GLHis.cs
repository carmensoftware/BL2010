using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class GLHis : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public GLHis()
        {
            SelectCommand = "SELECT * FROM GL.vGLHis";
            TableName = "vGLHis";
        }

        /// <summary>
        ///     This function is used for get table structure of GL.AccountDetailView by
        ///     using stored procedure "GL.GetAccountDetailViewList".
        /// </summary>
        /// <param name="dsTrialBalLst"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public void GetvGLHisTrialBalanceList(DataSet dsTrialBalLst, int viewID, string connStr)
        {
            var dtTrialBal = new DataTable();
            var gLHis = new GLHis();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());


            // Get data                
            dtTrialBal = gLHis.DbRead("GL.GetvGLHisTrialBalanceList", dbParams, connStr);

            // Return result
            if (dsTrialBalLst.Tables[TableName] != null)
            {
                dsTrialBalLst.Tables.Remove(TableName);
            }

            dtTrialBal.TableName = TableName;
            dsTrialBalLst.Tables.Add(dtTrialBal);
        }


        /// <summary>
        ///     This stored procedure is used for get all Account Detail List Report data in view GL.vGLHis.
        /// </summary>
        /// <param name="dsAccDtLst"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetvGLHisAccountDetailList(DataSet dsAccDtLst, int viewID, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetvGLHisAccountDetailList", dsAccDtLst, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This stored procedure is used for get all account detail report data that related to specified account code from
        ///     view GL.vGLHis.
        /// </summary>
        /// <param name="dsAccDtLst"></param>
        /// <param name="accountCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetvGLHisAccountDetailList_AccountCode(DataSet dsAccDtLst, string accountCode, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[0];

            // Create parameter
            dbParams[0] = new DbParameter("@AccountCode", accountCode);

            // Get data
            result = DbRetrieve("GL.GetvGLHisAccountDetailList_AccountCode", dsAccDtLst, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}