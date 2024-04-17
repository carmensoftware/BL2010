using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class VendorView : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public VendorView()
        {
            SelectCommand = "SELECT * FROM AP.VendorView";
            TableName = "VendorView";
        }

        /// <summary>
        ///     Get vendor view schema
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorViewSchema(DataSet vendorViewID, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetVendorViewList", vendorViewID, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get vendor  view using vendor  view id
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="vendorVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorView(int vendorViewID, DataSet vendorVendor, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var result = DbRetrieve("AP.GetVendorView", vendorVendor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get all vendor  view depend on user id.
        /// </summary>
        /// <returns></returns>
        public DataTable GetVendorViewList(int userID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@CreatedBy", userID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var dtVendorView = DbRead("AP.GetVendorViewListByCreatedBy", dbParams, connStr);

            // Return result
            return dtVendorView;
        }

        /// <summary>
        ///     Generate query of assigned VendorViewID
        /// </summary>
        /// <returns></returns>
        public string GetVendorViewQuery(int vendorViewID, int userID, string connStr)
        {
            // Generate columns
            var columnList = VendorViewColumn.GetVendorViewColumn(vendorViewID, connStr);

            // Generate where clause
            var whereClause = VendorViewCriteria.GetVendorViewCriteria(vendorViewID, userID, connStr);

            // Generate query
            var vendorViewQuery = string.Format("SELECT {0}{1} VendorCode FROM AP.vVendor {2}", 
                columnList, (columnList != string.Empty ? " , " : string.Empty), 
                (whereClause != string.Empty 
                ? string.Format(" WHERE {0}", whereClause) 
                : string.Empty));

            return vendorViewQuery;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetVendorViewQueryPreview(DataSet dsPreview, int userID, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;

            // Generate columns
            if (dsPreview.Tables["VendorViewColumn"] != null)
            {
                columnList = VendorViewColumn.GetVendorViewColumnPreview(dsPreview.Tables["VendorViewColumn"], connStr);
            }

            // Generate where clause
            if (dsPreview.Tables["VendorViewCriteria"] != null)
            {
                whereClause = VendorViewCriteria.GetVendorViewCriteriaPreview(dsPreview, userID, connStr);
            }

            // Generate query
            var vendorViewQuery = string.Format("SELECT {0}{1} VendorCode FROM AP.vVendor {2}",
                columnList, (columnList != string.Empty ? " , " : string.Empty),
                (whereClause != string.Empty 
                ? string.Format(" WHERE {0}", whereClause) 
                : string.Empty));

            return vendorViewQuery;
        }


        /// <summary>
        ///     Validate query before save
        /// </summary>
        /// <returns></returns>
        public bool GetVendorViewIsValidQuery(DataTable vendorView, DataTable vendorViewCriteria,
            DataTable vendorViewColumn, string connStr)
        {
            var columnList = string.Empty;
            var whereClause = string.Empty;
            var dbParams = new DbParameter[vendorViewCriteria.DefaultView.Count];
            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (vendorViewColumn != null)
            {
                foreach (DataRow dr in vendorViewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += string.Format("{0}{1}", 
                            (columnList != string.Empty ? "," : string.Empty), 
                            field.GetFieldName(dr["FieldID"].ToString(), connStr));
                    }
                }
            }

            // Generate Criteria
            // Non-Advance option
            if (vendorView.Rows[0]["IsAdvance"].ToString() != "True")
            {
                foreach (DataRow dr in vendorViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        whereClause += string.Format("{0}{1} {2} @{3}{1} {4}", 
                            (whereClause != string.Empty ? " " : string.Empty), 
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"], dr["LogicalOp"]);
                    }
                }
            }
                // Advance option
            else
            {
                whereClause = vendorView.Rows[0]["AvanceOption"].ToString();

                foreach (DataRow dr in vendorViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        var eachWhereClause = string.Format("{0} {1} @{2}{0}", 
                            field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Operator"], dr["SeqNo"]);

                        whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                    }
                }
            }

            // Generate parameter
            if (vendorViewCriteria.Rows.Count > 0)
            {
                foreach (DataRow dr in vendorViewCriteria.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dbParams[(int) dr["SeqNo"] - 1] =new DbParameter(string.Format("@{0}{1}",
                            dr["SeqNo"], field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                                dr["Value"].ToString());
                    }
                }
            }
            else
            {
                dbParams = null;
            }

            // Generate query
            var vendorViewQuery = string.Format("SELECT {0}{1} VendorCode FROM AP.vVendor{2}",
                columnList, (columnList != string.Empty ? " , " : string.Empty),
                (whereClause != string.Empty 
                ? string.Format(" WHERE {0}", whereClause) 
                : string.Empty));

            // Vlidate query
            var result = DbExecuteQuery(vendorViewQuery, dbParams, connStr);

            // Return result
            return result != null;
        }

        /// <summary>
        ///     Get vendor  view using vendor  view id
        /// </summary>
        /// <param name="vendorViewID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorView(int vendorViewID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create Parameter
            dbParams[0] = new DbParameter("@VendorViewID", vendorViewID.ToString(CultureInfo.InvariantCulture));

            // Get data
            var vendorView = DbRead("AP.GetVendorView", dbParams, connStr);

            // Return result
            return vendorView;
        }

        /// <summary>
        ///     Get lastest vendor  view id
        /// </summary>
        /// <returns></returns>
        public int GetVendorViewMaxID(string connStr)
        {
            // Get data
            var maxAccountViewID = DbReadScalar("AP.GetVendorViewMaxID", null, connStr);

            // Return result
            return maxAccountViewID;
        }

        /// <summary>
        ///     Commit to data base for save
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var vendorViewCriteria = new VendorViewCriteria();
            var vendorViewColumn = new VendorViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(savedData, vendorViewCriteria.SelectCommand, vendorViewCriteria.TableName);
            dbSaveSource[2] = new DbSaveSource(savedData, vendorViewColumn.SelectCommand, vendorViewColumn.TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit to data base for delete
        /// </summary>
        /// <param name="deletedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet deletedData, string connStr)
        {
            var vendorViewCriteria = new VendorViewCriteria();
            var vendorViewColumn = new VendorViewColumn();
            var dbSaveSource = new DbSaveSource[3];

            // Create SaveSource object
            dbSaveSource[0] = new DbSaveSource(deletedData, vendorViewCriteria.SelectCommand,vendorViewCriteria.TableName);
            dbSaveSource[1] = new DbSaveSource(deletedData, vendorViewColumn.SelectCommand, vendorViewColumn.TableName);
            dbSaveSource[2] = new DbSaveSource(deletedData, SelectCommand, TableName);

            // Commit to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}