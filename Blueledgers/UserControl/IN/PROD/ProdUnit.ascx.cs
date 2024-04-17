using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.IN.PROD
{
    public partial class ProdUnit : BaseUserControl
    {
        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.IN.ProdUnit _prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product _product = new Blue.BL.Option.Inventory.Product();
        private string _productCode;
        private DataSet _dsProdUnit = new DataSet();

        public string ProductCode
        {
            get
            {
                _productCode = ViewState["ProductCode"] != null ? ViewState["ProductCode"].ToString() : string.Empty;
                return _productCode;
            }
            set
            {
                _productCode = value;
                ViewState["ProductCode"] = _productCode;
            }
        }

        public override void DataBind()
        {
            base.DataBind();

            if (_dsProdUnit != null)
            {
                _dsProdUnit.Clear();
            }

            // Display Conversion Rate data of specified ProductCode.
            var getProdUnit = _prodUnit.GetList(_dsProdUnit, ProductCode, _bu.GetConnectionString(Request.Params["BuCode"]));

            if (getProdUnit)
            {
                if (_dsProdUnit != null)
                {
                    grd_ProdUnit.DataSource = _dsProdUnit.Tables[_prodUnit.TableName];
                    grd_ProdUnit.DataBind();
                }
            }

            // Display Product list
            ddl_Product.DataSource = _product.GetList(_bu.GetConnectionString(Request.Params["BuCode"]));
            ddl_Product.DataTextField = "ProductDesc1";
            ddl_Product.DataValueField = "ProductCode";
            ddl_Product.DataBind();
            ddl_Product.SelectedValue = ProductCode;

            // Display all unit
            if (_dsProdUnit != null && _dsProdUnit.Tables["UnitList"] != null)
            {
                _dsProdUnit.Tables["UnitList"].Clear();
            }

            var getUnit = _prodUnit.GetUnitList(_dsProdUnit, ProductCode, _bu.GetConnectionString(Request.Params["BuCode"]));

            if (getUnit)
            {
                if (_dsProdUnit != null)
                {
                    grd_Unit.DataSource = _dsProdUnit.Tables["UnitList"];
                    grd_Unit.DataBind();
                }
            }

            Session["dsProdUnit"] = _dsProdUnit;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Do nothings
            }
            else
            {
                _dsProdUnit = Session["dsProdUnit"] as DataSet;
            }
        }

        /// <summary>
        ///     Open popup for and new unit conversion rate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkb_New_Click(object sender, EventArgs e)
        {
            //ddl_Product.Value = this.ProductCode;

            // Show popup
            pop_ProdUnit.ShowOnPageLoad = true;
        }

        //protected void ddl_Product_Load(object sender, EventArgs e)
        //{
        //    // Display Product list data.
        //    ddl_Product.DataSource = product.GetActiveList(LoginInfo.ConnStr);
        //    ddl_Product.DataBind();
        //}
        //protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grd_Unit.DataSource = prodUnit.GetList(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
        //    grd_Unit.DataBind();
        //}
        /// <summary>
        ///     Display Product data.
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        /// <summary>
        ///     Refresh product unit conversion rate list.
        /// </summary>
        /// <summary>
        ///     Refresh product unit conversion rate list.
        /// </summary>
        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dsProdUnit.Tables["UnitList"] != null)
            {
                _dsProdUnit.Tables["UnitList"].Clear();
            }

            var getUnit = _prodUnit.GetUnitList(_dsProdUnit, ddl_Product.SelectedItem.Value,
                _bu.GetConnectionString(Request.Params["BuCode"]));

            if (getUnit)
            {
                grd_Unit.DataSource = _dsProdUnit.Tables["UnitList"];
                grd_Unit.DataBind();
            }

            Session["dsProdUnit"] = _dsProdUnit;
        }

        /// <summary>
        ///     Display product unit conversion data for edit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Unit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("chk_Selected") != null)
                {
                    var chkSelected = e.Row.FindControl("chk_Selected") as CheckBox;
                    if (chkSelected != null)
                    {
                        chkSelected.Checked = bool.Parse(DataBinder.Eval(e.Row.DataItem, "Selected").ToString());
                    }
                }
            }
        }

        /// <summary>
        ///     Update Unit Conversion Rate to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OK_Click(object sender, EventArgs e)
        {
            // Delete all exist conversion rate
            //foreach (DataRow drProdUnit in dsProdUnit.Tables[prodUnit.TableName].Rows)
            //{
            //    drProdUnit.Delete();
            //}

            for (var i = _dsProdUnit.Tables[_prodUnit.TableName].Rows.Count - 1; i >= 0; i--)
            {
                var drProdUnit = _dsProdUnit.Tables[_prodUnit.TableName].Rows[i];
                drProdUnit.Delete();
            }

            // Check base unit required by check only one rate = 1.00
            var baseUnitCount = 0;

            foreach (GridViewRow gvrUnit in grd_Unit.Rows)
            {
                // Insert new conversion rate (only selected unit which rate not equal to 0)  
                var chkSelected = gvrUnit.FindControl("chk_Selected") as CheckBox;
                var lblUnit = gvrUnit.FindControl("lbl_Unit") as Label;
                var txtRate = gvrUnit.FindControl("txt_Rate") as TextBox;

                if (chkSelected != null && chkSelected.Checked)
                {
                    if (txtRate != null && decimal.Parse(txtRate.Text.Trim()) != 0)
                    {
                        var drNewProdUnit = _dsProdUnit.Tables[_prodUnit.TableName].NewRow();
                        drNewProdUnit["ProductCode"] = ProductCode;
                        if (lblUnit != null) drNewProdUnit["OrderUnit"] = lblUnit.Text;
                        drNewProdUnit["Rate"] = decimal.Parse(txtRate.Text.Trim());
                        drNewProdUnit["IsDefault"] = false;
                        drNewProdUnit["UnitType"] = "O";
                        _dsProdUnit.Tables[_prodUnit.TableName].Rows.Add(drNewProdUnit);

                        if (decimal.Parse(txtRate.Text.Trim()) == 1)
                        {
                            baseUnitCount++;
                        }
                    }
                }
            }

            if (baseUnitCount == 0)
            {
                // Display error message : There is no base unit assign
                lbl_Error.Text = @"There is no base unit assign (Rate=1)";
                pop_Error.ShowOnPageLoad = true;
                return;
            }
            //else if (baseUnitCount > 1)
            //{
            //    // Display error message : Connot assign more than one Rate = 1
            //    lbl_Error.Text = "Conversion Rate = 1 can assign to only one Unit";
            //    pop_Error.ShowOnPageLoad = true;
            //    return;
            //}

            // Save change to database
            var saved = _prodUnit.Save(_dsProdUnit, _bu.GetConnectionString(Request.Params["BuCode"]));

            if (saved)
            {
                DataBind();
                pop_ProdUnit.ShowOnPageLoad = false;
            }
        }

        /// <summary>
        ///     Close popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_ProdUnit.ShowOnPageLoad = false;
        }
    }
}