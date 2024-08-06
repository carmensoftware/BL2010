using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class UnitConv : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private readonly Blue.BL.Option.Inventory.UnitConv unitConv = new Blue.BL.Option.Inventory.UnitConv();

        private DataSet dsUnitConv = new DataSet();
        private DataTable dtUnit = new DataTable();
        private Blue.BL.dbo.Lang lang = new Blue.BL.dbo.Lang();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            //base.Page_Load(sender, e);
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsUnitConv = (DataSet) Session["dsUnitConv"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            var getUnitConv = unitConv.GetList(dsTmp, ref MsgError, LoginInfo.ConnStr);

            if (getUnitConv)
            {
                dsUnitConv = dsTmp;
            }
            else
            {
                // Display error message
                lbl_Error.Text = Resources.MsgError.ResourceManager.GetString(MsgError);

                return;
            }

            Page_Setting();

            Session["dsUnitConv"] = dsUnitConv;
        }

        /// <summary>
        ///     Display business unit data which retrieved from Page_Retrieve procedure.
        /// </summary>
        private void Page_Setting()
        {
            SetLookup();

            grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
            grd_UnitConv.DataBind();

            lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();

            btn_New.Enabled = true;
        }

        /// <summary>
        /// </summary>
        private void SetLookup()
        {
            dtUnit = null;
            dtUnit = unit.GetList(LoginInfo.ConnStr);

            var drBlank = dtUnit.NewRow();
            dtUnit.Rows.InsertAt(drBlank, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UnitConv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            lbl_Error.Text = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display
                if (e.Row.FindControl("lbl_BaseUnit") != null)
                {
                    var lbl_BaseUnit = (Label) e.Row.FindControl("lbl_BaseUnit");
                    lbl_BaseUnit.Text = unit.GetName(DataBinder.Eval(e.Row.DataItem, "FUnitCode").ToString(),
                        LoginInfo.ConnStr);
                }

                // Display lookup baseunit
                if (e.Row.FindControl("ddl_BaseUnit") != null)
                {
                    var ddl_BaseUnit = (DropDownList) e.Row.FindControl("ddl_BaseUnit");

                    if (dtUnit != null)
                    {
                        ddl_BaseUnit.DataSource = dtUnit;
                        ddl_BaseUnit.DataTextField = "Name";
                        ddl_BaseUnit.DataValueField = "UnitCode";
                        ddl_BaseUnit.DataBind();
                        ddl_BaseUnit.SelectedValue = DataBinder.Eval(e.Row.DataItem, "FUnitCode").ToString();
                    }
                }

                // Display
                if (e.Row.FindControl("lbl_ConvUnit") != null)
                {
                    var lbl_ConvUnit = (Label) e.Row.FindControl("lbl_ConvUnit");
                    lbl_ConvUnit.Text = unit.GetName(DataBinder.Eval(e.Row.DataItem, "TUnitCode").ToString(),
                        LoginInfo.ConnStr);
                }

                // Display lookup baseunit
                if (e.Row.FindControl("ddl_ConvUnit") != null)
                {
                    var ddl_ConvUnit = (DropDownList) e.Row.FindControl("ddl_ConvUnit");

                    if (dtUnit != null)
                    {
                        ddl_ConvUnit.DataSource = dtUnit;
                        ddl_ConvUnit.DataTextField = "Name";
                        ddl_ConvUnit.DataValueField = "UnitCode";
                        ddl_ConvUnit.DataBind();
                        ddl_ConvUnit.SelectedValue = DataBinder.Eval(e.Row.DataItem, "TUnitCode").ToString();
                    }
                }

                // Display
                if (e.Row.FindControl("lbl_Factor") != null)
                {
                    var lbl_Factor = (Label) e.Row.FindControl("lbl_Factor");
                    lbl_Factor.Text = (Double.Parse(DataBinder.Eval(e.Row.DataItem, "Factor").ToString())).ToString();
                }

                // Display lookup baseunit
                if (e.Row.FindControl("txt_Factor") != null)
                {
                    var txt_Factor = (TextBox) e.Row.FindControl("txt_Factor");
                    txt_Factor.Text = DataBinder.Eval(e.Row.DataItem, "Factor").ToString();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UnitConv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lbl_Error.Text = string.Empty;

            if (dsUnitConv.Tables[unitConv.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                dsUnitConv.Tables[unitConv.TableName].Rows[e.RowIndex].Delete();
            }

            // Binding grid
            grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
            grd_UnitConv.EditIndex = -1;
            grd_UnitConv.DataBind();

            btn_New.Enabled = true;

            lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UnitConv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lbl_Error.Text = string.Empty;

            dsUnitConv.Tables[unitConv.TableName].Rows[e.RowIndex].Delete();

            // Delete row
            var MsgError = string.Empty;
            var deleted = unitConv.Save(dsUnitConv, ref MsgError, LoginInfo.ConnStr);

            if (deleted)
            {
                // Binding grid
                grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
                grd_UnitConv.DataBind();

                btn_New.Enabled = true;
                lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();

                // Save changed to session
                Session["dsUnitConv"] = dsUnitConv;
            }
            else
            {
                dsUnitConv.Tables[unitConv.TableName].Rows[e.RowIndex].RejectChanges();

                // Display error message                
                lbl_Error.Text = Resources.MsgError.ResourceManager.GetString(MsgError);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UnitConv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lbl_Error.Text = string.Empty;

            SetLookup();

            grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
            grd_UnitConv.EditIndex = e.NewEditIndex;
            grd_UnitConv.DataBind();

            btn_New.Enabled = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_UnitConv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var MsgError = string.Empty;
            lbl_Error.Text = string.Empty;

            var drUpdating = dsUnitConv.Tables[unitConv.TableName].Rows[e.RowIndex];

            var ddl_BaseUnit = (DropDownList) grd_UnitConv.Rows[e.RowIndex].FindControl("ddl_BaseUnit");
            var ddl_ConvUnit = (DropDownList) grd_UnitConv.Rows[e.RowIndex].FindControl("ddl_ConvUnit");
            var txt_Factor = (TextBox) grd_UnitConv.Rows[e.RowIndex].FindControl("txt_Factor");

            if (ddl_BaseUnit.SelectedValue != string.Empty & ddl_ConvUnit.SelectedValue != string.Empty &
                txt_Factor.Text != string.Empty)
            {
                // Updating for assign data.
                // FUnitCode
                drUpdating["FUnitCode"] = ddl_BaseUnit.SelectedItem.Value;

                // TUnitCode
                drUpdating["TUnitCode"] = ddl_ConvUnit.SelectedItem.Value;

                // Factor
                drUpdating["Factor"] = double.Parse(txt_Factor.Text);

                // UpdateDate
                drUpdating["UpdatedDate"] = ServerDateTime;

                // UpdateBy
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;


                // Save data from database            
                var result = unitConv.Save(dsUnitConv, ref MsgError, LoginInfo.ConnStr);

                if (result)
                {
                    // Refresh data in GridView
                    grd_UnitConv.EditIndex = -1;
                    grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
                    grd_UnitConv.DataBind();

                    btn_New.Enabled = true;

                    lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();

                    // Save changed to session
                    Session["dsUnitConv"] = dsUnitConv;
                }
                else
                {
                    // Display error message
                    lbl_Error.Text = Resources.MsgError.ResourceManager.GetString(MsgError);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_Factor_TextChanged(object sender, EventArgs e)
        {
            var txt_Factor = (TextBox) ((TextBox) sender).Parent.Parent.FindControl("txt_Factor");
            var dbFactor = Double.Parse(txt_Factor.Text);

            if (dbFactor < 0)
            {
                txt_Factor.Text = string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_BaseUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_BaseUnit = (DropDownList) ((DropDownList) sender).Parent.Parent.FindControl("ddl_BaseUnit");
            var ddl_ConvUnit = (DropDownList) ((DropDownList) sender).Parent.Parent.FindControl("ddl_ConvUnit");
            var txt_Factor = (TextBox) ((DropDownList) sender).Parent.Parent.FindControl("txt_Factor");

            if (ddl_BaseUnit.SelectedValue == ddl_ConvUnit.SelectedValue)
            {
                txt_Factor.Text = "1";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_ConvUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_BaseUnit = (DropDownList) ((DropDownList) sender).Parent.Parent.FindControl("ddl_BaseUnit");
            var ddl_ConvUnit = (DropDownList) ((DropDownList) sender).Parent.Parent.FindControl("ddl_ConvUnit");
            var txt_Factor = (TextBox) ((DropDownList) sender).Parent.Parent.FindControl("txt_Factor");

            if (ddl_BaseUnit.SelectedValue == ddl_ConvUnit.SelectedValue)
            {
                txt_Factor.Text = "1";
            }
        }

        /// <summary>
        ///     Click button new.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_New_Click(object sender, EventArgs e)
        {
            var drNew = dsUnitConv.Tables[unitConv.TableName].NewRow();

            drNew["FUnitCode"] = DBNull.Value;
            drNew["TUnitCode"] = DBNull.Value;
            drNew["Factor"] = DBNull.Value;
            drNew["CreatedDate"] = ServerDateTime;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["UpdatedDate"] = ServerDateTime;
            drNew["UpdatedBy"] = LoginInfo.LoginName;

            // Add new row
            dsUnitConv.Tables[unitConv.TableName].Rows.Add(drNew);

            // Set Lookup
            SetLookup();

            // Editing on new row
            grd_UnitConv.DataSource = dsUnitConv.Tables[unitConv.TableName];
            grd_UnitConv.EditIndex = grd_UnitConv.Rows.Count;
            grd_UnitConv.DataBind();

            lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();

            btn_New.Enabled = false;

            // Save changed to session
            Session["dsUnitConv"] = dsUnitConv;
        }
    }
}