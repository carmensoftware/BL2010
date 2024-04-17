using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN.STOREREQ
{
    public partial class StoreReqEdit : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsStoreReq = new DataSet();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StandardRequistion stdReq = new Blue.BL.IN.StandardRequistion();
        private readonly Blue.BL.IN.StandardRequisitionDetail stdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        private readonly Blue.BL.IN.storeRequisition storeReq = new Blue.BL.IN.storeRequisition();
        private readonly Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();
        private readonly Blue.BL.APP.WF workflow = new Blue.BL.APP.WF();
        private DataSet dsStoreReqFromStdReq = new DataSet();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreReqFromStdReq = (DataSet)Session["dsStoreReqFromStdReq"];
            }
        }

        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "TEMPLATE")
            {
                dsStoreReqFromStdReq = (DataSet)Session["dsStdReq"];
            }
            else
            {
                dsStoreReqFromStdReq = (DataSet)Session["dsStoreReqDt"];
            }

            // Modified on: 06/11/2017, By: Fon
            //locat.GetList(dsStoreReqFromStdReq, LoginInfo.ConnStr);

            locat.GetList(dsStoreReqFromStdReq, LoginInfo.LoginName, LoginInfo.ConnStr);
            // End Modified.

            Session["dsStoreReqFromStdReq"] = dsStoreReqFromStdReq;
            Page_Setting();
        }

        private void Page_Setting()
        {
            var drStoreReq = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0];
            de_DeliveryDate.Date = ServerDateTime.Date.AddDays(1);
            txt_Desc.Text = drStoreReq["Description"].ToString();

            // Note By: Fon, Message: U have to alter [IN].[StandardRequisitionDetail_GetListByRefId]
            grd_StoreReqFromStdReq.DataSource = dsStoreReqFromStdReq.Tables[stdReqDt.TableName];
            grd_StoreReqFromStdReq.DataBind();
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreReqLst.aspx");
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            if (ddl_Store.Value == null)
            {
                lbl_Warning.Text = "Please select 'Requested From'.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(250);
                return;
            }

            storeReq.GetStructure(dsStoreReq, LoginInfo.ConnStr);
            storeReqDt.GetStructure(dsStoreReq, LoginInfo.ConnStr);

            //Create Header
            var drStoreReq = dsStoreReq.Tables[storeReq.TableName].NewRow();
            drStoreReq["RequestCode"] = storeReq.GetNewRequestCode(ServerDateTime, LoginInfo.ConnStr);
            drStoreReq["Description"] = txt_Desc.Text;
            drStoreReq["DeliveryDate"] = de_DeliveryDate.Date;
            drStoreReq["LocationCode"] = ddl_Store.Value;
            drStoreReq["CreateBy"] = LoginInfo.LoginName;
            drStoreReq["CreateDate"] = ServerDateTime.Date;
            drStoreReq["UpdateBy"] = LoginInfo.LoginName;
            drStoreReq["UpdateDate"] = ServerDateTime.Date;
            dsStoreReq.Tables[storeReq.TableName].Rows.Add(drStoreReq);

            //Create Details
            for (var i = 0; i < grd_StoreReqFromStdReq.Rows.Count; i++)
            {
                decimal number;
                var onHand = GetOnHand(
                    dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["ProductCode"].ToString(),
                    dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString());


                var txt_Qty = grd_StoreReqFromStdReq.Rows[i].FindControl("txt_Qty") as ASPxSpinEdit;
                var lbl_OnHand = grd_StoreReqFromStdReq.Rows[i].FindControl("lbl_OnHand") as Label;

                if (string.IsNullOrEmpty(txt_Qty.Text))
                {
                    continue;

                }
                else if (!decimal.TryParse(txt_Qty.Text, out number))
                {
                    //check number
                    //Please entry only 1-9
                    lbl_Warning.Text = "Input Quantity Request";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
                else if (decimal.Parse(txt_Qty.Text) <= 0)
                {
                    lbl_Warning.Text = "'Qty Requested' must be greater than 0";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }

                // {OP Remark} Remove check onhand 
                //else if (decimal.Parse(txt_Qty.Text) > decimal.Parse(lbl_OnHand.Text))
                //{
                //    lbl_Warning.Text = "Can not request quantity more than on hand";
                //    pop_Warning.ShowOnPageLoad = true;
                //    pop_Warning.Width = Unit.Pixel(300);

                //    txt_Qty.Text = String.Format("{0:N}", decimal.Parse(lbl_OnHand.Text));
                //    SetFocus(txt_Qty);
                //    return;
                //}
                var drStoreReqDt = dsStoreReq.Tables[storeReqDt.TableName].NewRow();
                drStoreReqDt["CategoryCode"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["CategoryCode"];
                drStoreReqDt["ProductCode"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["ProductCode"];
                drStoreReqDt["RequestUnit"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["RequestUnit"];
                drStoreReqDt["ToLocationCode"] = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"];
                drStoreReqDt["DeliveryDate"] = ServerDateTime.Date.AddDays(1);
                drStoreReqDt["RequestQty"] = decimal.Parse(txt_Qty.Text);
                drStoreReqDt["ApprQty"] = decimal.Parse(txt_Qty.Text);
                drStoreReqDt["ApprStatus"] = workflow.GetDtApprStatus("IN", "SR", LoginInfo.ConnStr);
                dsStoreReq.Tables[storeReqDt.TableName].Rows.Add(drStoreReqDt);
            }

            Session["dsStoreReqDt"] = dsStoreReq;

            Response.Redirect("StoreReqEdit.aspx?MODE=SR&VID=" + Request.Params["VID"] + "&BuCode=" +
                              LoginInfo.BuInfo.BuCode);
        }

        protected void grd_StoreReqFromStdReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("hf_LocationCode") != null)
                {
                    var hf_LocationCode = e.Row.FindControl("hf_LocationCode") as HiddenField;
                    hf_LocationCode.Value = DataBinder.Eval(e.Row.DataItem, "locationCode").ToString();
                }
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }

                if (e.Row.FindControl("lbl_EnglishName") != null)
                {
                    var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                    lbl_EnglishName.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_LocalName") != null)
                {
                    var lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                    lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_OnHand") != null)
                {
                    var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;

                    if (ddl_Store.Value != null)
                    {
                        lbl_OnHand.Text = String.Format(DefaultQtyFmt,
                            GetOnHand(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                ddl_Store.Value.ToString()));
                    }
                    else
                    {
                        lbl_OnHand.Text = "0.00";
                    }
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                }
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            //ddl_Store.DataSource = dsStoreReqFromStdReq.Tables[locat.TableName];

            // Modified on: 06/11/2017, By: Fon
            //ddl_Store.DataSource =
            //    locat.GetLookup(dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString(),
            //        LoginInfo.ConnStr);

            string toLocation = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString();
            

            /*** Do like StoreReqEdit.aspx ***/
            var ds = dsStoreReqFromStdReq.Tables[locat.TableName];
            //ds.DefaultView.RowFilter = "EOP = 1";
            ds.DefaultView.RowFilter = string.Format("EOP = 1 AND LocationCode <> '{0}'", toLocation);
            var dt = ds.DefaultView.ToTable();
            ddl_Store.DataSource = dt;
            ddl_Store.ValueField = "LocationCode";
            ddl_Store.DataBind();
            // End Modified.
        }

        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            var productLookup = new ProductLookup();

            foreach (GridViewRow grd_Row in grd_StoreReqFromStdReq.Rows)
            {
                var lbl_ProductCode = grd_Row.FindControl("lbl_ProductCode") as Label;
                var lbl_OnHand = grd_Row.FindControl("lbl_OnHand") as Label;
                var hf_LocationCode = grd_Row.FindControl("hf_LocationCode") as HiddenField;
                lbl_OnHand.Text = String.Format(DefaultQtyFmt, GetOnHand(lbl_ProductCode.Text, ddl_Store.Value.ToString()));

                if (ddl_Store.Value != null)
                {
                    var txt_Qty = grd_Row.FindControl("txt_Qty") as ASPxSpinEdit;
                    var ds = productLookup.GetRecordBy2Location(hf_LocationCode.Value, ddl_Store.Value.ToString(),
                        lbl_ProductCode.Text, LoginInfo.ConnStr);
                    txt_Qty.Enabled = ds.Tables[0].Rows.Count > 0;
                    if (ds.Tables[0].Rows.Count == 0) txt_Qty.Text = "";
                    if (txt_Qty.Enabled)
                    {
                        txt_Qty.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        txt_Qty.BackColor = System.Drawing.Color.LightGray;
                    }
                }
            }
        }

        private decimal GetOnHand(string productCode, string locationCode)
        {
            if (dsStoreReq.Tables[prDt.TableName] != null)
            {
                dsStoreReq.Tables[prDt.TableName].Clear();
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var get = prDt.GetStockSummary(dsStoreReq, productCode, locationCode, date, LoginInfo.ConnStr);

            if (get)
            {
                if (dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty && dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
                {
                    return decimal.Parse(dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
                }
            }


            return 0;
        }
    }
}