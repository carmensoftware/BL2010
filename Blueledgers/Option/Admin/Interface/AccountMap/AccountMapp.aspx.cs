using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using BlueLedger.PL.BaseClass;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        //private string MsgError = string.Empty;

        private DataTable dtAccountMapp = new DataTable();
        private DataTable dtView = new DataTable();
        private string lastViewName = string.Empty;

        private DataTable dtType
        {
            get
            {
                if (ViewState["dtType"] == null)
                    ViewState["dtType"] = new DataTable();
                return (DataTable)ViewState["dtType"];
            }
            set
            {
                ViewState["dtType"] = value;
            }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                lbl_Message.Text = string.Empty;
                pop_ImportExport.ShowOnPageLoad = false;
                Session.Remove("dtAccountMapp");
                Session.Remove("dtView");

                Page_Retrieve();
                txtSearch.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
                Session["IsEditBarClick"] = "0";
                Session["IsEditViewName"] = "0"; // 0: New, 1: Edit
            }
            else
            {

                Session["IsEditBarClick"] = (string)Session["IsEditBarClick"];
                Session["IsEditViewName"] = (string)Session["IsEditViewName"];
                dtAccountMapp = (DataTable)Session["dtAccountMapp"];
                dtView = (DataTable)Session["dtView"];
            }

        }

        private void Page_Retrieve()
        {
            Bind_ddlViewName();
            Page_Setting();
        }

        private void Page_Setting()
        {
            pnViewDetail.Visible = false;

            if (ddlViewName.SelectedValue.ToString() == "")
            {   //For sometime that U delete all row in SQL DataBase
                InsertMainKeyView();
                Response.Redirect("AccountMapp.aspx");
            }
            OutLook_AccountMappWithType(dtView);
            Session["dtView"] = (DataTable)dtView;
        }

        private void Bind_ddlViewName()
        {
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            string sql = "SELECT * FROM [ADMIN].[AccountMappView] ORDER BY ViewName";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter daView = new SqlDataAdapter(cmd);
            try
            {
                con.Open();

                dtView.Clear();
                daView.Fill(dtView);

                ddlViewName.DataSource = dtView;
                ddlViewName.DataTextField = "ViewName";
                ddlViewName.DataValueField = "ID";
                ddlViewName.DataBind();

                con.Close();
            }
            catch
            {
            }

        }

        protected void InsertMainKeyView()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);

            string strSql = string.Format(@"insert into [ADMIN].[AccountMappView] 
                (ViewName, BusinessUnitCode, StoreCode, CategoryCode, SubCategoryCode, ItemGroupCode, 
                KeyA1, KeyA2, KeyA3, KeyA4, KeyA5, KeyA6, KeyA7, KeyA8, KeyA9,
                A1, A2, A3, A4, A5, A6, A7, A8, A9, PostType) 
                values( 'Main Key', 1, 1, 1, 1, 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 'AP'); ");

            try
            {
                cnn.Open();
                SqlCommand insertcmd = new SqlCommand(strSql, cnn);
                insertcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { Response.Write(ex); }
            cnn.Close();
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "EDIT": //2016-10-05 Add case Edit on Menu_CmdBar
                        BarEditOnClick();
                        break;

                    case "SAVE":
                        saveToDatatable(); //2016-10-10 Add to DataTable.
                        BarSaveOnClick(); //2016-10-05 Save to DataBase.
                        BarBackOnClick();
                        break;

                    case "PRINT":
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                        break;

                    case "BACK":
                        BarBackOnClick();
                        break;

                    case "IMPORT":
                        BarImportOnClick();
                        break;

                    // Added on: 23/11/2017, For: New feature(Get new account-Mapping)
                    case "GETNEW":
                        GetApiData();

                        string errMsg = string.Empty;
                        bool returnExe = EXEC_spGetNewAccountMapping(ref errMsg);
                        
                        if (!returnExe)
                        {
                            // lbl_Warning.Text = errMsg;
                            // pop_Warning.ShowOnPageLoad = true;
                        }

                        Response.Redirect("AccountMapp.aspx");
                        break;
                    // End Added.
                }
            }
        }

        private void GetApiData()
        {
            string values = config.GetValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);

            lbl_Message.Text = string.Empty;

            if (values != string.Empty)
            {
                var keys = new KeyValues();

                keys.Text = values;

                string intfType = keys.Value("type");

                if (intfType.ToLower() == "api")
                {
                    //string apiAuth = keys.Value("auth");
                    string apiAccount = keys.Value("accountcode");
                    string apiDepartment = keys.Value("department");

                    string baseUrl = keys.Value("host");
                    string auth = keys.Value("auth"); //"direct 5d86b2f7dfcf74be49800a860510a365|32d256d1-e073-431a-8130-b0f113fcc15c";

                    try
                    {
                        var api = new API(baseUrl, auth);

                        // Account Code
                        var jsonAcc = api.Get(apiAccount);

                        //lbl_Message.Text = jsonAcc;

                        var account = JsonConvert.DeserializeObject<RootAcc>(jsonAcc);

                        config.DbParseQuery("DELETE FROM [ADMIN].AccountCode;", null, LoginInfo.ConnStr);

                        foreach (DataAcc item in account.Data)
                        {
                            string sql = " INSERT INTO [ADMIN].AccountCode (Id, AccCode, AccDesc1, AccDesc2, AccNature, AccType, IsActive, UpdateDate, UpdateBy)";
                            sql += string.Format(" VALUES({0},'{1}',N'{2}',N'{3}','{4}','{5}','{6}','{7}','{8}')",
                                    item.Id,
                                    item.AccCode,
                                    item.Description,
                                    item.Description2,
                                    item.Nature,
                                    item.Type,
                                    item.Active,
                                    item.LastModified.ToString("yyyy-MM-dd hh:mm:ss"),
                                    item.UserModified
                                );

                            config.DbParseQuery(sql, null, LoginInfo.ConnStr);
                        }

                        // Account Department
                        var jsonDep = api.Get(apiDepartment);

                        var department = JsonConvert.DeserializeObject<RootDept>(jsonDep);

                        config.DbParseQuery("DELETE FROM [ADMIN].AccountDepartment;", null, LoginInfo.ConnStr);

                        foreach (DataDept item in department.Data)
                        {
                            string sql = " INSERT INTO [ADMIN].AccountDepartment (Id, DeptCode, DeptDesc, UpdateDate, UpdateBy)";
                            sql += string.Format(" VALUES({0},'{1}','{2}','{3}','{4}')",
                                    item.Id,
                                    item.DeptCode,
                                    item.Description,
                                    item.LastModified.ToString("yyyy-MM-dd hh:mm:ss"),
                                    item.UserModified
                                );

                            config.DbParseQuery(sql, null, LoginInfo.ConnStr);
                        }

                    }
                    catch (Exception ex)
                    {
                        //lbl_Message.Text = "Error : " + ex.Message;
                        //throw new Exception(ex.Message);
                        //Response.Write(string.Format("<script>alert(`{0}`);</script>", ex.Message));
                    }

                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri(baseUrl);
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    client.DefaultRequestHeaders.Add("Authorization", auth);

                    //    // Account Code
                    //    HttpResponseMessage respAcc = client.GetAsync(keys.Value("accountcode")).Result;


                    //    if (respAcc.IsSuccessStatusCode)
                    //    {
                    //        var json = respAcc.Content.ReadAsStringAsync().Result;

                    //        var obj = JsonConvert.DeserializeObject<RootAcc>(json);

                    //        config.DbParseQuery("DELETE FROM [ADMIN].AccountCode;", null, LoginInfo.ConnStr);

                    //        foreach (DataAcc item in obj.Data)
                    //        {
                    //            string sql = " INSERT INTO [ADMIN].AccountCode (Id, AccCode, AccDesc1, AccDesc2, AccNature, AccType, IsActive, UpdateDate, UpdateBy)";
                    //            sql += string.Format(" VALUES({0},'{1}',N'{2}',N'{3}','{4}','{5}','{6}','{7}','{8}')",
                    //                    item.Id,
                    //                    item.AccCode,
                    //                    item.Description,
                    //                    item.Description2,
                    //                    item.Nature,
                    //                    item.Type,
                    //                    item.Active,
                    //                    item.LastModified.ToString("yyyy-MM-dd hh:mm:ss"),
                    //                    item.UserModified
                    //                );

                    //            config.DbParseQuery(sql, null, LoginInfo.ConnStr);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //Console.WriteLine("Internal server Error");
                    //        Response.Write("<script>alert('Internal Error');</script>");
                    //    }

                    //    // Account Department
                    //    HttpResponseMessage respDept = client.GetAsync(keys.Value("department")).Result;

                    //    if (respDept.IsSuccessStatusCode)
                    //    {
                    //        var json = respDept.Content.ReadAsStringAsync().Result;

                    //        var obj = JsonConvert.DeserializeObject<RootDept>(json);

                    //        config.DbParseQuery("DELETE FROM [ADMIN].AccountDepartment;", null, LoginInfo.ConnStr);

                    //        foreach (DataDept item in obj.Data)
                    //        {
                    //            string sql = " INSERT INTO [ADMIN].AccountDepartment (Id, DeptCode, DeptDesc, UpdateDate, UpdateBy)";
                    //            sql += string.Format(" VALUES({0},'{1}','{2}','{3}','{4}')",
                    //                    item.Id,
                    //                    item.DeptCode,
                    //                    item.Description,
                    //                    item.LastModified.ToString("yyyy-MM-dd hh:mm:ss"),
                    //                    item.UserModified
                    //                );

                    //            config.DbParseQuery(sql, null, LoginInfo.ConnStr);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Internal server Error");
                    //    }



                    //}
                }
                else if (intfType.ToLower() == "server")
                {
                    string connStr = keys.Value("ConnectionString");
                }




            }



            //throw new NotImplementedException();
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void BarEditOnClick()
        {
            //menu_CmdBar.Items[1].Visible = true;
            //menu_CmdBar.Items[3].Visible = true;
            //menu_CmdBar.Items[0].Visible = false;
            menu_CmdBar.Items.FindByName("Edit").Visible = false;
            menu_CmdBar.Items.FindByName("Save").Visible = true;
            menu_CmdBar.Items.FindByName("Back").Visible = true;
            menu_CmdBar.Items.FindByName("Import").Visible = false;
            menu_CmdBar.Items.FindByName("GetNew").Visible = false;

            btnCreateView.Visible = false;
            btnEditView.Visible = false;
            ddlViewName.Enabled = false;
            txtSearch.Enabled = false;

            Session["IsEditBarClick"] = "1";
            if (txtSearch.Text == "")
            {
                OutLook_AccountMappWithType(dtView);
            }
            else
            {
                filterByText((DataTable)Session["dtAccountMapp"], txtSearch.Text);
                SetAccountMappByView(dtView);
            }
        }

        protected void BarBackOnClick()
        {
            btnCreateView.Visible = true;
            btnEditView.Visible = true;

            menu_CmdBar.Items.FindByName("Save").Visible = false;
            menu_CmdBar.Items.FindByName("Back").Visible = false;
            menu_CmdBar.Items.FindByName("Edit").Visible = true;
            menu_CmdBar.Items.FindByName("Import").Visible = true;
            menu_CmdBar.Items.FindByName("GetNew").Visible = true;

            ddlViewName.Enabled = true;
            txtSearch.Enabled = true;
            Bind_AccountMapp();
            Session["IsEditBarClick"] = "0";
            if (txtSearch.Text != "")
            {
                filterByText((DataTable)Session["dtAccountMapp"], txtSearch.Text);
                SetAccountMappByView(dtView);
            }
            else
            { OutLook_AccountMappWithType(dtView); }
        }

        protected void BarSaveOnClick()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);

            int rowIndex = getRowIndexViewByID(dtView, ddlViewName.SelectedValue.ToString());
            string postType = (string)dtView.Rows[rowIndex]["PostType"];
            DataTable dtAccountMapp = (DataTable)Session["dtAccountMapp"];
            try
            {
                cnn.Open();

                foreach (DataRow row in dtAccountMapp.GetChanges().Rows)
                {
                    string strSql = "update [Admin].accountMapp ";
                    strSql += "Set A1=@A1, A2=@A2, A3=@A3, A4=@A4, A5=@A5, A6=@A6, A7=@A7, A8=@A8, A9=@A9 ";
                    strSql += "where PostType = '" + postType + "' ";
                    strSql += "AND ID = '" + row["ID"].ToString() + "'";
                    SqlCommand cmd = new SqlCommand(strSql, cnn);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@A1", row["A1"]);
                    cmd.Parameters.AddWithValue("@A2", row["A2"]);
                    cmd.Parameters.AddWithValue("@A3", row["A3"]);
                    cmd.Parameters.AddWithValue("@A4", row["A4"]);
                    cmd.Parameters.AddWithValue("@A5", row["A5"]);
                    cmd.Parameters.AddWithValue("@A6", row["A6"]);
                    cmd.Parameters.AddWithValue("@A7", row["A7"]);
                    cmd.Parameters.AddWithValue("@A8", row["A8"]);
                    cmd.Parameters.AddWithValue("@A9", row["A9"]);
                    cmd.ExecuteNonQuery();
                }

                cnn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void btnSaveView_Click(object sender, EventArgs e)
        {
            checkBeforeSave();
        }

        protected void checkBeforeSave()
        {
            if (txtViewName.Text != "")
            {
                if (cblValue.SelectedItem != null)
                {
                    string msgError = string.Empty;
                    bool returnSave = saveView(ref msgError);
                    if (!returnSave)
                    {
                        lbl_Warning.Text = msgError;
                        pop_Warning.ShowOnPageLoad = true;
                    }
                    else
                        Response.Redirect("AccountMapp.aspx");
                }
                else
                {
                    lbl_Warning.Text = "Field is required." + "<br/>" + "You have to choose value.";
                    pop_Warning.ShowOnPageLoad = true;
                    //clearFiled();
                }
            }
            else
            {
                lbl_Warning.Text = "Field is required." + "<br/>" + "You have to fill View Name.";
                pop_Warning.ShowOnPageLoad = true;
                //clearFiled();
            }
        }

        protected bool saveView(ref string msgErr)
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                int rowIndex = getRowIndexViewByID(dtView, ddlViewName.SelectedValue.ToString());
                string strSql = string.Empty;
                if ((string)Session["IsEditViewName"] == "0")
                {
                    // Modified on: 23/11/2017, For: Add new KeyA1 - KeyA9
                    strSql = string.Format(@"insert into [ADMIN].[AccountMappView] 
                        (ViewName, BusinessUnitCode, StoreCode, CategoryCode, SubCategoryCode, ItemGroupCode, 
                        KeyA1, KeyA2, KeyA3, KeyA4, KeyA5, KeyA6, KeyA7, KeyA8, KeyA9,
                        DescKeyA1, DescKeyA2, DescKeyA3, DescKeyA4, DescKeyA5, DescKeyA6, DescKeyA7, DescKeyA8, DescKeyA9,
                        A1, A2, A3, A4, A5, A6, A7, A8, A9, PostType, 
                        TypeA1, TypeA2, TypeA3, TypeA4, TypeA5, TypeA6, TypeA7, TypeA8, TypeA9, 
                        DescA1, DescA2, DescA3, DescA4, DescA5, DescA6, DescA7, DescA8, DescA9) 
                        
                        values( @ViewName, @BusinessUnitCode, @StoreCode, @CategoryCode, @SubCategoryCode, @ItemGroupCode, 
                        @KeyA1, @KeyA2, @KeyA3, @KeyA4, @KeyA5, @KeyA6, @KeyA7, @KeyA8, @KeyA9,
                        @DescKeyA1, @DescKeyA2, @DescKeyA3, @DescKeyA4, @DescKeyA5, @DescKeyA6, @DescKeyA7, @DescKeyA8, @DescKeyA9,
                        
                        @A1, @A2, @A3, @A4, @A5, @A6, @A7, @A8, @A9, @PostType, 
                        @TypeA1, @TypeA2, @TypeA3, @TypeA4, @TypeA5, @TypeA6, @TypeA7, @TypeA8, @TypeA9,
                        @DescA1, @DescA2, @DescA3, @DescA4, @DescA5, @DescA6, @DescA7, @DescA8, @DescA9); ");

                }

                if ((string)Session["IsEditViewName"] == "1")
                {
                    // Modified on: 23/11/2017, For: Add new KeyA1 - KeyA9
                    strSql = string.Format(@"update [ADMIN].[AccountMappView] 
                        set ViewName=@ViewName, BusinessUnitCode=@BusinessUnitCode, 
                        StoreCode=@StoreCode, CategoryCode=@CategoryCode, 
                        SubCategoryCode=@SubCategoryCode, ItemGroupCode=@ItemGroupCode, 
                        
                        KeyA1=@KeyA1, KeyA2=@KeyA2, KeyA3=@KeyA3, KeyA4=@KeyA4, KeyA5=@KeyA5, 
                        KeyA6=@KeyA6, KeyA7=@KeyA7, KeyA8=@KeyA8, KeyA9=@KeyA9,
                        DescKeyA1=@DescKeyA1, DescKeyA2=@DescKeyA2, DescKeyA3=@DescKeyA3, DescKeyA4=@DescKeyA4, DescKeyA5=@DescKeyA5, 
                        DescKeyA6=@DescKeyA6, DescKeyA7=@DescKeyA7, DescKeyA8=@DescKeyA8, DescKeyA9=@DescKeyA9,                        

                        A1=@A1, A2=@A2, A3=@A3, A4=@A4, A5=@A5, A6=@A6, A7=@A7, A8=@A8, A9=@A9, PostType=@PostType, 
                        TypeA1=@TypeA1, TypeA2=@TypeA2, TypeA3=@TypeA3, TypeA4=@TypeA4, TypeA5=@TypeA5, TypeA6=@TypeA6, TypeA7=@TypeA7, TypeA8=@TypeA8, TypeA9=@TypeA9,  
                        DescA1=@DescA1, DescA2=@DescA2, DescA3=@DescA3, DescA4=@DescA4, DescA5=@DescA5, 
                        DescA6=@DescA6, DescA7=@DescA7, DescA8=@DescA8, DescA9=@DescA9 
                        where ID= '{0}'", dtView.Rows[rowIndex]["ID"]);
                }

                SqlCommand insertcmd = new SqlCommand(strSql, cnn);
                insertcmd.Parameters.Clear();
                insertcmd.Parameters.AddWithValue("@ViewName", txtViewName.Text);
                insertcmd.Parameters.AddWithValue("@BusinessUnitCode", cblView.Items[0].Selected);
                insertcmd.Parameters.AddWithValue("@StoreCode", cblView.Items[1].Selected);
                insertcmd.Parameters.AddWithValue("@CategoryCode", cblView.Items[2].Selected);
                insertcmd.Parameters.AddWithValue("@SubCategoryCode", cblView.Items[3].Selected);
                insertcmd.Parameters.AddWithValue("@ItemGroupCode", cblView.Items[4].Selected);

                // Added on: 23/11/2017
                insertcmd.Parameters.AddWithValue("@KeyA1", cblView_KeyA.Items.FindByValue("KeyA1").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA2", cblView_KeyA.Items.FindByValue("KeyA2").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA3", cblView_KeyA.Items.FindByValue("KeyA3").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA4", cblView_KeyA.Items.FindByValue("KeyA4").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA5", cblView_KeyA.Items.FindByValue("KeyA5").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA6", cblView_KeyA.Items.FindByValue("KeyA6").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA7", cblView_KeyA.Items.FindByValue("KeyA7").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA8", cblView_KeyA.Items.FindByValue("KeyA8").Selected);
                insertcmd.Parameters.AddWithValue("@KeyA9", cblView_KeyA.Items.FindByValue("KeyA9").Selected);
                // End Modified.

                insertcmd.Parameters.AddWithValue("@A1", cblValue.Items[0].Selected);
                insertcmd.Parameters.AddWithValue("@A2", cblValue.Items[1].Selected);
                insertcmd.Parameters.AddWithValue("@A3", cblValue.Items[2].Selected);
                insertcmd.Parameters.AddWithValue("@A4", cblValue.Items[3].Selected);
                insertcmd.Parameters.AddWithValue("@A5", cblValue.Items[4].Selected);
                insertcmd.Parameters.AddWithValue("@A6", cblValue.Items[5].Selected);
                insertcmd.Parameters.AddWithValue("@A7", cblValue.Items[6].Selected);
                insertcmd.Parameters.AddWithValue("@A8", cblValue.Items[7].Selected);
                insertcmd.Parameters.AddWithValue("@A9", cblValue.Items[8].Selected);
                insertcmd.Parameters.AddWithValue("@PostType", ddlPostType.SelectedItem.ToString());

                insertcmd.Parameters.AddWithValue("@TypeA1", ddlTxtType1.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA2", ddlTxtType2.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA3", ddlTxtType3.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA4", ddlTxtType4.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA5", ddlTxtType5.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA6", ddlTxtType6.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA7", ddlTxtType7.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA8", ddlTxtType8.SelectedValue.ToString());
                insertcmd.Parameters.AddWithValue("@TypeA9", ddlTxtType9.SelectedValue.ToString());

                // Do like these because of DBNull.Value != "". So U can sill display OLD HEADERTEXT!
                #region Key Description
                if (cblView_KeyA.Items.FindByValue("KeyA1").Selected && txtDescKeyA1.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA1", txtDescKeyA1.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA1", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA2").Selected && txtDescKeyA2.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA2", txtDescKeyA2.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA2", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA3").Selected && txtDescKeyA3.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA3", txtDescKeyA3.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA3", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA4").Selected && txtDescKeyA4.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA4", txtDescKeyA4.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA4", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA5").Selected && txtDescKeyA5.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA5", txtDescKeyA5.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA5", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA6").Selected && txtDescKeyA6.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA6", txtDescKeyA6.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA6", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA7").Selected && txtDescKeyA7.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA7", txtDescKeyA7.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA7", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA8").Selected && txtDescKeyA8.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA8", txtDescKeyA8.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA8", DBNull.Value);

                if (cblView_KeyA.Items.FindByValue("KeyA9").Selected && txtDescKeyA9.Text != "")
                    insertcmd.Parameters.AddWithValue("@DescKeyA9", txtDescKeyA9.Text);
                else insertcmd.Parameters.AddWithValue("@DescKeyA9", DBNull.Value);
                #endregion

                #region Value Description
                if (cblValue.Items.FindByValue("A1").Selected && txtDESCA1.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA1", txtDESCA1.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA1", DBNull.Value); }

                if (cblValue.Items.FindByValue("A2").Selected && txtDESCA2.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA2", txtDESCA2.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA2", DBNull.Value); }

                if (cblValue.Items.FindByValue("A3").Selected && txtDESCA3.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA3", txtDESCA3.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA3", DBNull.Value); }

                if (cblValue.Items.FindByValue("A4").Selected && txtDESCA4.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA4", txtDESCA4.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA4", DBNull.Value); }

                if (cblValue.Items.FindByValue("A5").Selected && txtDESCA5.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA5", txtDESCA5.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA5", DBNull.Value); }

                if (cblValue.Items.FindByValue("A6").Selected && txtDESCA6.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA6", txtDESCA6.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA6", DBNull.Value); }

                if (cblValue.Items.FindByValue("A7").Selected && txtDESCA7.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA7", txtDESCA7.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA7", DBNull.Value); }

                if (cblValue.Items.FindByValue("A8").Selected && txtDESCA8.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA8", txtDESCA8.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA8", DBNull.Value); }

                if (cblValue.Items.FindByValue("A9").Selected && txtDESCA9.Text != "")
                { insertcmd.Parameters.AddWithValue("@DescA9", txtDESCA9.Text); }
                else { insertcmd.Parameters.AddWithValue("@DescA9", DBNull.Value); }
                #endregion

                try
                {
                    insertcmd.ExecuteNonQuery();
                    cnn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    msgErr = ex.Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                msgErr = ex.Message;
                return false;
            }
        }

        protected void btnDeleteView_Click(object sender, EventArgs e)
        {
            clearFiled();
            deleteView();
            Response.Redirect("AccountMapp.aspx");
            pnViwBar.Visible = true;
            pnViewDetail.Visible = false;
            gvAccountMap.Visible = true;
        }

        protected void deleteView()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                string strSql = "DELETE FROM [ADMIN].[AccountMappView] Where ID = @ID";
                SqlCommand deletecmd = new SqlCommand(strSql, cnn);
                deletecmd.Parameters.Clear();
                deletecmd.Parameters.AddWithValue("@ID", ddlViewName.SelectedValue);
                deletecmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void btnCancelView_Click(object sender, EventArgs e)
        {
            clearFiled();
            pnViwBar.Visible = true;
            pnViewDetail.Visible = false;
            gvAccountMap.Visible = true;
            menu_CmdBar.Items.FindByName("Edit").Visible = true;
            menu_CmdBar.Items.FindByName("GetNew").Visible = true;
        }

        protected void clearFiled()
        {
            txtSearch.Text = null;
            txtViewName.Text = null;
            cblView.ClearSelection();
            cblView.Items[0].Selected = true;
            cblView_KeyA.ClearSelection();
            cblValue.ClearSelection();

            txtDescKeyA1.Text = string.Empty;
            txtDescKeyA2.Text = string.Empty;
            txtDescKeyA3.Text = string.Empty;
            txtDescKeyA4.Text = string.Empty;
            txtDescKeyA5.Text = string.Empty;
            txtDescKeyA6.Text = string.Empty;
            txtDescKeyA7.Text = string.Empty;
            txtDescKeyA8.Text = string.Empty;
            txtDescKeyA9.Text = string.Empty;

            txtDESCA1.Text = string.Empty;
            txtDESCA2.Text = string.Empty;
            txtDESCA3.Text = string.Empty;
            txtDESCA4.Text = string.Empty;
            txtDESCA5.Text = string.Empty;
            txtDESCA6.Text = string.Empty;
            txtDESCA7.Text = string.Empty;
            txtDESCA8.Text = string.Empty;
            txtDESCA9.Text = string.Empty;

            Session["IsEditViewName"] = "0";
        }

        protected void btnCreateView_Click(object sender, EventArgs e)
        {
            clearFiled();
            Session["IsEditViewName"] = "0";
            Bind_ddlPostType();

            pnViewDetail.Visible = true;
            pnViwBar.Visible = false;
            gvAccountMap.Visible = false;
            menu_CmdBar.Items.FindByName("Edit").Visible = false;
            menu_CmdBar.Items.FindByName("GetNew").Visible = false;
            btnDeleteView.Visible = false;
        }

        private void Bind_DdlTxtType()
        {
            ddlTxtType1.Items.Clear();

            ddlTxtType1.Items.Add(new ListItem("Text", string.Empty));
            ddlTxtType1.Items.Add(new ListItem("Account Code", "AccCode"));
            ddlTxtType1.Items.Add(new ListItem("Account Department", "AccDept"));


            DropdownCloneItems(ddlTxtType1, ddlTxtType2);
            DropdownCloneItems(ddlTxtType1, ddlTxtType3);
            DropdownCloneItems(ddlTxtType1, ddlTxtType4);
            DropdownCloneItems(ddlTxtType1, ddlTxtType5);
            DropdownCloneItems(ddlTxtType1, ddlTxtType6);
            DropdownCloneItems(ddlTxtType1, ddlTxtType7);
            DropdownCloneItems(ddlTxtType1, ddlTxtType8);
            DropdownCloneItems(ddlTxtType1, ddlTxtType9);

        }

        private void DropdownCloneItems(DropDownList ddlSourc, DropDownList ddlTarget)
        {
            ddlTarget.Items.Clear();

            foreach (ListItem item in ddlSourc.Items)
            {
                ListItem newItem = new ListItem();

                newItem.Value = item.Value;
                newItem.Text = item.Text;
                ddlTarget.Items.Add(newItem);
            }

        }


        protected void Bind_ddlPostType()
        {
            #region comment
            //string connetionString = LoginInfo.ConnStr;
            //SqlConnection cnn = new SqlConnection(connetionString);
            //string strsql = "select distinct posttype from [admin].accountmapp ";
            //strsql += "order by posttype ASC";
            //SqlCommand myCommand = new SqlCommand(strsql, cnn);
            //SqlDataAdapter da = new SqlDataAdapter(myCommand);
            //DataTable dtpostType = new DataTable();
            //dtpostType.Clear();
            //da.Fill(dtpostType);
            //ddlPostType.DataSource = dtpostType;
            //ddlPostType.DataTextField = "posttype";
            //ddlPostType.DataBind();
            #endregion

            //if (ddlPostType.Items.Count < 1)
            //{
            //    ddlPostType.Items.Add(new ListItem("AP", "AP"));
            //    ddlPostType.Items.Add(new ListItem("GL", "GL"));
            //}

            DataTable dt = config.DbExecuteQuery("SELECT '' as PostType UNION ALL SELECT DISTINCT PostType FROM [ADMIN].AccountMapp ORDER BY PostType", null, LoginInfo.ConnStr);
            ddlPostType.DataSource = dt;
            ddlPostType.DataValueField = "PostType";
            ddlPostType.DataTextField = "PostType";
            ddlPostType.DataBind();

        }

        protected void btnEditView_Click(object sender, EventArgs e)
        {
            clearFiled();
            Session["IsEditViewName"] = "1";
            pnViewDetail.Visible = true;
            pnViwBar.Visible = false;
            OutLook_AccountMappWithType(dtView);

            txtViewName.Text = ddlViewName.SelectedItem.ToString();
            gvAccountMap.Visible = false;
            menu_CmdBar.Items.FindByName("Edit").Visible = false;
            menu_CmdBar.Items.FindByName("GetNew").Visible = false;
            btnDeleteView.Visible = true;
        }

        protected void ddlViewName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OutLook_AccountMappWithType(dtView);
            txtSearch.Text = string.Empty;
        }

        protected void SetAccountMappByView(DataTable dt)
        {
            bool isEditBarClick = Convert.ToBoolean(Convert.ToInt32(Session["IsEditBarClick"]));
            bool isEditViewClick = Convert.ToBoolean(Convert.ToInt32(Session["IsEditViewName"]));

            int rowIndex = getRowIndexViewByID(dt, ddlViewName.SelectedValue.ToString());

            gvAccountMap.Columns[1].Visible = (bool)dtView.Rows[rowIndex]["BusinessUnitCode"];
            gvAccountMap.Columns[2].Visible = (bool)dtView.Rows[rowIndex]["StoreCode"];
            gvAccountMap.Columns[3].Visible = (bool)dtView.Rows[rowIndex]["CategoryCode"];
            gvAccountMap.Columns[4].Visible = (bool)dtView.Rows[rowIndex]["SubCategoryCode"];
            gvAccountMap.Columns[5].Visible = (bool)dtView.Rows[rowIndex]["ItemGroupCode"];

            // Added on: 23/11/2017, For: New key field.
            gvAccountMap.Columns[6].Visible = (bool)dtView.Rows[rowIndex]["KeyA1"];
            gvAccountMap.Columns[7].Visible = (bool)dtView.Rows[rowIndex]["KeyA2"];
            gvAccountMap.Columns[8].Visible = (bool)dtView.Rows[rowIndex]["KeyA3"];
            gvAccountMap.Columns[9].Visible = (bool)dtView.Rows[rowIndex]["KeyA4"];
            gvAccountMap.Columns[10].Visible = (bool)dtView.Rows[rowIndex]["KeyA5"];
            gvAccountMap.Columns[11].Visible = (bool)dtView.Rows[rowIndex]["KeyA6"];
            gvAccountMap.Columns[12].Visible = (bool)dtView.Rows[rowIndex]["KeyA7"];
            gvAccountMap.Columns[13].Visible = (bool)dtView.Rows[rowIndex]["KeyA8"];
            gvAccountMap.Columns[14].Visible = (bool)dtView.Rows[rowIndex]["KeyA9"];
            // End Added.

            // Set Column Header Text
            DataRow dr = dtView.Rows[rowIndex];
            gvAccountMap.Columns[15].HeaderText = dr["DescA1"].ToString();
            gvAccountMap.Columns[16].HeaderText = dr["DescA1"].ToString();
            gvAccountMap.Columns[17].HeaderText = dr["DescA2"].ToString();
            gvAccountMap.Columns[18].HeaderText = dr["DescA2"].ToString();
            gvAccountMap.Columns[19].HeaderText = dr["DescA3"].ToString();
            gvAccountMap.Columns[20].HeaderText = dr["DescA3"].ToString();
            gvAccountMap.Columns[21].HeaderText = dr["DescA4"].ToString();
            gvAccountMap.Columns[22].HeaderText = dr["DescA4"].ToString();
            gvAccountMap.Columns[23].HeaderText = dr["DescA5"].ToString();
            gvAccountMap.Columns[24].HeaderText = dr["DescA5"].ToString();
            gvAccountMap.Columns[25].HeaderText = dr["DescA6"].ToString();
            gvAccountMap.Columns[26].HeaderText = dr["DescA6"].ToString();
            gvAccountMap.Columns[27].HeaderText = dr["DescA7"].ToString();
            gvAccountMap.Columns[28].HeaderText = dr["DescA7"].ToString();
            gvAccountMap.Columns[29].HeaderText = dr["DescA8"].ToString();
            gvAccountMap.Columns[30].HeaderText = dr["DescA8"].ToString();
            gvAccountMap.Columns[31].HeaderText = dr["DescA9"].ToString();
            gvAccountMap.Columns[32].HeaderText = dr["DescA9"].ToString();

            /* When user click edit. */
            // Label
            gvAccountMap.Columns[15].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A1"];
            gvAccountMap.Columns[17].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A2"];
            gvAccountMap.Columns[19].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A3"];
            gvAccountMap.Columns[21].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A4"];
            gvAccountMap.Columns[23].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A5"];
            gvAccountMap.Columns[25].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A6"];
            gvAccountMap.Columns[27].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A7"];
            gvAccountMap.Columns[29].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A8"];
            gvAccountMap.Columns[31].Visible = (isEditBarClick) ? false : (bool)dtView.Rows[rowIndex]["A9"];

            // TextBox
            gvAccountMap.Columns[16].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A1"] : false;
            gvAccountMap.Columns[18].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A2"] : false;
            gvAccountMap.Columns[20].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A3"] : false;
            gvAccountMap.Columns[22].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A4"] : false;
            gvAccountMap.Columns[24].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A5"] : false;
            gvAccountMap.Columns[26].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A6"] : false;
            gvAccountMap.Columns[28].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A7"] : false;
            gvAccountMap.Columns[30].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A8"] : false;
            gvAccountMap.Columns[32].Visible = (isEditBarClick) ? (bool)dtView.Rows[rowIndex]["A9"] : false;


            if (isEditViewClick)
            {
                cblView.Items[0].Selected = (bool)dtView.Rows[rowIndex]["BusinessUnitCode"];
                cblView.Items[1].Selected = (bool)dtView.Rows[rowIndex]["StoreCode"];
                cblView.Items[2].Selected = (bool)dtView.Rows[rowIndex]["CategoryCode"];
                cblView.Items[3].Selected = (bool)dtView.Rows[rowIndex]["SubCategoryCode"];
                cblView.Items[4].Selected = (bool)dtView.Rows[rowIndex]["ItemGroupCode"];

                // Added on: 23/11/2017
                cblView_KeyA.Items.FindByValue("KeyA1").Selected = (bool)dtView.Rows[rowIndex]["KeyA1"];
                cblView_KeyA.Items.FindByValue("KeyA2").Selected = (bool)dtView.Rows[rowIndex]["KeyA2"];
                cblView_KeyA.Items.FindByValue("KeyA3").Selected = (bool)dtView.Rows[rowIndex]["KeyA3"];
                cblView_KeyA.Items.FindByValue("KeyA4").Selected = (bool)dtView.Rows[rowIndex]["KeyA4"];
                cblView_KeyA.Items.FindByValue("KeyA5").Selected = (bool)dtView.Rows[rowIndex]["KeyA5"];
                cblView_KeyA.Items.FindByValue("KeyA6").Selected = (bool)dtView.Rows[rowIndex]["KeyA6"];
                cblView_KeyA.Items.FindByValue("KeyA7").Selected = (bool)dtView.Rows[rowIndex]["KeyA7"];
                cblView_KeyA.Items.FindByValue("KeyA8").Selected = (bool)dtView.Rows[rowIndex]["KeyA8"];
                cblView_KeyA.Items.FindByValue("KeyA9").Selected = (bool)dtView.Rows[rowIndex]["KeyA9"];
                // End Added.

                cblValue.Items[0].Selected = (bool)dtView.Rows[rowIndex]["A1"];
                cblValue.Items[1].Selected = (bool)dtView.Rows[rowIndex]["A2"];
                cblValue.Items[2].Selected = (bool)dtView.Rows[rowIndex]["A3"];
                cblValue.Items[3].Selected = (bool)dtView.Rows[rowIndex]["A4"];
                cblValue.Items[4].Selected = (bool)dtView.Rows[rowIndex]["A5"];
                cblValue.Items[5].Selected = (bool)dtView.Rows[rowIndex]["A6"];
                cblValue.Items[6].Selected = (bool)dtView.Rows[rowIndex]["A7"];
                cblValue.Items[7].Selected = (bool)dtView.Rows[rowIndex]["A8"];
                cblValue.Items[8].Selected = (bool)dtView.Rows[rowIndex]["A9"];

                //If get DBNULL.Value then jump out.
                #region Key Description
                if (dtView.Rows[rowIndex]["DescKeyA1"] != DBNull.Value) txtDescKeyA1.Text = (string)dtView.Rows[rowIndex]["DescKeyA1"];
                if (dtView.Rows[rowIndex]["DescKeyA2"] != DBNull.Value) txtDescKeyA2.Text = (string)dtView.Rows[rowIndex]["DescKeyA2"];
                if (dtView.Rows[rowIndex]["DescKeyA3"] != DBNull.Value) txtDescKeyA3.Text = (string)dtView.Rows[rowIndex]["DescKeyA3"];
                if (dtView.Rows[rowIndex]["DescKeyA4"] != DBNull.Value) txtDescKeyA4.Text = (string)dtView.Rows[rowIndex]["DescKeyA4"];
                if (dtView.Rows[rowIndex]["DescKeyA5"] != DBNull.Value) txtDescKeyA5.Text = (string)dtView.Rows[rowIndex]["DescKeyA5"];
                if (dtView.Rows[rowIndex]["DescKeyA6"] != DBNull.Value) txtDescKeyA6.Text = (string)dtView.Rows[rowIndex]["DescKeyA6"];
                if (dtView.Rows[rowIndex]["DescKeyA7"] != DBNull.Value) txtDescKeyA7.Text = (string)dtView.Rows[rowIndex]["DescKeyA7"];
                if (dtView.Rows[rowIndex]["DescKeyA8"] != DBNull.Value) txtDescKeyA8.Text = (string)dtView.Rows[rowIndex]["DescKeyA8"];
                if (dtView.Rows[rowIndex]["DescKeyA9"] != DBNull.Value) txtDescKeyA9.Text = (string)dtView.Rows[rowIndex]["DescKeyA9"];
                #endregion
                #region Value description
                if (dtView.Rows[rowIndex]["DescA1"] != DBNull.Value) txtDESCA1.Text = (string)dtView.Rows[rowIndex]["DescA1"];
                if (dtView.Rows[rowIndex]["DescA2"] != DBNull.Value) txtDESCA2.Text = (string)dtView.Rows[rowIndex]["DescA2"];
                if (dtView.Rows[rowIndex]["DescA3"] != DBNull.Value) txtDESCA3.Text = (string)dtView.Rows[rowIndex]["DescA3"];
                if (dtView.Rows[rowIndex]["DescA4"] != DBNull.Value) txtDESCA4.Text = (string)dtView.Rows[rowIndex]["DescA4"];
                if (dtView.Rows[rowIndex]["DescA5"] != DBNull.Value) txtDESCA5.Text = (string)dtView.Rows[rowIndex]["DescA5"];
                if (dtView.Rows[rowIndex]["DescA6"] != DBNull.Value) txtDESCA6.Text = (string)dtView.Rows[rowIndex]["DescA6"];
                if (dtView.Rows[rowIndex]["DescA7"] != DBNull.Value) txtDESCA7.Text = (string)dtView.Rows[rowIndex]["DescA7"];
                if (dtView.Rows[rowIndex]["DescA8"] != DBNull.Value) txtDESCA8.Text = (string)dtView.Rows[rowIndex]["DescA8"];
                if (dtView.Rows[rowIndex]["DescA9"] != DBNull.Value) txtDESCA9.Text = (string)dtView.Rows[rowIndex]["DescA9"];
                #endregion


                #region Type
                Bind_DdlTxtType();
                if (dtView.Rows[rowIndex]["TypeA1"] != DBNull.Value) ddlTxtType1.SelectedValue = dtView.Rows[rowIndex]["TypeA1"].ToString();
                if (dtView.Rows[rowIndex]["TypeA2"] != DBNull.Value) ddlTxtType2.SelectedValue = dtView.Rows[rowIndex]["TypeA2"].ToString();
                if (dtView.Rows[rowIndex]["TypeA3"] != DBNull.Value) ddlTxtType3.SelectedValue = dtView.Rows[rowIndex]["TypeA3"].ToString();
                if (dtView.Rows[rowIndex]["TypeA4"] != DBNull.Value) ddlTxtType4.SelectedValue = dtView.Rows[rowIndex]["TypeA4"].ToString();
                if (dtView.Rows[rowIndex]["TypeA5"] != DBNull.Value) ddlTxtType5.SelectedValue = dtView.Rows[rowIndex]["TypeA5"].ToString();
                if (dtView.Rows[rowIndex]["TypeA6"] != DBNull.Value) ddlTxtType6.SelectedValue = dtView.Rows[rowIndex]["TypeA6"].ToString();
                if (dtView.Rows[rowIndex]["TypeA7"] != DBNull.Value) ddlTxtType7.SelectedValue = dtView.Rows[rowIndex]["TypeA7"].ToString();
                if (dtView.Rows[rowIndex]["TypeA8"] != DBNull.Value) ddlTxtType8.SelectedValue = dtView.Rows[rowIndex]["TypeA8"].ToString();
                if (dtView.Rows[rowIndex]["TypeA9"] != DBNull.Value) ddlTxtType9.SelectedValue = dtView.Rows[rowIndex]["TypeA9"].ToString();
                #endregion


                Bind_ddlPostType();

                if (ddlPostType.Items.FindByText(dtView.Rows[rowIndex]["PostType"].ToString()) != null)
                    ddlPostType.SelectedValue = (string)dtView.Rows[rowIndex]["PostType"];
                Control_cvlValueLookUp();
            }




        }

        private int getRowIndexViewByID(DataTable dtView, string id)
        {
            // int rowIndex = -1;
            // for (var i = 0; i < dtView.Rows.Count; i++)
            // {
            //     if (dtView.Rows[i]["ID"].ToString() == id)
            //     {
            //         rowIndex = i;
            //         break;
            //     }
            // }

            int rowIndex = ddlViewName.SelectedIndex;
            //lbl_Message.Text = "Row index = " + rowIndex.ToString();
            return rowIndex;

        }

        protected void OutLook_AccountMappWithType(DataTable dt)
        {
            SetAccountMappByView(dt);
            Bind_AccountMapp();
        }

        protected void gvAccountMap_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            int rowIndex = getRowIndexViewByID(dtView, ddlViewName.SelectedValue.ToString());

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                #region Key detail
                Label lblID = (Label)(e.Row.FindControl("lblID"));
                if (lblID != null)
                { lblID.Text = (string)DataBinder.Eval(e.Row.DataItem, "ID").ToString(); }

                Label lblSC = (Label)(e.Row.FindControl("lblSC"));
                if (lblSC != null)
                { lblSC.Text = (string)DataBinder.Eval(e.Row.DataItem, "StoreCode").ToString(); }
                Label lblSCdesc = (Label)(e.Row.FindControl("lblSCdesc"));
                if (lblSCdesc != null)
                { lblSCdesc.Text = (string)DataBinder.Eval(e.Row.DataItem, "LocationName").ToString(); }
                Label lblIGC = (Label)(e.Row.FindControl("lblIGC"));
                if (lblIGC != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "ItemGroupCode") != DBNull.Value)
                    { lblIGC.Text = (string)DataBinder.Eval(e.Row.DataItem, "ItemGroupCode"); }
                }
                Label lblIGCdesc = (Label)(e.Row.FindControl("lblIGCdesc"));
                if (lblIGCdesc != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "CategoryName03") != DBNull.Value)
                    { lblIGCdesc.Text = (string)DataBinder.Eval(e.Row.DataItem, "CategoryName03"); }
                }
                Label lblSCC = (Label)(e.Row.FindControl("lblSCC"));
                if (lblSCC != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "CategoryCode02") != DBNull.Value)
                    { lblSCC.Text = (string)DataBinder.Eval(e.Row.DataItem, "CategoryCode02"); }
                }
                Label lblSCCdesc = (Label)(e.Row.FindControl("lblSCCdesc"));
                if (lblSCCdesc != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "CategoryName02") != DBNull.Value)
                    { lblSCCdesc.Text = (string)DataBinder.Eval(e.Row.DataItem, "CategoryName02"); }
                }
                Label lblCC = (Label)(e.Row.FindControl("lblCC"));
                if (lblCC != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "CategoryCode01") != DBNull.Value)
                    { lblCC.Text = (string)DataBinder.Eval(e.Row.DataItem, "CategoryCode01"); }
                }
                Label lblCCdesc = (Label)(e.Row.FindControl("lblCCdesc"));
                if (lblCCdesc != null)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "CategoryName01") != DBNull.Value)
                    { lblCCdesc.Text = (string)DataBinder.Eval(e.Row.DataItem, "CategoryName01"); }
                }
                //End column Key Value
                #endregion

                #region Key Header
                if (dtView.Rows[rowIndex]["DescKeyA1"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[6].Text = (string)dtView.Rows[rowIndex]["DescKeyA1"];
                if (dtView.Rows[rowIndex]["DescKeyA2"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[7].Text = (string)dtView.Rows[rowIndex]["DescKeyA2"];
                if (dtView.Rows[rowIndex]["DescKeyA3"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[8].Text = (string)dtView.Rows[rowIndex]["DescKeyA3"];
                if (dtView.Rows[rowIndex]["DescKeyA4"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[9].Text = (string)dtView.Rows[rowIndex]["DescKeyA4"];
                if (dtView.Rows[rowIndex]["DescKeyA5"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[10].Text = (string)dtView.Rows[rowIndex]["DescKeyA5"];
                if (dtView.Rows[rowIndex]["DescKeyA6"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[11].Text = (string)dtView.Rows[rowIndex]["DescKeyA6"];
                if (dtView.Rows[rowIndex]["DescKeyA7"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[12].Text = (string)dtView.Rows[rowIndex]["DescKeyA7"];
                if (dtView.Rows[rowIndex]["DescKeyA8"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[13].Text = (string)dtView.Rows[rowIndex]["DescKeyA8"];
                if (dtView.Rows[rowIndex]["DescKeyA9"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[14].Text = (string)dtView.Rows[rowIndex]["DescKeyA9"];
                #endregion

                #region Value Header

                // Label
                //if (dtView.Rows[rowIndex]["DescA1"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[15].Text = (string)dtView.Rows[rowIndex]["DescA1"];
                //if (dtView.Rows[rowIndex]["DescA2"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[17].Text = (string)dtView.Rows[rowIndex]["DescA2"];
                //if (dtView.Rows[rowIndex]["DescA3"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[19].Text = (string)dtView.Rows[rowIndex]["DescA3"];
                //if (dtView.Rows[rowIndex]["DescA4"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[21].Text = (string)dtView.Rows[rowIndex]["DescA4"];
                //if (dtView.Rows[rowIndex]["DescA5"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[23].Text = (string)dtView.Rows[rowIndex]["DescA5"];
                //if (dtView.Rows[rowIndex]["DescA6"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[25].Text = (string)dtView.Rows[rowIndex]["DescA6"];
                //if (dtView.Rows[rowIndex]["DescA7"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[27].Text = (string)dtView.Rows[rowIndex]["DescA7"];
                //if (dtView.Rows[rowIndex]["DescA8"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[29].Text = (string)dtView.Rows[rowIndex]["DescA8"];
                //if (dtView.Rows[rowIndex]["DescA9"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[31].Text = (string)dtView.Rows[rowIndex]["DescA9"];

                if ((string)Session["IsEditBarClick"] == "1")
                {
                    //if (dtView.Rows[rowIndex]["DescA1"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[16].Text = (string)dtView.Rows[rowIndex]["DescA1"];
                    //if (dtView.Rows[rowIndex]["DescA2"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[18].Text = (string)dtView.Rows[rowIndex]["DescA2"];
                    //if (dtView.Rows[rowIndex]["DescA3"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[20].Text = (string)dtView.Rows[rowIndex]["DescA3"];
                    //if (dtView.Rows[rowIndex]["DescA4"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[22].Text = (string)dtView.Rows[rowIndex]["DescA4"];
                    //if (dtView.Rows[rowIndex]["DescA5"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[24].Text = (string)dtView.Rows[rowIndex]["DescA5"];
                    //if (dtView.Rows[rowIndex]["DescA6"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[26].Text = (string)dtView.Rows[rowIndex]["DescA6"];
                    //if (dtView.Rows[rowIndex]["DescA7"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[28].Text = (string)dtView.Rows[rowIndex]["DescA7"];
                    //if (dtView.Rows[rowIndex]["DescA8"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[30].Text = (string)dtView.Rows[rowIndex]["DescA8"];
                    //if (dtView.Rows[rowIndex]["DescA9"] != DBNull.Value) gvAccountMap.HeaderRow.Cells[32].Text = (string)dtView.Rows[rowIndex]["DescA9"];
                }
                #endregion



            }

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Cells[15].Text = dtView.Rows[rowIndex]["DescA1"].ToString();
            //    e.Row.Cells[16].Text = dtView.Rows[rowIndex]["DescA1"].ToString();
            //    e.Row.Cells[17].Text = dtView.Rows[rowIndex]["DescA2"].ToString();
            //    e.Row.Cells[18].Text = dtView.Rows[rowIndex]["DescA2"].ToString();
            //    e.Row.Cells[19].Text = dtView.Rows[rowIndex]["DescA3"].ToString();
            //    e.Row.Cells[20].Text = dtView.Rows[rowIndex]["DescA3"].ToString();
            //    e.Row.Cells[21].Text = dtView.Rows[rowIndex]["DescA4"].ToString();
            //    e.Row.Cells[22].Text = dtView.Rows[rowIndex]["DescA4"].ToString();
            //    e.Row.Cells[23].Text = dtView.Rows[rowIndex]["DescA5"].ToString();
            //    e.Row.Cells[24].Text = dtView.Rows[rowIndex]["DescA5"].ToString();
            //    e.Row.Cells[25].Text = dtView.Rows[rowIndex]["DescA6"].ToString();
            //    e.Row.Cells[26].Text = dtView.Rows[rowIndex]["DescA6"].ToString();
            //    e.Row.Cells[27].Text = dtView.Rows[rowIndex]["DescA7"].ToString();
            //    e.Row.Cells[28].Text = dtView.Rows[rowIndex]["DescA7"].ToString();
            //    e.Row.Cells[29].Text = dtView.Rows[rowIndex]["DescA8"].ToString();
            //    e.Row.Cells[30].Text = dtView.Rows[rowIndex]["DescA8"].ToString();
            //    e.Row.Cells[31].Text = dtView.Rows[rowIndex]["DescA9"].ToString();
            //    e.Row.Cells[32].Text = dtView.Rows[rowIndex]["DescA9"].ToString();
            //}


            DataRow dr = dtView.Rows[rowIndex];

            var txt = e.Row.FindControl("txtA1") as TextBox;
            var ddl = e.Row.FindControl("ddlTxtA1") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA1"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A1").ToString());

            txt = e.Row.FindControl("txtA2") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA2") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA2"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A2").ToString());

            txt = e.Row.FindControl("txtA3") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA3") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA3"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A3").ToString());

            txt = e.Row.FindControl("txtA4") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA4") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA4"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A4").ToString());

            txt = e.Row.FindControl("txtA5") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA5") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA5"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A5").ToString());

            txt = e.Row.FindControl("txtA6") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA6") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA6"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A6").ToString());

            txt = e.Row.FindControl("txtA7") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA7") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA7"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A7").ToString());

            txt = e.Row.FindControl("txtA8") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA8") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA8"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A8").ToString());

            txt = e.Row.FindControl("txtA9") as TextBox;
            ddl = e.Row.FindControl("ddlTxtA9") as DropDownList;
            if (txt != null)
                SetControl(dr["TypeA9"].ToString(), txt, ddl, DataBinder.Eval(e.Row.DataItem, "A9").ToString());
        }

        private void SetControl(string typeControl, TextBox txt, DropDownList ddl, string value = "")
        {
            if (string.IsNullOrEmpty(typeControl))
            {
                txt.Visible = true;
                ddl.Visible = false;
                txt.Text = value;
            }
            else
            {
                txt.Visible = false;
                ddl.Visible = true;
                SetDataForType(typeControl, ddl);
                if (string.IsNullOrEmpty(value))
                    ddl.SelectedIndex = 0;
                else
                    ddl.SelectedValue = value;
            }

        }

        private void SetDataForType(string typeName, DropDownList ddl)
        {
            string sql = string.Empty;
            switch (typeName.ToLower())
            {
                case "acccode":
                    sql = "SELECT NULL as AccCode, '' as AccName UNION ALL SELECT AccCode, CONCAT(AccCode,' : ',AccDesc1, ' | ', AccDesc2,' | ', AccNature) as AccName FROM [ADMIN].AccountCode WHERE AccType NOT IN ('Statistic', 'Header') ORDER BY AccCode";

                    ddl.DataSource = config.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                    ddl.DataTextField = "AccName";
                    ddl.DataValueField = "AccCode";
                    ddl.DataBind();
                    break;

                case "accdept":
                    sql = "SELECT NULL as DeptCode, '' as DeptName UNION ALL SELECT DeptCode, CONCAT(DeptCode, ' | ',DeptDesc) DeptName FROM [ADMIN].AccountDepartment ORDER BY DeptCode";

                    ddl.DataSource = config.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                    ddl.DataTextField = "DeptName";
                    ddl.DataValueField = "DeptCode";
                    ddl.DataBind();
                    break;
            }
        }

        protected void gvAccountMap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtAccountMapp = (DataTable)Session["dtAccountMapp"];

            if ((string)Session["IsEditBarClick"] == "1")
                saveToDatatable();
            else if (Session["IsEditBarClick"].ToString() == "0")
            {
                if (txtSearch.Text == null)
                    Bind_AccountMapp();
                else
                    filterByText(dtAccountMapp, txtSearch.Text);
            }

            gvAccountMap.PageIndex = e.NewPageIndex;
            gvAccountMap.DataSource = dtAccountMapp;
            gvAccountMap.DataBind();
        }

        protected void gvAccountMap_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortDir = string.Empty;

            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                SortDir = "Desc";
            }

            else
            {
                dir = SortDirection.Ascending;
                SortDir = "Asc";
            }

            //DataView sortedView = new DataView(dt);
            DataView sortedView = new DataView(dtAccountMapp);

            sortedView.Sort = e.SortExpression + " " + SortDir;
            gvAccountMap.DataSource = sortedView;
            gvAccountMap.DataBind();

        }

        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        protected void Bind_AccountMapp()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            int rowIndex = getRowIndexViewByID(dtView, ddlViewName.SelectedValue.ToString());
            try
            {
                cnn.Open();

                string strsql = "select [Admin].AccountMapp.*, [in].storelocation.LocationName ";
                strsql += ",level03.CategoryCode AS CategoryCode03 ,level03.CategoryName AS CategoryName03 ";
                strsql += ",level02.CategoryCode AS CategoryCode02 ,level02.CategoryName AS CategoryName02 ";
                strsql += ",level01.CategoryCode AS CategoryCode01 ,level01.CategoryName AS CategoryName01 ";

                strsql += "from [Admin].accountMapp INNER JOIN [in].storelocation ";
                strsql += "ON [Admin].accountMapp.StoreCode = [in].storelocation.LocationCode LEFT JOIN ";
                strsql += "(select levelNo, ParentNo, CategoryCode, CategoryName from [in].productcategory where levelNo = '3') level03 ";
                strsql += "ON [Admin].accountMapp.ItemGroupCode = level03.CateGoryCode LEFT JOIN ";
                strsql += "(select levelNo, ParentNo, CategoryCode, CategoryName from [in].productcategory where levelNo = '2') level02 ";
                strsql += "ON level03.ParentNo = level02.CategoryCode LEFT JOIN ";
                strsql += "(select levelNo, ParentNo, CategoryCode, CategoryName from [in].productcategory where levelNo = '1') level01 ";
                strsql += "ON level02.ParentNo = level01.CategoryCode ";
                strsql += "where PostType = '" + (string)dtView.Rows[rowIndex]["PostType"] + "'";

                SqlCommand myCommand = new SqlCommand(strsql, cnn);
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                DataTable dtAccountMapp = new DataTable();

                da.Fill(dtAccountMapp);

                Session["dtAccountMapp"] = (DataTable)dtAccountMapp;

                gvAccountMap.DataSource = dtAccountMapp;
                gvAccountMap.DataBind();

                cnn.Close();


            }
            catch (Exception ex)
            {
                lbl_Message.Text = ex.Message;
            }
        }

        protected int findIdAfterFilter(string id)
        {
            int rowIndex = -1;
            for (int i = 0; i < dtAccountMapp.Rows.Count; i++)
            {
                if (dtAccountMapp.Rows[i]["ID"].ToString() == id)
                {
                    rowIndex = i;
                    break;
                }
            }
            return rowIndex;
        }

        protected void saveToDatatable()
        {
            int r = 0;
            int rowIndexStart = gvAccountMap.PageIndex * gvAccountMap.PageSize;
            for (int i = rowIndexStart; i < rowIndexStart + gvAccountMap.Rows.Count; i++)
            {
                TextBox txtA1 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA1");
                TextBox txtA2 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA2");
                TextBox txtA3 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA3");
                TextBox txtA4 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA4");
                TextBox txtA5 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA5");
                TextBox txtA6 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA6");
                TextBox txtA7 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA7");
                TextBox txtA8 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA8");
                TextBox txtA9 = (TextBox)gvAccountMap.Rows[r].FindControl("txtA9");


                DropDownList ddlA1 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA1");
                DropDownList ddlA2 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA2");
                DropDownList ddlA3 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA3");
                DropDownList ddlA4 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA4");
                DropDownList ddlA5 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA5");
                DropDownList ddlA6 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA6");
                DropDownList ddlA7 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA7");
                DropDownList ddlA8 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA8");
                DropDownList ddlA9 = (DropDownList)gvAccountMap.Rows[r].FindControl("ddlTxtA9");

                int rowIndex = i;
                if (txtSearch.Text != string.Empty)
                {
                    Label lblID = (Label)gvAccountMap.Rows[r].FindControl("lblID");
                    rowIndex = findIdAfterFilter(lblID.Text);
                }

                if (!gvAccountMap.Columns[6].Visible)
                    dtAccountMapp.Rows[rowIndex]["A1"] = ddlA1.Visible ? ddlA1.SelectedValue.ToString() : txtA1.Text;
                if (!gvAccountMap.Columns[7].Visible)
                    dtAccountMapp.Rows[rowIndex]["A2"] = ddlA2.Visible ? ddlA2.SelectedValue.ToString() : txtA2.Text;
                if (!gvAccountMap.Columns[8].Visible)
                    dtAccountMapp.Rows[rowIndex]["A3"] = ddlA3.Visible ? ddlA3.SelectedValue.ToString() : txtA3.Text;
                if (!gvAccountMap.Columns[9].Visible)
                    dtAccountMapp.Rows[rowIndex]["A4"] = ddlA4.Visible ? ddlA4.SelectedValue.ToString() : txtA4.Text;
                if (!gvAccountMap.Columns[10].Visible)
                    dtAccountMapp.Rows[rowIndex]["A5"] = ddlA5.Visible ? ddlA5.SelectedValue.ToString() : txtA5.Text;
                if (!gvAccountMap.Columns[11].Visible)
                    dtAccountMapp.Rows[rowIndex]["A6"] = ddlA6.Visible ? ddlA6.SelectedValue.ToString() : txtA6.Text;
                if (!gvAccountMap.Columns[12].Visible)
                    dtAccountMapp.Rows[rowIndex]["A7"] = ddlA7.Visible ? ddlA7.SelectedValue.ToString() : txtA7.Text;
                if (!gvAccountMap.Columns[13].Visible)
                    dtAccountMapp.Rows[rowIndex]["A8"] = ddlA8.Visible ? ddlA8.SelectedValue.ToString() : txtA8.Text;
                if (!gvAccountMap.Columns[14].Visible)
                    dtAccountMapp.Rows[rowIndex]["A9"] = ddlA9.Visible ? ddlA9.SelectedValue.ToString() : txtA9.Text;
                r++;
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            filterByText((DataTable)Session["dtAccountMapp"], txtSearch.Text);
        }

        private void filterByText(DataTable dtAccountMapp, string txtSearch)
        {
            // dtAccountMapp already have where postType = ''
            string str = string.Format(@"BusinessUnitCode like '%{0}%' OR StoreCode like '%{0}%' OR 
                CategoryCode like '%{0}%' OR SubCategoryCode like '%{0}%' OR ItemGroupCode like '%{0}%' OR 
                
                A1 like '%{0}%' OR A2 like '%{0}%' OR A3 like '%{0}%' OR A4 like '%{0}%' OR A5 like '%{0}%' OR 
                A6 like '%{0}%' OR A7 like '%{0}%' OR A8 like '%{0}%' OR A9 like '%{0}%' OR
                
                LocationName like '%{0}%' OR CategoryName03 like '%{0}%' OR CategoryCode02 like '%{0}%' OR
                CategoryName02 like '%{0}%' OR CategoryCode01 like '%{0}%' OR CategoryName01 like '%{0}%' ", txtSearch);
            // End Modified.

            string strFilter = string.Format(str);
            dtAccountMapp.DefaultView.RowFilter = strFilter;
            gvAccountMap.DataSource = dtAccountMapp;
            gvAccountMap.DataBind();
        }

        #region "Added on 2016-11"
        protected void btnUploadHide_Click(object sender, EventArgs e)
        {
            string fileName = Session["fileName"].ToString();
            readFromCSV(fileName);
            Bind_AccountMapp();
            pop_ImportExport.ShowOnPageLoad = false;
        }

        protected void BarImportOnClick()
        {
            lbl_UploadMessage.Text = string.Empty;

            pop_ImportExport.ShowOnPageLoad = true;
            //if (gvAccountMap.Rows.Count > 0)
            //{
            //    //get all row when paging is enabled.
            //    //fromGridToDataTable();
            //    pop_ImportExport.ShowOnPageLoad = true;
            //}
            //else
            //{
            //    lbl_Warning.Text = string.Format("No data.");
            //    pop_Warning.ShowOnPageLoad = true;
            //}
        }

        /* protected void fromGridToDataTable()
        {
            DataTable dtExport = new DataTable();
            int columnIndex = 0;
            foreach (TableCell cell in gvAccountMap.HeaderRow.Cells)
            {
                if (cell.Text == "ID")
                { dtExport.Columns.Add(cell.Text); }
                if (gvAccountMap.Columns[columnIndex].Visible)
                { dtExport.Columns.Add(cell.Text); }
                columnIndex++;
            }

            foreach (GridViewRow row in gvAccountMap.Rows)
            {
                Label StoreCode = (Label)row.FindControl("lblSC");
                Label CategoryCode = (Label)row.FindControl("lblCC");
                Label SubCate = (Label)row.FindControl("lblSCC");
                Label ItemGroup = (Label)row.FindControl("lblIGC");
                Label ID = (Label)row.FindControl("lblID");

                // Added on: 24/11/2017, For: New key field.
                Label KeyA1 = (Label)row.FindControl("lblKeyA1");
                Label KeyA2 = (Label)row.FindControl("lblKeyA2");
                Label KeyA3 = (Label)row.FindControl("lblKeyA3");
                Label KeyA4 = (Label)row.FindControl("lblKeyA4");
                Label KeyA5 = (Label)row.FindControl("lblKeyA5");
                Label KeyA6 = (Label)row.FindControl("lblKeyA6");
                Label KeyA7 = (Label)row.FindControl("lblKeyA7");
                Label KeyA8 = (Label)row.FindControl("lblKeyA8");
                Label KeyA9 = (Label)row.FindControl("lblKeyA9");
                // End Added.

                Label a1 = (Label)row.FindControl("lblA1");
                Label a2 = (Label)row.FindControl("lblA2");
                Label a3 = (Label)row.FindControl("lblA3");
                Label a4 = (Label)row.FindControl("lblA4");
                Label a5 = (Label)row.FindControl("lblA5");
                Label a6 = (Label)row.FindControl("lblA6");
                Label a7 = (Label)row.FindControl("lblA7");
                Label a8 = (Label)row.FindControl("lblA8");
                Label a9 = (Label)row.FindControl("lblA9");
                dtExport.Rows.Add();
                int visible_col = 1;
                for (int i = 0; i < gvAccountMap.Columns.Count; i++)
                {
                    if (gvAccountMap.Columns[i].HeaderText == "ID")
                    { dtExport.Rows[row.RowIndex]["ID"] = ID.Text; }
                    if (gvAccountMap.Columns[i].Visible)
                    {
                        switch (i)
                        {
                            case 1: dtExport.Rows[row.RowIndex]["BusinessUnitCode"] = row.Cells[i].Text;
                                visible_col++;
                                break;
                            case 2: dtExport.Rows[row.RowIndex]["StoreCode"] = StoreCode.Text;
                                visible_col++;
                                break;
                            case 3: dtExport.Rows[row.RowIndex]["CategoryCode"] = CategoryCode.Text;
                                visible_col++;
                                break;
                            case 4: dtExport.Rows[row.RowIndex]["SubCategoryCode"] = SubCate.Text;
                                visible_col++;
                                break;
                            case 5: dtExport.Rows[row.RowIndex]["ItemGroupCode"] = ItemGroup.Text;
                                visible_col++;
                                break;

                            // Added on: 24/11/2017, For: New key field.
                            case 6: dtExport.Rows[row.RowIndex][visible_col] = KeyA1.Text;
                                visible_col++;
                                break;
                            case 7: dtExport.Rows[row.RowIndex][visible_col] = KeyA2.Text;
                                visible_col++;
                                break;
                            case 8: dtExport.Rows[row.RowIndex][visible_col] = KeyA3.Text;
                                visible_col++;
                                break;
                            case 9: dtExport.Rows[row.RowIndex][visible_col] = KeyA4.Text;
                                visible_col++;
                                break;
                            case 10: dtExport.Rows[row.RowIndex][visible_col] = KeyA5.Text;
                                visible_col++;
                                break;
                            case 11: dtExport.Rows[row.RowIndex][visible_col] = KeyA6.Text;
                                visible_col++;
                                break;
                            case 12: dtExport.Rows[row.RowIndex][visible_col] = KeyA7.Text;
                                visible_col++;
                                break;
                            case 13: dtExport.Rows[row.RowIndex][visible_col] = KeyA8.Text;
                                visible_col++;
                                break;
                            case 14: dtExport.Rows[row.RowIndex][visible_col] = KeyA9.Text;
                                visible_col++;
                                break;
                            // End Added.

                            case 15: dtExport.Rows[row.RowIndex][visible_col] = a1.Text;
                                visible_col++;
                                break;
                            case 17: dtExport.Rows[row.RowIndex][visible_col] = a2.Text;
                                visible_col++;
                                break;
                            case 19: dtExport.Rows[row.RowIndex][visible_col] = a3.Text;
                                visible_col++;
                                break;
                            case 21: dtExport.Rows[row.RowIndex][visible_col] = a4.Text;
                                visible_col++;
                                break;
                            case 23: dtExport.Rows[row.RowIndex][visible_col] = a5.Text;
                                visible_col++;
                                break;
                            case 25: dtExport.Rows[row.RowIndex][visible_col] = a6.Text;
                                visible_col++;
                                break;
                            case 27: dtExport.Rows[row.RowIndex][visible_col] = a7.Text;
                                visible_col++;
                                break;
                            case 29: dtExport.Rows[row.RowIndex][visible_col] = a8.Text;
                                visible_col++;
                                break;
                            case 31: dtExport.Rows[row.RowIndex][visible_col] = a9.Text;
                                visible_col++;
                                break;
                        }
                    }
                }
            }
            Session["dtExport"] = (DataTable)dtExport;
        } */


        protected void btnExportHide_Click(object sender, EventArgs e)
        {
            dataTableToCSV(ddlViewName.SelectedValue.ToString(), ddlViewName.SelectedItem.ToString());
        }
        #endregion

        #region Added on: 2017-11
        protected void cblView_KeyA_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control_cvlValueLookUp();
        }

        protected void Control_cvlValueLookUp()
        {
            txtDESCA1.Enabled = true;
            txtDESCA2.Enabled = true;
            txtDESCA3.Enabled = true;
            txtDESCA4.Enabled = true;
            txtDESCA5.Enabled = true;
            txtDESCA6.Enabled = true;
            txtDESCA7.Enabled = true;
            txtDESCA8.Enabled = true;
            txtDESCA9.Enabled = true;

            for (int i = 0; i < cblView_KeyA.Items.Count; i++)
            {
                if (cblView_KeyA.Items[i].Value.Contains("A"))
                {
                    string viewValue = cblView_KeyA.Items[i].Value;
                    int indexOfA = viewValue.IndexOf("A");
                    viewValue = viewValue.Substring(indexOfA);
                    cblValue.Items.FindByValue(viewValue).Enabled = (cblView_KeyA.Items[i].Selected) ? false : true;
                    cblValue.Items.FindByValue(viewValue).Selected = (cblView_KeyA.Items[i].Selected) ? false : cblValue.Items.FindByValue(viewValue).Selected;

                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A1")) txtDESCA1.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A2")) txtDESCA2.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A3")) txtDESCA3.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A4")) txtDESCA4.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A5")) txtDESCA5.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A6")) txtDESCA6.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A7")) txtDESCA7.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A8")) txtDESCA8.Enabled = false;
                    if (cblView_KeyA.Items[i].Selected && viewValue.Contains("A9")) txtDESCA9.Enabled = false;
                }
            }
        }

        protected bool EXEC_spGetNewAccountMapping(ref string errMsg)
        {
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format(@"EXEC [ADMIN].[GetNewAccountMapp]");

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                con.Close();
                errMsg = ex.Message;
                return false;
            }

        }
        #endregion

        protected string uploadFile()
        {
            string fileName = string.Empty;
            if (FileUploadControl.HasFile)
            {
                fileName = Path.GetFileName(FileUploadControl.FileName);
                FileUploadControl.SaveAs(Server.MapPath("~/CSV/") + fileName);
            }
            return fileName;
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lbl_UploadMessage.Text = string.Empty;

            if (FileUploadControl.HasFile)
            {
                string filePath = config.GetConfigValue("SYS", "ATTACH", "Path", LoginInfo.ConnStr) + "/Temp/";

                DirectoryInfo thisFolder = new DirectoryInfo(Server.MapPath(filePath));

                if (!thisFolder.Exists)
                    thisFolder.Create();

                string filename = filePath + Path.GetFileName(FileUploadControl.FileName);

                FileUploadControl.SaveAs(Server.MapPath(filename));


                //Read the contents of CSV file.  
                //string csvData = File.ReadAllText(Server.MapPath(filename));
                string[] lines = File.ReadAllLines(Server.MapPath(filename));

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] cols = lines[i].Split(',');

                    if (cols[0] != "")
                    {

                        string A1 = cols.Length > 6 ? cols[6] : string.Empty;
                        string A2 = cols.Length > 7 ? cols[7] : string.Empty;
                        string A3 = cols.Length > 8 ? cols[8] : string.Empty;
                        string A4 = cols.Length > 9 ? cols[9] : string.Empty;
                        string A5 = cols.Length > 10 ? cols[10] : string.Empty;
                        string A6 = cols.Length > 11 ? cols[11] : string.Empty;
                        string A7 = cols.Length > 12 ? cols[12] : string.Empty;
                        string A8 = cols.Length > 13 ? cols[13] : string.Empty;
                        string A9 = cols.Length > 14 ? cols[14] : string.Empty;


                        string sql = "UPDATE [ADMIN].AccountMapp SET ";
                        // sql += string.Format(" BusinessUnitCode = '{0}',", cols[1]);
                        // sql += string.Format(" StoreCode = '{0}',", cols[2]);
                        // sql += string.Format(" CategoryCode = '{0}',", cols[3]);
                        // sql += string.Format(" SubCategoryCode = '{0}',", cols[4]);
                        // sql += string.Format(" ItemGroupCode = '{0}',", cols[5]);
                        sql += string.Format(" A1 = '{0}',", A1);
                        sql += string.Format(" A2 = '{0}',", A2);
                        sql += string.Format(" A3 = '{0}',", A3);
                        sql += string.Format(" A4 = '{0}',", A4);
                        sql += string.Format(" A5 = '{0}',", A5);
                        sql += string.Format(" A6 = '{0}',", A6);
                        sql += string.Format(" A7 = '{0}',", A7);
                        sql += string.Format(" A8 = '{0}',", A8);
                        sql += string.Format(" A9 = '{0}'", A9);
                        // sql += string.Format(" PostType = '{0}'", ols[15]);
                        sql += string.Format(" WHERE ID = '{0}'", cols[0]);

                        config.DbParseQuery(sql, null, LoginInfo.ConnStr);
                        //lbl_Message.Text = lbl_Message.Text + sql + "<br/>";
                    }
                }



                pop_ImportExport.ShowOnPageLoad = false;
                lbl_Warning.Text = "Import completed";
                pop_Warning.ShowOnPageLoad = true;

            }
            else
            {
                lbl_UploadMessage.Text = "Please select file to import.";
            }
        }

        protected void btnUpload1_Click(object sender, EventArgs e)
        {
            lbl_UploadMessage.Text = string.Empty;
            if (FileUploadControl.HasFile)
            {
                string filePath = config.GetConfigValue("SYS", "ATTACH", "Path", LoginInfo.ConnStr) + "/Temp/";

                DirectoryInfo thisFolder = new DirectoryInfo(Server.MapPath(filePath));
                if (!thisFolder.Exists)
                {
                    thisFolder.Create();
                }

                string filename = filePath + Path.GetFileName(FileUploadControl.FileName);
                FileUploadControl.SaveAs(Server.MapPath(filename));


                //Read the contents of CSV file.  
                string csvData = File.ReadAllText(Server.MapPath(filename));
                string[] lineData = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                //lbl_Message.Text = lineData[0];

                //Create a DataTable.  
                DataTable dt = new DataTable();
                string[] columnHeaders = lineData[0].Split(',');
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    dt.Columns.Add(new DataColumn(columnHeaders[i], typeof(string)));
                }

                //lbl_Message.Text = dt.Columns.Count.ToString();
                for (int i = 1; i < lineData.Length; i++)
                {
                    string row = lineData[i];
                    if (!string.IsNullOrEmpty(row))
                    {
                        dt.Rows.Add();
                        int j = 0;
                        //Execute a loop over the columns.  
                        foreach (string cell in row.Split(','))
                        {
                            dt.Rows[dt.Rows.Count - 1][j] = cell;
                            j++;
                        }
                    }
                }


                string connetionString = LoginInfo.ConnStr;
                SqlConnection con = new SqlConnection(connetionString);
                //try
                //{
                con.Open();

                foreach (DataRow row in dt.Rows)
                {
                    string strSql = "UPDATE [Admin].AccountMapp ";
                    strSql += "Set ";

                    for (int i = 1; i < columnHeaders.Length; i++) // exclude ID
                    {
                        strSql += string.Format("{0} = '{1}',", columnHeaders[i], row[columnHeaders[i]]);
                    }
                    strSql = strSql.TrimEnd().Remove(strSql.Length - 1);
                    strSql += string.Format("WHERE ID= '{0}'", row[columnHeaders[0]]);

                    SqlCommand cmd = new SqlCommand(@strSql, con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                //}
                //catch
                //{
                //}


                //readFromCSV(filename);

                //Bind_AccountMapp();

                pop_ImportExport.ShowOnPageLoad = false;
                lbl_Warning.Text = "Import completed";
                pop_Warning.ShowOnPageLoad = true;

            }
            else
            {
                lbl_UploadMessage.Text = "Please select file to import.";
            }
        }

        protected bool readFromCSV(string fileName)
        {

            DataTable dtImportFile = new DataTable();
            try
            {
                using (System.IO.TextReader tr = File.OpenText(Server.MapPath(fileName)))
                {
                    string line;
                    int lineIndex = 0;
                    int rowViewIndex = 0;
                    while ((line = tr.ReadLine()) != null)
                    {
                        string[] items = line.Split(',');
                        for (int i = 0; i < items.Length; i++)
                        { items[i] = items[i].Trim(); }
                        if (lineIndex == 0)
                        { rowViewIndex = getRowIndexViewByID(dtView, items[1].ToString()); }
                        else if (lineIndex == 1)
                        {
                            for (int c = 0; c < items.Length; c++)
                            { dtImportFile.Columns.Add(new DataColumn(items[c], typeof(string))); }
                        }
                        else if (lineIndex > 1)
                        { dtImportFile.Rows.Add(items); }
                        lineIndex++;
                    }
                    saveImportToDatabase(rowViewIndex, dtImportFile);
                }
                return true;
            }
            catch
            { return false; }
        }

        protected void saveImportToDatabase(int rowViewIndex, DataTable dtImportFile)
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

                foreach (DataRow row in dtImportFile.Rows)
                {
                    string strSql = "update [Admin].accountMapp ";
                    strSql += "Set ";
                    if ((bool)dtView.Rows[rowViewIndex]["A1"]) { strSql += "A1=@A1,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A2"]) { strSql += "A2=@A2,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A3"]) { strSql += "A3=@A3,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A4"]) { strSql += "A4=@A4,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A5"]) { strSql += "A5=@A5,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A6"]) { strSql += "A6=@A6,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A7"]) { strSql += "A7=@A7,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A8"]) { strSql += "A8=@A8,"; }
                    if ((bool)dtView.Rows[rowViewIndex]["A9"]) { strSql += "A9=@A9,"; }
                    strSql = strSql.Remove(strSql.Length - 1);
                    strSql += " where ID='" + row["ID"] + "'";

                    SqlCommand cmd = new SqlCommand(strSql, cnn);
                    cmd.Parameters.Clear();

                    int col = 1;
                    if ((bool)dtView.Rows[rowViewIndex]["BusinessUnitCode"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["StoreCode"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["CategoryCode"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["SubCategoryCode"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["ItemGroupCode"]) { col++; }

                    // Added on: 23/11/2017, For: New key field
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA1"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA2"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA3"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA4"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA5"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA6"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA7"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA8"]) { col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["KeyA9"]) { col++; }
                    // End Added.

                    if ((bool)dtView.Rows[rowViewIndex]["A1"]) { cmd.Parameters.AddWithValue("@A1", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A2"]) { cmd.Parameters.AddWithValue("@A2", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A3"]) { cmd.Parameters.AddWithValue("@A3", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A4"]) { cmd.Parameters.AddWithValue("@A4", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A5"]) { cmd.Parameters.AddWithValue("@A5", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A6"]) { cmd.Parameters.AddWithValue("@A6", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A7"]) { cmd.Parameters.AddWithValue("@A7", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A8"]) { cmd.Parameters.AddWithValue("@A8", row[col]); col++; }
                    if ((bool)dtView.Rows[rowViewIndex]["A9"]) { cmd.Parameters.AddWithValue("@A9", row[col]); col++; }
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
            }
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Parent", "callbtnExportOfParent();", true);
            ExportToCsv(dtView, dtAccountMapp);
        }

        private void ExportToCsv(DataTable dtPattern, DataTable dtData)
        {
            DataRow drPattern = dtPattern.Rows[ddlViewName.SelectedIndex];
            string postType = drPattern["PostType"].ToString();
            string sql = string.Format("SELECT * FROM [ADMIN].AccountMapp WHERE Posttype = '{0}' ORDER BY StoreCode, CategoryCode, SubCategoryCode, ItemGroupCode", postType);
            DataTable dt = config.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            string fileName = string.Format("{0}_AccountMapping_{1}.csv", LoginInfo.BuInfo.BuCode, DateTime.Now.ToString("yyyyMMdd-hhmmss"));

            StringBuilder sb = new StringBuilder();
            // Header (Field Columns)
            // Column: ID
            string fieldNames = drPattern["ViewName"].ToString() + ",";

            // Column: Business Unit
            fieldNames += "Business Unit,";

            // Column: Store/Location
            fieldNames += "Store/Location,";

            // Column: Category
            fieldNames += "Category,";

            // Column: Sub-Category
            fieldNames += "Sub-Caetgory,";

            // Column: Itemgroup
            fieldNames += "Item-Group,";

            // Check Key or Value
            // Column: A1 
            fieldNames += Convert.ToBoolean(drPattern["KeyA1"]) ? drPattern["DescKeyA1"].ToString() : drPattern["DescA1"].ToString();
            fieldNames += ",";
            // Column: A2 
            fieldNames += Convert.ToBoolean(drPattern["KeyA2"]) ? drPattern["DescKeyA2"].ToString() : drPattern["DescA2"].ToString();
            fieldNames += ",";
            // Column: A3 
            fieldNames += Convert.ToBoolean(drPattern["KeyA3"]) ? drPattern["DescKeyA3"].ToString() : drPattern["DescA3"].ToString();
            fieldNames += ",";
            // Column: A4 
            fieldNames += Convert.ToBoolean(drPattern["KeyA4"]) ? drPattern["DescKeyA4"].ToString() : drPattern["DescA4"].ToString();
            fieldNames += ",";
            // Column: A5 
            fieldNames += Convert.ToBoolean(drPattern["KeyA5"]) ? drPattern["DescKeyA5"].ToString() : drPattern["DescA5"].ToString();
            fieldNames += ",";
            // Column: A6 
            fieldNames += Convert.ToBoolean(drPattern["KeyA6"]) ? drPattern["DescKeyA6"].ToString() : drPattern["DescA6"].ToString();
            fieldNames += ",";
            // Column: A7 
            fieldNames += Convert.ToBoolean(drPattern["KeyA7"]) ? drPattern["DescKeyA7"].ToString() : drPattern["DescA7"].ToString();
            fieldNames += ",";
            // Column: A8 
            fieldNames += Convert.ToBoolean(drPattern["KeyA8"]) ? drPattern["DescKeyA8"].ToString() : drPattern["DescA8"].ToString();
            fieldNames += ",";
            // Column: A9 
            fieldNames += Convert.ToBoolean(drPattern["KeyA9"]) ? drPattern["DescKeyA9"].ToString() : drPattern["DescA9"].ToString();
            fieldNames += ",";

            // Column: PostType
            fieldNames += "Post Type,";

            sb.AppendLine(fieldNames);

            // Data
            foreach (DataRow dr in dt.Rows)
            {
                string dataLine = string.Empty;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dataLine += dr[i] + ",";
                }
                sb.AppendLine(dataLine);
            }

            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "text/csv";
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }



        private void ExportToCsv1(DataTable dtPattern, DataTable dtData)
        {
            string fileName = string.Format("{0}_AccountMapping_{1}.csv", LoginInfo.BuInfo.BuCode, DateTime.Today.ToString("yyyyMMdd"));

            DataRow drPattern = dtPattern.Rows[0];
            string[] columnNames = new string[24];

            columnNames[0] = "ID";
            columnNames[1] = Convert.ToBoolean(drPattern["BusinessUnitCode"]) == true ? "BusinessUnitCode" : string.Empty;
            columnNames[2] = Convert.ToBoolean(drPattern["StoreCode"]) == true ? "StoreCode" : string.Empty;
            columnNames[3] = Convert.ToBoolean(drPattern["CategoryCode"]) == true ? "CategoryCode" : string.Empty;
            columnNames[4] = Convert.ToBoolean(drPattern["SubCategoryCode"]) == true ? "SubCategoryCode" : string.Empty;
            columnNames[5] = Convert.ToBoolean(drPattern["ItemGroupCode"]) == true ? "ItemGroupCode" : string.Empty;

            columnNames[6] = Convert.ToBoolean(drPattern["KeyA1"]) == true ? "KeyA1" : string.Empty;
            columnNames[7] = Convert.ToBoolean(drPattern["KeyA2"]) == true ? "KeyA2" : string.Empty;
            columnNames[8] = Convert.ToBoolean(drPattern["KeyA3"]) == true ? "KeyA3" : string.Empty;
            columnNames[9] = Convert.ToBoolean(drPattern["KeyA4"]) == true ? "KeyA4" : string.Empty;
            columnNames[10] = Convert.ToBoolean(drPattern["KeyA5"]) == true ? "KeyA5" : string.Empty;
            columnNames[11] = Convert.ToBoolean(drPattern["KeyA6"]) == true ? "KeyA6" : string.Empty;
            columnNames[12] = Convert.ToBoolean(drPattern["KeyA7"]) == true ? "KeyA7" : string.Empty;
            columnNames[13] = Convert.ToBoolean(drPattern["KeyA8"]) == true ? "KeyA8" : string.Empty;
            columnNames[14] = Convert.ToBoolean(drPattern["KeyA9"]) == true ? "KeyA9" : string.Empty;

            columnNames[15] = Convert.ToBoolean(drPattern["A1"]) == true ? "A1" : string.Empty;
            columnNames[16] = Convert.ToBoolean(drPattern["A2"]) == true ? "A2" : string.Empty;
            columnNames[17] = Convert.ToBoolean(drPattern["A3"]) == true ? "A3" : string.Empty;
            columnNames[18] = Convert.ToBoolean(drPattern["A4"]) == true ? "A4" : string.Empty;
            columnNames[19] = Convert.ToBoolean(drPattern["A5"]) == true ? "A5" : string.Empty;
            columnNames[20] = Convert.ToBoolean(drPattern["A6"]) == true ? "A6" : string.Empty;
            columnNames[21] = Convert.ToBoolean(drPattern["A7"]) == true ? "A7" : string.Empty;
            columnNames[22] = Convert.ToBoolean(drPattern["A8"]) == true ? "A8" : string.Empty;
            columnNames[23] = Convert.ToBoolean(drPattern["A9"]) == true ? "A9" : string.Empty;

            columnNames[5] = Convert.ToBoolean(drPattern["KeyA1"]) == true ? drPattern["DescKeyA1"].ToString() : string.Empty;
            columnNames[6] = Convert.ToBoolean(drPattern["KeyA2"]) == true ? drPattern["DescKeyA2"].ToString() : string.Empty;
            columnNames[7] = Convert.ToBoolean(drPattern["KeyA3"]) == true ? drPattern["DescKeyA3"].ToString() : string.Empty;
            columnNames[8] = Convert.ToBoolean(drPattern["KeyA4"]) == true ? drPattern["DescKeyA4"].ToString() : string.Empty;
            columnNames[9] = Convert.ToBoolean(drPattern["KeyA5"]) == true ? drPattern["DescKeyA5"].ToString() : string.Empty;
            columnNames[10] = Convert.ToBoolean(drPattern["KeyA6"]) == true ? drPattern["DescKeyA6"].ToString() : string.Empty;
            columnNames[11] = Convert.ToBoolean(drPattern["KeyA7"]) == true ? drPattern["DescKeyA7"].ToString() : string.Empty;
            columnNames[12] = Convert.ToBoolean(drPattern["KeyA8"]) == true ? drPattern["DescKeyA8"].ToString() : string.Empty;
            columnNames[13] = Convert.ToBoolean(drPattern["KeyA9"]) == true ? drPattern["DescKeyA9"].ToString() : string.Empty;

            columnNames[14] = Convert.ToBoolean(drPattern["A1"]) == true ? drPattern["DescA1"].ToString() : string.Empty;
            columnNames[15] = Convert.ToBoolean(drPattern["A2"]) == true ? drPattern["DescA2"].ToString() : string.Empty;
            columnNames[16] = Convert.ToBoolean(drPattern["A3"]) == true ? drPattern["DescA3"].ToString() : string.Empty;
            columnNames[17] = Convert.ToBoolean(drPattern["A4"]) == true ? drPattern["DescA4"].ToString() : string.Empty;
            columnNames[18] = Convert.ToBoolean(drPattern["A5"]) == true ? drPattern["DescA5"].ToString() : string.Empty;
            columnNames[19] = Convert.ToBoolean(drPattern["A6"]) == true ? drPattern["DescA6"].ToString() : string.Empty;
            columnNames[20] = Convert.ToBoolean(drPattern["A7"]) == true ? drPattern["DescA7"].ToString() : string.Empty;
            columnNames[21] = Convert.ToBoolean(drPattern["A8"]) == true ? drPattern["DescA8"].ToString() : string.Empty;
            columnNames[22] = Convert.ToBoolean(drPattern["A9"]) == true ? drPattern["DescA9"].ToString() : string.Empty;


            columnNames = columnNames.Where(c => c != string.Empty).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dtData.Rows)
            {
                //string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                string[] fieldValues = new string[columnNames.Length];

                for (int i = 0; i < columnNames.Length; i++)
                {
                    fieldValues[i] = row[columnNames[i]].ToString();
                }

                //fieldValues[0] = Convert.ToBoolean(drPattern["BusinessUnitCode"]) == true ? row["BusinessUnitCode"].ToString() : string.Empty;
                //fieldValues[1] = Convert.ToBoolean(drPattern["StoreCode"]) == true ? row["StoreCode"].ToString() : string.Empty;
                //fieldValues[2] = Convert.ToBoolean(drPattern["CategoryCode"]) == true ? row["CategoryCode"].ToString() : string.Empty;
                //fieldValues[3] = Convert.ToBoolean(drPattern["SubCategoryCode"]) == true ? row["SubCategoryCode"].ToString() : string.Empty;
                //fieldValues[4] = Convert.ToBoolean(drPattern["ItemGroupCode"]) == true ? row["ItemGroupCode"].ToString() : string.Empty;

                //fieldValues[5] = Convert.ToBoolean(drPattern["KeyA1"]) == true ? row["A1"].ToString() : string.Empty;
                //fieldValues[6] = Convert.ToBoolean(drPattern["KeyA2"]) == true ? row["A2"].ToString() : string.Empty;
                //fieldValues[7] = Convert.ToBoolean(drPattern["KeyA3"]) == true ? row["A3"].ToString() : string.Empty;
                //fieldValues[8] = Convert.ToBoolean(drPattern["KeyA4"]) == true ? row["A4"].ToString() : string.Empty;
                //fieldValues[9] = Convert.ToBoolean(drPattern["KeyA5"]) == true ? row["A5"].ToString() : string.Empty;
                //fieldValues[10] = Convert.ToBoolean(drPattern["KeyA6"]) == true ? row["A6"].ToString() : string.Empty;
                //fieldValues[11] = Convert.ToBoolean(drPattern["KeyA7"]) == true ? row["A7"].ToString() : string.Empty;
                //fieldValues[12] = Convert.ToBoolean(drPattern["KeyA8"]) == true ? row["A8"].ToString() : string.Empty;
                //fieldValues[13] = Convert.ToBoolean(drPattern["KeyA9"]) == true ? row["A9"].ToString() : string.Empty;

                //fieldValues[14] = Convert.ToBoolean(drPattern["A1"]) == true ? row["A1"].ToString() : string.Empty;
                //fieldValues[15] = Convert.ToBoolean(drPattern["A2"]) == true ? row["A2"].ToString() : string.Empty;
                //fieldValues[16] = Convert.ToBoolean(drPattern["A3"]) == true ? row["A3"].ToString() : string.Empty;
                //fieldValues[17] = Convert.ToBoolean(drPattern["A4"]) == true ? row["A4"].ToString() : string.Empty;
                //fieldValues[18] = Convert.ToBoolean(drPattern["A5"]) == true ? row["A5"].ToString() : string.Empty;
                //fieldValues[19] = Convert.ToBoolean(drPattern["A6"]) == true ? row["A6"].ToString() : string.Empty;
                //fieldValues[20] = Convert.ToBoolean(drPattern["A7"]) == true ? row["A7"].ToString() : string.Empty;
                //fieldValues[21] = Convert.ToBoolean(drPattern["A8"]) == true ? row["A8"].ToString() : string.Empty;
                //fieldValues[22] = Convert.ToBoolean(drPattern["A9"]) == true ? row["A9"].ToString() : string.Empty;


                //fieldValues = fieldValues.Where(c => c != string.Empty).ToArray();

                sb.AppendLine(string.Join(",", fieldValues));
            }


            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "text/csv";
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void dataTableToCSV(string ddlViewValue, string ddlViewItem)
        {
            string fileName = string.Format("AccountMapp_V{0}_P{1}", ddlViewValue, gvAccountMap.PageIndex);
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "text/csv";

                StringBuilder sb = new StringBuilder();
                DataTable dtExport = (DataTable)Session["dtExport"];

                //sb.Append("View: ," + ddlViewName.SelectedValue.ToString() + "," + ddlViewName.SelectedItem.ToString() + "\r\n");
                sb.Append("View: ," + ddlViewValue + "," + ddlViewItem + "\r\n");
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    if (i < dtExport.Columns.Count - 1)
                    { sb.Append(dtExport.Columns[i].ColumnName + ','); }
                    else { sb.Append(dtExport.Columns[i].ColumnName); }
                }
                sb.Append("\r\n");

                foreach (DataRow row in dtExport.Rows)
                {
                    int index = 0;
                    foreach (DataColumn col in dtExport.Columns)
                    {
                        if (index < dtExport.Columns.Count - 1)
                        { sb.Append(row[col].ToString() + ','); }
                        else
                        { sb.Append(row[col].ToString()); }
                        index++;
                    } sb.Append("\r\n");
                }

                // Be careful, if U use updatePanel. 
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                lbl_Message.Text = string.Format("{0}", ex.Message);
            }
        }

    }

    [Serializable]
    public class AccountCode
    {
        public int Id { get; set; }
        public string AccCode { get; set; }
        public string AccDesc1 { get; set; }
        public string AccDesc2 { get; set; }
        public string AccNature { get; set; }
        public string AccType { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    [Serializable]
    public class AccountDepartment
    {
        public int Id { get; set; }
        public string DeptCode { get; set; }
        public string DeptDesc { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }



    public class RootAcc
    {
        public IEnumerable<DataAcc> Data { get; set; }
        public Paging Paging { get; set; }
    }

    [Serializable]
    public class DataAcc
    {
        public int Id { get; set; }
        public string AccCode { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Nature { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public bool Used { get; set; }
        public string UserModified { get; set; }
        public DateTime LastModified { get; set; }
    }


    public class RootDept
    {
        public IEnumerable<DataDept> Data { get; set; }
        public Paging Paging { get; set; }
    }

    [Serializable]
    public class DataDept
    {
        public string UserModified { get; set; }
        public DateTime LastModified { get; set; }
        public int Id { get; set; }
        public string DeptCode { get; set; }
        public string Description { get; set; }
    }

    public class Paging
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int PageCount { get; set; }
        public int TotalRecordCount { get; set; }
        public object Next { get; set; }
        public object Prev { get; set; }
    }



}