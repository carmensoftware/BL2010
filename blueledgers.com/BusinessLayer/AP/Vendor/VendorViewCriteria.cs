using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.AP
{
    public class VendorViewCriteria : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorViewCriteria()
        {
            SelectCommand = "SELECT * FROM AP.VendorViewCriteria";
            TableName = "VendorViewCriteria";
        }

        /// <summary>
        ///     Get vendor voucher view criteria using vendor voucher view id
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendorViewCriteriaList(int vendorViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            DataTable vendorViewCriteria = DbRead("AP.GetVendorViewCriteriaListByVendorViewID", dbParams, connStr);

            //Return result
            return vendorViewCriteria;
        }

        /// <summary>
        ///     Generate where clause using vendorViewID
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetVendorViewCriteria(int vendorViewID, int userID, string connStr)
        {
            var whereClause = string.Empty;
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            DataTable vendorView = new VendorView().GetVendorView(vendorViewID, connStr);

            if (vendorView.Rows.Count > 0)
            {
                // Get vendor voucher view criteria data
                DataTable vendorViewCriteria = new VendorViewCriteria().GetVendorViewCriteriaList(vendorViewID, connStr);

                // Non-Advance option
                if (vendorView.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in vendorViewCriteria.Rows)
                    {
                        whereClause += string.Format("{0}{1} {2} @{3}{1} {4}",
                            (whereClause != string.Empty ? " " : string.Empty),
                            field.GetFieldName(dr["FieldID"].ToString(), connStr), 
                            dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                    }
                }
                    // Advance option
                else
                {
                    whereClause = vendorView.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in vendorViewCriteria.Rows)
                    {
                        var eachWhereClause = string.Format("{0} {1} @{2}{0}",
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"]);

                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }

                // Add display only own view
                if (!(bool) vendorView.Rows[0]["IsAll"])
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
        public static string GetVendorViewCriteriaPreview(DataSet dsPreview, int userID, string connStr)
        {
            var whereClause = string.Empty;

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Checking advance option
            if (dsPreview.Tables["VendorView"] != null)
            {
                DataTable vendorView = dsPreview.Tables["VendorView"];

                if (vendorView != null)
                {
                    // Get vendor view criteria data

                    if (dsPreview.Tables["VendorViewCriteria"] != null)
                    {
                        DataTable vendorViewCriteria = dsPreview.Tables["VendorViewCriteria"];

                        // Non-Advance option
                        if (vendorView.Rows[0]["IsAdvance"].ToString() != "True")
                        {
                            foreach (DataRow dr in vendorViewCriteria.Rows)
                            {
                                whereClause += string.Format("{0}{1} {2} @{3}{1} {4}", 
                                    (whereClause != string.Empty ? " " : string.Empty),
                                    field.GetFieldName(dr["FieldID"].ToString(), connStr), 
                                    dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                            }
                        }

                            //Advance option

                        else
                        {
                            whereClause = vendorView.Rows[0]["AvanceOption"].ToString();

                            foreach (DataRow dr in vendorViewCriteria.Rows)
                            {
                                var eachWhereClause = string.Format("{0} {1} @{2}{0}", 
                                    field.GetFieldName(dr["FieldID"].ToString(), connStr),
                                    dr["Operator"], dr["SeqNo"]);

                                whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                            }
                        }

                        // Add display only own view

                        if (!(bool) vendorView.Rows[0]["IsAll"])
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
        ///     Get vendor view criteria using vendor view id
        /// </summary>
        /// <returns></returns>
        public bool GetVendorViewCriteriaList(int vendorViewID, DataSet vendorViewCriteria, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            bool result = DbRetrieve("AP.GetVendorViewCriteriaListByVendorViewID", vendorViewCriteria, dbParams, TableName,
                connStr);

            //Return result
            return result;
        }

        /// <summary>
        ///     Get vendor voucher view criteria schema
        /// </summary>
        /// <param name="vendorViewCriteria"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorViewCriteriaSchema(DataSet vendorViewCriteria, string connStr)
        {
            // Get data
            bool result = DbRetrieveSchema("AP.GetVendorViewCriteriaList", vendorViewCriteria, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}