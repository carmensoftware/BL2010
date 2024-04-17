using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.EOP
{
    public partial class EOPPrint : BasePage
    {
        private readonly Blue.BL.PC.EOP eop = new Blue.BL.PC.EOP();
        private readonly Blue.BL.PC.EOPDt eopDt = new Blue.BL.PC.EOPDt();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private DataSet dsEOP = new DataSet();
        private Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsEOP = (DataSet) Session["dsEOP"];
            }
        }

        private void Page_Retrieve()
        {
            // Edit
            var EOPId = int.Parse(Request.Params["ID"]);

            var getEOP = eop.Get(dsEOP, EOPId, LoginInfo.ConnStr);

            if (!getEOP)
            {
                // Display Error Message
                return;
            }

            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            var getEOPDt = eopDt.GetList(dsEOP, EOPId, LoginInfo.ConnStr);

            if (!getEOPDt)
            {
                // Display Error Message
                return;
            }

            Session["dsEOP"] = dsEOP;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drEop = dsEOP.Tables[eop.TableName].Rows[0];

            lbl_Store.Text = drEop["StoreId"] + store.GetName(drEop["StoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----store.GetName2(drEop["StoreId"].ToString(), LoginInfo.ConnStr);
            lbl_Date.Text = DateTime.Parse(drEop["Date"].ToString()).Date.ToString("dd/MM/yyyy");
            lbl_EndDate.Text = DateTime.Parse(drEop["EndDate"].ToString()).ToString("dd/MM/yyyy");
            lbl_Status.Text = drEop["Status"].ToString();
            lbl_Description.Text = drEop["Description"].ToString();
            lbl_Remark.Text = drEop["Remark"].ToString();

            grd_Product.DataSource = dsEOP.Tables[eopDt.TableName];
            grd_Product.DataBind();
        }

        protected void grd_Product_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("txt_Qty") != null)
                {
                    var txt_Qty = (TextBox) e.Row.FindControl("txt_Qty");
                    txt_Qty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }
            }
        }
    }
}