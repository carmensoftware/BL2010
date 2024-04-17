using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public AccountReconcileDraft()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileDraft";
            TableName = "AccountReconcileDraft";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccRecDraft"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccRecDraft, string connStr)
        {
            var result = false;

            result = DbRetrieve("GL.GetAccountReconcileDraftList", dsAccRecDraft, null, TableName, connStr);

            return result;
        }

        /// <summary>
        ///     Get data by created by.
        /// </summary>
        /// <param name="dsAccRecDraft"></param>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByCreatedBy(DataSet dsAccRecDraft, int userId, string connStr)
        {
            var result = false;
            var dbParam = new DbParameter[1];

            dbParam[0] = new DbParameter("@CreatedBy", userId.ToString());

            result = DbRetrieve("GL.GetAccountReconcileDraftListByCreated", dsAccRecDraft, dbParam, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="dsSDVDraft"></param>
        /// <param name="sdvID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByAccRecNo(DataSet dsAccRecDraft, string AccRecNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccRecNo", AccRecNo);

            return DbRetrieve("GL.GetAccountReconcileDraftListByID", dsAccRecDraft, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get standardvoucherdraft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecDraft, string conStr)
        {
            return DbRetrieveSchema("GL.GetAccountReconcileDraftList", dsAccRecDraft, null, TableName, conStr);
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
        ///     Save data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var accRecDetailDraft = new AccountReconcileDraft();
            var accRecMiscDraft = new AccountReconcileMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accRecDetailDraft.SelectCommand, accRecDetailDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, accRecMiscDraft.SelectCommand, accRecMiscDraft.TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        /// <summary>
        ///     Delete.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet savedData, string connStr)
        {
            var accRecDetailDraft = new AccountReconcileDraft();
            var accRecMiscDraft = new AccountReconcileMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(savedData, accRecDetailDraft.SelectCommand, accRecDetailDraft.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, accRecMiscDraft.SelectCommand, accRecMiscDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        #endregion
    }
}