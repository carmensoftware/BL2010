using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option
{
    public partial class Currency : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private DataSet dsCurrency = new DataSet();
        private readonly string moduleID = "2.9.8"; // ModuleId of [PC]

        private string CurrencyMode
        {
            get { return ViewState["CurrencyMode"].ToString(); }
            set { ViewState["CurrencyMode"] = value; }
        }

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsCurrency = (DataSet)Session["dsCurrency"];
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsCurrency.Clear();

            var getCurrency = currency.GetList(dsCurrency, LoginInfo.ConnStr);

            if (getCurrency)
            {
                // Assign Primarykey                
                dsCurrency.Tables[currency.TableName].PrimaryKey = GetPK();

                Session["dsCurrency"] = dsCurrency;
            }
            else
            {
                // Display Error Message
                return;
            }

            Page_Setting();
        }

        /// <summary>
        ///     Display Unit Data.
        /// </summary>
        private void Page_Setting()
        {
            //grd_Currency.DataSource = dsCurrency.Tables[currency.TableName];
            //grd_Currency.DataBind();

            grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
            grd_Currency2.DataBind();
            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3)
                ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 7)
                ? menu_CmdBar.Items.FindByName("Delete").Visible : false;
        }

        /// <summary>
        ///     Define statement for create/delete/print
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>


        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void Create()
        {
            // Disable Create & Delete
            menu_CmdBar.Items[0].Enabled = false; // Create
            menu_CmdBar.Items[1].Enabled = false; // Delete

            // Add new stkInDt row
            var drNew = dsCurrency.Tables[currency.TableName].NewRow();

            drNew["CurrencyCode"] = string.Empty;

            dsCurrency.Tables[currency.TableName].Rows.Add(drNew);

            grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
            grd_Currency2.EditIndex = dsCurrency.Tables[currency.TableName].Rows.Count - 1;
            grd_Currency2.DataBind();

            CurrencyMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            //if (grd_Currency.Selection.Count > 0)
            //{
            //    pop_ConfrimDelete.ShowOnPageLoad = true;
            //}

            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Delete selected Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_Currency.GetSelectedFieldValues("CurrencyCode");

            //foreach (string delCurrencyCode in columnValues)
            //{
            //    foreach (DataRow drDeleting in dsCurrency.Tables[currency.TableName].Rows)
            //    {
            //        if (drDeleting.RowState != DataRowState.Deleted)
            //        {
            //            if (drDeleting["CurrencyCode"].ToString().ToUpper() == delCurrencyCode.ToUpper())
            //            {
            //                drDeleting.Delete();
            //            }
            //        }
            //    }
            //}

            for (var i = grd_Currency2.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_Currency2.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsCurrency = (DataSet)Session["dsCurrency"];

                    dsCurrency.Tables[currency.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveCurrency = currency.Save(dsCurrency, LoginInfo.ConnStr);

            if (saveCurrency)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                Page_Retrieve();
            }
        }

        /// <summary>
        ///     Canceling Delete Delivery Point and Deselect all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_Currency.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Print Unit List
        /// </summary>
        //private void Print()
        //{
        //    var objArrList = new ArrayList();

        //    var sb = new StringBuilder();

        //    for (var i = 0; i < dsCurrency.Tables[currency.TableName].Rows.Count; i++)
        //    {
        //        var ListID = dsCurrency.Tables[currency.TableName].Rows[i]["CurrencyCode"].ToString();
        //        sb.Append("'" + ListID + "',");
        //    }
        //    if (sb.Length > 0)
        //    {
        //        objArrList.Add(sb.ToString().Substring(0, sb.Length - 1));
        //    }
        //    else
        //    {
        //        objArrList.Add('*');
        //    }

        //    Session["s_arrNo"] = objArrList;
        //    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=220" + "&BuCode=" +
        //                     LoginInfo.BuInfo.BuCode;
        //    ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //        "<script>window.open('" + reportLink + "','_blank')</script>");
        //}

        #endregion

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "grd_Currency"

        ///// <summary>
        ///// Re-binding Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Currency_OnLoad(object sender, EventArgs e)
        //{
        //    grd_Currency.DataSource = dsCurrency.Tables[currency.TableName];
        //    grd_Currency.DataBind();
        //}

        ///// <summary>
        ///// Assign Default Value for New Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Currency_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    e.NewValues["CurrencyCode"] = string.Empty;
        //    e.NewValues["Desc"]         = string.Empty;
        //    e.NewValues["OthDesc"]      = string.Empty;
        //    e.NewValues["IsActived"]    = true;
        //    e.NewValues["CreatedBy"]    = LoginInfo.LoginName;
        //    e.NewValues["CreatedDate"]  = ServerDateTime;
        //    e.NewValues["UpdatedBy"]    = LoginInfo.LoginName;
        //    e.NewValues["UpdatedDate"]  = ServerDateTime;
        //}

        ///// <summary>
        ///// Create New Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Currency_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsCurrency.Tables[currency.TableName].NewRow();

        //    drInserting["CurrencyCode"] = e.NewValues["CurrencyCode"].ToString();
        //    drInserting["Desc"]         = e.NewValues["Desc"].ToString();
        //    drInserting["OthDesc"]      = e.NewValues["Desc"].ToString();
        //    drInserting["IsActived"]    = e.NewValues["IsActived"].ToString();
        //    drInserting["CreatedBy"]    = LoginInfo.LoginName;
        //    drInserting["CreatedDate"]  = ServerDateTime;
        //    drInserting["UpdatedBy"]    = LoginInfo.LoginName;
        //    drInserting["UpdatedDate"]  = ServerDateTime;

        //    dsCurrency.Tables[currency.TableName].Rows.Add(drInserting);

        //    // Save to database
        //    bool saveCurrency = currency.Save(dsCurrency, LoginInfo.ConnStr);

        //    if (saveCurrency)
        //    {
        //        grd_Currency.DataSource = dsCurrency.Tables[currency.TableName];
        //        grd_Currency.CancelEdit();
        //        grd_Currency.DataBind();

        //        e.Cancel = true;

        //        Session["dsCurrency"] = dsCurrency;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsCurrency.Tables[currency.TableName].RejectChanges();
        //        grd_Currency.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        ///// <summary>
        ///// Save Existing Unit Changed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Currency_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    DataRow drUpdating = dsCurrency.Tables[currency.TableName].Rows[grd_Currency.EditingRowVisibleIndex];

        //    drUpdating["Desc"]          = e.NewValues["Desc"].ToString();
        //    drUpdating["OthDesc"]       = e.NewValues["OthDesc"].ToString();
        //    drUpdating["IsActived"]     = e.NewValues["IsActived"].ToString();
        //    drUpdating["UpdatedBy"]     = LoginInfo.LoginName;
        //    drUpdating["UpdatedDate"]   = ServerDateTime;

        //    // Save to database
        //    bool saveCurrency = currency.Save(dsCurrency, LoginInfo.ConnStr);

        //    if (saveCurrency)
        //    {
        //        grd_Currency.DataSource = dsCurrency.Tables[currency.TableName];
        //        grd_Currency.CancelEdit();
        //        grd_Currency.DataBind();

        //        e.Cancel = true;

        //        Session["dsCurrency"] = dsCurrency;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsCurrency.Tables[currency.TableName].RejectChanges();
        //        grd_Currency.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsCurrency.Tables[currency.TableName].Columns["CurrencyCode"];

            return primaryKeys;
        }

        #endregion

        protected void grd_Currency2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsCurrency = (DataSet)Session["dsCurrency"];

            if (CurrencyMode == "NEW")
            {
                dsCurrency.Tables[currency.TableName].Rows[dsCurrency.Tables[currency.TableName].Rows.Count - 1].Delete();
            }

            if (CurrencyMode == "EDIT")
            {
                dsCurrency.Tables[currency.TableName].Rows[dsCurrency.Tables[currency.TableName].Rows.Count - 1]
                    .CancelEdit();
            }

            grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
            grd_Currency2.EditIndex = -1;
            grd_Currency2.DataBind();

            // Enable Create & Delete
            menu_CmdBar.Items[0].Enabled = true; // Create
            menu_CmdBar.Items[1].Enabled = true; // Delete

            CurrencyMode = string.Empty;
        }

        protected void grd_Currency2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsCurrency = (DataSet)Session["dsCurrency"];

            grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
            grd_Currency2.EditIndex = e.NewEditIndex;
            grd_Currency2.DataBind();

            for (var i = grd_Currency2.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_Currency2.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                var Chk_All = grd_Currency2.HeaderRow.FindControl("Chk_All") as CheckBox;

                Chk_All.Enabled = false;
                Chk_Item.Enabled = false;
            }

            var txt_Code = grd_Currency2.Rows[grd_Currency2.EditIndex].FindControl("txt_Code") as TextBox;
            txt_Code.Enabled = false;

            // Disable Create & Delete
            menu_CmdBar.Items[0].Enabled = false; // Create
            menu_CmdBar.Items[1].Enabled = false; // Delete

            CurrencyMode = "EDIT";
        }

        protected void grd_Currency2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsCurrency = (DataSet)Session["dsCurrency"];

            var txt_Code = grd_Currency2.Rows[e.RowIndex].FindControl("txt_Code") as TextBox;
            var txt_Desc = grd_Currency2.Rows[e.RowIndex].FindControl("txt_Desc") as TextBox;
            var chk_Actived = grd_Currency2.Rows[e.RowIndex].FindControl("chk_Actived") as CheckBox;

            if (CurrencyMode == "NEW")
            {
                var drNew = dsCurrency.Tables[currency.TableName].Rows[grd_Currency2.EditIndex];

                drNew["CurrencyCode"] = txt_Code.Text;
                drNew["Desc"] = txt_Desc.Text;
                drNew["IsActived"] = chk_Actived.Checked;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;

                //int countTypeCode = adjType.Get_CountByCodeType(drNew["AdjType"].ToString(), drNew["AdjCode"].ToString(), LoginInfo.ConnStr);

                //if (countTypeCode > 0)
                //{
                //    lbl_Warning.Text = "Can not to Insert data.";
                //    pop_Warning.ShowOnPageLoad = true;
                //    return;
                //}
                //else
                //{
                var save = currency.Save(dsCurrency, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
                    grd_Currency2.EditIndex = -1;
                    grd_Currency2.DataBind();
                }

                Page_Retrieve();
                //}
            }
            else
            {
                var drUpdating = dsCurrency.Tables[currency.TableName].Rows[e.RowIndex];

                drUpdating["Desc"] = txt_Desc.Text;
                drUpdating["IsActived"] = chk_Actived.Checked;
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                drUpdating["UpdatedDate"] = ServerDateTime;

                var save = currency.Save(dsCurrency, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
                    grd_Currency2.EditIndex = -1;
                    grd_Currency2.DataBind();
                }
            }

            grd_Currency2.DataSource = dsCurrency.Tables[currency.TableName];
            grd_Currency2.EditIndex = -1;
            grd_Currency2.DataBind();

            // Enable Create & Delete
            menu_CmdBar.Items[0].Enabled = true; // Create
            menu_CmdBar.Items[1].Enabled = true; // Delete

            CurrencyMode = string.Empty;
        }

        protected void grd_Currency2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Added on: 06/10/2017, By: Fon
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (pagePermiss < 3)
                    e.Row.Cells[1].Attributes.Add("style", "display: none;");
            }
            // End Added.

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Added on: 06/10/2017, By: Fon
                if (pagePermiss < 3)
                    e.Row.Cells[1].Attributes.Add("style", "display: none;");
                // End Added.

                // Code.
                if (e.Row.FindControl("lbl_Code") != null)
                {
                    var lbl_Code = e.Row.FindControl("lbl_Code") as Label;
                    lbl_Code.Text = DataBinder.Eval(e.Row.DataItem, "CurrencyCode").ToString();
                }
                if (e.Row.FindControl("txt_Code") != null)
                {
                    var txt_Code = e.Row.FindControl("txt_Code") as TextBox;
                    txt_Code.Text = DataBinder.Eval(e.Row.DataItem, "CurrencyCode").ToString();
                }

                // Description.
                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Desc").ToString();
                }
                if (e.Row.FindControl("txt_Desc") != null)
                {
                    var txt_Desc = e.Row.FindControl("txt_Desc") as TextBox;
                    txt_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Desc").ToString();
                }

                // Actived.
                if (e.Row.FindControl("Img_Btn_ChkBox") != null)
                {
                    var Img_Btn_ChkBox = e.Row.FindControl("Img_Btn_ChkBox") as ImageButton;
                    Img_Btn_ChkBox.Enabled = false;

                    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived")))
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_True.png";
                    }
                    else
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_False.png";
                    }
                }
                if (e.Row.FindControl("chk_Actived") != null)
                {
                    var chk_Actived = e.Row.FindControl("chk_Actived") as CheckBox;
                    if (DataBinder.Eval(e.Row.DataItem, "IsActived") != DBNull.Value)
                    {
                        chk_Actived.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived"));
                    }
                    else
                    {
                        chk_Actived.Checked = true;
                    }
                }
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }
    }
}