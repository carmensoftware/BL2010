using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ApprLv : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.ApprLv appLv = new Blue.BL.Option.Inventory.ApprLv();
        private DataSet dsApprLv = new DataSet();

        private string ApprLvMode
        {
            get { return Session["ApprLvMode"].ToString(); }
            set { Session["ApprLvMode"] = value; }
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
                dsApprLv = (DataSet) Session["dsApprLv"];
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            dsApprLv.Clear();

            var getApprLv = appLv.GetListApprLv(dsApprLv, LoginInfo.ConnStr);

            if (getApprLv)
            {
                // Assign Primarykey                
                dsApprLv.Tables[appLv.TableName].PrimaryKey = GetPK();

                Session["dsApprLv"] = dsApprLv;
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
            grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
            grd_ApprLv.DataBind();
        }

        /// <summary>
        ///     Define statement for create/delete/print
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
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
            //grd_ApprLv.AddNewRow();
            var drNew = dsApprLv.Tables[appLv.TableName].NewRow();
            drNew["ApprLvCode"] = string.Empty;
            drNew["Name"] = string.Empty;
            drNew["IsActived"] = true;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["CreatedDate"] = ServerDateTime;
            drNew["UpdatedBy"] = LoginInfo.LoginName;
            drNew["UpdatedDate"] = ServerDateTime;
            dsApprLv.Tables[appLv.TableName].Rows.Add(drNew);

            grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
            grd_ApprLv.EditIndex = dsApprLv.Tables[appLv.TableName].Rows.Count - 1;
            grd_ApprLv.DataBind();

            ApprLvMode = "NEW";
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            var selection = new List<object>();

            for (var i = 0; i < grd_ApprLv.Rows.Count; i++)
            {
                var chk_Item = grd_ApprLv.Rows[i].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    selection.Add(dsApprLv.Tables[appLv.TableName].Rows[i]["ApprLvCode"]);
                }
            }

            //if (grd_ApprLv.Selection.Count > 0)
            if (selection.Count > 0)
            {
                pop_ConfrimDelete.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     Delete selected Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_ApprLv.GetSelectedFieldValues("ApprLvCode");
            var columnValues = new List<object>();

            for (var i = 0; i < grd_ApprLv.Rows.Count; i++)
            {
                var chk_Item = grd_ApprLv.Rows[i].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    columnValues.Add(dsApprLv.Tables[appLv.TableName].Rows[i]["ApprLvCode"]);
                }
            }

            foreach (string delApprLvCode in columnValues)
            {
                foreach (DataRow drDeleting in dsApprLv.Tables[appLv.TableName].Rows)
                {
                    if (drDeleting.RowState != DataRowState.Deleted)
                    {
                        if (drDeleting["ApprLvCode"].ToString().ToUpper() == delApprLvCode.ToUpper())
                        {
                            drDeleting.Delete();
                        }
                    }
                }
            }

            // Save to database
            var saveApprLv = appLv.Save(dsApprLv, LoginInfo.ConnStr);

            if (saveApprLv)
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
            //grd_ApprLv.Selection.UnselectAll();
            for (var i = 0; i < grd_ApprLv.Rows.Count; i++)
            {
                var chk_Item = grd_ApprLv.Rows[i].FindControl("chk_Item") as CheckBox;
                chk_Item.Checked = false;
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
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

        #region "grd_ApprLv"

        /// <summary>
        ///     Re-binding Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ApprLv_OnLoad(object sender, EventArgs e)
        {
        }

        protected void grd_ApprLv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_ApprLvCode") != null)
                {
                    var lbl_ApprLvCode = e.Row.FindControl("lbl_ApprLvCode") as Label;
                    lbl_ApprLvCode.Text = DataBinder.Eval(e.Row.DataItem, "ApprLvCode").ToString();
                }

                if (e.Row.FindControl("txt_ApprLvCode") != null)
                {
                    var txt_ApprLvCode = e.Row.FindControl("txt_ApprLvCode") as TextBox;
                    txt_ApprLvCode.Text = DataBinder.Eval(e.Row.DataItem, "ApprLvCode").ToString();
                }

                if (e.Row.FindControl("lbl_Name") != null)
                {
                    var lbl_Name = e.Row.FindControl("lbl_Name") as Label;
                    lbl_Name.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }

                if (e.Row.FindControl("txt_Name") != null)
                {
                    var txt_Name = e.Row.FindControl("txt_Name") as TextBox;
                    txt_Name.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                }

                if (e.Row.FindControl("chk_Actived") != null)
                {
                    var chk_Actived = e.Row.FindControl("chk_Actived") as CheckBox;
                    chk_Actived.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActived"));
                }
            }
        }

        protected void grd_ApprLv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
            grd_ApprLv.EditIndex = e.NewEditIndex;
            grd_ApprLv.DataBind();

            ApprLvMode = "EDIT";
        }

        protected void grd_ApprLv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var txt_ApprLvCode = grd_ApprLv.Rows[grd_ApprLv.EditIndex].FindControl("txt_ApprLvCode") as TextBox;
            var txt_Name = grd_ApprLv.Rows[grd_ApprLv.EditIndex].FindControl("txt_Name") as TextBox;
            var chk_Actived = grd_ApprLv.Rows[grd_ApprLv.EditIndex].FindControl("chk_Actived") as CheckBox;

            var drUpdating = dsApprLv.Tables[appLv.TableName].Rows[grd_ApprLv.Rows[grd_ApprLv.EditIndex].DataItemIndex];
            drUpdating["ApprLvCode"] = txt_ApprLvCode.Text;
            drUpdating["Name"] = txt_Name.Text;
            drUpdating["IsActived"] = chk_Actived.Checked;

            // Save to database
            var saveUnit = appLv.Save(dsApprLv, LoginInfo.ConnStr);

            if (saveUnit)
            {
                grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
                grd_ApprLv.EditIndex = -1;
                grd_ApprLv.DataBind();

                e.Cancel = true;

                Session["dsApprLv"] = dsApprLv;
            }
            else
            {
                // Display Error Message    
                //dsApprLv.Tables[appLv.TableName].RejectChanges();
                dsApprLv.Tables[appLv.TableName].Rows[dsApprLv.Tables[appLv.TableName].Rows.Count - 1].Delete();
                grd_ApprLv.EditIndex = -1;

                e.Cancel = true;
            }

            //grd_ApprLv.DataSource   = dsApprLv.Tables[appLv.TableName];
            //grd_ApprLv.EditIndex    = -1;
            //grd_ApprLv.DataBind();

            ApprLvMode = string.Empty;
        }

        protected void grd_ApprLv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (ApprLvMode == "NEW")
            {
                dsApprLv.Tables[appLv.TableName].Rows[dsApprLv.Tables[appLv.TableName].Rows.Count - 1].Delete();
            }

            if (ApprLvMode == "EDIT")
            {
                dsApprLv.Tables[appLv.TableName].Rows[dsApprLv.Tables[appLv.TableName].Rows.Count - 1].CancelEdit();
            }

            grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
            grd_ApprLv.EditIndex = -1;
            grd_ApprLv.DataBind();

            ApprLvMode = string.Empty;
        }

        //protected void grd_ApprLv_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    e.NewValues["ApprLvCode"]     = string.Empty;
        //    e.NewValues["Name"]         = string.Empty;
        //    e.NewValues["IsActived"]    = true;
        //    e.NewValues["CreatedBy"]    = LoginInfo.LoginName;
        //    e.NewValues["CreatedDate"]  = ServerDateTime;
        //    e.NewValues["UpdatedBy"]    = LoginInfo.LoginName;
        //    e.NewValues["UpdatedDate"]  = ServerDateTime;
        //}
        //protected void grd_ApprLv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsApprLv.Tables[appLv.TableName].NewRow();
        //    drInserting["ApprLvCode"] = e.NewValues["ApprLvCode"].ToString();
        //    drInserting["Name"]         = e.NewValues["Name"].ToString();
        //    drInserting["IsActived"]    = e.NewValues["IsActived"].ToString();
        //    drInserting["CreatedBy"]    = LoginInfo.LoginName;
        //    drInserting["CreatedDate"]  = ServerDateTime;
        //    drInserting["UpdatedBy"]    = LoginInfo.LoginName;
        //    drInserting["UpdatedDate"]  = ServerDateTime;
        //    dsApprLv.Tables[appLv.TableName].Rows.Add(drInserting);
        //    // Save to database
        //    bool saveUnit = appLv.Save(dsApprLv, LoginInfo.ConnStr);
        //    if (saveUnit)
        //    {
        //        grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
        //        //grd_ApprLv.CancelEdit();
        //        grd_ApprLv.EditIndex = -1;
        //        grd_ApprLv.DataBind();
        //        e.Cancel = true;
        //        Session["dsApprLv"] = dsApprLv;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsApprLv.Tables[appLv.TableName].RejectChanges();
        //        //grd_ApprLv.CancelEdit();
        //        grd_ApprLv.EditIndex = -1;
        //        e.Cancel = true;
        //    }
        //}
        /// <summary>
        ///     Assign Default Value for New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Create New Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Save Existing Unit Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_ApprLv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    DataRow drUpdating = dsApprLv.Tables[appLv.TableName].Rows[grd_ApprLv.EditingRowVisibleIndex];
        //    drUpdating["Name"]          = e.NewValues["Name"].ToString();
        //    drUpdating["IsActived"]     = e.NewValues["IsActived"].ToString();
        //    drUpdating["UpdatedBy"]     = LoginInfo.LoginName;
        //    drUpdating["UpdatedDate"]   = ServerDateTime;
        //    // Save to database
        //    bool saveUnit = appLv.Save(dsApprLv, LoginInfo.ConnStr);
        //    if (saveUnit)
        //    {
        //        grd_ApprLv.DataSource = dsApprLv.Tables[appLv.TableName];
        //        grd_ApprLv.CancelEdit();
        //        grd_ApprLv.DataBind();
        //        e.Cancel = true;
        //        Session["dsApprLv"] = dsApprLv;
        //    }
        //    else
        //    {
        //        // Display Error Message    
        //        dsApprLv.Tables[appLv.TableName].RejectChanges();
        //        grd_ApprLv.CancelEdit();
        //        e.Cancel = true;
        //    }
        //}

        #endregion

        #region "Misc"
        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsApprLv.Tables[appLv.TableName].Columns["ApprLvCode"];

            return primaryKeys;
        }

        #endregion
    }
}