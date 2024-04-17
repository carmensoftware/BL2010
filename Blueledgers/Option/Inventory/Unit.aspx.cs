using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class Unit : BasePage
    {
        #region "Attributes"

        private DataSet dsUnit = new DataSet();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        #endregion

        #region "Operations"

        private string UnitMode
        {
            get { return ViewState["UnitMode"].ToString(); }
            set { ViewState["UnitMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsUnit = (DataSet)Session["dsUnit"];
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsUnit.Clear();

            var getUnit = unit.GetList(dsUnit, LoginInfo.ConnStr);

            if (getUnit)
            {
                // Assign Primarykey                
                dsUnit.Tables[unit.TableName].PrimaryKey = GetPK();
                Session["dsUnit"] = dsUnit;
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
            // Asp GridView.
            string filter = string.Format("IsActived = '{0}'", true);
            dsUnit.Tables[unit.TableName].DefaultView.RowFilter = filter;
            grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit1.EditIndex = -1;
            grd_Unit1.DataBind();

            menu_ItemClick.Items[0].Enabled = true; //Create
            menu_ItemClick.Items[1].Enabled = true; //Delete
        }

        /// <summary>
        ///     Define statement for create/delete/print
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick_Click(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
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

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void Create()
        {
            //grd_Unit.AddNewRow();

            // Asp Gridview.
            var drNew = dsUnit.Tables[unit.TableName].NewRow();

            drNew["UnitCode"] = string.Empty;
            drNew["IsActived"] = true;


            dsUnit.Tables[unit.TableName].Rows.Add(drNew);

            grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit1.EditIndex = dsUnit.Tables[unit.TableName].Rows.Count - 1;
            grd_Unit1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = false; //Create
            menu_ItemClick.Items[1].Enabled = false; //Delete

            //2016-10-31 Maneetip
            ASPxPopupCreate.ShowOnPageLoad = true;

            //UnitMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            //if (grd_Unit.Selection.Count > 0)
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
            //List<object> columnValues = grd_Unit.GetSelectedFieldValues("UnitCode");


            //foreach (string delUnitCode in columnValues)
            //{
            //    foreach (DataRow drDeleting in dsUnit.Tables[unit.TableName].Rows)
            //    {
            //        if (drDeleting.RowState != DataRowState.Deleted)
            //        {
            //            if (drDeleting["UnitCode"].ToString().ToUpper() == delUnitCode.ToUpper())
            //            {
            //                drDeleting.Delete();
            //            }
            //        }
            //    }
            //}

            for (var i = grd_Unit1.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_Unit1.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;

                if (Chk_Item.Checked)
                {
                    dsUnit = (DataSet)Session["dsUnit"];

                    dsUnit.Tables[unit.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveUnit = unit.Save(dsUnit, LoginInfo.ConnStr);

            if (saveUnit)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                Page_Retrieve();
            }
            else
            {
                pop_WarningDelete.ShowOnPageLoad = true;
                return;
            }
        }

        /// <summary>
        ///     Canceling Delete Delivery Point and Deselect all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_Unit.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Print Unit List
        /// </summary>
        //private void Print()
        //{
        //    var objArrList = new ArrayList();

        //    var sb = new StringBuilder();

        //    for (var i = 0; i < dsUnit.Tables[unit.TableName].Rows.Count; i++)
        //    {
        //        var ListID = dsUnit.Tables[unit.TableName].Rows[i]["UnitCode"].ToString();
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
        //    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=180" + "&BuCode=" +
        //                     LoginInfo.BuInfo.BuCode.ToString();
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

        #region "grd_Unit"

        ///// <summary>
        ///// Re-binding Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Unit_OnLoad(object sender, EventArgs e)
        //{
        //    grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
        //    grd_Unit.DataBind();
        //}

        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["UnitCode"] = string.Empty;
            e.NewValues["Name"] = string.Empty;
            e.NewValues["IsActived"] = true;
            e.NewValues["CreatedBy"] = LoginInfo.LoginName;
            e.NewValues["CreatedDate"] = ServerDateTime;
            e.NewValues["UpdatedBy"] = LoginInfo.LoginName;
            e.NewValues["UpdatedDate"] = ServerDateTime;
        }

        ///// <summary>
        ///// Create New Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Unit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsUnit.Tables[unit.TableName].NewRow();

        //    drInserting["UnitCode"]     = e.NewValues["UnitCode"].ToString();
        //    drInserting["Name"]         = e.NewValues["Name"].ToString();
        //    drInserting["IsActived"]    = e.NewValues["IsActived"].ToString();
        //    drInserting["CreatedBy"]    = LoginInfo.LoginName;
        //    drInserting["CreatedDate"]  = ServerDateTime;
        //    drInserting["UpdatedBy"]    = LoginInfo.LoginName;
        //    drInserting["UpdatedDate"]  = ServerDateTime;

        //    dsUnit.Tables[unit.TableName].Rows.Add(drInserting);

        //    // Save to database
        //    bool saveUnit = unit.Save(dsUnit, LoginInfo.ConnStr);

        //    if (saveUnit)
        //    {
        //        //grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
        //        //grd_Unit.CancelEdit();
        //        //grd_Unit.DataBind();

        //        e.Cancel = true;

        //        Session["dsUnit"] = dsUnit;
        //    }
        //    else
        //    { 
        //        // Display Error Message    
        //        dsUnit.Tables[unit.TableName].RejectChanges();
        //        grd_Unit.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        ///// <summary>
        ///// Save Existing Unit Changed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Unit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    DataRow drUpdating = dsUnit.Tables[unit.TableName].Rows[grd_Unit.EditingRowVisibleIndex];

        //    drUpdating["Name"]          = e.NewValues["Name"].ToString();
        //    drUpdating["IsActived"]     = e.NewValues["IsActived"].ToString();
        //    drUpdating["UpdatedBy"]     = LoginInfo.LoginName;
        //    drUpdating["UpdatedDate"]   = ServerDateTime;

        //    // Save to database
        //    bool saveUnit = unit.Save(dsUnit, LoginInfo.ConnStr);

        //    if (saveUnit)
        //    {
        //        grd_Unit.DataSource = dsUnit.Tables[unit.TableName];
        //        grd_Unit.CancelEdit();
        //        grd_Unit.DataBind();

        //        e.Cancel = true;

        //        Session["dsUnit"] = dsUnit;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsUnit.Tables[unit.TableName].RejectChanges();
        //        grd_Unit.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsUnit.Tables[unit.TableName].Columns["UnitCode"];

            return primaryKeys;
        }

        #endregion

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
            pop_ConfrimDelete.ShowOnPageLoad = false;

            return;
            //this.Page_Retrieve();
        }

        // ASP Grid View.
        protected void grd_Unit1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Edit
                //if (e.Row.FindControl("lnkb_Edit") != null)
                //{
                //    LinkButton lnkb_Edit = (LinkButton)e.Row.FindControl("lnkb_Edit");
                //    string CommentOwner = (string)DataBinder.Eval(e.Row.DataItem, "CreatedBy");
                //    if (CommentOwner != LoginInfo.LoginName)
                //    {
                //        lnkb_Edit.Enabled = false;
                //    }
                //    else
                //    {
                //        lnkb_Edit.Enabled = true;
                //    }
                //}

                // Code.
                if (e.Row.FindControl("lbl_Code") != null)
                {
                    var lbl_Code = e.Row.FindControl("lbl_Code") as Label;
                    lbl_Code.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                }
                //if (e.Row.FindControl("txt_Code") != null)
                //{
                //    var txt_Code = e.Row.FindControl("txt_Code") as TextBox;
                //    txt_Code.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                //}
                if (e.Row.FindControl("lbl_txtCode") != null)
                {
                    var lbl_txtCode = e.Row.FindControl("lbl_txtCode") as Label;
                    lbl_txtCode.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                }

                // Description.
                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }
                if (e.Row.FindControl("txt_Desc") != null)
                {
                    var txt_Desc = e.Row.FindControl("txt_Desc") as TextBox;
                    txt_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }

                // IsAvctived.
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
                        chk_Actived.Checked = false;
                    }
                }
            }
        }

        protected void grd_Unit1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsUnit = (DataSet)Session["dsUnit"];

            //var txt_Code = grd_Unit1.Rows[e.RowIndex].FindControl("txt_Code") as TextBox;
            var lbl_txtCode = grd_Unit1.Rows[e.RowIndex].FindControl("lbl_txtCode") as Label;
            var txt_Desc = grd_Unit1.Rows[e.RowIndex].FindControl("txt_Desc") as TextBox;
            var chk_Actived = grd_Unit1.Rows[e.RowIndex].FindControl("chk_Actived") as CheckBox;

            if (UnitMode == "NEW")
            {

                //var drNew = dsUnit.Tables[unit.TableName].Rows[grd_Unit1.EditIndex];
                //var CountUnit = unit.Get_CountByUnitCode(txt_Code.Text.ToString(), LoginInfo.ConnStr);

                //if (CountUnit > 0)
                //{
                //    lbl_Warning.Text = "Unit Code is exists.";
                //    pop_WarningDelete.ShowOnPageLoad = true;
                //    return;
                //}
                //else
                //{
                //    drNew["UnitCode"] = txt_Code.Text.ToString();
                //}

                //drNew["Name"] = txt_Desc.Text.ToString();
                //drNew["IsActived"] = chk_Actived.Checked;
                //drNew["CreatedBy"] = LoginInfo.LoginName;
                //drNew["CreatedDate"] = ServerDateTime;
                //drNew["UpdatedBy"] = LoginInfo.LoginName;
                //drNew["UpdatedDate"] = ServerDateTime;

                //var save = unit.Save(dsUnit, LoginInfo.ConnStr);

                //if (save)
                //{
                //    grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
                //    grd_Unit1.EditIndex = -1;
                //    grd_Unit1.DataBind();
                //}

                //Page_Retrieve();
            }
            else
            {
                var drUpdating = dsUnit.Tables[unit.TableName].Rows[e.RowIndex];
                //drUpdating["UnitCode"] = txt_Code.Text.ToString();
                //drUpdating["UnitCode"] = lbl_txtCode.Text.ToString();
                //Maneetip Edited on 2016-11-01 = Because, Error told that UnitCode is unique.

                drUpdating["Name"] = txt_Desc.Text.ToString();
                drUpdating["IsActived"] = chk_Actived.Checked;
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                drUpdating["UpdatedDate"] = ServerDateTime;

                var save = unit.Save(dsUnit, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
                    grd_Unit1.EditIndex = -1;
                    grd_Unit1.DataBind();
                }
            }

            //Disable 
            menu_ItemClick.Items[0].Enabled = true; //Create
            menu_ItemClick.Items[1].Enabled = true; //Delete

            UnitMode = string.Empty;
        }

        protected void grd_Unit1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsUnit = (DataSet)Session["dsUnit"];

            if (UnitMode == "NEW")
            {
                dsUnit.Tables[unit.TableName].Rows[dsUnit.Tables[unit.TableName].Rows.Count - 1].Delete();
            }

            if (UnitMode == "EDIT")
            {
                dsUnit.Tables[unit.TableName].Rows[dsUnit.Tables[unit.TableName].Rows.Count - 1].CancelEdit();
            }

            grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit1.EditIndex = -1;
            grd_Unit1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = true;
            menu_ItemClick.Items[1].Enabled = true;

            UnitMode = string.Empty;
        }

        protected void grd_Unit1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsUnit = (DataSet)Session["dsUnit"];

            grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit1.EditIndex = e.NewEditIndex;
            grd_Unit1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = false; //Create
            menu_ItemClick.Items[1].Enabled = false; //Delete

            UnitMode = "EDIT";
        }


        # region "ADDED by Maneetip"
        protected void btnCancel_create_Click(object sender, EventArgs e)
        {
            ASPxPopupCreate.ShowOnPageLoad = false;
            Response.Redirect("Unit.aspx");
        }
        protected void btnSave_create_Click(object sender, EventArgs e)
        {
            var drNew = dsUnit.Tables[unit.TableName].Rows[grd_Unit1.EditIndex];
            var CountUnit = unit.Get_CountByUnitCode(txtCode.Text.ToString(), LoginInfo.ConnStr);
            if (CountUnit > 0)
            {
                lbl_Warning.Text = "Unit Code is exists.";
                pop_WarningDelete.ShowOnPageLoad = true;
                return;
            }
            else
            { drNew["UnitCode"] = txtCode.Text.ToString(); }

            if (txtDesc.Text != string.Empty)
            { drNew["Name"] = txtDesc.Text.ToString(); }
            else { drNew["Name"] = txtCode.Text.ToString(); }

            drNew["IsActived"] = cbActive.Checked;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["CreatedDate"] = ServerDateTime;
            drNew["UpdatedBy"] = LoginInfo.LoginName;
            drNew["UpdatedDate"] = ServerDateTime;

            var save = unit.Save(dsUnit, LoginInfo.ConnStr);
            if (!save)
            {
                lbl_Warning.Text = "Save failed!";
                pop_WarningDelete.ShowOnPageLoad = true;
            }
            Response.Redirect("Unit.aspx");
        }

        protected void ddlActiveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterIsActiveANDTextbox();
        }
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            filterIsActiveANDTextbox();
        }

        protected void filterIsActiveANDTextbox()
        {
            string filter = string.Empty;
            if (txtSearch.Text != string.Empty)
            {
                string Search = string.Format("(UnitCode like '%{0}%' OR ", txtSearch.Text);
                Search += string.Format("Name Like '%{0}%')", txtSearch.Text);
                filter = string.Format(Search + "AND ");
            }

            if (ddlActiveStatus.SelectedItem.Text == "Only Active")
            { filter += string.Format("IsActived = '{0}'", true); }
            else if (ddlActiveStatus.SelectedItem.Text == "All")
            { filter += string.Format("(IsActived = '{0}' OR IsActived = '{1}')", true, false); }

            dsUnit.Tables[unit.TableName].DefaultView.RowFilter = filter;
            grd_Unit1.DataSource = dsUnit.Tables[unit.TableName];
            grd_Unit1.DataBind();
        }
        #endregion
    }
}