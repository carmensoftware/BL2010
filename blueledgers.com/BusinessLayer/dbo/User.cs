using System;
using System.Data;
using Blue.DAL;
using System.Security.Cryptography;

namespace Blue.BL.dbo
{
    public class User : DbHandler
    {
        private readonly BUUser buUser = new BUUser();
        private readonly Option.Admin.Security.UserRole userRole = new Option.Admin.Security.UserRole();
        private readonly Blue.BL.ADMIN.UserStore userStore = new Blue.BL.ADMIN.UserStore();

        /* 
            2026-06-30 = andalanta = 7+1=8
            2026-06-30 = floralcourthotel = 5+6 = 11


            2026-05-31 = ramadadmabangkok = 19
            2026-05-31 = chi = 2
            2026-05-31 = HotelSensai (Chiang Mai) = 3+8 = 11 ปรับเป็น 10 


            2026-04-30 = indochinephuket = 27

            2026-03-31 = wyndhamjomtienpattaya = 20 + 2 = 22
            2026-03-31 = silavadeepoolsparesort = 28
            2026-03-31 = DeevanaPhuket = 65=> 69

            2026-03-31 = manor = 4
          
            2026-02-28 = cafedelmarphuket =46
            2026-02-28 = gcmt = 1
            2026-02-28 = dewaphuketresort = 14
            2026-02-28 = belairebangkok = 11

            2026-02-28 = mybeachphuket = 10+? = 13
            2026-02-28 = sorahotels = 9
          
            2026-01-31 = bayvillaskohphangan = 1+5+1 = 7
            2026-01-31 = hotellotussukhumvit = 15+2 = 17 + 2 = 19
            2026-01-31 = verandaresortautograph = 3+10 = 13
            2026-01-31 = urbanalangsuanbkk = 7
            2026-01-31 = legacysuites = 22
          
          
            2026-01-31 = bwpluscarapace =17
            2026-01-31 = boulevardhotelbangkok = 10 + 17 = 27
            2026-01-31 = bestwesternchatuchak = 9
            2026-01-31 = Phuket Emerald = 15+5 = 20 (15)

            2026-01-31 = ibis = 999
            2026-01-31 = surestaytheiconicari = 10        
            2026-01-31 = teerapatgroup = 5 + 5 = 10
            2026-01-31 = wyndhamgardenphuketkamala = 25
            2026-01-31 = absoluteresorts = 1
            2026-01-31 = brownstarling = 18

            ------------------------------------------------------------------
         
            2025-12-31 = bestwesternratchada = 10        
         
            2025-11-30 = skyviewresortphuket = 11
            2025-11-30 = wyndhamgardennaithon = 21
            2025-11-30 = chalongmarina = 8+2=10
            2025-11-30 = korabeachresort = 10 + 22 = 32

            2025-11-30 = tinbaron = 18
            2025-11-30 = siamese = 24+2 = 26 + 20 + 10 = 56
             

            2024-10-30 = BlumonkeyKrabi = 6
            2025-10-31 = waltonsuitessukhumvitbangkok = 5
            2025-10-31 = blumonkeybangtaophuket = 6
             
            2025-10-30 = sztowerthai = 13
             
             
            2025-09-30 = rasahospitality = 73 + 17 = 90
            2025-09-30 = varanahotel = 16
            2025-09-30 = TheSarojin = 3
            2025-09-30 = kckohchang = 17
            2025-09-30 = theyamaphuket = 12
            2025-09-30 = sunshine-residences = 10
          
          
          
          
          
            2025-08-31 = Diamond Cottage = 15+5=20
            2025-08-31 = chatriumniseko = 9
            2025-08-31 = palayanagroup = 2+20 = 22
            2025-08-31 = surestaytheiconic = 11
            2025-08-31 = thekaset = 10
            2025-08-31 = thenappathong = 8+5=13
            2025-08-31 = surestaytheiconic = 4+7=11
            2025-08-31 = themarinphuket = 19
            2025-08-31 = celessamui = 25


            2025-07-31 = Nexen = 20
            2025-07-31 = Zeavola = 28
            2025-07-31 = bestwesternclicksathorn11 = 10
            2025-07-31 = bwsanctuary = 10
            2025-07-31 = MysKhaoyai = 12
            2025-07-31 = silqandsq = 9+5=14
          
          
            2025-07-31 = BaanSamuiResort = 8
            2025-07-31 = firaphuketbeachclub = 5+3=8
            2025-07-31 = granddiamondsuites = 6+4 = 10
            2025-07-31 = HOPINN (OnPermise) = 27 
            2025-07-31 = kappasensesubud = 17
            2025-07-31 = legacysuites = 15 + 7 = 22
            2025-07-31 = maduzi = 20
            2025-07-31 = maraleina = 23+1 = 24
            2025-07-31 = selinaserenityrawai = 8
          
          
          
          
          
            2025-07-31 = twinlotus (on permise) = 16
          
          
          
          
            2025-06-30 = paresaresort = 23

            2025-06-30 = hopinnth = 27 



            ---------------------------------------------------------------







            --------- Expired -------------

            2025-02-28 = blumonkeybangsaen = 5
         
          
            2025-01-31 = OKM = 50
            2025-01-31 = MHRH = 3

            2024-07-31 = publichouse = 6
            2024-07-31 = rxvwellness = 35
            2024-05-31 = tribehotels = 8 *ย้ายไปรวมกับ siamese
            2024-03-31 = homa = 24
            2024-03-31 = Lilit = 8

            2023-10-31 = did3 (SHotel) = 10
            2023-10-31 = Best Western Premier Bayphere Pattaya = 25         
            2023-01-31 = 12TheResidence = 3
            2023-01-31 = SPM = 20 
        
        */

        private DateTime licenseExpiredDate = new DateTime(2025, 9, 30);
        private int licenseActiveUser = 110;
        

        public User()
        {
            SelectCommand = "SELECT * FROM [User]";
            TableName = "User";
        }

        /// <summary>
        ///     Correcting user name and password.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        /// 

        public int GetActiveUser()
        {
            string sqlSelect = "SELECT COUNT(*) as ActiveUserCount FROM [dbo].[User] WHERE IsActived = 1 AND LoginName NOT IN ('support@carmen','support@genex')";

            return (int)DbExecuteQuery(sqlSelect, null).Rows[0][0]; 

        }

        public int GetActiveUserLicense()
        {
            return licenseActiveUser;
        }

        public DateTime GetLicenseExpiredDate()
        {
            return licenseExpiredDate;
        }


        public bool GetUserListByLoginName(DataSet dsUser, string loginName, string password)
        {
            password = GnxLib.EnDecryptString(password, GnxLib.EnDeCryptor.EnCrypt, GnxLib.KEY_LOGIN_PASSWORD);


            string sqlSelect = "SELECT COUNT(*) as IsValid FROM [dbo].[User] WHERE LoginName = '" + loginName + "' AND Password = '" + password + "'";

            if ((int)DbExecuteQuery(sqlSelect, null).Rows[0][0] == 1)
            {

                DbRetrieve("[dbo].[User_GetList]", dsUser, null, TableName);
                return true;
            }
            else
            {
                dsUser = null;
                return false;
            }
        }

        public bool CheckLogin(DataSet dsUser, string LoginName, string Password, ref string MsgError, string connStr)
        {
            //DateTime tempLicenseDate = new DateTime(2016, 9, 9, 0, 0, 0);
            //if (DateTime.Now.Date <= tempLicenseDate.Date)
            //{
            //    licenseActiveUser = 500;
            //}

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            bool isSuccess = DbRetrieve("[dbo].[User_CheckLogin]", dsUser, dbParams, TableName, ref MsgError);

            if (isSuccess)
            {
                if (dsUser.Tables[TableName].Rows.Count > 0)
                {
                    // Encode password to compare with encoded password in database
                    Password = GnxLib.EnDecryptString(Password, GnxLib.EnDeCryptor.EnCrypt, GnxLib.KEY_LOGIN_PASSWORD);

                    var dr = dsUser.Tables[TableName].Rows[0];

                    if (dr["Password"].ToString() != Password)
                    {
                        dsUser = null; // Empty user table
                        // Password incorrect
                        MsgError = "Msg002";
                        return false;
                    }

                    // -- Check requirement for authenticate user
                    if ((bool)dr["IsActived"] == false)
                    {
                        // Inactived User
                        MsgError = "Msg005";
                        return false;
                    }
                    else if (DateTime.Now.Date > licenseExpiredDate.Date)
                    {
                        // License Expired
                        MsgError = "License has been expired since " + licenseExpiredDate.ToShortDateString();
                        return false;
                    }
                    else if (GetActiveUser() > licenseActiveUser)
                    {
                        MsgError = "You have more users than your license allows. Please contact administrator to purchase the license.";
                        return false;
                    }
                    else if (DateTime.Now.Date >= licenseExpiredDate.AddDays(-30))
                    {
                        // License Expired
                        MsgError = "License will be expired on " + licenseExpiredDate.ToShortDateString();
                        return true;
                    }

                }
                else
                {
                    // Does not exist login name   
                    MsgError = "Msg003";
                    return false;
                }
            }
            else
            {
                // Unknow error
                //MsgError = "Msg004";
                dsUser = null;
                return false;
            }

            return true;
        }

        public bool CheckLogin(DataSet dsUser, string LoginName, string Password, ref string MsgError)
        {
            return CheckLogin(dsUser, LoginName, Password, ref MsgError, string.Empty);
        }


        /// <summary>
        ///     Get User data by BuCode
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="BuCode"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsUser, string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("dbo.User_GetList_BuCode", dsUser, dbParams, TableName);
        }

        /// <summary>
        ///     Gets User Data by LoginName
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool Get(DataSet dsUser, string LoginName)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            return DbRetrieve("dbo.User_Get_LoginName", dsUser, dbParams, TableName);
        }

        /// <summary>
        ///     Get User Table Structure.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsUser)
        {
            return DbRetrieve("[dbo].[User_GetSchema]", dsUser, null, TableName);
        }

        /// <summary>
        ///     Get all user related to bu group.
        /// </summary>
        /// <param name="BuGrpCode"></param>
        /// <returns></returns>
        public DataTable GetList(string BuGrpCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);

            return DbRead("dbo.User_GetWFApproval", dbParams);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookUp(string BuCode)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            // Create parameters
            var dtLookUp = DbRead("dbo.User_GetList_BuCode", dbParams);

            if (dtLookUp != null)
            {
                var drBlank = dtLookUp.NewRow();
                dtLookUp.Rows.InsertAt(drBlank, 0);
            }

            return dtLookUp;
        }

        /// <summary>
        ///     Delete User, BuUser, UserRole and UserStore data.
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsUser, string ConnStr)
        {
            var dbSaveSource1 = new DbSaveSource[2];
            dbSaveSource1[0] = new DbSaveSource(dsUser, buUser.SelectCommand, buUser.TableName);
            dbSaveSource1[1] = new DbSaveSource(dsUser, SelectCommand, TableName);

            var delUser = DbCommit(dbSaveSource1, string.Empty);

            if (delUser)
            {
                var dbSaveSource2 = new DbSaveSource[2];
                dbSaveSource2[0] = new DbSaveSource(dsUser, userRole.SelectCommand, userRole.TableName);
                dbSaveSource2[1] = new DbSaveSource(dsUser, userStore.SelectCommand, userStore.TableName);

                // Delete UserRole and UserStore
                return DbCommit(dbSaveSource2, ConnStr);
            }
            return false;
        }

        /// <summary>
        ///     Update new password
        /// </summary>
        /// <param name="dsUser"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUser)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsUser, SelectCommand, TableName);

            return DbCommit(dbSaveSource, string.Empty);
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUser, string ConnStr)
        {
            var dbSaveSource1 = new DbSaveSource[2];
            dbSaveSource1[0] = new DbSaveSource(dsUser, SelectCommand, TableName);
            dbSaveSource1[1] = new DbSaveSource(dsUser, buUser.SelectCommand, buUser.TableName);

            var saveUser = DbCommit(dbSaveSource1, string.Empty);

            if (saveUser)
            {
                var dbSaveSource2 = new DbSaveSource[1];
                dbSaveSource2[0] = new DbSaveSource(dsUser, userRole.SelectCommand, userRole.TableName);

                return DbCommit(dbSaveSource2, ConnStr);
            }

            return false;
        }



    }
}