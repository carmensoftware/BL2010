using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class AccountViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public AccountViewCriteria()
        {
            this.SelectCommand = "SELECT * FROM Reference.AccountViewCriteria";
            this.TableName = "AccountViewCriteria";
        }

        /// <summary>
        ///     Get account view criteria using account view id
        /// </summary>
        /// <returns></returns>
        public bool GetAccountViewCriteriaList(int accountViewID, DataSet accountViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get data
            result = DbRetrieve("Reference.GetAccountViewCriteriaListByAccountViewID", accountViewCriteria, dbParams,
                this.TableName, connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get account view criteria using account view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccountViewCriteriaList(int accountViewID, string connStr)
        {
            var accountViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

            // Get data
            accountViewCriteria = DbRead("Reference.GetAccountViewCriteriaListByAccountViewID", dbParams, connStr);

            //Return result
            return accountViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetAccountViewCriteria(int accountViewID, int userID, string connStr)
        {
            var accountView = new DataTable();
            var accountViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            accountView = new AccountView().GetAccountView(accountViewID, connStr);

            if (accountView != null)
            {
                // Get account view criteria data

                accountViewCriteria = new AccountViewCriteria().GetAccountViewCriteriaList(accountViewID, connStr);

                // Non-Advance option
                if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in accountViewCriteria.Rows)
                    {
                        whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                       field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                       " " +
                                       "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                       dr["LogicalOp"];
                    }
                }
                    // Advance option
                else
                {
                    whereClause = accountView.Rows[0]["AdvanceOption"].ToString();

                    foreach (DataRow dr in accountViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"] + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) accountView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }

        /// <summary>
        ///     Get account view criteria schema
        /// </summary>
        /// <param name="accountViewCriteria"></param>
        /// <returns></returns>
        public bool GetAccountViewCriteriaSchema(DataSet accountViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("Reference.GetAccountViewCriteriaList", accountViewCriteria, null, this.TableName,
                connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate where clause using datatable.
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetAccountViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var accountView = new DataTable();
            var accountViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int accountViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["AccountView"] != null)
            {
                accountView = dsPreview.Tables["AccountView"];
                accountViewID = int.Parse(accountView.Rows[0]["AccountViewID"].ToString());

                if (accountView != null)
                {
                    // Get account view criteria data
                    if (dsPreview.Tables["AccountViewCriteria"] != null)
                    {
                        accountViewCriteria = dsPreview.Tables["AccountViewCriteria"];

                        // Non-Advance option
                        if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in accountViewCriteria.Rows)
                            {
                                whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                               field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                               dr["Operator"] + " " +
                                               "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) +
                                               " " + dr["LogicalOp"];
                            }
                        }
                            // Advance option
                        else
                        {
                            whereClause = accountView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in accountViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) accountView.Rows[0]["IsAll"])
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

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetCriteriaColumns(string connStr)
        {
            return DbRead("reference.GetAccountViewCriteriaAvailableColumns", null, connStr);
        }

        #endregion
    }
}