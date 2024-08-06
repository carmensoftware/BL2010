using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using BlueLedger.PL.BaseClass;


public partial class Option_Admin_Security_User_UserProfile : BasePage //System.Web.UI.Page
{
    #region User Definition

    private readonly string moduleID = "99.98.1.2";
    private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
    private readonly Blue.BL.dbo.User _user = new Blue.BL.dbo.User();

    private static SqlConnection _conn = new SqlConnection();
    private static Dictionary<string, DataSet> _buData = new Dictionary<string, DataSet>();
    private static List<string> _buNew = new List<string>();
    private static List<string> _buDel = new List<string>();
    private static string _mode = string.Empty;
    private static string _loginName = string.Empty;
    private static string _buCode;

    private DataSet dsUserDepartment = null;


    private bool IsEdit()
    {
        return rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr) >= 3;
    }

    private bool IsDelete()
    {
        return rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr) >= 7;
    }


    private void AlertBox(string text)
    {
        LabelPopMessageBox.Text = text;
        Pop_MessageBox.Show();

    }

    private void ResetVariables()
    {
        _conn.Close();
        _buData.Clear();
        _buNew.Clear();
        _buDel.Clear();
        _mode = string.Empty;
        _loginName = string.Empty;

        dsUserDepartment = null;
    }

    private void AddBuToList(string buCode)
    {
        // Add to ListBU
        ListBU.Items.Add(buCode);

        // Add to _buData
        if (_buData.ContainsKey(buCode))
            _buData.Add(buCode, new DataSet());

        // Add to _buNew
        if (!_buNew.Contains(buCode))
            _buNew.Add(buCode);

        // Add to _buDel
        if (_buDel.Contains(buCode))
            _buDel.Remove(buCode);
    }

    private void RemoveBuFromList(string buCode)
    {
        // Remove from ListBU
        ListBU.Items.Remove(ListBU.SelectedItem);

        // Remove from _buData
        if (_buData.ContainsKey(buCode))
            _buData.Remove(buCode);

        // Remove from _buNew
        if (_buNew.Contains(buCode))
            _buNew.Remove(buCode);

        // Add to _buDel
        if (!_buDel.Contains(buCode))
            _buDel.Add(buCode);
    }

    private void GetUserDepartmentDataSet(string buCode, string loginName)
    {
        if (dsUserDepartment == null)
            dsUserDepartment = new DataSet();

        using (SqlConnection conn = new SqlConnection(GetSqlConnectionString(buCode)))
        {
            conn.Open();
            string sql = string.Format("SELECT DepCode FROM [ADMIN].[UserDepartment] WHERE LoginName = '{0}'", loginName);
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
            {
                adapter.Fill(dsUserDepartment, buCode);
            }
            Session["dsUserDepartment"] = dsUserDepartment;

            DataTable dt = new DataTable();
            string sqlDep = "SELECT DepCode, DepName FROM [ADMIN].[Department] ORDER BY DepCode ";
            using (SqlDataAdapter adapterDep = new SqlDataAdapter(sqlDep, conn))
            {
                adapterDep.Fill(dt);
            }
            DataRow dr = dt.NewRow();
            dr["DepCode"] = string.Empty;
            dr["DepName"] = string.Empty;
            dt.Rows.InsertAt(dr, 0);


            DdlDepartment.DataTextField = "DepName";
            DdlDepartment.DataValueField = "DepCode";
            DdlDepartment.DataSource = dt;
            DdlDepartment.DataBind();

            if (dsUserDepartment.Tables[buCode].Rows.Count > 0)
            {
                string depCode = dsUserDepartment.Tables[buCode].Rows[0]["DepCode"].ToString();
                if (DdlDepartment.Items.FindByValue(depCode) != null)
                    DdlDepartment.SelectedValue = depCode;
            }
        }


    }

    private string GetSqlConnectionString(string buCode)
    {
        string sysConnStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        string connStr = string.Empty;

        if (buCode == string.Empty)  // sysDatabase
            connStr = sysConnStr;
        else
        {
            SqlConnection conn = new SqlConnection(sysConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT ServerName, DatabaseName, UserName, [Password] FROM dbo.Bu WHERE BuCode = @BuCode";
            cmd.Parameters.AddWithValue("@BuCode", buCode);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                string serverName = reader["Servername"].ToString();
                string databaseName = reader["DatabaseName"].ToString();
                string userName = reader["UserName"].ToString();
                string password = Blue.BL.GnxLib.EnDecryptString(reader["Password"].ToString(), Blue.BL.GnxLib.EnDeCryptor.DeCrypt);

                connStr = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3};", serverName, databaseName, userName, password);
            }
            reader.Close();
            conn.Close();
        }

        return connStr;
    }

    private void ConnectToDatabase(string buCode)
    {
        _conn = new SqlConnection(GetSqlConnectionString(buCode));
        try
        {
            _conn.Open();

        }
        catch (Exception ex)
        {
            AlertBox(ex.ToString());
        }
    }

    private void SetControlMode(string mode)
    {
        bool isView = mode.ToUpper() == "VIEW";

        BtnEdit.Visible = isView;
        BtnChangePassword.Visible = isView;
        BtnDelUser.Visible = isView;

        BtnSave.Visible = !isView;
        BtnCancel.Visible = !isView;

        BtnBUAdd.Visible = !isView;
        BtnBUDel.Visible = !isView;

        BtnRoleSelAll.Visible = !isView;
        BtnRoleSelNone.Visible = !isView;

        BtnLocationSelAll.Visible = !isView;
        BtnLocationSelNone.Visible = !isView;

        // Status
        if (isView)
            ActiveUser.Attributes.Add("disabled", "disabled");
        else
            ActiveUser.Attributes.Remove("disabled");

        // Login Name
        TextLoginName.Enabled = mode.ToUpper() == "CREATE";

        LabelPassword.Visible = mode.ToUpper() == "CREATE";
        TextPassword.Visible = mode.ToUpper() == "CREATE";

        LabelPasswordConfirm.Visible = mode.ToUpper() == "CREATE";
        TextPasswordConfirm.Visible = mode.ToUpper() == "CREATE";

        TextFirstName.Enabled = !isView;
        TextMidName.Enabled = !isView;
        TextLastname.Enabled = !isView;
        TextEmail.Enabled = !isView;
        TextJobTitle.Enabled = !isView;
        DdlDepartment.Enabled = !isView;
        DdlBusinessUnit.Enabled = !isView;

        if (isView)
        {
            ListRole.SelectionMode = DevExpress.Web.ASPxEditors.ListEditSelectionMode.Single;
            ListLocation.SelectionMode = DevExpress.Web.ASPxEditors.ListEditSelectionMode.Single;
        }
        else
        {
            ListRole.SelectionMode = DevExpress.Web.ASPxEditors.ListEditSelectionMode.CheckColumn;
            ListLocation.SelectionMode = DevExpress.Web.ASPxEditors.ListEditSelectionMode.CheckColumn;
        }

    }

    private void GetNewUser()
    {
        TextLoginName.Text = string.Empty;
        TextPassword.Text = string.Empty;
        TextFirstName.Text = string.Empty;
        TextMidName.Text = string.Empty;
        TextLastname.Text = string.Empty;
        TextEmail.Text = string.Empty;
        TextJobTitle.Text = string.Empty;
        ActiveUser.Checked = true;

    }

    private void GetUserInfo(string loginName)
    {
        ConnectToDatabase("");

        string sql = string.Format("SELECT * FROM dbo.[User] WHERE LoginName = N'{0}'", loginName);
        // End Modified.

        SqlCommand cmd = new SqlCommand(sql, _conn);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            reader.Read();
            TextLoginName.Text = reader["LoginName"].ToString();
            TextPassword.Text = reader["Password"].ToString();
            TextFirstName.Text = reader["FName"].ToString();
            TextMidName.Text = reader["MName"].ToString();
            TextLastname.Text = reader["LName"].ToString();
            TextEmail.Text = reader["Email"].ToString();
            TextJobTitle.Text = reader["JobTitle"].ToString();
            ActiveUser.Checked = (Convert.ToBoolean(reader["IsActived"]) == true);

            LabelUserName.Text = TextFirstName.Text + " " + TextMidName.Text + " " + TextLastname.Text;

        }
        else
        {
            TextLoginName.Text = string.Empty;
            TextPassword.Text = string.Empty;
            TextFirstName.Text = string.Empty;
            TextMidName.Text = string.Empty;
            TextLastname.Text = string.Empty;
            TextEmail.Text = string.Empty;
            TextJobTitle.Text = string.Empty;
            ActiveUser.Checked = false;
            AlertBox("\"" + loginName + "\", user not found.");
        }
        reader.Close();
        GetBuOfUser(loginName);

    }

    private void GetBuOfUser(string loginName)
    {
        ConnectToDatabase("");

        string sql = "SELECT a.BuCode ";
        sql += "FROM dbo.[BuUser] a ";
        sql += "LEFT JOIN dbo.[Bu] b ON a.BuCode=b.BuCode ";
        sql += "WHERE  b.Isactived=1 AND a.LoginName=@loginName";
        SqlCommand cmd = new SqlCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@loginName", loginName);
        SqlDataReader reader = cmd.ExecuteReader();

        ListBU.Items.Clear();
        while (reader.Read())
        {
            ListBU.Items.Add(reader["BuCode"].ToString());
        }
        reader.Close();
    }

    private void GetBuList()
    {
        ConnectToDatabase("");

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = _conn;
        cmd.CommandText = "SELECT BuCode, BuCode + ' : ' + BuName as BuName FROM dbo.Bu WHERE IsActived = 1";
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);

        // Display only avialable
        for (int i = 0; i < ListBU.Items.Count; i++)
        {
            string buCode = ListBU.Items[i].Text;
            ds.Tables[0].Select("BuCode = '" + buCode + "'")[0].Delete();  // return DataRow[], delete only the first row.
        }

        ListPopBuAdd.DataSource = ds;
        ListPopBuAdd.ValueField = "BuCode";
        ListPopBuAdd.TextField = "BuName";
        ListPopBuAdd.DataBind();


    }

    private void SaveSelectionOfRoleAndLocationToDataTable(string oldBuCode)
    {
        if (oldBuCode != string.Empty)
        {
            // Set Role
            if (_buData.ContainsKey(oldBuCode))
            {
                for (int i = 0; i < _buData[oldBuCode].Tables["Role"].Rows.Count; i++)
                {
                    _buData[oldBuCode].Tables["Role"].Rows[i]["IsActive"] = ListRole.Items[i].Selected;
                }

                // Set Location
                for (int i = 0; i < _buData[oldBuCode].Tables["Location"].Rows.Count; i++)
                {
                    _buData[oldBuCode].Tables["Location"].Rows[i]["IsActive"] = ListLocation.Items[i].Selected;
                }
            }
        }

    }

    private void GetRoleAndLocationOfUser(string buCode, string loginName)
    {
        ConnectToDatabase(buCode);
        string sql = string.Empty;

        // Set Role
        if (_mode.ToUpper() == "VIEW")
        {
            // Modified on: 22/01/2018, By: Fon
            #region comment
            //sql = "SELECT RoleName";
            //sql += " FROM [ADMIN].[UserRole]";
            //sql += " WHERE IsActive=1 AND LoginName=@loginName";
            //sql += " ORDER BY RoleName ";

            //SqlCommand cmd = new SqlCommand(sql, _conn);
            //cmd.Parameters.AddWithValue("@loginName", loginName);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            //ListRole.Items.Clear();
            //ListRole.TextField = "RoleDesc";
            //ListRole.ValueField = "RoleName";
            //ListRole.DataSource = ds;
            //ListRole.DataBind();
            #endregion

            sql = string.Format(@"SELECT r.RoleName, r.RoleDesc FROM [ADMIN].[UserRole] AS ur
                JOIN [ADMIN].[Role] AS r ON r.RoleName = ur.RoleName
                WHERE r.IsActive=1 AND ur.IsActive =1
                AND LoginName=@loginName 
                GROUP BY r.RoleName, r.RoleDesc
                ORDER BY r.RoleName ");

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@loginName", loginName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ListRole.Items.Clear();
            ListRole.TextField = "RoleDesc";
            ListRole.ValueField = "RoleName";
            ListRole.DataSource = ds;
            ListRole.DataBind();
            // End Modified.
        }
        else
        {
            // Modified on: 22/01/2018, By: Fon
            //sql = "SELECT r.RoleName, r.RoleDesc ISNULL(ur.IsActive, 0) as IsActive";
            //sql += " FROM [ADMIN].[Role] r";
            //sql += " LEFT JOIN [ADMIN].UserRole ur ON ur.RoleName = r.RoleName AND ur.LoginName = @loginName";
            //sql += " ORDER BY r.RoleName";

            sql = string.Format(@"SELECT r.RoleName, r.RoleDesc, ISNULL(ur.IsActive, 0) as IsActive
                FROM [ADMIN].[Role] r
                LEFT JOIN [ADMIN].UserRole ur ON ur.RoleName = r.RoleName AND ur.LoginName = @loginName
                ORDER BY r.RoleName ");
            // End Modified.

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@loginName", loginName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            if (!_buData.ContainsKey(buCode))  // The first time access, adding BuCode
                _buData.Add(buCode, new DataSet());

            if (!_buData[buCode].Tables.Contains("Role"))  // The first load
            {
                da.Fill(_buData[buCode], "Role");

                ListRole.Items.Clear();
            }

            ListRole.TextField = "RoleDesc";
            ListRole.ValueField = "RoleName";
            ListRole.DataSource = _buData[buCode].Tables["Role"];
            ListRole.DataBind();

            // Set active role
            for (int i = 0; i < ListRole.Items.Count; i++)
            {
                ListRole.Items[i].Selected = Convert.ToBoolean(_buData[buCode].Tables["Role"].Rows[i]["IsActive"]);
            }


        }


        // Set Location
        if (_mode == "VIEW")
        {
            sql = "SELECT a.LocationCode, a.LocationCode +' : '+b.LocationName As LocationName ";
            sql += " FROM [ADMIN].UserStore a ";
            sql += " LEFT JOIN [IN].StoreLocation b ON a.LocationCode=b.LocationCode ";
            sql += " WHERE LoginName=@loginName";
            sql += " ORDER BY LocationName ";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@loginName", loginName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ListLocation.Items.Clear();
            ListLocation.TextField = "LocationName";
            ListLocation.ValueField = "LocationCode";
            //ListLocation.DataSource = dsLocation;
            ListLocation.DataSource = ds;
            ListLocation.DataBind();
        }
        else
        {
            sql = "SELECT sl.LocationCode, sl.LocationCode + ' : ' + sl.LocationName as LocationName, CASE WHEN sl.LocationCode=us.LocationCode THEN 1 ELSE 0 END as IsActive";
            sql += " FROM [IN].StoreLocation sl";
            sql += " LEFT JOIN [ADMIN].UserStore us ON us.LocationCode = sl.LocationCode AND us.LoginName = @loginName";
            sql += " WHERE sl.IsActive = 1";
            sql += " ORDER BY sl.LocationCode";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@loginName", loginName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            if (!_buData.ContainsKey(buCode))
                _buData.Add(buCode, new DataSet());

            if (!_buData[buCode].Tables.Contains("Location"))  // The first load
            {
                da.Fill(_buData[buCode], "Location");

                ListLocation.Items.Clear();
            }

            ListLocation.TextField = "LocationName";
            ListLocation.ValueField = "LocationCode";
            ListLocation.DataSource = _buData[buCode].Tables["Location"];
            ListLocation.DataBind();


            for (int i = 0; i < ListLocation.Items.Count; i++)
            {
                ListLocation.Items[i].Selected = Convert.ToBoolean(_buData[buCode].Tables["Location"].Rows[i]["IsActive"]);
            }
        }

    }

    private string GetEncryptPassword(string painText)
    {
        return Blue.BL.GnxLib.EnDecryptString(painText, Blue.BL.GnxLib.EnDeCryptor.EnCrypt, Blue.BL.GnxLib.KEY_LOGIN_PASSWORD);
    }

    private void UpdatePassword(string loginName, string password)
    {
        ConnectToDatabase("");

        string passwordEncrypt = GetEncryptPassword(password);
        string sql = string.Empty;
        sql = " UPDATE dbo.[User] ";
        sql += "SET Password=@Password,";
        sql += "    LastUpdatedPassword=@Date";
        sql += " WHERE LoginName=@LoginName";
        SqlCommand cmd = new SqlCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        cmd.Parameters.AddWithValue("@Password", passwordEncrypt);
        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
        cmd.ExecuteNonQuery();
    }

    private string CheckRequiredField()
    {
        if (TextLoginName.Text == string.Empty)
            return "Login name is requred.";
        else if (TextPasswordConfirm.Visible && TextPassword.Text != string.Empty && TextPasswordConfirm.Text != TextPassword.Text)
            return "Password does not match with confirm password.";
        else if (TextFirstName.Text == string.Empty)
            return "First name is required";
        else if (TextEmail.Text == string.Empty)
            return "Email is required";
        else if (ListBU.Items.Count == 0)
            return "Business Unit is required";
        else
            return string.Empty;
    }

    private bool CheckLoginName(string loginName)
    {
        SqlConnection conn = new SqlConnection(GetSqlConnectionString(""));
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT COUNT(*) as RecordCount FROM dbo.[User] WHERE LoginName = @LoginName";
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        conn.Open();
        bool hasExist = (int)cmd.ExecuteScalar() > 0;
        conn.Close();
        return !hasExist;  // return true (if not exist)
    }

    private string SaveUser(string loginName)
    {
        loginName = loginName.Trim();


        bool validUser = true;
        string sql = string.Empty;

        string message = CheckRequiredField();  // return empty if no errors

        if (message == string.Empty)
        {
            // Save to SysDb
            SqlConnection sysConn = new SqlConnection(GetSqlConnectionString(""));
            SqlCommand sysCmd = new SqlCommand();
            sysCmd.Connection = sysConn;
            sysConn.Open();


            if (ActiveUser.Checked)
            {
                int activeUserLicense = _user.GetActiveUserLicense();
                int activeUserCurrent = _user.GetActiveUser() + 1;
                if (activeUserCurrent > activeUserLicense)
                {
                    message = "Active users are exceed than license. This user will be inactived.";
                    ActiveUser.Checked = false;
                }
            }


            // 1. Save to <SysDB>.dbo.User
            if (_mode.ToUpper() == "CREATE")
            {
                validUser = CheckLoginName(loginName);
                if (validUser)
                {
                    // dbo.User
                    // Modified on: 18/01/2018, By: Fon
                    //sql = "INSERT INTO dbo.[User]( LoginName, Password, FName, MName, LName,";
                    //sql += "                              Email, IsActived, JobTitle, HomePage)";
                    //sql += " VALUES (@LoginName, @Password, @FirstName, @MidName, @LastName,";
                    //sql += "         @Email, @IsActived, @JobTitle, '~/Option/User/Default.aspx')";

                    sql = string.Format(@"INSERT INTO dbo.[User]( LoginName, Password, FName, MName, LName,
                        Email, IsActived, JobTitle, HomePage, [DepartmentCode] ) 
                        VALUES (@LoginName, @Password, @FirstName, @MidName, @LastName,
                        @Email, @IsActived, @JobTitle, '~/Option/User/Default.aspx', @DepartmentCode)");
                    // End Modified.


                    sysCmd.CommandText = sql;
                    sysCmd.Parameters.Clear();
                    sysCmd.Parameters.AddWithValue("@LoginName", TextLoginName.Text);
                    sysCmd.Parameters.AddWithValue("@Password", GetEncryptPassword(TextPasswordConfirm.Text));
                    sysCmd.Parameters.AddWithValue("@FirstName", TextFirstName.Text);
                    sysCmd.Parameters.AddWithValue("@MidName", TextMidName.Text);
                    sysCmd.Parameters.AddWithValue("@LastName", TextLastname.Text);
                    sysCmd.Parameters.AddWithValue("@Email", TextEmail.Text);
                    sysCmd.Parameters.AddWithValue("@IsActived", ActiveUser.Checked);
                    sysCmd.Parameters.AddWithValue("@JobTitle", TextJobTitle.Text);
                    //sysCmd.Parameters.AddWithValue("@DepartmentCode", DdlDepartment.SelectedValue);
                    sysCmd.Parameters.AddWithValue("@DepartmentCode", string.Empty);

                    sysCmd.ExecuteNonQuery();

                }
                else  // not valid user such as duplicate or wrong format
                {
                    if (loginName == string.Empty)
                        message = "Login name is required.";
                    else
                        //message = "This '" + loginName + "' is not available for using as an login name.";
                        message = string.Format("'{0}', the login name is already existed.", loginName); //"This '" + loginName + "' is not available for using as an login name.";
                    return message;
                }
            }
            else  // Edit
            {
                sql += " UPDATE dbo.[User] SET ";
                //sql += "   Password = @Password,";
                sql += "   FName = @FName,";
                sql += "   MName = @MName,";
                sql += "   LName = @LName,";
                sql += "   Email = @Email,";
                sql += "   IsActived = @IsActived,";
                sql += "   JobTitle = @JobTitle";

                // Added on: 18/01/2018, By: Fon
                sql += " , [DepartmentCode] = @DepartmentCode";
                // End Added.

                sql += " WHERE LoginName = @LoginName";
                sysCmd.CommandText = sql;
                //cmd.Parameters.AddWithValue("@Password", GetEncryptPassword(TextPassword.Text));
                sysCmd.Parameters.AddWithValue("@FName", TextFirstName.Text);
                sysCmd.Parameters.AddWithValue("@MName", TextMidName.Text);
                sysCmd.Parameters.AddWithValue("@LName", TextLastname.Text);
                sysCmd.Parameters.AddWithValue("@Email", TextEmail.Text);
                sysCmd.Parameters.AddWithValue("@IsActived", ActiveUser.Checked);
                sysCmd.Parameters.AddWithValue("@Jobtitle", TextJobTitle.Text);
                sysCmd.Parameters.AddWithValue("@LoginName", loginName);
                //sysCmd.Parameters.AddWithValue("@DepartmentCode", DdlDepartment.SelectedValue);
                sysCmd.Parameters.AddWithValue("@DepartmentCode", string.Empty);

                sysCmd.ExecuteNonQuery();
            }
            // end of step (1)

            // 2. Check deleted BU
            foreach (string buCode in _buDel)
            {
                sysCmd.CommandText = "DELETE FROM dbo.BuUser WHERE LoginName = @LoginName AND BuCode = @BuCode";
                sysCmd.Parameters.Clear();
                sysCmd.Parameters.AddWithValue("@LoginName", TextLoginName.Text);
                sysCmd.Parameters.AddWithValue("@BuCode", buCode);
                sysCmd.ExecuteNonQuery();

                // Delete Role and Location of buCode
                SqlConnection buConn = new SqlConnection(GetSqlConnectionString(buCode));
                SqlCommand buCmd = new SqlCommand();
                buCmd.Connection = buConn;
                buConn.Open();

                buCmd.CommandText = "DELETE FROM [ADMIN].UserRole WHERE LoginName = @LoginName";
                buCmd.Parameters.Clear();
                buCmd.ExecuteNonQuery();

                buCmd.CommandText = "DELETE FROM [ADMIN].UserStore WHERE LoginName = @LoginName";
                buCmd.Parameters.Clear();
                buCmd.ExecuteNonQuery();

                buCmd.CommandText = "DELETE FROM [ADMIN].UserDepartment WHERE LoginName = @LoginName";
                buCmd.Parameters.Clear();
                buCmd.ExecuteNonQuery();
            } // end of step (2)
            _buDel.Clear();


            // 3. Save new BU to <SysDB>.dbo.BuUser and each BU with Role and Location
            foreach (var newBuCode in _buNew)
            {

                sql = "INSERT INTO dbo.[BuUser](Bucode, LoginName, Theme, DispLang)";
                sql += " SELECT BuCode, @LoginName, 'Default', LangCode";
                sql += " FROM dbo.BuFmt";
                sql += " WHERE BuCode = @BuCode";

                sysCmd.CommandText = sql;
                sysCmd.Parameters.Clear();
                sysCmd.Parameters.AddWithValue("@LoginName", TextLoginName.Text);
                sysCmd.Parameters.AddWithValue("@BuCode", newBuCode);
                sysCmd.ExecuteNonQuery();

                // Role and Location permission of BU

                // Add Role 
                if (_buData.ContainsKey(newBuCode))
                {
                    SqlConnection buConn = new SqlConnection(GetSqlConnectionString(newBuCode));
                    SqlCommand buCmd = new SqlCommand();
                    buCmd.Connection = buConn;
                    buConn.Open();

                    if (_buData[newBuCode].Tables.Contains("Role"))
                    {
                        buCmd.CommandText = "DELETE FROM [ADMIN].UserRole WHERE LoginName = @LoginName";
                        buCmd.Parameters.Clear();
                        buCmd.Parameters.AddWithValue("@LoginName", loginName);
                        buCmd.ExecuteNonQuery();

                        DataTable dt = _buData[newBuCode].Tables["Role"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string roleName = dt.Rows[i]["RoleName"].ToString();
                            bool isActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);

                            buCmd.CommandText = "INSERT INTO [ADMIN].UserRole (LoginName, RoleName, IsActive)";
                            buCmd.CommandText += "VALUES (@LoginName, @RoleName, @IsActive)";
                            buCmd.Parameters.Clear();
                            buCmd.Parameters.AddWithValue("@LoginName", loginName);
                            buCmd.Parameters.AddWithValue("@RoleName", roleName);
                            buCmd.Parameters.AddWithValue("@IsActive", isActive);
                            buCmd.ExecuteNonQuery();
                        }
                    }

                    if (_buData[newBuCode].Tables.Contains("Location"))
                    {
                        buCmd.CommandText = "DELETE FROM [ADMIN].UserStore WHERE LoginName = @LoginName";
                        buCmd.Parameters.Clear();
                        buCmd.Parameters.AddWithValue("@LoginName", loginName);
                        buCmd.ExecuteNonQuery();

                        DataTable dt = _buData[newBuCode].Tables["Location"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsActive"]) == true)
                            {

                                buCmd.CommandText = "INSERT INTO [ADMIN].UserStore (LoginName, LocationCode)";
                                buCmd.CommandText += "VALUES (@LoginName, @LocationCode)";
                                buCmd.Parameters.Clear();
                                buCmd.Parameters.AddWithValue("@LoginName", loginName);
                                buCmd.Parameters.AddWithValue("@LocationCode", dt.Rows[i]["LocationCode"].ToString());
                                buCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    _buData.Remove(newBuCode);
                    buConn.Close();

                } // if newBuCode exist in _buData
            } // end of step (3)
            _buNew.Clear();

            // 4. Update existing BU
            foreach (var item in _buData)
            {
                // Role and Location permission of BU
                string buCode = item.Key.ToString();
                // Add Role 
                if (_buData.ContainsKey(buCode))
                {
                    SqlConnection buConn = new SqlConnection(GetSqlConnectionString(buCode));
                    SqlCommand buCmd = new SqlCommand();
                    buCmd.Connection = buConn;
                    buConn.Open();

                    if (_buData[buCode].Tables.Contains("Role"))
                    {
                        buCmd.CommandText = "DELETE FROM [ADMIN].UserRole WHERE LoginName = @LoginName";
                        buCmd.Parameters.Clear();
                        buCmd.Parameters.AddWithValue("@LoginName", loginName);
                        buCmd.ExecuteNonQuery();

                        DataTable dt = _buData[buCode].Tables["Role"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string roleName = dt.Rows[i]["RoleName"].ToString();
                            bool isActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);

                            buCmd.CommandText = "INSERT INTO [ADMIN].UserRole (LoginName, RoleName, IsActive)";
                            buCmd.CommandText += "VALUES (@LoginName, @RoleName, @IsActive)";
                            buCmd.Parameters.Clear();
                            buCmd.Parameters.AddWithValue("@LoginName", loginName);
                            buCmd.Parameters.AddWithValue("@RoleName", roleName);
                            buCmd.Parameters.AddWithValue("@IsActive", isActive);
                            buCmd.ExecuteNonQuery();
                        }
                    }

                    if (_buData[buCode].Tables.Contains("Location"))
                    {
                        buCmd.CommandText = "DELETE FROM [ADMIN].UserStore WHERE LoginName = @LoginName";
                        buCmd.Parameters.Clear();
                        buCmd.Parameters.AddWithValue("@LoginName", loginName);
                        buCmd.ExecuteNonQuery();

                        DataTable dt = _buData[buCode].Tables["Location"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[i]["IsActive"]) == true)
                            {
                                buCmd.CommandText = "INSERT INTO [ADMIN].UserStore (LoginName, LocationCode)";
                                buCmd.CommandText += "VALUES (@LoginName, @LocationCode)";
                                buCmd.Parameters.Clear();
                                buCmd.Parameters.AddWithValue("@LoginName", loginName);
                                buCmd.Parameters.AddWithValue("@LocationCode", dt.Rows[i]["LocationCode"].ToString());
                                buCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    buConn.Close();
                }



            }  // end of step (4)
            _buData.Clear();
            sysConn.Close();

            // 5. Insert/Update User-Department of each BU
            // Update [Admin].UserDepartment

            if (dsUserDepartment != null)
                foreach (DataTable dt in dsUserDepartment.Tables)
                {
                    string buCode = dt.TableName.ToString();

                    SqlConnection buConn = new SqlConnection(GetSqlConnectionString(buCode));
                    SqlCommand buCmd = new SqlCommand();
                    buCmd.Connection = buConn;
                    buConn.Open();

                    buCmd.CommandText = "DELETE FROM [ADMIN].UserDepartment WHERE LoginName = @LoginName";
                    buCmd.Parameters.Clear();
                    buCmd.Parameters.AddWithValue("@LoginName", loginName);
                    buCmd.ExecuteNonQuery();


                    if (dt.Rows.Count > 0)
                    {
                        buCmd.CommandText = "INSERT INTO [ADMIN].UserDepartment (LoginName, DepCode)";
                        buCmd.CommandText += "VALUES (@LoginName, @DepCode)";
                        buCmd.Parameters.Clear();
                        buCmd.Parameters.AddWithValue("@LoginName", loginName);

                        buCmd.Parameters.AddWithValue("@DepCode", dt.Rows[0]["DepCode"].ToString());
                        buCmd.ExecuteNonQuery();
                    }

                    buConn.Close();



                }

        }

        return message;
    }

    private void Cancel()
    {
        Response.Redirect("UserManage.aspx?mode=VIEW&USER=" + TextLoginName.Text + "");
    }

    private void DeleteUser(string loginName)
    {
        ConnectToDatabase("");

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = _conn;
        cmd.CommandText = "SELECT BuCode FROM dbo.BuUser WHERE LoginName = @LoginName";
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string buCode = reader["BuCode"].ToString();
            ConnectToDatabase(buCode);
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.Connection = _conn;

            cmdDel.CommandText = "DELETE FROM [ADMIN].[UserRole] WHERE LoginName = @LoginName";
            cmdDel.Parameters.AddWithValue("@LoginName", loginName);
            cmdDel.ExecuteNonQuery();

            cmdDel.CommandText = "DELETE FROM [ADMIN].[UserStore] WHERE LoginName = @LoginName";
            cmdDel.Parameters.Clear();
            cmdDel.Parameters.AddWithValue("@LoginName", loginName);
            cmdDel.ExecuteNonQuery();

        }
        reader.Close();

        ConnectToDatabase("");
        cmd.Connection = _conn;
        cmd.CommandText = "DELETE FROM dbo.[BuUser] WHERE LoginName=@LoginName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        cmd.ExecuteNonQuery();

        cmd.CommandText = "DELETE FROM dbo.[User] WHERE LoginName=@LoginName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        cmd.ExecuteNonQuery();
    }

    private void InactiveUser(string loginName)
    {
        ConnectToDatabase("");

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = _conn;
        cmd.CommandText = "UPDATE dbo.[User] SET IsActive=0 WHERE LoginName = @LoginName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@LoginName", loginName);
        cmd.ExecuteNonQuery();
    }

    #endregion // User Definition

    // -------------------------------------------------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------------------------------

    protected override void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Remove("dsUserDepartment");
            Session.Remove("BuCode");
            dsUserDepartment = null;

            ResetVariables();

            if (Request.QueryString["mode"] == null)
                _mode = "CREATE";
            else
            {
                _mode = Request.QueryString["mode"];

                if (Request.QueryString["user"] == null)
                    _loginName = string.Empty;
                else
                    _loginName = Request.QueryString["user"].ToString().Trim();
            }

            SetControlMode(_mode);

            if (_mode.ToUpper() != "CREATE")
            {
                //Bind_ddl_Department();
                GetUserInfo(_loginName);
            }
            else
                GetNewUser();

        }  // if (!IsPostBack)
        else
        {
            dsUserDepartment = (DataSet)Session["dsUserDepartment"];

            string Password = TextPassword.Text;
            TextPassword.Attributes.Add("value", Password);
            Password = TextPasswordConfirm.Text;
            TextPasswordConfirm.Attributes.Add("value", Password);
        }

    }

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (IsEdit())
        {
            ResetVariables();
            Response.Redirect("UserProfile.aspx?mode=EDIT&user=" + TextLoginName.Text);
        }
        else
            AlertBox("No permission to edit");
    }

    protected void BtnChangePassword_Click(object sender, EventArgs e)
    {
        if (IsEdit())
        {
            LabelChangePasswordAlert.Text = "";
            Pop_ChangePassword.Show();
        }
        else
            AlertBox("No permission to edit");
    }

    protected void BtnPopChangePasswordSave_Click(object sender, EventArgs e)
    {
        if (TextChangePassword.Text != string.Empty && TextChangePassword.Text == TextChangePasswordConfirm.Text)
        {
            string messageError = CheckPasswordCondition(TextChangePassword.Text);
            if (messageError == string.Empty)
            {
                UpdatePassword(TextLoginName.Text, TextChangePassword.Text);
                AlertBox("Change password successfully.");
            }
            else
            {
                //lbl_PasswordError.Text = messageError;
                LabelChangePasswordAlert.Text = messageError;
                Pop_ChangePassword.Show();

            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "pop_Password_Close", "alert('Password is saved.'); $find('pop_ResetPassword').hide();", true);
        }
        else if (TextChangePassword.Text != TextChangePasswordConfirm.Text)
        {
            LabelChangePasswordAlert.Text = "Password does not match.";
            Pop_ChangePassword.Show();
        }
        else
        {
            LabelChangePasswordAlert.Text = "Invalid password or password is empty.";
            Pop_ChangePassword.Show();
        }

    }

    protected void BtnDelUser_Click(object sender, EventArgs e)
    {
        if (IsDelete())
            Pop_DelUserConfirm.Show();
        else
            AlertBox("No permission to delete.");
    }

    protected void BtnPopDelUserConfirmYes_Click(object sender, EventArgs e)
    {
        DeleteUser(TextLoginName.Text);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Delelete", "parent.$find('pop_UserInfo').hide();window.top.location.reload();", true);
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        // Save current state of selected BU (Role and Location)
        if (ListBU.SelectedIndex > -1)
            SaveSelectionOfRoleAndLocationToDataTable(ListBU.SelectedItem.Text);


        string message = SaveUser(TextLoginName.Text);  // return string.Empty if no any error

        if (message != string.Empty)
        {
            AlertBox(message);
        }
        else
            Response.Redirect("UserProfile.aspx?mode=VIEW&USER=" + TextLoginName.Text + "");

    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        if (_mode.ToUpper() == "CREATE")
            Response.Redirect("UserProfile.aspx?mode=CREATE");
        else
            Response.Redirect("UserProfile.aspx?mode=VIEW&user=" + TextLoginName.Text);
    }

    protected void BtnBUAdd_Click(object sender, EventArgs e)
    {
        GetBuList();
        Pop_BuAdd.Show();
    }

    protected void BtnPopBuAddOk_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListPopBuAdd.Items.Count; i++)
        {
            if (ListPopBuAdd.Items[i].Selected)
                AddBuToList(ListPopBuAdd.Items[i].Value.ToString());
        }


    }

    protected void BtnBUDel_Click(object sender, EventArgs e)
    {
        Pop_BuDel.Show();
    }

    protected void BtnPopBuDelYes_Click(object sender, EventArgs e)
    {
        RemoveBuFromList(ListBU.SelectedItem.Text);

        if (ListBU.Items.Count == 0)
        {
            ListRole.Items.Clear();
            ListLocation.Items.Clear();
        }
        else
        {
            ListBU.SelectedIndex = 0;
        }

    }

    protected void ListBU_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBU.SelectedIndex > -1)
        {
            string buCode = ListBU.SelectedItem.Value.ToString();
            GetUserDepartmentDataSet(buCode, _loginName);

            if (_mode.ToUpper() != "VIEW" && Session["BuCode"] != null)
            {
                string oldBuCode = (string)Session["BuCode"];
                SaveSelectionOfRoleAndLocationToDataTable(oldBuCode);
                //Bind_ddl_Department();
            }
            Session["BuCode"] = buCode;
            GetRoleAndLocationOfUser(buCode, _loginName);

        }
    }


    protected void DdlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlDepartment.SelectedItem != null)
        {

            string buCode = ListBU.SelectedItem.Value.ToString();
            string depCode = DdlDepartment.SelectedValue.ToString();

            if (dsUserDepartment.Tables[buCode].Rows.Count == 0)
            {
                DataTable dt = dsUserDepartment.Tables[buCode];
                DataRow dr = dt.NewRow();
                dr["DepCode"] = depCode;
                dt.Rows.InsertAt(dr, 0);
            }
            else
            {
                dsUserDepartment.Tables[buCode].Rows[0]["DepCode"] = depCode;

            }

        }
    }

    protected void BtnRoleSelAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListRole.Items.Count; i++)
        {
            ListRole.Items[i].Selected = true;
        }
    }

    protected void BtnRoleSelNone_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListRole.Items.Count; i++)
        {
            ListRole.Items[i].Selected = false;
        }
    }

    protected void BtnLocationSelAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListLocation.Items.Count; i++)
        {
            ListLocation.Items[i].Selected = true;
        }
    }

    protected void BtnLocationSelNone_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < ListLocation.Items.Count; i++)
        {
            ListLocation.Items[i].Selected = false;
        }
    }

    // Added on: 18/01/2018, By: Fon
    #region About Department
    protected DataTable Get_vDepartment()
    {
        // Modified on: 29/03/2018,
        //string connetionString = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
        //SqlConnection cnn = new SqlConnection(connetionString);
        //try
        //{
        //    cnn.Open();
        //    string strsql = string.Format(@" SELECT * FROM [dbo].[vDepartment] ");
        //    SqlCommand myCommand = new SqlCommand(strsql, cnn);
        //    SqlDataAdapter da = new SqlDataAdapter(myCommand);
        //    DataTable dtDep = new DataTable();
        //    cnn.Close();

        //    da.Fill(dtDep);
        //    return dtDep;

        //}

        string buCode = string.Empty;
        if (Session["BuCode"] != null) buCode = Session["BuCode"].ToString();
        if (ListBU.Items.Count > 0) buCode = ListBU.SelectedItem.Text;

        string connetionString = GetSqlConnectionString(buCode);
        SqlConnection cnn = new SqlConnection(connetionString);
        try
        {
            cnn.Open();
            string strsql = string.Format(@" SELECT * FROM [ADMIN].[Department] ");
            SqlCommand myCommand = new SqlCommand(strsql, cnn);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            DataTable dtDep = new DataTable();

            da.Fill(dtDep);
            cnn.Close();
            return dtDep;
        }

        catch (Exception ex)
        {
            cnn.Close();
            //LabelUserName.Text = string.Format("{0}", ex.Message);
            return null;
        }
        // End Modified.
    }

    //protected void Bind_ddl_Department()
    //{
    //    DropDownList ddl_Department = (DropDownList)DdlDepartment;
    //    var deptList = Get_vDepartment();
    //    ddl_Department.DataSource = deptList;
    //    ddl_Department.DataTextField = "DepName";
    //    ddl_Department.DataValueField = "DepCode";
    //    ddl_Department.DataBind();

    //    ddl_Department.Items.Insert(0, new ListItem(string.Empty, string.Empty));
    //    //ddl_Department.SelectedIndex = 0;
    //}

    #endregion

    protected void TextLoginName_TextChanged(object sender, EventArgs e)
    {
        TextBox txt_Login = (TextBox)sender;
        lbl_errLogin.Visible = false;

        if (txt_Login.Text.Contains("@") && txt_Login.Text.Contains(".com"))
        {
            txt_Login.Text = string.Empty;
            lbl_errLogin.Text = "Cannot use email.";
            lbl_errLogin.Visible = true;
        }
    }
    // End Added.

    private string CheckPasswordCondition(string password)
    {
        string messageError = string.Empty;

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnStr"].ToString());
        con.Open();
        string sql = "SELECT * FROM dbo.[Config]";
        SqlCommand cmd = new SqlCommand(sql, con);
        SqlDataReader reader = cmd.ExecuteReader();


        int passwordLength = 0;
        bool isComplexity = false;

        if (reader.HasRows)
            while (reader.Read())
            {
                string key = reader["Key"].ToString().ToLower();
                string val = reader["Value"].ToString();

                if (key == "password.length")
                    passwordLength = Convert.ToInt32(val);
                else if (key == "password.complexity")
                    isComplexity = val.ToLower() == "true" || val == "1";
            }

        reader.Close();
        //con.Close();


        if (password.Length < passwordLength)
            messageError = string.Format("Password must be of minimum {0} characters length", passwordLength.ToString());

        if (isComplexity)
        {
            if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) == -1)
                messageError = "Password must contain at least one symbol";

            if (!password.Any(c => char.IsDigit(c)))
                messageError = "Password must contain at least one number";

            if (!(password.Any(c => char.IsUpper(c)) && password.Any(c => char.IsLower(c))))
                messageError = "Password must contain lowercase and uppercase";

        }


        return messageError;
    }

}