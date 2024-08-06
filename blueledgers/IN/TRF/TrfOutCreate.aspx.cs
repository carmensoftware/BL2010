using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfOutDt : BasePage
    {
        #region "Attributes"

        private DataSet dsStoreReq = new DataSet();
        private DataSet dsTrfOut = new DataSet();
        private Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.IN.storeRequisition storeReq = new Blue.BL.IN.storeRequisition();
        private Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();
        private Blue.BL.IN.TransferOut trfOut = new Blue.BL.IN.TransferOut();
        private Blue.BL.IN.TransferOutDt trfOutDt = new Blue.BL.IN.TransferOutDt();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreReq = (DataSet) Session["dsTrfOut"];
            }
        }

        private void Page_Retrieve()
        {
            storeReq.GetCompleteStoreReqList(dsStoreReq, LoginInfo.ConnStr);
            //storeReqDt.GetListByHeaderId(dsStoreReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            Session["dsTrfOut"] = dsStoreReq;

            Page_Setting();
        }

        private void Page_Setting()
        {
            lbl_Header.Text = "Store Requisition List";
            tb_TrfOutList.Visible = false;

            grd_StoreReqList.DataSource = dsStoreReq.Tables[storeReq.TableName];
            grd_StoreReqList.DataBind();
        }

        //protected void grd_StoreReqDt_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colLocationName = (GridViewDataColumn)grd_StoreReqDt.Columns["Store Name"];
        //        ASPxLabel lbl_gStoreName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colLocationName, "lbl_gStoreName");

        //        if (lbl_gStoreName != null)
        //        {
        //            lbl_gStoreName.Text = locat.GetName(e.GetValue("ToLocationCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductEngName = (GridViewDataColumn)grd_StoreReqDt.Columns["English Name"];
        //        ASPxLabel lbl_EnglishName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colProductEngName, "lbl_EnglishName");

        //        if (lbl_EnglishName != null)
        //        {
        //            lbl_EnglishName.Text = product.GetName(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductLocalName = (GridViewDataColumn)grd_StoreReqDt.Columns["Local Name"];
        //        ASPxLabel lbl_LocalName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colProductLocalName, "lbl_LocalName");

        //        if (lbl_LocalName != null)
        //        {
        //            lbl_LocalName.Text = product.GetName2(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colDeliveryDate = (GridViewDataColumn)grd_StoreReqDt.Columns["Delivery Date"];
        //        ASPxLabel lbl_DeliveryDate = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colDeliveryDate, "lbl_DeliveryDate");

        //        if (lbl_DeliveryDate != null)
        //        {
        //            lbl_DeliveryDate.Text = DateTime.Parse(e.GetValue("DeliveryDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
        //        }

        //    }
        //}

        //protected void grd_StoreReqDt_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        //{
        //    if (!e.Expanded)
        //    {
        //        return;
        //    }
        //    grd_StoreReqDt.DetailRows.CollapseAllRows();
        //    grd_StoreReqDt.DetailRows.ExpandRow(e.VisibleIndex);

        //    DataRow drExpand    = dsStoreReq.Tables[storeReqDt.TableName].Rows[e.VisibleIndex];
        //    string errorMsg     = string.Empty;

        //    // Display Transaction Detail ---------------------------------------------------------

        //    Label lbl_Debit = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Debit") as Label;
        //    lbl_Debit.Text  = drExpand["DebitACCode"].ToString();

        //    Label lbl_DebitName = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_DebitName") as Label;
        //    //lbl_DebitName.Text = drExpand["DiccountAmt"].ToString();

        //    Label lbl_Credit    = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Credit") as Label;
        //    lbl_Credit.Text     = drExpand["CreditACCode"].ToString();

        //    Label lbl_CreditName    = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_CreditName") as Label;
        //    //lbl_CreditName.Text = drExpand["TaxRate"].ToString();


        //    // Display Comment --------------------------------------------------------------     

        //    Label lbl_Comment   = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Comment") as Label;
        //    lbl_Comment.Text    = drExpand["Comment"].ToString();

        //}

        protected void btn_Generate_Click(object sender, EventArgs e)
        {
            pop_ConfirmGenerate.ShowOnPageLoad = true;
        }

        protected void btn_ConfirmGenerate_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_StoreReqList.GetSelectedFieldValues("RefId");

            var columnValues = new List<string>();

            for (var i = 0; i <= grd_StoreReqList.Rows.Count - 1; i++)
            {
                var chk_Item = grd_StoreReqList.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    columnValues.Add(dsStoreReq.Tables[storeReq.TableName].Rows[i]["RefId"].ToString());
                }
            }

            dsStoreReq.Tables[storeReq.TableName].Clear();

            for (var i = 0; i < columnValues.Count; i++)
            {
                storeReq.GetListById(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);
                storeReqDt.GetListByHeaderId(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);
            }

            //int count = dsStoreReq.Tables[storeReqDt.TableName].Rows.Count;
            //int countRound = 0;
            var storeReqId = string.Empty;

            for (var i = 0; i < dsStoreReq.Tables[storeReqDt.TableName].Rows.Count; i++)
            {
                //for (int j = 0; j < dsStoreReq.Tables[storeReqDt.TableName].Rows.Count; j++)
                //{
                if (i + 1 >= dsStoreReq.Tables[storeReqDt.TableName].Rows.Count)
                {
                    if (dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["ToLocationCode"].ToString() ==
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["ToLocationCode"].ToString()
                        &&
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["DeliveryDate"].ToString() ==
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["DeliveryDate"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        storeReqId = storeReqId + "," +
                                     dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["RefId"].ToString();
                    }
                }
                else if (i + 1 < dsStoreReq.Tables[storeReqDt.TableName].Rows.Count)
                {
                    if (dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["ToLocationCode"].ToString() ==
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[i + 1]["ToLocationCode"].ToString()
                        &&
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["DeliveryDate"].ToString() ==
                        dsStoreReq.Tables[storeReqDt.TableName].Rows[i + 1]["DeliveryDate"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        storeReqId = storeReqId + "," +
                                     dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["RefId"].ToString();
                    }
                }
                //}
            }

            trfOut.GetStructure(dsTrfOut, LoginInfo.ConnStr);
            trfOutDt.GetStructure(dsTrfOut, LoginInfo.ConnStr);


            if (storeReqId == string.Empty)
            {
                var drTrfOut = dsTrfOut.Tables[trfOut.TableName].NewRow();
                drTrfOut["RefId"] = trfOut.GetNewId(LoginInfo.ConnStr);
                drTrfOut["FromStoreId"] = dsStoreReq.Tables[storeReq.TableName].Rows[0]["LocationCode"];
                drTrfOut["ToStoreId"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["ToLocationCode"];
                drTrfOut["Status"] = "Saved";
                drTrfOut["DeliveryDate"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["DeliveryDate"];
                drTrfOut["CreateBy"] = LoginInfo.LoginName;
                drTrfOut["CreateDate"] = ServerDateTime.Date;
                drTrfOut["UpdateBy"] = LoginInfo.LoginName;
                drTrfOut["UpdateDate"] = ServerDateTime.Date;
                dsTrfOut.Tables[trfOut.TableName].Rows.Add(drTrfOut);

                foreach (DataRow drStoreReq in dsStoreReq.Tables[storeReqDt.TableName].Rows)
                {
                    var drTrfOutDt = dsTrfOut.Tables[trfOutDt.TableName].NewRow();
                    drTrfOutDt["RefId"] = drTrfOut["RefId"];
                    drTrfOutDt["QtyAllocate"] = drStoreReq["RequestQty"];
                    drTrfOutDt["QtyOut"] = drStoreReq["RequestQty"];
                    drTrfOutDt["Unit"] = drStoreReq["RequestUnit"];
                    drTrfOutDt["ProductCode"] = drStoreReq["ProductCode"];
                    drTrfOutDt["SRId"] = drStoreReq["DocumentId"];
                    drTrfOutDt["CreditAcc"] = drStoreReq["CreditACCode"];
                    drTrfOutDt["DebitAcc"] = drStoreReq["DebitACCode"];
                    dsTrfOut.Tables[trfOutDt.TableName].Rows.Add(drTrfOutDt);
                }
            }
            else
            {
                //int count = 0;
                var id = storeReqId.Split(',');
                //IEnumerable<string> TrfOutId = id.Distinct();

                for (var i = 0; i < id.Length; i++)
                {
                    for (var j = 0; j < dsStoreReq.Tables[storeReqDt.TableName].Rows.Count; j++)
                    {
                        if (id[i] == dsStoreReq.Tables[storeReqDt.TableName].Rows[j]["RefId"].ToString())
                        {
                            var drTrfOut = dsTrfOut.Tables[trfOut.TableName].NewRow();
                            drTrfOut["RefId"] = trfOut.GetNewId(LoginInfo.ConnStr);
                            drTrfOut["FromStoreId"] = dsStoreReq.Tables[storeReq.TableName].Rows[0]["LocationCode"];
                            drTrfOut["ToStoreId"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[j]["ToLocationCode"];
                            drTrfOut["Status"] = "Saved";
                            drTrfOut["DeliveryDate"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[j]["DeliveryDate"];
                            drTrfOut["CreateBy"] = LoginInfo.LoginName;
                            drTrfOut["CreateDate"] = ServerDateTime.Date;
                            drTrfOut["UpdateBy"] = LoginInfo.LoginName;
                            drTrfOut["UpdateDate"] = ServerDateTime.Date;
                            dsTrfOut.Tables[trfOut.TableName].Rows.Add(drTrfOut);

                            trfOut.Save(dsTrfOut, LoginInfo.ConnStr);
                        }
                    }
                }

                foreach (DataRow drTrfOut in dsTrfOut.Tables[trfOut.TableName].Rows)
                {
                    foreach (DataRow drStoreReq in dsStoreReq.Tables[storeReqDt.TableName].Rows)
                    {
                        if (drStoreReq["ToLocationCode"].ToString() == drTrfOut["ToStoreId"].ToString() &&
                            drStoreReq["DeliveryDate"].ToString() == drTrfOut["DeliveryDate"].ToString())
                        {
                            var drTrfOutDt = dsTrfOut.Tables[trfOutDt.TableName].NewRow();
                            drTrfOutDt["RefId"] = drTrfOut["RefId"];
                            drTrfOutDt["QtyAllocate"] = drStoreReq["RequestQty"];
                            drTrfOutDt["QtyOut"] = drStoreReq["RequestQty"];
                            drTrfOutDt["Unit"] = drStoreReq["RequestUnit"];
                            drTrfOutDt["ProductCode"] = drStoreReq["ProductCode"];
                            drTrfOutDt["SRId"] = drStoreReq["DocumentId"];
                            drTrfOutDt["CreditAcc"] = drStoreReq["CreditACCode"];
                            drTrfOutDt["DebitAcc"] = drStoreReq["DebitACCode"];
                            dsTrfOut.Tables[trfOutDt.TableName].Rows.Add(drTrfOutDt);
                        }
                    }
                }
            }

            var Save = trfOut.Save(dsTrfOut, LoginInfo.ConnStr);

            if (Save)
            {
                lbl_Header.Text = "Transfer Out List";
                tb_StoreReqList.Visible = false;
                tb_TrfOutList.Visible = true;

                grd_TrfOut.DataSource = dsTrfOut.Tables[trfOut.TableName];
                grd_TrfOut.DataBind();

                pop_ConfirmGenerate.ShowOnPageLoad = false;
            }
        }

        protected void btn_Abort_Click(object sender, EventArgs e)
        {
            pop_ConfirmGenerate.ShowOnPageLoad = false;

            Response.Redirect("TrfOutLst.aspx");
        }

        //protected void grd_TrfOut_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colLocationName = (GridViewDataColumn)grd_TrfOut.Columns["Store Name"];
        //        ASPxLabel lbl_StoreName = (ASPxLabel)grd_TrfOut.FindRowCellTemplateControl(e.VisibleIndex, colLocationName, "lbl_StoreName");

        //        if (lbl_StoreName != null)
        //        {
        //            lbl_StoreName.Text = locat.GetName(e.GetValue("ToStoreId").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colDeliveryDate = (GridViewDataColumn)grd_TrfOut.Columns["Delivery Date"];
        //        ASPxLabel lbl_DeliveryDate = (ASPxLabel)grd_TrfOut.FindRowCellTemplateControl(e.VisibleIndex, colDeliveryDate, "lbl_DeliveryDate");

        //        if (lbl_DeliveryDate != null)
        //        {
        //            lbl_DeliveryDate.Text = DateTime.Parse(e.GetValue("DeliveryDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
        //        }

        //        GridViewDataColumn colTotalQty = (GridViewDataColumn)grd_TrfOut.Columns["Total Qty"];
        //        ASPxLabel lbl_TotalQty = (ASPxLabel)grd_TrfOut.FindRowCellTemplateControl(e.VisibleIndex, colTotalQty, "lbl_TotalQty");

        //        if (lbl_TotalQty != null)
        //        {
        //            lbl_TotalQty.Text = String.Format("{0:0.00}", trfOutDt.GetTotalQty(e.GetValue("RefId").ToString(), LoginInfo.ConnStr));
        //        }
        //    }
        //}

        //protected void grd_StoreReqList_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colDate = (GridViewDataColumn)grd_StoreReqList.Columns["Date"];
        //        ASPxLabel lbl_Date = (ASPxLabel)grd_StoreReqList.FindRowCellTemplateControl(e.VisibleIndex, colDate, "lbl_Date");

        //        if (lbl_Date != null)
        //        {
        //            lbl_Date.Text = DateTime.Parse(e.GetValue("CreateDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
        //        }
        //    }
        //}

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfOutLst.aspx");
        }

        protected void grd_TrfOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_StoreName") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName") as Label;
                    lbl_StoreName.Text = locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToStoreId").ToString(),
                        LoginInfo.ConnStr);
                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "ToStoreId").ToString(), LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_DeliveryDate") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate") as Label;
                    lbl_DeliveryDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_TotalQty") != null)
                {
                    var lbl_TotalQty = e.Row.FindControl("lbl_TotalQty") as Label;
                    lbl_TotalQty.Text = String.Format("{0:0.00}",
                        trfOutDt.GetTotalQty(DataBinder.Eval(e.Row.DataItem, "RefId").ToString(), LoginInfo.ConnStr));
                }
            }
        }

        protected void grd_StoreReqList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lbl_Date = e.Row.FindControl("lbl_Date") as Label;
                    lbl_Date.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreateDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }
            }
        }
    }
}