using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherTool : DbHandler
    {
        #region "Attributies"

        public int JournalVoucherToolID { get; set; }

        public int MenuID { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public JournalVoucherTool()
        {
            SelectCommand = "SELECT * FROM Get.JournalVoucherTool";
            TableName = "JournalVoucherTool";
        }

        /// <summary>
        ///     Return dataset from journalvoucher tool.
        /// </summary>
        /// <param name="dsJournalVoucher"></param>
        public void GetList(DataSet dsJournalVoucher, string connStr)
        {
            DbRetrieve("GL.GetJournalVoucherToolList", dsJournalVoucher, null, TableName, connStr);
        }

        #endregion
    }
}