using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileMisc : DbHandler
    {
        #region "Attributies"

        public string AccRecNo{get;set;}

        public Guid FieldID { get; set; }

        public string Value{get;set;}

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccountReconcileMisc()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileMisc";
            TableName = "AccountReconcileMisc";
        }

        /// <summary>
        ///     Get all AccounReconcileMisc data the relate to specified AccRecNo.
        /// </summary>
        /// <param name="dsAccRecMiscDraft"></param>
        /// <param name="AccRecNo"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccRecMisc, string AccRecNo, string ConnectionString)
        {
            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@AccRecNo", AccRecNo);

            return DbRetrieve("GL.GetAccountReconcileMiscByAccRecNo", dsAccRecMisc, dbParam, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get structure of AccounReconcileMisc table.
        /// </summary>
        /// <param name="dsAccRecMiscDraft"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccRecMisc, string ConnectionString)
        {
            return DbRetrieveSchema("GL.GetAccountReconcileMiscList", dsAccRecMisc, null, TableName, ConnectionString);
        }

        #endregion
    }
}