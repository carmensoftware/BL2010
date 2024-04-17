using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL.ImpGL
{
    public class ImpGLViewCriteria : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public ImpGLViewCriteria()
        {
            SelectCommand = "SELECT * FROM GL.ImpGLViewCriteria";
            TableName = "ImpGLViewCriteria";
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetImpGLViewCriteriaList(int impGLViewID, string connStr)
        {
            var impGLViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get data
            impGLViewCriteria = DbRead("GL.GetImpGLViewCriteriaListByImpGLViewID", dbParams, connStr);

            //Return result
            return impGLViewCriteria;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="dsImpGLViewCriteria"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLViewCriteriaList(int impGLViewID, DataSet dsImpGLViewCriteria, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get data
            result = DbRetrieve("GL.GetImpGLViewCriteriaListByImpGLViewID", dsImpGLViewCriteria, dbParams, TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewCriteria"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLViewCriteriaSchema(DataSet impGLViewCriteria, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetImpGLViewCriteriaList", impGLViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetImpGLViewCriteria(int impGLViewID, int userID, string connStr)
        {
            var impGLView = new DataTable();
            var impGLViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];
            var whereClause = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            impGLView = new ImpGLView().GetImpGLView(impGLViewID, connStr);

            if (impGLView != null)
            {
                // Get journalvoucher view criteria data
                impGLViewCriteria = new ImpGLViewCriteria().GetImpGLViewCriteriaList(impGLViewID, connStr);

                // Non-Advance option
                if (impGLView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in impGLViewCriteria.Rows)
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
                    whereClause = impGLView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in impGLViewCriteria.Rows)
                    {
                        var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                              dr["Operator"]
                                              + " " + "@" + dr["SeqNo"] +
                                              field.GetFieldName(dr["FieldID"].ToString(), connStr);
                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) impGLView.Rows[0]["IsAll"])
                {
                    whereClause += (whereClause != string.Empty ? " AND " : string.Empty) + " CreatedBy = " + userID;
                }
            }

            // Return result
            return whereClause;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetImpGLViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var impGLView = new DataTable();
            var impGLViewCriteria = new DataTable();
            var dbParams = new DbParameter[1];

            var whereClause = string.Empty;

            int impGLViewID;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["ImpGLView"] != null)
            {
                impGLView = dsPreview.Tables["ImpGLView"];
                impGLViewID = int.Parse(impGLView.Rows[0]["ImpGLViewID"].ToString());

                if (impGLView != null)
                {
                    // Get impGL view criteria data
                    if (dsPreview.Tables["ImpGLViewCriteria"] != null)
                    {
                        impGLViewCriteria = dsPreview.Tables["ImpGLViewCriteria"];

                        // Non-Advance option
                        if (impGLView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in impGLViewCriteria.Rows)
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
                            whereClause = impGLView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in impGLViewCriteria.Rows)
                            {
                                var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                      dr["Operator"]
                                                      + " " + "@" + dr["SeqNo"] +
                                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view
                        if (!(bool) impGLView.Rows[0]["IsAll"])
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
    }
}