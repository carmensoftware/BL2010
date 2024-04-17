using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class DeliPoint : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.StoreLct StoreLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private DataSet dsDeliPoint = new DataSet();
        private DataTable dtStoreLct = new DataTable();

        #endregion

        #region "Operations"

        private string DeliMode
        {
            get { return ViewState["DeliMode"].ToString(); }
            set { ViewState["DeliMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsDeliPoint = (DataSet) Session["dsDeliPoint"];
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsDeliPoint.Clear();

            var getUnit = deliPoint.GetList(dsDeliPoint, LoginInfo.ConnStr);

            if (getUnit)
            {
                // Assign Primarykey                
                dsDeliPoint.Tables[deliPoint.TableName].PrimaryKey = GetPK();

                Session["dsDeliPoint"] = dsDeliPoint;
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
            grd_DeliPoint.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint.DataBind();

            // Asp Gridview.
            grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint1.EditIndex = -1;
            grd_DeliPoint1.DataBind();
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
                    //var sb = new StringBuilder();
                    //var grdd = grd_DeliPoint; // Content2.FindControl("grd_DeliPoint");

                    //var ListKey = string.Empty;
                    //for (var i = 0; i < grdd.VisibleRowCount; i++)
                    //{
                    //    var prId = grdd.GetRowValues(i, "DptCode").ToString();
                    //    //sb.Append("'" + prId + "',");
                    //    sb.Append(prId + ",");
                    //}


                    //if (sb.Length > 0)
                    //{
                    //    ListKey = sb.ToString().Substring(0, sb.Length - 1);
                    //}
                    //else
                    //{
                    //    ListKey = "'*'";
                    //}

                    //var dtReport = new DataTable();
                    //var cl = new DataColumn("BUCode");
                    //dtReport.Columns.Add(cl);

                    //cl = new DataColumn("No");
                    //dtReport.Columns.Add(cl);

                    //var dr = dtReport.NewRow();
                    //dr[0] = LoginInfo.BuInfo.BuCode;
                    //dr[1] = ListKey;
                    //dtReport.Rows.Add(dr);

                    //Session["dtBuKeys"] = dtReport;

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=190";
                    //Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                    ////Response.Write("<script>");
                    ////Response.Write("window.open('" + reportLink + "','_blank'  )");
                    ////Response.Write("</script>");

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        //private String GetArrDLPNo()
        //{

        //    StringBuilder sb = new StringBuilder();

        //    ASPxGridView grdd = (ASPxGridView)grd_DeliPoint;// Content2.FindControl("grd_DeliPoint");

        //    for (int i = 0; i < grdd.VisibleRowCount; i++)
        //    {
        //        string prId = grdd.GetRowValues(i, "DptCode").ToString();
        //        //sb.Append("'" + prId + "',");
        //        sb.Append(  prId + ",");
        //    }
        //    if (sb.Length > 0)
        //    {
        //       return  sb.ToString().Substring(0, sb.Length - 1);
        //    }
        //    else
        //    {
        //        return  "'*'";
        //    }
        //}
        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void Create()
        {
            //grd_DeliPoint.Selection.UnselectAll();
            //grd_DeliPoint.AddNewRow();

            // Grid View.
            var drNew = dsDeliPoint.Tables[deliPoint.TableName].NewRow();

            drNew["DptCode"] = deliPoint.GetNewID(LoginInfo.ConnStr);

            dsDeliPoint.Tables[deliPoint.TableName].Rows.Add(drNew);

            grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint1.EditIndex = dsDeliPoint.Tables[deliPoint.TableName].Rows.Count - 1;
            grd_DeliPoint1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = false;
            menu_ItemClick.Items[1].Enabled = false;

            DeliMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            //// cancel all add/edit
            //grd_DeliPoint.CancelEdit();

            //if (grd_DeliPoint.Selection.Count > 0)
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
            //List<object> columnValues = grd_DeliPoint.GetSelectedFieldValues("DptCode");


            //foreach (int delDptCode in columnValues)
            //{
            //    foreach (DataRow drDeleting in dsDeliPoint.Tables[deliPoint.TableName].Rows)
            //    {
            //        if (drDeleting.RowState != DataRowState.Deleted)
            //        {
            //            if (drDeleting["DptCode"].ToString().ToUpper() == delDptCode.ToString().ToUpper())
            //            {
            //                int CountDeli = StoreLct.Get_CountByDeliveryPoint(drDeleting["DptCode"].ToString(), LoginInfo.ConnStr);
            //                if (CountDeli>0)
            //                {
            //                    //Display Error Message
            //                    pop_Warning.ShowOnPageLoad = true;
            //                    pop_ConfrimDelete.ShowOnPageLoad = false;

            //                    return;
            //                }

            //                drDeleting.Delete();
            //            }
            //        }
            //    }
            //}

            for (var i = grd_DeliPoint1.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_DeliPoint1.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsDeliPoint = (DataSet) Session["dsDeliPoint"];

                    var drDeleting = dsDeliPoint.Tables[deliPoint.TableName].Rows[i];

                    var CountDeli = StoreLct.Get_CountByDeliveryPoint(drDeleting["DptCode"].ToString(),
                        LoginInfo.ConnStr);
                    //----02/03/2012----StoreLct.Get_CountByDeliveryPoint2(drDeleting["DptCode"].ToString(), LoginInfo.ConnStr);

                    if (CountDeli > 0)
                    {
                        pop_Warning.ShowOnPageLoad = true;
                        pop_ConfrimDelete.ShowOnPageLoad = false;

                        return;
                    }
                    dsDeliPoint.Tables[deliPoint.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveDeliPoint = deliPoint.Save(dsDeliPoint, LoginInfo.ConnStr);

            if (saveDeliPoint)
            {
                //grd_DeliPoint.Selection.UnselectAll();
                //pop_ConfrimDelete.ShowOnPageLoad = false;
                //grd_DeliPoint.Selection.UnselectAll();

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
            //grd_DeliPoint.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Print Unit List
        /// </summary>
        private void Print()
        {
        }

        #endregion

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "grd_DeliPoint"

        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_DeliPoint_OnLoad(object sender, EventArgs e)
        {
            grd_DeliPoint.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint.DataBind();
        }

        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_DeliPoint_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["Name"] = string.Empty;
            e.NewValues["IsActived"] = true;
            e.NewValues["CreatedBy"] = LoginInfo.LoginName;
            e.NewValues["CreatedDate"] = ServerDateTime;
            e.NewValues["UpdatedBy"] = LoginInfo.LoginName;
            e.NewValues["UpdatedDate"] = ServerDateTime;
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_DeliPoint_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var drInserting = dsDeliPoint.Tables[deliPoint.TableName].NewRow();

            drInserting["DptCode"] = deliPoint.GetNewID(LoginInfo.ConnStr);
            drInserting["Name"] = e.NewValues["Name"].ToString();
            drInserting["IsActived"] = e.NewValues["IsActived"].ToString();
            drInserting["CreatedBy"] = LoginInfo.LoginName;
            drInserting["CreatedDate"] = ServerDateTime;
            drInserting["UpdatedBy"] = LoginInfo.LoginName;
            drInserting["UpdatedDate"] = ServerDateTime;

            dsDeliPoint.Tables[deliPoint.TableName].Rows.Add(drInserting);

            // Save to database
            var saveDeliPoint = deliPoint.Save(dsDeliPoint, LoginInfo.ConnStr);

            if (saveDeliPoint)
            {
                grd_DeliPoint.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
                grd_DeliPoint.CancelEdit();
                grd_DeliPoint.DataBind();

                e.Cancel = true;

                Session["dsDeliPoint"] = dsDeliPoint;
            }
            else
            {
                // Display Error Message    
                dsDeliPoint.Tables[deliPoint.TableName].RejectChanges();
                grd_DeliPoint.CancelEdit();

                e.Cancel = true;
            }
        }

        /// <summary>
        ///     Save Existing Unit Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_DeliPoint_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var drUpdating = dsDeliPoint.Tables[deliPoint.TableName].Rows[grd_DeliPoint.EditingRowVisibleIndex];

            drUpdating["Name"] = e.NewValues["Name"].ToString();
            drUpdating["IsActived"] = e.NewValues["IsActived"].ToString();
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;
            drUpdating["UpdatedDate"] = ServerDateTime;

            // Save to database
            var saveDeliPoint = deliPoint.Save(dsDeliPoint, LoginInfo.ConnStr);

            if (saveDeliPoint)
            {
                grd_DeliPoint.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
                grd_DeliPoint.CancelEdit();
                grd_DeliPoint.DataBind();

                e.Cancel = true;

                Session["dsDeliPoint"] = dsDeliPoint;
            }
            else
            {
                // Display Error Message    
                dsDeliPoint.Tables[deliPoint.TableName].RejectChanges();
                grd_DeliPoint.CancelEdit();

                e.Cancel = true;
            }
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            grd_DeliPoint.Selection.UnselectAll();
            pop_Warning.ShowOnPageLoad = false;
        }

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsDeliPoint.Tables[deliPoint.TableName].Columns["DptCode"];

            return primaryKeys;
        }

        #endregion

        // Grid View.
        protected void grd_DeliPoint1_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    lbl_Code.Text = DataBinder.Eval(e.Row.DataItem, "DptCode").ToString();
                }
                if (e.Row.FindControl("txt_Code") != null)
                {
                    var txt_Code = e.Row.FindControl("txt_Code") as TextBox;
                    txt_Code.Text = DataBinder.Eval(e.Row.DataItem, "DptCode").ToString();
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

        protected void grd_DeliPoint1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsDeliPoint = (DataSet) Session["dsDeliPoint"];

            grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint1.EditIndex = e.NewEditIndex;
            grd_DeliPoint1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = false;
            menu_ItemClick.Items[1].Enabled = false;

            DeliMode = "EDIT";
        }

        protected void grd_DeliPoint1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsDeliPoint = (DataSet) Session["dsDeliPoint"];

            if (DeliMode == "NEW")
            {
                dsDeliPoint.Tables[deliPoint.TableName].Rows[dsDeliPoint.Tables[deliPoint.TableName].Rows.Count - 1]
                    .Delete();
            }

            if (DeliMode == "EDIT")
            {
                dsDeliPoint.Tables[deliPoint.TableName].Rows[dsDeliPoint.Tables[deliPoint.TableName].Rows.Count - 1]
                    .CancelEdit();
            }

            grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
            grd_DeliPoint1.EditIndex = -1;
            grd_DeliPoint1.DataBind();

            //Disable 
            menu_ItemClick.Items[0].Enabled = true;
            menu_ItemClick.Items[1].Enabled = true;

            DeliMode = string.Empty;
        }

        protected void grd_DeliPoint1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsDeliPoint = (DataSet) Session["dsDeliPoint"];

            var txt_Code = grd_DeliPoint1.Rows[e.RowIndex].FindControl("txt_Code") as TextBox;
            var txt_Desc = grd_DeliPoint1.Rows[e.RowIndex].FindControl("txt_Desc") as TextBox;
            var chk_Actived = grd_DeliPoint1.Rows[e.RowIndex].FindControl("chk_Actived") as CheckBox;

            if (DeliMode == "NEW")
            {
                var drNew = dsDeliPoint.Tables[deliPoint.TableName].Rows[grd_DeliPoint1.EditIndex];

                drNew["DptCode"] = txt_Code.Text;
                drNew["Name"] = txt_Desc.Text;
                drNew["IsActived"] = chk_Actived.Checked;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;

                var save = deliPoint.Save(dsDeliPoint, LoginInfo.ConnStr);

                if (save)
                {
                    grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
                    grd_DeliPoint1.EditIndex = -1;
                    grd_DeliPoint1.DataBind();
                }
            }
            else
            {
                var drUpdating = dsDeliPoint.Tables[deliPoint.TableName].Rows[e.RowIndex];

                drUpdating["Name"] = txt_Desc.Text;
                drUpdating["IsActived"] = chk_Actived.Checked;
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                drUpdating["UpdatedDate"] = ServerDateTime;

                var save = deliPoint.Save(dsDeliPoint, LoginInfo.ConnStr);

                if (save)
                {
                    grd_DeliPoint1.DataSource = dsDeliPoint.Tables[deliPoint.TableName];
                    grd_DeliPoint1.EditIndex = -1;
                    grd_DeliPoint1.DataBind();
                }
            }

            //Disable 
            menu_ItemClick.Items[0].Enabled = true;
            menu_ItemClick.Items[1].Enabled = true;

            DeliMode = string.Empty;
        }
    }
}