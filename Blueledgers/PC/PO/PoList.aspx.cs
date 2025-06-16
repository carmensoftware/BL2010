using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BlueLedger.PL.BaseClass;
using System.Text;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.PC.PO
{
    public partial class PoList : BasePage
    {
        #region "Attributes"

        private readonly Blue.DAL.DbHandler _dbHandler = new Blue.DAL.DbHandler();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private readonly Blue.BL.PC.PO.PO _po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt _poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.PC.PR.PRDt _prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.AP.Vendor _vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.APP.WF _workflow = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private DataSet _dsPo = new DataSet();
        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private readonly string module = "2.2";

        #endregion

        // Permission
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(module, LoginInfo.LoginName, LoginInfo.ConnStr);
            if (pagePermiss < 3)
            {
                ListPage.CreateItems.Visible = false;
                ListPage.ClosePOItems.Visible = false;
            }
        }

        #region --Event(s)--

        protected override void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                pop_PrintByDate.ShowOnPageLoad = false;

                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From Purchase Request", "PR"));
                ListPage.CreateItems.Items.FindByName("PR").NavigateUrl = "~/PC/PO/PoList.aspx?MENU=PR";


                ListPage.ClosePOItems.NavigateUrl = "~/PC/PO/PoList.aspx?MENU=CLOSEPO";

                ListPage.ExportItems.NavigateUrl = "~/PC/PO/PoList.aspx?MENU=EXPORT";


                //ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Print by Date", "PrintByDate"));
                //ListPage.PrintItems.Items.FindByName("PrintByDate").NavigateUrl = "~/PC/PO/PoList.aspx?MENU=PrintByDate";

                var enablePrintByBatch = config.GetValue("PC", "PO", "PrintByBatch", LoginInfo.ConnStr);

                if (!string.IsNullOrEmpty(enablePrintByBatch) && (enablePrintByBatch == "1" || enablePrintByBatch.ToLower() == "true"))
                {
                    var btn_PrintByDate = new DevExpress.Web.ASPxMenu.MenuItem("Print by Date ...", "PrintByDate");

                    btn_PrintByDate.ItemStyle.ForeColor = System.Drawing.Color.White;
                    btn_PrintByDate.ItemStyle.Font.Size = FontUnit.Smaller;
                    btn_PrintByDate.NavigateUrl = "~/PC/PO/PoList.aspx?MENU=PrintByDate";
                    ListPage.menuItems.Insert(ListPage.menuItems.Count - 2, btn_PrintByDate);
                }

                ListPage.DataBind();
            }


            if (Request.Params["MENU"] != null)
            {
                var vid = Request.QueryString["VID"] != null ? Request.QueryString["VID"].ToString() : "";
                var page = Request.QueryString["page"] != null ? Request.QueryString["page"].ToString() : "1";

                Session["MENU"] = Request.Params["MENU"].ToString();
                Response.Redirect("PoList.aspx" + string.Format("?VID={0}&page={1}", vid, page));
            }

            // Action menu by Session
            if (Session["MENU"] != null)
            {
                var mode = Session["MENU"].ToString();

                Session.Remove("MENU");

                switch (mode.ToUpper())
                {
                    case "PR":
                        DisplayPrList();
                        break;
                    case "CLOSEPO":
                        DisplayClosePO();
                        break;
                    case "EXPORT":
                        DisplayExport();
                        break;
                    case "PRINTBYDATE":
                        DisplayPrintByDate();
                        break;
                }
            }

            Control_HeaderMenuBar();
            base.Page_Load(sender, e);
        }

        protected void btn_SuccessOk_Click(object sender, EventArgs e)
        {
            // Redirec to POEdit.aspx page
            Response.Redirect("PoList.aspx");
        }

        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            GeneratePO();
            //SavePO();
        }

        protected void btn_Generate_Click(object sender, EventArgs e)
        {
            var selectedItems = grd_PRList2.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("Chk_Item")).Checked);

            if (selectedItems > 0)
            {
                lbl_ConfirmGenerate.Text = string.Format("{0} selected item(s) to generate PO.", selectedItems);
                pop_Confirm.ShowOnPageLoad = true;
            }
            else
            {
                //lbl_Warning.Text = @"Do not have data to generate PO.";
                lbl_Warning.Text = "Please select any Purchase Request.";
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        protected void btn_Abort_Click(object sender, EventArgs e)
        {
            //grd_PRList.Selection.UnselectAll();

            //Unselect All
            for (var i = 0; i <= grd_PRList2.Rows.Count - 1; i++)
            {
                var chkItem = (CheckBox)grd_PRList2.Rows[i].FindControl("Chk_Item");
                chkItem.Checked = false;
            }

            pop_Confirm.ShowOnPageLoad = false;
            pop_PR.ShowOnPageLoad = false;
        }

        protected void grd_PRList2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (e.Row.FindControl("lbl_RefNo") != null)
                    {
                        var lblRef = (Label)e.Row.FindControl("lbl_RefNo");
                        lblRef.Text = DataBinder.Eval(e.Row.DataItem, "PRNo").ToString();
                    }
                    if (e.Row.FindControl("lbl_PrType") != null)
                    {
                        var lblPrType = (Label)e.Row.FindControl("lbl_PrType");
                        lblPrType.Text = DataBinder.Eval(e.Row.DataItem, "PrTypeDesc").ToString();
                    }
                    if (e.Row.FindControl("lbl_PRDate") != null)
                    {
                        var lblPRDate = (Label)e.Row.FindControl("lbl_PRDate");
                        lblPRDate.Text =
                            DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString())
                                .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                    }
                    if (e.Row.FindControl("lbl_vendor_Nm") != null)
                    {
                        var lblvendor = (Label)e.Row.FindControl("lbl_vendor_Nm");
                        lblvendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorName").ToString();
                    }
                    if (e.Row.FindControl("lbl_BUCode") != null)
                    {
                        var lblBuCode = (Label)e.Row.FindControl("lbl_BUCode");
                        lblBuCode.Text = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();
                    }
                    if (e.Row.FindControl("lbl_DescPricelist") != null)
                    {
                        var lblDescPricelist = (Label)e.Row.FindControl("lbl_DescPricelist");
                        lblDescPricelist.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                    }
                    if (e.Row.FindControl("lbl_DeliDate") != null)
                    {
                        var lblDeliDate = (Label)e.Row.FindControl("lbl_DeliDate");
                        lblDeliDate.Text =
                            DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString())
                                .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                    }
                    break;
            }
        }

        protected void grd_Success2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (e.Row.FindControl("lbl_RefNo") != null)
                    {
                        var lblRef = (Label)e.Row.FindControl("lbl_RefNo");
                        lblRef.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                    }
                    if (e.Row.FindControl("lbl_Vendor") != null)
                    {
                        var lblVendorNm = (Label)e.Row.FindControl("lbl_Vendor");
                        //var lblPRDate = (Label)e.Row.FindControl("lbl_Vendor")
                        lblVendorNm.Text = DataBinder.Eval(e.Row.DataItem, "VendorName").ToString();
                    }
                    if (e.Row.FindControl("lbl_VendorEmail") != null)
                    {
                        var lblVendorEmail = (Label)e.Row.FindControl("lbl_VendorEmail");
                        lblVendorEmail.Text = DataBinder.Eval(e.Row.DataItem, "VendorEmail").ToString();
                    }
                    if (e.Row.FindControl("lbl_Amount") != null)
                    {
                        var lblAmount = (Label)e.Row.FindControl("lbl_Amount");
                        var decAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "Amount").ToString());
                        lblAmount.Text = String.Format(DefaultAmtFmt, decAmt);
                    }
                    if (e.Row.FindControl("lbl_CompositeKey") != null)
                    {
                        var lblCompositeKey = (Label)e.Row.FindControl("lbl_CompositeKey");
                        lblCompositeKey.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                        //DataBinder.Eval(e.Row.DataItem, "PoNo").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "PoDtNo").ToString();
                    }
                    break;
            }
        }

        protected void grd_Success2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                var lbl_RefNo = gvRow.FindControl("lbl_RefNo") as Label;

                DataSet ds = new DataSet();
                string strErr = string.Empty;
                // Following from: PO.aspx
                if (_po.GetListByPoNo2(ds, ref strErr, lbl_RefNo.Text, LoginInfo.ConnStr))
                {
                    if (ds.Tables[_po.TableName].Rows.Count > 0)
                        if (ds.Tables[_po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                            ds.Tables[_po.TableName].Rows[0]["DocStatus"] = "Printed";

                    var resultSave = _po.SaveOnlyPO(ds, LoginInfo.ConnStr);
                    Report rpt = new Report();
                    rpt.PrintForm(this, "../../RPT/PrintForm.aspx", lbl_RefNo.Text, "PurchaseOrderForm");
                }

            }
        }

        protected void grd_ClosePO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (e.Row.FindControl("lbl_RefNo") != null)
                    {
                        var lblRef = (Label)e.Row.FindControl("lbl_RefNo");
                        lblRef.Text = DataBinder.Eval(e.Row.DataItem, "PONo").ToString();
                    }
                    if (e.Row.FindControl("lbl_DeliDate") != null)
                    {
                        var lblDeliDate = (Label)e.Row.FindControl("lbl_DeliDate");
                        lblDeliDate.Text =
                            DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliDate").ToString())
                                .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                    }
                    if (e.Row.FindControl("lbl_Desc") != null)
                    {
                        var lblDesc = (Label)e.Row.FindControl("lbl_Desc");
                        lblDesc.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                    }
                    if (e.Row.FindControl("lbl_Status") != null)
                    {
                        var lblStatus = (Label)e.Row.FindControl("lbl_Status");
                        lblStatus.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                    }
                    break;
            }
        }

        protected void Chk_ItemAll_CheckedChanged(object sender, EventArgs e)
        {
            var chkAll = (CheckBox)grd_ClosePO.HeaderRow.FindControl("Chk_ItemAll");

            if (chkAll.Checked.ToString() == "True")
            {
                for (var i = 0; i < grd_ClosePO.Rows.Count; i++)
                {
                    var chkSelect = (CheckBox)grd_ClosePO.Rows[i].FindControl("Chk_ClosePO");
                    chkSelect.Checked = true;
                }
            }
            else
            {
                for (var i = 0; i < grd_ClosePO.Rows.Count; i++)
                {
                    var chkSelect = (CheckBox)grd_ClosePO.Rows[i].FindControl("Chk_ClosePO");
                    chkSelect.Checked = false;
                }
            }
        }

        protected void chk_AllPR_CheckedChanged(object sender, EventArgs e)
        {
            var chkAllPR = grd_PRList2.HeaderRow.FindControl("chk_AllPR") as CheckBox;

            if (chkAllPR != null && chkAllPR.Checked.ToString() == "True")
            {
                for (var i = 0; i < grd_PRList2.Rows.Count; i++)
                {
                    var chkItem = grd_PRList2.Rows[i].FindControl("Chk_Item") as CheckBox;
                    if (chkItem != null) chkItem.Checked = true;
                }
            }
            else
            {
                for (var i = 0; i < grd_PRList2.Rows.Count; i++)
                {
                    var chkItem = grd_PRList2.Rows[i].FindControl("Chk_Item") as CheckBox;
                    if (chkItem != null) chkItem.Checked = false;
                }
            }
        }

        protected void Chk_Item_CheckedChanged(object sender, EventArgs e)
        {
            var chkAllPR = grd_PRList2.HeaderRow.FindControl("chk_AllPR") as CheckBox;
            var intChk = 0;

            for (var i = 0; i <= grd_PRList2.Rows.Count - 1; i++)
            {
                var chkItem = grd_PRList2.Rows[i].FindControl("Chk_Item") as CheckBox;
                if (chkItem != null && chkItem.Checked)
                {
                    intChk += 1;
                }
            }

            if (chkAllPR != null) chkAllPR.Checked = grd_PRList2.Rows.Count == intChk;
        }

        protected void btn_OkClosePO_Click(object sender, EventArgs e)
        {
            var intChk = 0;

            for (var i = 0; i < grd_ClosePO.Rows.Count; i++)
            {
                var chkItem = (CheckBox)grd_ClosePO.Rows[i].FindControl("Chk_ClosePO");

                if (chkItem.Checked)
                {
                    intChk += 1;
                }
            }

            if (intChk == 0)
            {
                pop_Warning.ShowOnPageLoad = true;
                lbl_Warning.Text = @"Please select data for close PO.";
            }
            else
            {
                pop_ConfirmClosePO.ShowOnPageLoad = true;
            }
        }

        protected void btn_ConfirmClosePO_Click(object sender, EventArgs e)
        {
            var msgError = string.Empty;

            _dsPo = (DataSet)Session["dsClosePO"];

            for (var i = 0; i < grd_ClosePO.Rows.Count; i++)
            {
                var chkItem = (CheckBox)grd_ClosePO.Rows[i].FindControl("Chk_ClosePO");

                if (chkItem.Checked)
                {
                    _dsPo.Tables[_po.TableName].Rows[i]["DocStatus"] = "Closed";
                    _dsPo.Tables[_po.TableName].Rows[i]["UpdatedDate"] = ServerDateTime;
                    _dsPo.Tables[_po.TableName].Rows[i]["UpdatedBy"] = LoginInfo.LoginName;

                    var result = _poDt.GetListByPoNo(_dsPo, ref msgError,
                        _dsPo.Tables[_po.TableName].Rows[i]["PONo"].ToString(), LoginInfo.ConnStr);

                    if (result && _dsPo.Tables[_poDt.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow drPODt in _dsPo.Tables[_poDt.TableName].Rows)
                        {
                            var ordqty = Convert.ToDecimal(drPODt["OrdQty"].ToString());
                            var rcvqty = Convert.ToDecimal(drPODt["RcvQty"].ToString());
                            var cancelqty = Convert.ToDecimal(drPODt["CancelQty"].ToString());

                            if (ordqty >= rcvqty + cancelqty)
                            {
                                drPODt["CancelQty"] = cancelqty.ToString();
                            }
                        }
                    }
                }
            }

            // Commit change to database
            var chkSave = _po.Save(_dsPo, LoginInfo.ConnStr);

            if (chkSave)
            {
                _dsPo.Clear();
                grd_ClosePO.DataSource = _dsPo;
                grd_ClosePO.DataBind();


                foreach (DataRow dr in _dsPo.Tables[_po.TableName].Rows)
                {
                    _transLog.Save("PC", "PO", dr["PoNo"].ToString(), "CLOSED", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);
                }



                GetResultPoListByClosePO();

                pop_ConfirmClosePO.ShowOnPageLoad = false;
            }
        }

        protected void btn_CancelClosePO_Click(object sender, EventArgs e)
        {
            pop_ConfirmClosePO.ShowOnPageLoad = true;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            _dsPo.Clear();

            grd_ClosePO.DataSource = null;
            grd_ClosePO.DataBind();

            btn_OkClosePO.Enabled = false;

            pop_ClosePO.ShowOnPageLoad = false;
        }

        protected void btn_SearchClosePO_Click(object sender, EventArgs e)
        {
            GetResultPoListByClosePO();
        }

        protected void ddl_CurrCode_Init(object sender, EventArgs e)
        {
            var defaultCurrency = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

            //ddl_CurrCode.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_CurrCode.DataSource = _prDt.DbExecuteQuery("SELECT DISTINCT CurrencyCode FROM PC.PrDt ORDER BY CurrencyCode", null, LoginInfo.ConnStr);
            ddl_CurrCode.DataTextField = "CurrencyCode";
            ddl_CurrCode.DataValueField = "CurrencyCode";
            ddl_CurrCode.DataBind();
            ddl_CurrCode.SelectedValue = defaultCurrency;
        }

        protected void ddl_CurrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_CurrCode = (DropDownList)sender;
            var dsTmp = new DataSet();
            var msgError = string.Empty;

            var result = _prDt.GetList_Approve_GroupByPrNo(dsTmp, ref msgError, LoginInfo.ConnStr);
            if (result)
            {
                string currCode = ddl_CurrCode.SelectedValue;
                DataView dV = dsTmp.Tables[_prDt.TableName].DefaultView;
                dV.RowFilter = string.Format("CurrencyCode = '{0}'", currCode);

                grd_PRList2.DataSource = dV;
                grd_PRList2.DataBind();
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grd_Success2.Rows.Count; i++)
            {
                var chk_Ref = (CheckBox)grd_Success2.Rows[i].FindControl("chk_Ref");
                var lbl_RefNo = (Label)grd_Success2.Rows[i].FindControl("lbl_RefNo");
                if (chk_Ref != null && lbl_RefNo != null)
                {
                    DataSet ds = new DataSet();
                    string strErr = string.Empty;
                    if (chk_Ref.Checked && chk_Ref.Enabled)
                    {
                        // Following from: PO.aspx
                        if (_po.GetListByPoNo2(ds, ref strErr, lbl_RefNo.Text, LoginInfo.ConnStr))
                        {
                            if (ds.Tables[_po.TableName].Rows.Count > 0)
                                if (ds.Tables[_po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                                    ds.Tables[_po.TableName].Rows[0]["DocStatus"] = "Printed";

                            var resultSave = _po.SaveOnlyPO(ds, LoginInfo.ConnStr);
                            Report rpt = new Report();
                            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", lbl_RefNo.Text, "PurchaseOrderForm");

                            // Comment: Still focus on new tab.
                            // About: browser.tabs.loadDivertedInBackground=true !!!
                            //string script = "<script> window.open('" + "../../RPT/PrintForm.aspx" + "?ID=" + lbl_RefNo.Text + "&Report=" + "PurchaseOrderForm" + "', '_blank'); ";

                            //script += "window.blur(); ";
                            //script += " window.focus();";
                            //script += " window.parent.focus();";

                            //script += " </script>";
                            //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "newForm", script, false);
                        }

                        chk_Ref.Enabled = false;
                    }

                    else if (!chk_Ref.Checked)
                    { chk_Ref.Enabled = true; }
                }

            }
        }

        protected void btn_SendMail_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("Sent");
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns.Add("PoNo", Type.GetType("System.String"));
            dt.Columns.Add("VendorName", Type.GetType("System.String"));
            dt.Columns.Add("VendorEmail", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
            dt.Columns.Add("Error", Type.GetType("System.String"));

            // Set up the ID column as the PrimaryKey
            DataColumn[] prmk = new DataColumn[1];
            prmk[0] = dt.Columns["ID"];
            dt.PrimaryKey = prmk;
            dt.Columns["ID"].AutoIncrement = true;
            dt.Columns["ID"].AutoIncrementSeed = 1;
            dt.Columns["ID"].ReadOnly = true;


            //var errors = new List<string>();

            for (int i = 0; i < grd_Success2.Rows.Count; i++)
            {
                var chk_Ref = (CheckBox)grd_Success2.Rows[i].FindControl("chk_Ref");
                if (chk_Ref != null)
                {
                    if (chk_Ref.Checked)
                    {
                        PoReportMail poReportMail = new PoReportMail();

                        var lbl_RefNo = (Label)grd_Success2.Rows[i].FindControl("lbl_RefNo");
                        var lbl_Vendor = (Label)grd_Success2.Rows[i].FindControl("lbl_Vendor");

                        string poNo = lbl_RefNo.Text;
                        string vendorCode = lbl_Vendor.Text.Split('-')[0].ToString().Trim();
                        string vendorEmail = poReportMail.GetVendorEmail(vendorCode, LoginInfo.ConnStr);

                        string error = string.Empty;
                        if (vendorEmail == string.Empty)
                            error = string.Format("{0} not found email.", vendorCode);
                        else
                            error = poReportMail.SendEmailToSupplier(poNo, vendorEmail, LoginInfo.ConnStr);

                        DataRow dr = dt.NewRow();
                        dr["PoNo"] = poNo;
                        dr["VendorName"] = lbl_Vendor.Text;
                        dr["VendorEmail"] = vendorEmail;
                        if (error == string.Empty)
                        {
                            // Update DocStatus = "Printed"
                            _dbHandler.DbExecuteQuery(string.Format("UPDATE PC.PO SET DocStatus='Printed' WHERE PoNo = '{0}'", poNo), null, LoginInfo.ConnStr);
                            dr["Status"] = "Sent";

                        }
                        else
                        {
                            dr["Status"] = "Error";
                            dr["Error"] = error;
                            //errors.Add(string.Format("{0} : {1}", poNo, error));
                        }

                        dt.Rows.Add(dr);
                    }
                }
            }



            grd_MailResult.DataSource = dt;
            grd_MailResult.DataBind();
            pop_MailResult.ShowOnPageLoad = true;

            //if (errors.Count > 0)
            //{
            //    string er = string.Join("\n", errors.ToArray());
            //    lbl_Warning.Text = er;
            //}
            //else
            //    lbl_Warning.Text = "Sent.";

            //lbl_Warning.Width = 320;
            //pop_Warning.ShowOnPageLoad = true;
        }

        protected void grd_MailResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbl_Status = (Label)e.Row.FindControl("lbl_Status");
                if (lbl_Status != null)
                {
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                    lbl_Status.ToolTip = lbl_Status.Text == string.Empty ? lbl_Status.Text : DataBinder.Eval(e.Row.DataItem, "Status").ToString();

                }
            }

        }

        protected void Chk_AllPO_CheckedChanged(object sender, EventArgs e)
        {
            var chkAllPO = (CheckBox)grd_Success2.HeaderRow.FindControl("chk_allPO");
            if (chkAllPO != null)
            {
                for (int i = 0; i < grd_Success2.Rows.Count; i++)
                {
                    var chk_Ref = (CheckBox)grd_Success2.Rows[i].FindControl("chk_Ref");
                    if (chk_Ref != null) chk_Ref.Checked = chkAllPO.Checked;
                }
            }
        }

        protected void chk_Ref_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            string dateType = "0";
            string fDate, tDate;
            string fLoc, tLoc;
            string fCat, tCat;
            string fSCat, tSCat;
            string fIG, tIG;
            string fProd, tProd;
            string docStatus = "0";
            string orderBy = "0";

            dateType = ddl_DateType.SelectedValue.ToString();

            fDate = de_DateFrom.Date.ToString("yyyy-MM-dd");
            tDate = de_DateTo.Date.ToString("yyyy-MM-dd");

            fLoc = ddl_FLocation.Value.ToString();
            tLoc = ddl_TLocation.Value.ToString();

            fCat = ddl_FCat.Value.ToString();
            tCat = ddl_TCat.Value.ToString();

            fSCat = ddl_FSCat.Value.ToString();
            tSCat = ddl_TSCat.Value.ToString();

            fIG = ddl_FIG.Value.ToString();
            tIG = ddl_TIG.Value.ToString();

            fProd = ddl_FProduct.Value.ToString();
            tProd = ddl_TProduct.Value.ToString();

            docStatus = ddl_DocStatus.SelectedValue.ToString();
            orderBy = ddl_OrderBy.SelectedIndex.ToString();


            string sql = "EXEC PC.POExport ";
            sql += string.Format(" @DateType='{0}',", dateType);
            sql += string.Format(" @FDate='{0}',", fDate);
            sql += string.Format(" @TDate='{0}',", tDate);
            sql += string.Format(" @FLocation='{0}',", fLoc);
            sql += string.Format(" @TLocation='{0}',", tLoc);
            sql += string.Format(" @FCat='{0}',", fCat);
            sql += string.Format(" @TCat='{0}',", tCat);
            sql += string.Format(" @FSCat='{0}',", fSCat);
            sql += string.Format(" @TSCat='{0}',", tSCat);
            sql += string.Format(" @FIG='{0}',", fIG);
            sql += string.Format(" @TIG='{0}',", tIG);
            sql += string.Format(" @FProduct='{0}',", fProd);
            sql += string.Format(" @TProduct='{0}',", tProd);
            sql += string.Format(" @DocStatus='{0}',", docStatus);
            sql += string.Format(" @OrderBy='{0}'", orderBy);

            DataTable dt = _po.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);
            string exportPattern = config.GetValue("PC", "PO", "ExportColumns", LoginInfo.ConnStr);
            if (exportPattern == string.Empty)
            {
                // Default
                ExportToFile(dt, ",");
            }
            else
            {
                //DataTable dt2 = new DataTable();
                //dt2 = dt.Copy();

                //exportPattern = "[Po Date],[Po No]";

                List<string> fields = exportPattern.Split(',').ToList();

                DataTable dt2 = new DataTable();
                foreach (string field in fields)
                {
                    string columnName = field.Replace("[", "").Replace("]", "").Trim();

                    if (dt.Columns.IndexOf(columnName) >= 0) // found
                    {
                        DataColumn dc = dt.Columns[columnName];
                        dt2.Columns.Add(columnName, dc.DataType);
                    }

                }

                // data
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dt2.NewRow();

                    foreach (DataColumn dc in dt2.Columns)
                    {
                        drNew[dc.ColumnName] = dr[dc.ColumnName];
                    }

                    dt2.Rows.Add(drNew);
                }

                ExportToFile(dt2, ",");
            }

        }

        private void ExportToFile(DataTable dt, string separator, string pattern = "")
        {

            //Response.ClearContent();
            //Response.Clear();
            //Response.ContentType = "text/plain";
            //Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedData.txt;");

            string fileName = string.Format("POExport-{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();

            string header = config.GetConfigValue("PC", "PO", "ExportHeader", LoginInfo.ConnStr);
            if (header.Trim() != string.Empty)
            {
                sb.AppendLine(header);
            }



            string excludeColumnHeader = config.GetConfigValue("PC", "PO", "ExportExcludeColumnHeader", LoginInfo.ConnStr);


            if (excludeColumnHeader == "0") // include column header
            {
                string columnNames = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnNames += dt.Columns[i].ColumnName + ",";
                }
                sb.AppendLine(columnNames);
            }
            //string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            //sb.AppendLine(string.Join(separator, columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.AppendLine(string.Join(separator, fields));
            }

            // the most easy way as you have type it
            Response.Write(sb.ToString());

            Response.Flush();
            Response.End();
        }

        protected void btn_Config_Click(object sender, EventArgs e)
        {
            string exportHeader = config.GetConfigValue("PC", "PO", "ExportHeader", LoginInfo.ConnStr);
            string excludeColumnHeader = config.GetConfigValue("PC", "PO", "ExportExcludeColumnHeader", LoginInfo.ConnStr);
            string exportColumn = config.GetValue("PC", "PO", "ExportColumns", LoginInfo.ConnStr);

            txt_ExportHeader.Text = exportHeader;
            chk_ExcludeColumnHeader.Checked = excludeColumnHeader == "1" ? true : false;
            txt_ExportColumns.Text = exportColumn;



            pn_Export.Visible = false;
            pn_Config.Visible = true;
        }

        protected void btn_ConfigSave_Click(object sender, EventArgs e)
        {
            config.SetConfigValue("PC", "PO", "ExportHeader", txt_ExportHeader.Text, LoginInfo.ConnStr);
            config.SetConfigValue("PC", "PO", "ExportExcludeColumnHeader", chk_ExcludeColumnHeader.Checked ? "1" : "0", LoginInfo.ConnStr);
            config.SetConfigValue("PC", "PO", "ExportColumns", txt_ExportColumns.Text, LoginInfo.ConnStr);

            pn_Export.Visible = true;
            pn_Config.Visible = false;
        }

        protected void btn_ConfigCancel_Click(object sender, EventArgs e)
        {
            pn_Export.Visible = true;
            pn_Config.Visible = false;
        }


        // pop_PrintByDate

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
            var sql = new StringBuilder();


            sql.AppendLine("SELECT PoNo");
            sql.AppendLine("FROM [PC].[PO]");
            sql.AppendLine("WHERE");

            var dateType = ddl_Print_DateType.SelectedIndex == 0 ? "PoDate" : "DeliDate";
            var frDate = date_Print_From.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var toDate = date_Print_To.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            sql.AppendFormat(" {0} BETWEEN '{1}' AND '{2}' ", dateType, frDate, toDate);

            var frVendor = ddl_Print_VendorFrom.SelectedItem.Value.ToString();
            var toVendor = ddl_Print_VendorTo.SelectedItem.Value.ToString();

            sql.AppendFormat(" AND Vendor BETWEEN '{0}' AND '{1}' ", frVendor, toVendor);

            var docStatusList = new List<string>();

            if (chk_Report_Status_Approved.Checked)
                docStatusList.Add("Approved");
            if (chk_Report_Status_Printed.Checked)
                docStatusList.Add("Printed");
            if (chk_Report_Status_Partial.Checked)
                docStatusList.Add("Partial");
            if (chk_Report_Status_Completed.Checked)
                docStatusList.Add("Completed");
            if (chk_Report_Status_Closed.Checked)
                docStatusList.Add("Closed");

            sql.AppendFormat(" AND DocStatus IN ('{0}')", string.Join("','", docStatusList));
            sql.AppendLine("ORDER BY Vendor, PoDate, PoNo");



            var dt = _po.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var items = dt.AsEnumerable()
                    .Select(x => x.Field<string>("PoNo"))
                    .ToArray();

                var id = Guid.NewGuid().ToString();

                Session[id] = string.Join(",", items);

                //var id = string.Join(",", items);
                var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
                var report = "PurchaseOrderForm";
                var url = urlBuilder.ToString().TrimEnd('/') + string.Format("/RPT/PrintForm.aspx?Report={0}&ID={1}", report, id);
                //var url = urlBuilder.ToString().TrimEnd('/') + string.Format("/RPT/PrintForm.aspx?ID={0}&Report={1}", id, report);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", string.Format("window.open('{0}','_newtab');", url), true);
            }
            else
            {
                lbl_Warning.Text = "No data";
                pop_Warning.ShowOnPageLoad = true;

            }
        }

        #endregion

        #region --MENU--

        private void DisplayPrList()
        {
            var dsTmp = new DataSet();
            var msgError = string.Empty;

            var result = _prDt.GetList_Approve_GroupByPrNo(dsTmp, ref msgError, LoginInfo.ConnStr);

            if (result)
            {
                string currCode = ddl_CurrCode.SelectedValue;
                DataView dv = dsTmp.Tables[_prDt.TableName].DefaultView;

                dv.RowFilter = string.Format("CurrencyCode = '{0}'", currCode);
                grd_PRList2.DataSource = dv;
                grd_PRList2.DataBind();
            }
            //}

            pop_PR.ShowOnPageLoad = true;
        }

        private void DisplayClosePO()
        {
            pop_ClosePO.ShowOnPageLoad = true;

            var dtVendor = _vendor.GetVendorGroupByPO(LoginInfo.ConnStr);
            ddl_Vendor.DataSource = dtVendor;
            ddl_Vendor.DataBind();

            dte_DeliDate.Date = ServerDateTime.Date;

            GetResultPoListByClosePO();
        }

        private void DisplayExport()
        {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            de_DateFrom.Date = firstDayOfMonth;
            de_DateTo.Date = date;

            DataTable dtLocation = _po.DbExecuteQuery("SELECT LocationCode Code, LocationCode + ' : ' + LocationName as Item FROM [IN].StoreLocation ORDER BY LocationCode", null, LoginInfo.ConnStr);
            ddl_FLocation.DataSource = dtLocation;
            ddl_FLocation.ValueField = "Code";
            ddl_FLocation.TextField = "Item";
            ddl_FLocation.DataBind();
            ddl_FLocation.SelectedIndex = 0;

            ddl_TLocation.DataSource = dtLocation;
            ddl_TLocation.ValueField = "Code";
            ddl_TLocation.TextField = "Item";
            ddl_TLocation.DataBind();
            ddl_TLocation.SelectedIndex = ddl_TLocation.Items.Count - 1;


            DataTable dtCategory = _po.DbExecuteQuery("SELECT CategoryCode Code, CategoryCode + ' : ' + CategoryName as Item FROM [IN].ProductCategory WHERE LevelNo=1 ORDER BY CategoryCode", null, LoginInfo.ConnStr);
            ddl_FCat.DataSource = dtCategory;
            ddl_FCat.ValueField = "Code";
            ddl_FCat.TextField = "Item";
            ddl_FCat.DataBind();
            ddl_FCat.SelectedIndex = 0;

            ddl_TCat.DataSource = dtCategory;
            ddl_TCat.ValueField = "Code";
            ddl_TCat.TextField = "Item";
            ddl_TCat.DataBind();
            ddl_TCat.SelectedIndex = ddl_TCat.Items.Count - 1;


            DataTable dtSubCategory = _po.DbExecuteQuery("SELECT CategoryCode Code, CategoryCode + ' : ' + CategoryName as Item FROM [IN].ProductCategory WHERE LevelNo=2 ORDER BY CategoryCode", null, LoginInfo.ConnStr);
            ddl_FSCat.DataSource = dtSubCategory;
            ddl_FSCat.ValueField = "Code";
            ddl_FSCat.TextField = "Item";
            ddl_FSCat.DataBind();
            ddl_FSCat.SelectedIndex = 0;

            ddl_TSCat.DataSource = dtSubCategory;
            ddl_TSCat.ValueField = "Code";
            ddl_TSCat.TextField = "Item";
            ddl_TSCat.DataBind();
            ddl_TSCat.SelectedIndex = ddl_TSCat.Items.Count - 1;

            DataTable dtItemGroup = _po.DbExecuteQuery("SELECT CategoryCode Code, CategoryCode + ' : ' + CategoryName as Item FROM [IN].ProductCategory WHERE LevelNo=3 ORDER BY CategoryCode", null, LoginInfo.ConnStr);
            ddl_FIG.DataSource = dtItemGroup;
            ddl_FIG.ValueField = "Code";
            ddl_FIG.TextField = "Item";
            ddl_FIG.DataBind();
            ddl_FIG.SelectedIndex = 0;

            ddl_TIG.DataSource = dtItemGroup;
            ddl_TIG.ValueField = "Code";
            ddl_TIG.TextField = "Item";
            ddl_TIG.DataBind();
            ddl_TIG.SelectedIndex = ddl_TIG.Items.Count - 1;

            DataTable dtProduct = _po.DbExecuteQuery("SELECT CategoryCode Code, CategoryCode + ' : ' + CategoryName as Item FROM [IN].ProductCategory WHERE LevelNo=3 ORDER BY CategoryCode", null, LoginInfo.ConnStr);
            ddl_FProduct.DataSource = dtProduct;
            ddl_FProduct.ValueField = "Code";
            ddl_FProduct.TextField = "Item";
            ddl_FProduct.DataBind();
            ddl_FProduct.SelectedIndex = 0;

            ddl_TProduct.DataSource = dtProduct;
            ddl_TProduct.ValueField = "Code";
            ddl_TProduct.TextField = "Item";
            ddl_TProduct.DataBind();
            ddl_TProduct.SelectedIndex = ddl_TProduct.Items.Count - 1;


            DataTable dt = _po.DbExecuteQuery("exec pc.poexport @FDate='2000-01-01', @TDate='2000-01-01'", null, LoginInfo.ConnStr);

            string columns = string.Empty;
            foreach (DataColumn dc in dt.Columns)
            {
                columns += string.Format("[{0}] , ", dc.ColumnName);
            }

            lbl_AvialableFields.Text = columns;

            pop_Export.ShowOnPageLoad = true;
        }

        private void DisplayPrintByDate()
        {
            pop_PrintByDate.ShowOnPageLoad = true;

            date_Print_From.Date = DateTime.Today;
            date_Print_To.Date = DateTime.Today;

            var dtVendor = _po.DbExecuteQuery("SELECT VendorCode as [Code], [Name] FROM AP.Vendor WHERE IsActive=1 ORDER BY VendorCode", null, LoginInfo.ConnStr);

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



        }

        #endregion

        #region --Method(s)

        private void GetResultPoListByClosePO()
        {
            var strVendor = ddl_Vendor.SelectedItem.Value;
            var dteDeliDate = dte_DeliDate.Date;

            if ((strVendor != string.Empty) & (dteDeliDate.ToString() != DateTime.MinValue.ToString()))
            {
                var result = _po.GetListByClosePO(_dsPo, strVendor, dteDeliDate, LoginInfo.ConnStr);

                if (result)
                {
                    grd_ClosePO.DataSource = _dsPo.Tables[_po.TableName];
                    grd_ClosePO.DataBind();

                    btn_OkClosePO.Enabled = true;
                }
            }
            else
            {
                _dsPo.Clear();

                grd_ClosePO.DataSource = null;
                grd_ClosePO.DataBind();

                btn_OkClosePO.Enabled = false;
            }

            Session["dsClosePO"] = _dsPo;
        }

        protected void SaveOneBu()
        {
            //    // Get Pr, PrDt Table Schema
            //    var dsPO = new DataSet();
            //    var dsPoHd = new DataSet();
            //    var dsPoDt = new DataSet();
            //    var msgError = string.Empty;
            //    var strChkPrNo = string.Empty;
            //    var strChkBu = string.Empty;
            //    var strDescPR = string.Empty;

            //    // For Create New PONo.
            //    var getBuCode = string.Empty;
            //    var genPoNo = string.Empty;

            //    // Get Schema 
            //    var getPoSchema = _po.GetStructure2(dsPO, LoginInfo.ConnStr);
            //    var getPoDtSchema = _poDt.GetSchema(dsPO, LoginInfo.ConnStr);

            //    var intChkSelect = 0;

            //    //=== Generate data to Create PO And Pop Up Show PoNo ===
            //    for (var i = 0; i <= grd_PRList2.Rows.Count - 1; i++)
            //    {
            //        var chkItem = (CheckBox)grd_PRList2.Rows[i].FindControl("Chk_Item");
            //        var lblRefNo = (Label)grd_PRList2.Rows[i].FindControl("lbl_RefNo");
            //        var lblDescPricelist = (Label)grd_PRList2.Rows[i].FindControl("lbl_DescPricelist");

            //        if (chkItem.Checked)
            //        {
            //            intChkSelect += 1;
            //            strDescPR = lblDescPricelist.Text;

            //            if (strChkPrNo != string.Empty)
            //            {
            //                strChkPrNo += ", '" + lblRefNo.Text + "'";
            //            }
            //            else
            //            {
            //                strChkPrNo = "'" + lblRefNo.Text + "'";
            //            }
            //        }
            //    }

            //    if (strChkPrNo != string.Empty)
            //    {
            //        // Get data group by vendor.
            //        var getTemplateHd = _prDt.GetHd_ByPrNo(dsPoHd, ref msgError, strChkPrNo, LoginInfo.ConnStr);

            //        // Get PR Detail All Data for update field
            //        var getTemplateDt = _prDt.Get_ByPrNo(dsPO, ref msgError, strChkPrNo, LoginInfo.ConnStr);

            //        if (dsPoHd.Tables[_prDt.TableName].Rows.Count > 0)
            //        {
            //            var newPoNo = _po.GetNewID(ServerDateTime, LoginInfo.ConnStr);

            //            for (var y = 0; y < dsPoHd.Tables[_prDt.TableName].Rows.Count; y++)
            //            {
            //                var drPRDtGroup = dsPoHd.Tables[_prDt.TableName].Rows[y];

            //                // Create to PO -------------------------------------------------------------
            //                var drPO = dsPO.Tables[_po.TableName].NewRow();

            //                drPO["PoNo"] = newPoNo;
            //                drPO["PoDate"] = ServerDateTime;

            //                //ให้ user ป้อนเอง เพราะว่าสินค้าตัวเดียวกันอาจจะให้ Delivery Date ต่างกันก็ได้ 
            //                //drPO["DeliDate"]    = DateTime.Parse(drPRDtGroup["ReqDate"].ToString());

            //                //Fix Default
            //                drPO["Currency"] = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
            //                drPO["ExchageRate"] = 1;
            //                drPO["DocStatus"] = "Approved";
            //                drPO["IsVoid"] = "False";
            //                //drPO["Buyer"]       = LoginInfo.LoginName;

            //                drPO["Vendor"] = drPRDtGroup["VendorCode"].ToString();
            //                drPO["CreditTerm"] = _vendor.GetCreditTerm(drPRDtGroup["VendorCode"].ToString(),
            //                    LoginInfo.ConnStr);

            //                //2012-10-23 Check for get Description from PR but select only one PR.
            //                if (intChkSelect == 1)
            //                {
            //                    drPO["Description"] = strDescPR;
            //                }

            //                if (ListPage.WorkFlowEnable)
            //                {
            //                    drPO["ApprStatus"] = _workflow.GetHdrApprStatus("PC", "PO", LoginInfo.ConnStr);
            //                }

            //                drPO["CreatedDate"] = ServerDateTime;
            //                drPO["CreatedBy"] = LoginInfo.LoginName;
            //                drPO["UpdatedDate"] = ServerDateTime;
            //                drPO["UpdatedBy"] = LoginInfo.LoginName;

            //                dsPO.Tables[_po.TableName].Rows.Add(drPO);

            //                // Create to PoDt -------------------------------------------------------------

            //                var dtPoDt = _prDt.GetDt_ByPrNo(strChkPrNo, drPRDtGroup["VendorCode"].ToString(),
            //                    drPRDtGroup["ReqDate"].ToString(), LoginInfo.ConnStr);

            //                if (dtPoDt.Rows.Count > 0)
            //                {
            //                    foreach (DataRow drTemplateDt in dtPoDt.Rows)
            //                    {
            //                        //Update field buyer   
            //                        drPO["Buyer"] = drTemplateDt["Buyer"].ToString();
            //                        drPO["DeliDate"] = DateTime.Parse(drTemplateDt["ReqDate"].ToString());

            //                        //Create PoDt.
            //                        var drPoDt = dsPO.Tables[_poDt.TableName].NewRow();
            //                        drPoDt["PoNo"] = drPO["PoNo"];
            //                        drPoDt["PoDt"] = dsPO.Tables[_poDt.TableName].Rows.Count + 1;
            //                        drPoDt["Product"] = drTemplateDt["ProductCode"].ToString();
            //                        drPoDt["Descen"] = drTemplateDt["Descen"].ToString();
            //                        drPoDt["Descll"] = drTemplateDt["Descll"].ToString();
            //                        drPoDt["Unit"] = drTemplateDt["OrderUnit"].ToString();
            //                        drPoDt["ReqDate"] = drTemplateDt["ReqDate"].ToString();
            //                        drPoDt["DeliveryPoint"] = drTemplateDt["DeliPoint"].ToString();
            //                        drPoDt["Location"] = drTemplateDt["LocationCode"].ToString();
            //                        drPoDt["IsAdj"] = drTemplateDt["TaxAdj"].ToString();
            //                        drPoDt["Buyer"] = drTemplateDt["Buyer"].ToString(); //LoginInfo.LoginName;
            //                        drPoDt["CancelQty"] = "0.00";

            //                        drPoDt["Comment"] = drTemplateDt["Comment"].ToString();

            //                        if (drTemplateDt["ApprQty"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["OrdQty"] = drTemplateDt["ApprQty"].ToString();
            //                        }

            //                        if (drTemplateDt["FOCQty"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["FOCQty"] = drTemplateDt["FOCQty"].ToString();
            //                        }

            //                        if (drTemplateDt["RcvQty"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["RcvQty"] = drTemplateDt["RcvQty"].ToString();
            //                        }

            //                        if (drTemplateDt["DiscPercent"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["Discount"] = drTemplateDt["DiscPercent"].ToString();
            //                        }

            //                        if (drTemplateDt["DiscAmt"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["DisCountAmt"] = drTemplateDt["DiscAmt"].ToString();
            //                        }

            //                        if (drTemplateDt["TaxAmt"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["TaxAmt"] = drTemplateDt["TaxAmt"].ToString();
            //                        }

            //                        if (drTemplateDt["NetAmt"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["NetAmt"] = drTemplateDt["NetAmt"].ToString();
            //                        }

            //                        if (drTemplateDt["TotalAmt"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["TotalAmt"] = drTemplateDt["TotalAmt"].ToString();
            //                        }

            //                        if (drTemplateDt["TaxType"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["TaxType"] = drTemplateDt["TaxType"].ToString();
            //                        }

            //                        if (drTemplateDt["TaxRate"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["TaxRate"] = drTemplateDt["TaxRate"].ToString();
            //                        }

            //                        if (drTemplateDt["Price"].ToString() != string.Empty)
            //                        {
            //                            drPoDt["Price"] = drTemplateDt["Price"].ToString();
            //                        }

            //                        dsPO.Tables[_poDt.TableName].Rows.Add(drPoDt);

            //                        //Set PR Detail Because this PRNo create PO. ----------------------------------------------
            //                        for (var i = 0; i < dsPO.Tables[_prDt.TableName].Rows.Count; i++)
            //                        {
            //                            var drUpdatePrDt = dsPO.Tables[_prDt.TableName].Rows[i];

            //                            var dateUpdate = DateTime.Parse(drUpdatePrDt["ReqDate"].ToString());
            //                            var datePoDt = DateTime.Parse(drPoDt["ReqDate"].ToString());

            //                            //if (drUpdatePrDt["PONo"].ToString() == string.Empty & drUpdatePrDt["PODtNo"].ToString() == string.Empty)
            //                            //{                                     
            //                            if ((drUpdatePrDt["ProductCode"].ToString() == drPoDt["Product"].ToString()) &
            //                                (drUpdatePrDt["OrderUnit"].ToString() == drPoDt["Unit"].ToString()) &
            //                                (drUpdatePrDt["LocationCode"].ToString() == drPoDt["Location"].ToString()) &
            //                                (drUpdatePrDt["DiscPercent"].ToString() == drPoDt["Discount"].ToString()) &
            //                                (drUpdatePrDt["DiscAmt"].ToString() == drPoDt["DisCountAmt"].ToString()) &
            //                                (drUpdatePrDt["TaxType"].ToString() == drPoDt["TaxType"].ToString()) &
            //                                (drUpdatePrDt["TaxRate"].ToString() == drPoDt["TaxRate"].ToString()) &
            //                                (drUpdatePrDt["Buyer"].ToString() == drPoDt["Buyer"].ToString()) &
            //                                (drUpdatePrDt["TaxAdj"].ToString() == drPoDt["IsAdj"].ToString()) &
            //                                (drUpdatePrDt["DeliPoint"].ToString() == drPoDt["DeliveryPoint"].ToString()) &
            //                                (drUpdatePrDt["Price"].ToString() == drPoDt["Price"].ToString()) &
            //                                (drUpdatePrDt["VendorCode"].ToString() == drPRDtGroup["VendorCode"].ToString()) &
            //                                (drUpdatePrDt["Comment"].ToString() == drPoDt["Comment"].ToString()) &
            //                                dateUpdate.Equals(datePoDt))
            //                            {
            //                                drUpdatePrDt["PONo"] = drPoDt["PoNo"];
            //                                drUpdatePrDt["PODtNo"] = drPoDt["PoDt"];
            //                                //drUpdatePrDt["OrderQty"] = drPoDt["OrdQty"];

            //                                // Update field BuCode follow by PR
            //                                drPoDt["BuCode"] = drUpdatePrDt["BuCode"].ToString();
            //                            }
            //                        }
            //                    }
            //                }

            //                newPoNo = string.Format("{0}{1}", newPoNo.Substring(0, newPoNo.Length - 4),
            //                    (int.Parse(newPoNo.Substring(newPoNo.Length - 4, 4)) + 1).ToString().PadLeft(4, '0'));
            //            }
            //        }

            //        Session["dsTemplate"] = dsPO;

            //        var result = _po.SavePRPO(dsPO, LoginInfo.ConnStr);

            //        if (result)
            //        {
            //            //grd_Template.Selection.UnselectAll();
            //            //pop_Template.ShowOnPageLoad = false;

            //            pop_Success.ShowOnPageLoad = true;

            //            // Add columns for show vendor name and amount.
            //            if (dsPO.Tables[_po.TableName].Rows.Count > 0)
            //            {
            //                var dcNew = new DataColumn("Amount", Type.GetType("System.Int32"));
            //                dsPO.Tables[_po.TableName].Columns.Add(dcNew);

            //                var dc = new DataColumn("VendorName", Type.GetType("System.String"));
            //                dsPO.Tables[_po.TableName].Columns.Add(dc);

            //                for (var w = 0; w < dsPO.Tables[_po.TableName].Rows.Count; w++)
            //                {
            //                    var drPoAdd = dsPO.Tables[_po.TableName].Rows[w];

            //                    drPoAdd["VendorName"] = string.Format("{0} - {1}", drPoAdd["Vendor"],
            //                        _vendor.GetName(drPoAdd["Vendor"].ToString(), LoginInfo.ConnStr));
            //                    drPoAdd["Amount"] = _poDt.GetSumTotalAmt(drPoAdd["PoNo"].ToString(), LoginInfo.ConnStr);
            //                }
            //            }

            //            //grd_Success.DataSource = dsPO.Tables[po.TableName];
            //            //grd_Success.ClientSideEvents.RowDblClick = "function(s, e) { OnGridDoubleClick(e.visibleIndex, 'PoNo'); }";
            //            //grd_Success.DataBind();

            //            grd_Success2.DataSource = dsPO.Tables[_po.TableName];
            //            //grd_Success.ClientSideEvents.RowDblClick = "function(s, e) { OnGridDoubleClick(e.visibleIndex, 'PoNo'); }";
            //            grd_Success2.DataBind();

            //            // Insert signature
            //            foreach (DataRow drGenPO in dsPO.Tables[_po.TableName].Rows)
            //            {
            //                var poInsertSignature = new Blue.BL.PC.PO.PO();
            //                poInsertSignature.InsertSignature(drGenPO["PONo"].ToString(), LoginInfo.ConnStr);
            //            }
            //        }
            //        else
            //        {
            //            //Message Error
            //            pop_Confirm.ShowOnPageLoad = false;
            //            pop_Warning.ShowOnPageLoad = true;
            //            lbl_Warning.Text = @"Cannot save data.";
            //        }
            //    }
            //    else
            //    {
            //        //Message Error
            //        pop_Confirm.ShowOnPageLoad = false;
            //        pop_Warning.ShowOnPageLoad = true;
            //        lbl_Warning.Text = @"Please select data for generate PO.";
            //    }
        }

        protected void GeneratePO()
        {
            SavePO();

            //var selectedItems = grd_PRList2.Rows.Cast<GridViewRow>()
            //    .Where(x => ((CheckBox)x.FindControl("Chk_Item")).Checked)
            //    .Select(x => ((Label)x.FindControl("lbl_RefNo")).Text.Trim())
            //    .ToArray();

            //var currCode = ddl_CurrCode.SelectedValue;
            //var currRate = currency.GetLastCurrencyRate(currCode, DateTime.Now, LoginInfo.ConnStr);



        }


        protected void SavePO()
        {
            var selectedPrNo = string.Empty;
            var prDesc = string.Empty;

            //=== Generate data to Create PO And Pop Up Show PoNo ===
            for (var i = 0; i <= grd_PRList2.Rows.Count - 1; i++)
            {
                var chkItem = (CheckBox)grd_PRList2.Rows[i].FindControl("Chk_Item");
                var lblRefNo = (Label)grd_PRList2.Rows[i].FindControl("lbl_RefNo");
                var lblDescPricelist = (Label)grd_PRList2.Rows[i].FindControl("lbl_DescPricelist");

                if (chkItem.Checked)
                {
                    prDesc = lblDescPricelist.Text;

                    if (selectedPrNo != string.Empty)
                        selectedPrNo += string.Format(", '{0}'", lblRefNo.Text);
                    else
                        selectedPrNo = string.Format("'{0}'", lblRefNo.Text);
                }
            }

            if (selectedPrNo != string.Empty)
            {
                string errorMessage = string.Empty;

                // Check if selected PR are not using

                string sql = string.Format("SELECT COUNT(*) as RecordCount FROM PC.PrDt WHERE PrNo IN ({0}) AND ISNULL(PoNo, '') <> '' AND CurrencyCode = '{1}' ", selectedPrNo, ddl_CurrCode.SelectedValue.ToString());
                DataTable dt = _po.DbExecuteQuery(sql, null, LoginInfo.ConnStr);


                DataTable dtGenPO = new DataTable();

                if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                    dtGenPO = GenPOFromPR(selectedPrNo, prDesc, LoginInfo.LoginName, LoginInfo.ConnStr, ref errorMessage);
                else
                    dtGenPO = null;

                if (dtGenPO == null)
                {
                    //Message Error
                    pop_Confirm.ShowOnPageLoad = false;
                    pop_Warning.ShowOnPageLoad = true;
                    //lbl_Warning.Text = @"Cannot save data.";

                    // Modifeid on: 23/01/2018, By: Fon
                    lbl_Warning.Text = "Error! process was interrupted (" + errorMessage + ")";
                    // End Modified.
                }
                else
                {
                    pop_Success.ShowOnPageLoad = true;

                    // Add columns for show vendor name and amount.
                    if (dtGenPO.Rows.Count > 0)
                    {
                        var dcNew = new DataColumn("Amount", Type.GetType("System.Int32"));
                        dtGenPO.Columns.Add(dcNew);

                        var dc = new DataColumn("VendorName", Type.GetType("System.String"));
                        dtGenPO.Columns.Add(dc);

                        for (var w = 0; w < dtGenPO.Rows.Count; w++)
                        {
                            var drPoAdd = dtGenPO.Rows[w];

                            drPoAdd["VendorName"] = string.Format("{0} - {1}", drPoAdd["Vendor"],
                                _vendor.GetName(drPoAdd["Vendor"].ToString(), LoginInfo.ConnStr));
                            drPoAdd["Amount"] = _poDt.GetSumTotalAmt(drPoAdd["PoNo"].ToString(), LoginInfo.ConnStr);
                        }

                        // Added on: 25/09/2017, By: Fon
                        ClassLogTool pctool = new ClassLogTool();
                        pctool.SaveActionLog("PO", dtGenPO.Rows[0]["PoNo"].ToString(), "Create");
                        // End Added.
                    }

                    grd_Success2.DataSource = dtGenPO;
                    grd_Success2.DataBind();

                    // Insert signature
                    foreach (DataRow drGenPO in dtGenPO.Rows)
                    {
                        var poInsertSignature = new Blue.BL.PC.PO.PO();
                        poInsertSignature.InsertSignature(drGenPO["PONo"].ToString(), LoginInfo.ConnStr);
                    }
                }
            }
            else
            {
                //Message Error
                pop_Confirm.ShowOnPageLoad = false;
                pop_Warning.ShowOnPageLoad = true;
                lbl_Warning.Text = @"Please select data for generate PO.";
            }
        }

        private DataTable GenPOFromPR(string prList, string strDesc, string loginName, string connStr, ref string errorMessage)
        {
            var currCode = ddl_CurrCode.SelectedValue;
            var currRate = currency.GetLastCurrencyRate(currCode, DateTime.Now, LoginInfo.ConnStr);

            errorMessage = string.Empty;

            // Modified on: 11/08/2017, By: Fon

            DataTable dtPrDt = new DataTable();

            try
            {
                // Get all pr detail which are ready ot generate po
                string cmdPrDt = string.Format(
                    @"SELECT PrDt.* 
                      FROM PC.PrDt 
                      LEFT JOIN PC.Pr ON Pr.PRNo = PrDt.PRNo 
                      WHERE PrDt.PRNo IN ({0})
                        AND (CHARINDEX('R', PrDt.ApprStatus) = 0)
                        AND (CHARINDEX('_', PrDt.ApprStatus) = 0)
                        AND CurrencyCode = '{1}'
                      ORDER BY PrDt.VendorCode, PrDt.ReqDate,PrDt.BuCode, PrDt.LocationCode, PrDt.DeliPoint, PrDt.ProductCode"
                    , prList, currCode);

                dtPrDt = _dbHandler.DbExecuteQuery(cmdPrDt, null, connStr);

                #region dtPrDt is not null
                if (dtPrDt != null)
                {
                    //Modified: 15/08/2017, By:Fon
                    #region List of all PrDt

                    if (dtPrDt.Rows.Count > 0)
                    {
                        var cmdQuery = new ArrayList();
                        var cmdParam = new ArrayList();

                        // Generate command to update PrDt
                        // Initial PoCode and PoDtNo for the first PrDt
                        var poNo = _po.GetNewID(ServerDateTime, LoginInfo.ConnStr);
                        var poDtNo = 1;

                        //SortedList<string, int> poPair = new SortedList<string, int>();

                        List<List<string>> po_poDt = new List<List<string>>();

                        #region cmd UpDate PrDt
                        try
                        {
                            for (var i = 0; i < dtPrDt.Rows.Count; i++)
                            {
                                var cmdUpdPrDt = @" UPDATE  PC.PrDt 
                                                    SET     PONo = @PoNo, 
                                                            PoDtNo = @PoDtNo 
                                                    WHERE   PRNo = @PrNo 
                                                            AND PrDtNo = @PrDtNo 
                                                            AND PrDt.ApprStatus not LIKE '%R%' 
                                                            AND CHARINDEX('_', PrDt.ApprStatus) = 0 ";
                                var dbParamsUpdPrDt = new Blue.DAL.DbParameter[4];

                                // Define Parameter
                                if (i == 0)
                                {
                                    // For the first PrDt use generate po from database                        
                                    dbParamsUpdPrDt[0] = new Blue.DAL.DbParameter("@PoNo", poNo);
                                    dbParamsUpdPrDt[1] = new Blue.DAL.DbParameter("@PoDtNo", poDtNo.ToString());
                                }
                                else
                                #region Row > 0
                                {
                                    //2014-08-15 Add PR Comment Condition for seperat PO 
                                    //Compare to all previous row with the conditions which use for grouping multiple pr to a po 

                                    if (dtPrDt.Rows[i]["VendorCode"].Equals(dtPrDt.Rows[i - 1]["VendorCode"]) &&
                                        dtPrDt.Rows[i]["ReqDate"].Equals(dtPrDt.Rows[i - 1]["ReqDate"]))
                                    {
                                        // Group to previous po
                                        // Do not change PoCode
                                        if (dtPrDt.Rows[i]["BuCode"].Equals(dtPrDt.Rows[i - 1]["BuCode"]) &&
                                            dtPrDt.Rows[i]["LocationCode"].Equals(dtPrDt.Rows[i - 1]["LocationCode"]) &&
                                            dtPrDt.Rows[i]["DeliPoint"].Equals(dtPrDt.Rows[i - 1]["DeliPoint"]) &&
                                            dtPrDt.Rows[i]["ProductCode"].Equals(dtPrDt.Rows[i - 1]["ProductCode"]) &&
                                            dtPrDt.Rows[i]["OrderUnit"].Equals(dtPrDt.Rows[i - 1]["OrderUnit"]) &&
                                            dtPrDt.Rows[i]["Price"].Equals(dtPrDt.Rows[i - 1]["Price"]) &&
                                            dtPrDt.Rows[i]["DiscPercent"].Equals(dtPrDt.Rows[i - 1]["DiscPercent"]) &&
                                            dtPrDt.Rows[i]["TaxType"].Equals(dtPrDt.Rows[i - 1]["TaxType"]) &&
                                            dtPrDt.Rows[i]["TaxRate"].Equals(dtPrDt.Rows[i - 1]["TaxRate"]) &&
                                            dtPrDt.Rows[i]["Comment"].Equals(dtPrDt.Rows[i - 1]["Comment"]))
                                        {
                                            // Group to previous po detail
                                            // Do not increase PoDtNo
                                        }
                                        else
                                        {
                                            // Group to previous new po detail
                                            // Increase PoDtNo by 1.
                                            poDtNo += 1;
                                        }
                                    }
                                    else
                                    {
                                        // Group to new po
                                        poNo = poNo.Substring(0, poNo.Length - 4) + (Convert.ToInt32(poNo.Substring(poNo.Length - 4, 4)) + 1).ToString().PadLeft(4, '0');
                                        poDtNo = 1;
                                    }


                                    dbParamsUpdPrDt[0] = new Blue.DAL.DbParameter("@PoNo", poNo);
                                    dbParamsUpdPrDt[1] = new Blue.DAL.DbParameter("@PoDtNo", poDtNo.ToString());
                                }
                                #endregion

                                dbParamsUpdPrDt[2] = new Blue.DAL.DbParameter("@PrNo", dtPrDt.Rows[i]["PrNo"].ToString());
                                dbParamsUpdPrDt[3] = new Blue.DAL.DbParameter("@PrDtNo", dtPrDt.Rows[i]["PrDtNo"].ToString());

                                // Modified on: 17/08/2017, By: Fon
                                List<string> poPair = new List<string>();
                                poPair.Add(poNo);
                                poPair.Add(poDtNo.ToString());
                                po_poDt.Add(poPair);
                                // End modified.

                                cmdQuery.Add(cmdUpdPrDt);
                                cmdParam.Add(dbParamsUpdPrDt);

                            }

                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message.ToString();
                            return null;
                        }
                        #endregion

                        // Commit PR
                        var result1 = _dbHandler.DbCommit(cmdQuery, cmdParam, connStr);

                        cmdQuery.Clear();
                        cmdParam.Clear();

                        // Generate Po from selected Pr
                        // Po Header
                        #region cmd Insert PO
                        string defaultCurrency = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);

                        var cmdInsPoHdr = string.Format(
                            @"  INSERT INTO [PC].[Po] ( [PoNo], [PoDate], [Description], [DeliDate], [Vendor], [Buyer], [Currency], [ExchageRate],
                                                        [CreditTerm], [DocStatus], [IsVoid], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy],
                                                        [CurrencyCode], [CurrencyRate] ) 
                                                                            
                                SELECT 
                                    DISTINCT(prdt.PoNo), 
                                    GETDATE() AS [PoDate], 
                                    (SELECT TOP(1) h.[Description] FROM PC.PrDt d JOIN PC.Pr h on h.PRNo = d.PRNo WHERE d.PONo = prdt.PONo) AS [Description], 
                                    prdt.ReqDate, 
                                    prdt.VendorCode, 
                                    NULL AS [Buyer], 
                                    '{1}' as Currency,
                                    Ref.GetLastCurrencyRate( GETDATE(), prdt.CurrencyCode) as [ExchageRate],
                                    Vendor.CreditTerm, 
                                    'Approved' AS DocStatus, 
                                    'False' AS [IsVoid], 
                                    GETDATE() AS [CreatedDate], 
                                    @LoginName AS [CreatedBy], 
                                    GETDATE() AS [UpdatedDate], 
                                    @LoginName AS [UpdatedBy], 
                                    '{1}' as CurrencyCode, 
                                    Ref.GetLastCurrencyRate( prdt.ReqDate, prdt.CurrencyCode) AS [CurrencyRate]
                                FROM PC.PrDt prdt 
                                LEFT JOIN PC.Pr ON (Pr.PRNo = PrDt.PRNo) 
                                LEFT JOIN AP.Vendor ON (Vendor.VendorCode = PrDt.VendorCode) 
                                WHERE prdt.PRNo IN ({0})
                                    AND prdt.CurrencyCode = '{1}'
                                    AND (CHARINDEX('R', PrDt.ApprStatus) = 0 )
                                    AND (CHARINDEX('_', PrDt.ApprStatus) = 0 )"
                                , prList, currCode); // End Modified.

                        var dbParamsInsPoHdr = new Blue.DAL.DbParameter[1];
                        dbParamsInsPoHdr[0] = new Blue.DAL.DbParameter("@LoginName", loginName);

                        cmdQuery.Add(cmdInsPoHdr);
                        cmdParam.Add(dbParamsInsPoHdr);
                        #endregion

                        string cmdInsPoDt = string.Format(
                                            @"  DECLARE @DigitAmt INT = APP.DigitAmt()

                                                INSERT INTO [PC].[PoDt] (
                                                      [PoNo], [PoDt], [BuCode], [Location], [Product]
                                                    , [Descen], [Descll], [DeliveryPoint], [OrdQty], [Unit]
                                                    , [FOCQty], [RcvQty], [CancelQty], [Price], [Discount]
                                                    , [DiscountAmt] 
                                                    , [TaxType], [TaxRate], [IsAdj]
                                                    , [NetAmt]
                                                    , [TaxAmt]
                                                    , [TotalAmt]
                                                    , [CurrNetAmt], [CurrDiscAmt], [CurrTaxAmt], [CurrTotalAmt] 
                                                    , [Buyer]
                                                    , [Comment]
                                                )
                                                SELECT  prdt.PoNo, prdt.PoDtNo, prdt.BuCode, prdt.LocationCode, prdt.ProductCode 
                                                        , prdt.Descen, prdt.Descll, prdt.DeliPoint, SUM(prdt.ApprQty) AS OrdQty, prdt.OrderUnit 
                                                        , SUM(prdt.FOCQty) AS FOCQty, 0 AS RcvQty, 0 AS CancelQty, prdt.Price, prdt.DiscPercent
                                                        , SUM(ROUND( prdt.DiscAmt * Ref.GetLastCurrencyRate( prdt.ReqDate, prdt.CurrencyCode), @DigitAmt) ) AS DiscAmt
                                                        , prdt.TaxType, prdt.TaxRate, 'false' AS IsAdj
                                                        , SUM(ROUND( prdt.CurrNetAmt * Ref.GetLastCurrencyRate( prdt.ReqDate, prdt.CurrencyCode), @DigitAmt)) AS NetAmt
                                                        , SUM(ROUND( prdt.CurrTaxAmt * Ref.GetLastCurrencyRate( prdt.ReqDate, prdt.CurrencyCode), @DigitAmt)) AS TaxAmt
                                                        , SUM(ROUND( prdt.CurrTotalAmt * Ref.GetLastCurrencyRate( prdt.ReqDate, prdt.CurrencyCode), @DigitAmt)) AS TotalAmt
                                                        , SUM(prdt.CurrNetAmt) AS CurrNetAmt, SUM(prdt.CurrDiscAmt) AS CurrDiscAmt, SUM(prdt.CurrTaxAmt) AS CurrTaxAmt, SUM(prdt.CurrTotalAmt) AS CurrTotalAmt
                                                        , NULL AS Buyer
                                                        , prdt.Comment
                                                        -- , CAST(STUFF((  SELECT CASE WHEN t.Comment IS NOT NULL AND RTRIM(LTRIM(t.Comment)) <> '' THEN '; ' + t.Comment ELSE '' END 
                                                        --                FROM PC.PrDt t WHERE t.VendorCode = prdt.VendorCode AND t.ReqDate = prdt.ReqDate AND t.BuCode = prdt.BuCode 
                                                        --                AND t.LocationCode = prdt.LocationCode AND t.DeliPoint = prdt.DeliPoint AND t.ProductCode = prdt.ProductCode 
                                                        --                AND t.OrderUnit = prdt.OrderUnit AND t.Price = prdt.Price AND t.DiscPercent = prdt.DiscPercent 
                                                        --                AND t.TaxType = prdt.TaxType AND t.TaxRate = prdt.TaxRate FOR XML PATH ('')), 1, 2, '') AS NVARCHAR(300)) AS Comment 
                                                    
                                                FROM [PC].PrDt 
                                                WHERE prdt.PRNo IN ({0}) 
                                                    AND (CHARINDEX('R', prdt.ApprStatus) = 0 )
                                                    AND (CHARINDEX('_', prdt.ApprStatus) = 0 ) 
                                                    AND [CurrencyCode] = @CurrCode                       
                                                GROUP BY    prdt.VendorCode, prdt.PoNo, prdt.PoDtNo, prdt.BuCode, prdt.LocationCode, prdt.ProductCode, prdt.Descen, 
                                                            prdt.Descll, prdt.ReqDate, prdt.DeliPoint, prdt.OrderUnit, prdt.Price, prdt.DiscPercent, prdt.TaxType, prdt.TaxRate, prdt.Comment ", prList);


                        var dbParamsInsPoDt = new Blue.DAL.DbParameter[1];
                        dbParamsInsPoDt[0] = new Blue.DAL.DbParameter("@CurrCode", currCode);

                        cmdQuery.Add(cmdInsPoDt);
                        cmdParam.Add(dbParamsInsPoDt);

                        var result = _dbHandler.DbCommit(cmdQuery, cmdParam, connStr);

                        if (result)
                        {
                            // Modified on: 15/08/2017, By: Fon

                            var cmdSelPoHdr = string.Format(
                                @"  SELECT h.[PoNo], h.[PoDate], h.[Description], h.[Vendor], h.[Currency], h.[Buyer], h.[ExchageRate]
                                            , h.[CreditTerm], h.[DocStatus], h.[ApprStatus], h.[IsVoid], h.[CreatedDate], h.[CreatedBy]
                                            , h.[UpdatedDate], h.[UpdatedBy]
                                            , SUM(d.NetAmt) AS NetAmt, SUM(d.TaxAmt) AS TaxAmt, SUM(d.TotalAmt) AS TotalAmt 
                                            , h.CurrencyCode, h.CurrencyRate
                                            , SUM(d.CurrNetAmt) AS CurrNetAmt, SUM(d.CurrTaxAmt) AS CurrTaxAmt, SUM(d.CurrTotalAmt) AS  CurrTotalAmt
                                            , a.Address6 as VendorEmail
            
                                    FROM PC.Po h 
                                    LEFT JOIN PC.PoDt d ON (d.PoNo = h.PoNo)
                                    LEFT JOIN AP.Vendor v ON v.VendorCode = h.Vendor
                                    LEFT JOIN [Profile].[Address] a ON a.ProfileCode = v.ProfileCode

                                    WHERE h.PoNo IN (SELECT DISTINCT(PoNo) FROM PC.PrDt WHERE PRNo IN ({0})) 
                                      AND h.CurrencyCode = '{1}'
                                    GROUP BY h.[PoNo], h.[PoDate], h.[Description], h.[Vendor], h.[Currency], h.[Buyer], h.[ExchageRate], h.[CreditTerm]
                                            , h.[DocStatus], h.[ApprStatus], h.[IsVoid], h.[CreatedDate], h.[CreatedBy], h.[UpdatedDate], h.[UpdatedBy]
                                            , h.[CurrencyCode], h.[CurrencyRate]
                                            , a.Address6"
                                , prList, currCode);
                            // End Modified.

                            var dt = _dbHandler.DbExecuteQuery(cmdSelPoHdr, null, connStr);

                            foreach (DataRow dr in dt.Rows)
                            {
                                _transLog.Save("PC", "PO", dr["PoNo"].ToString(), "CREATE", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);
                            }



                            return dt;
                        }
                        else
                        {
                            errorMessage = "#002: Error for inserting to PO";
                            return null;
                        }
                    }
                    #endregion
                    else
                    {
                        // For: Filter currency Code that return 0 records.
                        errorMessage = "#003: Not found any PR.";
                    }
                }
                else
                {
                    errorMessage = "#001: Invalid query (Select PR).";
                }

                #endregion
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return null;
        }

        #endregion

        private void ShowWarning(string text)
        {
            lbl_Warning.Text = text;
            pop_Warning.ShowOnPageLoad = true;
        }
    }
}