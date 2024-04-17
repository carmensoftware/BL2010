using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.AccountListing
{
    public class AccountDetailViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public AccountDetailViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.AccountDetailViewCriteria";
            TableName = "AccountDetailViewCriteria";
        }

        /// <summary>
        ///     This function is used for get table structure of table GL.AccountDetailViewCriteria by
        ///     using stored procedure "GL.GetAccountDetailViewCriteriaList".
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsAccDtView, string connStr)
        {
            // Declare variable
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountDetailViewCriteriaList", dsAccDtView, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     This function is used for get all GL.AccountDetailViewCriteria data that related to
        ///     specified view id by using stored procedure "GL.GetAccountDetailViewCriteria_ViewID".
        /// </summary>
        /// <param name="dsAccDtView"></param>
        /// <param name="viewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccDtView, int viewID, string connStr)
        {
            // Declare variable
            var result = false;

            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", viewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountDetailViewCriteriaList_ViewID", dsAccDtView, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate whereClause list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetCriteriaQuery(DataSet dsPreview, int userID, string connStr)
        {
            int accDtViewID;
            var whereClause = string.Empty;
            var accDtView = new DataTable();
            var accDtViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["AccountDetailView"] != null)
            {
                accDtView = dsPreview.Tables["AccountDetailView"];

                accDtViewID = int.Parse(accDtView.Rows[0]["AccountDetailViewID"].ToString());

                if (accDtView != null)
                {
                    // Get trialBalance view criteria data
                    if (dsPreview.Tables["AccountDetailViewCriteria"] != null)
                    {
                        accDtViewCriteria = dsPreview.Tables["AccountDetailViewCriteria"];

                        // Non-Advance option
                        if (accDtView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in accDtViewCriteria.Rows)
                            {
                                whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                               field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                               dr["Operator"] + " " +
                                               "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                               " " + dr["LogicalOp"];
                            }
                        }
                            //Advance option
                        else
                        {
                            whereClause = accDtView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in accDtViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) accDtView.Rows[0]["IsAll"])
                        {
                            whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " +
                                           userID;
                        }
                    }
                }
            }

            // Return result
            return whereClause;
        }

        #endregion
    }
}