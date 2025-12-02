using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using Blue.BL;


namespace BlueLedger.PL.UserControls
{
    public partial class WFControl : BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.APP.ViewHandler _vh = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.WF _workFlow = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt _workFlowDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.PC.PR.PRDt _prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product _prod = new Blue.BL.Option.Inventory.Product();

        private string _bu = string.Empty;
        private string _columnName = string.Empty;
        private string _connStr = string.Empty;
        private string _controlID = string.Empty;

        private bool _countAppr;
        private string[] _dbParam;

        private DataSet _ds;
        private string _redirectTarget = string.Empty;
        private int _reject;
        private string _tableDtSchema = string.Empty;
        private string _tableSchema = string.Empty;
        private string _viewNo = string.Empty;
        private int _wfId;
        private int _wfStep;
        private string _module;
        private string _subModule;
        private string _refNo;


        public string ConnStr
        {
            get { return ViewState["ConnStr"] == null ? _connStr : ViewState["ConnStr"].ToString(); }
            set
            {
                _connStr = value;
                ViewState["ConnStr"] = _connStr;
            }
        }

        public string Bu
        {
            get { return ViewState["Bu"] == null ? _bu : ViewState["Bu"].ToString(); }
            set
            {
                _bu = value;
                ViewState["Bu"] = _bu;
            }
        }

        public string ColumnName
        {
            get { return ViewState["ColumnName"] == null ? _columnName : ViewState["ColumnName"].ToString(); }
            set
            {
                _columnName = value;
                ViewState["ColumnName"] = _columnName;
            }
        }

        public string[] DBParam
        {
            get { return ViewState["DBParam"] == null ? _dbParam : (string[])ViewState["DBParam"]; }
            set
            {
                _dbParam = value;
                ViewState["DBParam"] = _dbParam;
            }
        }

        public string ControlID
        {
            get { return ViewState["ControlID"] == null ? _controlID : ViewState["ControlID"].ToString(); }
            set
            {
                _controlID = value;
                ViewState["ControlID"] = _controlID;
            }
        }

        public string RedirectTarget
        {
            get
            {
                return ViewState["RedirectTarget"] == null ? _redirectTarget : ViewState["RedirectTarget"].ToString();
            }
            set
            {
                _redirectTarget = value;
                ViewState["RedirectTarget"] = _redirectTarget;
            }
        }

        public string TableSchema
        {
            get { return ViewState["TableSchema"] == null ? _tableSchema : ViewState["TableSchema"].ToString(); }
            set
            {
                _tableSchema = value;
                ViewState["TableSchema"] = _tableSchema;
            }
        }

        public string TableDtSchema
        {
            get { return ViewState["TableDtSchema"] == null ? _tableDtSchema : ViewState["TableDtSchema"].ToString(); }
            set
            {
                _tableDtSchema = value;
                ViewState["TableDtSchema"] = _tableDtSchema;
            }
        }

        public DataSet Ds
        {
            get { return ViewState["Ds"] == null ? _ds : (DataSet)ViewState["Ds"]; }
            set
            {
                _ds = value;
                ViewState["Ds"] = _ds;


                DataColumnCollection columns = Ds.Tables[TableSchema].Columns;
                if (columns.Contains("RequestCode"))
                    _refNo = Ds.Tables[TableSchema].Rows[0]["RequestCode"].ToString();
                else if (columns.Contains("PrNo"))
                    _refNo = Ds.Tables[TableSchema].Rows[0]["PrNo"].ToString();
                else
                    _refNo = string.Empty;
            }
        }

        public int WfStep
        {
            get { return ViewState["WFStep"] == null ? _wfStep : int.Parse(ViewState["WFStep"].ToString()); }
            set
            {
                _wfStep = value;
                ViewState["WFStep"] = _wfStep;
            }
        }

        public int Reject
        {
            get { return ViewState["Reject"] == null ? _reject : int.Parse(ViewState["Reject"].ToString()); }

            set
            {
                _reject = value;
                ViewState["Reject"] = _reject;
            }
        }

        public int WFId
        {
            get { return ViewState["WFId"] == null ? _wfId : int.Parse(ViewState["WFId"].ToString()); }
            set
            {
                _wfId = value;
                ViewState["WFId"] = _wfId;
            }
        }

        public string Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

        public string SubModule
        {
            get
            {
                return _subModule;
            }
            set
            {
                _subModule = value;
            }
        }


        public bool CountAppr
        {
            get { return ViewState["CountAppr"] == null ? _countAppr : bool.Parse(ViewState["CountAppr"].ToString()); }
            set
            {
                _countAppr = value;
                ViewState["CountAppr"] = _countAppr;
            }
        }

        //public int[] AutoApprStep
        //{
        //    get
        //    {
        //        if (ViewState["AutoApprStep"] == null)
        //        {
        //            return this._autoApprStep;
        //        }
        //        return (int[])ViewState["AutoApprStep"];
        //    }
        //    set
        //    {
        //        this._autoApprStep = value;
        //        ViewState["AutoApprStep"] = this._autoApprStep;
        //    }
        //}

        public string ViewNo
        {
            get { return ViewState["ViewNo"] == null ? _viewNo : ViewState["ViewNo"].ToString(); }
            set
            {
                _viewNo = value;
                ViewState["ViewNo"] = _viewNo;
            }
        }

        #endregion

        // ------------------------------------------------------------------------------------------------------

        #region "Operation"

        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        public override void DataBind()
        {
            base.DataBind();
            Page_Retrieve();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Retrieve()
        {
            Page_Setting();
        }

        protected void Page_Setting()
        {
            Visible = CheckWFenable();
        }

        //private DateTime GetEndDateTimeOfOpenPeriod()
        //{
        //    SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
        //    conn.Open();

        //    string sql;
        //    sql = "SELECT DATEADD(DAY, DATEDIFF(DAY, 1, CAST(EndDate  AS DATE)), '23:55:00:000') as OpenPeriod ";
        //    sql = sql + " FROM [IN].Period";
        //    sql = sql + " WHERE IsClose='false' or IsClose IS NULL";
        //    sql = sql + " ORDER BY [Year] ASC, PeriodNumber ASC";

        //    var cmd = new SqlCommand(sql, conn);

        //    cmd.ExecuteScalar();

        //    SqlDataReader reader = cmd.ExecuteReader();
        //    reader.Read();

        //    return DateTime.Parse(reader["OpenPeriod"].ToString());

        //}



        protected void btn_Appr_Click(object sender, EventArgs e)
        {
            //ASPxGridView grd_PrDt = (ASPxGridView)this.Parent.FindControl(this.ControlID.ToString());
            //List<object> columnValues = grd_PrDt.GetSelectedFieldValues(this.ColumnName.ToString());


            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0)
            {
                // return;
                pop_ReqVendor.ShowOnPageLoad = true;
                Lb_WarningInfo.Text = @"Please select item to Approve";
            }

            pop_ConfirmApprove.ShowOnPageLoad = false;


            var dtWf = _workFlow.DbExecuteQuery("SELECT StepNo FROM APP.WF WHERE WFId=2", null, LoginInfo.ConnStr);
            var lastStep = dtWf != null && dtWf.Rows.Count > 0 ? Convert.ToInt32(dtWf.Rows[0][0]) : 0;

            if (WfStep == lastStep) // Check Period Date for CommittedDate (Before Issue)
            {
                var startOfOpeningPeriod = DateTime.Today.Date;
                var endOfOpeningPeriod = DateTime.Today.Date;

                var dtPeriod = _workFlow.DbExecuteQuery("SELECT TOP(1) StartDate, EndDate FROM [IN].[Period] WHERE IsClose=0 ORDER BY [StartDate]", null, LoginInfo.ConnStr);


                if (dtPeriod != null && dtPeriod.Rows.Count > 0)
                {
                    startOfOpeningPeriod = Convert.ToDateTime(dtPeriod.Rows[0]["StartDate"]).Date;
                    endOfOpeningPeriod = Convert.ToDateTime(dtPeriod.Rows[0]["EndDate"]).Date;

                    if (DateTime.Today.Date > endOfOpeningPeriod)
                    {
                        var message = "<b>Period is opening from {0} to {1}</b>.<br/><br/>";

                        message += "Issue is able to process only in the opening period.<br/>";
                        message += "Please closed period before issued this document.<br/>";
 
                        lbl_Warning.Text = string.Format(message, startOfOpeningPeriod.ToString("dd/MM/yyyy"), endOfOpeningPeriod.ToString("dd/MM/yyyy"));
                        pop_Warning.ShowOnPageLoad = true;
                        
                        return;
                    }

                }
                else
                {
                    lbl_Warning.Text = "Period not set.";
                    pop_Warning.ShowOnPageLoad = true;
                }




                //if (TableSchema == "StoreRequisition" && WfStep == 3) // Check Period Date for CommittedDate (Before Issue)
                //{
                //    DateTime OpenPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr).AddHours(23).AddMinutes(57);
                //    DateTime InvCommittedDate;
                //    DateTime DocDate = DateTime.Parse(Ds.Tables[TableSchema].Rows[0]["DeliveryDate"].ToString());

                //    if (DateTime.Today > OpenPeriod)  // Over than open period (DateTime)
                //    {
                //        if (DocDate.Date <= OpenPeriod.Date)
                //            InvCommittedDate = OpenPeriod;
                //        else
                //            InvCommittedDate = DateTime.Today;
                //    }
                //    else // In period
                //        InvCommittedDate = DateTime.Today;

                //    if (DateTime.Today > OpenPeriod && DocDate.Date > OpenPeriod.Date)
                //    {
                //        pop_ConfirmApprove.ShowOnPageLoad = false;
                //        pop_Warning.ShowOnPageLoad = true;
                //        lbl_Warning.Text = "This document is not allowed to issue.<br><br>The document is created out of opening period. (@" + DocDate.Date.ToShortDateString() + ") <br><br>Please closing period before issue this document.";
                //    }

                //    // Check partial issue
                //    bool allowPartial = true;
                //    DataTable dt = Ds.Tables[TableDtSchema];

                //    if (!allowPartial)
                //    {
                //        // check all detail must be selected
                //        bool selectedAll = true;

                //        var grdDt = Parent.FindControl(ControlID) as GridView;

                //        if (grdDt != null)
                //            foreach (GridViewRow grdRow in grdDt.Rows)
                //            {
                //                var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                //                if (!chkItem.Checked && !dt.Rows[grdRow.RowIndex]["ApprStatus"].ToString().Contains('R'))
                //                {
                //                    selectedAll = false;
                //                    break;
                //                }
                //            }

                //        if (!selectedAll)
                //        {
                //            pop_ConfirmApprove.ShowOnPageLoad = false;
                //            pop_Warning.ShowOnPageLoad = true;
                //            lbl_Warning.Text = "This document is not allowed partial issue. Please select all items.";
                //        }

                //    }


            }

            pop_ConfirmApprove.ShowOnPageLoad = true;



        }

        protected void btn_ConfirmApprove_Click(object sender, EventArgs e)
        {
            int wfStepCount;
            DataSet ds = new DataSet();
            _workFlow.Get(ds, "IN", "SR", LoginInfo.ConnStr);
            wfStepCount = Convert.ToInt32(ds.Tables[0].Rows[0]["StepNo"]);

            pop_ConfirmApprove.ShowOnPageLoad = false;

            lbl_WarningApprQty.Text = string.Empty;

            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0)
            {
                pop_ConfirmApprove.ShowOnPageLoad = false;
                return;
            }


            for (var i = 0; i < columnValues.Count; i++)
            {
                if (Ds.Tables[_prDt.TableName] != null)
                {
                    Ds.Tables[_prDt.TableName].Clear();
                }

                for (var j = 0; j < Ds.Tables[TableDtSchema].Rows.Count; j++)
                {
                    var dtSchema = Ds.Tables[TableDtSchema].Rows;
                    if (dtSchema[j]["RefId"].ToString() != columnValues[i].ToString()) continue;

                    string sDate;
                    try
                    {
                        sDate = Ds.Tables[TableSchema].Rows[0]["PRDate"].ToString();
                    }
                    catch
                    {
                        try
                        {
                            sDate = Ds.Tables[TableSchema].Rows[0]["CreatedDate"].ToString();
                        }
                        catch
                        {
                            try
                            {
                                //sDate = Ds.Tables[TableSchema].Rows[0]["CreateDate"].ToString();
                                sDate = DateTime.Today.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }

                    // Check Onhand
                    if (WfStep < wfStepCount) continue;

                    var get = _prDt.GetStockSummary(Ds, dtSchema[j]["ProductCode"].ToString()
                        , Ds.Tables[TableSchema].Rows[0]["LocationCode"].ToString(), sDate, ConnStr);
                    var onHand = decimal.Parse(Ds.Tables[_prDt.TableName].Rows[0]["OnHand"].ToString() == string.Empty
                        ? "0"
                        : Ds.Tables[_prDt.TableName].Rows[0]["OnHand"].ToString());

                    // Check null Approve qty
                    if (string.IsNullOrEmpty(dtSchema[j]["allocateQty"].ToString()))
                    {
                        if (string.IsNullOrEmpty(dtSchema[j]["ApprQty"].ToString()))
                        {
                            dtSchema[j]["allocateQty"] = dtSchema[j]["RequestQty"].ToString();
                            dtSchema[j]["ApprQty"] = dtSchema[j]["RequestQty"].ToString();
                        }
                        else
                        {
                            dtSchema[j]["allocateQty"] = dtSchema[j]["ApprQty"].ToString();
                        }
                    }

                    var qty = decimal.Parse(dtSchema[j]["allocateQty"].ToString());

                    if (onHand - qty >= 0) continue;
                    lbl_Warning.Text = string.Format(@"Approve quantity of product {0} : {1} : {2}  must be less than on hand."
                            , dtSchema[j]["ProductCode"]
                            , _prod.GetName(dtSchema[j]["ProductCode"].ToString(), ConnStr)
                            , _prod.GetName2(dtSchema[j]["ProductCode"].ToString(), ConnStr));
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (lbl_WarningApprQty.Text == string.Empty)
            {
                ExcAppr(columnValues);
            }
            else
            {
                lbl_WarningApprQty.Text = lbl_WarningApprQty.Text +
                                          @"<br>These product cannot approve all request quantity.<br>Do you want to approve max quantity <br>that you can approve?";
                pop_WarningApprQty.Width = Unit.Pixel(350);
                pop_WarningApprQty.ShowOnPageLoad = true;
            }
        }

        protected void btn_ApprQty_Yes_Click(object sender, EventArgs e)
        {
            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();


            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            ExcAppr(columnValues);
        }

        protected void btn_ApprQty_No_Click(object sender, EventArgs e)
        {
            pop_WarningApprQty.ShowOnPageLoad = false;
            pop_ConfirmApprove.ShowOnPageLoad = false;
        }

        private void ExcAppr(List<object> columnValues)
        {
            var approve = 0;
            var notApprove = 0;

            var grdPrDt = Parent.FindControl(ControlID) as GridView;

            //foreach (int prDtNo in columnValues)
            for (var index = 0; index < columnValues.Count; index++)
            {
                if (CountAppr)
                {
                    //if (grd_PrDt.GetRowValuesByKeyValue(prDtNo, "ApprStatus").ToString().Contains('R'))
                    if (grdPrDt != null && grdPrDt.Rows[index].Cells[grdPrDt.Columns.Count - 1].Text.Contains('R'))
                    {
                        continue;
                    }

                    if (grdPrDt != null &&
                        !grdPrDt.Rows[index].Cells[grdPrDt.Columns.Count - 1].Text.Substring(WfStep * 10, 10)
                            .Contains('_'))
                    {
                        continue;
                    }

                    if (Ds.Tables[TableDtSchema].Rows[index]["VendorCode"].ToString() == "")
                    {
                        notApprove++;
                        continue;
                    }
                    approve++;

                    //if (!grd_PrDt.GetRowValuesByKeyValue(prDtNo, "ApprStatus").ToString().Substring(WFStep * 10, 10).Contains('_'))
                    //if (grd_PrDt.GetRowValuesByKeyValue(prDtNo, "VendorCode").ToString() == "")
                }

                ExecuteApprove(columnValues[index].ToString(), WfStep);

                // *************************** ไม่รู้ว่าเอาไว้ทำอะไร ****************************
                //if (AutoApprStep == null)
                //{
                //    if (Bu != string.Empty)
                //    {
                //        if (bu.IsHQ(Bu))
                //        {
                //            for (int i = 0; i < AutoApprStep.Length; i++)
                //            {
                //                this.ExecuteApprove(columnValues[index].ToString(), int.Parse(AutoApprStep[i].ToString()));
                //            }
                //        }
                //    }
                //}
            }

            if (approve == 0 && notApprove > 0)
            {
                lbl_Approve_Chk.Text = @"No item approved.";
            }
            else
            {
                if (approve > 0 && notApprove == 0)
                {
                    lbl_Approve_Chk.Text = @"Approved item(s) are successful";
                }
                else if (approve > 0 && notApprove > 0)
                {
                    lbl_Approve_Chk.Text = string.Format("{0} item(s) are approved.<br> {1} item(s) are not approved.", approve,
                        notApprove);
                }
                else
                {
                    lbl_Approve_Chk.Text = @"Approved item(s) are successful";
                }

                _transLog.Save(_module, _subModule, _refNo, "APPROVE", string.Empty, LoginInfo.LoginName, ConnStr);
            }

            pop_ConfirmApprove.ShowOnPageLoad = false;
            //pop_Approve.ShowOnPageLoad = true;

            Page_Retrieve();
            pop_Approve.ShowOnPageLoad = true;
        }

        protected void btn_CancelApprove_Click(object sender, EventArgs e)
        {
            pop_ConfirmApprove.ShowOnPageLoad = false;
            pop_Approve.ShowOnPageLoad = false;
        }

        protected void btn_OK_PopApprove_Click(object sender, EventArgs e)
        {
            Response.Redirect(RedirectTarget);
        }

        protected void btn_ConfirmReject_Click(object sender, EventArgs e)
        {

            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0)
            {
                return;
            }

            var sbPrDtNo = new StringBuilder();
            var rejRule = _workFlowDt.GetRejRule(WFId, WfStep, ConnStr);

            string setDtNo = string.Empty;
            string refId = string.Empty;

            for (var index = 0; index < columnValues.Count; index++)
            {
                //if (grd_PrDt.GetRowValuesByKeyValue(prDtNo, "ApprStatus").ToString().Contains('R'))
                //if (grd_PrDt.Rows[prDtNo].Cells[grd_PrDt.Columns.Count - 1].Text.Contains('R'))
                if (Ds.Tables[TableDtSchema].Rows[index]["ApprStatus"].ToString().Contains('R'))
                {
                    Reject++;
                    // continue;
                }

                var step = (WfStep - 1) * 10;
                if (Ds.Tables[TableDtSchema].Rows[index]["ApprStatus"].ToString().Substring(step).StartsWith("_"))
                {
                    // continue;
                    Reject++;
                }
                //sbPrDtNo.Append((sbPrDtNo.Length > 0 ? ", " + prDtNo.ToString() + " " : prDtNo.ToString()));
                string docNo = Ds.Tables[TableSchema].Rows[0][ColumnName].ToString();
                string docDtNo = columnValues[index].ToString();

                var dbParams = new Blue.DAL.DbParameter[5];
                dbParams[0] = new Blue.DAL.DbParameter(DBParam[0], docNo);
                dbParams[1] = new Blue.DAL.DbParameter(DBParam[1], docDtNo);
                dbParams[2] = new Blue.DAL.DbParameter("@Step", WfStep.ToString());
                dbParams[3] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);
                dbParams[4] = new Blue.DAL.DbParameter("@Comment", txt_RejectMessage.Text.Trim());

                if (rejRule != string.Empty)
                {
                    refId = docNo;
                    setDtNo += docDtNo + ",";

                    _workFlowDt.ExecuteRule(rejRule, dbParams, ConnStr);
                }
                else
                {
                    break;
                }
            }

            setDtNo = setDtNo.Remove(setDtNo.Length - 1);

            SendMail_Reject(refId, setDtNo);

            pop_ConfirmReject.ShowOnPageLoad = false;

            lbl_RejectSuccess.Text = @"Reject Successfully ";
            pop_RejectSuccess.ShowOnPageLoad = true;

            _transLog.Save("IN", "SR", _refNo, "REJECT", string.Empty, LoginInfo.LoginName, ConnStr);

        }

        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            //ASPxGridView grd_PrDt = (ASPxGridView)this.Parent.FindControl(this.ControlID.ToString());
            //List<object> columnValues = grd_PrDt.GetSelectedFieldValues(this.ColumnName.ToString());

            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0)
            {
                // return;
                //pop_ReqVendor.ShowOnPageLoad = true;
                //Lb_WarningInfo.Text = @"No any item is selected.";
                lbl_Warning.Text = @"No any item is selected.";
                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                pop_ConfirmReject.ShowOnPageLoad = true;
                //pop_Reject.ShowOnPageLoad = true;
            }
        }

        protected void btn_SendBack_Click(object sender, EventArgs e)
        {
            //ASPxGridView grd_PrDt = (ASPxGridView)this.Parent.FindControl(this.ControlID.ToString());
            //List<object> columnValues = grd_PrDt.GetSelectedFieldValues(this.ColumnName.ToString());

            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0)
            {
                // return;
                pop_ReqVendor.ShowOnPageLoad = true;
                Lb_WarningInfo.Text = @"No any item is selected.";
            }
            else
            {
                pop_SendBack.ShowOnPageLoad = true;
            }
        }

        protected void btn_CancelReject_Click(object sender, EventArgs e)
        {
            pop_ConfirmReject.ShowOnPageLoad = false;
            //pop_Reject.ShowOnPageLoad = false;
        }

        protected void ddl_SendBack_Load(object sender, EventArgs e)
        {
            ddl_SendBack.DataSource = _workFlowDt.GetSandBackStepList(WFId, WfStep, 0, ConnStr);
            ddl_SendBack.DataBind();
        }

        protected void btn_Reject_OK_Click(object sender, EventArgs e)
        {
            pop_ConfirmReject.ShowOnPageLoad = true;
        }

        protected void btn_Reject_Cancel_Click(object sender, EventArgs e)
        {
        }

        protected void btn_SandBack_OK_Click(object sender, EventArgs e)
        {
            if (ddl_SendBack.SelectedIndex >= 0)
            {
                pop_ConfirmSendback.ShowOnPageLoad = true;
                lbl_Sendback.Text = @"Confirm to send back to " + ddl_SendBack.Text;
            }
            else
            {
                lbl_Warning.Text = "Please select step to send back.";
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        protected void btn_ConfirmSendback_Click(object sender, EventArgs e)
        {
            var grdPrDt = Parent.FindControl(ControlID) as GridView;
            var columnValues = new List<object>();

            if (grdPrDt != null)
                foreach (GridViewRow grdRow in grdPrDt.Rows)
                {
                    var chkItem = grdRow.FindControl("chk_Item") as CheckBox;

                    if (chkItem != null && chkItem.Checked)
                    {
                        columnValues.Add(Ds.Tables[TableDtSchema].Rows[grdRow.RowIndex][ColumnName]);
                    }
                }

            if (columnValues.Count == 0) return;
            if (ddl_SendBack.Value == null) return;

            var sbPrDtNo = new StringBuilder();

            var sendBkRule = _workFlowDt.GetSendBkRule(WFId, int.Parse(ddl_SendBack.Value.ToString()), ConnStr);

            foreach (int prDtNo in columnValues)
            {
                var step = (WfStep - 1) * 10;
                var dbParams = new Blue.DAL.DbParameter[6];
                dbParams[0] = new Blue.DAL.DbParameter(DBParam[0], Ds.Tables[TableSchema].Rows[0][ColumnName].ToString());
                dbParams[1] = new Blue.DAL.DbParameter(DBParam[1], prDtNo.ToString());
                dbParams[2] = new Blue.DAL.DbParameter("@CurrentStep", WfStep.ToString());
                dbParams[3] = new Blue.DAL.DbParameter("@Step", ddl_SendBack.Value.ToString());
                dbParams[4] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);
                dbParams[5] = new Blue.DAL.DbParameter("@Comment", txt_SendBackMessage.Text.Trim());

                if (sendBkRule != "")
                {
                    _workFlowDt.ExecuteRule(sendBkRule, dbParams, ConnStr);
                }
            }
            //workFlowDt.ExcecuteApprRule(StorePrefix.ToString() + "_SENDBACK", dbParams, ConnStr);

            pop_SendBack.ShowOnPageLoad = false;
            pop_ConfirmSendback.ShowOnPageLoad = false;
            // this.Page_Retrieve();
            pop_SendBackSuccess.ShowOnPageLoad = true;

            _transLog.Save("IN", "SR", _refNo, "SENDBACK", string.Empty, LoginInfo.LoginName, ConnStr);


            //string CurrentURL = Request.Url.AbsoluteUri;
            //Response.Redirect(CurrentURL);
            //this.Page_Retrieve();
        }

        protected void btn_CancelSendback_Click(object sender, EventArgs e)
        {
            pop_ConfirmSendback.ShowOnPageLoad = false;
        }

        private void ExecuteApprove(string prDrNo, int step)
        {
            var apprRule = _workFlowDt.GetApprRule(WFId, step, ConnStr);
            var dbParams = new Blue.DAL.DbParameter[3];
            dbParams[0] = new Blue.DAL.DbParameter(DBParam[0], Ds.Tables[TableSchema].Rows[0][ColumnName].ToString());
            dbParams[1] = new Blue.DAL.DbParameter(DBParam[1], prDrNo);
            dbParams[2] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

            if (apprRule != "")
            {
                _workFlowDt.ExecuteRule(apprRule, dbParams, ConnStr);
            }

            /* DbParameter[] dbParams = new DbParameter[3];
             dbParams[0] = new DbParameter(this.DBParam[0], this.Ds.Tables[pr.TableName].Rows[0][ColumnName.ToString()].ToString());
             dbParams[1] = new DbParameter(this.DBParam[1], PrDrNo.ToString());
             dbParams[2] = new DbParameter("@LoginName", LoginInfo.LoginName);
             workFlowDt.ExcecuteApprRule(StorePrefix.ToString() + "_APPR_STEP_" + Step.ToString(), dbParams, ConnStr);*/
        }

        private bool CheckWFenable()
        {
            return _vh.GetIsWFEnable(int.Parse(ViewNo), ConnStr) && _workFlowDt.GetAppr(WFId, WfStep, ConnStr) > 0;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_RejectSuccess.ShowOnPageLoad = false;

            if (Reject == Ds.Tables[TableDtSchema].Rows.Count)
            {
                Response.Redirect(RedirectTarget);
            }
            else
            {
                var currentUrl = Request.Url.AbsoluteUri;
                Response.Redirect(currentUrl);
                //this.Page_Retrieve();
            }
        }

        protected void btn_SendBackOk_Click(object sender, EventArgs e)
        {
            pop_SendBackSuccess.ShowOnPageLoad = false;
            Response.Redirect(RedirectTarget);
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        #endregion

        protected string SendMail(string fromMail, string toMail, string ccMail, string subject, string body)
        {
            string errorMessage = string.Empty;

            #region Email Calss

            Mail email = new Mail();

            string encryptSMTP = config.GetValue("SYS", "Mail", "ServerString", LoginInfo.ConnStr);
            KeyValues smtpConfig = new KeyValues();
            smtpConfig.Text = GnxLib.EnDecryptString(encryptSMTP, GnxLib.EnDeCryptor.DeCrypt);


            bool isAuth = smtpConfig.Value("authenticate").ToUpper() == "TRUE";

            email.SmtpServer = smtpConfig.Value("smtp");
            email.Port = Convert.ToInt16(smtpConfig.Value("port"));
            email.EnableSsl = smtpConfig.Value("enablessl").ToUpper() == "TRUE";
            email.IsAuthentication = isAuth;
            if (isAuth)
            {
                email.Name = smtpConfig.Value("name");
                email.UserName = smtpConfig.Value("username");
                email.Password = smtpConfig.Value("password");
            }

            email.From = fromMail;
            email.To = toMail;
            email.CC = ccMail;
            email.Subject = subject;
            email.Body = body;

            return email.Send();

            #endregion

        }

        private string GetEmailBody(string refId, string dtNo, string Title)
        {
            string html = string.Empty;


            return html;
        }

        private void SendMail_Reject(string refId, string dtno)
        {
            string enableRejectMail = config.GetValue("WF", "SR", "EnableRejectMail", LoginInfo.ConnStr);

            if (enableRejectMail.ToLower() == "1" || enableRejectMail.ToLower() == "true")
            {
                string fromMail = string.Empty;
                string toMail = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;


                string sql = @"SELECT ISNULL(u.Email, '') email, RefId, RequestCode, LocationCode, [Description], CreateDate
                            FROM [IN].StoreRequisition sr
                            LEFT JOIN [Admin].vUser u ON u.LoginName COLLATE DATABASE_DEFAULT = sr.CreateBy COLLATE DATABASE_DEFAULT
                            WHERE RefId = '{0}'";

                DataTable dt = config.DbExecuteQuery(string.Format(sql, refId), null, LoginInfo.ConnStr);

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows[0]["email"].ToString() != string.Empty)
                        toMail = dt.Rows[0]["email"].ToString();

                    if (toMail != string.Empty)
                    {
                        string wfStepName = _workFlowDt.Get(WFId, WfStep, LoginInfo.ConnStr).Rows[0]["StepDesc"].ToString();
                        string requestCode = dt.Rows[0]["RequestCode"].ToString();
                        string createdDate = Convert.ToDateTime(dt.Rows[0]["CreateDate"]).ToLongDateString();
                        string description = dt.Rows[0]["Description"].ToString();

                        sql = @"SELECT srdt.*, l.LocationName, p.ProductDesc1, p.ProductDesc2
                                FROM [IN].StoreRequisitionDetail srdt
                                LEFT JOIN [IN].StoreLocation l ON l.LocationCode = srdt.ToLocationCode
                                LEFT JOIN [IN].Product p ON p.ProductCode = srdt.ProductCode
                                WHERE DocumentId = '{0}' AND RefId IN ({1})";
                        DataTable dtSrDt = config.DbExecuteQuery(string.Format(sql, refId, dtno), null, LoginInfo.ConnStr);

                        body = "<body>";
                        body += "<div style='margin:0 auto; width: 70%; font-family:  Arial, Helvetica, sans-serif;'>";
                        body += string.Format(@"<h3 style='color:#4285F4;'>{0} ({1})</h3>", LoginInfo.BuInfo.BuCode, LoginInfo.BuInfo.BuName);
                        //body += string.Format(@"<div style='display:block; width:100%; background-color:#4285F4; color:white;'>
                        body += string.Format(@"<div style='display:block; width:100%; background-color:Red; color:white;'>
                                                    <div style='padding: 50px 0 10px 10px; color:white; font-size:1.2em;'><b>Reject: {0}</b></div>
                                                </div>", requestCode);
                        body += string.Format(@"
                                <br/>
                                <table>
                                    <tr>
                                        <td style='width:120px;'><b>Workflow:</b></td>
                                        <td>{0}</td>
                                    </tr>
                                    <tr>
                                        <td><b>Date:</b></td>
                                        <td>{1}</td>
                                    </tr>
                                    <tr>
                                        <td><b>Desription:</b></td>
                                        <td>{2}</td>
                                    </tr>
                                </table>", wfStepName, createdDate, description);
                        sql = string.Format("SELECT TOP(1) Comment FROM APP.WFHis WHERE Process = 'R' AND RefNo = '{0}' AND RefNoDt IN ({1}) ORDER BY Id DESC", refId, dtno);
                        DataTable dtWf = config.DbExecuteQuery(string.Format(sql, refId, dtno), null, LoginInfo.ConnStr);
                        if (dtWf.Rows.Count > 0)
                        {
                            body += string.Format("<br /><div><b>Reject Comment: </b>{0}</div>", dtWf.Rows[0]["Comment"].ToString());
                        }
                        body += "<br />";

                        //body += string.Format("<b>Rejected!</b> Store Requisition No. <b>{0}</b> Date: {1}<br /><br />", requestCode, dt.Rows[0]["CreateDate"].ToString());
                        body += "<table cellspacing='5' cellpadding='5' boder='1'>";
                        body += "<tr><th align='Left'>Location</th><th align='Left'>Product</th><th align='Left'>Unit</th><th align='Right'>Qty</th></tr>";

                        foreach (DataRow dr in dtSrDt.Rows)
                        {
                            body += string.Format("<tr><td align='Left'><b>{0}</b>:{1}</td><td align='Left'><b>{2}</b>:{3}</td><td align='Left'>{4}</td><td align='Right'>{5:N2}</td></tr>",
                                dr["ToLocationCode"].ToString(),
                                dr["LocationName"].ToString(),
                                dr["ProductCode"].ToString(),
                                dr["ProductDesc1"].ToString(),
                                dr["RequestUnit"].ToString(),
                                dr["RequestQty"].ToString()
                                );
                        }

                        body += "</table>";

                        string url = string.Empty;
                        string domain = config.GetConfigValue("APP", "IM", "WebServer", LoginInfo.ConnStr);
                        string subDomain = config.GetConfigValue("APP", "IM", "WebName", LoginInfo.ConnStr);
                        if (subDomain == string.Empty)
                            url = domain;
                        else
                            url = domain + "/" + subDomain;

                        body += string.Format(@"<br/>
                                    <a href='{0}/IN/STOREREQ/StoreReqDt.aspx?BuCode={1}&ID={2}&VID=32' target='_blank' style='display:inline-block; border:none; margin:4px 2px; padding: 10px 15px; cursor:pointer; background-color:#4285F4; color:white; text-align:center; text-decoration:none; font-size: 0.9em;'>View Detail</a>
                                    <br/>
                                    <hr/>
                                    <p style='font-size:0.8em; color:grey;'><b>Store Requisition</b> by <span style='color:#4285F4'>Blueledgers</span></p>", url, LoginInfo.BuInfo.BuCode, refId);


                        body += "</div>";
                        body += "</body>";

                        subject = string.Format("Notification: Some items are rejected from Store Requisition ({0})", requestCode);


                        SendMail(fromMail, toMail, string.Empty, subject, body);
                    }


                }

            }
        }



    }
}