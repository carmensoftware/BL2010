using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvLg : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StdJvLg()
        {
            SelectCommand = "SELECT * FROM GL.StdJvLg";
            TableName = "StdJvLg";
        }


        /// <summary>
        ///     Get standardvoucher ledger data structure.
        /// </summary>
        /// <param name="dsStdJvLg"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJvLg, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvLedgersList", dsStdJvLg, null, TableName, strConn);
        }


        /// <summary>
        ///     Get ledgers data for standard voucher.
        ///     By using StdJvNo as parameters.
        /// </summary>
        /// <param name="dsLedgers"></param>
        /// <param name="srtHdrNo"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJvLg, string srtStdJvNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StdJvNo", srtStdJvNo);

            return DbRetrieve("GL.GetStdJvLedgersList_StdJvNo", dsStdJvLg, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get the number of ledgers ralated to specified account code.
        /// </summary>
        /// <param name="strAccCode"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public int Count(string strAccCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", strAccCode);

            return DbReadScalar("GL.GetLedgersCount_AccCode", dbParams, strConn);
        }

        #endregion
    }
}