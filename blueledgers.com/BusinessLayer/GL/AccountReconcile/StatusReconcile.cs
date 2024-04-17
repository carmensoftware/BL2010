using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StatusReconcile : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public StatusReconcile()
        {
            SelectCommand = "SELECT * FROM GL.StatusReconcile";
            TableName = "StatusReconcile";
        }

        /// <summary>
        ///     Get List
        /// </summary>
        /// <param name="dsStatus"></param>
        /// <param name="connStr"></param>
        public bool GetListAll(DataSet dsStatus, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetStatusReconcile", dsStatus, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get status code data for DropDownList
        /// </summary>
        /// <returns></returns>
        public DataTable GetDropDownList(string connStr)
        {
            var dsStatus = new DataSet();

            // Get data
            GetListAll(dsStatus, connStr);

            // Return result
            if (dsStatus.Tables[TableName] != null)
            {
                var drBlank = dsStatus.Tables[TableName].NewRow();
                dsStatus.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsStatus.Tables[TableName];
        }

        #endregion
    }
}