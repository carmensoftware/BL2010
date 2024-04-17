using System.Data;
using System.Globalization;
using System.Text;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.GL.Account
{
    public class AccountViewColumn : DbHandler
    {
        #region "Attributies"

/*
        private APP.Field _field = new APP.Field();
*/

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public AccountViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.AccountViewCols";
            TableName = "AccountViewColumn";
        }

        /// <summary>
        ///     Get account view column list related to specified view id.
        /// </summary>
        /// <param name="dsAccViewCols"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccViewCols, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString(CultureInfo.InvariantCulture));
            return DbRetrieve("GL.GetAccViewColsList_ViewID", dsAccViewCols, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get table structure.
        /// </summary>
        /// <param name="dsAccViewCols"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccViewCols, string strConn)
        {
            return DbRetrieveSchema("GL.GetAccViewColsList", dsAccViewCols, null, TableName, strConn);
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
            sbColsList.Append("[AccCode] AS PK ");

            // Add view column.
            var dsAccViewCols = new DataSet();
            var result = GetList(dsAccViewCols, intViewID, strConn);

            if (result)
            {
                foreach (DataRow drAccViewCols in dsAccViewCols.Tables[TableName].Rows)
                {
                    sbColsList.Append(", [" + drAccViewCols["FieldName"] + "]");
                }
            }

            return sbColsList.ToString();
        }

        #endregion
    }
}