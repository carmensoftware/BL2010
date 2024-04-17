using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvViewCrtr : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public StdJvViewCrtr()
        {
            SelectCommand = "SELECT * FROM GL.StdJvViewCrtr";
            TableName = "StdJvViewCrtr";
        }

        /// <summary>
        ///     Get standard voucher view criteria list related with specified view id.
        /// </summary>
        /// <param name="dsStdJvViewCrtr"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStdJvViewCrtr, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetStdJvViewCrtrList_ViewID", dsStdJvViewCrtr, dbParams, TableName, strConn);
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
            var dsStdJvViewCrtr = new DataSet();
            var result = GetList(dsStdJvViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsStdJvViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE ");

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsStdJvViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drStdJvViewCrtr = dsStdJvViewCrtr.Tables[TableName].Rows[i];

                        if (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsStdJvViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drStdJvViewCrtr = dsStdJvViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Append("[" + drStdJvViewCrtr["FieldName"] + "] " + drStdJvViewCrtr["Operator"] +
                                          (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                                           drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                              ? " @" + drStdJvViewCrtr["SeqNo"]
                                              : " ") +
                                          (drStdJvViewCrtr["LogicalOp"].ToString() != string.Empty
                                              ? " " + drStdJvViewCrtr["LogicalOp"] + " "
                                              : " "));

                        // Add parameter
                        if (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drStdJvViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drStdJvViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drStdJvViewCrtr["SeqNo"],
                                    "%" + drStdJvViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drStdJvViewCrtr["SeqNo"],
                                    drStdJvViewCrtr["Value"].ToString());
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
            var dsStdJvViewCrtr = new DataSet();
            var result = GetList(dsStdJvViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsStdJvViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE " + srtAdvOpt);

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsStdJvViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drStdJvViewCrtr = dsStdJvViewCrtr.Tables[TableName].Rows[i];

                        if (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsStdJvViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drStdJvViewCrtr = dsStdJvViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Replace(drStdJvViewCrtr["SeqNo"].ToString(),
                            "[" + drStdJvViewCrtr["FieldName"] + "] " + drStdJvViewCrtr["Operator"] +
                            (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                             drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                ? " @" + drStdJvViewCrtr["SeqNo"]
                                : " "));

                        // Add parameter
                        if (drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drStdJvViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drStdJvViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drStdJvViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drStdJvViewCrtr["SeqNo"],
                                    "%" + drStdJvViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drStdJvViewCrtr["SeqNo"],
                                    drStdJvViewCrtr["Value"].ToString());
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
        ///     Get criteria table structure
        /// </summary>
        /// <param name="dsJVViewCrtr"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJVViewCrtr, string strConn)
        {
            return DbRetrieveSchema("GL.GetStdJvViewCrtrList", dsJVViewCrtr, null, TableName, strConn);
        }

        #endregion
    }
}