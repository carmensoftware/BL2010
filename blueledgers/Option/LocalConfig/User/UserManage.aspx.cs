using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


    public partial class Option_LocalConfig_User_UserManage: System.Web.UI.Page
    {
        #region Declare Var

        private string connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        private SqlConnection cnn;
        private static string Mode;
        private static string Username;
        private string DefalutDatabase = System.Configuration.ConfigurationManager.AppSettings["SysDb"].ToString(); //SYS NAME DB
        private static DataSet DBuUserSelect;
        private static DataSet DBuUser;
        private static DataTable[] DBRoleUser;
        private static DataTable[] DBLocationUser;
        private static string[] BuArray;
        private static bool[] BuStatus;
        private static DataSet dbulist;
        private static string BuSelectsListBox;
        private static DataSet DbuNotList;
        private Boolean SaveUserSys = false;
        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
       
    
        #endregion
   

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {


                if ((BlueLedger.PL.BaseClass.LoginInformation)Session["LoginInfo"] != null)
                {
                    Mode = Request.QueryString["MODE"];
                    if (Mode == null || Mode == "")
                    {
                        Response.Redirect("UserManage.aspx?mode=CREATE");
                    }
                    Username = Request.QueryString["USER"];
                    GetMode(Mode, Username);
                    mp1.BehaviorID = "BtnDeleteUser";
                    mp2.BehaviorID = "BtnSelectBu";
                }
                else
                {
                    Response.Redirect("~/ErrorPages/SessionTimeOut.aspx");
                }

            }
        }


        #region GETDATA FUNCTION & FUNCTION
        private void GetMode(string Mode,string LoginN)
        {
            GetBuCode(null);
            int bucount = dbulist.Tables[0].Rows.Count;
            DBRoleUser = new DataTable[bucount];
            DBLocationUser = new DataTable[bucount];
            BuArray = new string[bucount];
            BuStatus = new Boolean[bucount];
            for (int i = 0; i < dbulist.Tables[0].Rows.Count; i++)
            {
                string TableBu = dbulist.Tables[0].Rows[i][0].ToString();
                DBRoleUser[i] = new DataTable(TableBu);
                DBRoleUser[i].Columns.Add("LoginName");
                DBRoleUser[i].Columns.Add("RoleName");
                DBRoleUser[i].Columns.Add("IsActive");
                BuStatus[i] = false;
                DBLocationUser[i] = new DataTable(TableBu);
                DBLocationUser[i].Columns.Add("LoginName");
                DBLocationUser[i].Columns.Add("LocationCode");
                DBLocationUser[i].Columns.Add("IsActive");
                BuArray[i] = TableBu;
            }
            if (Mode == "EDIT" || Mode == "edit")
                {
                    if (LoginN != null && LoginN != "")
                    {
                        GetUser(LoginN);
                        if (TxtLoginName.Text != "")
                        {
                            TxtLoginName.Enabled = false;
                            ActiveEdit(true);
                        }
                    }
                }
                else if (Mode == "VIEW" || Mode == "view")
                {
                    GetUser(LoginN);
                    GetBuCode(null);
                    ActiveView(false);
                }
                else if (Mode == "CREATE" || Mode == "create" || Mode == "" || Mode == null)
                {
                    ActiveEdit(true);
                    btnResetPassword.Visible = false;
                }
        }

        private void GetUser(string LoginN)
        {
            ConnectionSql(DefalutDatabase);
            string strsql = "SELECT LoginName,Password,FName,MName,LName,Email,JobTitle,IsActived ";
            strsql += "FROM dbo.[User] ";
            strsql += "WHERE LoginName='" + LoginN + "'";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dUser = new DataSet();
            da.Fill(dUser);
            if (dUser.Tables[0].Rows.Count > 0)
            {
                TxtLoginName.Text = dUser.Tables[0].Rows[0][0].ToString();
                txtPassword.Text = dUser.Tables[0].Rows[0][1].ToString();
                TxtFirstName.Text = dUser.Tables[0].Rows[0][2].ToString();
                TxtMidName.Text = dUser.Tables[0].Rows[0][3].ToString();
                TxtLastname.Text = dUser.Tables[0].Rows[0][4].ToString();
                TxtEmail.Text = dUser.Tables[0].Rows[0][5].ToString();
                TxtJobTitle.Text = dUser.Tables[0].Rows[0][6].ToString();
              //  ChkStatus.Checked = Convert.ToBoolean(dUser.Tables[0].Rows[0][7]);
                if (Convert.ToBoolean(dUser.Tables[0].Rows[0][7]) == true)
                {
                    ActiveUser.Checked = true;
                    InActiveUser.Checked = false;
                }
                else
                {
                    ActiveUser.Checked = false;
                    InActiveUser.Checked = true;
                }
                GetUserBu(LoginN);
            }
            else
            {
                LblMessage.Text ="Can't Not Find User " + LoginN + "";
                BtnSave.Visible = false;
            }
            
        }

        private void GetUserBu(string LoginN)
        {
            ConnectionSql(DefalutDatabase);
            string strsql = "SELECT a.BuCode ";
            strsql += "FROM dbo.[BuUser] a ";
            strsql += "LEFT JOIN dbo.[Bu] b ON a.BuCode=b.BuCode ";
            strsql += "WHERE LoginName='" + LoginN + "' AND b.Isactived=1";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dBUser = new DataSet();
            da.Fill(dBUser);
            ListBuOn.Items.Clear();
            for (int i = 0;i< dBUser.Tables[0].Rows.Count;i++ )
            {
              ListBuOn.Items.Add(dBUser.Tables[0].Rows[i][0].ToString());
            }
         }

        private bool GetBuUserActive(string LoginN,string BuCode)
        {
            ConnectionSql(DefalutDatabase);
            string strsql = "SELECT a.BuCode ";
            strsql += "FROM dbo.[BuUser] a ";
            strsql += "LEFT JOIN dbo.[Bu] b ON a.BuCode=b.BuCode ";
            strsql += "WHERE LoginName=@LoginN AND a.BuCode=@BuCode   AND b.Isactived=1";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            myCommand.Parameters.AddWithValue("@LoginN", LoginN);
            myCommand.Parameters.AddWithValue("@BuCode", BuCode);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dBUser = new DataSet();
            da.Fill(dBUser);
            if (dBUser.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void GetBuCode(string BuCode)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL;
            if (BuCode == null)
            {
                BuCode = "";
                strSQL = "Select * FROM [dbo].[Bu] Where Isactived=1  Order By IsHq Desc,buCode ";
            }
            else
            {
                strSQL = "Select * FROM [dbo].[Bu] where BuCode = @BuCode  And Isactived=1 Order By IsHq Desc,buCode";
            }
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@BuCode", BuCode);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            dbulist = new DataSet();
            da.Fill(dbulist);
            ListBuIn.TextField = "BuCode";
            ListBuIn.DataSource = dbulist;
            ListBuIn.DataBind();
        }

        private string getEncryptPassword(string painText)
        {
         return Blue.BL.GnxLib.EnDecryptString(painText,Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
        }

        private string GetDataBaseName(string Bucode)
        {
            ConnectionSql(DefalutDatabase);
            string strsql = "SELECT DatabaseName ";
            strsql += "FROM [dbo].[Bu] ";
            strsql += "WHERE BuCode='" + Bucode + "'";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            SqlDataReader rdr = myCommand.ExecuteReader();
            rdr.Read();
            Bucode = rdr.GetString(0);
            return Bucode;

        }

        private void GetRole(string bucode)
        {
            String strsql = "";
            string DatabaseName = GetDataBaseName(bucode);
            ConnectionSql(DatabaseName);
            if (Mode == "VIEW" || Mode == "view")
            {
                strsql = "SELECT RoleName,IsActive ";
                strsql += "FROM ADMIN.[UserRole] ";
                strsql += "WHERE IsActive=1 AND LoginName='" + TxtLoginName.Text.Trim() +"' ";
                strsql += "ORDER BY RoleName ";
            }
            else
            {
                strsql = "SELECT RoleName,IsActive ";
                strsql += "FROM ADMIN.[Role] ";
                strsql += "WHERE IsActive=1 ";
                strsql += "ORDER BY RoleName ";
            }
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dRole = new DataSet();
            da.Fill(dRole);
            ListRoleName.Items.Clear();
            ListRoleName.TextField = "Rolename";
            ListRoleName.DataSource = dRole;
            ListRoleName.DataBind();
            RoleUserSelect(DatabaseName,bucode);

        }

        private void GetLocation(string bucode)
        {
            String strsql = "";
            string DatabaseName = GetDataBaseName(bucode);
            ConnectionSql(DatabaseName);
            if (Mode == "VIEW")
            {
                strsql = "SELECT a.LocationCode +' : '+b.LocationName As LocationCode ";
                strsql += "FROM [ADMIN].UserStore a ";
                strsql += "LEFT JOIN [IN].StoreLocation b ON a.LocationCode=b.LocationCode ";
                strsql += "WHERE LoginName='"+TxtLoginName.Text+"' ";
                strsql += "ORDER BY LocationCode ";
            }
            else
            {
                strsql = "SELECT LocationCode +' : '+LocationName As LocationCode ";
                strsql += "FROM [IN].StoreLocation ";
                strsql += "WHERE IsActive=1 ";
                strsql += "ORDER BY LocationCode ";
            }
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dLocation = new DataSet();
            da.Fill(dLocation);
            ListLocation.Items.Clear();
            ListLocation.TextField = "LocationCode";
            ListLocation.DataSource = dLocation;
            ListLocation.DataBind();
            LocationUserSelect(DatabaseName, bucode);
        }

        private bool CheckNullValues(string Values)
        {
            if (Values == null || Values == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ActiveView(bool active)
        {
            //ChkStatus.Enabled = false;
            ActiveUser.Disabled = true;
            InActiveUser.Disabled = true;
            ListRoleName.SelectionMode=DevExpress.Web.ASPxEditors.ListEditSelectionMode.Single;
            ListLocation.SelectionMode = DevExpress.Web.ASPxEditors.ListEditSelectionMode.Single;
            //Selected.Text = "BU";
            ListBuIn.Visible = active;
            BtnIn.Enabled = active;
            BtnOut.Enabled = active;
            BtnSave.Enabled = !active;
            BtnSave.Visible = true;
            BtnSave.Text = "Edit";
            BtnSaveFinal.Enabled = active;
            BtnCancel.Visible = active;
            TxtEmail.Enabled = active;
            TxtFirstName.Enabled = active;
            TxtJobTitle.Enabled = active;
            TxtLastname.Enabled = active;
            TxtLoginName.Enabled = active;
            TxtMidName.Enabled = active;
            txtPassword.Enabled = active;
            BtnSaveFinal.Visible = active;
            ASPxLabel2.Visible = false;
            txtPassword.Visible = false;
            btnResetPassword.Enabled = false;

     
        }

        private void ActiveEdit(bool active)
        {
            BtnSave.Visible = !active;
            if (Mode == "CREATE" || Mode == "create")
            {
                BtnSave.Visible = active;
                BtnCancel.Visible = false;
                BtnSave.Text = "Save";
                BtnSaveFinal.Visible = !active;

            }
            else
            {
                btnResetPassword.Enabled = true;
                BtnSave.Visible = active;
                ASPxLabel2.Visible = false;
                txtPassword.Visible = false;
                BtnSave.Text = "Delete";
            }
        }

        private int BuActiving(string Bucode)
        {
            int nbu = -1;
            for (int i = 0; i < BuArray.Length; i++)
            {
                if (BuArray[i] == BuSelectsListBox)
                {
                    nbu = i;
                }
            }
            return nbu;
        }

        private void LocationUserSelect(string DatabaseName,string bucode)
        {
            ConnectionSql(DatabaseName);
            String strsql = "SELECT LoginName,LocationCode ";
            strsql += "FROM [ADMIN].UserStore ";
            strsql += "WHERE LoginName=@LoginName ";
            strsql += "ORDER BY LocationCode ";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", TxtLoginName.Text);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dLocationUserSelect = new DataSet();
            da.Fill(dLocationUserSelect);
            int Nbu = -1;
            for (int i = 0; i < ListLocation.Items.Count; i++)
            {
                ListLocation.Items[i].Selected = false;
            }

            for (int i = 0; i < BuArray.Length; i++)
            {
                if (BuArray[i] == bucode)
                {
                    Nbu = i;
                }
            }
            if (DBLocationUser[Nbu].Rows.Count == 0)
            {
                for (int i = 0; i < dLocationUserSelect.Tables[0].Rows.Count; i++)
                {
                    string Location = dLocationUserSelect.Tables[0].Rows[i][1].ToString().Trim();
                    string LocationList = ListLocation.Items[i].ToString().Split(':')[0].Trim();
                    if (Location == LocationList)
                    {
                        ListLocation.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < DBLocationUser[Nbu].Rows.Count; i++)
                {
                    string Location = DBLocationUser[Nbu].Rows[i][1].ToString().Split(':')[0].Trim();
                    bool isactive=Convert.ToBoolean(DBLocationUser[Nbu].Rows[i][2]);
                    string LocationList = ListLocation.Items[i].ToString().Split(':')[0].Trim();
                    if (Location == LocationList && isactive==true)
                    {
                        ListLocation.Items[i].Selected = true;
                    }
                }
            }

        }

        private void RoleUserSelect(string DatabaseName, string bucode)
        {
            ConnectionSql(DatabaseName);
            String strsql = "SELECT RoleName,IsActive ";
            strsql += "FROM [ADMIN].[UserRole] ";
            strsql += "WHERE IsActive=1 And LoginName=@LoginName ";
            strsql += "ORDER BY RoleName ";
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", TxtLoginName.Text);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataSet dRoleUserActive = new DataSet();
            da.Fill(dRoleUserActive);
            int Nbu = -1;
             for (int i = 0; i < BuArray.Length; i++)
                {
                    if (BuArray[i] == bucode)
                    {
                        Nbu = i;
                    }
                }
            if (DBRoleUser[Nbu].Rows.Count == 0)
            {
                for (int i = 0; i < dRoleUserActive.Tables[0].Rows.Count; i++)
                {
                    if (dRoleUserActive.Tables[0].Rows[i][0].ToString() == ListRoleName.Items[i].ToString()
                        && Convert.ToBoolean(dRoleUserActive.Tables[0].Rows[i][1]) == true)
                    {
                        ListRoleName.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < DBRoleUser[Nbu].Rows.Count; i++)//.Tables.Rows.Count; i++)
                {
                    if (DBRoleUser[Nbu].Rows[i][1].ToString() == ListRoleName.Items[i].ToString()
                        && Convert.ToBoolean(DBRoleUser[Nbu].Rows[i][2]) == true)
                    {
                        ListRoleName.Items[i].Selected = true;
                    }
                }
            }
        }

        private void GetBuNotInListOnDatabase(string BuCodeComma)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL;
            strSQL = "Select BuCode FROM [dbo].[Bu] where BuCode NOT IN (@BuCode)  And Isactived=1 Order By IsHq Desc,buCode";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@BuCode", BuCodeComma);
            myCommand.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DbuNotList = new DataSet();
            da.Fill(DbuNotList);
        }

        private void SetSelectLocationToDataTable(string BuCode, int BuAr)
        {
            string LoginName = "";
            DBLocationUser[BuAr].Rows.Clear();
            for (int i = 0; i < ListLocation.Items.Count; i++)
            {
                LoginName = TxtLoginName.Text.ToString().Split(':')[0].ToString();
                string Location = ListLocation.Items[i].ToString();
                if (ListLocation.Items[i].Selected == true)
                {
                    DBLocationUser[BuAr].Rows.Add(LoginName, Location, true);
                }
                else
                {
                    DBLocationUser[BuAr].Rows.Add(LoginName, Location, false);
                }
            }

        }

        private void SetSelectRoleToDataTable(string BuCode, int BuAr)
        {
            DBRoleUser[BuAr].Rows.Clear();
            for (int i = 0; i < ListRoleName.Items.Count; i++)
            {
                string LoginName = TxtLoginName.Text.ToString();
                string RoleName = ListRoleName.Items[i].ToString();
                bool IsActive = false;
                if (ListRoleName.Items[i].Selected == true)
                {
                    IsActive = true;
                }

                DBRoleUser[BuAr].Rows.Add(LoginName, RoleName, IsActive);
            }
        }

        #endregion


        #region Database CONNECT INSERT UPDATE DELETE

        private void ConnectionSql(string DatabaseName)
        {
            cnn = new SqlConnection();

            if (DefalutDatabase != DatabaseName)
            {
                connetionString = connetionString.Replace(DefalutDatabase, DatabaseName);
            }
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

            }
            catch (Exception ex)
            {
                cnn.Close();
                string ErrorMess = ex.ToString();
            }
            connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        }

        private void InsertUser(string LoginName ,string Password ,string FirstName ,string MidName ,
                                string LastName, string Email, string Jobtitle, bool IsActive)
        {
             SqlCommand myCommand;
             ConnectionSql(DefalutDatabase);
             string strSQL = "";
             strSQL = " INSERT INTO dbo.[User]( LoginName,Password,FName,MName,LName,Email,IsActived,Jobtitle,HomePage)";
             strSQL += " VALUES ( @LoginName,@Password , @FirstName , @MidName ,@LastName,";
             strSQL += " @Email ,@IsActive, @Jobtitle,'~/Option/User/Default.aspx' )";
             myCommand = new SqlCommand(strSQL, cnn);
             myCommand.Parameters.AddWithValue("@LoginName", LoginName);
             myCommand.Parameters.AddWithValue("@Password", Password);
             myCommand.Parameters.AddWithValue("@FirstName", FirstName);
             myCommand.Parameters.AddWithValue("@MidName", MidName);
             myCommand.Parameters.AddWithValue("@LastName", LastName);
             myCommand.Parameters.AddWithValue("@Email", Email);
             myCommand.Parameters.AddWithValue("@IsActive", IsActive);
             myCommand.Parameters.AddWithValue("@Jobtitle", Jobtitle);
             myCommand.ExecuteNonQuery();
        }

        private void UpdateUser(string LoginName, string Password, string FirstName, string MidName,
                                string LastName, string Email, string Jobtitle, bool IsActive)
        {
            SqlCommand myCommand;
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = " UPDATE dbo.[User] ";
           // strSQL += "SET password=@Password;"
            strSQL +="SET FName=@FirstName ,MName=@MidName ,LName=@LastName,";
            strSQL += "Email=@Email,Jobtitle=@Jobtitle,IsActived=@IsActive ";
            strSQL += "Where LoginName=@LoginName";
            myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
           // myCommand.Parameters.AddWithValue("@Password", Password);
            myCommand.Parameters.AddWithValue("@FirstName", FirstName);
            myCommand.Parameters.AddWithValue("@MidName", MidName);
            myCommand.Parameters.AddWithValue("@LastName", LastName);
            myCommand.Parameters.AddWithValue("@Email", Email);
            myCommand.Parameters.AddWithValue("@IsActive", IsActive);
            myCommand.Parameters.AddWithValue("@Jobtitle", Jobtitle);
            myCommand.ExecuteNonQuery();
        }

        private void DeleteUser(string LoginName)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL = "DELETE dbo.[User] WHERE LoginName=@LoginName";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.ExecuteNonQuery();
        }


        private void InsertUserStore(string DatabaseName, string LoginName, string LocationCode)
        {
            ConnectionSql(DatabaseName);
            string strSQL = "";
            strSQL = " INSERT INTO [ADMIN].UserStore(  LoginName,LocationCode) ";
            strSQL += " VALUES ( @LoginName,@LocationCode)";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.Parameters.AddWithValue("@LocationCode", LocationCode);
            myCommand.ExecuteNonQuery();

        }

        private void InsertRoleName(string DatabaseName, string LoginName, string RoleName, bool IsActive)
        {
            ConnectionSql(DatabaseName);
            string strSQL = "";
            strSQL = " INSERT INTO Admin.[UserRole]( LoginName,RoleName,IsActive)";
            strSQL += " VALUES ( @LoginName,@RoleName , @IsActive)";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.Parameters.AddWithValue("@RoleName", RoleName);
            myCommand.Parameters.AddWithValue("@IsActive", IsActive);
            myCommand.ExecuteNonQuery();
        }


        private void SelectBuUser(string LoginName)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL =  "SELECT d.BuCode,d.LoginName ";
            strSQL += "FROM [dbo].BuUser d ";
            strSQL += "LEFT JOIN [dbo].Bu b ON d.BuCode=b.BuCode ";
            strSQL +="WHERE LoginName=@LoginName AND IsActived=1";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            DBuUserSelect = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(DBuUserSelect);
        }


        private void AvailedBuUser(string LoginName)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = "SELECT d.BuCode,d.LoginName ";
            strSQL += "FROM [dbo].BuUser d ";
            strSQL += "LEFT JOIN [dbo].Bu b ON d.BuCode=b.BuCode ";
            strSQL += "WHERE LoginName=@LoginName AND Isactived=1";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            DBuUser = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(DBuUser);
        }


        private void InsertBuUser(string BuName, string DatabaseName, string LoginName)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = " INSERT INTO [dbo].BuUser(  BuCode,LoginName,Theme,DispLang) ";
            strSQL += " VALUES ( @BuCode,@LoginName , @Theme,@DispLang)";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@BuCode", BuName);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.Parameters.AddWithValue("@Theme", "Default");
            myCommand.Parameters.AddWithValue("@DispLang", "en-US");
            myCommand.ExecuteNonQuery();
        }

        private void DeleteBuUserByNotSelect(string BuName,string DatabaseName,string LoginName)
        {
            DeleteUserStore(DatabaseName, LoginName);
            DeleteRoleName(DatabaseName, LoginName);
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = " DELETE [dbo].BuUser ";
            strSQL += "WHERE LoginName=@LoginName And BuCode=@BuCode ";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@BuCode", BuName);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.ExecuteNonQuery();
        }

        private void DeleteBuUser(string LoginName)
        {
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = " DELETE [dbo].BuUser ";
            strSQL += "WHERE LoginName=@LoginName";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.ExecuteNonQuery();
        }


        private void DeleteUserStore(string DatabaseName, string LoginName)
        {
            ConnectionSql(DatabaseName);
            string strSQL = "DELETE [ADMIN].UserStore ";
            strSQL += "WHERE LoginName=@LoginName ";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.ExecuteNonQuery();
        }

        private void DeleteRoleName(string DatabaseName, string LoginName)
        {
            ConnectionSql(DatabaseName);
            string strSQL = "";
            strSQL = " DELETE Admin.[UserRole] ";
            strSQL += "WHERE LoginName=@LoginName";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", LoginName);
            myCommand.ExecuteNonQuery();
        }

        private void UpdatePassword(string Password)
        {
            string PasswordEncryp=getEncryptPassword(Password);
            ConnectionSql(DefalutDatabase);
            string strSQL = "";
            strSQL = " UPDATE dbo.[User] ";
            strSQL += "SET Password=@Password";
            strSQL += " WHERE LoginName=@LoginName";
            SqlCommand myCommand = new SqlCommand(strSQL, cnn);
            myCommand.Parameters.AddWithValue("@LoginName", TxtLoginName.Text);
            myCommand.Parameters.AddWithValue("@Password", PasswordEncryp);
            myCommand.ExecuteNonQuery();
        }

        #endregion

        #region EVENT & ACTION
        protected void BtnIn_Click(object sender, EventArgs e)
        {
            ListBuIn.SelectedIndex = 0;
            mp2.Show();
        }

        protected void ResetPassword_Click(object sender, EventArgs e)
        {
          mp3.Show();
        }

        protected void BtnSavePasswords_Click(object sender, EventArgs e)
        {
            if (txtResetPass.Text == null || txtResetPass.Text == "")
            {
                lblMesageRepass.Text = "Please enter Password";
                mp3.Show();
            }
            else if (txtResetPass2.Text == null || txtResetPass2.Text == "")
            {
                lblMesageRepass.Text = "Please enter re password";
                mp3.Show();
            }

            else  if (txtResetPass.Text != txtResetPass2.Text)
            {
                lblMesageRepass.Text = "Password and re Password not match";
                mp3.Show();
            }
            else if (txtResetPass.Text == txtResetPass2.Text)
            {
                UpdatePassword(txtResetPass.Text);
                lblMesageRepass.Text = "Password reset complete";
                mp3.Show();
            }





        }

        
        protected void BtnOut_Click(object sender, EventArgs e)
        {
            if (ListBuOn.SelectedIndex >= 0)
            {
                lblDelete.Text = "Do you want to delete BU";
                lblDeletes.Text = ListBuOn.SelectedItem.ToString();//TxtLoginName.Text;
                mp1.Show();
            }
            else
            {
                return;
            }
        }


        private Boolean savebufinal()
        {
            try
            {
                if (CheckNullValues(TxtLoginName.Text) == true)
                {
                    LblMessage.Text = "Please Insert Login Name";
                    return false;
                }

                if (CheckNullValues(TxtEmail.Text) == true)
                {
                    LblMessage.Text = "Please Insert Email";
                    return false;
                }

                SaveUserBu();

                if (SaveUserSys)
                {
                    return true;
                }
                if (ListBuOn.Items.Count <= 0)
                {
                    for (int del = 0; del < ListBuIn.Items.Count; del++)
                    {
                        string LoginName = TxtLoginName.Text;
                        string BuName = ListBuIn.Items[del].ToString();
                        string DatabaseName = GetDataBaseName(BuName);
                        DeleteBuUserByNotSelect(BuName, DatabaseName, LoginName); //DELETE BuUser From Bu Select 
                    }
                    //Response.Redirect("UserManage.aspx?MODE=VIEW&USER=" + TxtLoginName.Text + "");
                    return true;

                }
                else
                {
                    BuSelectsListBox = ListBuOn.SelectedItem.ToString();
                    // INSERT LIST ROLE TO DATATABLE AND LOCATION
                    string LoginName = TxtLoginName.Text;
                    SelectBuUser(LoginName);
                    string BuCodeing = ListBuOn.SelectedItem.ToString();
                    int numberBu = BuActiving(BuCodeing);
                    SetSelectRoleToDataTable(BuCodeing, numberBu);
                    SetSelectLocationToDataTable(BuCodeing, numberBu);
                    string[] BuInList = new string[ListBuOn.Items.Count];
                    //INSERT ON TABLE BUUSER SYS
                    for (int i = 0; i < ListBuOn.Items.Count; i++)
                    {
                        string BuName = ListBuOn.Items[i].ToString();
                        string DatabaseName = GetDataBaseName(BuName);
                        string BuIn = null;

                        for (int sel = 0; sel < DBuUserSelect.Tables[0].Rows.Count; sel++)
                        {
                            string bu = DBuUserSelect.Tables[0].Rows[sel][0].ToString();
                            if (bu == BuName)
                            {
                                BuIn = "NotNull";
                            }
                        }

                        if (BuIn == null)
                        {
                            InsertBuUser(BuName, DatabaseName, LoginName); //INSERT BuUser From Bu Select 
                        }
                        BuInList[i] = BuName;
                    }
                    //INSERT ON TABLE BUUSER SYS
                    ////////////////////////////////////

                    if (ListBuIn.Items.Count != BuInList.Length)
                    {
                        string NotBu = "";
                        for (int i = 0; i < BuInList.Length; i++)
                        {
                            if (i == 0)
                            {
                                NotBu += BuInList[i];
                            }
                            else
                            {
                                NotBu += "," + BuInList[i];
                            }
                        }
                        GetBuNotInListOnDatabase(NotBu);

                        for (int j = 0; j < DbuNotList.Tables[0].Rows.Count; j++)
                        {
                            string bu = DbuNotList.Tables[0].Rows[j][0].ToString();
                            string DatabaseName = GetDataBaseName(bu);
                            DeleteRoleName(DatabaseName, LoginName);
                            DeleteUserStore(DatabaseName, LoginName);
                            DeleteBuUserByNotSelect(bu, DatabaseName, LoginName);
                        }
                    }
                    //DELETE ON TABLE BUUSER SYS
                    int number = DBRoleUser.Length;
                    //INSERT DATATABLE TO DATABASE WITH BUCODE
                    for (int i = 0; i < number; i++)
                    {
                        string DatabaseName = GetDataBaseName(BuArray[i]);
                        if (DBRoleUser[i].Rows.Count > 0)
                        {
                            DeleteRoleName(DatabaseName, LoginName);
                            for (int j = 0; j < DBRoleUser[i].Rows.Count; j++) // INSERT Role Name
                            {
                                bool IsActive = Convert.ToBoolean(DBRoleUser[i].Rows[j][2]);
                                string RoleName = DBRoleUser[i].Rows[j][1].ToString();
                                InsertRoleName(DatabaseName, LoginName, RoleName, IsActive);
                            }
                        }

                        if (DBLocationUser[i].Rows.Count > 0)
                        {
                            DeleteUserStore(DatabaseName, LoginName);
                            for (int k = 0; k < DBLocationUser[i].Rows.Count; k++)
                            {
                                if (Convert.ToBoolean(DBLocationUser[i].Rows[k][2]) == true)
                                {
                                    string LocationCode = DBLocationUser[i].Rows[k][1].ToString().Split(':')[0];
                                    InsertUserStore(DatabaseName, LoginName, LocationCode);
                                }
                            }

                        }
                    }
                    ListBuOn.Enabled = true;
                //    Response.Redirect("UserManage.aspx?MODE=VIEW&USER=" + TxtLoginName.Text + "");
                    return true;

                }
            }
            catch (Exception ex)
            {
                string ErrorCode = ex.ToString();
              //  Response.Redirect("UserManage.aspx?MODE=VIEW&USER=" + TxtLoginName.Text + "");
                return true;

            }
        }


        protected void BtnSave_Click1(object sender, EventArgs e)
        {
            if (Mode == "CREATE" || Mode == "create")
            {
               Boolean savestatus =savebufinal();
               if (savestatus)
               {
                   Response.Redirect("UserManage.aspx?mode=VIEW&USER=" + TxtLoginName.Text + "");
               }
   
            }
            else if (Mode == "view" || Mode == "VIEW")
            {
                Response.Redirect("UserManage.aspx?mode=EDIT&USER=" + TxtLoginName.Text + "");
            }
            else if (Mode == "edit" || Mode == "EDIT")
            {
                lblDelete.Text = "Do You Want To Delete User";
                lblDeletes.Text = TxtLoginName.Text;
                mp1.Show();
            }
            else
            {
                return;
            }

       }

        private void SaveUserBu()
        {
            SqlCommand myCommand;
            try
            {
                if (Mode == "view" || Mode == "VIEW")
                {
                    Response.Redirect("UserManage.aspx?mode=EDIT&USER=" + TxtLoginName.Text + "");
                }

                string LoginName = TxtLoginName.Text;

                string Password = getEncryptPassword(txtPassword.Text);
                if (CheckNullValues(Password) == true)
                {
                    SaveUserSys = true;
                          return;  //
                }

                string FirstName = TxtFirstName.Text;
                //if (CheckNullValues(FirstName) == true) return;

                string LastName = TxtLastname.Text;

                string MidName = TxtMidName.Text;

                string Email = TxtEmail.Text;

                string Jobtitle = TxtJobTitle.Text;

                bool IsActive;
                if (ActiveUser.Checked == true)
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }

                if (Mode == "CREATE" || Mode == null || Mode == "" || Mode == "create")
                {
                    ConnectionSql(DefalutDatabase);
                    string sqlcheckU = "SELECT LoginName FROM dbo.[User]";
                    sqlcheckU += "WHERE LoginName='" + LoginName + "'";
                    myCommand = new SqlCommand(sqlcheckU, cnn);//Check User In Dbo.User Sys
                    SqlDataReader rdr = myCommand.ExecuteReader();
                    rdr.Read();
                    string Bucode;
                    try
                    {
                        Bucode = rdr.GetString(0);
                    }
                    catch
                    {
                        Bucode = null;
                    }
                    if (Bucode == LoginName)
                    {
                        LblMessage.Text = "Please Check Login Name \n " + LoginName + " Is Duplicate";
                        SaveUserSys = true;
                        return;
                    }
                    InsertUser(LoginName, Password, FirstName, MidName, LastName, Email, Jobtitle, IsActive);
                    LblMessage.Text = "Save Complete";
                    GetBuCode(null);
                    //Active(false);
                }
                else if (Mode == "EDIT" || Mode == "edit")
                {
                    ConnectionSql(DefalutDatabase);
                    UpdateUser(LoginName, Password, FirstName, MidName, LastName, Email, Jobtitle, IsActive);
                    LblMessage.Text = "Update Complete";
                    GetBuCode(null);
                    BtnSave.Enabled = true;
                    SaveUserSys = false;
                }
                else if (Mode == "VIEW" || Mode == "view")
                {
                    LblMessage.Text = "View Mode";
                    GetBuCode(null);
                    ListRoleName.Enabled = false;
                    ListLocation.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = "Please Check Values";
            }
        }



        protected void BtnSaveFinal_Click(object sender, EventArgs e)
        {
            Boolean savestatus = savebufinal();
            if (savestatus)
            {
                Response.Redirect("UserManage.aspx?mode=VIEW&USER=" + TxtLoginName.Text + "");
            }

        }


  

        protected void ListBuOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nbu = BuActiving(BuSelectsListBox);

            if (ListBuOn.Items.Count > 0)
            {
                string Select = ListBuOn.SelectedItem.ToString();
                if (ListRoleName.Items.Count > 0 || ListLocation.Items.Count>0)
                {
                 SetSelectRoleToDataTable(BuSelectsListBox, nbu);
                 GetRole(Select);
                 SetSelectLocationToDataTable(BuSelectsListBox, nbu);
                 GetLocation(Select);
                }
                else
                {
                    GetRole(Select);
                    GetLocation(Select);
                }
            }
            BuSelectsListBox = ListBuOn.SelectedItem.ToString();
        }


        protected void View_Click(object sender, EventArgs e)
        {
          //  Response.Redirect("UserManage.aspx?mode=VIEW&USER=" + TxtLoginName.Text + "");
        }


        protected void BtnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblDelete.Text == "Do You Want To Delete User")
                {
                    if (Mode == "EDIT" || Mode == "edit")
                    {
                        string LoginName = TxtLoginName.Text.ToString();
                        AvailedBuUser(TxtLoginName.Text.ToString());
                        for (int i = 0; i < DBuUser.Tables[0].Rows.Count; i++)
                        {
                            string BuCode = DBuUser.Tables[0].Rows[i][0].ToString();
                            string DbName = GetDataBaseName(BuCode);
                            DeleteRoleName(DbName, LoginName);
                            DeleteUserStore(DbName, LoginName);

                        }
                        DeleteBuUser(LoginName);
                        DeleteUser(LoginName);


                    }
                    Response.Redirect("UserManage.aspx?MODE=CREATE");
                }
                else
                {
                    if (ListBuOn.Items.Count == 0 || ListBuOn.SelectedIndex <= -1)
                    {
                        return;
                    }
                    string LoginName = TxtLoginName.Text;
                    string BuName = ListBuOn.SelectedItem.ToString();//ListBuOn.Items[i].Text.ToString();
                    string DatabaseName = GetDataBaseName(BuName);

                    if (ListBuOn.Items.Count <= 0)
                    {
                        ListRoleName.Items.Clear();
                        ListLocation.Items.Clear();
                    }
                    else if (ListBuOn.Items.Count > 1)
                    {

                        ListBuOn.Items.Remove(ListBuOn.SelectedItem);
                        ListBuOn.SelectedIndex = 1;
                        string Select = ListBuOn.Items[0].ToString();
                        GetRole(Select);
                        GetLocation(Select);
                        BuSelectsListBox = Select;
                    }
                    else if (ListBuOn.Items.Count == 1)
                    {
                        ListBuOn.Items.Remove(ListBuOn.SelectedItem);
                        ListRoleName.Items.Clear();
                        ListLocation.Items.Clear();
                        ListBuOn.SelectedIndex = -1;
                    }
                }
                // 
            }
            catch (Exception ex)
            {
                string Exce = ex.ToString();
            }
          
        }

        protected void BtnSelectBu_Click(object sender, EventArgs e)
        {
            
            string LoginName = TxtLoginName.Text;
            string BuName = ListBuIn.SelectedItem.ToString();
            string DatabaseName = GetDataBaseName(BuName);
            string Select = ListBuIn.SelectedItem.Text;
            var str = ListBuOn.Items.FindByText(Select);
            if (str == null)
            {
                if (ListBuOn.Items.Count <= 0)
                {
                    ListBuOn.Items.Add(Select);
                    GetRole(Select);
                    GetLocation(Select);
                    ListBuOn.SelectedIndex = 0;
                    BuSelectsListBox = Select;
                }
                else
                {
                    ListBuOn.Items.Add(Select);
                }
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            if (Mode == "EDIT" || Mode == "edit")
            {
                Response.Redirect("UserManage.aspx?mode=VIEW&USER=" + TxtLoginName.Text + "");
            }

        }
      
        #endregion







        protected void BtnSavePassword_Click(object sender, EventArgs e)
        {
            lblDelete.Text = "Do you want to delete this BU?";
            lblDeletes.Text = ListBuOn.SelectedItem.ToString();//TxtLoginName.Text;
            mp2.Show();
        }

        private void showModalReset()
        {
            mp2.Show();
        }

}
