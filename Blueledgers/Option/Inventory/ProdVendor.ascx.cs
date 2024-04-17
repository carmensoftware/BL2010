using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdVendor : BaseUserControl
    {
        private readonly Blue.BL.IN.VendorProd vendorProd = new Blue.BL.IN.VendorProd();

        private string _connStr;
        private DataSet _dataSource = new DataSet();
        private string _productCode;

        public string ProductCode
        {
            get
            {
                if (ViewState["ProductCode"] != null)
                {
                    _productCode = ViewState["ProductCode"].ToString();
                }

                return _productCode;
            }
            set
            {
                _productCode = value;
                ViewState["ProductCode"] = _productCode;
            }
        }

        public string ConnStr
        {
            get
            {
                if (ViewState["ConnStr"] != null)
                {
                    _connStr = ViewState["ConnStr"].ToString();
                }

                return _connStr;
            }
            set
            {
                _connStr = value;
                ViewState["ConnStr"] = _connStr;
            }
        }

        /// <summary>
        ///     Keep [IN].ProdLoc data
        /// </summary>
        private DataSet DataSource
        {
            get
            {
                if (Session["DataSourceProdLoc"] != null)
                {
                    _dataSource = (DataSet) Session["DataSourceProdLoc"];
                }

                return _dataSource;
            }
            set
            {
                _dataSource = value;
                Session["DataSourceProdLoc"] = _dataSource;
            }
        }

        public override void DataBind()
        {
            base.DataBind();

            // Get Vendor's Product data.
            var dsVendorProd = new DataSet();

            var result = vendorProd.GetVendorProdList(dsVendorProd, ProductCode, ConnStr);

            if (result)
            {
                grd_ProdVendor.DataSource = dsVendorProd;
                grd_ProdVendor.DataBind();
                DataSource = dsVendorProd;
            }

            // Check select all
            var count = 0;

            foreach (GridViewRow gvr_ProdVendor in grd_ProdVendor.Rows)
            {
                var chk_SelVendor = gvr_ProdVendor.FindControl("chk_SelVendor") as CheckBox;

                if (chk_SelVendor.Checked)
                {
                    count++;
                }
            }

            var chk_SelAllVendor = grd_ProdVendor.HeaderRow.FindControl("chk_SelAllVendor") as CheckBox;
            chk_SelAllVendor.Checked = (grd_ProdVendor.Rows.Count == count ? true : false);
        }

        protected void imgb_Save_Click(object sender, ImageClickEventArgs e)
        {
            var dsVendorProd = new DataSet();

            // Get Exist Vendor's Product Data.
            var getvendorProd = vendorProd.GetList(dsVendorProd, ProductCode, ConnStr);

            if (!getvendorProd)
            {
                return;
            }

            // Delete Exist Vendor's Product Data.
            for (var i = dsVendorProd.Tables[vendorProd.TableName].Rows.Count - 1; i >= 0; i--)
            {
                dsVendorProd.Tables[vendorProd.TableName].Rows[i].Delete();
            }

            // Create New Vendor's Product Data.
            foreach (GridViewRow gvrVendorProd in grd_ProdVendor.Rows)
            {
                var chk_SelVendor = gvrVendorProd.FindControl("chk_SelVendor") as CheckBox;
                var hf_VendorCode = gvrVendorProd.FindControl("hf_VendorCode") as HiddenField;

                if (chk_SelVendor.Checked)
                {
                    var drNewVendorProd = dsVendorProd.Tables[vendorProd.TableName].NewRow();
                    drNewVendorProd["ProductCode"] = ProductCode;
                    drNewVendorProd["VendorCode"] = hf_VendorCode.Value;
                    dsVendorProd.Tables[vendorProd.TableName].Rows.Add(drNewVendorProd);
                }
            }

            // Save Change to Database.
            vendorProd.Save(dsVendorProd, ConnStr);

            // Refresh control
            DataBind();
        }
    }
}