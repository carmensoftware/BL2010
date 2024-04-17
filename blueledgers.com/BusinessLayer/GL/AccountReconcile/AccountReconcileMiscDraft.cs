using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileMiscDraft : DbHandler
    {
        #region "Attributies"

        //private AccountReconcileDraft _AccountReconcileDraft;
        //private string _accRecNo;
        //private Guid fieldID;
        //private string value;

        //public string AccRecNo
        //{
        //    get
        //    {
        //        return AccRecNo;
        //    }

        //    set
        //    {
        //        AccRecNo = value;
        //    }
        //}

        //public Guid FieldID
        //{
        //    get
        //    {
        //        return fieldID;
        //    }

        //    set
        //    {
        //        fieldID = value;
        //    }
        //}

        //public string Value
        //{
        //    get
        //    {
        //        return value;
        //    }

        //    set
        //    {
        //        value = value;
        //    }
        //}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountReconcileMiscDraft()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileMiscDraft";
            TableName = "AccountReconcileMiscDraft";
        }

        /// <summary>
        ///     Get all AccounReconcileMiscDraft data the relate to specified AccRecNo.
        /// </summary>
        /// <param name="dsAccRecMiscDraft"></param>
        /// <param name="AccRecNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccRecMiscDraft, string AccRecNo, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@AccRecNo", AccRecNo);

            return DbRetrieve("GL.GetAccountReconcileMiscDraftByAccRecNo", dsAccRecMiscDraft, dbParam, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get structure of AccounReconcileMiscDraft table.
        /// </summary>
        /// <param name="dsAccRecMiscDraft"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecMiscDraft, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetAccountReconcileMiscDraftList", dsAccRecMiscDraft, null, TableName,
                ConnectionString);
        }

        #endregion
    }
}