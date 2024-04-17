using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN.REC
{
    public partial class RecCancelItem : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly DataSet dsPoUpdate = new DataSet();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private string MsgError = string.Empty;
        private string PoNo = string.Empty;
        private decimal _totalAmt;

        private Blue.BL.Option.Admin.Interface.AccountMapp accountMapp =
            new Blue.BL.Option.Admin.Interface.AccountMapp();

        private Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private DataSet dsPOList = new DataSet();
        private DataSet dsRecCancelEdit = new DataSet();
        private DataSet dsRecCancelEditCount = new DataSet();
        private DataSet dsSave = new DataSet();
        private Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private decimal grandTotalAmt;
        private Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();

        private string netAcCode = string.Empty;
        private Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private string status = string.Empty;
        private string taxAcCode = string.Empty;
        private decimal totalCancelAmtUpdate;
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();


        /// <summary>
        ///     Max RecDtNo
        /// </summary>
        public decimal TotalAmt
        {
            get
            {
                _totalAmt = (ViewState["TotalAmt"] == null ? 0 : (decimal) ViewState["TotalAmt"]);
                return _totalAmt;
            }
            set
            {
                _totalAmt = value;
                ViewState.Add("TotalAmt", _totalAmt);
            }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsRecCancelEdit = (DataSet) Session["dsRecCancelEdit"];
                //this.Page_Setting();
            }

            Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var MODE = Request.QueryString["MODE"];


            if (MODE.ToUpper() == "CANCELITEM")
            {
                dsRecCancelEdit = (DataSet) Session["dsPo"];
            }

            Session["dsRecCancelEdit"] = dsRecCancelEdit;
        }

        private void Page_Setting()
        {
            if (Request.Params["Type"] != null)
            {
                if (Request.Params["Type"] == "HQ")
                {
                    lbl_Rec_Nm1.Text = " - Cancel from HQ PO";
                }
            }

            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.DataBind();

            //Default select all item in grd_CancelItemEdit
            foreach (GridViewRow grd_Row in grd_CancelItemEdit.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                chk_Item.Checked = true;
            }
        }

        #region " Cancel By Item"

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_CancelItemEdit_OnRowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //Delete Row
            //DataRow drDeleting = dsRecCancelEdit.Tables[poDt.TableName].Rows;

            if (dsRecCancelEdit.Tables[poDt.TableName].Rows.Count > 0)
            {
                foreach (DataRow drPoDt in dsRecCancelEdit.Tables[poDt.TableName].Rows)
                {
                    if (drPoDt.RowState != DataRowState.Deleted)
                    {
                        if (drPoDt["PoDt"].ToString() == e.Keys[0].ToString())
                        {
                            drPoDt.Delete();
                            break;
                        }
                    }
                }
            }

            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.DataBind();

            Session["dsRecCancelEdit"] = dsRecCancelEdit;

            e.Cancel = true;
        }

        protected void se_CancelQtyEdit_NumberChanged(object sender, EventArgs e)
        {
            decimal qtycancel = 0;
            decimal price = 0;
            decimal ordqty = 0;
            decimal rcvqty = 0;

            if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("se_CancelQtyEdit") != null)
            {
                var se_CancelQtyEdit =
                    grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("se_CancelQtyEdit") as
                        ASPxSpinEdit;
                qtycancel = Convert.ToDecimal(se_CancelQtyEdit.Text);
            }

            if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_RcvQty") != null)
            {
                var lbl_RcvQty =
                    grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_RcvQty") as Label;
                rcvqty = Convert.ToDecimal(lbl_RcvQty.Text);
            }

            if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_OrderQty") != null)
            {
                var lbl_OrderQty =
                    grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_OrderQty") as Label;
                ordqty = Convert.ToDecimal(lbl_OrderQty.Text);
            }

            if (ordqty >= qtycancel + rcvqty)
            {
                if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_Price") != null)
                {
                    var lbl_Price =
                        grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_Price") as Label;
                    price = Convert.ToDecimal(lbl_Price.Text);
                }

                if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt =
                        grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("lbl_TotalAmt") as Label;
                    //lbl_CancelTotalAmt.Text = (qtycancel * price).ToString();
                    lbl_TotalAmt.Text = String.Format("{0:0.00}", (qtycancel*price));
                    totalCancelAmtUpdate = (qtycancel*price);
                }
            }
            else
            {
                lbl_Warning.Text = "Cancel Quantity and Receive Quantity more than Order Quantity";
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CommentOK_Click(object sender, EventArgs e)
        {
            var poNo = string.Empty;


            if (grd_CancelItemEdit.Rows.Count > 0)
            {
                // Find updating row table in.inventory.
                if (dsRecCancelEdit.Tables[recDt.TableName] != null)
                {
                    poNo = dsRecCancelEdit.Tables[recDt.TableName].Rows[0]["PoNo"].ToString();

                    // Get po data.
                    po.GetListByPoNo2(dsPoUpdate, ref MsgError, poNo, hf_ConnStr.Value);

                    // Get PoDt Data                
                    poDt.GetListByPoNo(dsPoUpdate, ref MsgError, poNo, hf_ConnStr.Value);


                    if (dsRecCancelEdit.Tables[recDt.TableName].Rows.Count > 0)
                    {
                        for (var i = 0; i < dsRecCancelEdit.Tables[recDt.TableName].Rows.Count; i++)
                        {
                            var drUpdate = dsRecCancelEdit.Tables[recDt.TableName].Rows[i];

                            for (var j = 0; j < dsPoUpdate.Tables[poDt.TableName].Rows.Count; j++)
                            {
                                var drPoDt = dsPoUpdate.Tables[poDt.TableName].Rows[j];


                                if (drUpdate["PoNo"].ToString().ToUpper() == drPoDt["PoNo"].ToString().ToUpper() &&
                                    drUpdate["PoDtNo"].ToString().ToUpper() == drPoDt["PoDt"].ToString().ToUpper())
                                {
                                    drPoDt["CancelQty"] =
                                        (Convert.ToDecimal(drUpdate["OrdQty"] == DBNull.Value
                                            ? 0
                                            : (decimal) drUpdate["OrdQty"]));
                                    drPoDt["Comment"] = txt_CommentClose.Text;
                                }
                            }
                        }

                        // Update status to closed.
                        var drPo = dsPoUpdate.Tables[po.TableName].Rows[0];
                        drPo["DocStatus"] = "Closed";
                    }
                }

                var result = po.Save(dsPoUpdate, hf_ConnStr.Value);

                if (result)
                {
                    Response.Redirect("RecLst.aspx");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CommentCancel_Click(object sender, EventArgs e)
        {
            var poNo = string.Empty;


            if (grd_CancelItemEdit.Rows.Count > 0)
            {
                // Find updating row table in.inventory.
                if (dsRecCancelEdit.Tables[recDt.TableName] != null)
                {
                    poNo = dsRecCancelEdit.Tables[recDt.TableName].Rows[0]["PoNo"].ToString();

                    // Get po data.
                    po.GetListByPoNo2(dsPoUpdate, ref MsgError, poNo, hf_ConnStr.Value);

                    // Get PoDt Data                
                    poDt.GetListByPoNo(dsPoUpdate, ref MsgError, poNo, hf_ConnStr.Value);


                    if (dsRecCancelEdit.Tables[recDt.TableName].Rows.Count > 0)
                    {
                        for (var i = 0; i < dsRecCancelEdit.Tables[recDt.TableName].Rows.Count; i++)
                        {
                            var drUpdate = dsRecCancelEdit.Tables[recDt.TableName].Rows[i];

                            for (var j = 0; j < dsPoUpdate.Tables[poDt.TableName].Rows.Count; j++)
                            {
                                var drPoDt = dsPoUpdate.Tables[poDt.TableName].Rows[j];


                                if (drUpdate["PoNo"].ToString().ToUpper() == drPoDt["PoNo"].ToString().ToUpper() &&
                                    drUpdate["PoDtNo"].ToString().ToUpper() == drPoDt["PoDt"].ToString().ToUpper())
                                {
                                    drPoDt["CancelQty"] =
                                        (Convert.ToDecimal(drUpdate["OrdQty"] == DBNull.Value
                                            ? 0
                                            : (decimal) drUpdate["OrdQty"]));
                                    drPoDt["Comment"] = (drUpdate["Status"] == DBNull.Value
                                        ? string.Empty
                                        : drUpdate["Status"].ToString());
                                }
                            }
                        }

                        // Update status to closed.
                        var drPo = dsPoUpdate.Tables[po.TableName].Rows[0];
                        drPo["DocStatus"] = "Closed";
                    }
                }

                var result = po.Save(dsPoUpdate, hf_ConnStr.Value);

                if (result)
                {
                    Response.Redirect("RecLst.aspx");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecLst.aspx");
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecEdit.aspx?MODE=CANCELITEM");
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            var columnValues = new List<object>();

            foreach (GridViewRow grd_Row in grd_CancelItemEdit.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_No = grd_Row.FindControl("lbl_No") as Label;
                    columnValues.Add(lbl_No.Text);
                }
            }

            //if (grd_CancelItemEdit.GetSelectedFieldValues("PoDt").Count <= 0)
            if (columnValues.Count <= 0)
            {
                lbl_Warning.Text = "Please select item";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            pop_ConfrimCancel.ShowOnPageLoad = true;
        }

        protected void btn_ConfrimCancel_Click(object sender, EventArgs e)
        {
            if (Request.Params["Type"] != null && Request.Params["Type"].ToUpper() == "HQ")
            {
                hf_ConnStr.Value = LoginInfo.HQConnStr;
            }

            //PoNo = dsRecCancelEdit.Tables[po.TableName].Rows[0]["PoNo"].ToString();

            //po.GetListByPoNo2(dsPoUpdate, ref MsgError, PoNo, hf_ConnStr.Value);
            //poDt.GetPoDtByPoNo(dsPoUpdate, ref MsgError, PoNo, hf_ConnStr.Value);

            //List<object> columnValues = grd_CancelItemEdit.GetSelectedFieldValues("PoDt");

            var columnValues = new List<object>();
            var columnKeys = new List<object>();

            foreach (GridViewRow grd_Row in grd_CancelItemEdit.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_No = grd_Row.FindControl("lbl_No") as Label;
                    columnValues.Add(lbl_No.Text);
                    columnKeys.Add(dsRecCancelEdit.Tables[poDt.TableName].Rows[grd_Row.RowIndex]["PoNo"]);
                }
            }


            for (var Keys = 0; Keys < columnKeys.Count; Keys++)
            {
                //PoNo = dsRecCancelEdit.Tables[poDt.TableName].Rows[Keys]["PoNo"].ToString();
                PoNo = columnKeys[Keys].ToString();
                po.GetListByPoNo2(dsPoUpdate, ref MsgError, PoNo, hf_ConnStr.Value);
                poDt.GetPoDtByPoNo(dsPoUpdate, ref MsgError, PoNo, hf_ConnStr.Value);
                //poDt.GetPoDtCancelByPoNo(dsPoUpdate, ref MsgError, PoNo, LoginInfo.BuFmtInfo.BuCode, hf_ConnStr.Value);

                //foreach (DataRow drCancel in dsPoUpdate.Tables[poDt.TableName].Rows)
                for (var i = 0; i < dsPoUpdate.Tables[poDt.TableName].Rows.Count; i++)
                {
                    var drCancel = dsPoUpdate.Tables[poDt.TableName].Rows[i];

                    if (drCancel.RowState != DataRowState.Deleted)
                    {
                        if (drCancel["PoDt"].ToString() == columnValues[Keys].ToString())
                        {
                            //Check receive quantity for check Cancel full PO from Receiving
                            //Rcv += Convert.ToDecimal(drCancel["RcvQty"].ToString());ordqty - rcvqty
                            var lbl_QtyCancel = grd_CancelItemEdit.Rows[Keys].FindControl("lbl_QtyCancel") as Label;

                            var ordqty = Convert.ToDecimal(drCancel["OrdQty"].ToString());
                            var rcvqty = Convert.ToDecimal(drCancel["RcvQty"].ToString());
                            //decimal cancelqty = Convert.ToDecimal(drCancel["CancelQty"]) == 0 ? Convert.ToDecimal(drCancel["QtyCancel"].ToString()) : Convert.ToDecimal(drCancel["CancelQty"].ToString());
                            var cancelqty = Convert.ToDecimal(lbl_QtyCancel.Text);

                            if (ordqty >= rcvqty + cancelqty + Convert.ToDecimal(drCancel["CancelQty"].ToString()))
                            {
                                drCancel["CancelQty"] = cancelqty + Convert.ToDecimal(drCancel["CancelQty"].ToString());
                            }
                            else
                            {
                                //grd_CancelItemEdit.Selection.UnselectAll();
                                foreach (GridViewRow grd_Row in grd_CancelItemEdit.Rows)
                                {
                                    var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                                    chk_Item.Checked = false;
                                }

                                lbl_Warning.Text = "Not able to cancel";
                                pop_Warning.ShowOnPageLoad = true;
                                return;
                            }
                        }
                    }
                }

                //poDt.Save(dsPoUpdate, hf_ConnStr.Value);

                var drPo = dsPoUpdate.Tables[po.TableName].Rows[0];
                decimal OrdQty = 0;
                decimal RcvQty = 0;
                decimal CancelQty = 0;

                //dsPoUpdate.Tables[poDt.TableName].Clear();
                //poDt.GetPoDtByPoNo(dsPoUpdate, ref MsgError, PoNo, hf_ConnStr.Value);
                foreach (DataRow drPoDt in dsPoUpdate.Tables[poDt.TableName].Rows)
                {
                    if (drPoDt.RowState != DataRowState.Deleted)
                    {
                        OrdQty += Convert.ToDecimal(drPoDt["OrdQty"].ToString());
                        RcvQty += Convert.ToDecimal(drPoDt["RcvQty"].ToString());
                        CancelQty += Convert.ToDecimal(drPoDt["CancelQty"].ToString());
                    }
                }

                //Check receive quantity for check Cancel full PO from Receiving
                if (OrdQty <= RcvQty + CancelQty && RcvQty == 0)
                {
                    drPo["DocStatus"] = "Cancelled";
                }
                else if (OrdQty <= RcvQty + CancelQty)
                {
                    drPo["DocStatus"] = "Closed";
                }
                else
                {
                    drPo["DocStatus"] = "Partial";
                }

                po.Save(dsPoUpdate, hf_ConnStr.Value);
                dsPoUpdate.Clear();
            }


            //DataRow drPo = dsPoUpdate.Tables[po.TableName].Rows[0];
            //decimal OrdQty = 0;
            //decimal RcvQty = 0;
            //decimal CancelQty = 0;

            //foreach (DataRow drPoDt in dsPoUpdate.Tables[poDt.TableName].Rows)
            //{
            //    if (drPoDt.RowState != DataRowState.Deleted)
            //    {
            //        OrdQty += Convert.ToDecimal(drPoDt["OrdQty"].ToString());
            //        RcvQty += Convert.ToDecimal(drPoDt["RcvQty"].ToString());
            //        CancelQty += Convert.ToDecimal(drPoDt["CancelQty"].ToString());
            //    }
            //}

            ////Check receive quantity for check Cancel full PO from Receiving
            //if (OrdQty <= RcvQty + CancelQty && RcvQty == 0 )
            //{
            //    drPo["DocStatus"] = "Cancelled";
            //}
            //else if (OrdQty <= RcvQty + CancelQty)
            //{
            //    drPo["DocStatus"] = "Closed";
            //}
            //else
            //{
            //    drPo["DocStatus"] = "Partial";
            //}

            //bool result = po.Save(dsPoUpdate, hf_ConnStr.Value);

            //if (result)
            //{
            Response.Redirect("RecLst.aspx");
            //}
        }

        protected void btn_CancelCancel_Click(object sender, EventArgs e)
        {
            pop_ConfrimCancel.ShowOnPageLoad = false;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            pop_ConfrimCancel.ShowOnPageLoad = false;
        }

        protected void grd_CancelItemEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal qtycancel = 0;
            decimal price = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_No") != null)
                {
                    var lbl_No = e.Row.FindControl("lbl_No") as Label;
                    lbl_No.Text = DataBinder.Eval(e.Row.DataItem, "PoDt").ToString();
                }

                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl_Location = e.Row.FindControl("lbl_Location") as Label;
                    lbl_Location.Text = DataBinder.Eval(e.Row.DataItem, "Location") + " : " +
                                        locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                                            hf_ConnStr.Value);
                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "Location").ToString(), hf_ConnStr.Value);
                    //lbl_Location.Text = locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl_Product = e.Row.FindControl("lbl_Product") as Label;

                    var ProductName = product.GetName(DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                        hf_ConnStr.Value);
                    var ProductDesc = product.GetName2(DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                        hf_ConnStr.Value);
                    lbl_Product.Text = DataBinder.Eval(e.Row.DataItem, "Product") + " : " + ProductName + " : " +
                                       ProductDesc;
                    //lbl_Product.Text = ProductName + " : " + ProductDesc;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("lbl_OrderQty") != null)
                {
                    var lbl_OrderQty = e.Row.FindControl("lbl_OrderQty") as Label;
                    lbl_OrderQty.Text = DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString();
                }

                if (e.Row.FindControl("lbl_RcvQty") != null)
                {
                    var lbl_RcvQty = e.Row.FindControl("lbl_RcvQty") as Label;
                    lbl_RcvQty.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                //DataRow drCancel = dsRecCancelEdit.Tables[poDt.TableName].Rows[e.Row.RowIndex];

                if (e.Row.FindControl("lbl_QtyCancel") != null)
                {
                    var lbl_QtyCancel = e.Row.FindControl("lbl_QtyCancel") as Label;

                    //check this row state not equal to delete
                    //if (drCancel.RowState != DataRowState.Deleted && Convert.ToDecimal(drCancel["CancelQty"]) != 0)
                    //{
                    //    lbl_QtyCancel.Text = String.Format("{0:N}", drCancel["CancelQty"]);
                    //    qtycancel = Convert.ToDecimal(drCancel["CancelQty"]);
                    //}
                    //else
                    //{
                    lbl_QtyCancel.Text = String.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "QtyCancel"));
                    qtycancel = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyCancel"));
                    //}
                }

                if (e.Row.FindControl("se_CancelQtyEdit") != null)
                {
                    var se_CancelQtyEdit = e.Row.FindControl("se_CancelQtyEdit") as ASPxSpinEdit;

                    //if (Convert.ToDecimal(drCancel["CancelQty"]) != 0)
                    //{
                    //    se_CancelQtyEdit.Text = Convert.ToDecimal(drCancel["CancelQty"]).ToString();
                    //}
                    //else
                    //{
                    se_CancelQtyEdit.Text = String.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "QtyCancel"));
                    qtycancel = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QtyCancel"));
                    //}
                }

                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var lbl_FocQty = e.Row.FindControl("lbl_FocQty") as Label;
                    lbl_FocQty.Text = String.Format("{0:N}",
                        DataBinder.Eval(e.Row.DataItem, "FOCQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FOCQty")));
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = String.Format("{0:N}",
                        DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price")));
                    price = DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                        ? 0
                        : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format("{0:N}", qtycancel*price);
                    grandTotalAmt += qtycancel*price;
                }

                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format("{0:N}", grandTotalAmt);
                }
            }
        }

        protected void grd_CancelItemEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            grandTotalAmt = 0;

            if (Request.Params["Type"] != null && Request.Params["Type"].ToUpper() == "HQ")
            {
                //change connection string for save in HQ
                hf_ConnStr.Value = LoginInfo.HQConnStr;
            }
            // Find updating row table receive detail.
            var drUpdating = dsRecCancelEdit.Tables[poDt.TableName].Rows[grd_CancelItemEdit.EditIndex];


            if (grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("se_CancelQtyEdit") != null)
            {
                var se_CancelQtyEdit =
                    grd_CancelItemEdit.Rows[grd_CancelItemEdit.EditIndex].FindControl("se_CancelQtyEdit") as
                        ASPxSpinEdit;

                drUpdating["QtyCancel"] = Convert.ToDecimal(se_CancelQtyEdit.Text);
                //drUpdating["CancelQty"] = qtycancelUpdate;
                //qtycancelUpdate = Convert.ToDecimal(se_CancelQtyEdit.Text);
            }

            //drUpdating["Comment"] = (txt_Comment.Text.ToStringq());

            drUpdating["TotalAmt"] = totalCancelAmtUpdate;
            drUpdating["Comment"] = e.NewValues["Comment"];

            //poDt.Save(dsRecCancelEdit, hf_ConnStr.Value);

            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.EditIndex = -1;
            grd_CancelItemEdit.DataBind();

            btn_Back.Enabled = true;
            btn_Cancel.Enabled = true;
            Session["dsRecCancelEdit"] = dsRecCancelEdit;
        }

        protected void grd_CancelItemEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grandTotalAmt = 0;
            btn_Back.Enabled = false;
            btn_Cancel.Enabled = false;

            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.EditIndex = e.NewEditIndex;
            grd_CancelItemEdit.DataBind();

            for (var i = grd_CancelItemEdit.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_CancelItemEdit.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                var Chk_All = grd_CancelItemEdit.HeaderRow.FindControl("Chk_All") as CheckBox;

                Chk_All.Enabled = false;
                Chk_Item.Enabled = false;
            }
        }

        protected void grd_CancelItemEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grandTotalAmt = 0;

            dsRecCancelEdit.Tables[poDt.TableName].Rows[dsRecCancelEdit.Tables[poDt.TableName].Rows.Count - 1]
                .CancelEdit();
            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.EditIndex = -1;
            grd_CancelItemEdit.DataBind();

            btn_Back.Enabled = true;
            btn_Cancel.Enabled = true;
        }

        protected void grd_CancelItemEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            grandTotalAmt = 0;

            if (dsRecCancelEdit.Tables[poDt.TableName].Rows.Count > 0)
            {
                //foreach (DataRow drPoDt in dsRecCancelEdit.Tables[poDt.TableName].Rows)
                for (var i = 0; i < dsRecCancelEdit.Tables[poDt.TableName].Rows.Count; i++)
                {
                    var drPoDt = dsRecCancelEdit.Tables[poDt.TableName].Rows[i];

                    if (drPoDt.RowState != DataRowState.Deleted)
                    {
                        //Check row index when delete
                        if (i == e.RowIndex)
                        {
                            drPoDt.Delete();
                            break;
                        }
                    }
                }
            }

            grd_CancelItemEdit.DataSource = dsRecCancelEdit.Tables[poDt.TableName];
            grd_CancelItemEdit.DataBind();

            Session["dsRecCancelEdit"] = dsRecCancelEdit;

            e.Cancel = true;
        }

        #endregion
    }
}