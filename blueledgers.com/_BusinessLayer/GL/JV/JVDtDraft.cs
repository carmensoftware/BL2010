using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class JVDtDraft : DbHandler
    {
        #region "Attributies"

        //private string userName = "Developer";
        //private int buID = 1;

        public string JVNo { get; set; }

        public int JVDtNo { get; set; }

        public string AccountCode { get; set; }

        public decimal DrAmt { get; set; }

        public decimal CrAmt { get; set; }

        public decimal DrBaseAmt { get; set; }

        public decimal CrBaseAmt { get; set; }

        public string Remark { get; set; }

        #endregion

        #region "Operations"

        public JVDtDraft()
        {
            SelectCommand = "SELECT * FROM GL.JVDtDraft";
            TableName = "JVDtDraft";
        }

        /// <summary>
        ///     Return dataset from journalvoucher detail by journal voucherno.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetByJVNo(DataSet dsJV, string jvNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JVNo", jvNo);

            var jvDt = new JVDt();
            jvDt.DbRetrieve("GL.GetJVDt_JVNo", dsJV, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get journal voucher detail data structure.
        /// </summary>
        /// <param name="dsJVDt"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJVDt, string strConn)
        {
            return DbRetrieveSchema("GL.GetJVDtDraftList", dsJVDt, null, TableName, strConn);
        }

        /// <summary>
        ///     Get journal voucher detail data related to specified jv no.
        /// </summary>
        /// <param name="dsJVDt"></param>
        /// <param name="strJVNo"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJVDt, string strJVNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JVNo", strJVNo);

            return DbRetrieve("GL.GetJVDtList_JVNo", dsJVDt, dbParams, TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetMaxNo(string strJVNo, string connStr)
        {
            var dtTmp = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@JVNo", strJVNo);

            // Get data
            dtTmp = DbRead("GL.GetJVDtMaxNo", dbParams, connStr);

            // Return result
            if (dtTmp.Rows[0]["JVDtNo"].ToString() == string.Empty)
            {
                return 0;
            }
            return int.Parse(dtTmp.Rows[0]["JVDtNo"].ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="dsJVDt"></param>
        /// <param name="AccCode"></param>
        /// <param name="connStr"></param>
        public void GetByAccCode(DataSet dsJVDt, string AccCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@AccCode", AccCode);

            var jv = new JV();
            jv.DbRetrieve("GL.GetJVDt_AccCode", dsJVDt, dbParams, TableName, connStr);
        }

        #endregion
    }
}