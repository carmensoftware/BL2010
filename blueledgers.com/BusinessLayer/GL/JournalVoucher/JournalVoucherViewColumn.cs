using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class JournalVoucherViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public JournalVoucherViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.JournalVoucherViewColumn";
            TableName = "JournalVoucherViewColumn";
        }

        public int JournalVoucherViewColumnID { get; set; }

        public int JournalVoucherViewID { get; set; }

        public int SeqNo { get; set; }

        /// <summary>
        ///     Generate column list using journal voucher view ID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetJournalVoucherViewColumn(int journalvoucherViewID, string connStr)
        {
            var journalvoucherViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get Data
            journalvoucherViewColumn = new DbHandler().DbRead(
                "GL.GetJournalVoucherViewColumnListByJournalVoucherViewID", dbParams, connStr);

            // Generate Column List
            if (journalvoucherViewColumn != null)
            {
                foreach (DataRow dr in journalvoucherViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetJournalVoucherViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var journalvoucherViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            journalvoucherViewColumn = dtPreview;

            // Generate Column List
            if (journalvoucherViewColumn != null)
            {
                foreach (DataRow dr in journalvoucherViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        ///     Get journalvoucher view column schema
        /// </summary>
        /// <param name="journalvoucherViewColumn"></param>
        /// <returns></returns>
        public bool GetJournalVoucherViewColumnSchema(DataSet journalvoucherViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetJournalVoucherViewColumnList", journalvoucherViewColumn, null, TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get column list using journalvoucherViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public bool GetJournalVoucherViewColumnList(int journalvoucherViewID, DataSet journalvoucherViewColumn,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@journalvoucherViewID", journalvoucherViewID.ToString());

            // Get Data
            result = DbRetrieve("GL.GetJournalVoucherViewColumnListByJournalVoucherViewID", journalvoucherViewColumn,
                dbParams, TableName, connStr);

            // return result
            return result;
        }

        #endregion
    }
}