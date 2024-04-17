using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.REC
{
    public partial class RECPrint : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliveryPoint =
            new Blue.BL.Option.Inventory.DeliveryPoint();

        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private DataSet dsRec = new DataSet();
        private Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();

        private string recNo = string.Empty;

        #endregion

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
                dsRec = (DataSet) Session["dsRec"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                recNo = Request.Params["ID"];

                var getStrLct = rec.GetListByRecNo(dsTmp, ref MsgError, recNo, LoginInfo.ConnStr);

                if (getStrLct)
                {
                    dsRec = dsTmp;

                    recDt.GetListByRecNo(dsRec, recNo, LoginInfo.ConnStr);
                }
                else
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }
            }

            Page_Setting();

            Session["dsRec"] = dsRec;
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            if (dsRec.Tables[rec.TableName] != null)
            {
                var drRec = dsRec.Tables[rec.TableName].Rows[0];

                lbl_RecNo.Text = drRec["RecNo"].ToString();

                if (drRec["RecDate"].ToString() != string.Empty)
                {
                    lbl_RecDate.Text = DateTime.Parse(drRec["RecDate"].ToString())
                        .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                lbl_DocStatus.Text = drRec["DocStatus"].ToString();
                lbl_InvNo.Text = drRec["InvoiceNo"].ToString();

                if (drRec["InvoiceDate"].ToString() != string.Empty)
                {
                    lbl_InvDate.Text =
                        DateTime.Parse(drRec["InvoiceDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                lbl_DeliPoint.Text = deliveryPoint.GetName(drRec["DeliPoint"].ToString(), LoginInfo.ConnStr);
                lbl_VendorCode.Text = drRec["VendorCode"].ToString();
                lbl_VendorNm.Text = vendor.GetName(drRec["VendorCode"].ToString(), LoginInfo.ConnStr);
                lbl_Currency.Text = drRec["CurrencyCode"].ToString();
                lbl_ExRateAudit.Text = drRec["ExRateAudit"].ToString();
                chk_CashConsign.Checked = bool.Parse(drRec["IsCashConsign"].ToString());
                lbl_Desc.Text = drRec["Description"].ToString();

                grd_RecEdit.DataSource = dsRec.Tables[recDt.TableName];
                grd_RecEdit.DataBind();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnk_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecEdit.aspx?MODE=EDIT&ID=" + dsRec.Tables[rec.TableName].Rows[0]["RecNo"]);
        }

        protected void lnk_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PC/REC/RecLst.aspx");
        }
    }
}