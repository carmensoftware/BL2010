using System.Data;
using Blue.DAL;

namespace Blue.BL.AR
{
    public class ARProfile : DbHandler
    {
        #region "Attributies"

        //DebtorDetail debtorDetail   = new DebtorDetail();
        private readonly ProfileAttachment _debtorAttachment = new ProfileAttachment();
        private readonly ProfileComment _debtorComment = new ProfileComment();
        private readonly ProfileDefaultWHT _debtorDefaultWht = new ProfileDefaultWHT();
        private readonly ProfileMisc _debtorMisc = new ProfileMisc();
        private readonly ProfileActiveLog _profileActiveLog = new ProfileActiveLog();
/*
        private ProfileCategory _debtorCategory = new ProfileCategory();
*/

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public ARProfile()
        {
            SelectCommand = "SELECT * FROM AR.Profile";
            TableName = "ARProfile";
        }

        /// <summary>
        ///     Get customer using id
        /// </summary>
        /// <param name="debtorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(int CustomerCode, string connStr)
        {
            var dtProfile = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CustomerCode", CustomerCode.ToString());

            // Get data
            dtProfile = DbRead("AR.GetProfileByCustomerCode", dbParams, connStr);

            // Return result
            return dtProfile;
        }

        public DataSet Get(DataSet dsProfile, string CustomerCode, string connStr)
        {
            // Create parameter
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CustomerCode", CustomerCode);

            // Get data
            DbRetrieve("AR.GetProfileByCustomerCode", dsProfile, dbParams, TableName, connStr);

            // Return result
            return dsProfile;
        }

        /// <summary>
        ///     Get debtor using id
        /// </summary>
        /// <param name="debtorCode"></param>
        /// <param name="dsDebtor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(string customerCode, DataSet dsCS, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@CustomerCode", customerCode);

            // Get data
            result = DbRetrieve("AR.GetProfileByCustomerCode", dsCS, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        ///// <summary>
        ///// return datatable and get searching match data.
        ///// </summary>
        ///// <param name="keyWord"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public DataTable GetDebtorListSearch(string keyWord, string connStr)
        //{
        //    // parameter value assing to param array.
        //    DbParameter[] dbParams  = new DbParameter[1];
        //    dbParams[0]             = new DbParameter("@KeyWord", keyWord);

        //    return this.DbRead("AR.GetDebtorListSearch", dbParams, connStr);
        //}

        /// <summary>
        ///     Get debtor using account view id
        /// </summary>
        /// <param name="dsAccount"></param>
        public void GetList(DataSet dsProfile, int profileViewID, int userID, string connStr)
        {
            var dtProfile = new DataTable();
            var dtProfileViewCriteria = new DataTable();
            var profile = new BL.AR.ARProfile();
            var profileViewQuery = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate query
            profileViewQuery = new ProfileView().GetViewQuery(profileViewID, userID, connStr);

            // Generate parameter
            dtProfileViewCriteria = new ProfileViewCriteria().GetList(profileViewID, connStr);

            if (dtProfileViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtProfileViewCriteria.Rows.Count];

                for (var i = 0; i < dtProfileViewCriteria.Rows.Count; i++)
                {
                    var dr = dtProfileViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtProfile = profile.DbExecuteQuery(profileViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtProfile = profile.DbExecuteQuery(profileViewQuery, null, connStr);
            }

            // Return resutl
            if (dsProfile.Tables[TableName] != null)
            {
                dsProfile.Tables.Remove(TableName);
            }

            dtProfile.TableName = TableName;
            dsProfile.Tables.Add(dtProfile);
        }

        //public bool GetList(DataSet dsProfile, string connStr)
        //{
        //    bool result = false;
        //    // Get data
        //    result = DbRetrieve("AR.GetDebtorForPopup", dsProfile, null, this.TableName, connStr);
        //    // Return result
        //    return result;
        //}
        /// <summary>
        ///     Get all of debtor except recurring type
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="userID"></param>
        /// <param name="connStr"></param>
        public void GetPreveiw(DataSet dsDebtor, int userID, string connStr)
        {
            var debtor = new ARProfile();
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            var dtDebtor = new DataTable();
            var dtDebtorViewCriteria = new DataTable();

            var debtorViewQuery = string.Empty;

            // Generate  query
            debtorViewQuery = new ProfileView().GetViewQueryPreview(dsDebtor, userID, connStr);

            // Generate  parameter
            dtDebtorViewCriteria = dsDebtor.Tables["ProfileViewCriteria"];

            if (dtDebtorViewCriteria.Rows.Count > 0)
            {
                var dbParams = new DbParameter[dtDebtorViewCriteria.Rows.Count];

                for (var i = 0; i < dtDebtorViewCriteria.Rows.Count; i++)
                {
                    var dr = dtDebtorViewCriteria.Rows[i];
                    dbParams[i] =
                        new DbParameter("@" + dr["SeqNo"] + field.GetFieldName(dr["FieldID"].ToString(), connStr),
                            dr["Value"].ToString());
                }

                // Get data                
                dtDebtor = debtor.DbExecuteQuery(debtorViewQuery, dbParams, connStr);
            }
            else
            {
                // Get data                
                dtDebtor = debtor.DbExecuteQuery(debtorViewQuery, null, connStr);
            }

            // Return result
            if (dsDebtor.Tables[TableName] != null)
            {
                dsDebtor.Tables.Remove(TableName);
            }

            dtDebtor.TableName = TableName;
            dsDebtor.Tables.Add(dtDebtor);
        }

        /// <summary>
        ///     Save to database
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProfile, string status, string connStr)
        {
            var profileMisc = new ProfileMisc();
            var profileComment = new ProfileComment();
            var profileAttachment = new ProfileAttachment();
            var address = new Blue.BL.Profile.Address();
            var proProfile = new Blue.BL.Profile.Profile();

            var result = false;

            if (status.ToUpper() == "EDIT")
            {
                var dbSaveSorce = new DbSaveSource[4];

                // Create dbSaveSource
                dbSaveSorce[0] = new DbSaveSource(dsProfile, SelectCommand, TableName);
                dbSaveSorce[1] = new DbSaveSource(dsProfile, profileMisc.SelectCommand, profileMisc.TableName);
                dbSaveSorce[2] = new DbSaveSource(dsProfile, profileAttachment.SelectCommand,
                    profileAttachment.TableName);
                dbSaveSorce[3] = new DbSaveSource(dsProfile, address.SelectCommand, address.TableName);
                //dbSaveSorce[2] = new DbSaveSource(dsDebtor, debtorDetail.SelectCommand, debtorDetail.TableName);
                //dbSaveSorce[2] = new DbSaveSource(dsDebtor, debtorComment.SelectCommand, debtorComment.TableName);

                // Save to database
                result = DbCommit(dbSaveSorce, connStr);
            }
            else
            {
                var dbSaveSorce = new DbSaveSource[4];

                // Create dbSaveSource
                dbSaveSorce[0] = new DbSaveSource(dsProfile, proProfile.SelectCommand, proProfile.TableName);
                dbSaveSorce[1] = new DbSaveSource(dsProfile, SelectCommand, TableName);
                dbSaveSorce[2] = new DbSaveSource(dsProfile, profileMisc.SelectCommand, profileMisc.TableName);
                dbSaveSorce[3] = new DbSaveSource(dsProfile, profileAttachment.SelectCommand,
                    profileAttachment.TableName);
                //dbSaveSorce[2] = new DbSaveSource(dsDebtor, debtorDetail.SelectCommand, debtorDetail.TableName);
                //dbSaveSorce[2] = new DbSaveSource(dsDebtor, debtorComment.SelectCommand, debtorComment.TableName);

                // Save to database
                result = DbCommit(dbSaveSorce, connStr);
            }

            // Return result
            return result;
        }

        /// <summary>
        ///     Delete from database
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsDebtor, string connStr)
        {
            var result = false;

            // Create dbSaveSource
            var dbSaveSorce = new DbSaveSource[6];
            dbSaveSorce[0] = new DbSaveSource(dsDebtor, _profileActiveLog.SelectCommand, _profileActiveLog.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsDebtor, _debtorAttachment.SelectCommand, _debtorAttachment.TableName);
            //dbSaveSorce[2]              = new DbSaveSource(dsDebtor, debtorCategory.SelectCommand, debtorCategory.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsDebtor, _debtorComment.SelectCommand, _debtorComment.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsDebtor, _debtorDefaultWht.SelectCommand, _debtorDefaultWht.TableName);
            dbSaveSorce[4] = new DbSaveSource(dsDebtor, _debtorMisc.SelectCommand, _debtorMisc.TableName);
            dbSaveSorce[5] = new DbSaveSource(dsDebtor, SelectCommand, TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsDebtor"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsDebtor, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("AR.GetProfileList", dsDebtor, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="debtorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string debtorCode, string connStr)
        {
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CustomerCode", debtorCode);

            DbRetrieve("AR.GetProfileByCustomerCode", dsTmp, dbParams, TableName, connStr);
            return dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
        }

        /// <summary>
        ///     Get data to lookup.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsDebtor = new DataSet();

            // Get Data            
            DbRetrieve("AR.GetProfileLookup", dsDebtor, null, TableName, connStr);

            // Return result
            return dsDebtor.Tables[TableName];
        }

        #endregion
    }
}