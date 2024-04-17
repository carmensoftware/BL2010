using System.Data;
using System.Text;
using Blue.DAL;

namespace Blue.BL.GL.Rec
{
    public class RecViewCrtr : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public RecViewCrtr()
        {
            SelectCommand = "SELECT * FROM GL.RecViewCrtr";
            TableName = "RecViewCrtr";
        }

        /// <summary>
        ///     Get table structure
        /// </summary>
        /// <param name="dsJVViewCrtr"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsViewCrtr, string strConn)
        {
            return DbRetrieveSchema("GL.GetRecViewCrtrList", dsViewCrtr, null, TableName, strConn);
        }

        /// <summary>
        ///     Get journal voucher view criteria list related to specified view id.
        /// </summary>
        /// <param name="dsJVViewCrtr"></param>
        /// <param name="intViewID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsViewCrtr, int intViewID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewID", intViewID.ToString());
            return DbRetrieve("GL.GetRecViewCrtrList_ViewID", dsViewCrtr, dbParams, TableName, strConn);
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
            var dsViewCrtr = new DataSet();
            var result = GetList(dsViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE ");

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drViewCrtr = dsViewCrtr.Tables[TableName].Rows[i];

                        if (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drViewCrtr = dsViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Append("[" + drViewCrtr["FieldName"] + "] " + drViewCrtr["Operator"] +
                                          (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                                           drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                              ? " @" + drViewCrtr["SeqNo"]
                                              : " ") +
                                          (drViewCrtr["LogicalOp"].ToString() != string.Empty
                                              ? " " + drViewCrtr["LogicalOp"] + " "
                                              : " "));

                        // Add parameter
                        if (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drViewCrtr["SeqNo"], "%" + drViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drViewCrtr["SeqNo"], drViewCrtr["Value"].ToString());
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
            var dsViewCrtr = new DataSet();
            var result = GetList(dsViewCrtr, intViewID, strConn);

            if (result)
            {
                if (dsViewCrtr.Tables[TableName].Rows.Count > 0)
                {
                    // Insert where clause to criteria query.
                    sbCrtrList.Append(" WHERE " + srtAdvOpt);

                    // Find number of parameters. The number is must not include criteria which have operator "IS NULL" and "IS NOT NULL"
                    var intParamsNo = 0;

                    for (var i = 0; i < dsViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drViewCrtr = dsViewCrtr.Tables[TableName].Rows[i];

                        if (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            intParamsNo++;
                        }
                    }

                    dbParams = new DbParameter[intParamsNo];

                    for (var i = 0; i < dsViewCrtr.Tables[TableName].Rows.Count; i++)
                    {
                        var drViewCrtr = dsViewCrtr.Tables[TableName].Rows[i];

                        // Add criterias
                        sbCrtrList.Replace(drViewCrtr["SeqNo"].ToString(),
                            "[" + drViewCrtr["FieldName"] + "] " + drViewCrtr["Operator"] +
                            (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                             drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL"
                                ? " @" + drViewCrtr["SeqNo"]
                                : " "));

                        // Add parameter
                        if (drViewCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                            drViewCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                        {
                            if (drViewCrtr["Operator"].ToString().ToUpper() == "LIKE" ||
                                drViewCrtr["Operator"].ToString().ToUpper() == "NOT LIKE")
                            {
                                dbParams[i] = new DbParameter("@" + drViewCrtr["SeqNo"], "%" + drViewCrtr["Value"] + "%");
                            }
                            else
                            {
                                dbParams[i] = new DbParameter("@" + drViewCrtr["SeqNo"], drViewCrtr["Value"].ToString());
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