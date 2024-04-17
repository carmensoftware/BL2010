using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvDt : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StdJvDt()
        {
            SelectCommand = "SELECT * FROM GL.StdJvDt";
            TableName = "StdJvDt";
        }

        /// <summary>
        ///     Get standard voucher detail data structure.
        /// </summary>
        /// <param name="dsStdJvDt"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJvDt, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvDtList", dsStdJvDt, null, TableName, strConn);
        }

        /// <summary>
        ///     Get standard voucher detail data related to specified stdjvno.
        /// </summary>
        /// <param name="dsStdJvDt"></param>
        /// <param name="strStdJvNo"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJvDt, string strStdJvNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StdJvNo", strStdJvNo);

            return DbRetrieve("GL.GetStdJvDtList_StdJvNo", dsStdJvDt, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Return dataset from standardvoucher detail by standardvoucherno.
        /// </summary>
        /// <param name="dsStdJv"></param>
        /// <param name="stdJvNo"></param>
        /// <param name="connStr"></param>
        public void GetByStdJvNo(DataSet dsStdJv, string stdJvNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@StdJvNo", stdJvNo);

            var stdJvDt = new StdJvDt();
            stdJvDt.DbRetrieve("GL.GetStdJvDt_StdJvNo", dsStdJv, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get Detail by stdjvNo
        /// </summary>
        /// <param name="stdJvNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetByStdJvNo(string stdJvNo, string connStr)
        {
            var dtStdJvDt = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@StdJvNo", stdJvNo);

            // Get data
            dtStdJvDt = DbRead("GL.GetStdJvDt_StdJvNo", dbParams, connStr);

            // Return result
            return dtStdJvDt;
        }

        #endregion
    }
}