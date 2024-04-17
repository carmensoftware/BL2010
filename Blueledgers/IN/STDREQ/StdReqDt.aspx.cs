using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STDREQ
{
    public partial class StdReqDt : BasePage
    {
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StandardRequistion stdReq = new Blue.BL.IN.StandardRequistion();
        private readonly Blue.BL.IN.StandardRequisitionDetail stdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private DataSet dsStdReq = new DataSet();
        private Blue.BL.IN.storeRequisition stoReq = new Blue.BL.IN.storeRequisition();
        private Blue.BL.IN.StoreRequisitionDetail stoReqDt = new Blue.BL.IN.StoreRequisitionDetail();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.10.11";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStdReq = (DataSet)Session["dsStdReq"];
            }
        }

        private void Page_Retrieve()
        {
            var getStdReq = stdReq.Get(dsStdReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            if (!getStdReq)
            {
                return;
            }

            var getStdReqDt = stdReqDt.Get(dsStdReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            if (!getStdReqDt)
            {
                return;
            }

            Session["dsStdReq"] = dsStdReq;

            Page_Setting();
        }

        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            var drStdReq = dsStdReq.Tables[stdReq.TableName].Rows[0];

            lbl_Ref.Text = drStdReq["RefId"].ToString();
            lbl_Desc.Text = drStdReq["Description"].ToString();
            lbl_Store.Text = drStdReq["LocationCode"].ToString();
            lbl_StoreName.Text = strLct.GetName(drStdReq["LocationCode"].ToString(), LoginInfo.ConnStr);
            chk_Status.Checked = bool.Parse(drStdReq["Status"].ToString());

            // Binding Gridview.
            grd_StdReqDt1.DataSource = dsStdReq.Tables[stdReqDt.TableName];
            grd_StdReqDt1.DataBind();

            #region comment
            //// Display Comment
            //PL.UserControls.Comment2 comment = (PL.UserControls.Comment2)((BlueLedger.PL.Master_In_Default)this.Master).FindControl("Comment");
            //comment.Module = "IN";
            //comment.SubModule = "REQ";
            //comment.RefNo = drStdReq["RefId"].ToString();
            //comment.Visible = true;
            //comment.DataBind();

            //// Display Attach
            //PL.UserControls.Attach2 attach = (PL.UserControls.Attach2)((BlueLedger.PL.Master_In_Default)this.Master).FindControl("Attach");
            //attach.BuCode = Request.Params["BuCode"].ToString();
            //attach.ModuleName = "STOREREQ";
            //attach.RefNo = drStdReq["RefId"].ToString();
            //attach.Visible = true;
            //attach.DataBind();

            //// Display Log
            //PL.UserControls.Log2 log = (PL.UserControls.Log2)((BlueLedger.PL.Master_In_Default)this.Master).FindControl("Log");
            //log.Module = "IN";
            //log.SubModule = "STOREREQ";
            //log.RefNo = drStdReq["RefId"].ToString();
            //log.Visible = true;
            //log.DataBind();
            #endregion
            Control_HeaderMenuBar();
        }

        // Added on: 03/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Create").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Create").Visible : false;
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
            menu_CmdBar.Items.FindByName("Delete").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Delete").Visible : false;
        }
        // End Added.

        protected void btn_ConfirmDelete_pop_Click(object sender, EventArgs e)
        {
            foreach (DataRow drStdDt in dsStdReq.Tables[stdReqDt.TableName].Rows)
            {
                drStdDt.Delete();
            }

            foreach (DataRow drStd in dsStdReq.Tables[stdReq.TableName].Rows)
            {
                drStd.Delete();
            }

            //Save To DataBase.
            var saveTemplate = stdReq.Delete(dsStdReq, LoginInfo.ConnStr);

            if (saveTemplate)
            {
                pop_ConfirmDelete.ShowOnPageLoad = false;
                pop_Delete.ShowOnPageLoad = true;
            }
        }

        protected void btn_CancelDelete_pop_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        //Link for Store Requisition
        protected void lnk_Requisition_Click(object sender, EventArgs e)
        {
            Session["dsStdReq"] = Session["dsStdReq"];

            Response.Redirect("~/IN/STOREREQ/StoreReqFromStdReq.aspx?MODE=template&VID=" +
                              Request.Cookies["[IN].[vStoreRequisition]"].Value);
        }

        protected void grd_StdReqDt1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }

                if (e.Row.FindControl("lbl_ProductDesc1") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    var lbl_ProductDesc1 = e.Row.FindControl("lbl_ProductDesc1") as Label;
                    lbl_ProductDesc1.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_ProductDesc2") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    var lbl_ProductDesc2 = e.Row.FindControl("lbl_ProductDesc2") as Label;
                    lbl_ProductDesc2.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_OrderUnit") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    var lbl_OrderUnit = e.Row.FindControl("lbl_OrderUnit") as Label;
                    lbl_OrderUnit.Text = product.GetOrderUnit(
                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
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

                case "EDIT":
                    Edit();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "COPY":
                    Copy();
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        private void Create()
        {
            Response.Redirect("StdReqEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&MODE=NEW&VID=" +
                              Request.Params["VID"]);
        }

        private void Edit()
        {
            Response.Redirect("StdReqEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&MODE=EDIT&ID=" +
                              Request.Params["ID"] +
                              "&VID=" + Request.Params["VID"]);
        }

        private void Delete()
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        //private void Print()
        //{
        //    var objArrList = new ArrayList();
        //    objArrList.Add("'" + Request.Params["ID"] + "'");
        //    Session["s_arrNo"] = objArrList;
        //    var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=332" + "&BuCode=" +
        //                      Request.Params["BuCode"];
        //    ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //        "<script>window.open('" + reportLink1 + "','_blank')</script>");
        //}

        private void Copy()
        {
            Response.Redirect("StdReqEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&MODE=Copy&ID=" +
                              Request.Params["ID"] +
                              "&VID=" + Request.Params["VID"]);
        }

        private void Back()
        {
            Response.Redirect("StdReqLst.aspx");
        }

        protected void btn_Delete_Success_Click(object sender, EventArgs e)
        {
            pop_Delete.ShowOnPageLoad = false;
            Response.Redirect("StdReqLst.aspx?ID=" + Request.Params["ID"]);
        }
    }
}