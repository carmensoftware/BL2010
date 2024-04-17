using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.TrBal
{
    public class TrBalViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public TrBalViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.TrBalViewCrtr";
            TableName = "TrBalViewCrtr";
        }

        /// <summary>
        ///     Get table structure
        /// </summary>
        /// <param name="dsAccViewCrtr"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsTrBalViewCrtr, string strConn)
        {
            return DbRetrieveSchema("[GL].[GetTrBalViewCrtrList]", dsTrBalViewCrtr, null, TableName, strConn);
        }

        /// <summary>
        ///     Get trial balance view criteria list related to specified view id.
        /// </summary>
        /// <param name="dsTrBalViewCrtr"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTrBalViewCrtr, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("[GL].[GetTrBalViewCrtrList_ViewID]", dsTrBalViewCrtr, dbParams, TableName, strConn);
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
            var dsTrBalViewCrtr = new DataSet();
            var result = GetList(dsTrBalViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsTrBalViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE ");

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsTrBalViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drTrBalViewCrtr = dsTrBalViewCrtr.Tables[TableName].Rows[i];

                        if (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsTrBalViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drTrBalViewCrtr = dsTrBalViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Append("[" + drTrBalViewCrtr["FieldName"] + "] " + drTrBalViewCrtr["Operator"] +
                                          (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                                           drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                              ? " @" + drTrBalViewCrtr["SeqNo"]
                                              : " ") +
                                          (drTrBalViewCrtr["LogicalOp"].ToString() != string.Empty
                                              ? " " + drTrBalViewCrtr["LogicalOp"] + " "
                                              : " "));

                        // Add parameter
                        if (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drTrBalViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drTrBalViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drTrBalViewCrtr["SeqNo"],
                                    "%" + drTrBalViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drTrBalViewCrtr["SeqNo"],
                                    drTrBalViewCrtr["Value"].ToString());
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
            var dsTrBalViewCrtr = new DataSet();
            var result = GetList(dsTrBalViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsTrBalViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE " + srtAdvOpt);

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsTrBalViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drTrBalViewCrtr = dsTrBalViewCrtr.Tables[TableName].Rows[i];

                        if (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsTrBalViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drTrBalViewCrtr = dsTrBalViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Replace(drTrBalViewCrtr["SeqNo"].ToString(),
                            "[" + drTrBalViewCrtr["FieldName"] + "] " + drTrBalViewCrtr["Operator"] +
                            (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                             drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                ? " @" + drTrBalViewCrtr["SeqNo"]
                                : " "));

                        // Add parameter
                        if (drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drTrBalViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drTrBalViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drTrBalViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drTrBalViewCrtr["SeqNo"],
                                    "%" + drTrBalViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drTrBalViewCrtr["SeqNo"],
                                    drTrBalViewCrtr["Value"].ToString());
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