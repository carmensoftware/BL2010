using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdUnit : BaseUserControl
    {
        public enum ProdUnitType
        {
            Inventory,
            Order,
            Recipe
        }

        private readonly Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private string _connStr;
        private DataSet _dataSource = new DataSet();

        private string _productCode;
        private ProdUnitType _unitType;

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

        public ProdUnitType UnitType
        {
            get
            {
                if (ViewState["UnitType"] != null)
                {
                    _unitType = (ProdUnitType) ViewState["UnitType"];
                }

                return _unitType;
            }
            set
            {
                _unitType = value;
                ViewState["UnitType"] = _unitType;
            }
        }

        private DataSet DataSource
        {
            get
            {
                switch (UnitType)
                {
                    case ProdUnitType.Order:

                        if (Session["DataSourceOrder"] != null)
                        {
                            _dataSource = (DataSet) Session["DataSourceOrder"];
                        }

                        break;

                    case ProdUnitType.Recipe:

                        if (Session["DataSourceRecipe"] != null)
                        {
                            _dataSource = (DataSet) Session["DataSourceRecipe"];
                        }

                        break;
                }

                return _dataSource;
            }
            set
            {
                _dataSource = value;

                switch (UnitType)
                {
                    case ProdUnitType.Order:

                        Session["DataSourceOrder"] = _dataSource;

                        break;

                    case ProdUnitType.Recipe:

                        Session["DataSourceRecipe"] = _dataSource;

                        break;
                }
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

        public override void DataBind()
        {
            base.DataBind();

            // Get ProdUnit data.
            switch (UnitType)
            {
                case ProdUnitType.Order:

                    var dsProdUnitOrder = new DataSet();
                    prodUnit.GetList(dsProdUnitOrder, ProductCode, "O", ConnStr);
                    DataSource = dsProdUnitOrder;

                    break;

                case ProdUnitType.Recipe:

                    var dsProdUnitRecipe = new DataSet();
                    prodUnit.GetList(dsProdUnitRecipe, ProductCode, "R", ConnStr);
                    DataSource = dsProdUnitRecipe;

                    break;
            }

            // Title
            lbl_Title.Text = UnitType.ToString();

            // Display ProdUnit data.
            grd_ProdUnit.DataSource = DataSource;
            grd_ProdUnit.EditIndex = -1;
            grd_ProdUnit.DataBind();
        }

        /// <summary>
        ///     Display product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdUnit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnkb_Del") != null)
                {
                    var lnkb_Del = e.Row.FindControl("lnkb_Del") as LinkButton;
                    lnkb_Del.OnClientClick = "return ConfrimDelete('" + DataBinder.Eval(e.Row.DataItem, "IsDefault") +
                                             "')";
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = unit.GetName(DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString(), ConnStr);
                }
                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl_Unit = e.Row.FindControl("ddl_Unit") as DropDownList;
                    ddl_Unit.DataSource = unit.GetUnitLookup(ConnStr);
                    ddl_Unit.DataTextField = "Name";
                    ddl_Unit.DataValueField = "UnitCode";
                    ddl_Unit.DataBind();
                    ddl_Unit.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                    ddl_Unit.SelectedValue = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                }

                if (e.Row.FindControl("lbl_Rate") != null)
                {
                    var lbl_Rate = e.Row.FindControl("lbl_Rate") as Label;
                    lbl_Rate.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Rate")).ToString("0.000");
                        // TODO: Number format should take from setting
                }
                if (e.Row.FindControl("txt_Rate") != null)
                {
                    var txt_Rate = e.Row.FindControl("txt_Rate") as TextBox;
                    txt_Rate.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Rate")).ToString("0.000");
                        // TODO: Number format should take from setting
                }

                if (e.Row.FindControl("chk_IsDefault") != null)
                {
                    var chk_IsDefault = e.Row.FindControl("chk_IsDefault") as CheckBox;
                    chk_IsDefault.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsDefault"));
                }
                if (e.Row.FindControl("chk_IsDefaultEdit") != null)
                {
                    var chk_IsDefaultEdit = e.Row.FindControl("chk_IsDefaultEdit") as CheckBox;
                    chk_IsDefaultEdit.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsDefault"));
                }
            }
        }

        /// <summary>
        ///     Edit exist product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdUnit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_ProdUnit.DataSource = DataSource;
            grd_ProdUnit.EditIndex = e.NewEditIndex;
            grd_ProdUnit.DataBind();
        }

        /// <summary>
        ///     Delete exist product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdUnit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Delete selected row
            DataSource.Tables[prodUnit.TableName].Rows[e.RowIndex].Delete();

            // Save change to database
            prodUnit.Save(DataSource, ConnStr);

            // Refresh ProdUnit data.
            DataBind();
        }

        /// <summary>
        ///     Update create/edit product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdUnit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // TODO: Validate only one rate = 1

            // Find Edit Controls
            var gvr_ProdUnit = grd_ProdUnit.Rows[e.RowIndex];
            var ddl_Unit = gvr_ProdUnit.FindControl("ddl_Unit") as DropDownList;
            var txt_Rate = gvr_ProdUnit.FindControl("txt_Rate") as TextBox;
            var chk_IsDefaultEdit = gvr_ProdUnit.FindControl("chk_IsDefaultEdit") as CheckBox;

            // Update change
            var drUpdating = DataSource.Tables[0].Rows[gvr_ProdUnit.DataItemIndex];
            drUpdating["ProductCode"] = ProductCode;
            drUpdating["OrderUnit"] = ddl_Unit.SelectedItem.Value;
            drUpdating["Rate"] = txt_Rate.Text.Trim();
            drUpdating["IsDefault"] = chk_IsDefaultEdit.Checked;
            drUpdating["UnitType"] = (UnitType == ProdUnitType.Order ? "O" : "R");

            // If default was checked, take of default unit flag (Order/Recipe) from another products
            if (chk_IsDefaultEdit.Checked)
            {
                for (var i = 0; i < DataSource.Tables[0].Rows.Count; i++)
                {
                    if (i != gvr_ProdUnit.DataItemIndex)
                    {
                        DataSource.Tables[0].Rows[i]["IsDefault"] = false;
                    }
                }
            }

            // Save changed to database
            prodUnit.Save(DataSource, ConnStr);

            // Update default Unit to product
            if (chk_IsDefaultEdit.Checked)
            {
                // Update product data.
                var dsProduct = new DataSet();
                var result = prod.Get(dsProduct, ProductCode, ConnStr);

                if (result)
                {
                    var drProduct = dsProduct.Tables[prod.TableName].Rows[0];

                    switch (UnitType)
                    {
                        case ProdUnitType.Order:

                            drProduct["OrderUnit"] = ddl_Unit.SelectedItem.Value;
                            drProduct["InventoryConvOrder"] = txt_Rate.Text.Trim();

                            break;

                        case ProdUnitType.Recipe:

                            drProduct["RecipeUnit"] = ddl_Unit.SelectedItem.Value;
                            drProduct["RecipeConvInvent"] = txt_Rate.Text.Trim();

                            break;
                    }

                    drProduct["UpdatedDate"] = ServerDateTime;
                    drProduct["UpdatedBy"] = LoginInfo.LoginName;
                }

                // Save change product to database
                prod.Save(dsProduct, ConnStr);

                // Refresh Page
                Response.Redirect(Request.Url.AbsoluteUri);
            }

            // Refresh prod unit data.
            DataBind();
        }

        /// <summary>
        ///     Cancel create/edit product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ProdUnit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataBind();
        }

        /// <summary>
        ///     Create new product unit (Order/Recipe)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgb_CreateOrder_Click(object sender, ImageClickEventArgs e)
        {
            // Default new row
            var drNew = DataSource.Tables[0].NewRow();
            drNew["ProductCode"] = ProductCode;
            drNew["OrderUnit"] = string.Empty;
            drNew["Rate"] = 0;
            drNew["IsDefault"] = false;
            drNew["UnitType"] = (UnitType == ProdUnitType.Order ? "O" : "R");
            DataSource.Tables[0].Rows.Add(drNew);

            // Edit on the new row
            grd_ProdUnit.DataSource = DataSource;
            grd_ProdUnit.EditIndex = DataSource.Tables[0].Rows.Count - 1;
            grd_ProdUnit.DataBind();
        }

        /// <summary>
        ///     Check duplicate unit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Unit = sender as DropDownList;

            foreach (GridViewRow gvrProdUnit in grd_ProdUnit.Rows)
            {
                if (gvrProdUnit.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = gvrProdUnit.FindControl("lbl_Unit") as Label;

                    if (lbl_Unit.Text.ToUpper() == ddl_Unit.SelectedItem.Value.ToUpper())
                    {
                        // Reset selected value to empty
                        ddl_Unit.SelectedValue = string.Empty;

                        // Display Error Duplicate Unit
                        AlertMessageBox("Duplicate Unit.");
                        //Response.Write("<script type=\"text/javascript\">alert('Duplicate Unit.')</script>");

                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Check at least one default unit require.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_IsDefaultEdit_CheckedChanged(object sender, EventArgs e)
        {
            var chk_IsDefaultEdit = sender as CheckBox;

            if (!chk_IsDefaultEdit.Checked)
            {
                chk_IsDefaultEdit.Checked = true;

                // Display warning message
                AlertMessageBox("At least one default unit is require");
                //Response.Write("<script type=\"text/javascript\">alert('At least one default unit is require')</script>");
            }
        }

        /// <summary>
        ///     Error message
        /// </summary>
        /// <param name="msg"></param>
        protected void AlertMessageBox(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof (Page), "Alert", "alert('" + msg + "');", true);
        }
    }
}