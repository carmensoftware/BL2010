using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.TrBal
{
    public class TrBalViewColumn : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public TrBalViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.TrBalViewCols";
            TableName = "TrBalViewColumn";
        }

        /// <summary>
        ///     Get trial balance view column list related to specified view id.
        /// </summary>
        /// <param name="dsTrBalViewCols"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTrBalViewCols, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("[GL].[GetTrBalViewColsList_ViewID]", dsTrBalViewCols, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get table structure.
        /// </summary>
        /// <param name="dsTrBalViewCols"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsTrBalViewCols, string strConn)
        {
            return DbRetrieveSchema("[GL].[GetTrBalViewColsList]", dsTrBalViewCols, null, TableName, strConn);
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
            sbColsList.Append(" CAST([FYear] AS NVARCHAR(4))+','+CAST([FPeriod] AS NVARCHAR(2))+','+[AccCode] AS PK ");

            // Add view column.
            var dsTrBalViewCols = new DataSet();
            var result = GetList(dsTrBalViewCols, intViewID, strConn);

            if (result)
            {
                foreach (DataRow drTrBalViewCols in dsTrBalViewCols.Tables[TableName].Rows)
                {
                    sbColsList.Append(", [" + drTrBalViewCols["FieldName"] + "]");
                }
            }

            return sbColsList.ToString();
        }

        #endregion
    }
}