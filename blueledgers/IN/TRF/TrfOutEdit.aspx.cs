using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfOutEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.TransferIn trfIn = new Blue.BL.IN.TransferIn();
        private readonly Blue.BL.IN.TransferInDt trfInDt = new Blue.BL.IN.TransferInDt();
        private readonly Blue.BL.IN.TransferOut trfOut = new Blue.BL.IN.TransferOut();
        private readonly Blue.BL.IN.TransferOutDt trfOutDt = new Blue.BL.IN.TransferOutDt();
        private DataSet dsTrfOutEdit = new DataSet();
        private decimal total;

        private string TrfOutEditMode
        {
            get { return Session["TrfOutEditMode"].ToString(); }
            set { Session["TrfOutEditMode"] = value; }
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
                dsTrfOutEdit = (DataSet) Session["dsTrfOutEdit"];
            }
        }

        private void Page_Retrieve()
        {
            trfOut.GetList(dsTrfOutEdit, Request.Params["ID"], LoginInfo.ConnStr);
            trfOutDt.GetList(dsTrfOutEdit, Request.Params["ID"], LoginInfo.ConnStr);

            Session["dsTrfOutEdit"] = dsTrfOutEdit;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTrfOutEdit = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0];

            //Show Info.
            lbl_Date.Text = DateTime.Parse(drTrfOutEdit["CreateDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drTrfOutEdit["RefId"].ToString();
            lbl_Status.Text = drTrfOutEdit["Status"].ToString();
            lbl_CommitDate.Text = drTrfOutEdit["CommitDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drTrfOutEdit["CommitDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_FromLocationCode.Text = drTrfOutEdit["FromStoreId"].ToString();
            lbl_FromLocationName.Text = drTrfOutEdit["FromStoreId"] + " : " +
                                        locat.GetName(drTrfOutEdit["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfOutEdit["FromStoreId"].ToString(), LoginInfo.ConnStr);
            de_DeliveryDate.Date = DateTime.Parse(drTrfOutEdit["DeliveryDate"].ToString());
            lbl_ToLocationCode.Text = drTrfOutEdit["ToStoreId"].ToString();
            lbl_ToLocationName.Text = drTrfOutEdit["ToStoreId"] + " : " +
                                      locat.GetName(drTrfOutEdit["ToStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfOutEdit["ToStoreId"].ToString(), LoginInfo.ConnStr);
            txt_Desc.Text = drTrfOutEdit["Description"].ToString();

            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.DataBind();
        }

        protected void grd_TrfOutEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (TrfOutEditMode.ToUpper() == "NEW")
            {
                dsTrfOutEdit.Tables[trfOutDt.TableName].Rows[dsTrfOutEdit.Tables[trfOutDt.TableName].Rows.Count - 1]
                    .Delete();
            }

            if (TrfOutEditMode.ToUpper() == "EDIT")
            {
                dsTrfOutEdit.Tables[trfOutDt.TableName].Rows[dsTrfOutEdit.Tables[trfOutDt.TableName].Rows.Count - 1]
                    .CancelEdit();
            }

            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.EditIndex = -1;
            grd_TrfOutEdit.DataBind();

            btn_Save.Enabled = true;
            btn_Commit.Enabled = true;
            TrfOutEditMode = string.Empty;
        }

        protected void grd_TrfOutEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.FindControl("tp_Information") != null)
                //{
                //ASPxPageControl tp_Information = e.Row.FindControl("tp_Information") as ASPxPageControl;

                if (e.Row.FindControl("ddl_ProductCode") != null)
                {
                    var ddl_ProductCode = e.Row.FindControl("ddl_ProductCode") as ASPxComboBox;
                    ddl_ProductCode.DataSource = product.GetLookUp_LocationCode(lbl_ToLocationCode.Text,
                        LoginInfo.ConnStr);
                    ddl_ProductCode.ValueField = "ProductCode";
                    ddl_ProductCode.DataBind();
                    ddl_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode");
                }

                if (e.Row.FindControl("txt_QtyTrOut") != null)
                {
                    //ASPxTextBox txt_QtyTrOut = e.Row.FindControl("txt_QtyTrOut") as ASPxTextBox;
                    var txt_QtyTrOut = e.Row.FindControl("txt_QtyTrOut") as TextBox;
                    txt_QtyTrOut.Text = DataBinder.Eval(e.Row.DataItem, "QtyOut").ToString();
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

                //if (e.Row.FindControl("ddl_Debit") != null)
                //{
                //    ASPxComboBox ddl_Debit  = e.Row.FindControl("ddl_Debit") as ASPxComboBox;
                //    ddl_Debit.Value         = DataBinder.Eval(e.Row.DataItem, "DebitAcc").ToString();
                //}

                //if (e.Row.FindControl("ddl_Credit") != null)
                //{
                //    ASPxComboBox ddl_Credit = e.Row.FindControl("ddl_Credit") as ASPxComboBox;
                //    ddl_Credit.Value        = DataBinder.Eval(e.Row.DataItem, "CreditAcc").ToString();
                //}
                //}

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr) + " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr);
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                //if (e.Row.FindControl("lbl_EnglishName") != null)
                //{
                //    Label lbl_EnglishName   = e.Row.FindControl("lbl_EnglishName") as Label;
                //    lbl_EnglishName.Text    = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) + " : " +
                //        product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                //    lbl_EnglishName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " + lbl_EnglishName.Text;
                //}

                //if (e.Row.FindControl("lbl_LocalName") != null)
                //{
                //    Label lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                //    lbl_LocalName.Text  = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                //}

                if (e.Row.FindControl("lbl_QtyAllocated") != null)
                {
                    var lbl_QtyAllocated = e.Row.FindControl("lbl_QtyAllocated") as Label;
                    lbl_QtyAllocated.Text = DataBinder.Eval(e.Row.DataItem, "QtyAllocate").ToString();
                    lbl_QtyAllocated.ToolTip = lbl_QtyAllocated.Text;
                    total += decimal.Parse(lbl_QtyAllocated.Text == string.Empty ? "0" : lbl_QtyAllocated.Text);
                }

                if (e.Row.FindControl("lbl_QtyTrOut") != null)
                {
                    var lbl_QtyTrOut = e.Row.FindControl("lbl_QtyTrOut") as Label;
                    lbl_QtyTrOut.Text = DataBinder.Eval(e.Row.DataItem, "QtyOut").ToString();
                    lbl_QtyTrOut.ToolTip = lbl_QtyTrOut.Text;
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

        protected void grd_TrfOutEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            btn_Save.Enabled = false;
            btn_Commit.Enabled = false;
            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.EditIndex = e.NewEditIndex;
            grd_TrfOutEdit.DataBind();

            TrfOutEditMode = "Edit";
        }

        protected void grd_TrfOutEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //ASPxPageControl tp_Information  = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var ddl_ProductCode =
                grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;
            var ddl_Debit = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddl_Credit = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            //ASPxTextBox txt_QtyTrOut        = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex]..FindControl("txt_QtyTrOut") as ASPxTextBox;
            var txt_QtyTrOut = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("txt_QtyTrOut") as TextBox;
            var txt_Comment = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("txt_Comment") as TextBox;
            var lbl_Unit = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_Unit") as Label;
            //Label lbl_ProductCode           = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_ProductCode") as Label;
            //Label lbl_EnglishName           = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_EnglishName") as Label;
            //Label lbl_LocalName             = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_LocalName") as Label;
            //Label lbl_QtyTrOut              = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_QtyTrOut") as Label;
            //Label lbl_Unit                  = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_Unit") as Label;

            //Check field cannot empty

            if (ddl_ProductCode != null)
            {
                if (ddl_ProductCode.Value == null)
                {
                    lbl_Warning.Text = "'SKU #' cannot empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

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

            if (txt_QtyTrOut != null)
            {
                if (txt_QtyTrOut.Text == string.Empty)
                {
                    lbl_Warning.Text = "'Qty Requested' cannot empty";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var drTrfOutEdit =
                dsTrfOutEdit.Tables[trfOutDt.TableName].Rows[grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].DataItemIndex
                    ];
            drTrfOutEdit["QtyOut"] = decimal.Parse(txt_QtyTrOut.Text);
            drTrfOutEdit["Unit"] = lbl_Unit.Text;
            //drTrfOutEdit["DebitAcc"]  = ddl_Debit.Value;
            //drTrfOutEdit["CreditAcc"] = ddl_Credit.Value;
            drTrfOutEdit["Comment"] = txt_Comment.Text;
            drTrfOutEdit["ProductCode"] = ddl_ProductCode.Value;

            //if (Request.Params["MODE"].ToUpper() == "EDIT" && TrfOutEditMode.ToUpper == "NEW")
            //{
            //    drTrfOutEdit["RefId"] = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0]["RefId"].ToString();
            //}

            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.EditIndex = -1;
            grd_TrfOutEdit.DataBind();

            btn_Save.Enabled = true;
            btn_Commit.Enabled = true;
            TrfOutEditMode = string.Empty;
        }

        protected void ddl_ProductCode_Load(object sender, EventArgs e)
        {
            var ddl_ProductCode = sender as ASPxComboBox;
            //ASPxPageControl tp_Information = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("tp_Information") as ASPxPageControl;

            if (ddl_ProductCode != null)
            {
                //ASPxComboBox ddl_ProductCode    = tp_Information.FindControl("ddl_ProductCode") as ASPxComboBox;
                ddl_ProductCode.DataSource = product.GetLookUp_LocationCode(lbl_ToLocationCode.Text, LoginInfo.ConnStr);
                ddl_ProductCode.ValueField = "ProductCode";
                ddl_ProductCode.DataBind();
            }
        }

        protected void ddl_ProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tp_Information =
                grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var ddl_ProductCode =
                grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;
            var lbl_Unit = grd_TrfOutEdit.Rows[grd_TrfOutEdit.EditIndex].FindControl("lbl_Unit") as Label;

            if (lbl_Unit != null)
            {
                lbl_Unit.Text = product.GetInvenUnit(ddl_ProductCode.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            var drCommit = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0];
            drCommit["Description"] = txt_Desc.Text;
            drCommit["Status"] = "Committed";
            drCommit["CommitDate"] = ServerDateTime.Date;
            drCommit["UpdateDate"] = ServerDateTime.Date;
            drCommit["UpdateBy"] = LoginInfo.LoginName;

            var Save = trfOut.Save(dsTrfOutEdit, LoginInfo.ConnStr);

            if (Save)
            {
                //Update Inventory
                inv.GetStructure(dsTrfOutEdit, LoginInfo.ConnStr);

                foreach (DataRow drTrfOutDt in dsTrfOutEdit.Tables[trfOutDt.TableName].Rows)
                {
                    var drInv = dsTrfOutEdit.Tables[inv.TableName].NewRow();
                    drInv["HdrNo"] = drTrfOutDt["RefId"].ToString();
                    drInv["DtNo"] = drTrfOutDt["Id"].ToString();
                    drInv["InvNo"] = drTrfOutDt["Id"].ToString();
                    drInv["ProductCode"] = drTrfOutDt["ProductCode"].ToString();
                    drInv["Location"] = drCommit["FromStoreId"].ToString();
                    drInv["IN"] = 0;
                    drInv["OUT"] = drTrfOutDt["QtyOut"];
                    drInv["Amount"] = inv.GetLastAvg(drCommit["FromStoreId"].ToString(),
                        drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["CommittedDate"] = ServerDateTime.Date;
                    drInv["Type"] = "TO";

                    dsTrfOutEdit.Tables[inv.TableName].Rows.Add(drInv);
                }

                inv.Save(dsTrfOutEdit, LoginInfo.ConnStr);


                //Generate Transfer In Document
                var dsTrfIn = new DataSet();

                trfIn.GetStructure(dsTrfIn, LoginInfo.ConnStr);
                trfInDt.GetStructure(dsTrfIn, LoginInfo.ConnStr);

                var drTrfInNew = dsTrfIn.Tables[trfIn.TableName].NewRow();
                drTrfInNew["RefId"] = trfIn.GetNewId(LoginInfo.ConnStr);
                drTrfInNew["FromStoreId"] = lbl_FromLocationCode.Text;
                drTrfInNew["ToStoreId"] = lbl_ToLocationCode.Text;
                drTrfInNew["Status"] = "Saved";
                drTrfInNew["CreateBy"] = LoginInfo.LoginName;
                drTrfInNew["CreateDate"] = ServerDateTime.Date;
                drTrfInNew["UpdateBy"] = LoginInfo.LoginName;
                drTrfInNew["UpdateDate"] = ServerDateTime.Date;
                dsTrfIn.Tables[trfIn.TableName].Rows.Add(drTrfInNew);

                foreach (DataRow drTrfOutDt in dsTrfOutEdit.Tables[trfOutDt.TableName].Rows)
                {
                    var drTrfInDtNew = dsTrfIn.Tables[trfInDt.TableName].NewRow();
                    drTrfInDtNew["RefId"] = drTrfInNew["RefId"];
                    drTrfInDtNew["ProductCode"] = drTrfOutDt["ProductCode"];
                    drTrfInDtNew["QtyOut"] = drTrfOutDt["QtyOut"];
                    drTrfInDtNew["QtyIn"] = drTrfOutDt["QtyOut"];
                    //drTrfInDtNew["DebitAcc"]  = DBNull.Value;
                    //drTrfInDtNew["CreditAcc"] = DBNull.Value;
                    drTrfInDtNew["Unit"] = drTrfOutDt["Unit"];
                    drTrfInDtNew["SRId"] = drTrfOutDt["SRId"];
                    drTrfInDtNew["TrfOutId"] = drTrfOutDt["RefId"];
                    drTrfInDtNew["TrfOutIdDt"] = drTrfOutDt["Id"];
                    dsTrfIn.Tables[trfInDt.TableName].Rows.Add(drTrfInDtNew);
                }

                var TrfInSave = trfIn.Save(dsTrfIn, LoginInfo.ConnStr);

                if (TrfInSave)
                {
                    Response.Redirect("TrfOutDt.aspx?ID=" + Request.Params["ID"]);
                }
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
            Response.Redirect("TrfOutDt.aspx?ID=" + Request.Params["ID"]);
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            btn_Save.Enabled = false;
            btn_Commit.Enabled = false;
            trfOutDt.GetStructure(dsTrfOutEdit, LoginInfo.ConnStr);

            var drNew = dsTrfOutEdit.Tables[trfOutDt.TableName].NewRow();
            drNew["RefId"] = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0]["RefId"].ToString();
            dsTrfOutEdit.Tables[trfOutDt.TableName].Rows.Add(drNew);

            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.EditIndex = dsTrfOutEdit.Tables[trfOutDt.TableName].Rows.Count - 1;
            grd_TrfOutEdit.DataBind();

            TrfOutEditMode = "New";
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var drSave = dsTrfOutEdit.Tables[trfOut.TableName].Rows[0];
            drSave["DeliveryDate"] = de_DeliveryDate.Date;
            drSave["Description"] = txt_Desc.Text;
            drSave["UpdateBy"] = LoginInfo.LoginName;
            drSave["UpdateDate"] = ServerDateTime.Date;

            var Save = trfOut.Save(dsTrfOutEdit, LoginInfo.ConnStr);

            if (Save)
            {
                Response.Redirect("TrfOutDt.aspx?ID=" + Request.Params["ID"]);
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        //protected void btn_ConfrimSave_Click(object sender, EventArgs e)
        //{

        //}

        protected void btn_ComfiremDelete_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < grd_TrfOutEdit.Rows.Count; i++)
            {
                var chk_Item = grd_TrfOutEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drDelete = dsTrfOutEdit.Tables[trfOutDt.TableName].Rows[i];

                    if (drDelete.RowState != DataRowState.Deleted)
                    {
                        drDelete.Delete();
                    }
                }
            }

            grd_TrfOutEdit.DataSource = dsTrfOutEdit.Tables[trfOutDt.TableName];
            grd_TrfOutEdit.EditIndex = -1;
            grd_TrfOutEdit.DataBind();

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        #endregion
    }
}