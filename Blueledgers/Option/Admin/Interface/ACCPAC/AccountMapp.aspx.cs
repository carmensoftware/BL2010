using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Admin.Interface.ACCPAC
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accountMapp =
            new Blue.BL.Option.Admin.Interface.AccountMapp();

        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();

        private readonly DataSet dsAccountMapp = new DataSet();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private string MsgError = string.Empty;
        private Blue.BL.GL.Account.Account account = new Blue.BL.GL.Account.Account();
        private DataSet dsAccountCategory = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Gets and display AccountMapp data when page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
                
            }
            else
            {
                dsAccountCategory = (DataSet) Session["dsAccountCategory"];
            }

            base.Page_Load(sender, e);
        }

        /// <summary>
        ///     Gets AccountMapp data.
        /// </summary>
        private void Page_Retrieve()
        {
            cmb_BusinessUnitCode.ValueField = "BuCode";
            cmb_BusinessUnitCode.TextField = "BuName";
            cmb_BusinessUnitCode.TextFormatString = "{0}";
            cmb_BusinessUnitCode.DataSource = buUser.GetList(LoginInfo.LoginName, ref MsgError);
            cmb_BusinessUnitCode.DataBind();
            cmb_BusinessUnitCode.Value = LoginInfo.BuInfo.BuCode;

            var dtLocation = storeLct.GetUsingLocation(LoginInfo.ConnStr);

            if (dtLocation.Rows.Count > 0)
            {
                cmb_Store.ValueField = "LocationCode";
                cmb_Store.TextField = "LocationName";
                cmb_Store.TextFormatString = "{0}";
                cmb_Store.DataSource = dtLocation;
                cmb_Store.DataBind();
                cmb_Store.Value = dtLocation.Rows[0]["LocationCode"];
            }
            else
            {
                lbl_Warning.Text = "There is no store location to map with account code.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            // get the catgory list based on type
            accountMapp.GetList(dsAccountCategory, cmb_Store.SelectedItem.Value.ToString(),
                cmb_Type.SelectedItem.Value.ToString(), LoginInfo.ConnStr);

            Session["dsAccountCategory"] = dsAccountCategory;

            Page_Setting();
        }

        /// <summary>
        ///     Display AccountMapp data.
        /// </summary>
        private void Page_Setting()
        {
            grd_AccountMapp.DataSource = dsAccountCategory.Tables[accountMapp.TableName];
            grd_AccountMapp.DataBind();

            cmb_BusinessUnitCode.Value = LoginInfo.BuInfo.BuCode;
        }

        protected void cmb_BusinessUnitCode_OnItemRequestedByValue(object source,
            ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox) source;

            comboBox.DataSource = buUser.GetList(LoginInfo.LoginName, ref MsgError);
            comboBox.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void cmb_Store_OnItemsRequestedByFilterCondition(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox) source;

            var filter = string.Format("%{0}%", e.Filter);
            var startIndex = int.Parse((e.BeginIndex + 1).ToString());
            var endIndex = int.Parse((e.EndIndex + 1).ToString());

            var dtStoreLocation = storeLct.GetListByRowFilter(filter, startIndex, endIndex, LoginInfo.ConnStr);
            //----02/03/2012----storeLct.GetListByRowFilter2(filter, startIndex, endIndex, LoginInfo.ConnStr);

            comboBox.DataSource = dtStoreLocation;
            comboBox.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void cmb_Store_OnItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox) source;

            comboBox.DataSource = storeLct.GetUsingLocation(LoginInfo.ConnStr);
                //storeLct.GetLookupForMapping(LoginInfo.ConnStr);
            comboBox.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_AccountMapp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (dsAccountMapp != null)
            {
                dsAccountMapp.Clear();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display businessunit Code
                if (e.Row.FindControl("lbl_BusinessUnit") != null)
                {
                    var lbl_BusinessUnit = (Label) e.Row.FindControl("lbl_BusinessUnit");
                    lbl_BusinessUnit.Text = LoginInfo.BuInfo.BuCode;
                }

                // Display businessunit Code
                if (e.Row.FindControl("hid_BusinessUnitCode") != null)
                {
                    var hid_BusinessUnitCode = (HiddenField) e.Row.FindControl("hid_BusinessUnitCode");
                    hid_BusinessUnitCode.Value = LoginInfo.BuInfo.BuCode;
                }

                // Display Store Code
                if (e.Row.FindControl("lbl_StoreCode") != null)
                {
                    var lbl_StoreCode = (Label) e.Row.FindControl("lbl_StoreCode");
                    lbl_StoreCode.Text = (cmb_Store.SelectedItem.Text == null
                        ? string.Empty
                        : cmb_Store.SelectedItem.Text);
                }

                // Display Store Code
                if (e.Row.FindControl("hid_StoreCode") != null)
                {
                    var hid_StoreCode = (HiddenField) e.Row.FindControl("hid_StoreCode");
                    hid_StoreCode.Value = (cmb_Store.SelectedItem.Value.ToString() == null
                        ? string.Empty
                        : cmb_Store.SelectedItem.Value.ToString());
                }

                // ItemGroupName
                if (e.Row.FindControl("lbl_ItemGroup") != null)
                {
                    var lbl_ItemGroup = (Label) e.Row.FindControl("lbl_ItemGroup");
                    lbl_ItemGroup.Text = DataBinder.Eval(e.Row.DataItem, "CategoryName").ToString();
                }

                // ItemGroupCode
                if (e.Row.FindControl("hid_ItemGroupCode") != null)
                {
                    var hid_ItemGroupCode = (HiddenField) e.Row.FindControl("hid_ItemGroupCode");
                    hid_ItemGroupCode.Value = DataBinder.Eval(e.Row.DataItem, "CategoryCode").ToString();

                    //accountMapp.GetAccountMappingListByBusinessUnitAndStoreAndItemGroup(dsAccountMapp, cmb_BusinessUnitCode.SelectedItem.Value.ToString(), cmb_Store.SelectedItem.Value.ToString(), hid_ItemGroupCode.Value.ToString(), LoginInfo.ConnStr);
                    accountMapp.GetAccountMappingListByBusinessUnitAndStoreAndItemGroup(dsAccountMapp,
                        LoginInfo.BuInfo.BuCode,
                        cmb_Store.SelectedItem.Value.ToString(), hid_ItemGroupCode.Value, LoginInfo.ConnStr);
                    Session["dsAccountMapp"] = dsAccountMapp;
                }

                // Accountcode
                if (e.Row.FindControl("txt_A1") != null)
                {
                    var txt_A1 = (TextBox) e.Row.FindControl("txt_A1");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A1.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A1"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A1").ToString();
                    }
                    else
                    {
                        txt_A1.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A2") != null)
                {
                    var txt_A2 = (TextBox) e.Row.FindControl("txt_A2");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A2.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A2"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A2").ToString();
                    }
                    else
                    {
                        txt_A2.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A3") != null)
                {
                    var txt_A3 = (TextBox) e.Row.FindControl("txt_A3");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A3.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A3"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A3").ToString();
                    }
                    else
                    {
                        txt_A3.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A4") != null)
                {
                    var txt_A4 = (TextBox) e.Row.FindControl("txt_A4");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A4.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A4"].ToString();
                            // DataBinder.Eval(e.Row.DataItem, "A4").ToString();
                    }
                    else
                    {
                        txt_A4.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A5") != null)
                {
                    var txt_A5 = (TextBox) e.Row.FindControl("txt_A5");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A5.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A5"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A5").ToString();
                    }
                    else
                    {
                        txt_A5.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A6") != null)
                {
                    var txt_A6 = (TextBox) e.Row.FindControl("txt_A6");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A6.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A6"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A6").ToString();
                    }
                    else
                    {
                        txt_A6.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A7") != null)
                {
                    var txt_A7 = (TextBox) e.Row.FindControl("txt_A7");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A7.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A7"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A7").ToString();
                    }
                    else
                    {
                        txt_A7.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A8") != null)
                {
                    var txt_A8 = (TextBox) e.Row.FindControl("txt_A8");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A8.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A8"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A8").ToString();
                    }
                    else
                    {
                        txt_A8.Text = string.Empty;
                    }
                }

                // Accountcode
                if (e.Row.FindControl("txt_A9") != null)
                {
                    var txt_A9 = (TextBox) e.Row.FindControl("txt_A9");

                    if (dsAccountMapp.Tables[accountMapp.TableName].Rows.Count > 0)
                    {
                        txt_A9.Text = dsAccountMapp.Tables[accountMapp.TableName].Rows[0]["A9"].ToString();
                            //DataBinder.Eval(e.Row.DataItem, "A9").ToString();
                    }
                    else
                    {
                        txt_A9.Text = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        ///     Get primary key
        /// </summary>
        /// <param name="dsUnit"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsAccountMapp)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsAccountMapp.Tables[accountMapp.TableName].Columns["ID"];

            return primaryKeys;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "SAVE":
                        var saved = Save();

                        if (saved)
                        {
                            // show messagebox
                        }
                        break;

                    case "PRINT":
                        var objArrList = new ArrayList();
                        objArrList.Add(cmb_Store.SelectedItem.Value.ToString());
                        objArrList.Add(cmb_Type.SelectedItem.Value.ToString());
                        Session["s_arrNo"] = objArrList;


                        var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=230" + "&BuCode=" +
                                         LoginInfo.BuInfo.BuCode;
                        Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("<script>");
                        //Response.Write("window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("</script>");
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void Back()
        {
            Response.Redirect("../../../../IN/Default.aspx");
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            Page.Validate();
            if (Page.IsValid)
            {
                dsAccountMapp.Clear();
                accountMapp.GetAccountMappingListByBusinessUnitAndStore(dsAccountMapp, LoginInfo.BuInfo.BuCode,
                    cmb_Store.SelectedItem.Value.ToString(), LoginInfo.ConnStr);

                // Delete the record for override changes.
                for (var i = dsAccountMapp.Tables[accountMapp.TableName].Rows.Count - 1; i >= 0; i--)
                {
                    var draccountMapp = dsAccountMapp.Tables[accountMapp.TableName].Rows[i];

                    if (draccountMapp.RowState != DataRowState.Deleted)
                    {
                        if ((draccountMapp["BusinessUnitCOde"].ToString().ToUpper() == LoginInfo.BuInfo.BuCode.ToUpper())
                            &&
                            (draccountMapp["StoreCode"].ToString().ToUpper() ==
                             cmb_Store.SelectedItem.Value.ToString().ToUpper()))
                        {
                            draccountMapp.Delete();
                        }
                    }
                }

                // For Summary Information.
                if (grd_AccountMapp.Rows.Count > 0)
                {
                    foreach (GridViewRow drReadAccMap in grd_AccountMapp.Rows)
                    {
                        var drAccMap = dsAccountMapp.Tables[accountMapp.TableName].NewRow();

                        drAccMap["ID"] = Guid.NewGuid();

                        if (drReadAccMap.FindControl("hid_BusinessUnitCode") != null)
                        {
                            var hid_BusinessUnitCode = (HiddenField) drReadAccMap.FindControl("hid_BusinessUnitCode");
                            drAccMap["BusinessUnitCode"] = (hid_BusinessUnitCode.Value == string.Empty
                                ? string.Empty
                                : hid_BusinessUnitCode.Value);
                        }

                        if (drReadAccMap.FindControl("hid_StoreCode") != null)
                        {
                            var hid_StoreCode = (HiddenField) drReadAccMap.FindControl("hid_StoreCode");
                            drAccMap["StoreCode"] = (hid_StoreCode.Value == string.Empty
                                ? string.Empty
                                : hid_StoreCode.Value);
                        }

                        // Needed to work later
                        drAccMap["CategoryCode"] = System.DBNull.Value;
                        drAccMap["SubCategoryCode"] = System.DBNull.Value;

                        if (drReadAccMap.FindControl("hid_ItemGroupCode") != null)
                        {
                            var hid_ItemGroupCode = (HiddenField) drReadAccMap.FindControl("hid_ItemGroupCode");
                            drAccMap["ItemGroupCode"] = (hid_ItemGroupCode.Value == string.Empty
                                ? string.Empty
                                : hid_ItemGroupCode.Value);
                        }


                        if (drReadAccMap.FindControl("txt_A1") != null)
                        {
                            var txt_A1 = (TextBox) drReadAccMap.FindControl("txt_A1");
                            drAccMap["A1"] = (txt_A1.Text == string.Empty ? string.Empty : txt_A1.Text);
                        }


                        if (drReadAccMap.FindControl("txt_A2") != null)
                        {
                            var txt_A2 = (TextBox) drReadAccMap.FindControl("txt_A2");
                            drAccMap["A2"] = (txt_A2.Text == string.Empty ? string.Empty : txt_A2.Text);
                        }


                        if (drReadAccMap.FindControl("txt_A3") != null)
                        {
                            var txt_A3 = (TextBox) drReadAccMap.FindControl("txt_A3");
                            drAccMap["A3"] = (txt_A3.Text == string.Empty ? string.Empty : txt_A3.Text);
                        }


                        if (drReadAccMap.FindControl("txt_A4") != null)
                        {
                            var txt_A4 = (TextBox) drReadAccMap.FindControl("txt_A4");
                            drAccMap["A4"] = (txt_A4.Text == string.Empty ? string.Empty : txt_A4.Text);
                        }

                        if (drReadAccMap.FindControl("txt_A5") != null)
                        {
                            var txt_A5 = (TextBox) drReadAccMap.FindControl("txt_A5");
                            drAccMap["A5"] = (txt_A5.Text == string.Empty ? string.Empty : txt_A5.Text);
                        }

                        if (drReadAccMap.FindControl("txt_A6") != null)
                        {
                            var txt_A6 = (TextBox) drReadAccMap.FindControl("txt_A6");
                            drAccMap["A6"] = (txt_A6.Text == string.Empty ? string.Empty : txt_A6.Text);
                        }

                        if (drReadAccMap.FindControl("txt_A7") != null)
                        {
                            var txt_A7 = (TextBox) drReadAccMap.FindControl("txt_A7");
                            drAccMap["A7"] = (txt_A7.Text == string.Empty ? string.Empty : txt_A7.Text);
                        }

                        if (drReadAccMap.FindControl("txt_A8") != null)
                        {
                            var txt_A8 = (TextBox) drReadAccMap.FindControl("txt_A8");
                            drAccMap["A8"] = (txt_A8.Text == string.Empty ? string.Empty : txt_A8.Text);
                        }

                        if (drReadAccMap.FindControl("txt_A9") != null)
                        {
                            var txt_A9 = (TextBox) drReadAccMap.FindControl("txt_A9");
                            drAccMap["A9"] = (txt_A9.Text == string.Empty ? string.Empty : txt_A9.Text);
                        }


                        // Add new record
                        dsAccountMapp.Tables[accountMapp.TableName].Rows.Add(drAccMap);
                    }
                }
            }


            var success = accountMapp.Save(dsAccountMapp, LoginInfo.ConnStr);

            if (success)
            {
                return true;
            }
            // Display error
            //this.MessageBox("Cann't perform successfully!");
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_BusinessUnitCode_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Radio button checked change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Type_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dsAccountCategory.Clear();

            accountMapp.GetList(dsAccountCategory, cmb_Store.SelectedItem.Value.ToString(),
                cmb_Type.SelectedItem.Value.ToString(), LoginInfo.ConnStr);

            Session["dsAccountCategory"] = dsAccountCategory;

            Page_Setting();
        }

        /// <summary>
        ///     Store loaction selectedindex changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dsAccountCategory != null)
            {
                dsAccountCategory.Clear();
            }

            if (dsAccountMapp != null)
            {
                dsAccountMapp.Clear();
            }

            accountMapp.GetList(dsAccountCategory, cmb_Store.SelectedItem.Value.ToString(),
                cmb_Type.SelectedItem.Value.ToString(), LoginInfo.ConnStr);

            Session["dsAccountCategory"] = dsAccountCategory;

            Page_Setting();
        }

        /// <summary>
        ///     Checkbox1 for Accountcode1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A1_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A1 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A1");

            if (chk_A1.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A1 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A1");

                    txt_A1.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A1 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A1");

                    txt_A1.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Checkbox2 for Accountcode2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A2_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A2 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A2");

            if (chk_A2.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A2 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A2");

                    txt_A2.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A2 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A2");

                    txt_A2.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Checkbox3 for Accountcode3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A3_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A3 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A3");

            if (chk_A3.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A3 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A3");

                    txt_A3.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A3 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A3");

                    txt_A3.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Checkbox4 for Accountcode4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A4_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A4 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A4");

            if (chk_A4.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A4 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A4");

                    txt_A4.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A4 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A4");

                    txt_A4.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Checkbox5 for Accountcode5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A5_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A5 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A5");

            if (chk_A5.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A5 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A5");

                    txt_A5.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A5 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A5");

                    txt_A5.Enabled = false;
                }
            }
        }

        /// <summary>
        ///     Checkbox6 for Accountcode6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A6_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A6 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A6");

            if (chk_A6.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A6 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A6");

                    txt_A6.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A6 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A6");

                    txt_A6.Enabled = false;
                }
            }
        }

        /// Checkbox7 for Accountcode7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A7_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A7 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A7");

            if (chk_A7.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A7 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A7");

                    txt_A7.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A7 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A7");

                    txt_A7.Enabled = false;
                }
            }
        }

        /// Checkbox8 for Accountcode8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A8_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A8 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A8");

            if (chk_A8.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A8 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A8");
                    txt_A8.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A8 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A8");
                    txt_A8.Enabled = false;
                }
            }
        }

        /// Checkbox8 for Accountcode8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_A9_OnCheckedChanged(object sender, EventArgs e)
        {
            var chk_A9 = (CheckBox) grd_AccountMapp.HeaderRow.FindControl("chk_A9");

            if (chk_A9.Checked)
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A9 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A9");
                    txt_A9.Enabled = true;
                }
            }
            else
            {
                for (var iii = 0; iii < grd_AccountMapp.Rows.Count; iii++)
                {
                    var txt_A9 = (TextBox) grd_AccountMapp.Rows[iii].FindControl("txt_A9");
                    txt_A9.Enabled = false;
                }
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        #endregion
    }
}