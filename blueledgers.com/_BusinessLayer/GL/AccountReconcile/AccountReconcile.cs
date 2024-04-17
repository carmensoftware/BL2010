using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcile : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public AccountReconcile()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcile";
            TableName = "AccountReconcile";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <param name="standardVoucherViewID"></param>
        /// <param name="connStr"></param>
        public bool GetList(DataSet dsAccRec, int AccRecViewID, string connStr)
        {
            var result = false;

            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccRecViewID", AccRecViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileGenerateColumnList", dsAccRec, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccRec"></param>
        /// <param name="AccRecNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccRec, string AccRecNo, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AccRecNo", AccRecNo);

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileByRecNo", dsAccRec, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all data
        /// </summary>
        /// <param name="dsAccRec"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListAll(DataSet dsAccRec, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileList", dsAccRec, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccRec"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRec, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountReconcileList", dsAccRec, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max accrecno
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetMaxNo(string connStr)
        {
            var dsAccRec = new DataSet();
            string maxNo;
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileMaxAccRecNo", dsAccRec, null, TableName, connStr);

            if (result)
            {
                maxNo = dsAccRec.Tables["AccountReconcile"].Rows[0]["AccRecNo"].ToString();
            }
            else
            {
                maxNo = string.Empty;
            }

            // Return result
            return maxNo;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccRec, string connStr)
        {
            var result = false;
            var accRecAttachment = new AccountReconcileAttachment();
            var accRecActLog = new AccountReconcileActLog();
            var accRecComment = new AccountReconcileComment();
            var accRecMisc = new AccountReconcileMisc();
            var accRecDetail = new AccountReconcileDetail();
            var jv = new JournalVoucher();
            var jvDetail = new JournalVoucherDetail();

            //DbSaveSource[] dbSaveSorce                  = new DbSaveSource[6];
            var dbSaveSorce = new DbSaveSource[8];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsAccRec, jv.SelectCommand, jv.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsAccRec, jvDetail.SelectCommand, jvDetail.TableName);

            dbSaveSorce[2] = new DbSaveSource(dsAccRec, SelectCommand, TableName);
            dbSaveSorce[3] = new DbSaveSource(dsAccRec, accRecDetail.SelectCommand, accRecDetail.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsAccRec, accRecMisc.SelectCommand, accRecMisc.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsAccRec, accRecComment.SelectCommand, accRecComment.TableName);
            dbSaveSorce[6] = new DbSaveSource(dsAccRec, accRecAttachment.SelectCommand, accRecAttachment.TableName);
            dbSaveSorce[7] = new DbSaveSource(dsAccRec, accRecActLog.SelectCommand, accRecActLog.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsStandardVoucher, string connStr)
        {
            var result = false;
            var accRecAttachment = new AccountReconcileAttachment();
            var accRecActLog = new AccountReconcileActLog();
            var accRecComment = new AccountReconcileComment();
            var accRecMisc = new AccountReconcileMisc();
            var accRecDetail = new AccountReconcileDetail();
            var dbSaveSorce = new DbSaveSource[6];

            // Create dbSaveSource            
            dbSaveSorce[0] = new DbSaveSource(dsStandardVoucher, accRecDetail.SelectCommand, accRecDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsStandardVoucher, accRecMisc.SelectCommand, accRecMisc.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsStandardVoucher, accRecAttachment.SelectCommand,
                accRecAttachment.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsStandardVoucher, accRecComment.SelectCommand, accRecComment.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsStandardVoucher, accRecActLog.SelectCommand, accRecActLog.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsStandardVoucher, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}