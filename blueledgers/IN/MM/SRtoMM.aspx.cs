using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.MM
{
    public partial class MoveMentDt : BasePage
    {
        #region "Attributes"

        //BL.IN.TransferOut trfOut                = new BL.IN.TransferOut();
        //BL.IN.TransferOutDt trfOutDt            = new BL.IN.TransferOutDt();
        private DataSet dsMM = new DataSet();
        private DataSet dsStoreReq = new DataSet();
        private Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.IN.Movement moveMent = new Blue.BL.IN.Movement();
        private Blue.BL.IN.MovementDt moveMentDt = new Blue.BL.IN.MovementDt();
        private Blue.BL.IN.storeRequisition storeReq = new Blue.BL.IN.storeRequisition();
        private Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreReq = (DataSet) Session["dsMM"];
            }
        }

        private void Page_Retrieve()
        {
            storeReq.GetCompleteStoreReqList(dsStoreReq, LoginInfo.ConnStr);
            //storeReqDt.GetListByHeaderId(dsStoreReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            Session["dsMM"] = dsStoreReq;

            Page_Setting();
        }

        private void Page_Setting()
        {
            lbl_Header.Text = "Store Requisition List";
            tb_TrfOutList.Visible = false;

            //grd_StoreReqList.DataSource = dsStoreReq.Tables[storeReq.TableName];
            //grd_StoreReqList.DataBind();

            // Normal Grid View.
            grd_StoreReq.DataSource = dsStoreReq.Tables[storeReq.TableName];
            grd_StoreReq.DataBind();
        }

        protected void btn_Generate_Click(object sender, EventArgs e)
        {
            pop_ConfirmGenerate.ShowOnPageLoad = true;
        }

        protected void btn_ConfirmGenerate_Click(object sender, EventArgs e)
        {
            var columnValues = new List<string>();

            for (var i = 0; i <= grd_StoreReq.Rows.Count - 1; i++)
            {
                var chk_Item = grd_StoreReq.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

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

            moveMent.GetStructure(dsMM, LoginInfo.ConnStr);
            moveMentDt.GetSchema(dsMM, LoginInfo.ConnStr);


            if (storeReqId == string.Empty)
            {
                var drTrfOut = dsMM.Tables[moveMent.TableName].NewRow();
                drTrfOut["RefId"] = moveMent.GetNewID(Request.Params["Prefix"].ToUpper().ToString(), LoginInfo.ConnStr);
                drTrfOut["FromStoreId"] = dsStoreReq.Tables[storeReq.TableName].Rows[0]["LocationCode"];
                drTrfOut["ToStoreId"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["ToLocationCode"];
                drTrfOut["Status"] = "Saved";
                drTrfOut["DeliveryDate"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[0]["DeliveryDate"];
                drTrfOut["CreatedBy"] = LoginInfo.LoginName;
                drTrfOut["CreatedDate"] = ServerDateTime.Date;
                drTrfOut["UpdatedBy"] = LoginInfo.LoginName;
                drTrfOut["UpdatedDate"] = ServerDateTime.Date;
                drTrfOut["TypeCode"] = string.Empty;
                dsMM.Tables[moveMent.TableName].Rows.Add(drTrfOut);

                foreach (DataRow drStoreReq in dsStoreReq.Tables[storeReqDt.TableName].Rows)
                {
                    var drTrfOutDt = dsMM.Tables[moveMentDt.TableName].NewRow();
                    drTrfOutDt["RefId"] = drTrfOut["RefId"];
                    drTrfOutDt["QtyAllocate"] = drStoreReq["RequestQty"];
                    drTrfOutDt["Qty"] = drStoreReq["RequestQty"];
                    drTrfOutDt["Unit"] = drStoreReq["RequestUnit"];
                    drTrfOutDt["ProductCode"] = drStoreReq["ProductCode"];
                    drTrfOutDt["SRId"] = drStoreReq["DocumentId"];
                    drTrfOutDt["CreditAcc"] = drStoreReq["CreditACCode"];
                    drTrfOutDt["DebitAcc"] = drStoreReq["DebitACCode"];
                    dsMM.Tables[moveMentDt.TableName].Rows.Add(drTrfOutDt);
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
                            var drTrfOut = dsMM.Tables[moveMent.TableName].NewRow();
                            drTrfOut["RefId"] = moveMent.GetNewID(Request.Params["Prefix"].ToUpper().ToString(),
                                LoginInfo.ConnStr);
                            drTrfOut["FromStoreId"] = dsStoreReq.Tables[storeReq.TableName].Rows[0]["LocationCode"];
                            drTrfOut["ToStoreId"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[j]["ToLocationCode"];
                            drTrfOut["Status"] = "Saved";
                            drTrfOut["DeliveryDate"] = dsStoreReq.Tables[storeReqDt.TableName].Rows[j]["DeliveryDate"];
                            drTrfOut["CreatedBy"] = LoginInfo.LoginName;
                            drTrfOut["CreatedDate"] = ServerDateTime.Date;
                            drTrfOut["UpdatedBy"] = LoginInfo.LoginName;
                            drTrfOut["UpdatedDate"] = ServerDateTime.Date;
                            drTrfOut["TypeCode"] = string.Empty;
                            dsMM.Tables[moveMent.TableName].Rows.Add(drTrfOut);

                            moveMent.Save(dsMM, LoginInfo.ConnStr);
                        }
                    }
                }

                foreach (DataRow drTrfOut in dsMM.Tables[moveMent.TableName].Rows)
                {
                    foreach (DataRow drStoreReq in dsStoreReq.Tables[storeReqDt.TableName].Rows)
                    {
                        if (drStoreReq["ToLocationCode"].ToString() == drTrfOut["ToStoreId"].ToString() &&
                            drStoreReq["DeliveryDate"].ToString() == drTrfOut["DeliveryDate"].ToString())
                        {
                            var drTrfOutDt = dsMM.Tables[moveMentDt.TableName].NewRow();
                            drTrfOutDt["RefId"] = drTrfOut["RefId"];
                            drTrfOutDt["QtyAllocate"] = drStoreReq["RequestQty"];
                            drTrfOutDt["Qty"] = drStoreReq["RequestQty"];
                            drTrfOutDt["Unit"] = drStoreReq["RequestUnit"];
                            drTrfOutDt["ProductCode"] = drStoreReq["ProductCode"];
                            drTrfOutDt["SRId"] = drStoreReq["DocumentId"];
                            drTrfOutDt["CreditAcc"] = drStoreReq["CreditACCode"];
                            drTrfOutDt["DebitAcc"] = drStoreReq["DebitACCode"];
                            dsMM.Tables[moveMentDt.TableName].Rows.Add(drTrfOutDt);
                        }
                    }
                }
            }

            var Save = moveMent.Save(dsMM, LoginInfo.ConnStr);

            if (Save)
            {
                lbl_Header.Text = "Transfer Out List";
                tb_StoreReqList.Visible = false;
                tb_TrfOutList.Visible = true;

                grd_TfOut.DataSource = dsMM.Tables[moveMent.TableName];
                grd_TfOut.DataBind();

                pop_ConfirmGenerate.ShowOnPageLoad = false;
            }
        }

        protected void btn_Abort_Click(object sender, EventArgs e)
        {
            pop_ConfirmGenerate.ShowOnPageLoad = false;

            Response.Redirect("TrfOutLst.aspx");
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            Response.Redirect("MoveMentLst.aspx");
        }

        protected void grd_StoreReq_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grd_TfOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_StoreName") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName") as Label;
                    lbl_StoreName.Text = locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToStoreId").ToString(),
                        LoginInfo.ConnStr);
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
                        moveMentDt.GetTotalQty(DataBinder.Eval(e.Row.DataItem, "RefId").ToString(), LoginInfo.ConnStr));
                }
            }
        }
    }
}