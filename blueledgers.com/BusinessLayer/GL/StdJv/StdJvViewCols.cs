using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvViewCols : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public StdJvViewCols()
        {
            SelectCommand = "SELECT * FROM GL.StdJvViewCols";
            TableName = "StdJvViewCols";
        }

        /// <summary>
        ///     Get standard voucher view column list based on related view id.
        /// </summary>
        /// <param name="dsStdJvViewCols"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJvViewCols, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetStdJvViewColsList_ViewID", dsStdJvViewCols, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get columns query of specified view id
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetColumn(int intViewID, string strConn)
        {
            var sbColsList = new StringBuilder();

            // Add primary key column for default.
            sbColsList.Append("[StdJvNo] AS PK ");

            // Add view column.
            var dsStdJvViewCols = new DataSet();
            var result = GetList(dsStdJvViewCols, intViewID, strConn);

            if (result)
            {
                foreach (DataRow drStdJvViewCols in dsStdJvViewCols.Tables[TableName].Rows)
                {
                    sbColsList.Append(", [" + drStdJvViewCols["FieldName"] + "]");
                }
            }

            return sbColsList.ToString();
        }

        /// <summary>
        ///     Get table structure.
        /// </summary>
        /// <param name="dsJVViewCols"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJvViewCols, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvViewColsList", dsStdJvViewCols, null, TableName, strConn);
        }

        #endregion
    }
}