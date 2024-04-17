using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.Rec
{
    public class RecViewCols : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public RecViewCols()
        {
            SelectCommand = "SELECT * FROM GL.RecViewCols";
            TableName = "RecViewCols";
        }

        /// <summary>
        ///     Get standard voucher view column list related to specified view id.
        /// </summary>
        /// <param name="dsStdJvViewCols"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsViewCols, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetRecViewColsList_ViewID", dsViewCols, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get table structure.
        /// </summary>
        /// <param name="dsStdJvViewCols"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsViewCols, string strConn)
        {
            return DbRetrieveSchema("GL.GetRecViewColsList", dsViewCols, null, TableName, strConn);
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
            sbColsList.Append("[RecNo] AS PK ");

            // Add view column.
            var dsViewCols = new DataSet();
            var result = GetList(dsViewCols, intViewID, strConn);

            if (result)
            {
                foreach (DataRow drViewCols in dsViewCols.Tables[TableName].Rows)
                {
                    sbColsList.Append(", [" + drViewCols["FieldName"] + "]");
                }
            }

            return sbColsList.ToString();
        }

        #endregion
    }
}