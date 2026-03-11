using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Globalization;

namespace BlueLedger.PL.PC.PR
{
    public partial class MLtoPr : BasePage
    {
        private readonly Blue.BL.ADMIN.Department dep = new Blue.BL.ADMIN.Department();
        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();
        private readonly Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private DataSet dsTemplateDt = new DataSet();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();

        private readonly Blue.BL.APP.Config con = new Blue.BL.APP.Config();

        private bool IsApplyLastPrice
        {
            set { ViewState["IsApplyLastPrice"] = value; }
            get { return ViewState["IsApplyLastPrice"] == null ? false : Convert.ToBoolean(ViewState["IsApplyLastPrice"]); }
        }


        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTemplateDt = (DataSet)Session["dsTemplateDt"];
            }

            base.Page_Load(sender, e);
        }

        private void Page_Retrieve()
        {
            var value = con.GetConfigValue("PC", "PR", "ApplyLastPrice", LoginInfo.ConnStr).ToLower();

            IsApplyLastPrice = value == "1" || value == "true";

            //string MsgError = string.Empty;
            dsTemplateDt = (DataSet)Session["dsTemplateDt"];


            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTemplate = dsTemplateDt.Tables[template.TableName].Rows[0];
            lbl_Desc.Text = drTemplate["Desc"].ToString();

            // Default Delivery Date to next day
            txt_DeliveryDate.Date = ServerDateTime.AddDays(1);

            grd_TemplateDetail.DataSource = dsTemplateDt.Tables[templateDt.TableName];
            grd_TemplateDetail.DataBind();
        }

        protected void Save()
        {
            var connStr = LoginInfo.ConnStr;
            var currencyCode = con.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
            // Get Pr, PrDt Table Schema

            var dsPr = new DataSet();
            var getPrSchema = pr.GetStructure(dsPr, connStr);
            var getPrDtSchema = prDt.GetStructure(dsPr, connStr);

            // Create PR Header
            var drPr = dsPr.Tables[pr.TableName].NewRow();
            var prNo = pr.GetNewID(ServerDateTime, connStr);
            var prDate = ServerDateTime;

            drPr["PRNo"] = prNo;
            drPr["PrDate"] = prDate;
            drPr["Description"] = lbl_Desc.Text;

            if (workFlow.GetIsActive("PC", "PR", LoginInfo.ConnStr))
            {
                drPr["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", connStr);
            }

            drPr["HOD"] = dep.GetHeadOfDep(LoginInfo.DepartmentCode, connStr);
            drPr["DocStatus"] = "In Process";
            drPr["CreatedDate"] = ServerDateTime;
            drPr["CreatedBy"] = LoginInfo.LoginName;
            drPr["UpdatedDate"] = ServerDateTime;
            drPr["UpdatedBy"] = LoginInfo.LoginName;
            dsPr.Tables[pr.TableName].Rows.Add(drPr);

            // Create PR Detail
            var prDtCount = 1;

            for (var i = 0; i < grd_TemplateDetail.Rows.Count; i++)
            {
                var txt_ReqQty = (TextBox)grd_TemplateDetail.Rows[i].FindControl("txt_ReqQty");

                if (string.IsNullOrEmpty(txt_ReqQty.Text))
                {
                    continue;
                }

                if (decimal.Parse(txt_ReqQty.Text) > 0)
                {
                    var drPrDt = dsPr.Tables[prDt.TableName].NewRow();

                    var productCode = dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex]["ProductCode"].ToString();
                    var orderUnit = dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex]["UnitCode"].ToString();

                    var price = 0m;
                    var reqQty = Convert.ToDecimal(txt_ReqQty.Text);
                    var taxType = product.GetTaxType(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    var taxRate = product.GetTaxRate(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);

                    if (IsApplyLastPrice)
                    {
                        var item = GetProductLastPrice(productCode, orderUnit, prDate);

                        price = item.LastPrice;

                        drPrDt["LastPrice"] = item.LastPrice;
                        drPrDt["RefNo"] = item.LastRecNo;
                        drPrDt["VendorProdCode"] = item.LastVendorCode;
                    }


                    drPrDt["PRNo"] = drPr["PRNo"];
                    drPrDt["PRDtNo"] = prDtCount;
                    drPrDt["BuCode"] = LoginInfo.BuInfo.BuCode;
                    drPrDt["LocationCode"] = dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex]["LocationCode"].ToString();
                    drPrDt["ProductCode"] = productCode;
                    drPrDt["FOCQty"] = 0;
                    drPrDt["ReqQty"] = reqQty;
                    drPrDt["ApprQty"] = reqQty;
                    drPrDt["OrderUnit"] = orderUnit;

                    drPrDt["Reqdate"] = txt_DeliveryDate.Date.ToString("yyyy-MM-dd");
                    drPrDt["HOD"] = drPr["HOD"].ToString();

                    drPrDt["TaxAdj"] = false;
                    drPrDt["TaxType"] = taxType;
                    drPrDt["TaxRate"] = taxRate;
                    drPrDt["Price"] = price;
                    drPrDt["DeliPoint"] = store.GetDeliveryPoint(drPrDt["LocationCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["Comment"] = string.Empty;

                    drPrDt["DiscPercent"] = 0;

                    // Added on: 30/01/2018, By: Fon
                    drPrDt["CurrencyCode"] = currencyCode;
                    drPrDt["CurrencyRate"] = 1;


                    var net = 0m;
                    var tax = 0m;
                    var total = 0m;

                    switch (taxType.ToUpper())
                    {
                        case "A":
                            net = RoundAmt(price * reqQty);
                            tax = net * (taxRate / 100);
                            total = net + tax;
                            break;
                        case "I":
                            total = RoundAmt(price * reqQty);
                            tax = RoundAmt(total * taxRate / (100 + taxRate));
                            net = total - tax;
                            
                            break;
                        default:
                            net = RoundAmt(price * reqQty);
                            tax = 0m;
                            total = net;
                            break;
                    }


                    drPrDt["CurrDiscAmt"] = 0;
                    drPrDt["CurrNetAmt"] = net;
                    drPrDt["CurrTaxAmt"] = tax;
                    drPrDt["CurrTotalAmt"] = total;

                    drPrDt["DiscAmt"] =0;
                    drPrDt["TaxAmt"] = tax;
                    drPrDt["NetAmt"] = net;
                    drPrDt["TotalAmt"] = total;


                    drPrDt["OrderQty"] = 0;
                    drPrDt["RcvQty"] = 0;
                    drPrDt["Descen"] = ""; // product.GetName(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["Descll"] = ""; // product.GetName2(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    // End Added.

                    if (workFlow.GetIsActive("PC", "PR", LoginInfo.ConnStr))
                    {
                        drPrDt["ApprStatus"] = workFlow.GetDtApprStatus("PC", "PR", LoginInfo.ConnStr);
                    }

                    dsPr.Tables[prDt.TableName].Rows.Add(drPrDt);

                    prDtCount++;
                }
            }

            Session["dsTemplate"] = dsPr;



            var buCode = LoginInfo.BuInfo.BuCode;
            var vid = Request.Cookies["[PC].[vPrList]"].Value;

            if (Request.Params["Type"].ToUpper() == "M")
            {
                Response.Redirect("PrEdit.aspx?BuCode=" + buCode + "&MODE=template&VID=" + vid + "&Type=M");
            }
            else
            {
                Response.Redirect("PrEdit.aspx?BuCode=" + buCode + "&MODE=template&VID=" + vid + "&Type=O");
            }
        }

        protected void Back()
        {
            Response.Redirect("PrList.aspx");
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;
                case "BACK":
                    Back();
                    break;
            }
        }


        private ProductLastPrice GetProductLastPrice(string productCode, string unitCode, DateTime date)
        {
            var info = new ProductLastPrice();

            info.ProductCode = productCode;
            info.UnitCode = unitCode;

            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(unitCode))
                return info;


            var query = @"
SELECT 
	TOP(1)
	RecDate,
	rec.RecNo,
	rec.VendorCode,
	v.[Name] as VendorName,
	Price
FROM 
	PC.REC
	JOIN PC.RECDt ON rec.RecNo=recdt.RecNo
	LEFT JOIN AP.Vendor v ON v.VendorCode=rec.VendorCode
WHERE
	rec.DocStatus = 'Committed'
	AND CommitDate <= @Date
	AND ProductCode = @ProductCode
	AND RcvUnit =@UnitCode
ORDER BY
	RecDate DESC,
	RecNo DESC
";
            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Date", date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                new Blue.DAL.DbParameter("@ProductCode", productCode),
                new Blue.DAL.DbParameter("@UnitCode", unitCode)
            };
            var dt = prDt.DbExecuteQuery(query, parameters, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                info.LastRecDate = Convert.ToDateTime(dr["RecDate"]);
                info.LastRecNo = dr["RecNo"].ToString();
                info.LastVendorCode = dr["VendorCode"].ToString();
                info.LastVendorName = dr["VendorName"].ToString();
                info.LastPrice = Convert.ToDecimal(dr["Price"]);
            }


            return info;
        }

        public class ProductLastPrice
        {
            public ProductLastPrice()
            {
                LastPrice = 0;
            }

            public string ProductCode { get; set; }
            public string UnitCode { get; set; }
            public DateTime LastRecDate { get; set; }
            public string LastRecNo { get; set; }
            public string LastVendorCode { get; set; }
            public string LastVendorName { get; set; }
            public decimal LastPrice { get; set; }
        }
    }
}