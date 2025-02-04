using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PT.RCP
{
    public partial class RecipeDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.PT.RCP.Rcp rcp = new Blue.BL.PT.RCP.Rcp();
        private readonly Blue.BL.PT.RCP.RcpDt rcpDt = new Blue.BL.PT.RCP.RcpDt();
        private readonly Blue.BL.Option.Inventory.StoreLct stoLocate = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        string rcpCode = string.Empty;
        private readonly string moduleID = "4.1";

        private DataSet dsRecipe = new DataSet();

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);

            hf_DefaultAmtDigit.Value = DefaultAmtDigit.ToString();
            hf_DefaultSvcRate.Value = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);
            hf_DefaultTaxRate.Value = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);

            hf_DefaultSvcRate.Value = string.IsNullOrEmpty(hf_DefaultSvcRate.Value) ? "0" : hf_DefaultSvcRate.Value;
            hf_DefaultTaxRate.Value = string.IsNullOrEmpty(hf_DefaultTaxRate.Value) ? "0" : hf_DefaultTaxRate.Value;

            lbl_TaxSvcRate.Text = string.Format("Service charge: {0}% , Tax Rate: {1}%", hf_DefaultSvcRate.Value.ToString(), hf_DefaultTaxRate.Value.ToString());

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsRecipe = (DataSet)Session["dsRecipe"];
            }
        }

        private void Page_Retrieve()
        {
            var MsgError = string.Empty;

            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                rcpCode = Request.Params["ID"];

                var getStrLct = rcp.GetList(dsRecipe, rcpCode, hf_ConnStr.Value);

                if (getStrLct)
                {
                    rcpDt.GetList(dsRecipe, rcpCode, hf_ConnStr.Value);
                }
                else
                {
                    // var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }
            }

            Page_Setting();
        }

        private void Page_Setting()
        {
            // Header
            var drRecipe = dsRecipe.Tables[rcp.TableName].Rows[0];

            lbl_RcpCode.Text = drRecipe["RcpCode"].ToString();
            lbl_RcpDesc1.Text = drRecipe["RcpDesc1"].ToString();
            lbl_RcpDesc2.Text = drRecipe["RcpDesc2"].ToString();
            lbl_RcpCateCode.Text = drRecipe["RcpCateCode"].ToString();
            lbl_RcpUnit.Text = drRecipe["RcpUnitCode"].ToString();
            txt_Preparation.Text = drRecipe["Preparation"].ToString();
            lbl_PrepTime.Text = drRecipe["PrepTime"].ToString();
            lbl_TotalTime.Text = drRecipe["TotalTime"].ToString();
            lbl_PortionSize.Text = string.Format("{0:#,##0}", drRecipe["PortionSize"]);
            lbl_RcpLocateCode.Text = stoLocate.GetName(drRecipe["LocationCode"].ToString(), LoginInfo.ConnStr);
            //lbl_PortionCost.Text = string.Format("{0:#,##0.00}", drRecipe["PortionCost"]);
            //lbl_ProfitRate.Text = drRecipe["ProfitRate"].ToString();
            //lbl_RcpPrice.Text = string.Format("{0:#,##0.00}", drRecipe["RcpPrice"]);

            lblCostOfPortion.Text = string.Format("{0:#,##0.00}", drRecipe["PortionCost"]);
            lblTotalCost.Text = string.Format("{0:#,##0.00}", drRecipe["RcpCost"]);
            lblTotalMix.Text = string.Format("{0:0.00}", drRecipe["MixRatio"]);
            var costOfTotalMix = Convert.ToDecimal(drRecipe["RcpCost"]) + Convert.ToDecimal(drRecipe["MixCost"]);
            //lblCostOfTotalMix.Text = string.Format("{0:#,##0.00}", drRecipe["MixCost"]);
            lblCostOfTotalMix.Text = string.Format("{0:#,##0.00}", drRecipe["MixCost"]);
            lblNetPrice.Text = string.Format("{0:#,##0.00}", costOfTotalMix);
            lblGrossPrice.Text = string.Format("{0:#,##0.00}", drRecipe["GrossPrice"]);
            lblNetCost.Text = string.Format("{0:0.00}", drRecipe["NetCost"]);
            lblGrossCost.Text = string.Format("{0:0.00}", drRecipe["GrossCost"]);

            if (((bool)drRecipe["IsActived"] == true))
                lbl_RcpStatus.Text = "Active";
            else
                lbl_RcpStatus.Text = "Inactive";
            txt_Note.Text = drRecipe["Remark"].ToString();
            if (drRecipe["RcpImage"] != DBNull.Value)
                img01.ImageUrl = drRecipe["RcpImage"].ToString();
            else img01.Visible = false;


            if ((bool)drRecipe["IsVoid"] == true)
            {
                menu_CmdBar.Items.FindByName("Edit").Visible = false;
                menu_CmdBar.Items.FindByName("Void").Visible = false;

                VoidMessage.Attributes.Add("style", "display:block;");
                lbl_VoidComment_Alert.Text = drRecipe["VoidComment"].ToString();

            }
            else
            {
                menu_CmdBar.Items.FindByName("Edit").Visible = true;
                menu_CmdBar.Items.FindByName("Void").Visible = true;

                VoidMessage.Attributes.Add("style", "display:none;");
            }

            // Details
            // OnEvent: grd_RecipeDt_Load();
            Session["dsRecipe"] = dsRecipe;
            Control_HeaderMenuBar();

        }

        // Added on: 03/10/2017, By: Fon
        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            if (pagePermiss < 3)
            {
                menu_CmdBar.Items.FindByName("Create").Visible = false;
                menu_CmdBar.Items.FindByName("Edit").Visible = false;
                menu_CmdBar.Items.FindByName("Update").Visible = false;
            }
            menu_CmdBar.Items.FindByName("Void").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Void").Visible : false;
        }
        // End Added.

        protected void grd_RecipeDt_Load(object sender, EventArgs e)
        {
            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.DataBind();
        }

        protected void grd_RecipeDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ctrl = e.Row.FindControl("lbl_ItemType");
                if (ctrl != null)
                {
                    string typeCode = DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString();
                    if (typeCode.ToUpper() == "P")
                        (ctrl as Label).Text = "Product";
                    else
                        (ctrl as Label).Text = "Recipe";
                }

                ctrl = e.Row.FindControl("lbl_ItemCode");
                string ingredientCode = string.Empty;
                if (ctrl != null)
                {
                    ingredientCode = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString();
                    (ctrl as Label).Text = ingredientCode;

                }

                ctrl = e.Row.FindControl("lbl_ItemDesc1");
                if (ctrl != null)
                    (ctrl as Label).Text = rcpDt.GetItemDesc1(ingredientCode, hf_ConnStr.Value);

                ctrl = e.Row.FindControl("lbl_ItemDesc2");
                if (ctrl != null)
                    (ctrl as Label).Text = rcpDt.GetItemDesc2(ingredientCode, hf_ConnStr.Value);

                ctrl = e.Row.FindControl("lbl_RcpDtQty");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));

                ctrl = e.Row.FindControl("lbl_RcpUnit");
                if (ctrl != null)
                    (ctrl as Label).Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();

                ctrl = e.Row.FindControl("lbl_BaseUnit");
                if (ctrl != null)
                    (ctrl as Label).Text = DataBinder.Eval(e.Row.DataItem, "BaseUnit").ToString();

                ctrl = e.Row.FindControl("lbl_UnitRate");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "UnitRate"));

                ctrl = e.Row.FindControl("lbl_BaseCost");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "BaseCost"));

                ctrl = e.Row.FindControl("lbl_NetCost");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetCost"));

                ctrl = e.Row.FindControl("lbl_WastageRate");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "SpoilRate"));

                ctrl = e.Row.FindControl("lbl_WastageCost");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "SpoilCost"));

                ctrl = e.Row.FindControl("lbl_TotalCost");
                if (ctrl != null)
                    (ctrl as Label).Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalCost"));
            }

        }


        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {

            switch (e.Item.Name.ToUpper())
            {
                // Added on: 15/09/2017, By: Fon, For: Update cost of Recipe.
                case "UPDATE":
                    var id = Request.Params["ID"] == null ? "" : Request.Params["ID"].ToString();
                    //var parameters = new Blue.DAL.DbParameter[1];

                    //parameters[0] = new Blue.DAL.DbParameter("@RcpCode", id);

                    //bool result = rcpDt.UpdateCostOfRcpLst(dbParams, LoginInfo.ConnStr);

                    //bool result = rcpDt.UpdateCostOfRecipe(parameters, LoginInfo.ConnStr);


                    bu.DbExecuteQuery(string.Format("EXEC [PT].[UpdateRecipeCost] '{0}'", id), null, LoginInfo.ConnStr);

                    Response.Redirect(Request.Url.ToString());
                    //AlertMessageBox(id);

                    break;
                // End Added.

                case "CREATE":
                    Response.Redirect("RecipeEdit.aspx?MODE=New&BuCode=" +
                                      LoginInfo.BuInfo.BuCode + "&VID=" +
                                      Request.Cookies["[PT].[vRcpList]"].Value);
                    break;

                case "EDIT":
                    Response.Redirect("RecipeEdit.aspx?BuCode=" + Request.Params["BuCode"] +
                                      "&MODE=EDIT&ID=" + Request.Params["ID"] +
                                      "&VID=" + Request.Params["VID"]);
                    break;

                case "VOID":
                    pop_ConfirmVoid.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);

                    var pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=RecipeForm";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    break;

                case "BACK":
                    Response.Redirect("RecipeList.aspx");
                    break;
            }
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            string comment = txt_VoidComment.Text;
            var drRecipe = dsRecipe.Tables[rcp.TableName].Rows[0];

            drRecipe["IsActived"] = 0;
            drRecipe["IsVoid"] = 1;
            drRecipe["VoidComment"] = comment;
            drRecipe["UpdatedDate"] = ServerDateTime;
            drRecipe["UpdatedBy"] = LoginInfo.LoginName;

            var result = rcp.Save(dsRecipe, hf_ConnStr.Value);

            if (result)
            {
                rcpCode = dsRecipe.Tables[rcp.TableName].Rows[0]["RcpCode"].ToString();
                Response.Redirect("RecipeDt.aspx?ID=" + rcpCode + "&BuCode=" + Request.Params["BuCode"]);
            }


        }

        protected void btn_Void_Info_OK_Click(object sender, EventArgs e)
        {
            pop_Information.ShowOnPageLoad = false;
            pop_ConfirmVoid.ShowOnPageLoad = false;

            Page_Retrieve();

        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }
    }
}