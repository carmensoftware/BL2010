using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.Account
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
            SelectCommand = "SELECT * FROM GL.AccountViewCrtr";
            TableName = "AccountViewCrtr";
        }

        /// <summary>
        ///     Get table structure
        /// </summary>
        /// <param name="dsAccViewCrtr"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccViewCrtr, string strConn)
        {
            return DbRetrieveSchema("GL.GetAccViewCrtrList", dsAccViewCrtr, null, TableName, strConn);
        }

        /// <summary>
        ///     Get account view criteria list related to specified view id.
        /// </summary>
        /// <param name="dsAccViewCrtr"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccViewCrtr, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetAccViewCrtrList_ViewID", dsAccViewCrtr, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get criteria query of specified view id
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetCriteria(int intViewID, ref DbParameter[] dbParams, string strConn)
        {
            var sbCrtrList = new StringBuilder();

            // Add view criteria list
            var dsAccViewCrtr = new DataSet();
            var result = GetList(dsAccViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsAccViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE ");

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsAccViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drAccViewCrtr = dsAccViewCrtr.Tables[TableName].Rows[i];

                        if (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsAccViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drAccViewCrtr = dsAccViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Append("[" + drAccViewCrtr["FieldName"] + "] " + drAccViewCrtr["Operator"] +
                                          (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                                           drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                              ? " @" + drAccViewCrtr["SeqNo"]
                                              : " ") +
                                          (drAccViewCrtr["LogicalOp"].ToString() != string.Empty
                                              ? " " + drAccViewCrtr["LogicalOp"] + " "
                                              : " "));

                        // Add parameter
                        if (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drAccViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drAccViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drAccViewCrtr["SeqNo"],
                                    "%" + drAccViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drAccViewCrtr["SeqNo"],
                                    drAccViewCrtr["Value"].ToString());
                            }
                        }
                    }

                    return sbCrtrList.ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get criteria query of specified view id in advance option.
        /// </summary>
        /// <param name="intViewID"></param>
        /// <param name="srtAdvOpt"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetAdvanceCriteria(int intViewID, ref DbParameter[] dbParams, string srtAdvOpt, string strConn)
        {
            var sbCrtrList = new StringBuilder();

            // Add view criteria list
            var dsAccViewCrtr = new DataSet();
            var result = GetList(dsAccViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsAccViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE " + srtAdvOpt);

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsAccViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drAccViewCrtr = dsAccViewCrtr.Tables[TableName].Rows[i];

                        if (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsAccViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drAccViewCrtr = dsAccViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Replace(drAccViewCrtr["SeqNo"].ToString(),
                            "[" + drAccViewCrtr["FieldName"] + "] " + drAccViewCrtr["Operator"] +
                            (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                             drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                ? " @" + drAccViewCrtr["SeqNo"]
                                : " "));

                        // Add parameter
                        if (drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drAccViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drAccViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drAccViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drAccViewCrtr["SeqNo"],
                                    "%" + drAccViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drAccViewCrtr["SeqNo"],
                                    drAccViewCrtr["Value"].ToString());
                            }
                        }
                    }

                    return sbCrtrList.ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get account view criteria using account view id
        /// </summary>
        /// <returns></returns>
        //public bool GetAccountViewCriteriaList(int accountViewID, DataSet accountViewCriteria, string connStr)
        //{
        //    bool result            = false;
        //    DbParameter[] dbParams = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

        //    // Get data
        //    result = DbRetrieve("Reference.GetAccountViewCriteriaListByAccountViewID", accountViewCriteria, dbParams, this.TableName, connStr);

        //    //Return result
        //    return result;
        //}

        /// <summary>
        /// Get account view criteria using account view id
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAccountViewCriteriaList(int accountViewID, string connStr)
        //{
        //    DataTable accountViewCriteria = new DataTable();
        //    DbParameter[] dbParams        = new DbParameter[1];

        //    // Create parameter
        //    dbParams[0] = new DbParameter("@AccountViewID", accountViewID.ToString());

        //    // Get data
        //    accountViewCriteria = DbRead("Reference.GetAccountViewCriteriaListByAccountViewID", dbParams, connStr);

        //    //Return result
        //    return accountViewCriteria;
        //}

        /// <summary>
        /// Generate where clause using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        //public static string GetAccountViewCriteria(int accountViewID, int userID, string connStr)
        //{ 
        //    DataTable accountView                 = new DataTable();
        //    DataTable accountViewCriteria         = new DataTable();
        //    DbParameter[] dbParams                = new DbParameter[1];
        //    string whereClause                    = string.Empty;
        //    BlueLedger.BL.Application.Field field = new BlueLedger.BL.Application.Field();

        //    // Checking advance option
        //    accountView = new AccountView().GetAccountView(accountViewID, connStr);

        //    if (accountView != null)
        //    {
        //        // Get account view criteria data

        //        accountViewCriteria = new AccountViewCriteria().GetAccountViewCriteriaList(accountViewID, connStr);

        //        // Non-Advance option
        //        if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
        //        {
        //            foreach (DataRow dr in accountViewCriteria.Rows)
        //            {
        //                whereClause += (whereClause != string.Empty ? " " : string.Empty) +
        //                    field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] + " " +
        //                    "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["LogicalOp"];
        //            }
        //        }
        //        // Advance option
        //        else
        //        {
        //            whereClause = accountView.Rows[0]["AdvanceOption"].ToString();

        //            foreach (DataRow dr in accountViewCriteria.Rows)
        //            {
        //                string eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] + " " + "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr);
        //                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
        //            }
        //        }

        //        // Add display only own view
        //        if (!(bool)accountView.Rows[0]["IsAll"])
        //        {
        //            whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID.ToString();
        //        }
        //    }

        //    // Return result
        //    return whereClause;
        //}

        /// <summary>
        /// Get account view criteria schema
        /// </summary>
        /// <param name="accountViewCriteria"></param>
        /// <returns></returns>
        //public bool GetAccountViewCriteriaSchema(DataSet accountViewCriteria, string connStr)
        //{
        //    bool result = false;

        //    // Get data
        //    result = DbRetrieveSchema("Reference.GetAccountViewCriteriaList", accountViewCriteria, null, this.TableName, connStr);

        //    // Return result
        //    return result;
        //}

        /// <summary>
        /// Generate where clause using datatable.
        /// </summary>
        /// <param name="dtPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public static string GetAccountViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        //{
        //    DataTable accountView         = new DataTable();
        //    DataTable accountViewCriteria = new DataTable();
        //    DbParameter[] dbParams        = new DbParameter[1];

        //    string whereClause = string.Empty;

        //    int accountViewID;

        //    BL.Application.Field field = new BL.Application.Field();

        //    // Checking advance option
        //    if (dsPreview.Tables["AccountView"] != null)
        //    {
        //        accountView   = dsPreview.Tables["AccountView"];
        //        accountViewID = int.Parse(accountView.Rows[0]["AccountViewID"].ToString());

        //        if (accountView != null)
        //        {
        //            // Get account view criteria data
        //            if (dsPreview.Tables["AccountViewCriteria"] != null)
        //            {
        //                accountViewCriteria = dsPreview.Tables["AccountViewCriteria"];

        //                // Non-Advance option
        //                if (accountView.Rows[0]["IsAdvance"].ToString() != "True")
        //                {
        //                    foreach (DataRow dr in accountViewCriteria.Rows)
        //                    {
        //                        whereClause += (whereClause != string.Empty ? " " : string.Empty) +

        //                        field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] + " " +

        //                        "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["LogicalOp"];
        //                    }
        //                }
        //                // Advance option
        //                else
        //                {
        //                    whereClause = accountView.Rows[0]["AvanceOption"].ToString();

        //                    foreach (DataRow dr in accountViewCriteria.Rows)
        //                    {
        //                        string eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"]

        //                        + " " + "@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr);

        //                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
        //                    }
        //                }

        //                // Add display only own view
        //                if (!(bool)accountView.Rows[0]["IsAll"])
        //                {
        //                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID.ToString();
        //                }
        //            }
        //        }
        //    }

        //    // Return result
        //    return whereClause;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public DataTable GetCriteriaColumns(string connStr)
        //{
        //    return DbRead("reference.GetAccountViewCriteriaAvailableColumns", null, connStr);
        //}

        #endregion
    }
}