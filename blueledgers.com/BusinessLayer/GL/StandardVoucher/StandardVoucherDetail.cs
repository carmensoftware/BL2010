using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StandardVoucherDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StandardVoucherDetail()
        {
            SelectCommand = "SELECT * FROM GL.StandardVoucherDetail";
            TableName = "StandardVoucherDetail";
        }

        /// <summary>
        ///     Get standard voucher detail list using standard voucher id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <param name="dsStandardVoucher"></param>
        /// <returns></returns>
        public bool GetStandardVoucherDetailList(int standardVoucherID, DataSet dsStandardVoucher, string connStr)
        {
            var result = false;
            //DataTable dtStandardVoucherDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherID", standardVoucherID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStandardVoucherDetailListByStandardVoucherID", dsStandardVoucher, dbParams,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get standard voucher detail using standard voucher id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <returns></returns>
        public DataTable GetStandardVoucherDetailListByStandardVoucherID(int standardVoucherID, string connStr)
        {
            var dtStandardVoucherDetail = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StandardVoucherID", standardVoucherID.ToString());

            // Get data
            dtStandardVoucherDetail = DbRead("GL.GetStandardVoucherDetailListByStandardVoucherID", dbParams, connStr);

            // Return result
            return dtStandardVoucherDetail;
        }

        /// <summary>
        ///     Get StandardVoucherDetail
        /// </summary>
        /// <param name="dsStandardVoucherDetail"></param>
        /// <returns></returns>
        public bool GetStandardVoucherDetailStructure(DataSet dsStandardVoucherDetail, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("GL.GetStandardVoucherDetailList", dsStandardVoucherDetail, null, TableName,
                connStr);

            // Return result
            return result;
        }

        public bool Delete(DataSet dsSVDetail, string connStr)
        {
            var result = false;
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource            
            dbSaveSorce[0] = new DbSaveSource(dsSVDetail, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}