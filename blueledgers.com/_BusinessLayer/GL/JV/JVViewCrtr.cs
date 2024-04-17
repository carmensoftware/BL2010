using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.JV
{
    public class JVViewCrtr : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public JVViewCrtr()
        {
            SelectCommand = "SELECT * FROM GL.JVViewCrtr";
            TableName = "JVViewCrtr";
        }

        /// <summary>
        ///     Get table structure
        /// </summary>
        /// <param name="dsJVViewCrtr"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsJVViewCrtr, string strConn)
        {
            return DbRetrieveSchema("GL.GetJVViewCrtrList", dsJVViewCrtr, null, TableName, strConn);
        }

        /// <summary>
        ///     Get journal voucher view criteria list related to specified view id.
        /// </summary>
        /// <param name="dsJVViewCrtr"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsJVViewCrtr, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetJVViewCrtrList_ViewID", dsJVViewCrtr, dbParams, TableName, strConn);
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
            var dsJVViewCrtr = new DataSet();
            var result = GetList(dsJVViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsJVViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE ");

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsJVViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drJVViewCrtr = dsJVViewCrtr.Tables[TableName].Rows[i];

                        if (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsJVViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drJVViewCrtr = dsJVViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Append("[" + drJVViewCrtr["FieldName"] + "] " + drJVViewCrtr["Operator"] +
                                          (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                                           drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                              ? " @" + drJVViewCrtr["SeqNo"]
                                              : " ") +
                                          (drJVViewCrtr["LogicalOp"].ToString() != string.Empty
                                              ? " " + drJVViewCrtr["LogicalOp"] + " "
                                              : " "));

                        // Add parameter
                        if (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drJVViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drJVViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drJVViewCrtr["SeqNo"],
                                    "%" + drJVViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drJVViewCrtr["SeqNo"],
                                    drJVViewCrtr["Value"].ToString());
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
            var dsJVViewCrtr = new DataSet();
            var result = GetList(dsJVViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsJVViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE " + srtAdvOpt);

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsJVViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drJVViewCrtr = dsJVViewCrtr.Tables[TableName].Rows[i];

                        if (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsJVViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drJVViewCrtr = dsJVViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Replace(drJVViewCrtr["SeqNo"].ToString(),
                            "[" + drJVViewCrtr["FieldName"] + "] " + drJVViewCrtr["Operator"] +
                            (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                             drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                ? " @" + drJVViewCrtr["SeqNo"]
                                : " "));

                        // Add parameter
                        if (drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drJVViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drJVViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drJVViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drJVViewCrtr["SeqNo"],
                                    "%" + drJVViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drJVViewCrtr["SeqNo"],
                                    drJVViewCrtr["Value"].ToString());
                            }
                        }
                    }

                    return sbCrtrList.ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        #endregion
    }
}