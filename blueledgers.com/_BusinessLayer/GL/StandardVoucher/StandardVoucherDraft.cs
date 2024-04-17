using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherDraft : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        public StandardVoucherDraft()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherDraft";
            TableName = "StandardVoucherDraft";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStandardVoucherDraft"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStandardVoucherDraft, string connStr)
        {
            var result = false;

            result = DbRetrieve("GL.GetStandardVoucherDraftList", dsStandardVoucherDraft, null, TableName, connStr);

            return result;
        }

        /// <summary>
        ///     Get data by created by.
        /// </summary>
        /// <param name="dsStandardVoucherDraft"></param>
        /// <param name="userId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByCreatedBy(DataSet dsStandardVoucherDraft, int userId, string connStr)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@CreatedBy", userId.ToString());

            return DbRetrieve("GL.GetStandardVoucherDraftListByCreatedBy", dsStandardVoucherDraft, dbParam, TableName,
                connStr);
        }

        /// <summary>
        ///     Get data by id.
        /// </summary>
        /// <param name="dsSDVDraft"></param>
        /// <param name="sdvID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByID(DataSet dsSDVDraft, string sdvID, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StandardVoucherID", sdvID);

            return DbRetrieve("GL.GetStandardVoucherDraftListByID", dsSDVDraft, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get standardvoucherdraft table schema.
        /// </summary>
        /// <param name="journalVoucher"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStandardVoucher, string conStr)
        {
            return DbRetrieveSchema("GL.GetStandardVoucherDraftList", dsStandardVoucher, null, TableName, conStr);
        }

        /// <summary>
        ///     Save data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var standardVoucherDetailDraft = new StandardVoucherDetailDraft();
            var standardVoucherMiscDraft = new StandardVoucherMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, standardVoucherDetailDraft.SelectCommand,
                standardVoucherDetailDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, standardVoucherMiscDraft.SelectCommand,
                standardVoucherMiscDraft.TableName);

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
            var standardVoucherDetailDraft = new StandardVoucherDetailDraft();
            var standardVoucherMiscDraft = new StandardVoucherMiscDraft();

            var result = false;

            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(savedData, standardVoucherDetailDraft.SelectCommand,
                standardVoucherDetailDraft.TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, standardVoucherMiscDraft.SelectCommand,
                standardVoucherMiscDraft.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call dbCommit and send savesource object is parameter
            result = DbCommit(dbSaveSource, connStr);

            return result;
        }

        #endregion
    }
}