using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class AccountReconcileViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public AccountReconcileViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.AccountReconcileViewCriteria";
            TableName = "AccountReconcileViewCriteria";
        }

        /// <summary>
        ///     Get accountreconcile view criteria using accrec view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountReconcileViewCriteriaList(int accRecViewID, string connStr)
        {
            var AccountReconcileViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@accRecViewID", accRecViewID.ToString());

            // Get data
            AccountReconcileViewCriteria = DbRead("GL.GetAccountReconcileViewCriteriaListByaccRecViewID", dbParams,
                connStr);

            //Return result
            return AccountReconcileViewCriteria;
        }

        /// <summary>
        ///     Get accountreconcile view criteria using accrec view id
        /// </summary>
        /// <returns></returns>
        public bool GetAccountReconcileViewCriteriaList(int accRecViewID, DataSet dsAccountReconcileViewCriteria,
            string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@accRecViewID", accRecViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetAccountReconcileViewCriteriaListByAccRecViewID", dsAccountReconcileViewCriteria,
                dbParams, TableName, connStr);

            //Return result
            return result;
        }


        /// <summary>
        ///     Get accountreconcile view criteria schema
        /// </summary>
        /// <param name="accountViewCriteria"></param>
        /// <returns></returns>
        public bool GetAccountReconcileViewCriteriaSchema(DataSet dsAccountReconcileViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetAccountReconcileViewCriteriaList", dsAccountReconcileViewCriteria, null,
                TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all available criterai column.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCriteriaColumns(string connStr)
        {
            return DbRead("GL.GetAccountReconcileViewCriteriaAvailableColumns", null, connStr);
        }

        /// <summary>
        ///     whereClause
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetAccountReconcileViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var AccountReconcileView = new DataTable();
            var AccountReconcileViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int AccRecViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance optionj
            if (dsPreview.Tables["AccountReconcileView"] != null)
            {
                AccountReconcileView = dsPreview.Tables["AccountReconcileView"];

                AccRecViewID = int.Parse(AccountReconcileView.Rows[0]["AccRecViewID"].ToString());

                if (AccountReconcileView != null)
                {
                    // Get AccountReconcile view criteria data

                    if (dsPreview.Tables["AccountReconcileViewCriteria"] != null)
                    {
                        AccountReconcileViewCriteria = dsPreview.Tables["AccountReconcileViewCriteria"];

                        // Non-Advance option
                        if (AccountReconcileView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in AccountReconcileViewCriteria.Rows)
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
                            whereClause = AccountReconcileView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in AccountReconcileViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) AccountReconcileView.Rows[0]["IsAll"])
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