using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option
{
    public partial class ExRate : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private DataSet dsExRate = new DataSet();

        private string ExRateMode
        {
            get { return ViewState["ExRateMode"].ToString(); }
            set { ViewState["ExRateMode"] = value; }
        }

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsExRate = (DataSet)Session["dsExRate"];
            }
        }

        /// <summary>
        ///     Get Exchange Rate Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsExRate.Clear();

            var getExRate = exRate.GetList(dsExRate, LoginInfo.ConnStr);

            if (getExRate)
            {
                // Assign Primarykey                
                dsExRate.Tables[exRate.TableName].PrimaryKey = GetPK();

                Session["dsExRate"] = dsExRate;
            }
            else
            {
                // Display Error Message
                return;
            }

            Page_Setting();
        }

        /// <summary>
        ///     Display Exchange Rate Data.
        /// </summary>
        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            //grd_ExRate.DataSource = dsExRate.Tables[exRate.TableName];
            //grd_ExRate.DataBind();

            grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
            grd_ExRate2.DataBind();
        }

        /// <summary>
        ///     Define statement for create/delete/print
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;

                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        /// <summary>
        ///     Create New Exchange Rate
        /// </summary>
        private void Create()
        {
            //grd_ExRate.AddNewRow();

            // Normal Grid View
            var drNew = dsExRate.Tables[exRate.TableName].NewRow();

            drNew["ExRateID"] = exRate.GetNewID(LoginInfo.ConnStr);
            drNew["EffDate"] = DateTime.Now;
            drNew["TCurrencyCode"] = string.Empty;
            drNew["FCurrencyCode"] = string.Empty;

            dsExRate.Tables[exRate.TableName].Rows.Add(drNew);

            grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
            grd_ExRate2.EditIndex = dsExRate.Tables[exRate.TableName].Rows.Count - 1;
            grd_ExRate2.DataBind();

            // Disable
            menu_CmdBar.Items[0].Enabled = false; // Create
            menu_CmdBar.Items[1].Enabled = false; // Delete

            ExRateMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Exchange Rate
        /// </summary>
        private void Delete()
        {
            //if (grd_ExRate.Selection.Count > 0)
            //{
            pop_ConfrimDelete.ShowOnPageLoad = true;
            //}
        }

        /// <summary>
        ///     Delete selected Exchange Rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_ExRate.GetSelectedFieldValues("ExRateID");

            //foreach (int delExRateID in columnValues)
            //{
            //    foreach (DataRow drDeleting in dsExRate.Tables[exRate.TableName].Rows)
            //    {
            //        if (drDeleting.RowState != DataRowState.Deleted)
            //        {
            //            if (int.Parse(drDeleting["ExRateID"].ToString()) == delExRateID)
            //            {
            //                drDeleting.Delete();
            //            }
            //        }
            //    }
            //}

            for (var i = grd_ExRate2.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_ExRate2.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsExRate = (DataSet)Session["dsExRate"];

                    dsExRate.Tables[exRate.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveExRate = exRate.Save(dsExRate, LoginInfo.ConnStr);

            if (saveExRate)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                Page_Retrieve();
            }
        }

        /// <summary>
        ///     Canceling Delete Delivery Point and Deselect all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_ExRate.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Print Exchange Rate List
        /// </summary>
        //private void Print()
        //{
        //    var objArrList = new ArrayList();

        //    var sb = new StringBuilder();

        //    for (var i = 0; i < dsExRate.Tables[exRate.TableName].Rows.Count; i++)
        //    {
        //        var ListID = dsExRate.Tables[exRate.TableName].Rows[i]["ExRateID"].ToString();
        //        sb.Append("" + ListID + ",");
        //    }
        //    if (sb.Length > 0)
        //    {
        //        objArrList.Add(sb.ToString().Substring(0, sb.Length - 1));
        //    }
        //    else
        //    {
        //        objArrList.Add("''");
        //    }

        //    Session["s_arrNo"] = objArrList;
        //    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=221" + "&BuCode=" +
        //                     LoginInfo.BuInfo.BuCode;
        //    ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //        "<script>window.open('" + reportLink + "','_blank')</script>");
        //}

        #endregion

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "grd_ExRate"

        //protected void grd_ExRate_OnLoad(object sender, EventArgs e)
        //{
        //    grd_ExRate.DataSource = dsExRate.Tables[exRate.TableName];
        //    grd_ExRate.DataBind();
        //}
        //protected void grd_ExRate_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    e.NewValues["ExRateID"] = exRate.GetNewID(LoginInfo.ConnStr);
        //    e.NewValues["EffDate"]  = ServerDateTime;            
        //}
        //protected void grd_ExRate_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsExRate.Tables[exRate.TableName].NewRow();
        //    drInserting["ExRateID"]         = exRate.GetNewID(LoginInfo.ConnStr);
        //    drInserting["EffDate"]          = DateTime.Parse(e.NewValues["EffDate"].ToString());
        //    drInserting["FCurrencyCode"]    = e.NewValues["FCurrencyCode"].ToString();
        //    drInserting["TCurrencyCode"]    = e.NewValues["TCurrencyCode"].ToString();
        //    drInserting["BuyingExRate"]     = e.NewValues["BuyingExRate"].ToString();
        //    drInserting["SellingExRate"]    = e.NewValues["SellingExRate"].ToString();
        //    drInserting["FixingExRate"]     = e.NewValues["FixingExRate"].ToString();
        //    dsExRate.Tables[exRate.TableName].Rows.Add(drInserting);
        //    // Save to database
        //    bool saveExRate = exRate.Save(dsExRate, LoginInfo.ConnStr);
        //    if (saveExRate)
        //    {
        //        grd_ExRate.DataSource = dsExRate.Tables[exRate.TableName];
        //        grd_ExRate.CancelEdit();
        //        grd_ExRate.DataBind();
        //        e.Cancel = true;
        //        Session["dsExRate"] = dsExRate;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsExRate.Tables[exRate.TableName].RejectChanges();
        //        grd_ExRate.CancelEdit();
        //        e.Cancel = true;
        //    }
        //}
        /// <summary>
        ///     Re-binding Exchange rate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Assign Default Value for New Exchange Rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Create New Exchange Rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Save Existing Exchange Rate Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_ExRate_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    DataRow drUpdating = dsExRate.Tables[exRate.TableName].Rows[grd_ExRate.EditingRowVisibleIndex];
        //    drUpdating["EffDate"]       = DateTime.Parse(e.NewValues["EffDate"].ToString());
        //    drUpdating["FCurrencyCode"] = e.NewValues["FCurrencyCode"].ToString();
        //    drUpdating["TCurrencyCode"] = e.NewValues["TCurrencyCode"].ToString();
        //    drUpdating["BuyingExRate"]  = e.NewValues["BuyingExRate"].ToString();
        //    drUpdating["SellingExRate"] = e.NewValues["SellingExRate"].ToString();
        //    drUpdating["FixingExRate"]  = e.NewValues["FixingExRate"].ToString();
        //    // Save to database
        //    bool saveExRate = exRate.Save(dsExRate, LoginInfo.ConnStr);
        //    if (saveExRate)
        //    {
        //        grd_ExRate.DataSource = dsExRate.Tables[exRate.TableName];
        //        grd_ExRate.CancelEdit();
        //        grd_ExRate.DataBind();
        //        e.Cancel = true;
        //        Session["dsExRate"] = dsExRate;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsExRate.Tables[exRate.TableName].RejectChanges();
        //        grd_ExRate.CancelEdit();
        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region "Misc"
        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsExRate.Tables[exRate.TableName].Columns["ExRateID"];

            return primaryKeys;
        }

        #endregion

        protected void grd_ExRate2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var FilterTextBox = e.Row.FindControl("FilteredTextBoxExtender") as FilteredTextBoxExtender;
                var FilterTextBox1 = e.Row.FindControl("FilteredTextBoxExtender1") as FilteredTextBoxExtender;
                var FilterTextBox2 = e.Row.FindControl("FilteredTextBoxExtender2") as FilteredTextBoxExtender;

                // Effective Date.
                if (e.Row.FindControl("lbl_Date") != null)
                {
                    var lbl_Date = e.Row.FindControl("lbl_Date") as Label;
                    lbl_Date.Text = DataBinder.Eval(e.Row.DataItem, "EffDate").ToString();
                }
                if (e.Row.FindControl("dte_Date") != null)
                {
                    var dte_Date = e.Row.FindControl("dte_Date") as ASPxDateEdit;
                    dte_Date.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "EffDate").ToString());
                }

                // From Currency
                if (e.Row.FindControl("ddl_From") != null)
                {
                    var ddl_From = e.Row.FindControl("ddl_From") as ASPxComboBox;
                    ddl_From.Value = DataBinder.Eval(e.Row.DataItem, "FCurrencyCode").ToString();
                }
                if (e.Row.FindControl("lbl_From") != null)
                {
                    var lbl_From = e.Row.FindControl("lbl_From") as Label;
                    lbl_From.Text = DataBinder.Eval(e.Row.DataItem, "FCurrencyCode").ToString();
                }

                // To Currency
                if (e.Row.FindControl("lbl_To") != null)
                {
                    var lbl_To = e.Row.FindControl("lbl_To") as Label;
                    lbl_To.Text = DataBinder.Eval(e.Row.DataItem, "TCurrencyCode").ToString();
                }
                if (e.Row.FindControl("ddl_To") != null)
                {
                    var ddl_To = e.Row.FindControl("ddl_To") as ASPxComboBox;
                    ddl_To.Value = DataBinder.Eval(e.Row.DataItem, "TCurrencyCode").ToString();
                }

                // Buying ExRate
                if (e.Row.FindControl("lbl_Buying") != null)
                {
                    var lbl_Buying = e.Row.FindControl("lbl_Buying") as Label;
                    lbl_Buying.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "BuyingExRate"));
                }
                if (e.Row.FindControl("txt_Buying") != null)
                {
                    var txt_Buying = e.Row.FindControl("txt_Buying") as TextBox;
                    txt_Buying.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "BuyingExRate"));
                }

                // Selling ExRate
                if (e.Row.FindControl("lbl_Selling") != null)
                {
                    var lbl_Selling = e.Row.FindControl("lbl_Selling") as Label;
                    lbl_Selling.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "SellingExRate"));
                }
                if (e.Row.FindControl("txt_Selling") != null)
                {
                    var txt_Selling = e.Row.FindControl("txt_Selling") as TextBox;
                    txt_Selling.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "SellingExRate"));
                }

                // Fixing ExRate
                if (e.Row.FindControl("lbl_Management") != null)
                {
                    var lbl_Management = e.Row.FindControl("lbl_Management") as Label;
                    lbl_Management.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "FixingExRate"));
                }
                if (e.Row.FindControl("txt_Management") != null)
                {
                    var txt_Management = e.Row.FindControl("txt_Management") as TextBox;
                    txt_Management.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "FixingExRate"));
                }
            }
        }

        protected void grd_ExRate2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsExRate = (DataSet)Session["dsExRate"];

            if (ExRateMode == "NEW")
            {
                dsExRate.Tables[exRate.TableName].Rows[dsExRate.Tables[exRate.TableName].Rows.Count - 1].Delete();
            }

            if (ExRateMode == "EDIT")
            {
                dsExRate.Tables[exRate.TableName].Rows[dsExRate.Tables[exRate.TableName].Rows.Count - 1].CancelEdit();
            }

            grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
            grd_ExRate2.EditIndex = -1;
            grd_ExRate2.DataBind();

            // Disable
            menu_CmdBar.Items[0].Enabled = true; // Create
            menu_CmdBar.Items[1].Enabled = true; // Delete

            ExRateMode = string.Empty;
        }

        protected void grd_ExRate2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsExRate = (DataSet)Session["dsExRate"];

            grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
            grd_ExRate2.EditIndex = e.NewEditIndex;
            grd_ExRate2.DataBind();

            // Disable
            menu_CmdBar.Items[0].Enabled = false; // Create
            menu_CmdBar.Items[1].Enabled = false; // Delete

            ExRateMode = "EDIT";
        }

        protected void grd_ExRate2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsExRate = (DataSet)Session["dsExRate"];

            var ddl_From = grd_ExRate2.Rows[e.RowIndex].FindControl("ddl_From") as ASPxComboBox;
            var ddl_To = grd_ExRate2.Rows[e.RowIndex].FindControl("ddl_To") as ASPxComboBox;
            var txt_Buying = grd_ExRate2.Rows[e.RowIndex].FindControl("txt_Buying") as TextBox;
            var txt_Selling = grd_ExRate2.Rows[e.RowIndex].FindControl("txt_Selling") as TextBox;
            var txt_Management = grd_ExRate2.Rows[e.RowIndex].FindControl("txt_Management") as TextBox;
            var dte_Date = grd_ExRate2.Rows[e.RowIndex].FindControl("dte_Date") as ASPxDateEdit;

            if (ExRateMode == "NEW")
            {
                var drNew = dsExRate.Tables[exRate.TableName].Rows[grd_ExRate2.EditIndex];

                drNew["EffDate"] = DateTime.Parse(dte_Date.Date.ToShortDateString());
                drNew["FCurrencyCode"] = ddl_From.Value.ToString();
                drNew["TCurrencyCode"] = ddl_To.Value.ToString();
                drNew["BuyingExRate"] = txt_Buying.Text;
                drNew["SellingExRate"] = txt_Selling.Text;
                drNew["FixingExRate"] = txt_Management.Text;

                //int countTypeCode = adjType.Get_CountByCodeType(drNew["AdjType"].ToString(), drNew["AdjCode"].ToString(), LoginInfo.ConnStr);

                //if (countTypeCode > 0)
                //{
                //    lbl_Warning.Text = "Can not to Insert data.";
                //    pop_Warning.ShowOnPageLoad = true;
                //    return;
                //}
                //else
                //{
                var save = exRate.Save(dsExRate, LoginInfo.ConnStr);

                if (save)
                {
                    grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
                    grd_ExRate2.EditIndex = -1;
                    grd_ExRate2.DataBind();
                }

                Page_Retrieve();
                //}
            }
            else
            {
                var drUpdating = dsExRate.Tables[exRate.TableName].Rows[e.RowIndex];

                drUpdating["EffDate"] = DateTime.Parse(dte_Date.Date.ToShortDateString());
                drUpdating["FCurrencyCode"] = ddl_From.Value.ToString();
                drUpdating["TCurrencyCode"] = ddl_To.Value.ToString();
                drUpdating["BuyingExRate"] = txt_Buying.Text;
                drUpdating["SellingExRate"] = txt_Selling.Text;
                drUpdating["FixingExRate"] = txt_Management.Text;

                var save = exRate.Save(dsExRate, LoginInfo.ConnStr);

                if (save)
                {
                    grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
                    grd_ExRate2.EditIndex = -1;
                    grd_ExRate2.DataBind();
                }
            }

            grd_ExRate2.DataSource = dsExRate.Tables[exRate.TableName];
            grd_ExRate2.EditIndex = -1;
            grd_ExRate2.DataBind();

            // Disable
            menu_CmdBar.Items[0].Enabled = true; // Create
            menu_CmdBar.Items[1].Enabled = true; // Delete

            ExRateMode = string.Empty;
        }
    }
}