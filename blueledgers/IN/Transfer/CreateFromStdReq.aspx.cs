using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.Transfer
{
    public partial class CreateFromStdReq : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsStoreReq = new DataSet();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StandardRequistion stdReq = new Blue.BL.IN.StandardRequistion();
        private readonly Blue.BL.IN.StandardRequisitionDetail stdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        private readonly Blue.BL.IN.Transfer trf = new Blue.BL.IN.Transfer();
        private readonly Blue.BL.IN.TransferDt trfDt = new Blue.BL.IN.TransferDt();
        //BL.APP.WF workflow                          = new BL.APP.WF();
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
                dsStoreReqFromStdReq = (DataSet) Session["dsStoreReqFromStdReq"];
            }
        }

        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "TEMPLATE")
            {
                dsStoreReqFromStdReq = (DataSet) Session["dsStdReq"];
            }
            else
            {
                dsStoreReqFromStdReq = (DataSet) Session["dsStoreReqDt"];
            }

            Session["dsStoreReqFromStdReq"] = dsStoreReqFromStdReq;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drStoreReq = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0];
            de_DeliveryDate.Date = ServerDateTime.Date.AddDays(1);
            txt_Desc.Text = drStoreReq["Description"].ToString();

            grd_StoreReqFromStdReq.DataSource = dsStoreReqFromStdReq.Tables[stdReqDt.TableName];
            grd_StoreReqFromStdReq.DataBind();
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfLst.aspx");
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            trf.GetStructure(dsStoreReq, LoginInfo.ConnStr);
            trfDt.GetStructure(dsStoreReq, LoginInfo.ConnStr);

            //Create Header
            var drStoreReq = dsStoreReq.Tables[trf.TableName].NewRow();
            drStoreReq["RequestCode"] = trf.GetNewRequestCode(LoginInfo.ConnStr);
            drStoreReq["Description"] = txt_Desc.Text;
            drStoreReq["DeliveryDate"] = de_DeliveryDate.Date;
            drStoreReq["LocationCode"] = ddl_Store.Value;
            drStoreReq["CreateBy"] = LoginInfo.LoginName;
            drStoreReq["CreateDate"] = ServerDateTime.Date;
            drStoreReq["UpdateBy"] = LoginInfo.LoginName;
            drStoreReq["UpdateDate"] = ServerDateTime.Date;
            dsStoreReq.Tables[trf.TableName].Rows.Add(drStoreReq);


            //Create Details
            for (var i = 0; i < grd_StoreReqFromStdReq.Rows.Count; i++)
            {
                decimal number;
                //decimal onHand = this.GetOnHand(dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["ProductCode"].ToString(),
                //    dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString());

                var txt_Qty = (TextBox) grd_StoreReqFromStdReq.Rows[i].FindControl("txt_Qty");
                var lbl_OnHand = grd_StoreReqFromStdReq.Rows[i].FindControl("lbl_OnHand") as Label;

                if (string.IsNullOrEmpty(txt_Qty.Text))
                {
                }
                if (!decimal.TryParse(txt_Qty.Text, out number))
                {
                    //check number
                    lbl_Warning.Text = "Please entry only 1-9";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
                if (decimal.Parse(txt_Qty.Text) <= 0)
                {
                    lbl_Warning.Text = "'Qty Requested' must more than 0";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
                if (decimal.Parse(txt_Qty.Text) > decimal.Parse(lbl_OnHand.Text))
                {
                    lbl_Warning.Text = "Can not request quantity more than on hand";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(300);

                    txt_Qty.Text = String.Format("{0:N}", decimal.Parse(lbl_OnHand.Text));
                    SetFocus(txt_Qty);
                    return;
                }
                var drStoreReqDt = dsStoreReq.Tables[trfDt.TableName].NewRow();
                drStoreReqDt["CategoryCode"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["CategoryCode"];
                drStoreReqDt["ProductCode"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["ProductCode"];
                drStoreReqDt["RequestUnit"] = dsStoreReqFromStdReq.Tables[stdReqDt.TableName].Rows[i]["RequestUnit"];
                drStoreReqDt["ToLocationCode"] = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"];
                drStoreReqDt["DeliveryDate"] = ServerDateTime.Date.AddDays(1);
                drStoreReqDt["RequestQty"] = decimal.Parse(txt_Qty.Text);
                //drStoreReqDt["ApprQty"] = decimal.Parse(txt_Qty.Text);
                //drStoreReqDt["ApprStatus"] = workflow.GetDtApprStatus("IN", "STOREREQ", LoginInfo.ConnStr);
                dsStoreReq.Tables[trfDt.TableName].Rows.Add(drStoreReqDt);
            }

            Session["dsStoreReqDt"] = dsStoreReq;

            Response.Redirect("TrfEdit.aspx?MODE=SR&VID=" + Request.Params["VID"]);
        }

        protected void grd_StoreReqFromStdReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                        lbl_OnHand.Text = String.Format("{0:N}",
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

                //if (e.Row.FindControl("txt_Qty") != null)
                //{
                //    TextBox txt_Qty = (TextBox)e.Row.FindControl("txt_Qty");
                //    txt_Qty.Text = DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString();
                //}
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
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
                if (dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty &&
                    dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
                {
                    return decimal.Parse(dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
                }
            }


            return 0;
        }

        //protected void txt_Qty_TextChanged(object sender, EventArgs e)
        //{
        //    //TextBox txt_Qty = grd_StoreReqFromStdReq.FindControl("txt_Qty") as TextBox;
        //    TextBox txt_Qty = grd_StoreReqFromStdReq.Rows[grd_StoreReqFromStdReq.].FindControl("txt_Qty") as TextBox;
        //    Label lbl_ProductCode = grd_StoreReqFromStdReq.FindControl("lbl_ProductCode") as Label;
        //    string locationCode = dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString();
        //    decimal onHand = 0;
        //    bool get = prDt.GetStockSummary(dsStoreReq, lbl_ProductCode.Text, locationCode, LoginInfo.ConnStr);

        //    if (get)
        //    {
        //        if (dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty && dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
        //        {
        //            onHand = decimal.Parse(dsStoreReq.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
        //        }
        //    }

        //    if (decimal.Parse(txt_Qty.Text) > onHand)
        //    {
        //        lbl_Warning.Text = "Can not request quantity more than on hand";
        //        pop_Warning.ShowOnPageLoad = true;

        //        txt_Qty.Text = String.Format("{0:N}", onHand);
        //        SetFocus(txt_Qty);
        //    }
        //}
        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow grd_Row in grd_StoreReqFromStdReq.Rows)
            {
                var lbl_ProductCode = grd_Row.FindControl("lbl_ProductCode") as Label;
                var lbl_OnHand = grd_Row.FindControl("lbl_OnHand") as Label;
                lbl_OnHand.Text = String.Format("{0:N}", GetOnHand(lbl_ProductCode.Text, ddl_Store.Value.ToString()));
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            ddl_Store.DataSource =
                locat.GetLookup(dsStoreReqFromStdReq.Tables[stdReq.TableName].Rows[0]["LocationCode"].ToString(),
                    LoginInfo.ConnStr);
            ddl_Store.ValueField = "LocationCode";
            ddl_Store.DataBind();
        }
    }
}