using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

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
            // Get Pr, PrDt Table Schema
            var dsPr = new DataSet();
            var getPrSchema = pr.GetStructure(dsPr, LoginInfo.ConnStr);
            var getPrDtSchema = prDt.GetStructure(dsPr, LoginInfo.ConnStr);

            // Create PR Header
            var drPr = dsPr.Tables[pr.TableName].NewRow();
            drPr["PRNo"] = pr.GetNewID(ServerDateTime, LoginInfo.ConnStr);
            drPr["PrDate"] = ServerDateTime;
            drPr["Description"] = lbl_Desc.Text;

            if (workFlow.GetIsActive("PC", "PR", LoginInfo.ConnStr))
            {
                drPr["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", LoginInfo.ConnStr);
            }

            drPr["HOD"] = dep.GetHeadOfDep(LoginInfo.DepartmentCode, LoginInfo.ConnStr);
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

                    drPrDt["PRNo"] = drPr["PRNo"];
                    drPrDt["PRDtNo"] = prDtCount;
                    drPrDt["ProductCode"] =
                        dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex][
                            "ProductCode"].ToString();
                    drPrDt["ReqQty"] = txt_ReqQty.Text;
                    drPrDt["HOD"] = drPr["HOD"].ToString();
                    drPrDt["BuCOde"] = LoginInfo.BuInfo.BuCode;
                    drPrDt["Reqdate"] = txt_DeliveryDate.Date.ToString("dd/MM/yyyy");
                    drPrDt["OrderQty"] = 0;
                    drPrDt["ApprQty"] = 0;
                    drPrDt["FOCQty"] = 0;
                    drPrDt["RcvQty"] = 0;
                    drPrDt["DiscPercent"] = 0;
                    drPrDt["DiscAmt"] = 0;
                    drPrDt["TaxAdj"] = false;
                    drPrDt["TaxAmt"] = 0;
                    drPrDt["NetAmt"] = 0;
                    drPrDt["TotalAmt"] = 0;
                    drPrDt["TaxType"] = product.GetTaxType(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["TaxRate"] = product.GetTaxRate(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["Price"] = 0;
                    drPrDt["OrderUnit"] =
                        dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex][
                            "UnitCode"].ToString();
                    drPrDt["LocationCode"] =
                        dsTemplateDt.Tables[templateDt.TableName].Rows[grd_TemplateDetail.Rows[i].DataItemIndex][
                            "LocationCode"].ToString();
                    drPrDt["DeliPoint"] = store.GetDeliveryPoint(drPrDt["LocationCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["Comment"] = string.Empty;
                    drPrDt["Descen"] = product.GetName(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drPrDt["Descll"] = product.GetName2(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);

                    // Added on: 30/01/2018, By: Fon
                    drPrDt["CurrNetAmt"] = 0;
                    drPrDt["CurrDiscAmt"] = 0;
                    drPrDt["CurrTaxAmt"] = 0;
                    drPrDt["CurrTotalAmt"] = 0;
                    drPrDt["CurrencyCode"] = con.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
                    drPrDt["CurrencyRate"] = 1;
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

            if (Request.Params["Type"].ToUpper() == "M")
            {
                // Redirec to PrEdit.aspx page
                Response.Redirect("PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=template&VID=" +
                                  Request.Cookies["[PC].[vPrList]"].Value + "&Type=M");
            }
            else
            {
                // Redirec to PrEdit.aspx page
                Response.Redirect("PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=template&VID=" +
                                  Request.Cookies["[PC].[vPrList]"].Value + "&Type=O");
            }


            //bool result = pr.Save(dsPr, LoginInfo.ConnStr);

            //if (result)
            //{
            //    // Update Approve Status
            //    DbParameter[] dbParams1 = new DbParameter[1];
            //    dbParams1[0] = new DbParameter("@PrNo", drPr["PRNo"].ToString());
            //    workFlowDt.ExcecuteApprRule("APP.WF_PR_APPR_STEP_1", dbParams1, LoginInfo.ConnStr);

            //    DbParameter[] dbParams2 = new DbParameter[2];
            //    dbParams2[0] = new DbParameter("@PrNo", drPr["PRNo"].ToString());
            //    dbParams2[1] = new DbParameter("@LoginName", LoginInfo.LoginName);
            //    workFlowDt.ExcecuteApprRule("APP.WF_PR_APPR_STEP_2", dbParams2, LoginInfo.ConnStr);

            //    // Redirec to PrEdit.aspx page
            //    //Response.Redirect("PrHQ.aspx?ID=" + drPr["PRNo"].ToString());
            //    Response.Redirect("Pr.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + drPr["PRNo"].ToString() + "&VID=" + Request.Params["VID"].ToString());
            //}
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
    }
}