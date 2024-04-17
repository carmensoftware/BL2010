using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.REC
{
    public partial class RecCommitByBatch : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly DataSet dsRecCommit = new DataSet();
        private readonly DataSet dsUpdatePO = new DataSet();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();

        private string MsgError = string.Empty;
        private DataSet dsInventory = new DataSet();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            //Check Login
            base.Page_Load(sender, e);
            Page_Setting();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        private void Page_Setting()
        {
            grd_RecCommitByBatch.DataSource = rec.GetListForCommitByBatch(hf_ConnStr.Value);
            grd_RecCommitByBatch.DataBind();
        }

        //private void UpdateInventoryForCommit(DataRow drRecDt)
        //{
        //    //FIFO Calculation
        //    var invQty = prodUnit.GetQtyAfterChangeUnit(drRecDt["ProductCode"].ToString(),
        //        prodUnit.GetInvenUnit(drRecDt["ProductCode"].ToString(), hf_ConnStr.Value),
        //        drRecDt["RcvUnit"].ToString(), decimal.Parse(drRecDt["RecQty"].ToString()), hf_ConnStr.Value);

        //    var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()), 2);
        //    var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
        //    var diffCount = Math.Round((netPrice - (pricePerQty * (invQty))), 2) * 100;

        //    if (diffCount == 0)
        //    {
        //        //Find Inventory and update
        //        foreach (DataRow drInv in dsRecCommit.Tables[inv.TableName].Rows)
        //        {
        //            if (drInv["HdrNo"].ToString() == drRecDt["RecNo"].ToString() &&
        //                drInv["DtNo"].ToString() == drRecDt["RecDtNo"].ToString())
        //            {
        //                drInv["InvNo"] = 1;
        //                drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
        //                drInv["Location"] = drRecDt["LocationCode"].ToString();
        //                drInv["IN"] = invQty;
        //                drInv["OUT"] = Convert.ToDecimal("0.00");
        //                //drInv["Amount"] = Convert.ToDecimal(drRecDt["NetAmt"] == null ? Convert.ToDecimal("0.00") : drRecDt["NetAmt"]);
        //                drInv["Amount"] = pricePerQty;
        //                drInv["FIFOAudit"] = pricePerQty;
        //                drInv["FIFOMng"] = System.DBNull.Value;
        //                drInv["FIFOBank"] = System.DBNull.Value;
        //                drInv["MAvgAudit"] = System.DBNull.Value;
        //                drInv["MAvgMng"] = System.DBNull.Value;
        //                drInv["MAvgBank"] = System.DBNull.Value;
        //                //drInv["PAvgAudit"] = inv.SetPAvgAudit(startDate, endDate, drInv["Location"].ToString(), drInv["ProductCode"].ToString(), LoginInfo.ConnStr);
        //                drInv["PAvgAudit"] = System.DBNull.Value;
        //                drInv["PAvgMng"] = System.DBNull.Value;
        //                drInv["PAvgBank"] = System.DBNull.Value;
        //                drInv["RptAudit"] = System.DBNull.Value;
        //                drInv["RptMng"] = System.DBNull.Value;
        //                drInv["RptBank"] = System.DBNull.Value;
        //                drInv["CommittedDate"] = ServerDateTime.Date;
        //                drInv["Type"] = "RC";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //Find Inventory
        //        for (var i = 0; i < 2; i++)
        //        {
        //            if (i == 0)
        //            {
        //                foreach (DataRow drInv in dsRecCommit.Tables[inv.TableName].Rows)
        //                {
        //                    //ดูว่าเป็น inventory ที่ต้องการ update ?
        //                    if (drRecDt["RecNo"].ToString() == drInv["HdrNo"].ToString() &&
        //                        drRecDt["RecDtNo"].ToString() == drInv["DtNo"].ToString())
        //                    {
        //                        drInv["InvNo"] = i + 1;
        //                        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
        //                        drInv["Location"] = drRecDt["LocationCode"].ToString();
        //                        drInv["IN"] = invQty - Math.Abs(diffCount);
        //                        drInv["OUT"] = Convert.ToDecimal("0.00");
        //                        //drInv["Amount"] = Convert.ToDecimal(drRecDt["NetAmt"] == null ? Convert.ToDecimal("0.00") : drRecDt["NetAmt"]);
        //                        drInv["Amount"] = pricePerQty;
        //                        drInv["FIFOAudit"] = pricePerQty;
        //                        drInv["FIFOMng"] = System.DBNull.Value;
        //                        drInv["FIFOBank"] = System.DBNull.Value;
        //                        drInv["MAvgAudit"] = System.DBNull.Value;
        //                        drInv["MAvgMng"] = System.DBNull.Value;
        //                        drInv["MAvgBank"] = System.DBNull.Value;
        //                        //drInv["PAvgAudit"] = inv.SetPAvgAudit(startDate, endDate, drInv["Location"].ToString(), drInv["ProductCode"].ToString(), LoginInfo.ConnStr);
        //                        drInv["PAvgAudit"] = System.DBNull.Value;
        //                        drInv["PAvgMng"] = System.DBNull.Value;
        //                        drInv["PAvgBank"] = System.DBNull.Value;
        //                        drInv["RptAudit"] = System.DBNull.Value;
        //                        drInv["RptMng"] = System.DBNull.Value;
        //                        drInv["RptBank"] = System.DBNull.Value;
        //                        drInv["CommittedDate"] = ServerDateTime.Date;
        //                        drInv["Type"] = "RC";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //add new row
        //                var drInvNew = dsRecCommit.Tables[inv.TableName].NewRow();

        //                drInvNew["HdrNo"] = drRecDt["RecNo"].ToString();
        //                drInvNew["DtNo"] = (drRecDt["RecDtNo"]);
        //                drInvNew["InvNo"] = i + 1;
        //                drInvNew["ProductCode"] = drRecDt["ProductCode"].ToString();
        //                drInvNew["Location"] = drRecDt["LocationCode"].ToString();
        //                drInvNew["IN"] = Math.Abs(diffCount);

        //                if (diffCount < 0)
        //                {
        //                    drInvNew["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
        //                    drInvNew["FIFOAudit"] = pricePerQty - Convert.ToDecimal("0.01");
        //                }
        //                else
        //                {
        //                    drInvNew["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
        //                    drInvNew["FIFOAudit"] = pricePerQty + Convert.ToDecimal("0.01");
        //                }

        //                drInvNew["OUT"] = Convert.ToDecimal("0.00");
        //                //drInvNew["Amount"] = Convert.ToDecimal(drRecDt["NetAmt"] == null ? Convert.ToDecimal("0.00") : drRecDt["NetAmt"]);
        //                drInvNew["FIFOMng"] = System.DBNull.Value;
        //                drInvNew["FIFOBank"] = System.DBNull.Value;
        //                drInvNew["MAvgAudit"] = System.DBNull.Value;
        //                drInvNew["MAvgMng"] = System.DBNull.Value;
        //                drInvNew["MAvgBank"] = System.DBNull.Value;
        //                //drInvNew["PAvgAudit"] = inv.SetPAvgAudit(startDate, endDate, drInvNew["Location"].ToString(), drInvNew["ProductCode"].ToString(), LoginInfo.ConnStr);
        //                drInvNew["PAvgAudit"] = System.DBNull.Value;
        //                drInvNew["PAvgMng"] = System.DBNull.Value;
        //                drInvNew["PAvgBank"] = System.DBNull.Value;
        //                drInvNew["RptAudit"] = System.DBNull.Value;
        //                drInvNew["RptMng"] = System.DBNull.Value;
        //                drInvNew["RptBank"] = System.DBNull.Value;
        //                drInvNew["CommittedDate"] = ServerDateTime.Date;
        //                drInvNew["Type"] = "RC";

        //                dsRecCommit.Tables[inv.TableName].Rows.Add(drInvNew);
        //            }
        //        }
        //    }
        //}


        private void UpdateInventoryForCommit(DataRow drRecDt)
        {
            var dsSave = dsRecCommit;

            RecFunc recFunc = new RecFunc(hf_ConnStr.Value.ToString());
            recFunc.UpdateInventoryForCommit(dsSave, drRecDt);

            // Remark for using shared funciton. (RecEdit, RecCommitByBatch, RecCreateManual)

            ////FIFO Calculation
            //var invQty = prodUnit.GetQtyAfterChangeUnit(drRecDt["ProductCode"].ToString(),
            //    prodUnit.GetInvenUnit(drRecDt["ProductCode"].ToString(), hf_ConnStr.Value),
            //    drRecDt["RcvUnit"].ToString(),
            //    decimal.Parse(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString()),
            //    hf_ConnStr.Value);

            //var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()), 2);
            //var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
            //var diffCount = Math.Round((netPrice - (pricePerQty * (invQty == 0 ? 1 : invQty))), 2) * 100;

            //if (diffCount == 0)
            //{
            //    var drInv = dsSave.Tables[inv.TableName].NewRow();

            //    drInv["HdrNo"] = drRecDt["RecNo"].ToString();
            //    drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
            //    drInv["InvNo"] = 1;
            //    drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
            //    drInv["Location"] = drRecDt["LocationCode"].ToString();
            //    drInv["IN"] = invQty;
            //    drInv["OUT"] = Convert.ToDecimal("0.00");
            //    drInv["Amount"] = pricePerQty;
            //    drInv["FIFOAudit"] = pricePerQty;
            //    drInv["FIFOMng"] = System.DBNull.Value;
            //    drInv["FIFOBank"] = System.DBNull.Value;
            //    drInv["MAvgAudit"] = System.DBNull.Value;
            //    drInv["MAvgMng"] = System.DBNull.Value;
            //    drInv["MAvgBank"] = System.DBNull.Value;
            //    drInv["PAvgAudit"] = 0; // Move the average cost calculation to after completed save Inventory Ledgers 
            //    drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["RecDate"];
            //    drInv["PAvgMng"] = System.DBNull.Value;
            //    drInv["PAvgBank"] = System.DBNull.Value;
            //    drInv["RptAudit"] = System.DBNull.Value;
            //    drInv["RptMng"] = System.DBNull.Value;
            //    drInv["RptBank"] = System.DBNull.Value;
            //    drInv["Type"] = "RC";

            //    dsSave.Tables[inv.TableName].Rows.Add(drInv);

            //    //// ขอเพิ่มเข้า fifo table
            //    //DataRow drFifo = dsFifo.Tables[Fifo.TableName].NewRow();
            //    //drFifo["HdrNo"] = drInv["HdrNo"];
            //    //drFifo["LocationCode"] = drInv["Location"];
            //    //drFifo["ProductCode"] = drInv["ProductCode"];
            //    //drFifo["RcvQty"] = drInv["IN"];
            //    //drFifo["RcvPrice"] = drInv["Amount"];
            //    //drFifo["RcvRemain"] = drInv["IN"];
            //    //drFifo["CreatedDate"] = drInv["CommittedDate"];
            //    //dsFifo.Tables[Fifo.TableName].Rows.Add(drFifo);
            //}
            //else
            //{
            //    for (var i = 0; i < 2; i++)
            //    {
            //        var drInv = dsSave.Tables[inv.TableName].NewRow();

            //        drInv["HdrNo"] = drRecDt["RecNo"].ToString();
            //        drInv["DtNo"] = (drRecDt["RecDtNo"]);
            //        drInv["InvNo"] = i + 1;
            //        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
            //        drInv["Location"] = drRecDt["LocationCode"].ToString();

            //        if (i == 0)
            //        {
            //            drInv["IN"] = invQty - Math.Abs(diffCount);
            //            drInv["FIFOAudit"] = pricePerQty;
            //            drInv["Amount"] = pricePerQty;
            //        }
            //        else
            //        {
            //            drInv["IN"] = Math.Abs(diffCount);

            //            if (diffCount < 0)
            //            {
            //                //case : QuantityPerUnit * Quantity > Net
            //                drInv["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
            //                drInv["FIFOAudit"] = pricePerQty - Convert.ToDecimal("0.01");
            //            }
            //            else
            //            {
            //                //case : QuantityPerUnit * Quantity < Net
            //                drInv["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
            //                drInv["FIFOAudit"] = pricePerQty + Convert.ToDecimal("0.01");
            //            }
            //        }

            //        drInv["OUT"] = Convert.ToDecimal("0.00");
            //        drInv["FIFOMng"] = System.DBNull.Value;
            //        drInv["FIFOBank"] = System.DBNull.Value;
            //        drInv["MAvgAudit"] = System.DBNull.Value;
            //        drInv["MAvgMng"] = System.DBNull.Value;
            //        drInv["MAvgBank"] = System.DBNull.Value;
            //        drInv["PAvgAudit"] = 0;
            //        drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["RecDate"];

            //        drInv["PAvgMng"] = System.DBNull.Value;
            //        drInv["PAvgBank"] = System.DBNull.Value;
            //        drInv["RptAudit"] = System.DBNull.Value;
            //        drInv["RptMng"] = System.DBNull.Value;
            //        drInv["RptBank"] = System.DBNull.Value;
            //        drInv["Type"] = "RC";

            //        dsSave.Tables[inv.TableName].Rows.Add(drInv);

            //        //// ขอเพิ่มเข้า fifo table
            //        //DataRow drFifo = dsFifo.Tables[Fifo.TableName].NewRow();
            //        //drFifo["HdrNo"] = drInv["HdrNo"];
            //        //drFifo["LocationCode"] = drInv["Location"];
            //        //drFifo["ProductCode"] = drInv["ProductCode"];
            //        //drFifo["RcvQty"] = drInv["IN"];
            //        //drFifo["RcvPrice"] = drInv["Amount"];
            //        //drFifo["RcvRemain"] = drInv["IN"];
            //        //drFifo["CreatedDate"] = drInv["CommittedDate"];
            //        //dsFifo.Tables[Fifo.TableName].Rows.Add(drFifo);
            //    }
            //}
        }


        protected void btn_CancelCommit_Click(object sender, EventArgs e)
        {
            pop_ConfrimCommit.ShowOnPageLoad = false;
        }

        protected void grd_RecCommitByBatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox chk_Item = e.Row.FindControl("chk_Item") as CheckBox;

                if (e.Row.FindControl("lbl_RecDate") != null)
                {
                    var lbl_RecDate = e.Row.FindControl("lbl_RecDate") as Label;
                    lbl_RecDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "RecDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                //if (e.Row.FindControl("lbl_RecNo") != null)
                //{
                //    Label lbl_RecNo = e.Row.FindControl("lbl_RecNo") as Label;
                //    lbl_RecNo.Text = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                //}

                if (e.Row.FindControl("hpl_RecNo") != null)
                {
                    var hpl_RecNo = e.Row.FindControl("hpl_RecNo") as HyperLink;
                    hpl_RecNo.NavigateUrl = "~/PC/REC/RecEdit.aspx?MODE=Edit" + "&ID=" +
                                            DataBinder.Eval(e.Row.DataItem, "RecNo") + "&BuCode=" +
                                            Request.Params["BuCode"];
                    hpl_RecNo.Text = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                }

                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();
                }

                if (e.Row.FindControl("lbl_Vendor") != null)
                {
                    var lbl_Vendor = e.Row.FindControl("lbl_Vendor") as Label;
                    lbl_Vendor.Text = DataBinder.Eval(e.Row.DataItem, "VendorName").ToString();
                }

                if (e.Row.FindControl("lbl_InvoiceNo") != null)
                {
                    var lbl_InvoiceNo = e.Row.FindControl("lbl_InvoiceNo") as Label;
                    lbl_InvoiceNo.Text =
                        (DataBinder.Eval(e.Row.DataItem, "InvoiceNo") == DBNull.Value
                            ? string.Empty
                            : DataBinder.Eval(e.Row.DataItem, "InvoiceNo")).ToString();
                }

                if (e.Row.FindControl("lbl_InvoiceDate") != null)
                {
                    var lbl_InvoiceDate = e.Row.FindControl("lbl_InvoiceDate") as Label;
                    lbl_InvoiceDate.Text = (DataBinder.Eval(e.Row.DataItem, "InvoiceDate") == DBNull.Value
                        ? string.Empty
                        : DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "InvoiceDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate));
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format("{0:#,###.00}", DataBinder.Eval(e.Row.DataItem, "Total"));
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    // lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "DocStatus").ToString();
                    if (DataBinder.Eval(e.Row.DataItem, "PoSource").ToString().Trim() == "")
                        lbl_Status.Text = "MANUAL";
                    else
                        lbl_Status.Text = "";
                }
            }
        }

        #region "Button"

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecLst.aspx");
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            var recList = string.Empty;
            var values = new List<string>();

            foreach (GridViewRow grd_Row in grd_RecCommitByBatch.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var hpl_RecNo = grd_Row.FindControl("hpl_RecNo") as HyperLink;
                    values.Add(hpl_RecNo.Text);
                }
            }

            if (values.Count == 0)
            {
                lbl_Warning.Text = "Please selecting any Receiving items";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            foreach (var recNo in values)
            {
                rec.GetListByRecNo(dsRecCommit, ref MsgError, recNo, hf_ConnStr.Value);

                foreach (DataRow drRecCommit in dsRecCommit.Tables[rec.TableName].Rows)
                {
                    //Invoice Number and Invoice Date not allow to empty.
                    if (drRecCommit["InvoiceNo"].ToString() == string.Empty ||
                        drRecCommit["InvoiceDate"].ToString() == string.Empty)
                    {
                        if (recList == string.Empty)
                        {
                            recList = recNo;
                        }
                        else
                        {
                            recList = recList + ", " + recNo;
                        }
                    }
                }

                dsRecCommit.Clear();
            }

            //if invoice number or invoice date is empty, cannot commit. 
            if (recList == string.Empty)
            {
                //var string msgConfirm = lbl_ConfirmCommit.Text;

                lbl_ConfirmCommit.Text = "Continue to commit " + values.Count.ToString() + " item(s)." + "<br>" + "Do you want to commit them?";
                pop_ConfrimCommit.ShowOnPageLoad = true;
            }
            else
            {
                lbl_Warning.Text = "Please insert invoice number and invoice date in Receiving number: " + recList;
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        protected void btn_ConfrimCommit_Click(object sender, EventArgs e)
        {
            //List<object> values = grd_RecCommitByBatch.GetSelectedFieldValues("RecNo");
            var value = string.Empty;

            var values = new List<string>();

            // Keep selected items to values[];
            foreach (GridViewRow grd_Row in grd_RecCommitByBatch.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var hpl_RecNo = grd_Row.FindControl("hpl_RecNo") as HyperLink;
                    values.Add(hpl_RecNo.Text);
                }
            }

            foreach (var recNo in values)
            {
                //Update table PC.Rec
                rec.GetListByRecNo(dsRecCommit, ref MsgError, recNo, hf_ConnStr.Value);


                for (var i = 0; i < dsRecCommit.Tables[rec.TableName].Rows.Count; i++)
                {
                    var drSave = dsRecCommit.Tables[rec.TableName].Rows[i];

                    if (drSave["RecNo"].ToString() == recNo)
                    {
                        drSave["DocStatus"] = "Committed";
                        drSave["UpdatedDate"] = ServerDateTime.Date;
                        drSave["UpdatedBy"] = LoginInfo.LoginName;
                        drSave["CommitDate"] = ServerDateTime.Date;
                        drSave["BatchNo"] = rec.GetBatchNo(hf_ConnStr.Value);
                    }
                }

                //Update table PC.RecDt
                recDt.GetListByRecNo(dsRecCommit, recNo, hf_ConnStr.Value);

                //Get inventory for update
                inv.GetListByHdrNo(dsRecCommit, recNo, hf_ConnStr.Value);

                for (var i = 0; i < dsRecCommit.Tables[recDt.TableName].Rows.Count; i++)
                {
                    var drSaveDt = dsRecCommit.Tables[recDt.TableName].Rows[i];

                    if (drSaveDt["RecNo"].ToString() == recNo)
                    {
                        drSaveDt["Status"] = "Committed";
                        UpdateInventoryForCommit(drSaveDt);
                    }
                }


            } //foreach (var recNo in values)

            var save = rec.Save(dsRecCommit, hf_ConnStr.Value);

            if (save)
            {
                ////Get StartDate and EndDate for update Avg in inventory
                //var startDate = period.GetStartDate(ServerDateTime.Date, hf_ConnStr.Value);
                //var endDate = period.GetEndDate(ServerDateTime.Date, hf_ConnStr.Value);

                // Update to podetail and Po status.
                decimal TotalOrdQty = 0;
                decimal TotalRcvQty = 0;
                decimal TotalCancelQty = 0;

                ////Update avg in table inventory
                //foreach (DataRow drInv in dsRecCommit.Tables[inv.TableName].Rows)
                //{
                //    inv.SetPAvgAudit(startDate, endDate, drInv["Location"].ToString(), drInv["ProductCode"].ToString(),
                //        hf_ConnStr.Value);
                //}

                inv.Save(dsRecCommit, LoginInfo.ConnStr);

                // Update Average Cost if it is aveage cost method
                foreach (var recNo in values)
                {
                    inv.UpdateAverageCostByDocument(recNo, LoginInfo.ConnStr);
                }



                //Retrieve Data PO and PODt for Update field. 
                foreach (DataRow drSelected in dsRecCommit.Tables[recDt.TableName].Rows)
                {
                    if (drSelected.RowState != DataRowState.Deleted)
                    {
                        if (dsRecCommit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != string.Empty &&
                            dsRecCommit.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode)
                        {
                            hf_ConnStr.Value = bu.GetConnectionString(dsRecCommit.Tables[rec.TableName].Rows[0]["PoSource"].ToString());
                        }

                        var update = po.UpdateRcvQty(drSelected["PoNo"].ToString(), drSelected["PoDtNo"].ToString(),
                            recDt.GetSumRecQty(drSelected["PoNo"].ToString(), drSelected["PoDtNo"].ToString(),
                                LoginInfo.ConnStr), hf_ConnStr.Value);

                        if (update)
                        {
                            po.GetListByPoNo2(dsUpdatePO, ref MsgError, drSelected["PoNo"].ToString(), hf_ConnStr.Value);
                            poDt.GetPoDtByPoNo(dsUpdatePO, ref MsgError, drSelected["PoNo"].ToString(), hf_ConnStr.Value);


                            //Check order quantity,receive quantity and cancel quantity.
                            //if receive quantity plus cancel quantitty equal to order quantity or more than order quantity,PO's document status change to 'Received'.
                            foreach (DataRow drPoDtCheck in dsUpdatePO.Tables[poDt.TableName].Rows)
                            {
                                if (drPoDtCheck.RowState != DataRowState.Deleted)
                                {
                                    TotalOrdQty += Convert.ToDecimal(drPoDtCheck["OrdQty"].ToString());
                                    TotalRcvQty += Convert.ToDecimal(drPoDtCheck["RcvQty"].ToString());
                                    TotalCancelQty += Convert.ToDecimal(drPoDtCheck["CancelQty"].ToString());
                                }
                            }

                            if (!string.IsNullOrEmpty(drSelected["PoNo"].ToString()))
                            {
                                if (TotalOrdQty <= TotalRcvQty + TotalCancelQty)
                                {
                                    //drPo["DocStatus"] = "Received";
                                    dsUpdatePO.Tables[po.TableName].Rows[0]["DocStatus"] = "Completed";
                                }
                                else
                                {
                                    //drPo["DocStatus"] = "Partial";
                                    dsUpdatePO.Tables[po.TableName].Rows[0]["DocStatus"] = "Partial";
                                }

                                po.SaveOnlyPO(dsUpdatePO, hf_ConnStr.Value);
                            }
                        }
                    }

                    dsUpdatePO.Clear();

                    TotalOrdQty = 0;
                    TotalRcvQty = 0;
                    TotalCancelQty = 0;
                }

                hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);

                pop_ConfrimCommit.ShowOnPageLoad = false;
                //grd_RecCommitByBatch.Selection.UnselectAll();

                for (var i = grd_RecCommitByBatch.Rows.Count - 1; i >= 0; i--)
                {
                    var chk_Item = grd_RecCommitByBatch.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                    chk_Item.Checked = false;
                }

                Response.Redirect("RecLst.aspx");

                pop_Warning.Text = "Commit by batch is complete";
                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                pop_ConfrimCommit.ShowOnPageLoad = false;
                //grd_RecCommitByBatch.Selection.UnselectAll();
                for (var i = grd_RecCommitByBatch.Rows.Count - 1; i >= 0; i--)
                {
                    var chk_Item = grd_RecCommitByBatch.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                    chk_Item.Checked = false;
                }

            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            //grd_RecCommitByBatch.Selection.UnselectAll();
            for (var i = grd_RecCommitByBatch.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_RecCommitByBatch.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                chk_Item.Checked = false;
            }
        }

        #endregion
    }
}