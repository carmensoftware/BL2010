using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Text;

namespace BlueLedger.PL.IN.REC
{
    public partial class RecLst : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accountMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config conf = new Blue.BL.APP.Config();

        private readonly DataSet dsPo = new DataSet();
        private readonly DataSet dsPoListCancel = new DataSet();
        private readonly DataSet dsPoListHQCancel = new DataSet();

        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.ADMIN.VendorMapp vendorMapp = new Blue.BL.ADMIN.VendorMapp();
        private Blue.BL.AP.Vendor _vendor = new Blue.BL.AP.Vendor();

        private string MsError = string.Empty;
        private decimal calAmount = Convert.ToDecimal("0.00");
        private decimal discountAmt = Convert.ToDecimal("0.00");
        private decimal discountPercentage = Convert.ToDecimal("0.00");
        private DataSet dsPoList = new DataSet();
        private DataSet dsPoListHQ = new DataSet();
        private DataTable dtLocation = new DataTable();
        private Blue.BL.GnxLib gnxlib = new Blue.BL.GnxLib();
        private decimal netAmt = Convert.ToDecimal("0.00");
        private decimal price = Convert.ToDecimal("0.00");
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private decimal qtycancel = Convert.ToDecimal("0.00");
        private decimal qtyrec = Convert.ToDecimal("0.00");
        private string statusPartial = "Partial";
        private string statusPrint = "Printed";
        private decimal tax = Convert.ToDecimal("0.00");
        private decimal taxAmt = Convert.ToDecimal("0.00");
        private decimal total = Convert.ToDecimal("0.00");
        private decimal totalAmount = Convert.ToDecimal("0.00");

        // Added on: 15/08/2017, By: Fon
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.3";

        //private decimal currNetAmt = Convert.ToDecimal("0.00");
        //private decimal currDiscAmt = Convert.ToDecimal("0.00");
        //private decimal currTaxAmt = Convert.ToDecimal("0.00");
        //private decimal currTotalAmt = Convert.ToDecimal("0.00");
        #endregion


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Session.Remove("vendor");

                var btn_PrintByDate = new DevExpress.Web.ASPxMenu.MenuItem("Print by Batch...", "PrintByBatch");

                btn_PrintByDate.ItemStyle.ForeColor = System.Drawing.Color.White;
                btn_PrintByDate.ItemStyle.Font.Size = FontUnit.Smaller;
                btn_PrintByDate.NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=PrintByBatch";
                ListPage2.menuItems.Insert(ListPage2.menuItems.Count - 2, btn_PrintByDate);


                Page_Retrieve();
            }
            else
            {
                dsPoList = (DataSet)Session["dsPoList"];
                dsPoListHQ = (DataSet)Session["dsPoListHQ"];

                if (Session["vendor"] != null)
                {

                    if (pop_SelectVendor.ShowOnPageLoad)
                    {
                        ddl_Vendor.DataSource = (DataTable)Session["vendor"];
                        ddl_Vendor.DataBind();
                    }

                    if (pop_Cancel.ShowOnPageLoad)
                    {
                        ddl_VendorCancel.DataSource = (DataTable)Session["vendor"];
                        ddl_VendorCancel.DataBind();
                    }

                    if (pop_HQ.ShowOnPageLoad)
                    {
                        ddl_VendorHQ.DataSource = (DataTable)Session["vendor"];
                        ddl_VendorHQ.DataBind();
                    }

                    if (pop_CancelHQ.ShowOnPageLoad)
                    {
                        ddl_VendorHQCancel.DataSource = (DataTable)Session["vendor"];
                        ddl_VendorHQCancel.DataBind();
                    }


                }

            }

            if (Request.Params["MENU"] != null)
            {
                Session["MENU"] = Request.Params["MENU"].ToString();
                Response.Redirect("RecLst.aspx");
            }

            // Action menu by Session
            if (Session["MENU"] != null)
            {
                string mode = Session["MENU"].ToString();
                Session.Remove("MENU");
                switch (mode.ToUpper())
                {
                    case "ME":
                        Response.Redirect("RecEdit.aspx?MODE=new" + "&VID=" + ListPage2.VID);
                        break;
                    //ต้องแก้เพื่อ Impact เพิ่ม Create Manual
                    case "POMANUAL":
                        //Response.Redirect("RecCreateManual.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new" + "&VID=" + ListPage2.VID);
                        //var id = Guid.NewGuid().ToString();
                        //Response.Redirect("RecEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new"); //
                        Response.Redirect(string.Format("RecEdit.aspx?BuCode={0}&VID={1}&MODE=new&ID={2}", LoginInfo.BuInfo.BuCode, ListPage2.VID, Guid.NewGuid().ToString())); //
                        break;

                    case "PO":
                        DisplayPOList();
                        break;

                    case "CANCEL":
                        DisplayCancelItemList();
                        break;

                    case "HQ":
                        DisplayHQItemList();
                        break;

                    case "CANCELHQ":
                        DisplayCancelHQ();
                        break;

                    case "COMMIT":
                        Response.Redirect("RecCommitByBatch.aspx?BuCode=" + LoginInfo.BuInfo.BuCode);
                        break;

                    case "PRINTBYBATCH":
                        DisplayPrintByBatch();
                        break;
                }
            }

        }

        private void Page_Retrieve()
        {
            dsPoList.Clear();
            dsPoListHQ.Clear();

            Session["dsPoList"] = dsPoList;
            Session["dsPoListHQ"] = dsPoListHQ;
            //dsPoList = (DataSet)Session["dsPoList"];
            //dsPoListHQ = (DataSet)Session["dsPoListHQ"];

            ListPage2.DataBind();


            Page_Setting();
        }

        /// <summary>
        ///     Display Unit Data.
        /// </summary>
        private void Page_Setting()
        {

            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From Purchase Order", "PO"));
            ListPage2.CreateItems.Items.FindByName("PO").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=PO";

            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Manually", "POManual"));
            ListPage2.CreateItems.Items.FindByName("POManual").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=POManual";

            //ต้องแก้เพื่อ Impact เพิ่ม Create Manual
            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Cancel By Item", "Cancel"));
            ListPage2.CreateItems.Items.FindByName("Cancel").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=Cancel";


            // Modified on: 30/03/2018, By: Fon, From: Leader approve.
            //if (!LoginInfo.BuInfo.IsHQ)

            bool isHQBUactive = false;
            DataSet dsBU = new DataSet();
            string hqBUcode = bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode);
            if (hqBUcode != string.Empty) bu.GetList(dsBU, hqBUcode, System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString());
            if (dsBU.Tables.Contains(bu.TableName))
                if (dsBU.Tables[bu.TableName].Rows.Count > 0)
                    isHQBUactive = (bool)dsBU.Tables[bu.TableName].Rows[0]["IsActived"];

            if (isHQBUactive && !LoginInfo.BuInfo.IsHQ)
            // End Modified.
            {
                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Receiving from HQ PO", "HQ"));
                ListPage2.CreateItems.Items.FindByName("HQ").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=HQ";
                ListPage2.CreateItems.Items.FindByName("HQ").BeginGroup = true;

                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Cancel from HQ PO", "CancelHQ"));
                ListPage2.CreateItems.Items.FindByName("CancelHQ").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=CancelHQ";
            }

            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Commit by Batch", "Commit"));
            ListPage2.CreateItems.Items.FindByName("Commit").NavigateUrl = "~/PC/REC/RecLst.aspx?MENU=Commit";
            ListPage2.CreateItems.Items.FindByName("Commit").BeginGroup = true;

            //else
            //{
            //    ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From Purchase Order", "PO"));
            //    ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Manually", "POManual"));
            //    //ต้องแก้เพื่อ Impact เพิ่ม Create Manual
            //    ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Cancel By Item", "Cancel"));
            //    ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Commit by Batch", "Commit"));
            //}

            //ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Receiving", "Print"));
            //ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
            Control_HeaderMenuBar();
        }

        // Added on: 06/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage2.CreateItems.Visible = (pagePermiss >= 3) ? ListPage2.CreateItems.Visible : false;
        }
        // End Added.

        /// <summary>
        ///     Display PO List
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        //private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        //{
        //    switch (e.Item.Name.ToUpper())
        //    {
        //        case "ME":
        //            Response.Redirect("RecEdit.aspx?MODE=new" + "&VID=" + ListPage2.VID);
        //            break;
        //        //ต้องแก้เพื่อ Ipact เพิ่ม Create Manual
        //        case "POMANUAL":
        //            Response.Redirect("RecCreateManual.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new" + "&VID=" +
        //                              ListPage2.VID);
        //            //Response.Redirect("RecEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new"); //
        //            break;

        //        case "PO":

        //            DisplayPOList();
        //            break;

        //        case "CANCEL":
        //            DisplayCancelItemList();
        //            break;

        //        case "HQ":

        //            DisplayHQItemList();
        //            break;

        //        case "CANCELHQ":
        //            DisplayCancelHQ();
        //            break;

        //        case "COMMIT":
        //            Response.Redirect("RecCommitByBatch.aspx?BuCode=" + LoginInfo.BuInfo.BuCode);
        //            break;

        //        case "PRINT":
        //            Session["dtBuKeys"] = ListPage2.dtBuKeys;

        //            var paramField = new string[3, 2];
        //            paramField[0, 0] = "rpmfromdate";
        //            paramField[1, 0] = "rpmtodate";
        //            paramField[2, 0] = "rpminvoice";
        //            paramField[0, 1] = "2011-01-01";
        //            paramField[1, 1] = "2011-01-01";
        //            paramField[2, 1] = "True";
        //            Session["paramField"] = paramField;

        //            var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=115";
        //            ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //                "<script>window.open('" + reportLink + "','_blank')</script>");
        //            break;
        //    }
        //}

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        protected void ddl_Vendor_Load(object sender, EventArgs e)
        {
            //ddl_Vendor.DataSource = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);

            //if (!IsPostBack)
            //{
            //    ddl_Vendor.SelectedIndex = 0;
            //}

            //ddl_Vendor.ValueField = "VendorCode";
            //ddl_Vendor.DataBind();
        }

        protected void ddl_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (hf_Vendor.Value != ddl_Vendor.Value.ToString())
            //{
            //    dsPoList.Clear();

            //    poDt.GetListByStatusAndVendor(dsPoList, statusPrint, statusPartial, ddl_Vendor.Value.ToString(),
            //        LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);

            //    grd_PoList.DataSource = dsPoList.Tables[poDt.TableName];
            //    grd_PoList.DataBind();

            //    Session["dsPoList"] = dsPoList;
            //    hf_Vendor.Value = ddl_Vendor.Value.ToString();
            //}
            if (hf_Vendor.Value != ddl_Vendor.Value.ToString())
            {
                Set_grd_PoList();
            }
        }

        protected void ddl_VendorCancel_Load(object sender, EventArgs e)
        {
            //ddl_VendorCancel.DataSource = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode,
            //    LoginInfo.ConnStr);

            //if (!IsPostBack)
            //{
            //    ddl_VendorCancel.SelectedIndex = 0;
            //}

            //ddl_VendorCancel.ValueField = "VendorCode";
            //ddl_VendorCancel.DataBind();
        }

        protected void ddl_VendorCancel_ValueChanged(object sender, EventArgs e)
        {
            if (hf_VendorCancel.Value != ddl_VendorCancel.Value.ToString())
            {
                dsPoListCancel.Clear();

                poDt.GetListByStatusAndVendor(dsPoListCancel, statusPrint, statusPartial,
                    ddl_VendorCancel.Value.ToString(), LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);
                grd_CancelItemList.DataSource = dsPoListCancel.Tables[poDt.TableName];
                grd_CancelItemList.DataBind();

                Session["dsPoList"] = dsPoListCancel;
                hf_VendorCancel.Value = ddl_VendorCancel.Value.ToString();
            }
        }

        protected void ddl_VendorHQ_Load(object sender, EventArgs e)
        {
            //ddl_VendorHQ.DataSource = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode,
            //    LoginInfo.HQConnStr);

            //if (!IsPostBack)
            //{
            //    ddl_VendorHQ.SelectedIndex = 0;
            //}

            //ddl_VendorHQ.ValueField = "VendorCode";
            //ddl_VendorHQ.DataBind();
        }

        protected void ddl_VendorHQ_ValueChanged(object sender, EventArgs e)
        {
            //if (hf_VendorHQ.Value != ddl_VendorHQ.Value.ToString())
            //{
            //    dsPoListHQ.Clear();

            //    poDt.GetListByStatusAndVendor(dsPoListHQ, statusPrint, statusPartial, ddl_VendorHQ.Value.ToString(),
            //        LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);
            //    grd_HQItemList.DataSource = dsPoListHQ.Tables[poDt.TableName];
            //    grd_HQItemList.DataBind();

            //    Session["dsPoListHQ"] = dsPoListHQ;
            //    hf_VendorHQ.Value = ddl_VendorHQ.Value.ToString();
            //}
            if (hf_VendorHQ.Value != ddl_VendorHQ.Value.ToString())
            {
                Set_grd_HQItemList();
            }
        }

        protected void ddl_VendorHQCancel_Load(object sender, EventArgs e)
        {
            //ddl_VendorHQCancel.DataSource = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode,
            //    LoginInfo.HQConnStr);

            //if (!IsPostBack)
            //{
            //    ddl_VendorHQCancel.SelectedIndex = 0;
            //}

            //ddl_VendorHQCancel.ValueField = "VendorCode";
            //ddl_VendorHQCancel.DataBind();
        }

        protected void ddl_VendorHQCancel_ValueChanged(object sender, EventArgs e)
        {
            if (hf_VendorHQCancel.Value != ddl_VendorHQCancel.Value.ToString())
            {
                dsPoListHQCancel.Clear();

                poDt.GetListByStatusAndVendor(dsPoListHQCancel, statusPrint, statusPartial,
                    ddl_VendorHQCancel.Value.ToString(), LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);
                grd_CancelHQ.DataSource = dsPoListHQCancel.Tables[poDt.TableName];
                grd_CancelHQ.DataBind();

                Session["dsPoListHQ"] = dsPoListHQCancel;
                hf_VendorHQCancel.Value = ddl_VendorHQCancel.Value.ToString();
            }
        }

        protected void ddl_VendorLocal_Load(object sender, EventArgs e)
        {
            //ddl_VendorLocal.DataSource = _vendor.GetList(LoginInfo.ConnStr);
            ////if (!IsPostBack)
            ////{
            ////    ddl_VendorLocal.SelectedIndex = 0;
            ////}
            //ddl_VendorLocal.ValueField = "VendorCode";
            //ddl_VendorLocal.DataBind();
        }

        protected void ddl_VendorLocal_Init(object sender, EventArgs e)
        {
            DataTable dt = _vendor.GetList(LoginInfo.ConnStr);

            ddl_VendorLocal.DataSource = dt;
            //Session.Remove("vendor");
            //Session["vendor"] = dt;
            ddl_VendorLocal.ValueField = "VendorCode";
            ddl_VendorLocal.DataBind();
        }

        protected void btn_ConfirmLocation_Click(object sender, EventArgs e)
        {
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_PoList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;
                    values.Add(lbl_PoNo.Text);
                }
            }

            CreateRec(values, LoginInfo.BuInfo.BuCode, ddl_Location.Value.ToString().Split(':')[0]);
        }

        protected void btn_ConfirmLocationHQ_Click(object sender, EventArgs e)
        {
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_HQItemList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;
                    values.Add(lbl_PoNo.Text);
                }
            }

            CreateRec(values, bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode), ddl_LocationHQ.Value.ToString().Split(':')[0]);
        }

        protected void grd_PoList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl_Vendor = e.Row.FindControl("lbl_Vendor") as Label;
                    lbl_Vendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode")
                    + " : " + DataBinder.Eval(e.Row.DataItem, "VendorName");
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text = DataBinder.Eval(e.Row.DataItem, "DeliveryDate") == DBNull.Value
                        ? string.Empty
                        : DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var lbl_PoNo = e.Row.FindControl("lbl_PoNo") as Label;
                    lbl_PoNo.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                }
            }
        }

        protected void grd_CancelItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl_Vendor = e.Row.FindControl("lbl_Vendor") as Label;
                    lbl_Vendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode")
                    + " : " + DataBinder.Eval(e.Row.DataItem, "VendorName");
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text = DataBinder.Eval(e.Row.DataItem, "DeliveryDate") == DBNull.Value
                        ? string.Empty
                        : DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var lbl_PoNo = e.Row.FindControl("lbl_PoNo") as Label;
                    lbl_PoNo.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                }

                if (e.Row.FindControl("lbl_QtyOrd") != null)
                {
                    var lbl_QtyOrd = e.Row.FindControl("lbl_QtyOrd") as Label;
                    lbl_QtyOrd.Text = DataBinder.Eval(e.Row.DataItem, "OrdQty").ToString();
                }

                if (e.Row.FindControl("lbl_QtyRec") != null)
                {
                    var lbl_QtyRec = e.Row.FindControl("lbl_QtyRec") as Label;
                    lbl_QtyRec.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                }
            }
        }

        protected void grd_HQItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl_Vendor = e.Row.FindControl("lbl_Vendor") as Label;
                    lbl_Vendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                    //+ " : " + DataBinder.Eval(e.Row.DataItem, "VendorName");
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text = DataBinder.Eval(e.Row.DataItem, "DeliveryDate") == DBNull.Value
                        ? string.Empty
                        : DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var lbl_PoNo = e.Row.FindControl("lbl_PoNo") as Label;
                    lbl_PoNo.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                }
            }
        }

        protected void grd_CancelHQ_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl_Vendor = e.Row.FindControl("lbl_Vendor") as Label;
                    lbl_Vendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                    //+ " : " + DataBinder.Eval(e.Row.DataItem, "VendorName");
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text = DataBinder.Eval(e.Row.DataItem, "DeliveryDate") == DBNull.Value
                        ? string.Empty
                        : DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_PoNo") != null)
                {
                    var lbl_PoNo = e.Row.FindControl("lbl_PoNo") as Label;
                    lbl_PoNo.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                }

                if (e.Row.FindControl("lbl_QtyOrd") != null)
                {
                    var lbl_QtyOrd = e.Row.FindControl("lbl_QtyOrd") as Label;
                    lbl_QtyOrd.Text = DataBinder.Eval(e.Row.DataItem, "OrdQty").ToString();
                }

                if (e.Row.FindControl("lbl_QtyRec") != null)
                {
                    var lbl_QtyRec = e.Row.FindControl("lbl_QtyRec") as Label;
                    lbl_QtyRec.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                }
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            pop_HQ.ShowOnPageLoad = false;

            //Response.Redirect("~/Option/Admin/Interface/Sun/VendorMapp.aspx");
        }

        /// <summary>
        /// </summary>
        private void DisplayPOList()
        {
            DataTable dt = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
            {

                ddl_Vendor.DataSource = dt;
                Session.Remove("vendor");
                Session["vendor"] = dt;

                ddl_Vendor.ValueField = "VendorCode";
                ddl_Vendor.DataBind();

                // Display maket list data
                pop_SelectVendor.HeaderText = "Purchase Order List";
                pop_SelectVendor.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningPo.Text = "PO is empty.";
                pop_WarningPo.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        private void DisplayCancelItemList()
        {
            DataTable dt = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);

            if (dt.Rows.Count > 0)
            {

                ddl_VendorCancel.DataSource = dt; // po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);
                Session.Remove("vendor");
                Session["vendor"] = dt;

                //if (!IsPostBack)
                //{
                //    ddl_VendorCancel.SelectedIndex = 0;
                //}
                ddl_VendorCancel.ValueField = "VendorCode";
                ddl_VendorCancel.DataBind();


                //Display maket list data
                //pop_Cancel.HeaderText = "Purchase Order List";
                pop_Cancel.HeaderText = "Purchase Order List (Cancel)";
                pop_Cancel.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningPo.Text = "PO is empty.";
                pop_WarningPo.ShowOnPageLoad = true;
            }
        }

        private void DisplayHQItemList()
        {
            DataTable dt = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);
            if (dt.Rows.Count > 0)
            {
                ddl_VendorHQ.DataSource = dt;
                Session.Remove("vendor");
                Session["vendor"] = dt;
                ddl_VendorHQ.DataBind();

                // Display maket list data
                pop_HQ.HeaderText = "Purchase Order List From HQ";
                pop_HQ.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningPo.Text = "PO from HQ is empty.";
                pop_WarningPo.ShowOnPageLoad = true;
            }
        }

        private void DisplayCancelHQ()
        {
            DataTable dt = po.GetVendorByPOStatus(statusPrint, statusPartial, LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);

            if (dt.Rows.Count > 0)
            {
                ddl_VendorHQCancel.DataSource = dt;
                Session.Remove("vendor");
                Session["vendor"] = dt;
                ddl_VendorHQCancel.DataBind();

                //Display Cancel From HQ List
                pop_CancelHQ.HeaderText = "Perchase Order List From HQ (Cancel)";
                pop_CancelHQ.ShowOnPageLoad = true;
            }
            else
            {
                lbl_WarningPo.Text = "PO from HQ is empty.";
                pop_WarningPo.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     Receiving from selected PO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ok_SelectVendor_Click(object sender, EventArgs e)
        {
            var listPO = new List<string>();

            //lbl_WarningPo.Text = grd_PoList.Rows.Count.ToString();
            //pop_WarningPo.ShowOnPageLoad = true;
            //return;

            foreach (GridViewRow grd_Row in grd_PoList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;
                    listPO.Add(lbl_PoNo.Text);
                }
            }

            if (listPO.Count > 0)
            {
                if (Convert.ToBoolean(conf.GetConfigValue("PC", "REC", "IsLocation", LoginInfo.ConnStr)))
                {
                    for (var i = 0; i < listPO.Count; i++)
                    {
                        // Get PO Data                
                        po.GetListByPoNo2(dsPo, ref MsError, listPO[0], LoginInfo.ConnStr);

                        if (i != 0)
                        {
                            //Table1 is name of table that added in dsPO and merged others.
                            dsPo.Tables["Table1"].Merge(poDt.GetLocationByPoNo(listPO[i], LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr));
                        }
                        else
                        {
                            dsPo.Tables.Add(poDt.GetLocationByPoNo(listPO[i], LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr));
                        }
                    }

                    //Get dupplicate location out.
                    //Table1 is name of table that added in dsPO and merged others.
                    var location = (from drLocation in dsPo.Tables["Table1"].AsEnumerable()
                                    select new
                                    {
                                        LocationCode = drLocation.Field<string>("LocationCode"),
                                        LocationName = drLocation.Field<string>("LocationName")
                                    }).Distinct();

                    ddl_Location.DataSource = location;
                    ddl_Location.ValueField = "LocationCode";
                    ddl_Location.TextField = "LocationName";
                    ddl_Location.SelectedIndex = 0;
                    ddl_Location.DataBind();

                    // Fixed on 2017/01/18
                    //pop_SelectVendor.ShowOnPageLoad = false;
                    pop_SelectVendor.Style["display"] = "none";
                    pop_SelectLocation.ShowOnPageLoad = true;
                }
                else
                {
                    //if (listPO.Count > 0)
                    //{
                    //    CreateRec(listPO, LoginInfo.BuInfo.BuCode, string.Empty);
                    //}
                    //else
                    //{
                    //    lbl_WarningPo.Text = "Please select PO.";
                    //    pop_WarningPo.ShowOnPageLoad = true;
                    //}
                }
            }
            else
            {
                // Display Message: PO is no selected.
                // Fixed on 2017/01/18
                lbl_WarningPo.Text = "Please select PO.";
                pop_WarningPo.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     For cancelbyItem process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_CancelItemList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;
                    values.Add(lbl_PoNo.Text);
                }
            }

            for (var i = 0; i < values.Count; i++)
            {
                // Get PO Data                
                po.GetListByPoNo2(dsPo, ref MsError, values[i], LoginInfo.ConnStr);

                // Get PO Detail Data
                poDt.GetPoDtCancelByPoNo(dsPo, ref MsError, values[i], LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);
            }

            //Session["dsPo"] = dsRec;
            Session["dsPo"] = dsPo;

            // Redirec to PrEdit.aspx page
            Response.Redirect("RecCancelItem.aspx?MODE=CancelItem&BuCode=" + LoginInfo.BuInfo.BuCode);
        }

        #region "From HQ"

        protected void btn_HQ_Click(object sender, EventArgs e)
        {
            var vendor = string.Empty;
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_HQItemList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;


                if (chk_Item != null)
                {
                    //test = lbl_PoNo.Text;

                    //chk_Item.Checked = true; // Just Try
                    if (chk_Item.Checked)
                    {
                        //if (lbl_PoNo.Text == "PO17040004")
                        values.Add(lbl_PoNo.Text);
                    }
                }
                else
                {
                    lbl_WarningPo.Text = "Checkbox is null.";
                    pop_WarningPo.ShowOnPageLoad = true;
                    return;
                }
            }

            // Added on: 2017/08/18 By:Fon, For: Un-check
            if (values.Count <= 0)
            {
                lbl_WarningPo.Text = "PO is empty.";
                //lbl_WarningPo.Text = values.Count.ToString();
                //lbl_WarningPo.Text = ((Label)grd_HQItemList.Rows[0].FindControl("lbl_PoNo")).Text;
                pop_WarningPo.ShowOnPageLoad = true;
                return;
            }
            // End Added.

            //Check Vendor Mapping
            for (var i = 0; i < values.Count; i++)
            {
                // Get PO Data                  
                po.GetListByPoNo2(dsPo, ref MsError, values[i], LoginInfo.HQConnStr);

                if (
                    vendorMapp.GetLocalVendorCode(dsPo.Tables[po.TableName].Rows[0]["Vendor"].ToString(),
                        LoginInfo.ConnStr) == string.Empty)
                {
                    if (vendor == string.Empty)
                    {
                        vendor = dsPo.Tables[po.TableName].Rows[0]["Vendor"].ToString();
                    }
                    else
                    {
                        vendor = vendor + ", " + dsPo.Tables[po.TableName].Rows[0]["Vendor"];
                    }
                }
            }

            if (vendor != string.Empty)
            {
                //lbl_Warning.Text = "Receiving cannot be process. Please map vendor number " + vendor;
                //pop_Warning.ShowOnPageLoad = true;

                DataTable dt = _vendor.GetList(LoginInfo.ConnStr);
                ddl_VendorLocal.DataSource = dt;

                // Just Comment on: 21/08/2017
                //Session.Remove("vendor");
                //Session["vendor"] = dt;
                // End comment.

                ddl_VendorLocal.ValueField = "VendorCode";
                ddl_VendorLocal.DataBind();


                lbl_VendorMapping_Text.Text = vendor;
                pop_VendorMapping.ShowOnPageLoad = true;
                return;
            }

            if (Convert.ToBoolean(conf.GetConfigValue("PC", "REC", "IsLocation", LoginInfo.ConnStr)))
            {
                for (var i = 0; i < values.Count; i++)
                {
                    if (i != 0)
                    {
                        //Table1 is name of table that added in dsPO and merged others.
                        dsPo.Tables["Table1"].Merge(poDt.GetLocationByPoNo(values[i], LoginInfo.BuInfo.BuCode,
                            LoginInfo.HQConnStr));
                    }
                    else
                    {
                        //Get Location Name because this PO created in HQ.
                        dtLocation = poDt.GetLocationByPoNo(values[i], LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);

                        foreach (DataRow drLocation in dtLocation.Rows)
                        {
                            drLocation["LocationName"] = storeLct.GetName(drLocation["LocationCode"].ToString(),
                                LoginInfo.ConnStr);
                        }

                        dsPo.Tables.Add(dtLocation);
                    }
                }

                //Get dupplicate location out.
                //Table1 is name of table that added in dsPO and merged others.
                var location = (from drLocation in dsPo.Tables["Table1"].AsEnumerable()
                                select new
                                {
                                    LocationCode = drLocation.Field<string>("LocationCode"),
                                    LocationName = drLocation.Field<string>("LocationName")
                                }).Distinct();

                ddl_LocationHQ.DataSource = location;
                ddl_LocationHQ.ValueField = "LocationCode";
                ddl_LocationHQ.TextField = "LocationName";
                ddl_LocationHQ.SelectedIndex = 0;
                ddl_LocationHQ.DataBind();

                pop_HQ.ShowOnPageLoad = false;
                pop_LocationHQList.ShowOnPageLoad = true;
            }
            else
            {
                if (values.Count > 0)
                {
                    CreateRec(values, bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode), string.Empty);
                }
            }

            // close pop-up never change result.
            //pop_HQ.ShowOnPageLoad = false;
        }

        protected void btn_CancelHQ_Click(object sender, EventArgs e)
        {
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_CancelHQ.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_PoNo = grd_Row.FindControl("lbl_PoNo") as Label;
                    values.Add(lbl_PoNo.Text);
                }
            }

            for (var i = 0; i < values.Count; i++)
            {
                // Get PO Data                
                po.GetListByPoNo2(dsPo, ref MsError, values[i], LoginInfo.HQConnStr);

                // Get PO Detail Data
                poDt.GetPoDtCancelByPoNo(dsPo, ref MsError, values[i], LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);
            }

            Session["dsPo"] = dsPo;

            // Redirec to PrEdit.aspx page
            Response.Redirect("RecCancelItem.aspx?MODE=CancelItem&BuCode=" + LoginInfo.BuInfo.BuCode + "&Type=HQ");
        }

        protected void BtnOkVendorMapping_Click(object sender, EventArgs e)
        {
            string vendorLocal = ddl_VendorLocal.Value.ToString();
            string vendorHQ = lbl_VendorMapping_Text.Text;

            string sql = "INSERT INTO [ADMIN].VendorMapp (ID, LocalCode, HQCode)";
            sql += " VALUES ((SELECT MAX(ID) + 1 FROM [ADMIN].VendorMapp), '" + vendorLocal + "', '" + vendorHQ + "')";
            Blue.DAL.SQL.SqlHelper cmd = new Blue.DAL.SQL.SqlHelper();
            cmd.SqlExecuteQuery(sql, null, LoginInfo.ConnStr);

            btn_HQ_Click(sender, e);


            //cmd.DbExecuteNonQuery
            pop_VendorMapping.ShowOnPageLoad = false;
        }

        protected void BtnCancelVendorMapping_Click(object sender, EventArgs e)
        {
            pop_VendorMapping.ShowOnPageLoad = false;
        }


        protected void btn_WarningPo_Click(object sender, EventArgs e)
        {
            pop_WarningPo.ShowOnPageLoad = false;
        }



        /// <summary>
        /// </summary>
        /// <param name="values"></param>
        protected void CreateRec(List<string> values, string buCode, string location)
        {
            // Get Rec, RecDt Table Schema
            var dsRec = new DataSet();
            rec.GetStructure(dsRec, LoginInfo.ConnStr);
            recDt.GetStructure(dsRec, LoginInfo.ConnStr);

            dsPo.Clear();

            for (var i = 0; i < values.Count; i++)
            {
                // Get PO Data             
                if (bu.IsHQ(buCode))
                {
                    po.GetListByPoNo2(dsPo, ref MsError, values[0], LoginInfo.HQConnStr);
                }
                else
                {
                    po.GetListByPoNo2(dsPo, ref MsError, values[0], LoginInfo.ConnStr);
                }

                var drPo = dsPo.Tables[po.TableName].Rows[0];

                // Insert to rec
                #region
                var drRec = dsRec.Tables[rec.TableName].NewRow();

                drRec["RecNo"] = rec.GetNewID(ServerDateTime, LoginInfo.ConnStr);
                drRec["RecDate"] = ServerDateTime;
                drRec["DeliPoint"] =
                    Convert.ToInt32(drPo["DeliPoint"] == DBNull.Value
                        ? 1
                        : Convert.ToInt32(drPo["DeliPoint"].ToString()));
                drRec["VendorCode"] = drPo["Vendor"].ToString();
                drRec["CreatedDate"] = ServerDateTime;
                drRec["CreatedBy"] = LoginInfo.LoginName;
                drRec["UpdatedDate"] = ServerDateTime;
                drRec["UpdatedBy"] = LoginInfo.LoginName;
                drRec["ExRateAudit"] = drPo["ExchageRate"].ToString();

                // Modified: 15/05/2014: By:Fon, For: New Muti-currency.
                //drRec["CurrencyCode"] = drPo["Currency"].ToString();
                string currCode = drPo["CurrencyCode"].ToString();
                decimal currRate = 1;
                currRate = currency.GetLastCurrencyRate(currCode, DateTime.Today, LoginInfo.ConnStr);

                if (currRate == null || currRate == 0)
                    currRate = Convert.ToDecimal(drPo["CurrencyRate"]);

                drRec["CurrencyCode"] = drPo["CurrencyCode"].ToString();
                drRec["CurrencyRate"] = currRate;
                // End Modified.

                drRec["PoSource"] = buCode;

                dsRec.Tables[rec.TableName].Rows.Add(drRec);

                //Get PO Detail Data
                if (bu.IsHQ(buCode))
                {
                    poDt.GetPoDtByPoNoForReceiving(dsPo, ref MsError, values[i], LoginInfo.BuInfo.BuCode, location, LoginInfo.HQConnStr);
                    poDt.GetListByPoNo(dsRec, ref MsError, values[i], LoginInfo.HQConnStr);
                }
                else
                {
                    poDt.GetPoDtByPoNoForReceiving(dsPo, ref MsError, values[i], LoginInfo.BuInfo.BuCode, location, LoginInfo.ConnStr);
                    poDt.GetListByPoNo(dsRec, ref MsError, values[i], LoginInfo.ConnStr);
                }
                #endregion

                // Insert to RecDt (For HQ multiple PO(s)
                #region
                foreach (DataRow drSelected in dsPo.Tables[poDt.TableName].Rows)
                {
                    if (drSelected["PoNo"].ToString() == values[i])
                    {
                        var drRecDt = dsRec.Tables[recDt.TableName].NewRow();

                        drRecDt["RecNo"] = rec.GetNewID(ServerDateTime, LoginInfo.ConnStr);
                        drRecDt["RecDtNo"] = dsRec.Tables[recDt.TableName].Rows.Count + 1;
                        drRecDt["LocationCode"] = drSelected["Location"];
                        drRecDt["ProductCode"] = drSelected["Product"];
                        drRecDt["Descen"] = drSelected["Descen"];
                        drRecDt["Descll"] = drSelected["Descll"];
                        drRecDt["UnitCode"] = drSelected["Unit"];
                        drRecDt["RcvUnit"] = drSelected["Unit"];
                        drRecDt["DiscAdj"] = false;
                        drRecDt["TaxAdj"] = false;
                        drRecDt["Rate"] = prodUnit.GetConvRate(drSelected["Product"].ToString(), drSelected["Unit"].ToString(), LoginInfo.ConnStr);


                        string taxType = drSelected["TaxType"].ToString();
                        decimal taxRate = Convert.ToDecimal(drSelected["TaxRate"] == DBNull.Value ? 0 : drSelected["TaxRate"]);
                        decimal discRate = Convert.ToDecimal(drSelected["Discount"] == DBNull.Value ? 0 : drSelected["Discount"]);
                        decimal price = Convert.ToDecimal(drSelected["Price"] == DBNull.Value ? 0 : drSelected["Price"]);

                        decimal orderQty = Convert.ToDecimal(drSelected["OrderQty"] == DBNull.Value ? 0 : drSelected["OrderQty"]);
                        decimal focQty = Convert.ToDecimal(drSelected["FOCQty"] == DBNull.Value ? 0 : drSelected["FOCQty"]);
                        decimal recQty = Convert.ToDecimal(drSelected["OrderQty"] == DBNull.Value ? 0 : drSelected["OrderQty"]);


                        decimal currNetAmt = Convert.ToDecimal(drSelected["CurrNetAmt"] == DBNull.Value ? 0 : drSelected["CurrNetAmt"]);
                        decimal currTaxAmt = Convert.ToDecimal(drSelected["CurrTaxAmt"] == DBNull.Value ? 0 : drSelected["CurrTaxAmt"]);
                        decimal currDiscAmt = Convert.ToDecimal(drSelected["CurrDiscAmt"] == DBNull.Value ? 0 : drSelected["CurrDiscAmt"]);
                        decimal currTotalAmt = Convert.ToDecimal(drSelected["CurrTotalAmt"] == DBNull.Value ? 0 : drSelected["CurrTotalAmt"]);

                        drRecDt["Price"] = price;
                        drRecDt["Discount"] = discRate;
                        drRecDt["TaxType"] = taxType;
                        drRecDt["TaxRate"] = taxRate;

                        if (drSelected["OrdQty"].ToString() == drSelected["OrderQty"].ToString()) // if full received, copy all
                        {
                            drRecDt["OrderQty"] = orderQty;
                            drRecDt["FOCQty"] = focQty;
                            drRecDt["RecQty"] = recQty;


                            drRecDt["CurrNetAmt"] = currNetAmt;
                            drRecDt["CurrTaxAmt"] = currTaxAmt;
                            drRecDt["CurrDiscAmt"] = currDiscAmt;
                            drRecDt["CurrTotalAmt"] = currTotalAmt;

                            drRecDt["NetAmt"] = RoundAmt(currNetAmt * currRate);
                            drRecDt["TaxAmt"] = RoundAmt(currTaxAmt * currRate);
                            drRecDt["DiccountAmt"] = RoundAmt(currDiscAmt * currRate);
                            drRecDt["TotalAmt"] = RoundAmt(currTotalAmt * currRate);

                        }
                        else // if partial received, must be re-calculated 
                        {
                            drRecDt["OrderQty"] = orderQty;
                            drRecDt["FOCQty"] = focQty;
                            drRecDt["RecQty"] = recQty;


                            decimal amount = RoundAmt(recQty * price);
                            decimal discAmt = RoundAmt(discRate == 0 ? 0 : amount * discRate / 100);

                            currDiscAmt = discAmt;

                            // Calculation
                            if (taxType == "I")
                            {
                                currNetAmt = RoundAmt(taxRate == 0 ? 0 : ((amount - discAmt) * 100) / (100 + taxRate));
                                currTaxAmt = amount - currNetAmt;
                                currTotalAmt = currNetAmt + currTaxAmt;
                            }
                            else // "N", "A"
                            {
                                currNetAmt = amount - discAmt;
                                currTaxAmt = RoundAmt(taxRate == 0 ? 0 : currNetAmt * taxRate / 100);
                                currTotalAmt = currNetAmt + currTaxAmt;
                            }


                            drRecDt["CurrDiscAmt"] = currDiscAmt;
                            drRecDt["CurrNetAmt"] = currNetAmt;
                            drRecDt["CurrTaxAmt"] = currTaxAmt;
                            drRecDt["CurrTotalAmt"] = currTotalAmt;

                            drRecDt["DiccountAmt"] = RoundAmt(currDiscAmt * currRate);
                            drRecDt["NetAmt"] = RoundAmt(currNetAmt * currRate);
                            drRecDt["TaxAmt"] = RoundAmt(currTaxAmt * currRate);
                            drRecDt["TotalAmt"] = RoundAmt(currTotalAmt * currRate);
                        }

                        drRecDt["PoNo"] = drSelected["PoNo"];
                        drRecDt["PoDtNo"] = drSelected["PoDt"];
                        drRecDt["NetDrAcc"] = accountMapp.GetA3Code(LoginInfo.BuInfo.BuCode, drSelected["Location"].ToString(), drSelected["Product"].ToString().Substring(0, 4), LoginInfo.ConnStr);
                        drRecDt["TaxDrAcc"] = drSelected["TaxAccCode"];
                        drRecDt["Status"] = System.DBNull.Value;
                        drRecDt["ExportStatus"] = false;

                        // Add new record
                        dsRec.Tables[recDt.TableName].Rows.Add(drRecDt);
                    }
                }
                #endregion

            }

            dsRec.Tables[poDt.TableName].Clear();
            Session["dsPo"] = dsRec;

            // Redirec to RecEdit.aspx page
            Response.Redirect("RecEdit.aspx?MODE=FPO&BuCode=" + LoginInfo.BuInfo.BuCode + "&VID=" + ListPage2.VID);
        }



        protected void btn_CancelLocation_Click(object sender, EventArgs e)
        {
            pop_SelectVendor.Style["display"] = "run-in";
            pop_SelectVendor.ShowOnPageLoad = false;
            pop_SelectLocation.ShowOnPageLoad = false;
        }

        #endregion

        // Added on: 17/08/2017, By: Fon
        // Note: If U cannot find value of ddl_location, it's mean your PoNo didn't exist in [pc].[Prdt]

        #region About Currency
        protected void comb_CurrCode_Init(object sender, EventArgs e)
        {
            Bind_comb_Currency(sender);
        }

        protected void comb_CurrCodeHQ_Init(object sender, EventArgs e)
        {
            Bind_comb_Currency(sender);
        }

        protected void comb_CurrCodeHQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_VendorHQ.Text != string.Empty)
                Set_grd_HQItemList();
        }

        protected void Bind_comb_Currency(object sender)
        {
            string defaultCurrCode = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
            ASPxComboBox comb_CurrCode = (ASPxComboBox)sender;
            comb_CurrCode.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            comb_CurrCode.DataBind();

            if (defaultCurrCode != string.Empty)
                comb_CurrCode.Value = defaultCurrCode;
        }

        protected void comb_CurrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Vendor.Text != string.Empty)
                Set_grd_PoList();
        }

        protected void Set_grd_PoList()
        {
            // Note: ddl.Value will not error because I call function when it already have value.

            hf_Vendor.Value = ddl_Vendor.Value.ToString();
            string currencyCode = comb_CurrCode.Value.ToString();

            if (hf_Vendor.Value != string.Empty)
            {
                dsPoList.Clear();

                //poDt.GetListByStatusAndVendor(dsPoList, statusPrint, statusPartial, ddl_Vendor.Value.ToString(),
                //    LoginInfo.BuInfo.BuCode, LoginInfo.ConnStr);

                //string strFilter = string.Format("CurrencyCode = '{0}'", comb_CurrCode.Value.ToString().Trim());
                //dsPoList.Tables[poDt.TableName].DefaultView.RowFilter = strFilter;

                poDt.GetListByStatusAndVendor(dsPoList, statusPrint, statusPartial, ddl_Vendor.Value.ToString(), LoginInfo.BuInfo.BuCode, currencyCode, LoginInfo.ConnStr);
                grd_PoList.DataSource = dsPoList.Tables[poDt.TableName];
                grd_PoList.DataBind();
                Session["dsPoList"] = dsPoList;
            }
        }

        protected void Set_grd_HQItemList()
        {
            // Note: ddl.Value will not error because I call function when it already have value.
            hf_VendorHQ.Value = ddl_VendorHQ.Value.ToString();
            if (hf_VendorHQ.Value != string.Empty)
            {
                dsPoListHQ.Clear();
                poDt.GetListByStatusAndVendor(dsPoListHQ, statusPrint, statusPartial, ddl_VendorHQ.Value.ToString(),
                    LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);

                string strFilter = string.Format("CurrencyCode = '{0}'", comb_CurrCodeHQ.Value.ToString().Trim());
                dsPoListHQ.Tables[poDt.TableName].DefaultView.RowFilter = strFilter;
                grd_HQItemList.DataSource = dsPoListHQ.Tables[poDt.TableName];
                grd_HQItemList.DataBind();
                Session["dsPoListHQ"] = dsPoListHQ;
            }
        }
        #endregion

        #region --Print by batch--
        private void DisplayPrintByBatch()
        {
            date_Print_From.Date = DateTime.Today.Date;
            date_Print_To.Date = DateTime.Today.Date;

            var dtVendor = bu.DbExecuteQuery("SELECT VendorCode as [Code], [Name] FROM AP.Vendor ORDER BY VendorCode", null, LoginInfo.ConnStr);

            var ddl1 = ddl_Print_VendorFrom;
            var ddl2 = ddl_Print_VendorTo;

            ddl1.Items.Clear();
            ddl2.Items.Clear();

            if (dtVendor != null && dtVendor.Rows.Count > 0)
            {
                var items = dtVendor.AsEnumerable()
                    .Select(x => new ListEditItem
                    {
                        Value = x.Field<string>("Code"),
                        Text = x.Field<string>("Code") + " : " + x.Field<string>("Name")
                    })
                    .ToArray();

                ddl1.Items.AddRange(items);
                ddl2.Items.AddRange(items);

                ddl1.SelectedIndex = 0;
                ddl2.SelectedIndex = items.Length - 1;
            }


            pop_PrintByBatch.ShowOnPageLoad = true;
        }

        protected void date_Print_From_DateChanged(object sender, EventArgs e)
        {
            var dateFrom = date_Print_From.Date;
            var dateTo = date_Print_To.Date;

            if (dateTo < dateFrom)
            {
                date_Print_To.Date = dateFrom;
            }
        }

        protected void date_Print_To_DateChanged(object sender, EventArgs e)
        {
            var dateFrom = date_Print_From.Date;
            var dateTo = date_Print_To.Date;

            if (dateTo < dateFrom)
            {
                date_Print_From.Date = dateTo;
            }
        }

        protected void btn_Print_View_Click(object sender, EventArgs e)
        {
            var sql = @"
;WITH
i AS(
	SELECT
		HdrNo,
		CAST(MIN(CommittedDate) AS DATE) AS CommittedDate
	FROM
		[IN].Inventory
	WHERE
		[Type]='RC'
		AND CAST(CommittedDate as DATE) BETWEEN @FrDate AND @ToDate
	GROUP BY
		HdrNo

)
SELECT
	RecNo
FROM
	PC.Rec
	LEFT JOIN i
		ON i.HdrNo=rec.RecNo
WHERE
	CASE @DateType
		WHEN 1 THEN InvoiceDate
		WHEN 2 THEN i.CommittedDate
		ELSE RecDate
	END BETWEEN @FrDate AND @ToDate
	AND rec.VendorCode BETWEEN CASE WHEN ISNULL(@FrVendor,'')='' THEN rec.VendorCode ELSE @FrVendor END AND CASE WHEN ISNULL(@ToVendor,'')='' THEN rec.VendorCode ELSE @ToVendor END
	AND DocStatus IN(
		CASE WHEN @IsReceive = 1 THEN 'Received' ELSE NULL END,
		CASE WHEN @IsCommit = 1 THEN 'Committed' ELSE NULL END,
		CASE WHEN @IsVoid = 1 THEN 'VOIDED' ELSE NULL END
	)
ORDER BY
	RecNo";
            var parameters = new List<Blue.DAL.DbParameter>();

            parameters.Add(new Blue.DAL.DbParameter("@DateType", ddl_Print_DateType.SelectedIndex.ToString()));
            parameters.Add(new Blue.DAL.DbParameter("@FrDate", date_Print_From.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)));
            parameters.Add(new Blue.DAL.DbParameter("@ToDate", date_Print_To.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)));
            parameters.Add(new Blue.DAL.DbParameter("@FrVendor", ddl_Print_VendorFrom.Value.ToString()));
            parameters.Add(new Blue.DAL.DbParameter("@ToVendor", ddl_Print_VendorTo.Value.ToString()));
            parameters.Add(new Blue.DAL.DbParameter("@IsReceive", chk_Report_Status_Received.Checked ? "1" : "0"));
            parameters.Add(new Blue.DAL.DbParameter("@IsCommit", chk_Report_Status_Committed.Checked ? "1" : "0"));
            parameters.Add(new Blue.DAL.DbParameter("@IsVoid", chk_Report_Status_Voided.Checked ? "1" : "0"));


            var dt = bu.DbExecuteQuery(sql.ToString(), parameters.ToArray(), LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var items = dt.AsEnumerable()
                    .Select(x => x.Field<string>("RecNo"))
                    .ToArray();

                var id = Guid.NewGuid().ToString();
                
                Session[id] = string.Join(",", items);


                var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
                var report = "ReceivingForm";
                var url = urlBuilder.ToString().TrimEnd('/') + string.Format("/RPT/PrintForm.aspx?Report={0}&ID={1}", report, id);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}','_newtab');", url), true);
            }
            else
            {
                lbl_Warning.Text = "No data";
                pop_Warning.ShowOnPageLoad = true;

            }
        }


        private void OpenReportNewTab(string id)
        {
            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
            var report = "ReceivingForm";
            var url = urlBuilder.ToString().TrimEnd('/') + string.Format("/RPT/PrintForm.aspx?Report={0}&ID={1}", report, id);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}','_newtab');", url), true);
        }

        #endregion

    }
}