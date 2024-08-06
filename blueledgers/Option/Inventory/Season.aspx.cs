using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class Season : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.Season season = new Blue.BL.Option.Inventory.Season();
        private DataSet dsSeason = new DataSet();

        private string SeasonMode
        {
            get { return ViewState["SeasonMode"].ToString(); }
            set { ViewState["SeasonMode"] = value; }
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
                dsSeason = (DataSet) Session["dsSeason"];
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsSeason.Clear();

            var getUnit = season.GetList(dsSeason, LoginInfo.ConnStr);

            if (getUnit)
            {
                // Assign Primarykey                
                dsSeason.Tables[season.TableName].PrimaryKey = GetPK();

                Session["dsSeason"] = dsSeason;
            }
            else
            {
                // Display Error Message
                return;
            }

            Page_Setting();
        }

        /// <summary>
        ///     Display Unit Data.
        /// </summary>
        private void Page_Setting()
        {
            //grd_Season.DataSource = dsSeason.Tables[season.TableName];
            //grd_Season.DataBind();

            grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            grd_Season2.EditIndex = -1;
            grd_Season2.DataBind();
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

                    break;
            }
        }

        /// <summary>
        ///     Create New Unit
        /// </summary>
        private void Create()
        {
            //grd_Season.AddNewRow();

            // Add new stkInDt row
            var drNew = dsSeason.Tables[season.TableName].NewRow();

            drNew["SeasonCode"] = string.Empty;

            dsSeason.Tables[season.TableName].Rows.Add(drNew);

            grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            grd_Season2.EditIndex = dsSeason.Tables[season.TableName].Rows.Count - 1;
            grd_Season2.DataBind();

            SeasonMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            //if (grd_Season.Selection.Count > 0)
            //{
            pop_ConfrimDelete.ShowOnPageLoad = true;
            //}
        }

        /// <summary>
        ///     Delete selected Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_Season.GetSelectedFieldValues("SeasonCode");

            //foreach (string delSeasonCode in columnValues)
            //{
            //    foreach (DataRow drDeleting in dsSeason.Tables[season.TableName].Rows)
            //    {
            //        if (drDeleting.RowState != DataRowState.Deleted)
            //        {
            //            if (drDeleting["SeasonCode"].ToString().ToUpper() == delSeasonCode.ToUpper())
            //            {
            //                drDeleting.Delete();
            //            }
            //        }
            //    }
            //}

            for (var i = grd_Season2.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_Season2.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsSeason = (DataSet) Session["dsSeason"];

                    dsSeason.Tables[season.TableName].Rows[i].Delete();
                }
            }

            // Save to database
            var saveSeason = season.Save(dsSeason, LoginInfo.ConnStr);

            if (saveSeason)
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
            //grd_Season.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
            pop_ConfrimDelete.ShowOnPageLoad = false;

            //this.Page_Retrieve();
        }

        /// <summary>
        ///     Print Unit List
        /// </summary>
        private void Print()
        {
        }

        #endregion

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "grd_Season"

        ///// <summary>
        ///// Re-binding Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Season_OnLoad(object sender, EventArgs e)
        //{
        //    grd_Season.DataSource = dsSeason.Tables[season.TableName];
        //    grd_Season.DataBind();
        //}

        ///// <summary>
        ///// Assign Default Value for New Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Season_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    e.NewValues["SeasonCode"]       = string.Empty;
        //    e.NewValues["SeasonName"]       = string.Empty;
        //    e.NewValues["SeasonPercent"]    = 0;
        //    e.NewValues["SeasonType"]       = string.Empty;
        //    e.NewValues["IsActive"]         = true;
        //    e.NewValues["CreatedBy"]        = LoginInfo.LoginName;
        //    e.NewValues["CreatedDate"]      = ServerDateTime;
        //    e.NewValues["UpdatedBy"]        = LoginInfo.LoginName;
        //    e.NewValues["UpdatedDate"]      = ServerDateTime;
        //}

        ///// <summary>
        ///// Create New Unit
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Season_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsSeason.Tables[season.TableName].NewRow();

        //    drInserting["SeasonCode"]       = e.NewValues["SeasonCode"].ToString();
        //    drInserting["SeasonName"]       = e.NewValues["SeasonName"].ToString();
        //    drInserting["SeasonPercent"]    = e.NewValues["SeasonPercent"].ToString();
        //    drInserting["SeasonType"]       = e.NewValues["SeasonType"].ToString();
        //    drInserting["IsActive"]         = e.NewValues["IsActive"].ToString();
        //    drInserting["CreatedBy"]        = LoginInfo.LoginName;
        //    drInserting["CreatedDate"]      = ServerDateTime;
        //    drInserting["UpdatedBy"]        = LoginInfo.LoginName;
        //    drInserting["UpdatedDate"]      = ServerDateTime;

        //    dsSeason.Tables[season.TableName].Rows.Add(drInserting);

        //    // Save to database
        //    bool saveSeason = season.Save(dsSeason, LoginInfo.ConnStr);

        //    if (saveSeason)
        //    {
        //        grd_Season.DataSource = dsSeason.Tables[season.TableName];
        //        grd_Season.CancelEdit();
        //        grd_Season.DataBind();

        //        e.Cancel = true;

        //        Session["dsSeason"] = dsSeason;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsSeason.Tables[season.TableName].RejectChanges();
        //        grd_Season.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        ///// <summary>
        ///// Save Existing Unit Changed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_Season_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    DataRow drUpdating = dsSeason.Tables[season.TableName].Rows[grd_Season.EditingRowVisibleIndex];

        //    drUpdating["SeasonName"]    = e.NewValues["SeasonName"].ToString();
        //    drUpdating["SeasonPercent"] = e.NewValues["SeasonPercent"].ToString();
        //    drUpdating["SeasonType"]    = e.NewValues["SeasonType"].ToString();
        //    drUpdating["IsActive"]      = e.NewValues["IsActive"].ToString();
        //    drUpdating["UpdatedBy"]     = LoginInfo.LoginName;
        //    drUpdating["UpdatedDate"]   = ServerDateTime;

        //    // Save to database
        //    bool saveSeason = season.Save(dsSeason, LoginInfo.ConnStr);

        //    if (saveSeason)
        //    {
        //        grd_Season.DataSource = dsSeason.Tables[season.TableName];
        //        grd_Season.CancelEdit();
        //        grd_Season.DataBind();

        //        e.Cancel = true;

        //        Session["dsSeason"] = dsSeason;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsSeason.Tables[season.TableName].RejectChanges();
        //        grd_Season.CancelEdit();

        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsSeason.Tables[season.TableName].Columns["SeasonCode"];

            return primaryKeys;
        }

        #endregion

        #region "Normal Gridview"

        protected void grd_Season2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Code.
                if (e.Row.FindControl("lbl_Code") != null)
                {
                    var lbl_Code = e.Row.FindControl("lbl_Code") as Label;
                    lbl_Code.Text = DataBinder.Eval(e.Row.DataItem, "SeasonCode").ToString();
                }
                if (e.Row.FindControl("txt_Code") != null)
                {
                    var txt_Code = e.Row.FindControl("txt_Code") as TextBox;
                    txt_Code.Text = DataBinder.Eval(e.Row.DataItem, "SeasonCode").ToString();
                }

                // Season Name.
                if (e.Row.FindControl("lbl_Percent") != null)
                {
                    var lbl_Percent = e.Row.FindControl("lbl_Percent") as Label;
                    lbl_Percent.Text = DataBinder.Eval(e.Row.DataItem, "SeasonPercent").ToString();
                }
                if (e.Row.FindControl("txt_Percent") != null)
                {
                    var txt_Percent = e.Row.FindControl("txt_Percent") as TextBox;
                    txt_Percent.Text = DataBinder.Eval(e.Row.DataItem, "SeasonPercent").ToString();
                }

                // Season Name.
                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SeasonName").ToString();
                }
                if (e.Row.FindControl("txt_Desc") != null)
                {
                    var txt_Desc = e.Row.FindControl("txt_Desc") as TextBox;
                    txt_Desc.Text = DataBinder.Eval(e.Row.DataItem, "SeasonName").ToString();
                }

                // Season Type.
                if (e.Row.FindControl("lbl_Type") != null)
                {
                    var lbl_Type = e.Row.FindControl("lbl_Type") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "SeasonType").ToString() == "IN")
                    {
                        lbl_Type.Text = "Increase";
                    }
                    else if (DataBinder.Eval(e.Row.DataItem, "SeasonType").ToString() == "DE")
                    {
                        lbl_Type.Text = "Decrease";
                    }
                }
                if (e.Row.FindControl("ddl_Type") != null)
                {
                    var ddl_Type = e.Row.FindControl("ddl_Type") as DropDownList;

                    if (DataBinder.Eval(e.Row.DataItem, "SeasonType").ToString() == "IN")
                    {
                        ddl_Type.SelectedItem.Value = "Increase";
                    }
                    else if (DataBinder.Eval(e.Row.DataItem, "SeasonType").ToString() == "DE")
                    {
                        ddl_Type.SelectedItem.Value = "Decrease";
                    }
                }

                // Actived.
                if (e.Row.FindControl("Img_Btn_ChkBox") != null)
                {
                    var Img_Btn_ChkBox = e.Row.FindControl("Img_Btn_ChkBox") as ImageButton;
                    Img_Btn_ChkBox.Enabled = false;

                    if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive")))
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_True.png";
                    }
                    else
                    {
                        Img_Btn_ChkBox.ImageUrl = "~/App_Themes/Default/Images/IN/STDREQ/chk_False.png";
                    }
                }
                if (e.Row.FindControl("chk_Actived") != null)
                {
                    var chk_Actived = e.Row.FindControl("chk_Actived") as CheckBox;
                    if (DataBinder.Eval(e.Row.DataItem, "IsActive") != DBNull.Value)
                    {
                        chk_Actived.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                    }
                    else
                    {
                        chk_Actived.Checked = false;
                    }
                }
            }
        }

        protected void grd_Season2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsSeason = (DataSet) Session["dsSeason"];

            grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            grd_Season2.EditIndex = e.NewEditIndex;
            grd_Season2.DataBind();

            SeasonMode = "EDIT";
        }

        protected void grd_Season2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsSeason = (DataSet) Session["dsSeason"];

            var ddl_Type = grd_Season2.Rows[e.RowIndex].FindControl("ddl_Type") as DropDownList;
            var txt_Code = grd_Season2.Rows[e.RowIndex].FindControl("txt_Code") as TextBox;
            var txt_Desc = grd_Season2.Rows[e.RowIndex].FindControl("txt_Desc") as TextBox;
            var txt_Percent = grd_Season2.Rows[e.RowIndex].FindControl("txt_Percent") as TextBox;
            var chk_Actived = grd_Season2.Rows[e.RowIndex].FindControl("chk_Actived") as CheckBox;

            if (string.IsNullOrEmpty(txt_Code.Text))
            {
                lbl_Warning.Text = "Season Code can not be blank.";
                pop_WarningDelete.ShowOnPageLoad = true;

                return;
            }
            if (dsSeason.Tables[season.TableName].Select(string.Format("SeasonCode='{0}'", txt_Code.Text)).Count() > 0)
            {
                lbl_Warning.Text = "Season Code is exists.";
                pop_WarningDelete.ShowOnPageLoad = true;

                return;
            }


            if (SeasonMode == "NEW")
            {
                var drNew = dsSeason.Tables[season.TableName].Rows[grd_Season2.EditIndex];

                drNew["SeasonCode"] = txt_Code.Text;
                drNew["SeasonName"] = txt_Desc.Text;
                drNew["SeasonPercent"] = decimal.Parse(txt_Percent.Text == string.Empty ? "0" : txt_Code.Text);
                drNew["SeasonType"] = ddl_Type.SelectedItem.Value;
                drNew["IsActive"] = chk_Actived.Checked;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;

                //int countTypeCode = Season.Get_CountByCodeType(drNew["AdjType"].ToString(), drNew["AdjCode"].ToString(), LoginInfo.ConnStr);

                //if (countTypeCode > 0)
                //{
                //    lbl_Warning.Text = "Can not to Insert data.";
                //    pop_Warning.ShowOnPageLoad = true;
                //    return;
                //}
                //else
                //{
                var save = season.Save(dsSeason, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Season2.DataSource = dsSeason.Tables[season.TableName];
                    grd_Season2.EditIndex = -1;
                    grd_Season2.DataBind();
                }

                Page_Retrieve();
                //}
            }
            else
            {
                var drUpdating = dsSeason.Tables[season.TableName].Rows[e.RowIndex];

                drUpdating["SeasonCode"] = txt_Code.Text;
                drUpdating["SeasonName"] = txt_Desc.Text;
                drUpdating["SeasonPercent"] = decimal.Parse(txt_Percent.Text == string.Empty ? "0" : txt_Code.Text);
                drUpdating["SeasonType"] = ddl_Type.SelectedItem.Value;
                drUpdating["IsActive"] = chk_Actived.Checked;
                drUpdating["UpdatedBy"] = LoginInfo.LoginName;
                drUpdating["UpdatedDate"] = ServerDateTime;

                var save = season.Save(dsSeason, LoginInfo.ConnStr);

                if (save)
                {
                    grd_Season2.DataSource = dsSeason.Tables[season.TableName];
                    grd_Season2.EditIndex = -1;
                    grd_Season2.DataBind();
                }
            }

            grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            grd_Season2.EditIndex = -1;
            grd_Season2.DataBind();

            SeasonMode = string.Empty;
        }

        protected void grd_Season2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsSeason = (DataSet) Session["dsSeason"];

            if (SeasonMode == "NEW")
            {
                dsSeason.Tables[season.TableName].Rows[dsSeason.Tables[season.TableName].Rows.Count - 1].Delete();
            }

            if (SeasonMode == "EDIT")
            {
                dsSeason.Tables[season.TableName].Rows[dsSeason.Tables[season.TableName].Rows.Count - 1].CancelEdit();
            }

            grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            grd_Season2.EditIndex = -1;
            grd_Season2.DataBind();

            SeasonMode = string.Empty;
        }

        #endregion

        protected void grd_Season2_Load(object sender, EventArgs e)
        {
            //grd_Season2.DataSource = dsSeason.Tables[season.TableName];
            //grd_Season2.DataBind();
        }
    }
}