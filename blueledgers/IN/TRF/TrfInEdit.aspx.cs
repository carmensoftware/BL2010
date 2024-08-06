using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfInEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.TransferIn trfIn = new Blue.BL.IN.TransferIn();
        private readonly Blue.BL.IN.TransferInDt trfInDt = new Blue.BL.IN.TransferInDt();
        private DataSet dsTrfInEdit = new DataSet();
        private decimal total;

        private string TrfInEditMode
        {
            get { return Session["TrfInEditMode"].ToString(); }
            set { Session["TrfInEditMode"] = value; }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTrfInEdit = (DataSet) Session["dsTrfInEdit"];
            }
        }

        private void Page_Retrieve()
        {
            trfIn.GetList(dsTrfInEdit, Request.Params["ID"], LoginInfo.ConnStr);
            trfInDt.GetList(dsTrfInEdit, Request.Params["ID"], LoginInfo.ConnStr);

            Session["dsTrfInEdit"] = dsTrfInEdit;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTrfInEdit = dsTrfInEdit.Tables[trfIn.TableName].Rows[0];

            //Show Info.
            lbl_Date.Text = DateTime.Parse(drTrfInEdit["CreateDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drTrfInEdit["RefId"].ToString();
            lbl_Status.Text = drTrfInEdit["Status"].ToString();
            lbl_CommitDate.Text = drTrfInEdit["CommitDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drTrfInEdit["CommitDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_FromLocationCode.Text = drTrfInEdit["FromStoreId"].ToString();
            lbl_FromLocationName.Text = drTrfInEdit["FromStoreId"] + " : " +
                                        locat.GetName(drTrfInEdit["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/20125----locat.GetName2(drTrfInEdit["FromStoreId"].ToString(), LoginInfo.ConnStr);
            lbl_ToLocationCode.Text = drTrfInEdit["ToStoreId"].ToString();
            lbl_ToLocationName.Text = drTrfInEdit["ToStoreId"] + " : " +
                                      locat.GetName(drTrfInEdit["ToStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfInEdit["ToStoreId"].ToString(), LoginInfo.ConnStr);
            txt_Desc.Text = drTrfInEdit["Description"].ToString();

            grd_TrfInEdit.DataSource = dsTrfInEdit.Tables[trfInDt.TableName];
            grd_TrfInEdit.DataBind();
        }

        protected void grd_TrfInEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.FindControl("tp_Information") != null)
                //{
                //    ASPxPageControl tp_Information = e.Row.FindControl("tp_Information") as ASPxPageControl;

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr) +
                                           " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("txt_QtyTrIn") != null)
                {
                    //ASPxTextBox txt_QtyTrIn = e.Row.FindControl("txt_QtyTrIn") as ASPxTextBox;
                    var txt_QtyTrIn = e.Row.FindControl("txt_QtyTrIn") as TextBox;
                    txt_QtyTrIn.Text = DataBinder.Eval(e.Row.DataItem, "QtyIn").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("ddl_Debit") != null)
                {
                    var ddl_Debit = e.Row.FindControl("ddl_Debit") as ASPxComboBox;
                    ddl_Debit.Value = DataBinder.Eval(e.Row.DataItem, "DebitAcc").ToString();
                }

                if (e.Row.FindControl("ddl_Credit") != null)
                {
                    var ddl_Credit = e.Row.FindControl("ddl_Credit") as ASPxComboBox;
                    ddl_Credit.Value = DataBinder.Eval(e.Row.DataItem, "CreditAcc").ToString();
                }
                //}

                //if (e.Row.FindControl("lbl_ProductCode") != null)
                //{
                //    Label lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                //    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " +product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) + " : " +
                //        product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr); ;
                //    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                //}

                //if (e.Row.FindControl("lbl_EnglishName") != null)
                //{
                //    Label lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                //    lbl_EnglishName.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) + " : " +
                //        product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                //    lbl_EnglishName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " + lbl_EnglishName.Text;

                //}

                //if (e.Row.FindControl("lbl_LocalName") != null)
                //{
                //    Label lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                //    lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                //}

                if (e.Row.FindControl("lbl_QtyTransfer") != null)
                {
                    var lbl_QtyTransfer = e.Row.FindControl("lbl_QtyTransfer") as Label;
                    lbl_QtyTransfer.Text = DataBinder.Eval(e.Row.DataItem, "QtyOut").ToString();
                    lbl_QtyTransfer.ToolTip = lbl_QtyTransfer.Text;
                    total += decimal.Parse(lbl_QtyTransfer.Text);
                }

                if (e.Row.FindControl("lbl_QtyTrIn") != null)
                {
                    var lbl_QtyTrIn = e.Row.FindControl("lbl_QtyTrIn") as Label;
                    lbl_QtyTrIn.Text = DataBinder.Eval(e.Row.DataItem, "QtyIn").ToString();
                    lbl_QtyTrIn.ToolTip = lbl_QtyTrIn.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }
            }

            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if (e.Row.FindControl("lbl_Total") != null)
            //    {
            //        Label lbl_Total = e.Row.FindControl("lbl_Total") as Label;
            //        lbl_Total.Text = String.Format("{0:N}", total);
            //    }
            //}
        }

        protected void grd_TrfInEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //ASPxPageControl tp_Information = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var lbl_ProductCode = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("lbl_ProductCode") as Label;
            var ddl_Debit = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddl_Credit = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            //ASPxTextBox txt_QtyTrIn = tp_Information.FindControl("txt_QtyTrIn") as ASPxTextBox;
            var txt_QtyTrIn = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("txt_QtyTrIn") as TextBox;
            var txt_Comment = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("txt_Comment") as TextBox;
            var lbl_Unit = grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].FindControl("lbl_Unit") as Label;
            //Label lbl_ProductCode           = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_ProductCode") as Label;
            //Label lbl_EnglishName           = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_EnglishName") as Label;
            //Label lbl_LocalName             = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_LocalName") as Label;
            //Label lbl_QtyTrOut              = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_QtyTrOut") as Label;
            //Label lbl_Unit                  = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_Unit") as Label;

            //Check field cannot empty

            //if (lbl_ProductCode != null)
            //{
            //    if (lbl_ProductCode.Value == null)
            //    {
            //        lbl_Warning.Text = "'SKU #' cannot empty";
            //        pop_Warning.ShowOnPageLoad = true;
            //        return;
            //    }
            //}

            //if (ddl_Debit != null)
            //{
            //    if (ddl_Debit.Value == null)
            //    {
            //        lbl_Warning.Text = "'Debit A/C' cannot empty";
            //        pop_Warning.ShowOnPageLoad = true;
            //        return;
            //    }
            //}

            //if (ddl_Credit != null)
            //{
            //    if (ddl_Credit.Value == null)
            //    {
            //        lbl_Warning.Text = "'Credit A/C' cannot empty";
            //        pop_Warning.ShowOnPageLoad = true;
            //        return;
            //    }
            //}

            if (txt_QtyTrIn != null)
            {
                if (txt_QtyTrIn.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Qty Requested' cannot empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var drTrfInEdit =
                dsTrfInEdit.Tables[trfInDt.TableName].Rows[grd_TrfInEdit.Rows[grd_TrfInEdit.EditIndex].DataItemIndex];
            drTrfInEdit["QtyIn"] = decimal.Parse(txt_QtyTrIn.Text);
            drTrfInEdit["Unit"] = lbl_Unit.Text;
            //drTrfOutEdit["DebitAcc"]  = ddl_Debit.Value;
            //drTrfOutEdit["CreditAcc"] = ddl_Credit.Value;
            drTrfInEdit["Comment"] = txt_Comment.Text;
            //drTrfInEdit["ProductCode"] = lbl_ProductCode.Value;

            //if (Request.Params["MODE"].ToUpper() == "EDIT" && TrfOutEditMode.ToUpper == "NEW")
            //{
            //    drTrfOutEdit["RefId"] = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0]["RefId"].ToString();
            //}

            grd_TrfInEdit.DataSource = dsTrfInEdit.Tables[trfInDt.TableName];
            grd_TrfInEdit.EditIndex = -1;
            grd_TrfInEdit.DataBind();

            btn_Save.Enabled = true;
            btn_Commit.Enabled = true;
            TrfInEditMode = string.Empty;
        }

        protected void grd_TrfInEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (TrfInEditMode.ToUpper() == "NEW")
            {
                dsTrfInEdit.Tables[trfInDt.TableName].Rows[dsTrfInEdit.Tables[trfInDt.TableName].Rows.Count - 1].Delete();
            }

            if (TrfInEditMode.ToUpper() == "EDIT")
            {
                dsTrfInEdit.Tables[trfInDt.TableName].Rows[dsTrfInEdit.Tables[trfInDt.TableName].Rows.Count - 1]
                    .CancelEdit();
            }

            grd_TrfInEdit.DataSource = dsTrfInEdit.Tables[trfInDt.TableName];
            grd_TrfInEdit.EditIndex = -1;
            grd_TrfInEdit.DataBind();

            btn_Save.Enabled = true;
            btn_Commit.Enabled = true;
            TrfInEditMode = string.Empty;
        }

        protected void grd_TrfInEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            btn_Save.Enabled = false;
            btn_Commit.Enabled = false;
            grd_TrfInEdit.DataSource = dsTrfInEdit.Tables[trfInDt.TableName];
            grd_TrfInEdit.EditIndex = e.NewEditIndex;
            grd_TrfInEdit.DataBind();

            TrfInEditMode = "Edit";
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            var drCommit = dsTrfInEdit.Tables[trfIn.TableName].Rows[0];
            drCommit["Status"] = "Committed";
            drCommit["Description"] = txt_Desc.Text;
            drCommit["CommitDate"] = ServerDateTime.Date;
            drCommit["UpdateDate"] = ServerDateTime.Date;
            drCommit["UpdateBy"] = LoginInfo.LoginName;

            var Save = trfIn.Save(dsTrfInEdit, LoginInfo.ConnStr);

            if (Save)
            {
                //Update Inventory
                inv.GetStructure(dsTrfInEdit, LoginInfo.ConnStr);

                foreach (DataRow drTrfInDt in dsTrfInEdit.Tables[trfInDt.TableName].Rows)
                {
                    var drInv = dsTrfInEdit.Tables[inv.TableName].NewRow();
                    drInv["HdrNo"] = drTrfInDt["RefId"].ToString();
                    drInv["DtNo"] = drTrfInDt["Id"].ToString();
                    drInv["InvNo"] = drTrfInDt["Id"].ToString();
                    drInv["ProductCode"] = drTrfInDt["ProductCode"].ToString();
                    drInv["Location"] = drCommit["ToStoreId"].ToString();
                    drInv["IN"] = drTrfInDt["QtyIn"];
                    drInv["OUT"] = 0;
                    drInv["Amount"] = inv.GetLastAvg(drCommit["ToStoreId"].ToString(),
                        drTrfInDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["CommittedDate"] = ServerDateTime.Date;
                    drInv["Type"] = "TI";

                    dsTrfInEdit.Tables[inv.TableName].Rows.Add(drInv);
                }

                inv.Save(dsTrfInEdit, LoginInfo.ConnStr);

                Response.Redirect("TrfInDt.aspx?ID=" + Request.Params["ID"]);
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        #region "Editors"

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfInDt.aspx?ID=" + Request.Params["ID"]);
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var drSave = dsTrfInEdit.Tables[trfIn.TableName].Rows[0];
            drSave["Description"] = txt_Desc.Text;
            drSave["UpdateBy"] = LoginInfo.LoginName;
            drSave["UpdateDate"] = ServerDateTime.Date;

            var Save = trfIn.Save(dsTrfInEdit, LoginInfo.ConnStr);

            if (Save)
            {
                Response.Redirect("TrfInDt.aspx?ID=" + Request.Params["ID"]);
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        protected void btn_ConfrimSave_Click(object sender, EventArgs e)
        {
            //if (Request.Params["MODE"].ToUpper() == "NEW")
            //{
            //    DataRow drInserting         = dsStoreReqEdit.Tables[storeReq.TableName].NewRow();
            //    drInserting["RequestCode"]  = null;
            //    drInserting["LocationCode"] = ddl_Store.Value;
            //    drInserting["Description"]  = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
            //    drInserting["DeliveryDate"] = de_DeliveryDate.Date;
            //    drInserting["Status"]       = true;
            //    drInserting["WFStep"]       = DBNull.Value;
            //    drInserting["ApprStatus"]   = DBNull.Value;
            //    drInserting["DocStatus"]    = "In Process";
            //    drInserting["CreateBy"]     = LoginInfo.LoginName;
            //    drInserting["CreateDate"]   = ServerDateTime.Date;
            //    drInserting["UpdateBy"]     = LoginInfo.LoginName;
            //    drInserting["UpdateDate"]   = ServerDateTime.Date;

            //    dsStoreReqEdit.Tables[storeReq.TableName].Rows.Add(drInserting);
            //}
            //else
            //{
            //    DataRow drSave          = dsStoreReqEdit.Tables[storeReq.TableName].Rows[0];
            //    drSave["RequestCode"]   = null;
            //    drSave["LocationCode"]  = ddl_Store.Value;
            //    drSave["Description"]   = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
            //    drSave["DeliveryDate"]  = de_DeliveryDate.Date;
            //    drSave["Status"]        = true; ;
            //    drSave["WFStep"]        = DBNull.Value;
            //    drSave["ApprStatus"]    = DBNull.Value;
            //    drSave["DocStatus"]     = "In Process";
            //    drSave["UpdateBy"]      = LoginInfo.LoginName;
            //    drSave["UpdateDate"]    = ServerDateTime.Date;
            //}

            //bool save = storeReq.Save(dsStoreReqEdit, LoginInfo.ConnStr);

            //if (save)
            //{
            //    if (Request.Params["MODE"].ToUpper() == "NEW" || Request.Params["MODE"].ToUpper() == "SR")
            //    {
            //        foreach (DataRow drStoreReqDt in dsStoreReqEdit.Tables[storeReqDt.TableName].Rows)
            //        {
            //            drStoreReqDt["DocumentId"] = storeReq.GetLastID(LoginInfo.ConnStr);
            //        }
            //    }

            //    bool saveStoreReqDt = storeReqDt.Save(dsStoreReqEdit, LoginInfo.ConnStr);

            //    if (saveStoreReqDt)
            //    {
            //        pop_ConfrimSave.ShowOnPageLoad = false;
            //        Response.Redirect("StoreReqDt.aspx?ID=" + dsStoreReqEdit.Tables[storeReqDt.TableName].Rows[0]["DocumentId"].ToString());
            //    }
            //}
        }

        protected void btn_ComfiremDelete_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < grd_TrfInEdit.Rows.Count; i++)
            {
                var chk_Item = grd_TrfInEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drDelete = dsTrfInEdit.Tables[trfInDt.TableName].Rows[i];

                    if (drDelete.RowState != DataRowState.Deleted)
                    {
                        drDelete.Delete();
                    }
                }
            }

            grd_TrfInEdit.DataSource = dsTrfInEdit.Tables[trfInDt.TableName];
            grd_TrfInEdit.EditIndex = -1;
            grd_TrfInEdit.DataBind();

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        #endregion
    }
}