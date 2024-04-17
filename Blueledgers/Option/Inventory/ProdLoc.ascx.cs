using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdLoc : BaseUserControl
    {
        private readonly Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private string _connStr;
        private DataSet _dataSource = new DataSet();
        private string _productCode;
        private Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();

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

        /// <summary>
        ///     Display product location data.
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            // Show message if no assign store.
            if (prodLoc.CountByProductCode(ProductCode, ConnStr) == 0)
            {
                lbl_MsgNoAssign.Text = "Product is not assigned to store.";
                    // TODO: This message should take from setting
            }
            else
            {
                lbl_MsgNoAssign.Text = "";
            }

            // Get data
            var dsProdLoc = new DataSet();

            var result = prodLoc.GetList_ProductCode(dsProdLoc, ProductCode, ConnStr);

            if (result)
            {
                grd_ProdLoc.DataSource = dsProdLoc;
                grd_ProdLoc.DataBind();
                DataSource = dsProdLoc;
            }

            // Check select all
            var count = 0;

            foreach (GridViewRow gvr_ProdLoc in grd_ProdLoc.Rows)
            {
                var chk_Sel = gvr_ProdLoc.FindControl("chk_Sel") as CheckBox;

                if (chk_Sel.Checked)
                {
                    count++;
                }
            }

            var chk_SelAll = grd_ProdLoc.HeaderRow.FindControl("chk_SelAll") as CheckBox;
            chk_SelAll.Checked = (grd_ProdLoc.Rows.Count == count ? true : false);
        }

        /// <summary>
        ///     Save change to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgb_Save_Click(object sender, ImageClickEventArgs e)
        {
            var dsProdLoc = new DataSet();

            // Get exist ProdLoc data.
            prodLoc.GetList(dsProdLoc, ProductCode, ConnStr);

            // Delete exist ProdLoc data.
            for (var i = dsProdLoc.Tables[prodLoc.TableName].Rows.Count - 1; i >= 0; i--)
            {
                dsProdLoc.Tables[prodLoc.TableName].Rows[i].Delete();
            }

            // Save only checked store
            foreach (GridViewRow gvr_ProdLoc in grd_ProdLoc.Rows)
            {
                if (gvr_ProdLoc.RowType == DataControlRowType.DataRow)
                {
                    // Find checkbox
                    var chk_Sel = gvr_ProdLoc.FindControl("chk_Sel") as CheckBox;
                    var hf_LocationCode = gvr_ProdLoc.FindControl("hf_LocationCode") as HiddenField;
                    var txt_Min = gvr_ProdLoc.FindControl("txt_Min") as TextBox;
                    var txt_Max = gvr_ProdLoc.FindControl("txt_Max") as TextBox;

                    if (chk_Sel.Checked)
                    {
                        var drNewProdLoc = dsProdLoc.Tables[prodLoc.TableName].NewRow();
                        drNewProdLoc["LocationCode"] = hf_LocationCode.Value;
                        drNewProdLoc["ProductCode"] = ProductCode;

                        if (txt_Min.Text.Trim() != string.Empty)
                        {
                            drNewProdLoc["MinQty"] = txt_Min.Text;
                        }

                        if (txt_Max.Text.Trim() != string.Empty)
                        {
                            drNewProdLoc["MaxQty"] = txt_Max.Text;
                        }

                        dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drNewProdLoc);
                    }
                }
            }

            // Save to database
            var saveProdLoc = prodLoc.Save(dsProdLoc, ConnStr);

            // Refresh control
            DataBind();
        }
    }
}