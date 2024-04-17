using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class JVViewCols : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public JVViewCols()
        {
            SelectCommand = "SELECT * FROM GL.JVViewCols";
            TableName = "JVViewCols";
        }

        /// <summary>
        ///     Get standard voucher view column list related to specified view id.
        /// </summary>
        /// <param name="dsStdJvViewCols"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJvViewCols, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetJVViewColsList_ViewID", dsStdJvViewCols, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get table structure.
        /// </summary>
        /// <param name="dsStdJvViewCols"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsStdJvViewCols, string strConn)
        {
            return DbRetrieveSchema("GL.GetJVViewColsList", dsStdJvViewCols, null, TableName, strConn);
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
            sbColsList.Append("[JVNo] AS PK ");

            // Add view column.
            var dsStdJvViewCols = new DataSet();
            var result = GetList(dsStdJvViewCols, intViewID, strConn);

            if (result)
            {
                foreach (DataRow drJVViewCols in dsStdJvViewCols.Tables[TableName].Rows)
                {
                    sbColsList.Append(", [" + drJVViewCols["FieldName"] + "]");
                }
            }

            return sbColsList.ToString();
        }

        #endregion
    }
}