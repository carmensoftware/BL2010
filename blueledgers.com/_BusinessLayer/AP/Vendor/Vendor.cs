using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.AP
{
    public class Vendor : DbHandler
    {
        #region "Attributies"

        private readonly Profile.Address _address = new Profile.Address();
        private readonly dbo.Bu _bu = new dbo.Bu();
        private readonly Profile.Contact _contact = new Profile.Contact();
        private readonly Profile.ContactDetail _contactDetail = new Profile.ContactDetail();
        private readonly Profile.Profile _profile = new Profile.Profile();

        /*
                private Profile.BankAccount _bankAccount = new Profile.BankAccount();
                private GnxLib _gnxLib = new GnxLib();
                private VendorCategory _vendorCategory = new VendorCategory();
                private VendorDefaultWHT _vendorDefaultWHT = new VendorDefaultWHT();
                private VendorMisc _vendorMisc = new VendorMisc();
        
        */

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Vendor()
        {
            SelectCommand = "SELECT * FROM AP.Vendor";
            TableName = "Vendor";
        }

        /// <summary>
        ///     Get vendor using id
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendor(string vendorCode, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            // Get data
            var dtVendor = DbRead("AP.GetVendorByCode", dbParams, connStr);

            // Return result
            return dtVendor;
        }

        /// <summary>
        ///     Get vendor existing by using vendor name.
        /// </summary>
        /// <param name="vendorName"></param>
        /// <param name="connStr">Connection String of Connecting Database</param>
        /// <param name="vendorCode"></param>
        /// <returns>true and VendorCode && VendorName != empty : found exactly match</returns>
        /// <returns>false and VendorCode != empty && VendorName == empty : found match code</returns>
        /// <returns>false and VendorCode == empty && VendorName != empty : found match name</returns>
        /// <returns>false and VendorCode == empty && VendorName == empty : not found</returns>
        public bool IsExist(ref string vendorCode, ref string vendorName, string connStr)
        {
            // Search by code and name
            var dbParams1 = new DbParameter[2];
            dbParams1[0] = new DbParameter("@VendorCode", vendorCode);
            dbParams1[1] = new DbParameter("@Name", vendorName);
            var dtVendor1 = DbRead("AP.GetVendor_VendorCode_Name", dbParams1, connStr);

            if (dtVendor1.Rows.Count > 0)
            {
                return true;
            }
            // Search by name
            var dbParams2 = new DbParameter[1];
            dbParams2[0] = new DbParameter("@Name", vendorName);
            var dtVendor2 = DbRead("AP.GetVendorByVendorName", dbParams2, connStr);

            if (dtVendor2.Rows.Count > 0)
            {
                vendorCode = string.Empty;
                return false;
            }
            // Search by code
            var dbParams3 = new DbParameter[1];
            dbParams3[0] = new DbParameter("@VendorCode", vendorCode);
            var dtVendor3 = DbRead("AP.GetVendorByCode", dbParams3, connStr);

            if (dtVendor3.Rows.Count > 0)
            {
                vendorName = string.Empty;
                return false;
            }
            // Not found
            vendorCode = string.Empty;
            vendorName = string.Empty;
            return false;

            //// Search by code
            //DbParameter[] dbParams2 = new DbParameter[1];
            //dbParams2[0] = new DbParameter("@VendorCode", VendorCode);
            //DataTable dtVendor2 = DbRead("AP.GetVendor_VendorCode", dbParams2, ConnStr);

            //if (dtVendor2.Rows.Count > 0)
            //{
            //    VendorName = string.Empty;
            //    return false;
            //}
            //else
            //{
            //    // Search by name
            //    DbParameter[] dbParams3 = new DbParameter[1];
            //    dbParams3[0] = new DbParameter("@Name", VendorName);
            //    DataTable dtVendor3 = DbRead("AP.GetVendor_Name", dbParams3, ConnStr);

            //    if (dtVendor3.Rows.Count > 0)
            //    {
            //        VendorCode = string.Empty;
            //        return false;
            //    }
            //    else
            //    {
            //        // Not found
            //        VendorCode = string.Empty;
            //        VendorName = string.Empty;
            //        return false;
            //    }
            //}      
        }

        /// <summary>
        ///     Get vendor using id
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendor(string vendorCode, DataSet dsVendor, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            // Get data
            var result = DbRetrieve("AP.GetVendorByCode", dsVendor, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get vendor name by profileCode
        /// </summary>
        /// <param name="profileCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetVendorName(string profileCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProfileCode", profileCode);

            DbRetrieve("AP.GetVendorNameByProfileCode", dsTmp, dbParams, TableName, connStr);

            var vendorName = dsTmp.Tables[TableName].Rows.Count > 0
                ? dsTmp.Tables[TableName].Rows[0]["VendorName"].ToString()
                : null;
            return vendorName;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetVendorListSearch(string keyWord, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@KeyWord", keyWord);

            return DbRead("AP.GetVendorListSearch", dbParams, connStr);
        }

        /// <summary>
        ///     Get vendor using account view id
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="vendorViewID"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetVendorList(DataSet dsVendor, int vendorViewID, int userID, string connStr)
        {
            DataTable dtVendor;
            var vendor = new Vendor();

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            var vendorViewQuery = new VendorView().GetVendorViewQuery(vendorViewID, userID, connStr);

            // Generate parameter
            var dtVendorViewCriteria = new VendorViewCriteria().GetVendorViewCriteriaList(vendorViewID, connStr);

            if (dtVendorViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtVendorViewCriteria.Rows.Count];

                for (var i = 0; i < dtVendorViewCriteria.Rows.Count; i++)
                {
                    var dr = dtVendorViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"],
                            field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtVendor = vendor.DbExecuteQuery(vendorViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtVendor = vendor.DbExecuteQuery(vendorViewQuery, null, connStr);
            }

            // Return resutl
            if (dsVendor.Tables[TableName] != null)
            {
                dsVendor.Tables.Remove(TableName);
            }

            dtVendor.TableName = TableName;
            dsVendor.Tables.Add(dtVendor);
        }

        /// <summary>
        ///     Get Vendor Preview.
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetVendorPreveiw(DataSet dsVendor, int userID, string connStr)
        {
            var vendorViewCriteria = new VendorViewCriteria();
            DataTable dtVendor;
            var vendor = new Vendor();

            var field = new Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate  query
            var vendorViewQuery = new VendorView().GetVendorViewQueryPreview(dsVendor, userID, connStr);


            // Generate  parameter
            var dtVendorViewCriteria = dsVendor.Tables[vendorViewCriteria.TableName];

            if (dtVendorViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtVendorViewCriteria.Rows.Count];

                for (var i = 0; i < dtVendorViewCriteria.Rows.Count; i++)
                {
                    var dr = dtVendorViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter(string.Format("@{0}{1}", dr["SeqNo"],
                            field.GetFieldName(dr["FieldID"].ToString(), connStr)),
                            dr["Value"].ToString());
                }

                // Get data                
                dtVendor = vendor.DbExecuteQuery(vendorViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtVendor = vendor.DbExecuteQuery(vendorViewQuery, null, connStr);
            }

            // Return result
            if (dsVendor.Tables[TableName] != null)
            {
                dsVendor.Tables.Remove(TableName);
            }

            dtVendor.TableName = TableName;
            dsVendor.Tables.Add(dtVendor);
        }

        /// <summary>
        ///     Get all of vendor except recurring type
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorList(DataSet dsVendor, string connStr)
        {
            // Get data
            var result = DbRetrieve("AP.GetVendorForPopup", dsVendor, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsVendor, string connStr)
        {
            var contact = new Profile.Contact();
            var contactDetail = new Profile.ContactDetail();
            var address = new Profile.Address();
            var profile = new Profile.Profile();

            //var vendorMisc = new VendorMisc();
            //var vendorComment = new VendorComment();
            //var vendorAttachment = new VendorAttachment();
            //var bankAccount = new Profile.BankAccount();
            //var invoiceDefault = new InvoiceDefault();
            //var invoiceDefaultDetail = new InvoiceDefaultDetail();
            //var paymentDefault = new PaymentDefault();
            //var paymentDefaultCash = new PaymentDefaultCash();
            //var paymentDefaultCheq = new PaymentDefaultCheq();
            //var paymentDefaultCredit = new PaymentDefaultCredit();
            //var paymentDefaultAuto = new PaymentDefaultAuto();
            //var paymentDefaultTrans = new PaymentDefaultTrans();
            //var vendorDefaultWHT = new VendorDefaultWHT();

            var dbSaveSorce = new DbSaveSource[5];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsVendor, SelectCommand, TableName);
            dbSaveSorce[1] = new DbSaveSource(dsVendor, profile.SelectCommand, profile.TableName);
            //dbSaveSorce[2] = new DbSaveSource(dsVendor, vendorMisc.SelectCommand, vendorMisc.TableName);
            //dbSaveSorce[3] = new DbSaveSource(dsVendor, bankAccount.SelectCommand, bankAccount.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsVendor, contact.SelectCommand, contact.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsVendor, contactDetail.SelectCommand, contactDetail.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsVendor, address.SelectCommand, address.TableName);
            //dbSaveSorce[7] = new DbSaveSource(dsVendor, invoiceDefaultDetail.SelectCommand, invoiceDefaultDetail.TableName);
            //dbSaveSorce[8] = new DbSaveSource(dsVendor, paymentDefaultCash.SelectCommand, paymentDefaultCash.TableName);
            //dbSaveSorce[9] = new DbSaveSource(dsVendor, paymentDefaultCheq.SelectCommand, paymentDefaultCheq.TableName);
            //dbSaveSorce[10] = new DbSaveSource(dsVendor, paymentDefaultCredit.SelectCommand, paymentDefaultCredit.TableName);
            //dbSaveSorce[11] = new DbSaveSource(dsVendor, paymentDefaultAuto.SelectCommand, paymentDefaultAuto.TableName);
            //dbSaveSorce[12] = new DbSaveSource(dsVendor, paymentDefaultTrans.SelectCommand, paymentDefaultTrans.TableName);
            //dbSaveSorce[13] = new DbSaveSource(dsVendor, vendorDefaultWHT.SelectCommand, vendorDefaultWHT.TableName);
            //dbSaveSorce[14] = new DbSaveSource(dsVendor, vendorAttachment.SelectCommand, vendorAttachment.TableName);
            //dbSaveSorce[15] = new DbSaveSource(dsVendor, paymentDefault.SelectCommand, paymentDefault.TableName);
            //dbSaveSorce[16] = new DbSaveSource(dsVendor, invoiceDefault.SelectCommand, invoiceDefault.TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsVendor, string connStr)
        {
            //bool result                = false;
            //DbSaveSource[] dbSaveSorce = new DbSaveSource[5];

            //// Create dbSaveSource
            //dbSaveSorce[0] = new DbSaveSource(dsVendor, vendorMisc.SelectCommand, vendorMisc.TableName);
            //dbSaveSorce[1] = new DbSaveSource(dsVendor, vendorDefaultWHT.SelectCommand, vendorDefaultWHT.TableName);
            //dbSaveSorce[2] = new DbSaveSource(dsVendor, BankAccount.SelectCommand, BankAccount.TableName);
            //dbSaveSorce[3] = new DbSaveSource(dsVendor, vendorCategory.SelectCommand, vendorCategory.TableName);
            //dbSaveSorce[4] = new DbSaveSource(dsVendor, this.SelectCommand, this.TableName);

            //// Save to database
            //result = DbCommit(dbSaveSorce, connStr);

            //// Return result
            //return result;

            var dbSaveSorce = new DbSaveSource[5];

            // Create dbSaveSource

            dbSaveSorce[0] = new DbSaveSource(dsVendor, _contactDetail.SelectCommand, _contactDetail.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsVendor, _contact.SelectCommand, _contact.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsVendor, _address.SelectCommand, _address.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsVendor, _profile.SelectCommand, _profile.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsVendor, SelectCommand, TableName);


            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get vendor database schema
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetVendorStructure(DataSet dsVendor, string connStr)
        {
            // Get data
            var result = DbRetrieveSchema("AP.GetVendorList", dsVendor, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get max vendor id
        /// </summary>
        /// <returns></returns>
        public int GetVendorMaxID(string connStr)
        {
            // Get data
            var result = DbReadScalar("AP.GetVendorMaxID", null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetNewVendorCode(string connStr)
        {
            var newVendorCode = string.Empty;

            var dtName = DbRead("AP.GetVendorMaxID", null, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    newVendorCode = dtName.Rows[0]["NewVendorCode"].ToString();
                }
            }

            return newVendorCode;
        }

        public string GetNewVendorCodeByName(string vendorName, string connStr)
        {
            var newVendorCode = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorName", vendorName);

            var dtName = DbRead("AP.GetNewVendorCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    newVendorCode = dtName.Rows[0][0].ToString();
                }
            }

            return newVendorCode;
        }

        /// <summary>
        ///     return datatable and get searching match data.
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSearchVendorList(string searchParam, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search_Param", searchParam);

            return DbRead("AP.GetSearchVendorList", dbParams, connStr);
        }

        public DataTable GetVendorGroupByPO(string connStr)
        {
            // Get data
            return DbRead("AP.GetGroupVendorByPO", null, connStr);
        }

        //--------------- CLR Procedure ---------------------
        /// <summary>
        ///     Get name by CLR procedure
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string vendorCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            var dtName = DbRead("AP.GetVendorByCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    strName = dtName.Rows[0]["Name"].ToString();
                }
            }

            return strName;
        }

        public string GetName(string vendorCode, string buCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            var dtName = DbRead("AP.GetVendorByCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    strName = dtName.Rows[0]["Name"].ToString();
                }
            }

            return strName;
        }

        public string GetTaxType(string vendorCode, string connStr)
        {
            var strTaxType = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            var dtName = DbRead("AP.GetVendorByCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    strTaxType = dtName.Rows[0]["TaxType"].ToString();
                }
            }

            return strTaxType;
        }

        public decimal GetTaxRate(string vendorCode, string connStr)
        {
            decimal decTaxRate = 0;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            var dtName = DbRead("AP.GetVendorByCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    decTaxRate = decimal.Parse(dtName.Rows[0]["TaxRate"].ToString());
                }
            }

            return decTaxRate;
        }

        public int GetCreditTerm(string vendorCode, string connStr)
        {
            var intCreditTerm = 0;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", vendorCode);

            //DataTable dtName = DbRead("dbo.AP_Vendor_GetName_Code", dbParams, connStr);
            var dtName = DbRead("AP.GetVendorByCode", dbParams, connStr);

            if (dtName != null)
            {
                if (dtName.Rows.Count > 0)
                {
                    intCreditTerm = int.Parse(dtName.Rows[0]["CreditTerm"].ToString());
                }
            }

            return intCreditTerm;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsVendor, string connStr)
        {
            // Get data
            //result = DbRetrieve("dbo.AP_Vendor_GetList", dsVendor, null, this.TableName, connStr);
            var result = DbRetrieve("AP.GetVendorList", dsVendor, null, TableName, connStr);

            // Return result
            return result;
        }

        public DataTable GetList(string connStr)
        {
            // Get data
            return DbRead("AP.GetVendorList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsVendor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetLookUp(DataSet dsVendor, string connStr)
        {
            // Create parameters
            //DbRetrieve("dbo.AP_Vendor_GetLookup", dsVendor, null, this.TableName, connStr);
            DbRetrieve("AP.GetVendorChkActive", dsVendor, null, TableName, connStr);

            return dsVendor;
        }

        public DataTable GetLookUp(string connStr)
        {
            //return this.DbRead("dbo.AP_Vendor_GetLookup", null, connStr);
            return DbRead("AP.GetVendorChkActive", null, connStr);
        }

        public bool GetListBu(DataSet dsVendor, string buCode, string vendorName)
        {
            // Get All actived Bu in specified BuGrpCode
            var dsBu = new DataSet();

            var result = _bu.Get(dsBu, buCode);

            if (result)
            {
                // Get all data in group
                foreach (DataRow drBu in dsBu.Tables[_bu.TableName].Rows)
                {
                    // Create Connection String by Business Unit
                    var connStr = new Common.ConnectionStringConstant().Get(drBu);

                    //string connStr = "Data Source=" + drBu["ServerName"].ToString() + "; " +
                    //    "Initial Catalog = " + drBu["DatabaseName"].ToString() + "; " +
                    //    "User ID = " + drBu["UserName"].ToString() + "; " +
                    //    "Password = " + GnxLib.EnDecryptString(drBu["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);

                    GetList(dsVendor, vendorName, connStr);
                }
            }

            return result;
        }

        public bool GetList(DataSet dsVendor, string vendorName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorName", vendorName);

            return DbRetrieve("[AP].[GetVendorByVendorName]", dsVendor, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get vendor code that does not have reference vendor code for export to carmen.
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetExpVendor(string dateFrom, string dateTo, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", dateFrom);
            dbParams[1] = new DbParameter("@ToDate", dateTo);

            return DbRead("[AP].[GetExpVendor]", dbParams, connStr);
        }

        #endregion
    }
}