using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Linq;


//using FastReport;
//using FastReport.Export;
//using FastReport.Utils;
//using System.Text.RegularExpressions;

namespace BlueLedger.PL.PC.PO
{
    public partial class PO : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accMapp = new Blue.BL.Option.Admin.Interface.AccountMapp();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();

        private readonly DataSet dsRecDt = new DataSet();
        private readonly DataSet dsStockSum = new DataSet();
        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();

        private readonly Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly PoReportMail poReportMail = new PoReportMail();

        private DataSet dsPo = new DataSet();
        private DataTable dtPrDt = new DataTable();
        private string PoNo = string.Empty;

        //int poIndex = -1;
        private decimal netamt;
        private decimal discAmt;
        private decimal netamtpr;
        private Blue.BL.IN.PriceList priceList = new Blue.BL.IN.PriceList();
        private decimal taxamt;
        private decimal taxamtpr;
        private decimal totalamt;
        private decimal totalamtpr;

        // Added on: 01/09/2017, By: Fon
        private decimal g_CurrNetAmt;
        private decimal g_CurrDiscAmt;
        private decimal g_CurrTaxAmt;
        private decimal g_CurrTotalAmt;
        private readonly string moduleID = "2.2";

        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }
        // End Added.
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //hf_ConnStr.Value = LoginInfo.ConnStr;            
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPo = (DataSet)Session["dsPo"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            //DataTable dtTest = new DataTable();

            //dtTest = transType.GetActiveList(LoginInfo.ConnStr);

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                PoNo = Request.Params["ID"];

                var getStrLct = po.GetListByPoNo2(dsTmp, ref MsgError, PoNo, hf_ConnStr.Value);

                if (getStrLct)
                {
                    dsPo = dsTmp;

                    poDt.GetListByPoNo(dsPo, ref MsgError, PoNo, hf_ConnStr.Value);
                }
                else
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }
            }

            Page_Setting();

            Session["dsPo"] = dsPo;
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            if (dsPo.Tables[po.TableName] != null)
            {
                var drPo = dsPo.Tables[po.TableName].Rows[0];
                var dtVendor = new DataTable();
                dtVendor = vendor.GetVendor(drPo["Vendor"].ToString(), hf_ConnStr.Value);

                lbl_PONumber.Text = drPo["PoNo"].ToString();
                lbl_Status.Text = drPo["DocStatus"].ToString();
                lbl_PODate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", drPo["PoDate"]);
                //DateTime.Parse(drPo["PoDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);//drPo["PoDate"].ToString();  
                //ddl_Buyer.Value         = drPo["Buyer"].ToString();
                lbl_Buyer.Text = drPo["Buyer"].ToString();
                lbl_Description.Text = drPo["Description"].ToString();
                lbl_Remark1.Text = drPo["AddField1"].ToString();
                lbl_Remark2.Text = drPo["AddField2"].ToString();
                lbl_Remark3.Text = drPo["AddField3"].ToString();
                lbl_CreditTerm.Text = drPo["CreditTerm"].ToString();
                lbl_VendorCode.Text = drPo["Vendor"] + " - " +
                                      vendor.GetName(drPo["Vendor"].ToString(), hf_ConnStr.Value);
                //Modefied: 15/058/2017; By: Fon
                //lbl_Currency.Text = drPo["Currency"].ToString();
                //lbl_Exchange.Text = drPo["ExchageRate"].ToString();
                lbl_Currency.Text = drPo["CurrencyCode"].ToString();
                lbl_Exchange.Text = drPo["CurrencyRate"].ToString();
                // End Modified.

                // lbl_VendorName.Text = dtVendor.Rows[0]["Name"].ToString();

                if (drPo["DeliDate"].ToString() != string.Empty)
                {
                    lbl_DeliveryDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", drPo["DeliDate"]);
                    //DateTime.Parse(drPo["DeliDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }
                else
                {
                    lbl_DeliveryDate.Text = string.Empty;
                }

                if (drPo["DocStatus"].ToString() == "Voided")
                {
                    //menu_CmdBar.Items[1].Enabled = false;
                }
            }

            Total();
            lbl_TNet.Text = String.Format(DefaultAmtFmt, netamt);
            lbl_TTax.Text = String.Format(DefaultAmtFmt, taxamt);
            lbl_TAmount.Text = String.Format(DefaultAmtFmt, totalamt);

            // Added on: 01/09/2017, By: Fon
            lbl_CurrGrandTitle.Text = string.Format("( {0} )", lbl_Currency.Text);
            lbl_BaseGrandTitle.Text = string.Format("( {0} )", baseCurrency);

            lbl_CurrTNet.Text = string.Format(DefaultAmtFmt, g_CurrNetAmt);
            lbl_CurrTTax.Text = string.Format(DefaultAmtFmt, g_CurrTaxAmt);
            lbl_CurrTAmount.Text = string.Format(DefaultAmtFmt, g_CurrTotalAmt);

            // Added on: 12/02/2018, For: Following from P'Oat request.
            lbl_CurrTDisc.Text = string.Format(DefaultAmtFmt, g_CurrDiscAmt);
            lbl_TDisc.Text = string.Format(DefaultAmtFmt, discAmt);

            // End Added.

            grd_PoDt.DataSource = dsPo.Tables[poDt.TableName];
            grd_PoDt.DataBind();

            //Assign print & back button navigate url
            //menu_CmdBar.Items[2].NavigateUrl    = "PoChangeDocStatus.aspx";
            //menu_CmdBar.Items[2].NavigateUrl = "~/RPT/Default.aspx?PoNo=" + dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            //menu_CmdBar.Items[2].Target         = "_blank";

            // Back Button.
            // If cmbBar has 'ClosePO'.
            menu_CmdBar.Items.FindByName("Mail").ToolTip = "Send email to vendor";

            var vid = Request.QueryString["VID"].ToString();
            var page = Request.QueryString["page"].ToString();

            menu_CmdBar.Items.FindByName("Back").ToolTip = "Back";
            menu_CmdBar.Items.FindByName("Back").NavigateUrl = string.Format("PoList.aspx?VID={0}&page={1}", vid, page); 
            //menu_CmdBar.Items[3].ToolTip     = "Back";
            //menu_CmdBar.Items[3].NavigateUrl = "PoList.aspx";

            // Display Activity Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "PC";
            log.SubModule = "PO";
            log.RefNo = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            log.Visible = true;
            log.DataBind();

            //((BlueLedger.PL.Master_In_Default)this.Master).AllowShowLog = true;
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).TableName    = "Admin.TransLog";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).Module       = "PC";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).SubModule    = "PO";
            //((BlueLedger.PL.Master.Pc.Blue)this.Master).RefNo        = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();

            // Display Comment       
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "PC";
            comment.SubModule = "PO";
            comment.RefNo = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "PC";
            attach.RefNo = dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString();
            attach.Visible = true;
            attach.DataBind();

            Control_HeaderMenuBar();
        }

        // Added on: 03/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
            menu_CmdBar.Items.FindByName("ClosePo").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("ClosePo").Visible : false;
            menu_CmdBar.Items.FindByName("Void").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Void").Visible : false;

            menu_CmdBar.Items.FindByName("Edit").Visible = lbl_Status.Text.ToUpper() == "VOIDED" ? false : menu_CmdBar.Items.FindByName("Edit").Visible;
            menu_CmdBar.Items.FindByName("ClosePo").Visible = lbl_Status.Text.ToUpper() == "VOIDED" ? false : menu_CmdBar.Items.FindByName("ClosePo").Visible;
            menu_CmdBar.Items.FindByName("Void").Visible = lbl_Status.Text.ToUpper() == "VOIDED" ? false : menu_CmdBar.Items.FindByName("Void").Visible;
            menu_CmdBar.Items.FindByName("Mail").Visible = lbl_Status.Text.ToUpper() == "VOIDED" ? false : menu_CmdBar.Items.FindByName("Mail").Visible;

            menu_CmdBar.Items.FindByName("Edit").Visible = lbl_Status.Text.ToUpper() == "CLOSED" ? false : menu_CmdBar.Items.FindByName("Edit").Visible;
            menu_CmdBar.Items.FindByName("ClosePo").Visible = lbl_Status.Text.ToUpper() == "CLOSED" ? false : menu_CmdBar.Items.FindByName("ClosePo").Visible;
            menu_CmdBar.Items.FindByName("Void").Visible = lbl_Status.Text.ToUpper() == "CLOSED" ? false : menu_CmdBar.Items.FindByName("Void").Visible;
            menu_CmdBar.Items.FindByName("Mail").Visible = lbl_Status.Text.ToUpper() == "CLOSED" ? false : menu_CmdBar.Items.FindByName("Mail").Visible;

            menu_CmdBar.Items.FindByName("Edit").Visible = lbl_Status.Text.ToUpper() == "COMPLETED" ? false : menu_CmdBar.Items.FindByName("Edit").Visible;
            menu_CmdBar.Items.FindByName("ClosePo").Visible = lbl_Status.Text.ToUpper() == "COMPLETED" ? false : menu_CmdBar.Items.FindByName("ClosePo").Visible;
            menu_CmdBar.Items.FindByName("Void").Visible = lbl_Status.Text.ToUpper() == "COMPLETED" ? false : menu_CmdBar.Items.FindByName("Void").Visible;
            menu_CmdBar.Items.FindByName("Mail").Visible = lbl_Status.Text.ToUpper() == "COMPLETED" ? false : menu_CmdBar.Items.FindByName("Mail").Visible;
        }
        // End Added.

        /// <summary>
        ///     Menu Click Event.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Menu click event.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            //ArrayList objArrList;
            //objArrList = new ArrayList();

            //objArrList.Add(dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString());
            //Session["s_arrNo"] = objArrList;

            switch (e.Item.Name.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("PoEdit.aspx?MODE=EDIT&BuCode=" + Request.Params["BuCode"] + "&ID=" +
                                      dsPo.Tables[po.TableName].Rows[0]["PoNo"]);
                    break;

                case "CLOSEPO":
                    pop_ClosePO.ShowOnPageLoad = true;
                    break;

                case "VOID":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    Print(source, e);
                    //if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                    //{
                    //    dsPo.Tables[po.TableName].Rows[0]["DocStatus"] = "Printed";
                    //}

                    //// Commit change to database
                    //var resultSave = po.SaveOnlyPO(dsPo, hf_ConnStr.Value);

                    //if (resultSave)
                    //{
                    //}

                    //// Send PoNo  to Session
                    //var objArrList = new ArrayList();
                    //objArrList.Add(dsPo.Tables[po.TableName].Rows[0]["PoNo"]);
                    //Session["s_arrNo"] = objArrList;

                    //var paramField = new string[1, 2];
                    //paramField[0, 0] = "BU";
                    //paramField[0, 1] = LoginInfo.BuInfo.BuName;
                    ////paramField[1, 0]    = "Pm-vPODt.PoNo";
                    //////paramField[1, 1]  = LoginInfo.BuInfo.BuName.ToString();
                    //Session["paramField"] = paramField;


                    //Session.Remove("SubReportName");
                    //Session["SubReportName"] = "prsign";

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=117" + "&BuCode=" + Request.Params["BuCode"];
                    ////Response.Write("<script>");
                    ////Response.Write("window.open('" + reportLink + "','_blank'  )");
                    ////Response.Write("</script>");
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");

                    ////var pageFile = "PoPrint2.aspx?PoNo=" + dsPo.Tables[po.TableName].Rows[0]["PoNo"];
                    ////ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    ////    "<script>window.open('" + pageFile + "','_blank')</script>");


                    break;

                case "MAIL":
                    string vendorCode = lbl_VendorCode.Text.Split('-')[0].ToString().Trim();
                    txt_Pop_SendMail_Email.Text = poReportMail.GetVendorEmail(vendorCode, hf_ConnStr.Value);
                    lbl_Pop_SendMail_VendorMail.Text = lbl_VendorCode.Text;
                    lbl_Pop_SendMail_Message.Text = string.Empty;
                    pop_SendMail.ShowOnPageLoad = true;
                    break;

            }
        }

        private void OnSendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Handle the callback if you need to do anything after the email is sent.
            lbl_Description.Text = "Sent at " + DateTime.Now.ToString();
        }

        /// <summary>
        /// </summary>
        protected void Total()
        {
            netamt = 0;
            discAmt = 0;
            taxamt = 0;
            totalamt = 0;

            g_CurrNetAmt = 0;
            g_CurrDiscAmt = 0;
            g_CurrTaxAmt = 0;
            g_CurrTotalAmt = 0;
            if (dsPo.Tables[poDt.TableName].Rows.Count > 0)
            {
                for (var z = 0; z < dsPo.Tables[poDt.TableName].Rows.Count; z++)
                {
                    if (dsPo.Tables[poDt.TableName].Rows[z]["NetAmt"].ToString() != string.Empty)
                    {
                        netamt += decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["NetAmt"].ToString());
                    }
                    else
                    {
                        netamt += 0;
                    }

                    if (dsPo.Tables[poDt.TableName].Rows[z]["TaxAmt"].ToString() != string.Empty)
                    {
                        taxamt += decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["TaxAmt"].ToString());
                    }
                    else
                    {
                        taxamt += 0;
                    }

                    // Added on: 01/09/2017
                    if (dsPo.Tables[poDt.TableName].Rows[z]["CurrNetAmt"].ToString() != string.Empty)
                        g_CurrNetAmt += decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["CurrNetAmt"].ToString());
                    else g_CurrNetAmt += 0;

                    if (dsPo.Tables[poDt.TableName].Rows[z]["CurrTaxAmt"].ToString() != string.Empty)
                        g_CurrTaxAmt += decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["CurrTaxAmt"].ToString());
                    else g_CurrTaxAmt += 0;

                    // Added on: 12/02/2018, For: Following from P'Oat Request.
                    g_CurrDiscAmt += (dsPo.Tables[poDt.TableName].Rows[z]["CurrDiscAmt"].ToString() != string.Empty)
                        ? decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["CurrDiscAmt"].ToString()) : 0;
                    discAmt += (dsPo.Tables[poDt.TableName].Rows[z]["DisCountAmt"].ToString() != string.Empty)
                        ? decimal.Parse(dsPo.Tables[poDt.TableName].Rows[z]["DisCountAmt"].ToString()) : 0;

                    // End Added.
                }
            }

            totalamt = netamt + taxamt;
            g_CurrTotalAmt = g_CurrNetAmt + g_CurrTaxAmt;
        }

        /// <summary>
        /// </summary>
        protected void TotalPR()
        {
            netamtpr = 0;
            taxamtpr = 0;
            totalamtpr = 0;

            if (dtPrDt != null)
            {
                if (dtPrDt.Rows.Count > 0)
                {
                    for (var z = 0; z < dtPrDt.Rows.Count; z++)
                    {
                        if (dtPrDt.Rows[z]["NetAmt"].ToString() != string.Empty)
                        {
                            netamtpr += decimal.Parse(dtPrDt.Rows[z]["NetAmt"].ToString());
                        }
                        else
                        {
                            netamtpr += 0;
                        }

                        if (dtPrDt.Rows[z]["TaxAmt"].ToString() != string.Empty)
                        {
                            taxamtpr += decimal.Parse(dtPrDt.Rows[z]["TaxAmt"].ToString());
                        }
                        else
                        {
                            taxamtpr += 0;
                        }
                    }
                }

                totalamtpr = netamtpr + taxamtpr;
                //lbl_TPrNet.Text = netamt.ToString();
                //lbl_TPrTax.Text = taxamt.ToString();
                //lbl_TPrAmount.Text = (netamt + taxamt).ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.TableName = "prDt";

            if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
            {
                // Update data on table pr.                 
                var chk = prDt.GetByPONo(dsPo, dsPo.Tables[po.TableName].Rows[0]["PoNo"].ToString(), hf_ConnStr.Value);

                if (chk)
                {
                    if (dsPo.Tables[prDt.TableName] != null)
                    {
                        foreach (DataRow drPoDt in dsPo.Tables[poDt.TableName].Rows)
                        {
                            foreach (DataRow drPrDt in dsPo.Tables[prDt.TableName].Rows)
                            {
                                if (drPrDt["PONo"].Equals(drPoDt["PONo"].ToString()) &
                                    drPrDt["PONo"].Equals(drPoDt["PONo"].ToString()))
                                {
                                    drPrDt["PONo"] = DBNull.Value;
                                    drPrDt["PODtNo"] = DBNull.Value;
                                }
                            }
                        }
                    }
                }

                // Update data on table po.
                dsPo.Tables[po.TableName].Rows[0]["IsVoid"] = true;
                dsPo.Tables[po.TableName].Rows[0]["DocStatus"] = "Voided";
                dsPo.Tables[po.TableName].Rows[0]["UpdatedDate"] = ServerDateTime;
                dsPo.Tables[po.TableName].Rows[0]["UpdatedBy"] = LoginInfo.LoginName;

                // Commit change to database
                var result = po.SavePRPO(dsPo, LoginInfo.ConnStr);
                //po.Save(dsPo, LoginInfo.ConnStr);

                if (result)
                {
                    pop_ConfrimDelete.ShowOnPageLoad = false;

                    dsPo.Clear();
                    Session["dsPo"] = dsPo;

                    // Added on: 21/09/2017, By: Fon
                    //ClassLogTool pctool = new ClassLogTool();
                    //pctool.SaveActionLog("PO", lbl_PONumber.Text, "Void");
                    // End Added.
                    _transLog.Save("PC", "PO", lbl_PONumber.Text, "VOIDED", txt_pop_ConfirmDelete_Remark.Text, LoginInfo.LoginName, LoginInfo.ConnStr);


                    Response.Redirect("~/PC/Po/PoList.aspx");
                }
            }
            else
            {
                pop_NotAllow.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_YesClosePO_Click(object sender, EventArgs e)
        {
            decimal ordqty;
            decimal rcvqty;
            decimal cancelqty;

            if (dsPo.Tables[po.TableName].Rows.Count > 0)
            {
                dsPo.Tables[po.TableName].Rows[0]["DocStatus"] = "Closed";
                dsPo.Tables[po.TableName].Rows[0]["UpdatedDate"] = ServerDateTime;
                dsPo.Tables[po.TableName].Rows[0]["UpdatedBy"] = LoginInfo.LoginName;

                for (var i = 0; i < dsPo.Tables[poDt.TableName].Rows.Count; i++)
                {
                    var drChange = dsPo.Tables[poDt.TableName].Rows[i];

                    ordqty = Convert.ToDecimal(drChange["OrdQty"].ToString());
                    rcvqty = Convert.ToDecimal(drChange["RcvQty"].ToString());
                    cancelqty = Convert.ToDecimal(drChange["CancelQty"].ToString());

                    if (ordqty >= rcvqty + cancelqty)
                    {
                        drChange["CancelQty"] = cancelqty.ToString();
                    }
                }

                // Commit change to database
                var result = po.Save(dsPo, LoginInfo.ConnStr);

                if (result)
                {
                    pop_ConfrimDelete.ShowOnPageLoad = false; // ?
                    pop_ClosePO.ShowOnPageLoad = false;

                    dsPo.Clear();
                    Session["dsPo"] = dsPo;

                    // Added on: 21/09/2017, By: Fon
                    //ClassLogTool pctool = new ClassLogTool();
                    //pctool.SaveActionLog("PO", lbl_PONumber.Text, "Close PO");
                    // End Added.

                    _transLog.Save("PC", "PO", lbl_PONumber.Text, "CLOSED", txt_pop_ClosePO_Remark.Text, LoginInfo.LoginName, LoginInfo.ConnStr);

                    Response.Redirect("~/PC/Po/PoList.aspx");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_NoClosePO_Click(object sender, EventArgs e)
        {
            pop_ClosePO.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OKNotAllow_Click(object sender, EventArgs e)
        {
            pop_NotAllow.ShowOnPageLoad = false;
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void mbt_Edit_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("PoEdit.aspx?MODE=EDIT&BuCode=" + Request.Params["BuCode"] + "&ID=" +
                              dsPo.Tables[po.TableName].Rows[0]["PoNo"]);
        }

        protected void grd_PoDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_SKU") != null)
                {
                    var lbl_SKU = e.Row.FindControl("lbl_SKU") as Label;
                    lbl_SKU.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    lbl_SKU.ToolTip = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                //if (e.Row.FindControl("lbl_Unit") != null)
                //{
                //    Label lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                //    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                //}

                if (e.Row.FindControl("lbl_QTYOrder") != null)
                {
                    var lbl_QTYOrder = e.Row.FindControl("lbl_QTYOrder") as Label;
                    lbl_QTYOrder.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "OrdQty")) + " " +
                                        DataBinder.Eval(e.Row.DataItem, "Unit");
                }

                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lbl_FOC = e.Row.FindControl("lbl_FOC") as Label;
                    lbl_FOC.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "FOCQty"));
                }

                if (e.Row.FindControl("lbl_RCV") != null)
                {
                    var lbl_RCV = e.Row.FindControl("lbl_RCV") as Label;
                    lbl_RCV.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RcvQty"));
                }

                if (e.Row.FindControl("lbl_Cancel") != null)
                {
                    var lbl_Cancel = e.Row.FindControl("lbl_Cancel") as Label;
                    lbl_Cancel.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "CancelQty"));
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chk_Adj = e.Row.FindControl("chk_Adj") as CheckBox;
                    chk_Adj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "IsAdj");
                }


                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var lbl_Amount = e.Row.FindControl("lbl_Amount") as Label;
                    lbl_Amount.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }

                if (e.Row.FindControl("lbl_NetAC") != null)
                {
                    var lbl_NetAC = e.Row.FindControl("lbl_NetAC") as Label;
                    var strProd = DataBinder.Eval(e.Row.DataItem, "Product").ToString();
                    lbl_NetAC.Text = accMapp.GetA3Code(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString(),
                        DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                        strProd.Substring(0, 4), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_TaxAC") != null)
                {
                    var lbl_TaxAC = e.Row.FindControl("lbl_TaxAC") as Label;
                    lbl_TaxAC.Text = prod.GetTaxAccCode(DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                        hf_ConnStr.Value);
                }


                //----------------------- Expand Po Detail, Stock Summary and Pr Detail ----------------------------
                //Binding display Po Detail.
                #region
                if (e.Row.FindControl("lbl_Receive") != null)
                {
                    var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                    lbl_Receive.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString());
                }

                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text =
                        Blue.BL.GnxLib.GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                }

                if (e.Row.FindControl("lbl_Disc") != null)
                {
                    var lbl_Disc = e.Row.FindControl("lbl_Disc") as Label;
                    lbl_Disc.Text = DataBinder.Eval(e.Row.DataItem, "Discount") + " %";
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate") + " %";
                }

                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl_NetAmt = e.Row.FindControl("lbl_NetAmt") as Label;
                    lbl_NetAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DisCountAmt"));
                }

                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl_TaxAmt = e.Row.FindControl("lbl_TaxAmt") as Label;
                    lbl_TaxAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                }

                // Added on: 29/08/2017, By: Fon
                if (e.Row.FindControl("lbl_CurrCurrDt") != null)
                {
                    Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                    lbl_CurrCurrDt.Text = string.Format("( {0} )", lbl_Currency.Text);
                }
                if (e.Row.FindControl("lbl_BaseCurrDt") != null)
                {
                    Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                    lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
                }
                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    Label lbl_CurrNetAmt = (Label)e.Row.FindControl("lbl_CurrNetAmt");
                    lbl_CurrNetAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }

                if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                {
                    Label lbl_CurrDiscAmt = (Label)e.Row.FindControl("lbl_CurrDiscAmt");
                    lbl_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }

                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    Label lbl_CurrTaxAmt = (Label)e.Row.FindControl("lbl_CurrTaxAmt");
                    lbl_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    Label lbl_CurrTotalAmt = (Label)e.Row.FindControl("lbl_CurrTotalAmt");
                    lbl_CurrTotalAmt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                // End on: 29/08/2017, By: Fon
                #endregion

                //Binding display Stock Summary.
                dsStockSum.Clear();

                var strConnStr = bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());
                var getPrDtStockSum = prDt.GetStockSummary(dsStockSum,
                    DataBinder.Eval(e.Row.DataItem, "Product").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                    lbl_PODate.Text, strConnStr);
                if (getPrDtStockSum)
                {
                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        lbl_OnHand.Text = String.Format(DefaultQtyFmt, dsStockSum.Tables[prDt.TableName].Rows[0]["OnHand"]);
                    }

                    if (e.Row.FindControl("lbl_OnOrder") != null)
                    {
                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                        lbl_OnOrder.Text = String.Format(DefaultQtyFmt, dsStockSum.Tables[prDt.TableName].Rows[0]["OnOrder"]);
                    }

                    if (e.Row.FindControl("lbl_Restock") != null)
                    {
                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;
                        lbl_Restock.Text = string.Format(DefaultQtyFmt, dsStockSum.Tables[prDt.TableName].Rows[0]["Restock"]);
                    }

                    if (e.Row.FindControl("lbl_Reorder") != null)
                    {
                        var lbl_Reorder = e.Row.FindControl("lbl_Reorder") as Label;
                        lbl_Reorder.Text = String.Format(DefaultQtyFmt, dsStockSum.Tables[prDt.TableName].Rows[0]["Reorder"]);
                    }

                    if (e.Row.FindControl("lbl_LastVendor") != null)
                    {
                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        lbl_LastVendor.Text = dsStockSum.Tables[prDt.TableName].Rows[0]["LastVendor"].ToString();
                    }

                    if (e.Row.FindControl("lbl_LastPrice") != null)
                    {
                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                        lbl_LastPrice.Text = String.Format(DefaultAmtFmt, dsStockSum.Tables[prDt.TableName].Rows[0]["LastPrice"]);
                    }
                }

                //Binding display Pr Detail.
                var poNo = lbl_PONumber.Text;
                var poDtNo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PoDt").ToString());
                decimal decQtyReq = 0;
                decimal decPrice = 0;
                decimal decAppr = 0;

                dtPrDt.Clear();
                dtPrDt = prDt.GetByPONoPODt(poNo, poDtNo, hf_ConnStr.Value);
                if (dtPrDt.Rows.Count > 0)
                {
                    for (var j = 0; j < dtPrDt.Rows.Count; j++)
                    {
                        decQtyReq += decimal.Parse(dtPrDt.Rows[j]["ReqQty"].ToString());
                        decPrice += decimal.Parse(dtPrDt.Rows[j]["Price"].ToString());
                        decAppr += decimal.Parse(dtPrDt.Rows[j]["ApprQty"].ToString());
                    }

                    if (e.Row.FindControl("lbl_Buyer") != null)
                    {
                        var lbl_Buyer = e.Row.FindControl("lbl_Buyer") as Label;
                        lbl_Buyer.Text = dtPrDt.Rows[0]["Buyer"].ToString();
                    }

                    if (e.Row.FindControl("lbl_BU") != null)
                    {
                        var lbl_BU = e.Row.FindControl("lbl_BU") as Label;
                        lbl_BU.Text = dtPrDt.Rows[0]["BUCode"].ToString();
                    }

                    if (e.Row.FindControl("lbl_Store") != null)
                    {
                        var lbl_Store = e.Row.FindControl("lbl_Store") as Label;
                        lbl_Store.Text = storeLct.GetName(dtPrDt.Rows[0]["LocationCode"].ToString(), hf_ConnStr.Value);
                        //----02/03/2012----storeLct.GetName2(dtPrDt.Rows[0]["LocationCode"].ToString(), hf_ConnStr.Value);
                    }

                    if (e.Row.FindControl("hpl_PRRef") != null)
                    {
                        var hpl_PRRef = e.Row.FindControl("hpl_PRRef") as HyperLink;
                        hpl_PRRef.NavigateUrl = "~/PC/PR/PR.aspx?VID=28&BuCode=" + dtPrDt.Rows[0]["BUCode"] + "&ID=" +
                                                dtPrDt.Rows[0]["PRNo1"];
                        hpl_PRRef.Text = dtPrDt.Rows[0]["PRNo1"].ToString();

                        //Label lbl_PRRef = e.Row.FindControl("lbl_PRRef") as Label;
                        //lbl_PRRef.Text  = dtPrDt.Rows[0]["PRNo1"].ToString();
                    }

                    if (e.Row.FindControl("lbl_PRDate") != null)
                    {
                        var lbl_PRDate = e.Row.FindControl("lbl_PRDate") as Label;
                        lbl_PRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", dtPrDt.Rows[0]["PRDate"]);
                    }

                    if (e.Row.FindControl("lbl_QtyReq") != null)
                    {
                        var lbl_QtyReq = e.Row.FindControl("lbl_QtyReq") as Label;
                        lbl_QtyReq.Text = String.Format(DefaultQtyFmt, decQtyReq) + " " + dtPrDt.Rows[0]["Unit"];
                    }

                    if (e.Row.FindControl("lbl_PricePR") != null)
                    {
                        var lbl_PricePR = e.Row.FindControl("lbl_PricePR") as Label;
                        lbl_PricePR.Text = String.Format(DefaultAmtFmt, dtPrDt.Rows[0]["Price"]);
                    }

                    if (e.Row.FindControl("lbl_Approve") != null)
                    {
                        var lbl_Approve = e.Row.FindControl("lbl_Approve") as Label;
                        lbl_Approve.Text = String.Format(DefaultQtyFmt, decAppr);
                    }

                    if (e.Row.FindControl("grd_PR") != null)
                    {
                        var grd_PR = e.Row.FindControl("grd_PR") as GridView;
                        grd_PR.DataSource = dtPrDt;
                        grd_PR.DataBind();
                    }
                }

                //Binding display Rec Detail.
                dsRecDt.Clear();
                recDt.GetRecDtByPoNoAndPoDtNo(dsRecDt, poNo, poDtNo, strConnStr); //hf_ConnStr.Value

                if (dsRecDt.Tables[recDt.TableName].Rows.Count > 0)
                {
                    var drRecDt = dsRecDt.Tables[recDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_Receive") != null)
                    {
                        var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                        lbl_Receive.Text = String.Format(DefaultQtyFmt, drRecDt["RecQty"]) + " " + drRecDt["RcvUnit"];
                    }

                    if (e.Row.FindControl("lbl_ConvRate") != null)
                    {
                        var lbl_ConvRate = e.Row.FindControl("lbl_ConvRate") as Label;
                        lbl_ConvRate.Text = String.Format("{0:N}", drRecDt["Rate"]);
                    }

                    if (e.Row.FindControl("lbl_BaseQty") != null)
                    {
                        var lbl_BaseQty = e.Row.FindControl("lbl_BaseQty") as Label;
                        var strProd = DataBinder.Eval(e.Row.DataItem, "Product").ToString();
                        var decBaseQty = prodUnit.GetQtyAfterChangeUnit(strProd, drRecDt["RcvUnit"].ToString(),
                            drRecDt["UnitCode"].ToString(),
                            decimal.Parse(drRecDt["OrderQty"].ToString()), hf_ConnStr.Value);
                        var strBaseUnit = prodUnit.GetInvenUnit(strProd, hf_ConnStr.Value);
                        lbl_BaseQty.Text = decBaseQty + " " + strBaseUnit;
                    }
                }
            }

            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if (e.Row.FindControl("lbl_TNet") != null)
            //    {
            //        Label lbl_TNet = e.Row.FindControl("lbl_TNet") as Label;
            //        lbl_TNet.Text = netamt.ToString();
            //    }

            //    if (e.Row.FindControl("lbl_TTax") != null)
            //    {
            //        Label lbl_TTax = e.Row.FindControl("lbl_TTax") as Label;
            //        lbl_TTax.Text = taxamt.ToString();
            //    }

            //    if (e.Row.FindControl("lbl_TAmount") != null)
            //    {
            //        Label lbl_TAmount = e.Row.FindControl("lbl_TAmount") as Label;
            //        lbl_TAmount.Text = totalamt.ToString();
            //    }
            //}
        }

        protected void grd_PoDt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_PoDt.DataSource = dsPo.Tables[poDt.TableName];
            grd_PoDt.EditIndex = e.NewEditIndex;
            grd_PoDt.DataBind();
        }

        protected void grd_PoDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
        }

        protected void grd_PrDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_SKU") != null)
                {
                    var lbl_SKU = e.Row.FindControl("lbl_SKU") as Label;
                    lbl_SKU.Text = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                    lbl_SKU.ToolTip = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("lbl_QTYOrder") != null)
                {
                    var lbl_QTYOrder = e.Row.FindControl("lbl_QTYOrder") as Label;
                    var txt_QtyOrder = e.Row.FindControl("lbl_QTYOrder") as TextBox;

                    lbl_QTYOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString();
                    txt_QtyOrder.Text = DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString();
                }

                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lbl_FOC = e.Row.FindControl("lbl_FOC") as Label;
                    lbl_FOC.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                }

                if (e.Row.FindControl("lbl_RCV") != null)
                {
                    var lbl_RCV = e.Row.FindControl("lbl_RCV") as Label;
                    lbl_RCV.Text = DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString();
                }

                if (e.Row.FindControl("lbl_Cancel") != null)
                {
                    var lbl_Cancel = e.Row.FindControl("lbl_Cancel") as Label;
                    lbl_Cancel.Text = DataBinder.Eval(e.Row.DataItem, "CancelQty").ToString();
                }

                if (e.Row.FindControl("lbl_grdPrice") != null)
                {
                    var lbl_grdPrice = e.Row.FindControl("lbl_grdPrice") as Label;
                    lbl_grdPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chk_Adj = e.Row.FindControl("chk_Adj") as CheckBox;
                    chk_Adj.Checked = (bool)DataBinder.Eval(e.Row.DataItem, "TaxAdj");
                }

                if (e.Row.FindControl("lbl_Net") != null)
                {
                    var lbl_Net = e.Row.FindControl("lbl_Net") as Label;
                    lbl_Net.Text = DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Tax") != null)
                {
                    var lbl_Tax = e.Row.FindControl("lbl_Tax") as Label;
                    lbl_Tax.Text = DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString();
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {
                    var lbl_Amount = e.Row.FindControl("lbl_Amount") as Label;
                    lbl_Amount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString();
                }

                // Display Transaction Detail ---------------------------------------------------------
                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lbl_Store = e.Row.FindControl("lbl_Store") as Label;
                    lbl_Store.Text = //DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() + " : " +
                        storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                    //----02/03/2012----storeLct.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_DeliveryPoint") != null)
                {
                    var lbl_DeliPoint = e.Row.FindControl("lbl_DeliveryPoint") as Label;
                    lbl_DeliPoint.Text = //DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString() + " : " + 
                        deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("lbl_DiscPercent") != null)
                {
                    var lbl_DiscPercent = e.Row.FindControl("lbl_DiscPercent") as Label;
                    lbl_DiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = DataBinder.Eval(e.Row.DataItem, "DiscAmt").ToString();
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("lbl_RefNo") != null)
                {
                    var lbl_RefNo = e.Row.FindControl("lbl_RefNo") as Label;
                    lbl_RefNo.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                }

                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lbl_ReqQty = e.Row.FindControl("lbl_ReqQty") as Label;
                    lbl_ReqQty.Text = DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString();
                }

                if (e.Row.FindControl("lbl_PRNo") != null)
                {
                    var lbl_PRNo = e.Row.FindControl("lbl_PRNo") as Label;
                    lbl_PRNo.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lbl_PRDate = e.Row.FindControl("lbl_PRDate") as Label;
                    lbl_PRDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                // Comment ----------------------------------------------------------------------------
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                // Display Stock Summary --------------------------------------------------------------      
                //if (e.Row.FindControl("uc_StockSummary") != null)
                //{
                //    StockSummary uc_StockSummary = e.Row.FindControl("uc_StockSummary") as StockSummary;

                //    uc_StockSummary.ProductCode  = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();
                //    uc_StockSummary.LocationCode = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                //    uc_StockSummary.ConnStr      = hf_ConnStr.Value;
                //    uc_StockSummary.DataBind();
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TPrNet") != null)
                {
                    var lbl_TPrNet = e.Row.FindControl("lbl_TPrNet") as Label;
                    lbl_TPrNet.Text = netamtpr.ToString();
                }

                if (e.Row.FindControl("lbl_TPrTax") != null)
                {
                    var lbl_TPrTax = e.Row.FindControl("lbl_TPrTax") as Label;
                    lbl_TPrTax.Text = taxamtpr.ToString();
                }

                if (e.Row.FindControl("lbl_TPrAmount") != null)
                {
                    var lbl_TPrAmount = e.Row.FindControl("lbl_TPrAmount") as Label;
                    lbl_TPrAmount.Text = totalamtpr.ToString();
                }
            }
        }

        protected void btn_ProdLog_Click(object sender, EventArgs e)
        {
            pop_ProductLog.ShowOnPageLoad = true;
        }

        protected void btn_SendMsg_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = true;
        }

        protected void btn_Exit_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        protected void btn_Send_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        protected void grd_PR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var strConnStr = bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());

                if (e.Row.FindControl("lbl_BU") != null)
                {
                    var lbl_BU = e.Row.FindControl("lbl_BU") as Label;
                    lbl_BU.Text = DataBinder.Eval(e.Row.DataItem, "BUCode").ToString();
                }

                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lbl_Store = e.Row.FindControl("lbl_Store") as Label;
                    lbl_Store.Text = storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                        strConnStr);
                    //----02/03/2012----storeLct.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), strConnStr);
                }

                if (e.Row.FindControl("hpl_PRRef") != null)
                {
                    //Label lbl_PRRef = e.Row.FindControl("lbl_PRRef") as Label;
                    //lbl_PRRef.Text  = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                    var hpl_PRRef = e.Row.FindControl("hpl_PRRef") as HyperLink;
                    hpl_PRRef.NavigateUrl = "~/PC/PR/PR.aspx?VID=28&BuCode=" + DataBinder.Eval(e.Row.DataItem, "BUCode") +
                                            "&ID=" + DataBinder.Eval(e.Row.DataItem, "PRNo1");
                    hpl_PRRef.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lbl_PRDate = e.Row.FindControl("lbl_PRDate") as Label;
                    lbl_PRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "PRDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString()).ToString();
                }

                if (e.Row.FindControl("lbl_QtyReq") != null)
                {
                    var lbl_QtyReq = e.Row.FindControl("lbl_QtyReq") as Label;
                    lbl_QtyReq.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty")) + " " + DataBinder.Eval(e.Row.DataItem, "Unit");
                }

                if (e.Row.FindControl("lbl_PricePR") != null)
                {
                    var lbl_PricePR = e.Row.FindControl("lbl_PricePR") as Label;
                    lbl_PricePR.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                if (e.Row.FindControl("lbl_Approve") != null)
                {
                    var lbl_Approve = e.Row.FindControl("lbl_Approve") as Label;
                    lbl_Approve.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                }

                if (e.Row.FindControl("lbl_Buyer") != null)
                {
                    var lbl_Buyer = e.Row.FindControl("lbl_Buyer") as Label;
                    lbl_Buyer.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                }
            }
        }
        protected void Print(object sender, EventArgs e)
        {
            string cmd = string.Format("SELECT COUNT(*) AS recordcount FROM PC.PRDt WHERE PoNo ='{0}'", Request.Params["ID"].ToString());
            DataTable dt = po.DbExecuteQuery(@cmd, null, hf_ConnStr.Value);
            if (dt.Rows[0][0].ToString() == "0")
            {
                string msg = "This Purchase Order is not associated with any Purchase Request, please contact administrator.";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", "alert('" + msg + "')", true);
            }
            else
            {

                if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
                {
                    dsPo.Tables[po.TableName].Rows[0]["DocStatus"] = "Printed";
                }

                // Commit change to database
                var resultSave = po.SaveOnlyPO(dsPo, hf_ConnStr.Value);

                //if (resultSave)
                //{
                //}
                // Send PoNo  to Session
                //var objArrList = new ArrayList();
                //objArrList.Add(dsPo.Tables[po.TableName].Rows[0]["PoNo"]);
                //Session["s_arrNo"] = objArrList;

                //var paramField = new string[1, 2];
                //paramField[0, 0] = "BU";
                //paramField[0, 1] = LoginInfo.BuInfo.BuName;
                ////paramField[1, 0]    = "Pm-vPODt.PoNo";
                //////paramField[1, 1]  = LoginInfo.BuInfo.BuName.ToString();
                //Session["paramField"] = paramField;


                //Session.Remove("SubReportName");
                //Session["SubReportName"] = "prsign";

                //Blue.BL.APP.Config config = new Blue.BL.APP.Config();
                //var pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseOrderForm";
                //ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");

                _transLog.Save("PC", "PO", lbl_PONumber.Text, "PRINT", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);


                Report rpt = new Report();
                rpt.PrintForm(this, "../../RPT/PrintForm.aspx", Request.Params["ID"].ToString(), "PurchaseOrderForm");
            }
        }


        protected void btn_Pop_SendMail_Send_Click(object sender, EventArgs e)
        {

            lbl_Pop_SendMail_Message.Text = poReportMail.SendEmailToSupplier(lbl_PONumber.Text, txt_Pop_SendMail_Email.Text, hf_ConnStr.Value);

            if (lbl_Pop_SendMail_Message.Text == string.Empty)
            {
                pop_SendMail.ShowOnPageLoad = false;
                _transLog.Save("PC", "PO", lbl_PONumber.Text, "EMAIL", string.Format("Sent email to {0}", txt_Pop_SendMail_Email.Text), LoginInfo.LoginName, LoginInfo.ConnStr);
                Page.Response.Redirect(Page.Request.Url.ToString(), true);


            }
        }


    }
}