using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace BlueLedger.PL.IN.STOREREQ
{
    public partial class StoreReqEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.IN.storeRequisition ReqH = new Blue.BL.IN.storeRequisition();
        private readonly Blue.BL.IN.StoreRequisitionDetail reqDt = new Blue.BL.IN.StoreRequisitionDetail();

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();

        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.Import.JobCode jobCode = new Blue.BL.Import.JobCode();

        private readonly Blue.BL.APP.Config conf = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.APP.WF workflow = new Blue.BL.APP.WF();

        private DataSet _dsReqEdit = new DataSet();
        private DataSet dsJobCode = new DataSet();


        private string StoreReqEditMode
        {
            get { return Session["StoreReqEditMode"].ToString(); }
            set { Session["StoreReqEditMode"] = value; }
        }

        private int WfStep
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFStep(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr);
                }

                var httpCookie = Request.Cookies["[IN].[vStoreRequisition]"];
                return (httpCookie == null) ? 0 : viewHandler.GetWFStep(int.Parse(httpCookie.Value), LoginInfo.ConnStr);
            }
        }

        private bool IsEditMode
        {
            get
            {
                return Request.Params["MODE"].ToUpper() == "EDIT";
            }
        }

        private int WfStepCount
        {
            get
            {
                DataSet ds = new DataSet();
                workflow.Get(ds, "IN", "SR", LoginInfo.ConnStr);

                return Convert.ToInt32(ds.Tables[0].Rows[0]["StepNo"]);
            }
        }

        // Added on: 13/11/2017, By: Fon
        private string IsSingleLocation
        {
            get
            {
                return conf.GetValue("IN", "SR", "SingleLocation", LoginInfo.ConnStr);
            }
        }

        private string LimitDetail
        {
            get
            {
                return conf.GetValue("IN", "SR", "LimitDetail", LoginInfo.ConnStr);
            }
        }

        //// Added on: 22/02/2018
        private string action
        {
            get
            {
                return Request.QueryString["ACTION"];
            }
        }

        private int wfID
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFId(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr);
                }

                var httpCookie = Request.Cookies["[IN].[vStoreRequisition]"];
                return (httpCookie == null) ? 0 : viewHandler.GetWFId(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr);
            }
        }
        // End Aded.

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                _dsReqEdit = (DataSet)Session["dsStoreReqEdit"];

            }

        }

        private void Page_Retrieve()
        {
            switch (Request.Params["MODE"].ToUpper())
            {
                case "NEW":
                    ReqH.GetStructure(_dsReqEdit, LoginInfo.ConnStr);
                    reqDt.GetStructure(_dsReqEdit, LoginInfo.ConnStr);
                    break;
                case "EDIT":
                    ReqH.GetListById(_dsReqEdit, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
                    reqDt.GetListByHeaderId(_dsReqEdit, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
                    break;
                case "SR":
                    _dsReqEdit = (DataSet)Session["dsStoreReqDt"];
                    break;
            }

            // Modified on: 06/11/2017, By: Fon
            //locat.GetList(_dsReqEdit, LoginInfo.ConnStr);

            locat.GetList(_dsReqEdit, LoginInfo.LoginName, LoginInfo.ConnStr);
            // End Modified.

            Session["dsStoreReqEdit"] = _dsReqEdit;
            Page_Setting();
        }

        private void Page_Setting()
        {
            // Display current process description
            var cookie = Request.Cookies["[IN].[vStoreRequisition]"];
            //if ((v != null) && (v.Value != string.Empty))
            if (!String.IsNullOrEmpty(cookie.ToString()))
            {
                lbl_Process.Text = viewHandler.GetDesc(int.Parse(cookie.Value), LoginInfo.ConnStr);
            }

            DataRow drStoreReq;
            switch (Request.Params["MODE"].ToUpper())
            {
                case "NEW":
                    //de_Date.Text = ServerDateTime.ToShortDateString();
                    de_Date.Value = ServerDateTime.Date;
                    de_ReqDate.Date = ServerDateTime.Date.AddDays(1);
                    td_ProcessStatus.Visible = false;
                    td_Delete.Visible = false;
                    ddl_JobCode.Value = jobCode.GetSingleJobCode(LoginInfo.ConnStr);
                    break;

                case "EDIT":
                    drStoreReq = _dsReqEdit.Tables[ReqH.TableName].Rows[0];

                    ddl_Store.DataSource = _dsReqEdit.Tables[locat.TableName];
                    ddl_Store.ValueField = "LocationCode";
                    ddl_Store.Value = drStoreReq["LocationCode"].ToString();
                    ddl_Store.DataBind();

                    //ddl_Type.Value = drStoreReq["AdjId"].ToString();
                    ddl_Type.Value = Convert.ToInt32(drStoreReq["AdjId"]);

                    txt_Desc.Text = drStoreReq["Description"].ToString();
                    de_ReqDate.Date = ServerDateTime.AddDays(1);
                    //de_Date.Text = DateTime.Parse(drStoreReq["CreateDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                    de_Date.Value = Convert.ToDateTime(drStoreReq["DeliveryDate"].ToString());
                    lbl_Ref.Text = drStoreReq["RequestCode"].ToString();
                    lbl_Status.Text = drStoreReq["DocStatus"].ToString();
                    ProcessStatus.ApprStatus = drStoreReq["ApprStatus"].ToString();
                    ddl_JobCode.Value = drStoreReq["ProjectRef"].ToString();
                    break;

                case "SR":
                    //de_Date.Text = ServerDateTime.ToShortDateString();
                    de_Date.Value = ServerDateTime.Date;
                    txt_Desc.Text = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["Description"].ToString();
                    de_ReqDate.Date = ServerDateTime.AddDays(1);

                    //td_Ref.Visible = false;
                    //td_RefName.Visible = false;
                    //td_Process.Visible = false;
                    td_ProcessStatus.Visible = false;

                    drStoreReq = _dsReqEdit.Tables[ReqH.TableName].Rows[0];
                    ddl_Type.Value = drStoreReq["AdjId"].ToString();

                    ddl_Store.DataSource = _dsReqEdit.Tables[locat.TableName];
                    ddl_Store.ValueField = "LocationCode";
                    ddl_Store.DataBind();
                    ddl_Store.Value = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["LocationCode"].ToString();

                    break;
            }

            //if (Request.Params["MODE"].ToUpper() == "EDIT" &&
            //    (viewHandler.GetDesc(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr).ToUpper() == "APPROVE" ||
            //     viewHandler.GetDesc(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr).ToUpper() == "ISSUE"))

            if (Request.Params["MODE"].ToUpper() == "EDIT"
                && (viewHandler.GetWFStep(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr) > 1))
            {
                grd_StoreReqEdit.Visible = false;
                grd_StoreReqAppr.Visible = true;
                //btn_Create.Visible = false;

                grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                grd_StoreReqAppr.DataBind();
            }
            else
            {
                grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                grd_StoreReqEdit.DataBind();
            }

            if (grd_StoreReqEdit.Rows.Count > 0)
            {
                ddl_Store.Enabled = false;
                // ddl_Type.Enabled = false; 
            }

            //txt_Desc.Text = string.Format("Limit: {0}", LimitDetail);

            // Added on: 22/02/2018, By: Fon, For: Following from P'Oat request.
            btn_Commit.Visible = (workFlowDt.GetAllowCreate(wfID, WfStep, LoginInfo.ConnStr)) ? true : false;
            // End Added.

            if (workFlowDt.GetAllowCreate(wfID, WfStep, LoginInfo.ConnStr)) // if create step
            {
                btn_Delete.Visible = true;
                btn_Create.Visible = true;
            }
            else // if not create step
            {

                de_Date.Enabled = false;
                ddl_Store.Enabled = false;
                ddl_Type.Enabled = false;
                ddl_JobCode.Enabled = false;
                txt_Desc.Enabled = false;

                btn_Delete.Visible = false;
                btn_Create.Visible = false;

            }
        }

        protected void grd_StoreReqEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (StoreReqEditMode.ToUpper() == "NEW")
                {
                    _dsReqEdit.Tables[reqDt.TableName].Rows[_dsReqEdit.Tables[reqDt.TableName].Rows.Count - 1].Delete();
                }

                if (StoreReqEditMode.ToUpper() == "EDIT")
                {
                    _dsReqEdit.Tables[reqDt.TableName].Rows[_dsReqEdit.Tables[reqDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (StoreReqEditMode.ToUpper() == "NEW")
                {
                    _dsReqEdit.Tables[reqDt.TableName].Rows[_dsReqEdit.Tables[reqDt.TableName].Rows.Count - 1].Delete();
                }

                if (StoreReqEditMode.ToUpper() == "EDIT")
                {
                    _dsReqEdit.Tables[reqDt.TableName].Rows[_dsReqEdit.Tables[reqDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.EditIndex = -1;
            grd_StoreReqEdit.DataBind();

            if (grd_StoreReqEdit.Rows.Count == 0)
            {
                ddl_Store.Enabled = true;
                ddl_Type.Enabled = true;

            }
            // End Modified.

            de_Date.Enabled = true;

            btn_Save.Visible = true;
            btn_Back.Visible = true;
            btn_Commit.Visible = true;

            de_ReqDate.Visible = true;
            btn_ReqDate_Ok.Visible = true;

            StoreReqEditMode = string.Empty;
        }

        protected void grd_StoreReqEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.EditIndex = e.NewEditIndex;
            grd_StoreReqEdit.DataBind();

            for (var i = grd_StoreReqEdit.Rows.Count - 1; i >= 0; i--)
            {
                var chkItem = grd_StoreReqEdit.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                var chkAll = grd_StoreReqEdit.HeaderRow.FindControl("Chk_All") as CheckBox;

                if (chkAll != null) chkAll.Enabled = false;
                if (chkItem != null) chkItem.Enabled = false;
            }

            ddl_Store.Enabled = false;
            // ddl_Type.Enabled = false;
            de_Date.Enabled = false;

            //hong visible button 20130909
            btn_Save.Visible = false;
            btn_Back.Visible = false;

            //btn_Create.Visible = false;
            //btn_Delete.Visible = false;

            de_ReqDate.Visible = false;
            btn_ReqDate_Ok.Visible = false;

            StoreReqEditMode = "Edit";
        }

        protected void grd_StoreReqEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            return;

            //ASPxPageControl tp_Information  = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var ddlGStore = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;
            var ddlProduct = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddlDebit = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddlCredit = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            //ASPxTextBox txt_QtyRequested    = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_QtyRequested") as ASPxTextBox;
            var txtQtyRequested = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_QtyRequested") as ASPxSpinEdit;
            var txtComment = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_Comment") as TextBox;
            var deGReqDate = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;
            var lblUnit = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("lbl_Unit") as Label;


            //Check field cannot empty
            if (ddlGStore != null)
            {
                if (ddlGStore.Value != null) return;
                lbl_Warning.Text = @"Please select 'Request From'";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(250);
                return;
            }

            if (ddlProduct != null)
            {
                if (ddlProduct.Value != null) return;
                lbl_Warning.Text = @"Please select 'SKU #'";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(200);
                return;
            }


            if (txtQtyRequested != null)
            {
                decimal number;

                if (txtQtyRequested.Text == string.Empty)
                {
                    //Please entry 'Qty Requested'
                    lbl_Warning.Text = @"Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
                else if (!decimal.TryParse(txtQtyRequested.Text, out number) || decimal.Parse(txtQtyRequested.Text) <= 0)
                {
                    //Please entry 'Qty Requested' only 1-9
                    //check number
                    lbl_Warning.Text = @"Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
            }

            //Update Date
            string productCode = ddlProduct.Value.ToString().Split(' ')[0];

            var drUpdating = _dsReqEdit.Tables[reqDt.TableName].Rows[grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].DataItemIndex];
            drUpdating["ToLocationCode"] = ddlGStore.Value;
            drUpdating["DeliveryDate"] = deGReqDate.Date;
            //drUpdating["CategoryCode"] = product.GetProductCategory(ddlProduct.Value.ToString(), LoginInfo.ConnStr).ToString();
            //drUpdating["ProductCode"] = ddlProduct.Value.ToString();
            drUpdating["CategoryCode"] = product.GetProductCategory(productCode, LoginInfo.ConnStr).ToString();
            drUpdating["ProductCode"] = productCode;

            drUpdating["RequestQty"] = RoundQty(decimal.Parse(txtQtyRequested.Text));
            drUpdating["ApprQty"] = RoundQty(decimal.Parse(txtQtyRequested.Text));
            //drUpdating["AllocateQty"] = RoundQty(decimal.Parse(txtQtyRequested.Text));
            drUpdating["RequestUnit"] = lblUnit.Text;
            drUpdating["DebitACCode"] = null;
            drUpdating["CreditACCode"] = null;
            drUpdating["Comment"] = txtComment.Text == string.Empty ? null : txtComment.Text;
            drUpdating["ApprStatus"] = workflow.GetDtApprStatus("IN", "SR", LoginInfo.ConnStr);

            // Modified on: 14/11/2017, By: Fon
            //grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            //grd_StoreReqEdit.EditIndex = -1;
            //grd_StoreReqEdit.DataBind();

            Control_Location(ddlGStore.Value.ToString(), _dsReqEdit.Tables[reqDt.TableName]);
            // End Modified.

            if (Request.Params["MODE"].ToUpper() == "EDIT" && StoreReqEditMode.ToUpper() == "NEW")
            {
                drUpdating["DocumentId"] = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RefId"].ToString();
            }

            de_Date.Enabled = false;
            //btn_Save.Enabled = true;
            //btn_Create.Enabled = true;

            StoreReqEditMode = string.Empty;
            td_Delete.Visible = true;

            //hong visible button 20130909
            btn_Save.Visible = true;
            btn_Back.Visible = true;

            //btn_Create.Visible = true;
            //btn_Delete.Visible = true;

            de_ReqDate.Visible = true;
            btn_ReqDate_Ok.Visible = true;
        }

        protected void grd_StoreReqEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW" || e.CommandName.ToUpper() == "UPDATE")
            {
                #region SaveNew || Update
                var ddlGStore =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;
                //if (ddlGStore == null) throw new NullReferenceException("ddl_gStore");

                var ddlProduct =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
                //if (ddlProduct == null) throw new NullReferenceException("ddl_Product");

                var ddlDebit =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
                //if (ddlDebit == null) throw new NullReferenceException("ddl_Debit");

                var ddlCredit =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
                //if (ddlCredit == null) throw new NullReferenceException("ddl_Credit");

                var txtQtyRequested =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_QtyRequested") as ASPxSpinEdit;
                //if (txtQtyRequested == null) throw new NullReferenceException("txt_QtyRequested");

                var txtComment = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_Comment") as TextBox;
                //if (txtComment == null) throw new NullReferenceException("txt_Comment");

                var deGReqDate =
                    grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;
                //if (deGReqDate == null) throw new NullReferenceException("de_gReqDate");

                var lblUnit = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("lbl_Unit") as Label;
                //if (lblUnit == null) throw new NullReferenceException("lbl_Unit");


                //Check field cannot empty
                if (ddlGStore == null || ddlGStore.Value == null)
                {
                    lbl_Warning.Text = @"Please select 'Store Name'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

                if (ddlProduct == null || ddlProduct.Value == null)
                {
                    lbl_Warning.Text = @"Please select 'SKU #'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(200);
                    return;
                }


                decimal number;

                if (txtQtyRequested == null || txtQtyRequested.Text == string.Empty)
                {
                    //Please entry 'Qty Requested'
                    lbl_Warning.Text = @"Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

                if (!decimal.TryParse(txtQtyRequested.Text, out number) || decimal.Parse(txtQtyRequested.Text) <= 0)
                {
                    //Please entry 'Qty Requested' only 1-9
                    //check number
                    lbl_Warning.Text = @"Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }


                if (deGReqDate == null || deGReqDate.Value == null)
                {
                    lbl_Warning.Text = @"Please select 'Request Date'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(200);
                    return;
                }

                //Update Date
                string productCode = ddlProduct.Value.ToString().Split(' ')[0];
                var drUpdating =
                    _dsReqEdit.Tables[reqDt.TableName].Rows[
                        grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].DataItemIndex];
                drUpdating["ToLocationCode"] = ddlGStore.Value;
                drUpdating["DeliveryDate"] = deGReqDate.Date;
                //drUpdating["CategoryCode"] = product.GetProductCategory(ddlProduct.Value.ToString(), LoginInfo.ConnStr);
                //drUpdating["ProductCode"] = ddlProduct.Value.ToString().Split(' ')[0];
                drUpdating["CategoryCode"] = product.GetProductCategory(productCode, LoginInfo.ConnStr);
                drUpdating["ProductCode"] = productCode;
                drUpdating["RequestQty"] = decimal.Parse(txtQtyRequested.Text);
                drUpdating["ApprQty"] = decimal.Parse(txtQtyRequested.Text);
                if (lblUnit != null) drUpdating["RequestUnit"] = lblUnit.Text;
                drUpdating["DebitACCode"] = null;
                drUpdating["CreditACCode"] = null;
                if (txtComment != null)
                    drUpdating["Comment"] = txtComment.Text == string.Empty ? null : txtComment.Text;
                drUpdating["ApprStatus"] = workflow.GetDtApprStatus("IN", "SR", LoginInfo.ConnStr);

                // Modified on: 14/11/2017, By: Fon
                //grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                //grd_StoreReqEdit.EditIndex = -1;
                //grd_StoreReqEdit.DataBind();

                bool returnC = Control_Location(ddlGStore.Value.ToString(), _dsReqEdit.Tables[reqDt.TableName]);
                // End Modified.

                if (Request.Params["MODE"].ToUpper() == "EDIT" && StoreReqEditMode.ToUpper() == "NEW")
                {
                    drUpdating["DocumentId"] = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RefId"].ToString();
                }

                btn_Save.Enabled = true;
                StoreReqEditMode = string.Empty;
                td_Delete.Visible = true;

                if (e.CommandName.ToUpper() == "SAVENEW")
                {
                    // Modified on: 15/11/2017
                    //Create();

                    if (returnC) Create();
                    // End Modified.
                }
                else
                {
                    de_Date.Enabled = false;
                    //btn_Save.Enabled = true;
                    //btn_Create.Enabled = true;
                    StoreReqEditMode = string.Empty;
                    td_Delete.Visible = true;

                    //hong visible button 20130909
                    btn_Save.Visible = true;
                    btn_Back.Visible = true;
                    btn_Commit.Visible = true;

                    //btn_Create.Visible = true;
                    //btn_Delete.Visible = true;

                    de_ReqDate.Visible = true;
                    btn_ReqDate_Ok.Visible = true;
                }
                #endregion
            }
        }

        protected void grd_StoreReqEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (_dsReqEdit.Tables[reqDt.TableName].Rows.Count > 0)
            {
                for (var i = 0; i < _dsReqEdit.Tables[reqDt.TableName].Rows.Count; i++)
                {
                    var drDelete = _dsReqEdit.Tables[reqDt.TableName].Rows[i];
                    if (drDelete.RowState == DataRowState.Deleted) continue;
                    if (i != e.RowIndex) continue;
                    drDelete.Delete();
                }
            }

            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.EditIndex = -1;
            grd_StoreReqEdit.DataBind();

            //hong visible button 20130909
            btn_Save.Visible = true;
            btn_Back.Visible = true;

            //btn_Create.Visible = true;
            //btn_Delete.Visible = true;

            de_ReqDate.Visible = true;
            btn_ReqDate_Ok.Visible = true;

            // Modified on: 09/11/2017, By: Fon
            if (grd_StoreReqEdit.Rows.Count == 0)
            {
                ddl_Store.Enabled = true;
                ddl_Type.Enabled = true;
            }
            // End Modiefied.

            Session["dsStoreReqEdit"] = _dsReqEdit;
            e.Cancel = true;
        }

        protected void grd_StoreReqEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var chkItem = e.Row.FindControl("chk_Item") as CheckBox;
            var lnkbEdit = e.Row.FindControl("lnkb_Edit") as LinkButton;
            var lnkbDel = e.Row.FindControl("lnkb_Del") as LinkButton;

            if (workFlowDt.GetAllowCreate(wfID, WfStep, LoginInfo.ConnStr)) // if allow create step, show Delete
            {
                if (lnkbDel != null)
                    lnkbDel.Visible = true;
                if (lnkbEdit != null)
                    lnkbEdit.Visible = true;
            }
            else
            {
                if (lnkbDel != null)
                    lnkbDel.Visible = false;
                if (lnkbEdit != null)
                    lnkbEdit.Visible = true;
            }

            if (grd_StoreReqEdit.EditIndex == -1)
            {
                btn_Save.Visible = true;

                btn_ReqDate_Ok.Visible = true;

                //btn_Create.Visible = true;
                //btn_Delete.Visible = true;

                de_ReqDate.Visible = true;
            }
            else
            {
                btn_Save.Visible = false;
                btn_ReqDate_Ok.Visible = false;
                de_ReqDate.Visible = false;

                //btn_Create.Visible = false;
                //btn_Delete.Visible = false;

            }

            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    #region "DataRow"

                    if (e.Row.FindControl("ddl_gStore") != null)
                    {
                        var ddlGStore = (ASPxComboBox)e.Row.FindControl("ddl_gStore");

                        if (ddl_Store.Value != null)
                        {
                            var locationFrom = ddl_Store.Value.ToString();
                            var adjID = ddl_Type.Value.ToString();

                            var ds = locat.GetLookupByMovementType(locationFrom, adjID, LoginInfo.LoginName, LoginInfo.ConnStr);

                            ds.DefaultView.RowFilter = "LocationCode <> '" + ddl_Store.Value + "'";
                            ddlGStore.DataSource = ds;
                            ddlGStore.ValueField = "LocationCode";
                            ddlGStore.Value = DataBinder.Eval(e.Row.DataItem, "ToLocationCode");
                            ddlGStore.DataBind();
                        }
                    }

                    if (e.Row.FindControl("ddl_Product") != null)
                    {
                        var ddlProduct = (ASPxComboBox)e.Row.FindControl("ddl_Product");
                        var ddlGStore = (ASPxComboBox)e.Row.FindControl("ddl_gStore");
                        var ddlStore = (ASPxComboBox)UdPnDetail.FindControl("ddl_Store");

                        if (ddlGStore.Value != null)
                        {
                            ddlProduct.DataSource = product.GetListByTwoLocation(ddlStore.Value.ToString(),
                                ddlGStore.Value.ToString(), LoginInfo.ConnStr);
                            ddlProduct.ValueField = "ProductCode";
                            ddlProduct.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode");
                            ddlProduct.DataBind();
                        }
                    }

                    if (e.Row.FindControl("txt_QtyRequested") != null)
                    {
                        var txtQtyRequested = (ASPxSpinEdit)e.Row.FindControl("txt_QtyRequested");
                        txtQtyRequested.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RequestQty"));
                    }

                    if (e.Row.FindControl("lbl_Unit") != null)
                    {
                        var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                        lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                    }

                    if (e.Row.FindControl("de_gReqDate") != null)
                    {
                        var deGReqDate = (ASPxDateEdit)e.Row.FindControl("de_gReqDate");
                        deGReqDate.Date = de_ReqDate.Date;
                    }

                    if (e.Row.FindControl("txt_Comment") != null)
                    {
                        var txtComment = (TextBox)e.Row.FindControl("txt_Comment");
                        txtComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    }

                    if (e.Row.FindControl("lbl_LocationCode") != null)
                    {
                        var lblLocationCode = (Label)e.Row.FindControl("lbl_LocationCode");
                        lblLocationCode.Text = DataBinder.Eval(e.Row.DataItem, "ToLocationCode")
                                               + @" : " +
                                               locat.GetName(
                                                   DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                                                   LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_ProductCode") != null)
                    {
                        var lblProductCode = (Label)e.Row.FindControl("lbl_ProductCode");
                        lblProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") +
                                              @" : " + product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) +
                                              @" : " + product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_QtyRequested") != null)
                    {
                        var lblQtyRequested = (Label)e.Row.FindControl("lbl_QtyRequested");
                        lblQtyRequested.Text = DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString() != string.Empty
                            ? string.Format(DefaultQtyFmt, decimal.Parse(DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString()))
                            : string.Format(DefaultQtyFmt, "0");
                    }

                    if (e.Row.FindControl("lbl_Unit") != null)
                    {
                        var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                        lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                    }

                    if (e.Row.FindControl("lbl_ReqDate") != null)
                    {
                        var lblReqDate = (Label)e.Row.FindControl("lbl_ReqDate");
                        lblReqDate.Text = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString()).ToShortDateString();
                    }

                    if (Request.Params["MODE"].ToUpper() != "NEW")
                    {
                        //******************************* Disyplay Stock Movement ********************************************
                        if (e.Row.FindControl("uc_StockMovement") != null)
                        {
                            var ucStockMovement =
                                e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
                            if (ucStockMovement != null)
                            {
                                ucStockMovement.HdrNo =
                                    _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RequestCode"].ToString();
                                ucStockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                                ucStockMovement.ConnStr = LoginInfo.ConnStr;
                                ucStockMovement.DataBind();
                            }
                        }
                    }

                    if (_dsReqEdit.Tables[prDt.TableName] != null)
                    {
                        _dsReqEdit.Tables[prDt.TableName].Clear();
                    }

                    if (ddl_Store.Value != null)
                    {
                        var getStock = prDt.GetStockSummary(_dsReqEdit, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), ddl_Store.Value.ToString(), de_Date.Date.ToShortDateString(), LoginInfo.ConnStr);

                        if (getStock)
                        {
                            var drStockSummary = _dsReqEdit.Tables[prDt.TableName].Rows[0];

                            if (e.Row.FindControl("lbl_OnHand") != null)
                            {
                                var lblOnHand = (Label)e.Row.FindControl("lbl_OnHand");

                                if (drStockSummary["OnHand"].ToString() != string.Empty &&
                                    drStockSummary["OnHand"] != null)
                                {
                                    lblOnHand.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"]);
                                }
                                else
                                {
                                    lblOnHand.Text = string.Format(DefaultQtyFmt, "0");
                                }

                                lblOnHand.ToolTip = lblOnHand.Text;
                            }

                            if (e.Row.FindControl("lbl_OnOrder") != null)
                            {
                                var lblOnOrder = (Label)e.Row.FindControl("lbl_OnOrder");

                                if (drStockSummary["OnOrder"].ToString() != string.Empty &&
                                    drStockSummary["OnOrder"] != null)
                                {
                                    lblOnOrder.Text = String.Format(DefaultQtyFmt, drStockSummary["OnOrder"]);
                                }
                                else
                                {
                                    lblOnOrder.Text = string.Format(DefaultQtyFmt, "0");
                                }

                                lblOnOrder.ToolTip = lblOnOrder.Text;
                            }

                            if (e.Row.FindControl("lbl_Reorder") != null)
                            {
                                var lblReorder = (Label)e.Row.FindControl("lbl_Reorder");

                                if (drStockSummary["Reorder"].ToString() != string.Empty &&
                                    drStockSummary["Reorder"] != null)
                                {
                                    lblReorder.Text = String.Format(DefaultQtyFmt, drStockSummary["Reorder"]);
                                }
                                else
                                {
                                    lblReorder.Text = string.Format(DefaultQtyFmt, "0");
                                }

                                lblReorder.ToolTip = lblReorder.Text;
                            }

                            if (e.Row.FindControl("lbl_Restock") != null)
                            {
                                var lblRestock = (Label)e.Row.FindControl("lbl_Restock");

                                if (drStockSummary["Restock"].ToString() != string.Empty &&
                                    drStockSummary["Restock"] != null)
                                {
                                    lblRestock.Text = String.Format(DefaultQtyFmt, drStockSummary["Restock"]);
                                }
                                else
                                {
                                    lblRestock.Text = string.Format(DefaultQtyFmt, "0");
                                }

                                lblRestock.ToolTip = lblRestock.Text;
                            }

                            if (e.Row.FindControl("lbl_LastPrice") != null)
                            {
                                var lblLastPrice = (Label)e.Row.FindControl("lbl_LastPrice");

                                if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                                    drStockSummary["LastPrice"] != null)
                                {
                                    lblLastPrice.Text = String.Format(DefaultAmtFmt, drStockSummary["LastPrice"]);
                                }
                                else
                                {
                                    lblLastPrice.Text = string.Format(DefaultAmtFmt, "0");
                                }

                                lblLastPrice.ToolTip = lblLastPrice.Text;
                            }

                            if (e.Row.FindControl("lbl_LastVendor") != null)
                            {
                                var lblLastVendor = (Label)e.Row.FindControl("lbl_LastVendor");
                                lblLastVendor.Text = drStockSummary["LastVendor"].ToString();
                                lblLastVendor.ToolTip = lblLastVendor.Text;
                            }
                        }
                    }

                    if (e.Row.FindControl("lbl_OnHand_Req") != null)
                    {
                        var lblOnHandReq = (Label)e.Row.FindControl("lbl_OnHand_Req");

                        if (_dsReqEdit.Tables[prDt.TableName] != null)
                        {
                            _dsReqEdit.Tables[prDt.TableName].Clear();
                        }

                        var getOnHand = prDt.GetStockSummary(_dsReqEdit,
                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                            DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                            de_Date.Date.ToShortDateString(), LoginInfo.ConnStr);

                        if (getOnHand)
                        {
                            var drStockSummary = _dsReqEdit.Tables[prDt.TableName].Rows[0];

                            if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                            {
                                lblOnHandReq.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"]);
                            }
                            else
                            {
                                lblOnHandReq.Text = string.Format(DefaultQtyFmt, "0.00");
                            }

                            lblOnHandReq.ToolTip = lblOnHandReq.Text;
                        }
                    }

                    if (e.Row.FindControl("lbl_ItemGroup") != null)
                    {
                        var lblItemGroup = (Label)e.Row.FindControl("lbl_ItemGroup");
                        lblItemGroup.Text =
                            prodCat.GetName(
                                product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr);
                        lblItemGroup.ToolTip = lblItemGroup.Text;
                    }

                    if (e.Row.FindControl("lbl_SubCate") != null)
                    {
                        var lblSubCate = (Label)e.Row.FindControl("lbl_SubCate");
                        lblSubCate.Text =
                            prodCat.GetName(
                                product.GetParentNoByCategoryCode(
                                    product.GetProductCategory(
                                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr);
                        lblSubCate.ToolTip = lblSubCate.Text;
                    }

                    if (e.Row.FindControl("lbl_Category") != null)
                    {
                        var lblCategory = (Label)e.Row.FindControl("lbl_Category");
                        lblCategory.Text =
                            prodCat.GetName(
                                product.GetParentNoByCategoryCode(
                                    product.GetParentNoByCategoryCode(
                                        product.GetProductCategory(
                                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr),
                                        LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                        lblCategory.ToolTip = lblCategory.Text;
                    }

                    if (grd_StoreReqEdit.Columns[grd_StoreReqEdit.Columns.Count - 1].HeaderText.ToUpper() == "PROCESS STATUS" ||
                        grd_StoreReqEdit.Columns[grd_StoreReqEdit.Columns.Count - 1].HeaderText.ToUpper() == "สถานะ")
                    {
                        var newStatus = string.Empty;

                        for (var i = 0;
                            i <= DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString().Length - 10;
                            i += 10)
                        {
                            if (DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString().Substring(i, 10).Contains('R'))
                            {
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/REJ.gif\" style=\"width: 8px; height: 16px\" />";
                            }
                            else if (
                                DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                    .ToString()
                                    .Substring(i, 10)
                                    .Contains('_') &&
                                DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                    .ToString()
                                    .Substring(i, 10)
                                    .Contains('P'))
                            {
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/PAR.gif\" style=\"width: 8px; height: 16px\" />";
                            }
                            else if (
                                DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                    .ToString()
                                    .Substring(i, 10)
                                    .Contains('_'))
                            {
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/NA.gif\" style=\"width: 8px; height: 16px\" />";
                            }
                            else
                            {
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/APP.gif\" style=\"width: 8px; height: 16px\" />";
                            }
                        }

                        e.Row.Cells[grd_StoreReqEdit.Columns.Count - 1].Text = newStatus;
                    }
                    break;

                    #endregion
            }
        }

        protected void grd_StoreReqAppr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                #region "Header"

                var lbl_AllocateQty = (Label)e.Row.FindControl("lbl_AllocateQty");
                if (lbl_AllocateQty != null)
                {
                    lbl_AllocateQty.Visible = WfStep == WfStepCount; // last step
                }


                #endregion
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region "DataRow"

                var chkItem = e.Row.FindControl("chk_Item") as CheckBox;
                var lnkbEdit = e.Row.FindControl("lnkb_Edit") as LinkButton;
                var lnkbDel = e.Row.FindControl("lnkb_Del") as LinkButton;

                if (chkItem != null)
                {
                    chkItem.Visible = true;

                    // ********** Display Button. **********
                    var status = DataBinder.Eval(e.Row.DataItem, "ApprStatus");
                    var j = WfStep;

                    if (WfStep < 1)
                        j = 1;

                    if (!status.ToString().Substring((j - 1) * 10, 10).Contains('_')
                        /*current status ที่เปลี่ยนสถานะไปแล้ว*/
                        || status.ToString().Substring(0, (j - 1) * 10).Contains('_')
                        /*previous status ที่ยังทำไม่เสร็จ*/)
                    {
                        chkItem.Visible = false;

                        if (lnkbEdit != null) lnkbEdit.Visible = false;
                        if (lnkbDel != null) lnkbDel.Visible = false;
                    }
                }

                if (lnkbDel != null)
                    lnkbDel.Visible = workFlowDt.GetAllowCreate(wfID, WfStep, LoginInfo.ConnStr);

                string toLocationCode = DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString();

                // ItemTemplate
                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lblLocationCode = (Label)e.Row.FindControl("lbl_LocationCode");
                    lblLocationCode.Text = toLocationCode + " : " + locat.GetName(toLocationCode, LoginInfo.ConnStr);
                    lblLocationCode.ToolTip = lblLocationCode.Text;
                }

                // EditTemplate
                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lblLocation = (Label)e.Row.FindControl("lbl_Location");
                    lblLocation.Text = toLocationCode + " : " + locat.GetName(toLocationCode, LoginInfo.ConnStr);
                    lblLocation.ToolTip = lblLocation.Text;

                    var hfLocationCode = e.Row.FindControl("hf_LocationCode") as HiddenField;
                    if (hfLocationCode != null)
                        hfLocationCode.Value = toLocationCode;
                }

                string productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();

                // ItemTemplate
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lblProductCode = (Label)e.Row.FindControl("lbl_ProductCode");
                    lblProductCode.Text = productCode + " : " + product.GetName(productCode, LoginInfo.ConnStr) + " : " + product.GetName2(productCode, LoginInfo.ConnStr);
                    lblProductCode.ToolTip = lblProductCode.Text;
                }

                // EditTemplate
                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lblProduct = (Label)e.Row.FindControl("lbl_Product");
                    lblProduct.Text = productCode + " : " + product.GetName(productCode, LoginInfo.ConnStr) + " : " + product.GetName2(productCode, LoginInfo.ConnStr);
                    lblProduct.ToolTip = lblProduct.Text;

                    var hfProductCode = (HiddenField)e.Row.FindControl("hf_ProductCode");
                    hfProductCode.Value = productCode;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lblUnit = (Label)e.Row.FindControl("lbl_Unit");
                    lblUnit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                }


                if (e.Row.FindControl("lbl_QtyRequested") != null)
                {
                    string requestQty = DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString();
                    var lblQtyRequested = (Label)e.Row.FindControl("lbl_QtyRequested");

                    lblQtyRequested.Text = string.Format(DefaultQtyFmt, requestQty == string.Empty ? 0 : decimal.Parse(requestQty));
                    lblQtyRequested.ToolTip = lblQtyRequested.Text;
                }

                // ItemTemplate
                if (e.Row.FindControl("lbl_QtyAppr") != null)
                {
                    var lblQtyAppr = (Label)e.Row.FindControl("lbl_QtyAppr");

                    lblQtyAppr.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    lblQtyAppr.ToolTip = lblQtyAppr.Text;
                }

                // EditTemplate
                if (e.Row.FindControl("txt_QtyAppr") != null)
                {
                    var txtQtyAppr = (TextBox)e.Row.FindControl("txt_QtyAppr");

                    txtQtyAppr.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    txtQtyAppr.Enabled = WfStep < WfStepCount;
                }

                // ItemTemplate
                if (e.Row.FindControl("lbl_QtyAllocate") != null)
                {
                    var lblQtyAllocate = (Label)e.Row.FindControl("lbl_QtyAllocate");

                    lblQtyAllocate.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "AllocateQty"));
                    lblQtyAllocate.Visible = WfStep == WfStepCount;

                }

                // EditTemplate
                if (e.Row.FindControl("txt_QtyAllocate") != null)
                {
                    var txtQtyAllocate = (TextBox)e.Row.FindControl("txt_QtyAllocate");

                    txtQtyAllocate.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "AllocateQty"));
                    txtQtyAllocate.Visible = WfStep == WfStepCount;
                }

                // ItemTemplate
                if (e.Row.FindControl("lbl_ReqDate") != null)
                {
                    var lblReqDate = (Label)e.Row.FindControl("lbl_ReqDate");
                    lblReqDate.Text = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString()).ToShortDateString();
                }

                // EditTemplate
                if (e.Row.FindControl("de_gReqDate") != null)
                {
                    var deGReqDate = (ASPxDateEdit)e.Row.FindControl("de_gReqDate");
                    deGReqDate.Date = de_ReqDate.Date;
                    deGReqDate.Enabled = false;
                }

                // -------------------------------------------------------------------------------------------------

                #region Summary section

                if (Request.Params["MODE"].ToUpper() != "NEW")
                {
                    //******************************* Disyplay Stock Movement ********************************************
                    if (e.Row.FindControl("uc_StockMovement") != null)
                    {
                        var ucStockMovement = e.Row.FindControl("uc_StockMovement") as PC.StockMovement;
                        ucStockMovement.HdrNo = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RequestCode"].ToString();
                        ucStockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                        ucStockMovement.ConnStr = LoginInfo.ConnStr;
                        ucStockMovement.DataBind();
                    }
                }


                if (_dsReqEdit.Tables[prDt.TableName] != null)
                {
                    _dsReqEdit.Tables[prDt.TableName].Clear();
                }

                if (ddl_Store.Value != null)
                {
                    var getStock = prDt.GetStockSummary(_dsReqEdit,
                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        ddl_Store.Value.ToString(), de_Date.Date.ToShortDateString(), LoginInfo.ConnStr);

                    if (getStock)
                    {
                        var drStockSummary = _dsReqEdit.Tables[prDt.TableName].Rows[0];

                        if (e.Row.FindControl("lbl_OnHand") != null)
                        {
                            var lblOnHand = (Label)e.Row.FindControl("lbl_OnHand");

                            if (drStockSummary["OnHand"].ToString() != string.Empty &&
                                drStockSummary["OnHand"] != null)
                            {
                                lblOnHand.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"].ToString());
                            }
                            else
                            {
                                lblOnHand.Text = @"0.00";
                            }

                            lblOnHand.ToolTip = lblOnHand.Text;
                        }

                        if (e.Row.FindControl("lbl_OnOrder") != null)
                        {
                            var lblOnOrder = (Label)e.Row.FindControl("lbl_OnOrder");

                            if (drStockSummary["OnOrder"].ToString() != string.Empty &&
                                drStockSummary["OnOrder"] != null)
                            {
                                lblOnOrder.Text = String.Format(DefaultQtyFmt, drStockSummary["OnOrder"].ToString());
                            }
                            else
                            {
                                lblOnOrder.Text = @"0.00";
                            }

                            lblOnOrder.ToolTip = lblOnOrder.Text;
                        }

                        if (e.Row.FindControl("lbl_Reorder") != null)
                        {
                            var lblReorder = (Label)e.Row.FindControl("lbl_Reorder");

                            if (drStockSummary["Reorder"].ToString() != string.Empty &&
                                drStockSummary["Reorder"] != null)
                            {
                                lblReorder.Text = String.Format(DefaultQtyFmt, drStockSummary["Reorder"].ToString());
                            }
                            else
                            {
                                lblReorder.Text = @"0.00";
                            }

                            lblReorder.ToolTip = lblReorder.Text;
                        }

                        if (e.Row.FindControl("lbl_Restock") != null)
                        {
                            var lblRestock = (Label)e.Row.FindControl("lbl_Restock");

                            if (drStockSummary["Restock"].ToString() != string.Empty &&
                                drStockSummary["Restock"] != null)
                            {
                                lblRestock.Text = String.Format(DefaultQtyFmt, drStockSummary["Restock"].ToString());
                            }
                            else
                            {
                                lblRestock.Text = @"0.00";
                            }

                            lblRestock.ToolTip = lblRestock.Text;
                        }

                        if (e.Row.FindControl("lbl_LastPrice") != null)
                        {
                            var lblLastPrice = (Label)e.Row.FindControl("lbl_LastPrice");

                            if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                                drStockSummary["LastPrice"] != null)
                            {
                                lblLastPrice.Text = String.Format(DefaultAmtFmt, drStockSummary["LastPrice"].ToString());
                            }
                            else
                            {
                                lblLastPrice.Text = @"0.00";
                            }

                            lblLastPrice.ToolTip = lblLastPrice.Text;
                        }

                        if (e.Row.FindControl("lbl_LastVendor") != null)
                        {
                            var lblLastVendor = (Label)e.Row.FindControl("lbl_LastVendor");
                            lblLastVendor.Text = drStockSummary["LastVendor"].ToString();
                            lblLastVendor.ToolTip = lblLastVendor.Text;
                        }
                    }
                }

                if (e.Row.FindControl("lbl_OnHand_Req") != null)
                {
                    var lblOnHandReq = (Label)e.Row.FindControl("lbl_OnHand_Req");

                    if (_dsReqEdit.Tables[prDt.TableName] != null)
                    {
                        _dsReqEdit.Tables[prDt.TableName].Clear();
                    }

                    var getOnHand = prDt.GetStockSummary(_dsReqEdit,
                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(), de_Date.Date.ToShortDateString(),
                        LoginInfo.ConnStr);

                    if (getOnHand)
                    {
                        var drStockSummary = _dsReqEdit.Tables[prDt.TableName].Rows[0];

                        if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                        {
                            lblOnHandReq.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"].ToString());
                        }
                        else
                        {
                            lblOnHandReq.Text = @"0.00";
                        }

                        lblOnHandReq.ToolTip = lblOnHandReq.Text;
                    }
                }

                if (e.Row.FindControl("lbl_ItemGroup") != null)
                {
                    var lblItemGroup = (Label)e.Row.FindControl("lbl_ItemGroup");
                    lblItemGroup.Text =
                        prodCat.GetName(
                            product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblItemGroup.ToolTip = lblItemGroup.Text;
                }

                if (e.Row.FindControl("lbl_SubCate") != null)
                {
                    var lblSubCate = (Label)e.Row.FindControl("lbl_SubCate");
                    lblSubCate.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetProductCategory(
                                    DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr),
                                LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblSubCate.ToolTip = lblSubCate.Text;
                }

                if (e.Row.FindControl("lbl_Category") != null)
                {
                    var lblCategory = (Label)e.Row.FindControl("lbl_Category");
                    lblCategory.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetParentNoByCategoryCode(
                                    product.GetProductCategory(
                                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblCategory.ToolTip = lblCategory.Text;
                }


                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txtComment = (TextBox)e.Row.FindControl("txt_Comment");
                    txtComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                #region
                if (grd_StoreReqAppr.Columns[grd_StoreReqAppr.Columns.Count - 1].HeaderText.ToUpper() == "PROCESS STATUS" ||
                    grd_StoreReqEdit.Columns[grd_StoreReqEdit.Columns.Count - 1].HeaderText.ToUpper() == "สถานะ")
                {
                    var newStatus = string.Empty;

                    //for (int i = 0; i <= e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text.Length - 10; i += 10)
                    for (var i = 0;
                        i <= DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString().Length - 10;
                        i += 10)
                    {
                        //if (e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text.Substring(i, 10).Contains('R'))
                        if (DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString().Substring(i, 10).Contains('R'))
                        {
                            newStatus = newStatus +
                                        "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/REJ.gif\" style=\"width: 8px; height: 16px\" />";
                        }
                        //else if (e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text.Substring(i, 10).Contains('_') && e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text.Substring(i, 10).Contains('P'))
                        else if (
                            DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                .ToString()
                                .Substring(i, 10)
                                .Contains('_') &&
                            DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                .ToString()
                                .Substring(i, 10)
                                .Contains('P'))
                        {
                            newStatus = newStatus +
                                        "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/PAR.gif\" style=\"width: 8px; height: 16px\" />";
                        }
                        //else if (e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text.Substring(i, 10).Contains('_'))
                        else if (
                            DataBinder.Eval(e.Row.DataItem, "ApprStatus")
                                .ToString()
                                .Substring(i, 10)
                                .Contains('_'))
                        {
                            newStatus = newStatus +
                                        "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/NA.gif\" style=\"width: 8px; height: 16px\" />";
                        }
                        else
                        {
                            newStatus = newStatus +
                                        "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/APP.gif\" style=\"width: 8px; height: 16px\" />";
                        }
                    }

                    e.Row.Cells[grd_StoreReqAppr.Columns.Count - 1].Text = newStatus;
                }
                #endregion
                #endregion // Summary

                #endregion // DataRow
            }
        }

        protected void grd_StoreReqAppr_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //hong visible button 20130909
            btn_Save.Visible = false;
            btn_Back.Visible = false;
            btn_Create.Visible = false;
            //btn_Delete.Visible = false;
            de_ReqDate.Visible = false;
            btn_ReqDate_Ok.Visible = false;

            grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqAppr.EditIndex = e.NewEditIndex;
            grd_StoreReqAppr.DataBind();

            for (var i = grd_StoreReqAppr.Rows.Count - 1; i >= 0; i--)
            {
                var chkItem = grd_StoreReqAppr.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                var chkAll = grd_StoreReqAppr.HeaderRow.FindControl("Chk_All") as CheckBox;

                if (chkAll != null) chkAll.Enabled = false;
                if (chkItem != null) chkItem.Enabled = false;
            }
        }

        protected void grd_StoreReqAppr_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var ddlGStore = (ASPxComboBox)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("ddl_gStore");
            var ddlProduct = (ASPxComboBox)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("ddl_Product");
            var txtQtyAllocate = (TextBox)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("txt_QtyAllocate");
            var txtQtyAppr = (TextBox)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("txt_QtyAppr");
            var txtComment = (TextBox)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("txt_Comment");
            var deGReqDate = (ASPxDateEdit)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("de_gReqDate");
            var lblUnit = (Label)grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("lbl_Unit");

            if (txtQtyAllocate.Visible)
            {
                if (txtQtyAllocate.Text == string.Empty)
                {
                    lbl_Warning.Text = @"'Qty Allocate' cannot empty";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

                decimal number;
                if (!decimal.TryParse(txtQtyAllocate.Text, out number) || decimal.Parse(txtQtyAllocate.Text) <= 0)
                {
                    lbl_Warning.Text = @"Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

            }

            if (txtQtyAppr.Visible)
            {
                decimal number;
                if (txtQtyAppr.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Qty Approve' cannot be empty";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

                if (!decimal.TryParse(txtQtyAppr.Text, out number) || decimal.Parse(txtQtyAppr.Text) <= 0)
                {
                    lbl_Warning.Text = "Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
            }

            var drUpdating = _dsReqEdit.Tables[reqDt.TableName].Rows[grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].DataItemIndex];

            drUpdating["DeliveryDate"] = deGReqDate.Date;
            drUpdating["ApprQty"] = decimal.Parse(txtQtyAppr.Text == string.Empty ? "0.00" : txtQtyAppr.Text);
            drUpdating["AllocateQty"] = decimal.Parse(txtQtyAllocate.Text == string.Empty ? "0.00" : txtQtyAllocate.Text);
            drUpdating["RequestUnit"] = lblUnit.Text;
            drUpdating["DebitACCode"] = null;
            drUpdating["CreditACCode"] = null;
            drUpdating["Comment"] = txtComment.Text == string.Empty ? null : txtComment.Text;

            grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqAppr.EditIndex = -1;
            grd_StoreReqAppr.DataBind();

            StoreReqEditMode = string.Empty;

            btn_Save.Visible = true;
            btn_Back.Visible = true;


            de_ReqDate.Visible = true;
            btn_ReqDate_Ok.Visible = true;
        }

        protected void grd_StoreReqAppr_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (_dsReqEdit.Tables[reqDt.TableName].Rows.Count > 0)
            {
                for (var i = 0; i < _dsReqEdit.Tables[reqDt.TableName].Rows.Count; i++)
                {
                    var drDelete = _dsReqEdit.Tables[reqDt.TableName].Rows[i];
                    if (drDelete.RowState == DataRowState.Deleted) continue;
                    if (i == e.RowIndex)
                    {
                        drDelete.Delete();
                    }
                }
            }

            grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqAppr.EditIndex = -1;
            grd_StoreReqAppr.DataBind();

            Session["dsStoreReqEdit"] = _dsReqEdit;
            e.Cancel = true;
        }

        protected void grd_StoreReqAppr_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            _dsReqEdit.Tables[reqDt.TableName].Rows[_dsReqEdit.Tables[reqDt.TableName].Rows.Count - 1].CancelEdit();

            grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqAppr.EditIndex = -1;
            grd_StoreReqAppr.DataBind();

            //hong visible button 20130909
            btn_Save.Visible = true;
            btn_Back.Visible = true;

            //btn_Create.Visible = true;
            //btn_Delete.Visible = true;

            de_ReqDate.Visible = true;
            btn_ReqDate_Ok.Visible = true;


            //Page_Setting();
        }

        //protected void btn_Warning_Click(object sender, EventArgs e)
        //{
        //    pop_Warning.ShowOnPageLoad = false;
        //}

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;

            //if (viewHandler.GetDesc(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr).ToUpper() == "APPROVE" ||
            //    viewHandler.GetDesc(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr).ToUpper() == "ISSUE")
            if (viewHandler.GetWFStep(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr) > 1)
            {
                for (var i = grd_StoreReqAppr.Rows.Count - 1; i >= 0; i--)
                {
                    var chkItem = grd_StoreReqAppr.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                    if (chkItem != null) chkItem.Checked = false;
                }
            }
            else
            {
                for (var i = grd_StoreReqEdit.Rows.Count - 1; i >= 0; i--)
                {
                    var chkItem = grd_StoreReqEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                    if (chkItem != null) chkItem.Checked = false;
                }
            }
        }

        protected void txt_QtyRequested_TextChanged(object sender, EventArgs e)
        {
            //var txtQtyRequested = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("txt_QtyRequested") as ASPxSpinEdit;
            //var ddlProduct = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            //var deGReqDate = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;

            //if (ddlProduct == null)
            //    return;

            //var onHand = GetOnHand(ddlProduct.Value.ToString(), ddl_Store.Value.ToString(), de_Date.Text);
            //decimal sumQtyForChkOnhand = 0;

            //foreach (GridViewRow grvRow in grd_StoreReqEdit.Rows)
            //{
            //    var lblQtyRequested = grvRow.FindControl("lbl_QtyRequested") as Label;

            //    if (_dsReqEdit.Tables[reqDt.TableName].Rows[grvRow.RowIndex]["ProductCode"].ToString() != "" &&
            //        lblQtyRequested != null)
            //    {
            //        if (_dsReqEdit.Tables[reqDt.TableName].Rows[grvRow.RowIndex]["ProductCode"].ToString() ==
            //            ddlProduct.Value.ToString())
            //        {
            //            sumQtyForChkOnhand += Convert.ToDecimal(lblQtyRequested.Text);
            //        }
            //    }

            //    if (_dsReqEdit.Tables[reqDt.TableName].Rows[grvRow.RowIndex]["ProductCode"].ToString() == "" ||
            //        txtQtyRequested == null) continue;
            //    if (_dsReqEdit.Tables[reqDt.TableName].Rows[grvRow.RowIndex]["ProductCode"].ToString() ==
            //        ddlProduct.Value.ToString())
            //    {
            //        sumQtyForChkOnhand += Convert.ToDecimal(txtQtyRequested.Text);
            //    }
            //}

            //if (txtQtyRequested != null && decimal.Parse(txtQtyRequested.Text) > onHand)
            //{
            //    lbl_Warning.Text = @"Cannot request quantity more than on hand";
            //    pop_Warning.ShowOnPageLoad = true;
            //    txtQtyRequested.Text = String.Format(DefaultQtyFmt, onHand);
            //}

            //if (sumQtyForChkOnhand <= onHand) return;
            //lbl_Warning.Text = @"Cannot approve quantity more than on hand";
            //pop_Warning.ShowOnPageLoad = true;
            //if (txtQtyRequested != null) txtQtyRequested.Text = "0.00";
        }

        protected void txt_QtyAppr_TextChanged(object sender, EventArgs e)
        {
            //var txtQtyAppr = grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("txt_QtyAppr") as TextBox;
            //var hfProductCode = grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("hf_ProductCode") as HiddenField;
        }

        protected void txt_QtyAllocate_TextChanged(object sender, EventArgs e)
        {
            //var txtQtyAllocate = grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("txt_QtyAllocate") as TextBox;
            //var hfProductCode = grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            //var deGReqDate = grd_StoreReqAppr.Rows[grd_StoreReqAppr.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;
        }

        private decimal GetOnHand(string productCode, string locationCode, string strDate)
        {
            if (_dsReqEdit.Tables[prDt.TableName] != null)
            {
                _dsReqEdit.Tables[prDt.TableName].Clear();
            }

            var get = prDt.GetStockSummary(_dsReqEdit, productCode, locationCode, strDate, LoginInfo.ConnStr);

            if (!get) return 0;

            if (_dsReqEdit.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty &&
                _dsReqEdit.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
            {
                return decimal.Parse(_dsReqEdit.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
            }

            return 0;
        }

        private void UpdateStock(string productId, string locationId, GridViewRow grdRow)
        {
            if (productId == null) throw new ArgumentNullException("productId");
            //ASPxComboBox ddl_Product = grd_StoreReq.Rows[grd_StoreReq.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            //ASPxComboBox ddl_gStore = grd_StoreReq.Rows[grd_StoreReq.EditIndex].FindControl("ddl_Product") as ASPxComboBox;

            //******************************* Stock Summary ******************************************************
            if (_dsReqEdit.Tables[prDt.TableName] != null)
            {
                _dsReqEdit.Tables[prDt.TableName].Clear();
            }

            var getStock = prDt.GetStockSummary(_dsReqEdit, productId, locationId, de_Date.Date.ToShortDateString(), LoginInfo.ConnStr);

            if (getStock)
            {
                var drStockSummary = _dsReqEdit.Tables[prDt.TableName].Rows[0];

                if (grdRow.FindControl("lbl_OnHand") != null)
                {
                    var lblOnHand = (Label)grdRow.FindControl("lbl_OnHand");

                    if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                    {
                        lblOnHand.Text = String.Format(DefaultQtyFmt, drStockSummary["OnHand"]);
                    }
                    else
                    {
                        lblOnHand.Text = @"0.00";
                    }

                    lblOnHand.ToolTip = lblOnHand.Text;
                }

                if (grdRow.FindControl("lbl_OnOrder") != null)
                {
                    var lblOnOrder = (Label)grdRow.FindControl("lbl_OnOrder");

                    if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                    {
                        lblOnOrder.Text = String.Format(DefaultQtyFmt, drStockSummary["OnOrder"]);
                    }
                    else
                    {
                        lblOnOrder.Text = string.Format(DefaultQtyFmt, "0"); ;
                    }

                    lblOnOrder.ToolTip = lblOnOrder.Text;
                }

                if (grdRow.FindControl("lbl_Reorder") != null)
                {
                    var lblReorder = (Label)grdRow.FindControl("lbl_Reorder");

                    if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                    {
                        lblReorder.Text = String.Format(DefaultQtyFmt, drStockSummary["Reorder"]);
                    }
                    else
                    {
                        lblReorder.Text = string.Format(DefaultQtyFmt, "0"); ;
                    }

                    lblReorder.ToolTip = lblReorder.Text;
                }

                if (grdRow.FindControl("lbl_Restock") != null)
                {
                    var lblRestock = (Label)grdRow.FindControl("lbl_Restock");

                    if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                    {
                        lblRestock.Text = String.Format(DefaultQtyFmt, drStockSummary["Restock"]);
                    }
                    else
                    {
                        lblRestock.Text = string.Format(DefaultQtyFmt, "0");
                    }

                    lblRestock.ToolTip = lblRestock.Text;
                }

                if (grdRow.FindControl("lbl_LastPrice") != null)
                {
                    var lblLastPrice = (Label)grdRow.FindControl("lbl_LastPrice");

                    if (drStockSummary["LastPrice"].ToString() != string.Empty && drStockSummary["LastPrice"] != null)
                    {
                        lblLastPrice.Text = String.Format(DefaultAmtFmt, drStockSummary["LastPrice"]);
                    }
                    else
                    {
                        lblLastPrice.Text = string.Format(DefaultAmtFmt, "0");
                    }

                    lblLastPrice.ToolTip = lblLastPrice.Text;
                }

                if (grdRow.FindControl("lbl_LastVendor") != null)
                {
                    var lblLastVendor = (Label)grdRow.FindControl("lbl_LastVendor");
                    lblLastVendor.Text = drStockSummary["LastVendor"].ToString();
                    lblLastVendor.ToolTip = lblLastVendor.Text;
                }
            }

            //****************************** Product Info **********************************************
            if (grdRow.FindControl("lbl_ItemGroup") != null)
            {
                var lblItemGroup = (Label)grdRow.FindControl("lbl_ItemGroup");
                lblItemGroup.Text = prodCat.GetName(product.GetProductCategory(productId, LoginInfo.ConnStr),
                    LoginInfo.ConnStr);
                lblItemGroup.ToolTip = lblItemGroup.Text;
            }

            if (grdRow.FindControl("lbl_SubCate") != null)
            {
                var lblSubCate = (Label)grdRow.FindControl("lbl_SubCate");
                lblSubCate.Text =
                    prodCat.GetName(
                        product.GetParentNoByCategoryCode(
                            product.GetProductCategory(productId, LoginInfo.ConnStr), LoginInfo.ConnStr),
                        LoginInfo.ConnStr);
                lblSubCate.ToolTip = lblSubCate.Text;
            }

            if (grdRow.FindControl("lbl_Category") == null) return;
            var lblCategory = (Label)grdRow.FindControl("lbl_Category");
            lblCategory.Text =
                prodCat.GetName(
                    product.GetParentNoByCategoryCode(
                        product.GetParentNoByCategoryCode(
                            product.GetProductCategory(productId, LoginInfo.ConnStr), LoginInfo.ConnStr),
                        LoginInfo.ConnStr), LoginInfo.ConnStr);
            lblCategory.ToolTip = lblCategory.Text;
        }

        protected void de_gReqDate_DateChanged(object sender, EventArgs e)
        {
        }

        protected void de_ReqDate_DateChanged(object sender, EventArgs e)
        {
        }

        protected void txt_Date_TextChanged(object sender, EventArgs e)
        {
            //for (var i = 0; i < grd_StoreReqEdit.Rows.Count; i++)
            //{
            //    var lblStore = (Label)grd_StoreReqEdit.Rows[i].FindControl("lbl_LocationCode");
            //    var lblProduct = (Label)grd_StoreReqEdit.Rows[i].FindControl("lbl_ProductCode");
            //    var lblQty = (Label)grd_StoreReqEdit.Rows[i].FindControl("lbl_QtyRequested");

            //    var storeCode = lblStore.Text.Split(':')[0].Trim();
            //    var storeName = lblStore.Text.Split(':')[1].Trim();
            //    var productCode = lblProduct.Text.Split(':')[0].Trim();

            //    var onHand = GetOnHand(productCode, storeCode, de_Date.Text);
            //    var qty = decimal.Parse(lblQty.Text);

            //    if (onHand - qty >= 0) continue;
            //    lbl_Warning.Text =
            //        string.Format("Commit quantity of product {0} : {1}, {2} must be less than on hand.",
            //            lblProduct.Text.Split(':')[0].Trim(),
            //            product.GetName(lblProduct.Text.Split(':')[0].Trim(), LoginInfo.ConnStr),
            //            product.GetName2(lblProduct.Text.Split(':')[0].Trim(), LoginInfo.ConnStr));
            //    pop_Warning.ShowOnPageLoad = true;
            //    return;
            //}
        }


        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }

        protected void ddl_Type_Load(object sender, EventArgs e)
        {
            var comboBox = (ASPxComboBox)sender;

            DataSet ds = new DataSet();
            DataTable dtAdjType = adjType.GetList("", LoginInfo.ConnStr);
            if (Request.Params["MODE"].ToUpper() == "SR")
            {
                string locationCode = string.Format("{0}", _dsReqEdit.Tables[reqDt.TableName].Rows[0]["ToLocationCode"]);
                locat.Get(ds, locationCode, LoginInfo.ConnStr);
                int eopCode = int.Parse(ds.Tables[locat.TableName].Rows[0]["EOP"].ToString());

                // Following from code before.
                if (eopCode == 1)       // Enter Counted Stock
                {
                    dtAdjType.DefaultView.RowFilter = "[AdjType] = 'Transfer'";
                    comboBox.DataSource = dtAdjType.DefaultView;
                }
                else if (eopCode == 2)  // Default Zero
                {
                    dtAdjType.DefaultView.RowFilter = "[AdjType] = 'Issue'";
                    comboBox.DataSource = dtAdjType.DefaultView;
                }
                else
                    comboBox.DataSource = dtAdjType;
            }
            else
                comboBox.DataSource = dtAdjType;
            // End Modified.

            comboBox.ValueField = "AdjID";
            comboBox.DataBind();
            if (comboBox.Value != null)
            {
                // comboBox.Text = adjType.GetName(comboBox.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region "Editors"
        protected void btn_Back_Click(object sender, EventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                Response.Redirect("StoreReqDt.aspx?ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"] +
                                  "&BuCode=" + LoginInfo.BuInfo.BuCode);
            }
            else
            {
                Response.Redirect("StoreReqLst.aspx");
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var ds = _dsReqEdit.Tables[locat.TableName];
            ds.DefaultView.RowFilter = "EOP <> 2";
            var dt = ds.DefaultView.ToTable();
            ddl_Store.DataSource = dt;
            ddl_Store.ValueField = "LocationCode";
            ddl_Store.DataBind();
        }



        protected void btn_Create_Click(object sender, EventArgs e)
        {
            if (ddl_Store.Value == null)
            {
                lbl_Warning.Text = @"Please select 'Requested From'.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            if (ddl_Type.Value == null)
            {
                lbl_Warning.Text = @"Please select 'Type'.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            //if (ddl_JobCode.Value == null)
            //{
            //    lbl_Warning.Text = @"Please select 'Job Code'.";
            //    pop_Warning.ShowOnPageLoad = true;
            //    return;
            //}

            Create();
        }

        private void Create()
        {
            btn_Back.Visible = false;
            btn_Save.Visible = false;
            btn_Commit.Visible = false;

            int limitDetails = 0;
            if (LimitDetail != string.Empty)
            {
                limitDetails = Convert.ToInt32(LimitDetail);
            }

            // < : create new detail, = : cannot
            if (LimitDetail == string.Empty
                || grd_StoreReqEdit.Rows.Count < limitDetails
                || limitDetails == 0)
            {
                #region Unlimit or Under-limit
                //check create rows empty
                var dt = _dsReqEdit.Tables[reqDt.TableName];
                for (var i = dt.Rows.Count; i > 0; i--)
                {
                    var item = dt.Rows[i - 1];
                    if (string.IsNullOrEmpty(item["ProductCode"].ToString()))
                    {
                        item.Delete();
                    }
                }

                //btn_Save.Enabled = false;
                //btn_Create.Enabled = false;
                ddl_Store.Enabled = false;

                // Modified on: 09/11/2017, By: Fon
                ddl_Type.Enabled = false;
                // End Modified.

                //hong visible button 20130909
                btn_Save.Visible = true;

                //btn_Create.Visible = true;
                //btn_Delete.Visible = true;

                de_ReqDate.Visible = true;
                btn_ReqDate_Ok.Visible = true;

                //ddl_Type.Enabled = false;
                //reqDt.GetStructure(dsStoreReqEdit, LoginInfo.ConnStr);

                var drNew = dt.NewRow();
                drNew["DeliveryDate"] = de_ReqDate.Date;

                if (grd_StoreReqEdit.Rows.Count > 0)
                {
                    // Find last Location and stamp to ToLocation
                    drNew["ToLocationCode"] = dt.Rows[dt.Rows.Count - 1]["ToLocationCode"].ToString();
                }

                dt.Rows.Add(drNew);

                //Session["dsStoreReqEdit"] = dsStoreReqEdit;

                grd_StoreReqEdit.DataSource = dt;
                grd_StoreReqEdit.EditIndex = dt.Rows.Count - 1;
                grd_StoreReqEdit.DataBind();

                de_Date.Enabled = false;
                StoreReqEditMode = "New";
                #endregion
            }
            else
            {
                lbl_Warning.Text = string.Format("SR is limited {0} detail(s).", limitDetails);
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var editRow = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex];
            var ddlProduct = editRow.FindControl("ddl_Product") as ASPxComboBox;
            var lblUnit = editRow.FindControl("lbl_Unit") as Label;

            if (ddlProduct.Text != string.Empty)
            {

                if (ddl_Store.Value != null)
                {
                    if ((lblUnit != null) && (ddlProduct != null))
                        lblUnit.Text = product.GetInvenUnit(ddlProduct.Value.ToString(), LoginInfo.ConnStr);

                    if (ddlProduct != null)
                        UpdateStock(ddlProduct.Value.ToString(), ddl_Store.Value.ToString(),
                            grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex]);
                }
                else
                {
                    if (ddlProduct != null) ddlProduct.Value = null;
                    lbl_Warning.Text = @"Please select 'Request From'.";
                    pop_Warning.ShowOnPageLoad = true;
                }
            }
        }


        protected void ddl_JobCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            var dsJobCode = new DataSet();

            var result = jobCode.GetJobCodeList(dsJobCode, e.Filter, (e.BeginIndex + 1).ToString(), (e.EndIndex + 1).ToString(), LoginInfo.ConnStr);

            if (!result) return;
            comboBox.DataSource = dsJobCode.Tables[jobCode.TableName];
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_JobCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            try
            {
                if (e.Value == null)
                    return;

                SqlDataSource1.SelectCommand = @"SELECT * FROM [IMPORT].[JobCode] WHERE (CODE = @CODE) ORDER BY CODE";
                SqlDataSource1.ConnectionString = LoginInfo.ConnStr;
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("CODE", TypeCode.String, e.Value.ToString());
                comboBox.DataSource = SqlDataSource1;
                comboBox.DataBind();
                comboBox.ToolTip = comboBox.Text;
            }
            catch (Exception ex)
            {
                comboBox.ToolTip = @"Found Exception.";
                LogManager.Error(ex);
            }
        }

        protected void ddl_Product_Load(object sender, EventArgs e)
        {
            //var ddlProduct = (sender as ASPxComboBox);
            //if (ddlProduct != null)
            //{
            //    var ddlGStore = ddlProduct.Parent.FindControl("ddl_gStore") as ASPxComboBox;

            //    if (ddlGStore.Value != null && ddl_Store.Value != null)
            //    {
            //        var ds = product.GetListByTwoLocation(ddl_Store.Value.ToString(), ddlGStore.Value.ToString(), LoginInfo.ConnStr);
            //        var dt = new ClassComboHelper().DataLimit(ds);

            //        string productCode = string.Empty;

            //        ddlProduct.DataSource = dt;
            //        ddlProduct.ValueField = "ProductCode";
            //        ddlProduct.DataBind();
            //    }
            //}
        }

        protected void ddl_Product_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var comboBox = source as ASPxComboBox;
            if (comboBox == null) return;

            var ddlGStore = comboBox.Parent.FindControl("ddl_gStore") as ASPxComboBox;

            if (e.Value == null || ddlGStore == null || ddlGStore.Value == null || ddl_Store.Value == null) return;

            try
            {
                new ProductLookup().ItemRequestedByValue2Location(ref comboBox,
                    SqlDataSource1, LoginInfo.ConnStr, e, ddl_Store.Value.ToString(),
                    ddlGStore.Value.ToString());
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        protected void ddl_Product_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var ddlProduct = source as ASPxComboBox;
            //if (ddlProduct == null) return;

            var ddlGStore = ddlProduct.Parent.FindControl("ddl_gStore") as ASPxComboBox;

            if (ddlGStore == null || ddlGStore.Value == null || ddl_Store.Value == null || e.Filter == null) return;

            //if (string.IsNullOrEmpty())
            //{
            //    ddlProduct.Value = null;
            //    ddlProduct.Text = string.Empty;
            //    return;
            //}



            var comboBox = (ASPxComboBox)source;
            new ProductLookup().ItemsRequestedByFilterCondition2Location(ref comboBox, SqlDataSource1, LoginInfo.ConnStr,
                e, ddl_Store.Value.ToString(), ddlGStore.Value.ToString());
        }

        protected void ddl_gStore_Load(object sender, EventArgs e)
        {
            var ddlGStore = sender as ASPxComboBox;
            if (ddlGStore == null) return;

            //ddl_gStore.DataSource = dsStoreReqEdit.Tables[locat.TableName];
            //var ds = locat.GetLookup(ddl_Store.Value.ToString(), LoginInfo.ConnStr);  // Edit by Ake (2013-11-26)

            var locationFrom = ddl_Store.Value.ToString();
            var adjID = ddl_Type.Value.ToString();

            // Modified on: 03/11/2017, By: Fon,
            // For: Filter detail(IN) by location that user able to be access.
            //var ds = locat.GetLookupByMovementType(locationFrom, adjID, LoginInfo.ConnStr);
            var ds = locat.GetLookupByMovementType(locationFrom, adjID, LoginInfo.LoginName, LoginInfo.ConnStr);
            // End Modified.

            ds.DefaultView.RowFilter = "LocationCode <> '" + locationFrom + "'";
            var dt = ds.DefaultView.ToTable();
            ddlGStore.DataSource = dt;
            ddlGStore.ValueField = "LocationCode";
            ddlGStore.DataBind();
        }

        protected void ddl_gStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlProduct = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddlGStore = sender as ASPxComboBox;
            if (ddlProduct == null || ddl_Store.Value == null || ddlGStore == null) return;

            // Get Store Location Name
            //var storeName = locat.GetName(ddlGStore.Value.ToString(), LoginInfo.ConnStr);


            //Check Period
            //if (period.GetIsValidDate(DateTime.Parse(de_Date.Text), ddl_Store.Value.ToString(), LoginInfo.ConnStr))
            //{
            //ddlProduct.Text = "";
            var ds = product.GetListByTwoLocation(
                ddl_Store.Value.ToString(),
                ddlGStore.Value.ToString(),
                LoginInfo.ConnStr);
            var dt = new ClassComboHelper().DataLimit(ds);
            ddlProduct.DataSource = ds;
            ddlProduct.ValueField = "ProductCode";
            ddlProduct.DataBind();

            //}
            //else
            //{
            //    ddl_Store.Value = string.Empty;
            //    lbl_WarningPeriod.Text = string.Format("Store {0} is not period.", storeName);
            //    pop_WarningPeriod.ShowOnPageLoad = true;
            //}
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (Save_Process())
                Response.Redirect("StoreReqDt.aspx?ID=" + _dsReqEdit.Tables[reqDt.TableName].Rows[0]["DocumentId"] + "&VID=" + Request.Params["VID"] + "&BuCode=" + LoginInfo.BuInfo.BuCode);
        }

        private void CreateAccountMap(DataSet dsStockOut, string connStr)
        {
            return;
            var ds = new DataSet();
            accMapp.GetStructure(ds, connStr);

            foreach (DataRow item in dsStockOut.Tables[reqDt.TableName].Rows)
            {
                if (item.RowState == DataRowState.Deleted) continue;

                var eop = string.Empty;
                var dsEop = new DataSet();
                locat.GetList(dsEop, connStr);
                if (dsEop.Tables[locat.TableName].Rows.Count > 0)
                {
                    var dtEop = dsEop.Tables[locat.TableName].Select("LocationCode = '" + item["ToLocationCode"] + "'");
                    if (dtEop.Any())
                    {
                        eop = dtEop[0]["EOP"].ToString();
                    }
                }

                if (eop != "2") continue;
                var p = product.GetProductCategory(item["ProductCode"].ToString(), connStr);

                var adjCode = string.Empty;
                var dsAdj = new DataSet();
                adjType.GetList(dsAdj, "ISSUE", connStr);
                if (dsAdj.Tables[adjType.TableName].Rows.Count > 0)
                {
                    var dtAdj = dsAdj.Tables[adjType.TableName].Select("AdjId = " + ddl_Type.Value.ToString());
                    if (dtAdj.Any())
                    {
                        adjCode = dtAdj[0]["AdjCode"].ToString();
                    }
                }

                var s = "BusinessUnitCode = '" + LoginInfo.BuInfo.BuCode + "'";
                s += " and StoreCode = '" + ddl_Store.Value + "'";
                s += " and ItemGroupCode = '" + p + "'";
                s += " and A1 = '" + adjCode + "'";
                //s += " and A2 = '" + item["ToLocationCode"] + "'";
                s += " and PostType = '" + Blue.BL.Option.Admin.Interface.PostType.GL + "' ";


                var dt = accMapp.GetList(connStr);
                var drs = dt.Select(s).ToList();

                if (drs.Count > 0) continue;
                var dr = ds.Tables[accMapp.TableName].NewRow();
                dr["ID"] = Guid.NewGuid(); // accMapp.GetNewID(connStr);
                dr["BusinessUnitCode"] = LoginInfo.BuInfo.BuCode;
                dr["StoreCode"] = ddl_Store.Value.ToString();
                dr["ItemGroupCode"] = p;
                dr["A1"] = adjCode;
                //dr["A2"] = item["ToLocationCode"];
                dr["PostType"] = "GL";

                ds.Tables[accMapp.TableName].Rows.Add(dr);
            }

            var save = accMapp.Save(ds, connStr);

            if (save)
            {
                //Response.Write("SUCCESS");
            }
        }

        private void UpdateApprStatus(int workflowStep)
        {

            var s = string.Format("APP.WF_STORE_APPR_STEP_{0}", workflowStep);
            var dbParams = new Blue.DAL.DbParameter[2];
            dbParams[0] = new Blue.DAL.DbParameter("@DocNo", _dsReqEdit.Tables[reqDt.TableName].Rows[0]["DocumentId"].ToString());
            dbParams[1] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);
            workFlowDt.ExcecuteApprRule(s, dbParams, LoginInfo.ConnStr);

            //string apprRules = workFlowDt.GetApprRule(2, workflowStep, LoginInfo.ConnStr);

            // End Modified.
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        protected void btn_ReqDate_Ok_Click(object sender, EventArgs e)
        {
            foreach (DataRow drStoreReqEdit in _dsReqEdit.Tables[reqDt.TableName].Rows)
            {
                drStoreReqEdit["DeliveryDate"] = de_ReqDate.Date;
            }

            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.DataBind();
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            if (viewHandler.GetWFStep(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr) > 1)
            {
                for (var i = grd_StoreReqAppr.Rows.Count - 1; i >= 0; i--)
                {
                    var chkItem = grd_StoreReqAppr.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                    if (chkItem == null || !chkItem.Checked) continue;
                    var drStoreReqDt = _dsReqEdit.Tables[reqDt.TableName].Rows[i];
                    if (drStoreReqDt.RowState != DataRowState.Deleted)
                    {
                        drStoreReqDt.Delete();
                    }
                }

                grd_StoreReqAppr.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                grd_StoreReqAppr.EditIndex = -1;
                grd_StoreReqAppr.DataBind();
            }
            else
            {
                for (var i = grd_StoreReqEdit.Rows.Count - 1; i >= 0; i--)
                {
                    var chkItem = grd_StoreReqEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                    if (chkItem == null || !chkItem.Checked) continue;
                    var drStoreReqDt = _dsReqEdit.Tables[reqDt.TableName].Rows[i];

                    if (drStoreReqDt.RowState != DataRowState.Deleted)
                    {
                        drStoreReqDt.Delete();
                    }
                }

                grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                grd_StoreReqEdit.EditIndex = -1;
                grd_StoreReqEdit.DataBind();
            }

            if (grd_StoreReqEdit.Rows.Count == 0)
            {
                ddl_Store.Enabled = true;
                ddl_Type.Enabled = true;
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            Commit_Process();
        }

        #endregion

        #region
        // Added on: 13/11/2017. By: Fon
        // In Page Setting, look like only grd_StoreReqEdit can editable.
        protected bool Control_Location(string locationCode, DataTable dt)
        {
            bool productMatchLocate = true;
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            // Single Location.
            if (isSingle)
            {
                int rowIndex = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!IsSame_Location(dt, rowIndex, locationCode))
                    {
                        lbl_Warning_ProductLocate.Text = string.Format(@"Some products in list do not exist in this location.
                                <br/> Do you want to change to selected location? ");
                        productMatchLocate = false;
                        pop_Product_Location.ShowOnPageLoad = true;
                        return productMatchLocate;
                    }
                    rowIndex++;
                }
            }

            Session["dsStoreReqEdit"] = _dsReqEdit;
            if (productMatchLocate)
            {
                grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
                grd_StoreReqEdit.EditIndex = -1;
                grd_StoreReqEdit.DataBind();
            }
            return productMatchLocate;
        }

        protected bool IsSame_Location(DataTable dt, int rowIndex, string location)
        {
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                string oldLocation = string.Empty;
                if (rowIndex > 0 && rowIndex <= dt.Rows.Count)
                {
                    // Mean last detail.
                    oldLocation = dt.Rows[rowIndex - 1]["ToLocationCode"].ToString();
                }
                else if (rowIndex == 0 && dt.Rows.Count >= 1)
                {
                    oldLocation = dt.Rows[rowIndex + 1]["ToLocationCode"].ToString();
                }
                return (oldLocation == location) ? true : false;
            }
        }

        protected bool IsProduct_InThisLocation(string productCode, string locationCode)
        {
            var ddlProduct = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddlGStore = grd_StoreReqEdit.Rows[grd_StoreReqEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;
            if (ddlProduct == null || ddl_Store.Value == null || ddlGStore == null)
                return false;

            var ds = product.GetListByTwoLocation(
                ddl_Store.Value.ToString(),
                ddlGStore.Value.ToString(),
                LoginInfo.ConnStr);
            DataTable dt = (DataTable)ds;

            string strSelect = string.Format("[ProductCode] = '{0}'", productCode);
            DataRow[] drSelect = dt.Select(strSelect);
            return (drSelect.Length > 0) ? true : false;
        }

        protected void Clear_Product(DataTable dt, int rowIndex)
        {
            DataRow dr = dt.Rows[rowIndex];
            dr["CategoryCode"] = string.Empty;
            dr["ProductCode"] = string.Empty;

            dr["RequestQty"] = 0.00;
            dr["ApprQty"] = 0.00;
            dr["AllocateQty"] = 0.00;

            dr["RequestUnit"] = string.Empty;
            dr["DebitACCode"] = DBNull.Value;
            dr["CreditACCode"] = DBNull.Value;

            dr["Comment"] = string.Empty;
            dr["ApprStatus"] = string.Empty;

            Session["dsStoreReqEdit"] = _dsReqEdit;
        }

        protected void btn_Yes_popPL_Click(object sender, EventArgs e)
        {
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            int editIndex = grd_StoreReqEdit.EditIndex;
            ASPxComboBox ddlgStore = (ASPxComboBox)grd_StoreReqEdit.Rows[editIndex].FindControl("ddl_gStore");
            DataTable dt = _dsReqEdit.Tables[reqDt.TableName];
            string locationChanged = ddlgStore.Value.ToString();
            int rowIndex = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (isSingle)
                {
                    dr["ToLocationCode"] = locationChanged;
                    if (!IsProduct_InThisLocation(dr["ProductCode"].ToString(), dr["ToLocationCode"].ToString()))
                    {
                        txt_Desc.Text += string.Format("Product: {0}/ Location: {1}, ", dr["ProductCode"].ToString(), locationChanged);
                        Clear_Product(dt, rowIndex);
                    }
                }
                rowIndex++;
            }

            Session["dsStoreReqEdit"] = _dsReqEdit;
            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.EditIndex = -1;
            grd_StoreReqEdit.DataBind();

            pop_Product_Location.ShowOnPageLoad = false;
        }

        protected void btn_No_popPL_Click(object sender, EventArgs e)
        {
            int editIndex = grd_StoreReqEdit.EditIndex;
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            if (isSingle)
            {
                DataTable dt = _dsReqEdit.Tables[reqDt.TableName];
                string locate = string.Empty;

                if (editIndex > 0 && editIndex <= dt.Rows.Count)
                { locate = dt.Rows[editIndex - 1]["ToLocationCode"].ToString(); }
                else if (editIndex == 0 && dt.Rows.Count >= 1)
                { locate = dt.Rows[editIndex + 1]["ToLocationCode"].ToString(); }

                dt.Rows[editIndex]["ToLocationCode"] = locate;
                if (!IsProduct_InThisLocation(dt.Rows[editIndex]["ProductCode"].ToString()
                    , dt.Rows[editIndex]["ToLocationCode"].ToString()))
                { Clear_Product(dt, editIndex); }
            }

            grd_StoreReqEdit.DataSource = _dsReqEdit.Tables[reqDt.TableName];
            grd_StoreReqEdit.EditIndex = -1;
            grd_StoreReqEdit.DataBind();

            Session["dsStoreReqEdit"] = _dsReqEdit;
            pop_Product_Location.ShowOnPageLoad = false;
        }
        #endregion

        #region Save & Commit

        protected bool Save_Process()
        {
            #region -- Validate data --
            //Check request store location.
            if (ddl_Store.Value == null)
            {
                lbl_Warning.Text = @"Please select 'Request To Store'.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(250);
                return false;
            }

            //Check request Type.
            if (ddl_Type.Value == null)
            {
                lbl_Warning.Text = @"Please select 'Movement Type'.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(250);
                return false;
            }

            // Check any details when edit

            if (grd_StoreReqEdit.Visible && grd_StoreReqEdit.Rows.Count == 0)
            {
                lbl_Warning.Text = "No detail.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(200);
                return false;
            }


            // Check any details when approve
            if (grd_StoreReqAppr.Visible && grd_StoreReqAppr.Rows.Count == 0)
            {
                lbl_Warning.Text = "No detail.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(200);
                return false;
            }

            //Check Store Location. Store Location in header and detail must not same.
            foreach (DataRow drStoreReqDetail in _dsReqEdit.Tables[reqDt.TableName].Rows)
            {
                if (drStoreReqDetail.RowState == DataRowState.Deleted) continue;
                if (drStoreReqDetail["ToLocationCode"].ToString() != ddl_Store.Value.ToString()) continue;
                lbl_Warning.Text = @"Source and Destination Store could not be same. Please re-select";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(440);
                return false;
            }

            // Check product inactive
            foreach (DataRow drStoreReqDetail in _dsReqEdit.Tables[reqDt.TableName].Rows)
            {
                if (drStoreReqDetail.RowState == DataRowState.Deleted)
                    continue;

                string productCode = drStoreReqDetail["ProductCode"].ToString();
                DataTable dt = product.GetProdList(productCode, LoginInfo.ConnStr);
                if (dt.Rows.Count == 0)
                {
                    pop_Warning.ShowOnPageLoad = true;
                    lbl_Warning.Text = string.Format("Product '{0}' does not exist.", productCode);
                    return false;
                }
                else if (Convert.ToBoolean(dt.Rows[0]["IsActive"]) == false)
                {
                    pop_Warning.ShowOnPageLoad = true;
                    lbl_Warning.Text = string.Format("Product '{0}' is inactive.", productCode);
                    return false;
                }
            }

            #endregion

            string _action = string.Empty;
            string hdrNo = string.Empty;

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                _action = "CREATE";
                var drInserting = _dsReqEdit.Tables[ReqH.TableName].NewRow();

                hdrNo = ReqH.GetNewRequestCode(ServerDateTime, LoginInfo.ConnStr);
                drInserting["RequestCode"] = hdrNo;
                drInserting["AdjId"] = ddl_Type.Value.ToString();
                drInserting["LocationCode"] = ddl_Store.Value;
                drInserting["Description"] = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
                drInserting["DeliveryDate"] = de_Date.Date;
                drInserting["ProjectRef"] = (string)ddl_JobCode.Value == string.Empty ? null : ddl_JobCode.Value;
                drInserting["Status"] = true;
                drInserting["WFStep"] = 1;
                drInserting["ApprStatus"] = workflow.GetHdrApprStatus("IN", "SR", LoginInfo.ConnStr);
                drInserting["DocStatus"] = "In Process";
                drInserting["CreateBy"] = LoginInfo.LoginName;
                drInserting["CreateDate"] = ServerDateTime.Date;
                drInserting["UpdateBy"] = LoginInfo.LoginName;
                drInserting["UpdateDate"] = ServerDateTime.Date;
                drInserting["IsVoid"] = 0;

                _dsReqEdit.Tables[ReqH.TableName].Rows.Add(drInserting);
            }
            else
            {
                _action = "MODIFY";

                var drSave = _dsReqEdit.Tables[ReqH.TableName].Rows[0];

                if (Request.Params["MODE"].ToUpper() == "SR")
                {
                    drSave["RequestCode"] = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RequestCode"].ToString();
                    hdrNo = drSave["RequestCode"].ToString();
                }

                drSave["AdjId"] = ddl_Type.Value.ToString();
                drSave["LocationCode"] = ddl_Store.Value;
                drSave["Description"] = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
                drSave["ProjectRef"] = (string)ddl_JobCode.Value == string.Empty ? null : ddl_JobCode.Value;
                drSave["Status"] = true;

                if (Request.Params["MODE"].ToUpper() == "SR" /*|| int.Parse(drSave["WFStep"].ToString()) == 1*/)
                {
                    drSave["WFStep"] = 1;
                    drSave["ApprStatus"] = workflow.GetHdrApprStatus("IN", "SR", LoginInfo.ConnStr);
                }

                drSave["DocStatus"] = "In Process";
                drSave["UpdateBy"] = LoginInfo.LoginName;
                drSave["UpdateDate"] = ServerDateTime.Date;
                drSave["IsVoid"] = 0;

                var dtWf = workflow.DbExecuteQuery("SELECT StepNo FROM [APP].WF WHERE WfId=2", null, LoginInfo.ConnStr);
                var lastStepNo = int.Parse(dtWf.Rows[0][0].ToString());

                foreach (DataRow drStoreReqDt in _dsReqEdit.Tables[reqDt.TableName].Rows)
                {
                    if (drStoreReqDt.RowState == DataRowState.Deleted)
                        continue;

                    if (WfStep <= 1)
                    {
                        drStoreReqDt["ApprQty"] = drStoreReqDt["RequestQty"];
                        drStoreReqDt["AllocateQty"] = drStoreReqDt["RequestQty"];
                    }
                    // else if (WfStep < lastStepNo)
                        // drStoreReqDt["AllocateQty"] = drStoreReqDt["ApprQty"];

                }


            }

            var save = ReqH.Save(_dsReqEdit, LoginInfo.ConnStr);

            if (!save)
                return false;


            if (Request.Params["MODE"].ToUpper() == "NEW" || Request.Params["MODE"].ToUpper() == "SR")
            {
                var refId = ReqH.GetLastID(LoginInfo.ConnStr);

                foreach (DataRow drStoreReqDt in _dsReqEdit.Tables[reqDt.TableName].Rows)
                {
                    drStoreReqDt["DocumentId"] = refId;
                }

                var dt = ReqH.DbExecuteQuery("SELECT RequestCode FROM [IN].StoreRequisition WHERE RefId = " + refId.ToString(), null, LoginInfo.ConnStr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var drSave = _dsReqEdit.Tables[ReqH.TableName].Rows[0];
                    drSave["RequestCode"] = dt.Rows[0]["RequestCode"].ToString();

                    drSave.AcceptChanges();
                }
            }

            var saveStoreReqDt = reqDt.Save(_dsReqEdit, LoginInfo.ConnStr);

            if (!saveStoreReqDt)
                return false;

            _transLog.Save("IN", "SR", hdrNo, _action, string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

            return true;
        }

        protected void Commit_Process()
        {
            if (WfStepCount == 1) // No workflow, only single step
            {
                if (CheckOnHand() && Save_Process())
                {
                    string requestCode = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RequestCode"].ToString();

                    UpdateApprStatus(WfStep);
                    var stepFrom = 1;
                    SendEmailWorkflow.Sen_SR_Approve(requestCode, stepFrom, LoginInfo.LoginName, LoginInfo.ConnStr);

                    _transLog.Save("IN", "SR", requestCode, "COMMIT", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);
                    Response.Redirect("StoreReqDt.aspx?ID=" + _dsReqEdit.Tables[reqDt.TableName].Rows[0]["DocumentId"] + "&VID=" + Request.Params["VID"] + "&BuCode=" + LoginInfo.BuInfo.BuCode);
                }
            }
            else
            {
                if (Save_Process())
                {
                    string requestCode = _dsReqEdit.Tables[ReqH.TableName].Rows[0]["RequestCode"].ToString();

                    UpdateApprStatus(WfStep);

                    var stepFrom = 1;
                    SendEmailWorkflow.Sen_SR_Approve(requestCode, stepFrom, LoginInfo.LoginName, LoginInfo.ConnStr);

                    _transLog.Save("IN", "SR", requestCode, "COMMIT", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);
                    Response.Redirect("StoreReqDt.aspx?ID=" + _dsReqEdit.Tables[reqDt.TableName].Rows[0]["DocumentId"] + "&VID=" + Request.Params["VID"] + "&BuCode=" + LoginInfo.BuInfo.BuCode);
                }

            }



        }

        private bool IssueOrTransferItems(string requestCode)
        {
            var commands = new StringBuilder();
            var refId = _dsReqEdit.Tables[reqDt.TableName].Rows[0]["RefId"].ToString();

            var dtSrDt = ReqH.DbExecuteQuery(string.Format("SELECT * FROM [IN].StoreRequisitionDetail WHERE DocumentId = '{0}'", refId), null, LoginInfo.ConnStr);
            if (dtSrDt != null && dtSrDt.Rows.Count > 0)
            {

                var moveType = ddl_Type.Value.ToString();
                var frLoc = ddl_Store.Value.ToString();
                var reqDate = de_ReqDate.Date;

                //foreach (DataRow dr in _dsReqEdit.Tables[reqDt.TableName].Rows)
                foreach (DataRow dr in dtSrDt.Rows)
                {
                    var dtNo = Convert.ToInt32(dr["RefId"].ToString());
                    var toLoc = dr["ToLocationCode"].ToString();
                    var prod = dr["ProductCode"].ToString();
                    var unit = dr["RequestUnit"].ToString();
                    var qty = Convert.ToDecimal(dr["RequestQty"].ToString());

                    var remain = 0m;

                    var sql = string.Format("SELECT ISNULL(SUM([IN]-[OUT]),0) as remain FROM [IN].Inventory WHERE Location='{0}' AND ProductCode = '{1}' AND CommittedDate <= '{2}'", frLoc, prod, reqDate.ToString("yyyy-MM-dd"));
                    var dt = workflow.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        remain = Convert.ToDecimal(dt.Rows[0][0].ToString());
                    }

                    if (qty <= remain)
                    {
                        var outQty = qty;
                        // EXECUTE [IN].[InsertInventorySR] @DocNo(RefId), @DocDtNo, @LocationCode, @ProductCode, @QtyOut, @IssueDate
                        sql = string.Format("EXECUTE [IN].[InsertInventorySR] '{0}', '{1}', '{2}', '{3}', {4}, '{5}'",
                            refId,
                            dtNo.ToString(),
                            frLoc,
                            prod,
                            outQty.ToString(),
                            reqDate.ToString("yyyy-MM-dd")
                            );
                        commands.AppendLine(sql);

                    }
                    else
                    {
                        lbl_Warning.Text = string.Format("Not enough quantity, '{0}' is requested {1} but available {2}.", prod, string.Format(DefaultQtyFmt, qty), string.Format(DefaultQtyFmt, remain));
                        pop_Warning.ShowOnPageLoad = true;
                        return false;
                    }

                    ReqH.DbExecuteQuery(commands.ToString(), null, LoginInfo.ConnStr);
                    sql = string.Format("UPDATE [IN].StoreRequisition SET DocStatus='Complete', ApprStatus='A', UpdateDate='{0}', UpdateBy='{1}' WHERE RefId = {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), LoginInfo.LoginName, refId);
                    ReqH.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                }

                return true;

            }
            else
                return false;
        }

        private bool CheckOnHand()
        {
            var moveType = ddl_Type.Value.ToString();
            var frLoc = ddl_Store.Value.ToString();
            var reqDate = de_ReqDate.Date;

            foreach (DataRow dr in _dsReqEdit.Tables[reqDt.TableName].Rows)
            {
                var dtNo = Convert.ToInt32(dr["RefId"].ToString());
                var toLoc = dr["ToLocationCode"].ToString();
                var prod = dr["ProductCode"].ToString();
                var unit = dr["RequestUnit"].ToString();
                var qty = Convert.ToDecimal(dr["RequestQty"].ToString());

                var remain = 0m;

                var sql = string.Format("SELECT ISNULL(SUM([IN]-[OUT]),0) as remain FROM [IN].Inventory WHERE Location='{0}' AND ProductCode = '{1}' AND CommittedDate <= '{2}'", frLoc, prod, reqDate.ToString("yyyy-MM-dd"));
                var dt = workflow.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
                if (dt != null && dt.Rows.Count > 0)
                {
                    remain = Convert.ToDecimal(dt.Rows[0][0].ToString());
                }

                if (qty > remain)
                {
                    lbl_Warning.Text = string.Format("Not enough quantity, '{0}' is requested {1} but available {2}.", prod, string.Format(DefaultQtyFmt, qty), string.Format(DefaultQtyFmt, remain));
                    pop_Warning.ShowOnPageLoad = true;
                    return false;
                }
            }

            return true;
        }



        #endregion
    }
}