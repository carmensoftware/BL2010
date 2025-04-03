using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

using System.Data.SqlClient;
using Blue.BL;
using System.Collections.Generic;
using Blue.DAL;
using System.Globalization;

namespace BlueLedger.PL.PC.PR
{
    public partial class PrEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();


        private readonly Blue.BL.PC.PL.PL pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt plDt = new Blue.BL.PC.PL.PLDt();

        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prdt = new Blue.BL.PC.PR.PRDt();

        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        private readonly Blue.BL.Import.JobCode jobCode = new Blue.BL.Import.JobCode();
        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();

        #endregion

        private string _MODE { get { return Request.QueryString["MODE"]; } }

        private string _VID { get { return Request.QueryString["VID"] == null ? "0" : Request.QueryString["VID"].ToString(); } }

        private string _ID { get { return Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString(); } }

        #region -- Properties --

        private string _DefaultCurrency
        {
            get { return ViewState["_DefaultCurrency"].ToString(); }
            set { ViewState["_DefaultCurrency"] = value; }
        }


        private ViewHandler _ViewHandler
        {
            get { return (ViewHandler)ViewState["ViewHandler"]; }
            set { ViewState["ViewHandler"] = value; }
        }

        private DataTable _dtPr
        {
            get { return (DataTable)ViewState["dtPr"]; }
            set { ViewState["dtPr"] = value; }
        }

        private DataTable _dtPrDt
        {
            get { return (DataTable)ViewState["dtPrDt"]; }
            set { ViewState["dtPrDt"] = value; }
        }


        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                CheckUserRequired();

                Page_Retrieve();
            }



        }

        private void Page_Retrieve()
        {
            _ViewHandler = GetViewHander(_VID);

            // not found view, go to list page
            if (_ViewHandler.ViewNo == 0)
            {
                Response.Redirect("~/PC/PR/PrList.aspx");
            }

            _DefaultCurrency = config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);


            _dtPr = GetPr(_ID);
            _dtPrDt = GetPrDt(_ID);

            ddl_PrType.Items.Clear();
            ddl_PrType.Items.AddRange(GetPrTypeItems());

            ddl_JobCode.Items.Clear();
            ddl_JobCode.Items.AddRange(GetJobCodeItems());




            //switch (_MODE.ToUpper())
            //{
            //    case "NEW":
            //        #region

            //        #endregion
            //        break;
            //    case "EDIT":
            //        #region

            //        #endregion
            //        break;
            //    case "TEMPLATE":
            //        #region

            //        #endregion
            //        break;
            //}


            Page_Setting();
        }

        private void Page_Setting()
        {
            // Menu Header
            lbl_ViewName.Text = _ViewHandler.ViewName;
            menu_Header.Items.FindByName("Commit").Visible = _ViewHandler.AllowCreate;

            #region -- Header --

            switch (_MODE.ToUpper())
            {
                case "NEW":
                    #region

                    #endregion
                    break;
                case "EDIT":
                    #region

                    #endregion
                    break;
                case "TEMPLATE":
                    #region

                    #endregion
                    break;
            }


            var drPr = IsDataValid(_dtPr) ? _dtPr.Rows[0] : null;


            lbl_PrNo.Text = _ID;
            date_PrDate.Date = IsDataValid(_dtPr) ? Convert.ToDateTime(drPr["PrDate"]) : DateTime.Today;
            lbl_Requestor.Text = IsDataValid(_dtPr) ? drPr["CreatedBy"].ToString() : LoginInfo.LoginName;

            ddl_PrType.Value = drPr == null ? null : drPr["PrType"].ToString();
            ddl_PrType.Enabled = _dtPrDt.Rows.Count == 0; // No selecting if there is any detail item.

            ddl_JobCode.Value = drPr == null ? null : drPr["AddField1"].ToString();
            lbl_Department.Text = drPr == null ? GetDepartmentItem().Text : GetDepartmentName(drPr["HOD"].ToString());
            lbl_Status.Text = drPr == null ? "In Process" : drPr["DocStatus"].ToString().Trim();

            txt_Desc.Text = drPr == null ? "" : drPr["Description"].ToString().Trim();

            #endregion


            #region -- Detail --


            gv_PrDt.DataSource = _dtPrDt;
            gv_PrDt.DataBind();

            #endregion
        }




        // Action(s)
        #region --Action(s)--

        protected void menu_Header_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    break;

                case "COMMIT":

                    break;

                case "BACK":
                    if (_MODE == "EDIT")
                    {
                        var buCode = Request.Params["BuCode"];
                        var vid = Request.Params["VID"];
                        var id = ""; // dsPR.Tables[pr.TableName].Rows[0]["PrNo"];
                        var url = string.Format("~/PC/PR/Pr.aspx?BuCode={0}&ID={1}&VID={2}", buCode, vid, id);

                        Response.Redirect(url);
                    }
                    else
                    {
                        Response.Redirect("~/PC/PR/PrList.aspx");
                    }
                    break;
            }
        }

        protected void menu_Detail_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateDetail();
                    break;

                case "DELETE":
                    DeleteDetail();
                    break;
            }
        }

        protected void btn_Apply_DeliveryDate_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region --Detail Action(s)--

        private void CreateDetail()
        {
        }

        private void DeleteDetail()
        {
        }

        #endregion


        #region -- gv_PrDt --

        protected void gv_PrDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                #region --Vendor--

                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Vendor") as Label;

                    var text = string.Format("<b>{0}</b><br />{1}", DataBinder.Eval(dataItem, "VendorCode").ToString(), DataBinder.Eval(dataItem, "VendorName").ToString());

                    lbl.Text = text;
                    lbl.ToolTip = lbl.Text;
                }

                #endregion

                #region --Location--
                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Location") as Label;

                    var text = string.Format("<b>{0}</b><br/>{1}", DataBinder.Eval(dataItem, "LocationCode").ToString(), DataBinder.Eval(dataItem, "LocationName").ToString());

                    lbl.Text = text;
                    lbl.ToolTip = lbl.Text;
                }

                #endregion

                #region --Product--

                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Product") as Label;


                    var desc1 = DataBinder.Eval(dataItem, "ProductDesc1").ToString().Trim();
                    var desc2 = DataBinder.Eval(dataItem, "ProductDesc2").ToString().Trim();

                    var text = string.Format("<b>{0}</b><br />{1}", DataBinder.Eval(dataItem, "ProductCode").ToString(), desc1);
                    if (desc1 != desc2)
                        text += string.Format("<br />{0}", desc2);

                    lbl.Text = text;
                    lbl.ToolTip = lbl.Text;
                }

                #endregion

                #region --Unit--
                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Unit") as Label;

                    var text = string.Format("<b>{0}</b><br/>{1}", DataBinder.Eval(dataItem, "OrderUnit").ToString(), DataBinder.Eval(dataItem, "OrderUnitName").ToString());

                    lbl.Text = text;
                    lbl.ToolTip = lbl.Text;
                }
                #endregion

                #region --Price--
                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Price") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(dataItem, "Price"));
                    lbl.ToolTip = lbl.Text;
                }
                #endregion

                #region --Req. Qty--

                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lbl = e.Row.FindControl("lbl_ReqQty") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(dataItem, "ReqQty"));
                    lbl.ToolTip = lbl.Text;
                }

                #endregion

                #region --Appr Qty--
                if (e.Row.FindControl("lbl_AppQty") != null)
                {
                    var lbl = e.Row.FindControl("lbl_AppQty") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(dataItem, "ApprQty"));
                    lbl.ToolTip = lbl.Text;
                }
                #endregion


                #region --Foc Qty--
                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var lbl = e.Row.FindControl("lbl_FocQty") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(dataItem, "FocQty"));
                    lbl.ToolTip = lbl.Text;
                }
                #endregion

                #region --CurrTotalAmt--
                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTotalAmt") as Label;

                    lbl.Text = string.Format("{0}<br />{1}", FormatAmt(DataBinder.Eval(dataItem, "CurrTotalAmt")), DataBinder.Eval(dataItem, "CurrencyCode"));
                    lbl.ToolTip = lbl.Text;

                }
                #endregion

                #region --TotalAmt--
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TotalAmt") as Label;

                    lbl.Text = string.Format("{0}<br />{1}", FormatAmt(DataBinder.Eval(dataItem, "CurrTotalAmt")), _DefaultCurrency);
                    lbl.ToolTip = lbl.Text;
                }
                #endregion

                #region --Delivery--

                if (e.Row.FindControl("lbl_Delivery") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Delivery") as Label;

                    var text = string.Format("&nbsp;&nbsp;{0}<br/>&nbsp;&nbsp;{1}", DataBinder.Eval(dataItem, "DeliPointName").ToString(), FormatDate(DataBinder.Eval(dataItem, "ReqDate")));

                    lbl.Text = text;
                    lbl.ToolTip = lbl.Text;
                }
                #endregion

                
                #region --Expand--



                #endregion


            }
        }

        protected void gv_PrDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW")
            {
            }
        }

        protected void gv_PrDt_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void gv_PrDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
        }

        protected void gv_PrDt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }

        protected void gv_PrDt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        #endregion


        #region -- Private method(s) --

        private string FormatAmt(object value)
        {
            return string.Format("{0:N" + DefaultAmtDigit.ToString() + "}", Convert.ToDecimal(value));
        }

        private string FormatQty(object value)
        {

            return string.Format("{0:N" + DefaultQtyDigit.ToString() + "}", Convert.ToDecimal(value));
        }

        private string FormatDate(object value)
        {
            return string.IsNullOrEmpty(value.ToString()) ? "" : Convert.ToDateTime(value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }


        private void CheckUserRequired()
        {
            var connStr = bu.GetConnectionString(Request.Params["BuCode"]);
            if (!string.IsNullOrEmpty(connStr))
            {
                var item = GetDepartmentItem();

                if (item == null)
                {
                    ShowWarning("Department is required. Please assign a department to this user.");

                    return;
                }

                var items = GetPrTypeItems();

                if (items.Count == 0)
                {
                    ShowWarning("PR Type must be assigned to user's roles. Please assign PR Type to user's role");

                    return;
                }
            }

        }

        private bool IsDataValid(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        private ViewHandler GetViewHander(string viewNo)
        {
            var result = new ViewHandler();

            var sql = @"
SELECT
	*
FROM 
	APP.ViewHandler v
	LEFT JOIN APP.WFDt ON wfdt.WFId=v.WFId AND wfdt.Step=v.WFStep
WHERE 
	ViewNo=@vid
";
            var dt = bu.DbExecuteQuery(sql, new DbParameter[] { new DbParameter("@vid", viewNo) }, hf_ConnStr.Value);

            if (dt.Rows.Count > 0)
            {
                result = dt.AsEnumerable()
                    .Select(x => new ViewHandler
                    {
                        ViewNo = x.Field<Int32>("ViewNo"),
                        ViewName = x.Field<string>("Desc"),
                        AllowCreate = x.Field<bool>("AllowCreate"),
                        EnableField = x.Field<string>("EnableField"),
                        HideField = x.Field<string>("HideField"),
                        NeededAppr = x.Field<Byte>("NeededAppr") == 1,
                        ApprRule = x.Field<string>("ApprRule"),
                        IsAllocateVendor = x.Field<bool>("IsAllocateVendor"),
                        SentEMail = x.Field<bool>("SentEMail")
                    })
                    .FirstOrDefault();
            }
            return result;
        }

        private DataTable GetPr(string prNo)
        {
            var dt = bu.DbExecuteQuery("SELECT TOP(1) * FROM PC.Pr WHERE PrNo=@prNo", new DbParameter[] { new DbParameter("@PrNo", prNo) }, hf_ConnStr.Value);


            return dt;
        }

        private DataTable GetPrDt(string prNo)
        {
            var query = @"
SELECT 
	prdt.* ,
	v.[Name] as VendorName,
	l.LocationName,
	p.ProductDesc1,
	p.ProductDesc2,
	u.[Name] as OrderUnitName,
	dp.[Name] as DeliPointName
FROM 
	PC.PrDt
	LEFT JOIN [AP].Vendor v
		ON v.VendorCode=prdt.VendorCode
	LEFT JOIN [IN].StoreLocation l
		ON l.LocationCode=prdt.LocationCode
	LEFT JOIN [IN].Product p
		ON p.ProductCode=prdt.ProductCode
	LEFT JOIN [IN].Unit u
		ON u.UnitCode=prdt.OrderUnit
	LEFT JOIN [IN].DeliveryPoint dp
		ON dp.DptCode=prdt.DeliPoint
WHERE 
	PrNo=@prNo 
ORDER BY 
	PrDtNo
";

            var dt = bu.DbExecuteQuery(query, new DbParameter[] { new DbParameter("@PrNo", prNo) }, hf_ConnStr.Value);


            return dt;
        }

        private ListEditItemCollection GetPrTypeItems()
        {
            var result = new ListEditItemCollection();

            var query = @"
;WITH
t AS(
	SELECT 
		ProdCateTypeCode as [Code]
	FROM 
		[ADMIN].UserRole 
		JOIN [IN].RoleType 
			ON userrole.RoleName = roletype.RoleName 
	WHERE 
		UserRole.IsActive = 1 
		AND LoginName = @LoginName
)
SELECT
	CAST(c.Code AS VARCHAR) as [Value],
	c.[Description] as [Text]

FROM
	t
	JOIN [IN].ProdCateType c
		ON t.Code = c.Code
WHERE
	c.IsActived = 1
ORDER BY
	c.Code
";
            var dt = bu.DbExecuteQuery(query, new DbParameter[] { new DbParameter("@LoginName", LoginInfo.LoginName) }, hf_ConnStr.Value);

            var items = dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                })
                .ToArray();

            result.AddRange(items);

            return result;
        }

        private ListEditItemCollection GetJobCodeItems()
        {
            var result = new ListEditItemCollection();

            var query = "SELECT '' AS [Value], '' AS [Text] UNION ALL SELECT Code as [Value], CONCAT([Code],' : ', [Description]) as [Text] FROM [IMPORT].JobCode WHERE IsActive=1 ORDER BY [Value]";
            var dt = bu.DbExecuteQuery(query, null, hf_ConnStr.Value);

            var items = dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                })
                .ToArray();

            result.AddRange(items);

            return result;
        }

        private ListEditItem GetDepartmentItem()
        {

            var query = @"
;WITH
usr AS(
	SELECT 
		DepartmentCode 
	FROM 
		[ADMIN].vUser 
	WHERE 
		LoginName = @LoginName
)
SELECT
	usr.DepartmentCode as [Value],
	d.DepName as [Text]
FROM
	usr
	JOIN [ADMIN].Department d
		ON d.DepCode=usr.DepartmentCode
";
            var dt = bu.DbExecuteQuery(query, new DbParameter[] { new DbParameter("@LoginName", LoginInfo.LoginName) }, hf_ConnStr.Value);

            var item = dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                })
                .FirstOrDefault();


            return item;
        }

        private string GetDepartmentName(string code)
        {
            var dt = bu.DbExecuteQuery("SELECT DepName FROM [ADMIN].Department WHERE DepCode=@code", new DbParameter[] { new DbParameter("@Code", code) }, hf_ConnStr.Value);

            return IsDataValid(dt) ? dt.Rows[0][0].ToString() : "";
        }
        #endregion


        #region -- Popup Event(s) --

        private void ShowWarning(string text)
        {
            pop_Alert.HeaderText = "Warning";

            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void ShowInfo(string text)
        {
            pop_Alert.HeaderText = "Information";

            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }



        #endregion

        [Serializable]
        public class ViewHandler
        {
            public ViewHandler()
            {
                ViewNo = 0;
                ViewName = "";
                AllowCreate = false;
                EnableField = "";
                HideField = "";
                NeededAppr = false;
                ApprRule = "";
                IsAllocateVendor = false;
                SentEMail = false;
            }

            public int ViewNo { get; set; }
            public string ViewName { get; set; }
            public bool AllowCreate { get; set; }
            public string EnableField { get; set; }
            public string HideField { get; set; }
            public bool NeededAppr { get; set; }
            public string ApprRule { get; set; }
            public bool IsAllocateVendor { get; set; }
            public bool SentEMail { get; set; }

        }
    }
}