using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JoutnalVoucherReport : DbHandler
    {
        #region "Attributies"

        public int JournalVoucherReportID { get; set; }

        public int MenuID { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public JoutnalVoucherReport()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherReport";
            TableName = "JournalVoucherReport";
        }

        /// <summary>
        ///     Return dataset from journalvoucher report.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetJournalReportList(DataSet dsJournalVoucher, string connStr)
        {
            var journalVoucherReport = new JoutnalVoucherReport();
            journalVoucherReport.DbRetrieve("GL.GetJournalVoucherReportList", dsJournalVoucher, null, TableName, connStr);
        }

        #endregion
    }
}