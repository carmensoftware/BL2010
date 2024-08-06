using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;

namespace BlueLedger.PL.IN.MM
{
    public partial class MoveMentEdit : BasePage
    {
        #region "Attributes"

        private string MoveMentId = string.Empty;
        private Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private DataSet dsMMEdit = new DataSet();
        private Blue.BL.GnxLib gnx = new Blue.BL.GnxLib();
        private Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private Blue.BL.IN.Movement moveMent = new Blue.BL.IN.Movement();
        private Blue.BL.IN.MovementDt moveMentDt = new Blue.BL.IN.MovementDt();
        private Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private decimal totalQty = 0;

        private string MMEditMode
        {
            get { return Session["MMEditMode"].ToString(); }
            set { Session["MMEditMode"] = value; }
        }

        private string MODE
        {
            get { return Request.QueryString["MODE"].ToString(); }
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
                dsMMEdit = (DataSet) Session["dsMMEdit"];
            }
        }

        private void Page_Retrieve()
        {
            dsMMEdit.Clear();

            if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            {
                var get = moveMent.GetStructure(dsMMEdit, LoginInfo.ConnStr);

                if (!get)
                {
                    return;
                }

                var getDt = moveMentDt.GetSchema(dsMMEdit, LoginInfo.ConnStr);

                if (!getDt)
                {
                    return;
                }
            }
            else
            {
                MoveMentId = Request.Params["ID"].ToString();

                var get = moveMent.GetList(dsMMEdit, MoveMentId, LoginInfo.ConnStr);

                if (!get)
                {
                    return;
                }

                var getDt = moveMentDt.GetList(dsMMEdit, MoveMentId, LoginInfo.ConnStr);

                if (!getDt)
                {
                    return;
                }
            }

            Session["dsMMEdit"] = dsMMEdit;

            Page_Setting();
        }

        private void Page_Setting()
        {
            if (MODE.ToUpper() == "NEW")
            {
                dsMMEdit.Clear();

                // Visible Commit button.
                menu_CmdBar.Items[1].Visible = false;

                //td_Commit.Visible = false;
                tr_HLable.Visible = false;
                //tr_DLabel.Visible = false;

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                    Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    lbl_FromStore_HD.Visible = false;
                    lbl_ToStore_HD.Visible = false;
                    tr_HLable.Visible = false;
                    //td_FStoreHead.Visible = false;
                    //td_FStoreName.Visible = false;
                    //tr_ToStore.Visible      = false;
                    //tr_ToStore_item.Visible = false;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TI")
                {
                    ddl_Type.Visible = false;
                    lbl_Type_HD.Visible = false;
                    //lbl_Type_H.Visible = false;
                    //lbl_DeliveryDate_H.Visible = false;
                    lbl_DeliveryDate_HD.Visible = false;
                    dte_DeliDate.Visible = false;
                }
                else
                {
                    ddl_Type.Visible = false;
                    lbl_Type_HD.Visible = false;
                }
            }
            else
            {
                var drMoveMent = dsMMEdit.Tables[moveMent.TableName].Rows[0];

                lbl_CreatedDate.Text = DateTime.Parse(drMoveMent["CreatedDate"].ToString()).ToString("dd/MM/yyyy");
                lbl_Ref.Text = drMoveMent["RefId"].ToString();
                lbl_Status.Text = drMoveMent["Status"].ToString();
                lbl_CommittedDate.Text = drMoveMent["CommittedDate"] == DBNull.Value
                    ? string.Empty
                    : DateTime.Parse(drMoveMent["CommittedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                ddl_Type.Text = adjType.GetName(drMoveMent["TypeCode"].ToString(), LoginInfo.ConnStr);
                txt_Desc.Text = drMoveMent["Description"].ToString();
                lbl_ToStore.Text = drMoveMent["ToStoreId"].ToString();
                lbl_ToStoreName.Text = strLct.GetName(drMoveMent["ToStoreId"].ToString(), LoginInfo.ConnStr);
                lbl_FromStore.Text = drMoveMent["FromStoreId"].ToString();
                lbl_FromStoreName.Text = strLct.GetName(drMoveMent["FromStoreId"].ToString(), LoginInfo.ConnStr);

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                    Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    lbl_FromStore_HD.Visible = false;
                    lbl_ToStore_HD.Visible = false;
                    tr_HLable.Visible = false;
                    //td_FStoreHead.Visible = false;
                    //td_FStoreName.Visible = false;
                    //tr_ToStore.Visible = false;
                    //tr_ToStore_item.Visible = false;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TI")
                {
                    ddl_Type.Visible = false;
                    lbl_Type_HD.Visible = false;
                    lbl_DeliveryDate_HD.Visible = false;
                    dte_DeliDate.Visible = false;
                }
                else
                {
                    ddl_Type.Visible = false;
                    lbl_Type_HD.Visible = false;

                    dte_DeliDate.Date = DateTime.Parse(drMoveMent["DeliveryDate"].ToString());
                }
            }

            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = -1;
            grd_MovementDt.DataBind();
        }

        protected void Save()
        {
            if (ddl_Type.Value == null)
            {
                lbl_Warning.Text = "Please select 'Type Code'.";
                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                pop_ConfirmSave.ShowOnPageLoad = true;
            }
        }

        protected void Back()
        {
            if (MODE.ToUpper() == "NEW")
            {
                Response.Redirect("MoveMentLst.aspx");
            }
            else
            {
                Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                  "&ID=" + Request.Params["ID"].ToString() +
                                  "&VID=" + Request.Params["VID"].ToString());
            }
        }

        protected void Commit()
        {
            var drCommit = dsMMEdit.Tables[moveMent.TableName].Rows[0];
            drCommit["Description"] = txt_Desc.Text;
            drCommit["Status"] = "Committed";
            drCommit["CommittedDate"] = ServerDateTime.Date;
            drCommit["UpdatedDate"] = ServerDateTime.Date;
            drCommit["UpdatedBy"] = LoginInfo.LoginName;

            var Save = moveMent.Save(dsMMEdit, LoginInfo.ConnStr);

            if (Save)
            {
                //Update Inventory
                inv.GetStructure(dsMMEdit, LoginInfo.ConnStr);

                //Get StartDate and EndDate for update Avg in inventory
                var startDate = period.GetStartDate(ServerDateTime.Date, LoginInfo.ConnStr);
                var endDate = period.GetEndDate(ServerDateTime.Date, LoginInfo.ConnStr);

                foreach (DataRow drTrfOutDt in dsMMEdit.Tables[moveMentDt.TableName].Rows)
                {
                    var drInv = dsMMEdit.Tables[inv.TableName].NewRow();
                    drInv["HdrNo"] = drTrfOutDt["RefId"].ToString();
                    drInv["DtNo"] = drTrfOutDt["DtId"].ToString();
                    drInv["InvNo"] = drTrfOutDt["DtId"].ToString();
                    drInv["ProductCode"] = drTrfOutDt["ProductCode"].ToString();

                    if (Request.Params["Prefix"].ToUpper() == "TO")
                    {
                        drInv["Location"] = drCommit["FromStoreId"].ToString();
                        drInv["IN"] = 0;
                        drInv["OUT"] = drTrfOutDt["Qty"];
                        drInv["Type"] = "TO";
                        drInv["Amount"] = inv.GetLastAvg(drCommit["FromStoreId"].ToString(),
                            drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    }
                    else if (Request.Params["Prefix"].ToUpper() == "TI")
                    {
                        drInv["Location"] = drCommit["FromStoreId"].ToString();
                        drInv["IN"] = drTrfOutDt["Qty"];
                        drInv["OUT"] = 0;
                        drInv["Type"] = "TI";
                        drInv["Amount"] = inv.GetLastAvg(drCommit["FromStoreId"].ToString(),
                            drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    }
                    else if (Request.Params["Prefix"].ToUpper() == "SO")
                    {
                        drInv["IN"] = 0;
                        drInv["OUT"] = drTrfOutDt["Qty"];
                        drInv["Type"] = "SO";
                        drInv["Location"] = drTrfOutDt["LocationCode"];
                        drInv["Amount"] = inv.GetLastAvg(drTrfOutDt["LocationCode"].ToString(),
                            drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    }
                    else
                    {
                        drInv["IN"] = drTrfOutDt["Qty"];
                        drInv["OUT"] = 0;
                        drInv["Type"] = "SI";
                        drInv["Location"] = drTrfOutDt["LocationCode"];
                        drInv["Amount"] = inv.GetLastAvg(drTrfOutDt["LocationCode"].ToString(),
                            drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    }

                    drInv["CommittedDate"] = ServerDateTime.Date;

                    dsMMEdit.Tables[inv.TableName].Rows.Add(drInv);
                }

                inv.Save(dsMMEdit, LoginInfo.ConnStr);

                //Update amount
                foreach (DataRow drInv in dsMMEdit.Tables[inv.TableName].Rows)
                {
                    drInv["PAvgAudit"] = inv.SetPAvgAudit(startDate, endDate, drInv["Location"].ToString(),
                        drInv["ProductCode"].ToString(), LoginInfo.ConnStr);
                }

                inv.Save(dsMMEdit, LoginInfo.ConnStr);

                if (Request.Params["Prefix"].ToUpper() == "TO")
                {
                    //Generate Transfer In Document
                    var dsTrfIn = new DataSet();

                    moveMent.GetStructure(dsTrfIn, LoginInfo.ConnStr);
                    moveMentDt.GetSchema(dsTrfIn, LoginInfo.ConnStr);

                    var drTrfInNew = dsTrfIn.Tables[moveMent.TableName].NewRow();
                    drTrfInNew["RefId"] = moveMent.GetNewID("TI", LoginInfo.ConnStr);
                    drTrfInNew["FromStoreId"] = lbl_FromStore.Text;
                    drTrfInNew["ToStoreId"] = lbl_ToStore.Text;
                    drTrfInNew["Status"] = "Saved";
                    drTrfInNew["CreatedBy"] = LoginInfo.LoginName;
                    drTrfInNew["CreatedDate"] = ServerDateTime.Date;
                    drTrfInNew["UpdatedBy"] = LoginInfo.LoginName;
                    drTrfInNew["UpdatedDate"] = ServerDateTime.Date;
                    drTrfInNew["TypeCode"] = string.Empty;
                    dsTrfIn.Tables[moveMent.TableName].Rows.Add(drTrfInNew);

                    foreach (DataRow drTrfOutDt in dsMMEdit.Tables[moveMentDt.TableName].Rows)
                    {
                        var drTrfInDtNew = dsTrfIn.Tables[moveMentDt.TableName].NewRow();
                        drTrfInDtNew["RefId"] = drTrfInNew["RefId"];
                        drTrfInDtNew["ProductCode"] = drTrfOutDt["ProductCode"];
                        drTrfInDtNew["Qty"] = drTrfOutDt["Qty"];
                        //drTrfInDtNew["QtyIn"] = drTrfOutDt["QtyOut"];
                        //drTrfInDtNew["DebitAcc"]  = DBNull.Value;
                        //drTrfInDtNew["CreditAcc"] = DBNull.Value;
                        drTrfInDtNew["Unit"] = drTrfOutDt["Unit"];
                        drTrfInDtNew["SRId"] = drTrfOutDt["SRId"];
                        drTrfInDtNew["TrfOutId"] = drTrfOutDt["RefId"];
                        drTrfInDtNew["TrfOutDtId"] = drTrfOutDt["DtId"];
                        dsTrfIn.Tables[moveMentDt.TableName].Rows.Add(drTrfInDtNew);
                    }

                    moveMent.Save(dsTrfIn, LoginInfo.ConnStr);
                }

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=SI&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=SO&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TI")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=TI&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TO")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=TO&VID=" + Request.Params["VID"]);
                }
            }
        }

        protected void ddl_Type_Load(object sender, EventArgs e)
        {
            // Fix ไปก่อน ไม่รู้ว่าจะทำยังไง
            if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
            {
                ddl_Type.DataSource = adjType.GetList("Stock In", LoginInfo.ConnStr);
            }
            else
            {
                ddl_Type.DataSource = adjType.GetList("Stock Out", LoginInfo.ConnStr);
            }

            ddl_Type.DataBind();
        }

        // Pop Up
        protected void btn_Confirm_Yes_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;

            //Check StockIn Detail.It must have data before save.
            if (grd_MovementDt.Rows.Count == 0)
            {
                lbl_Warning.Text = "Please clicks Create button to add item.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }
            else if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            {
                var drSave = dsMMEdit.Tables[moveMent.TableName].NewRow();

                if (MODE.ToUpper() == "NEW")
                {
                    drSave["RefId"] = moveMent.GetNewID(Request.Params["Prefix"].ToUpper().ToString(), LoginInfo.ConnStr);

                    foreach (DataRow drSaveDt in dsMMEdit.Tables[moveMentDt.TableName].Rows)
                    {
                        if (drSaveDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drSaveDt["RefId"] = drSave["RefId"].ToString();
                    }
                }

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                    Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    drSave["TypeCode"] = ddl_Type.Value;
                    drSave["Status"] = "Saved";
                    drSave["Description"] = txt_Desc.Text.ToString();
                    drSave["CreatedDate"] = ServerDateTime;
                    drSave["CreatedBy"] = LoginInfo.LoginName;
                    drSave["UpdatedDate"] = ServerDateTime;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TO")
                {
                }

                dsMMEdit.Tables[moveMent.TableName].Rows.Add(drSave);
            }
            else
            {
                var drSave = dsMMEdit.Tables[moveMent.TableName].Rows[0];
                drSave["Description"] = txt_Desc.Text;

                if (Request.Params["Prefix"].ToUpper() == "TO")
                {
                    drSave["DeliveryDate"] = dte_DeliDate.Date;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                         Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    drSave["TypeCode"] = ddl_Type.Value;
                }

                drSave["Status"] = "Saved";
                drSave["UpdatedBy"] = LoginInfo.LoginName;
                drSave["UpdatedDate"] = ServerDateTime.Date;
            }

            var save = moveMent.Save(dsMMEdit, LoginInfo.ConnStr);

            if (save)
            {
                if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=SI&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=SO&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TI")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=TI&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TO")
                {
                    Response.Redirect("MoveMentDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&ID=" + dsMMEdit.Tables[moveMent.TableName].Rows[0]["RefId"].ToString()
                                      + "&Prefix=TO&VID=" + Request.Params["VID"]);
                }
            }
        }

        protected void btn_Confirm_No_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
            return;
        }

        //Grid View
        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var ddl_Store = sender as ASPxComboBox;

            ddl_Store.DataSource = strLct.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);
            //ddl_Store.ValueField="
            ddl_Store.DataBind();
        }

        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tp_Information =
                grd_MovementDt.Rows[grd_MovementDt.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var ddl_Product = tp_Information.FindControl("ddl_Product") as ASPxComboBox;
            var ddl_Store = tp_Information.FindControl("ddl_Store") as ASPxComboBox;

            if (ddl_Product != null)
            {
                ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(), LoginInfo.ConnStr);
                ddl_Product.ValueField = "ProductCode";
                ddl_Product.DataBind();
            }
        }

        protected void ddl_Product_Load(object sender, EventArgs e)
        {
            var ddl_ProductCode = sender as ASPxComboBox;
            var ddl_Store = sender as ASPxComboBox;

            //ddl_ProductCode.Value = string.Empty;

            if (ddl_ProductCode != null && ddl_Store != null && ddl_Store.Value != null)
            {
                ddl_ProductCode.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(),
                    LoginInfo.ConnStr);
                ddl_ProductCode.DataBind();
            }
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tp_Information =
                grd_MovementDt.Rows[grd_MovementDt.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var ddl_Product = tp_Information.FindControl("ddl_Product") as ASPxComboBox;
            var lbl_Unit = tp_Information.FindControl("lbl_Unit") as Label;

            if (ddl_Product != null)
            {
                lbl_Unit.Text = product.GetOrderUnit(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        protected void Create()
        {
            var drNew = dsMMEdit.Tables[moveMentDt.TableName].NewRow();

            drNew["RefId"] = string.Empty;
            drNew["DtId"] = (dsMMEdit.Tables[moveMentDt.TableName].Rows.Count == 0
                ? 1
                : int.Parse(
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1][
                        "DtId"].ToString()) + 1);

            if (dsMMEdit.Tables[moveMentDt.TableName].Rows.Count > 0)
            {
                drNew["LocationCode"] =
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1][
                        "LocationCode"].ToString();
            }

            dsMMEdit.Tables[moveMentDt.TableName].Rows.Add(drNew);

            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1;
            grd_MovementDt.DataBind();

            MMEditMode = "NEW";
        }

        protected void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
            return;
        }

        protected void ddl_ProductCode_Load(object sender, EventArgs e)
        {
            var ddl_ProductCode = sender as ASPxComboBox;

            if (ddl_ProductCode != null)
            {
                ddl_ProductCode.DataSource = product.GetLookUp_LocationCode(lbl_ToStore.Text, LoginInfo.ConnStr);
                ddl_ProductCode.ValueField = "ProductCode";
                ddl_ProductCode.DataBind();
            }
        }

        protected void ddl_ProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tp_Information =
                grd_MovementDt.Rows[grd_MovementDt.EditIndex].FindControl("tp_Information") as ASPxPageControl;
            var p_TIO = tp_Information.FindControl("p_TIO") as Panel;
            var ddl_ProductCode = p_TIO.FindControl("ddl_ProductCode") as ASPxComboBox;
            var lbl_Unit_TIO = p_TIO.FindControl("lbl_Unit_TIO") as Label;

            if (lbl_Unit_TIO != null)
            {
                lbl_Unit_TIO.Text = product.GetInvenUnit(ddl_ProductCode.Value.ToString(), LoginInfo.ConnStr);
            }
        }

        // Grid View
        protected void grd_MovementDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var p_HeaderSI = e.Row.FindControl("p_HeaderSI") as Panel;
                var p_HeaderSO = e.Row.FindControl("p_HeaderSO") as Panel;
                var p_HeaderTIO = e.Row.FindControl("p_HeaderTIO") as Panel;

                // Check Movement of Type
                if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                {
                    p_HeaderSO.Visible = false;
                    p_HeaderTIO.Visible = false;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "SO")
                {
                    p_HeaderSI.Visible = false;
                    p_HeaderTIO.Visible = false;
                }
                else if (Request.Params["Prefix"].ToUpper().ToString() == "TI")
                {
                    p_HeaderSO.Visible = false;
                    p_HeaderSI.Visible = false;
                }
                else
                {
                    p_HeaderSO.Visible = false;
                    p_HeaderSI.Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("tp_Information") != null)
                {
                    var tp_Information = e.Row.FindControl("tp_Information") as ASPxPageControl;

                    var p_SIO = tp_Information.FindControl("p_SIO") as Panel;
                    var p_TIO = tp_Information.FindControl("p_TIO") as Panel;

                    if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                        Request.Params["Prefix"].ToUpper().ToString() == "SO")
                    {
                        p_TIO.Visible = false;

                        // Store Location.
                        var ddl_Store = tp_Information.FindControl("ddl_Store") as ASPxComboBox;

                        if (ddl_Store != null)
                        {
                            ddl_Store.Value = DataBinder.Eval(e.Row.DataItem, "LocationCode");
                        }

                        // Product.
                        if (tp_Information.FindControl("ddl_Product") != null && ddl_Store.Value != null)
                        {
                            var ddl_Product = tp_Information.FindControl("ddl_Product") as ASPxComboBox;

                            ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_Store.Value.ToString(),
                                LoginInfo.ConnStr);
                            ddl_Product.ValueField = "ProductCode";
                            ddl_Product.DataBind();

                            ddl_Product.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                        }

                        // Unit.
                        if (tp_Information.FindControl("lbl_Unit") != null)
                        {
                            var lbl_Unit = tp_Information.FindControl("lbl_Unit") as Label;
                            lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        }

                        // Qty.
                        if (tp_Information.FindControl("txt_Qty") != null)
                        {
                            var txt_Qty = tp_Information.FindControl("txt_Qty") as TextBox;
                            txt_Qty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                        }

                        // Unit Cost
                        if (Request.Params["Prefix"].ToUpper().ToString() != "SO")
                        {
                            if (tp_Information.FindControl("txt_UnitCost") != null)
                            {
                                var txt_UnitCost = tp_Information.FindControl("txt_UnitCost") as TextBox;
                                txt_UnitCost.Text = DataBinder.Eval(e.Row.DataItem, "UnitCost").ToString();
                            }
                        }
                        else
                        {
                            if (tp_Information.FindControl("lbl_UnitCost") != null &&
                                tp_Information.FindControl("txt_UnitCost") != null)
                            {
                                var lbl_UnitCost = tp_Information.FindControl("lbl_UnitCost") as Label;
                                var txt_UnitCost = tp_Information.FindControl("txt_UnitCost") as TextBox;
                                lbl_UnitCost.Visible = false;
                                txt_UnitCost.Visible = false;
                            }
                        }

                        // Debit
                        if (tp_Information.FindControl("ddl_Debit") != null)
                        {
                            var ddl_Debit = tp_Information.FindControl("ddl_Debit") as ASPxComboBox;
                            ddl_Debit.Value = DataBinder.Eval(e.Row.DataItem, "DebitAcc");
                        }

                        // Credit
                        if (tp_Information.FindControl("ddl_Credit") != null)
                        {
                            var ddl_Credit = tp_Information.FindControl("ddl_Credit") as ASPxComboBox;
                            ddl_Credit.Value = DataBinder.Eval(e.Row.DataItem, "CreditAcc");
                        }

                        // Comment
                        if (tp_Information.FindControl("txt_Comment") != null)
                        {
                            var txt_Comment = tp_Information.FindControl("txt_Comment") as TextBox;
                            txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                        }
                    }
                    else
                    {
                        var ddl_ProductCode = tp_Information.FindControl("ddl_ProductCode") as ASPxComboBox;
                        var lbl_ProductCode = tp_Information.FindControl("lbl_ProductCode") as Label;

                        // Transfer In/Out
                        p_SIO.Visible = false;

                        // Product.
                        if (Request.Params["Prefix"].ToUpper() == "TO")
                        {
                            lbl_ProductCode.Visible = false;

                            if (ddl_ProductCode != null)
                            {
                                ddl_ProductCode.DataSource = product.GetLookUp_LocationCode(lbl_ToStore.Text,
                                    LoginInfo.ConnStr);
                                ddl_ProductCode.ValueField = "ProductCode";
                                ddl_ProductCode.DataBind();

                                ddl_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                            }
                        }
                        else
                        {
                            ddl_ProductCode.Visible = false;

                            if (lbl_ProductCode != null)
                            {
                                lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " +
                                                       product.GetName(
                                                           DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                                           LoginInfo.ConnStr) + " : " +
                                                       product.GetName2(
                                                           DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                                           LoginInfo.ConnStr);
                            }
                        }

                        // Unit.
                        if (tp_Information.FindControl("lbl_Unit_TIO") != null)
                        {
                            var lbl_Unit_TIO = tp_Information.FindControl("lbl_Unit_TIO") as Label;
                            lbl_Unit_TIO.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        }

                        // Qty.
                        if (tp_Information.FindControl("txt_QtyTrInOut") != null &&
                            tp_Information.FindControl("lbl_QtyInOut") != null)
                        {
                            var lbl_QtyInOut = tp_Information.FindControl("lbl_QtyInOut") as Label;
                            var txt_QtyTrInOut = tp_Information.FindControl("txt_QtyTrInOut") as TextBox;

                            if (Request.Params["Prefix"].ToUpper().ToString() != "TO")
                            {
                                lbl_QtyInOut.Text = "Qty Tr/In";
                                txt_QtyTrInOut.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                            }
                            else
                            {
                                lbl_QtyInOut.Text = "Qty Tr/Out";
                                txt_QtyTrInOut.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                            }
                        }

                        // Debit
                        if (tp_Information.FindControl("ddl_Debit_TIO") != null)
                        {
                            var ddl_Debit = tp_Information.FindControl("ddl_Debit") as ASPxComboBox;
                            ddl_Debit.Value = DataBinder.Eval(e.Row.DataItem, "DebitAcc");
                        }

                        // Credit
                        if (tp_Information.FindControl("ddl_Credit_TIO") != null)
                        {
                            var ddl_Credit = tp_Information.FindControl("ddl_Credit") as ASPxComboBox;
                            ddl_Credit.Value = DataBinder.Eval(e.Row.DataItem, "CreditAcc");
                        }

                        // Comment
                        if (tp_Information.FindControl("txt_Comment_TIO") != null)
                        {
                            var txt_Comment = tp_Information.FindControl("txt_Comment") as TextBox;
                            txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                        }
                    }
                }

                var p_ItemSI = e.Row.FindControl("p_ItemSI") as Panel;
                var p_ItemSO = e.Row.FindControl("p_ItemSO") as Panel;
                var p_ItemTIO = e.Row.FindControl("p_ItemTIO") as Panel;

                if (p_ItemSI != null && p_ItemSO != null && p_ItemTIO != null)
                {
                    // Item Template.
                    if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                    {
                        p_ItemTIO.Visible = false;
                        p_ItemSO.Visible = false;

                        // Store Location.
                        if (e.Row.FindControl("lbl_LocationCode_SI") != null)
                        {
                            var lbl_LocationCode_SI = e.Row.FindControl("lbl_LocationCode_SI") as Label;
                            lbl_LocationCode_SI.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                        }

                        if (e.Row.FindControl("lbl_LocationName_SI") != null)
                        {
                            var lbl_LocationName_SI = e.Row.FindControl("lbl_LocationName_SI") as Label;
                            lbl_LocationName_SI.Text =
                                strLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_StoreName_SI") != null)
                        {
                            var lbl_StoreName_SI = e.Row.FindControl("lbl_StoreName_SI") as Label;
                            lbl_StoreName_SI.Text =
                                strLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        // Product 
                        if (e.Row.FindControl("lbl_ProductCode_SI") != null)
                        {
                            var lbl_ProductCode_SI = e.Row.FindControl("lbl_ProductCode_SI") as Label;
                            lbl_ProductCode_SI.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                        }

                        if (e.Row.FindControl("lbl_EnglishName_SI") != null)
                        {
                            var lbl_EnglishName_SI = e.Row.FindControl("lbl_EnglishName_SI") as Label;
                            lbl_EnglishName_SI.Text =
                                product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_LocalName_SI") != null)
                        {
                            var lbl_LocalName_SI = e.Row.FindControl("lbl_LocalName_SI") as Label;
                            lbl_LocalName_SI.Text =
                                product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_ItemDesc_SI") != null)
                        {
                            var lbl_ItemDesc_SI = e.Row.FindControl("lbl_ItemDesc_SI") as Label;
                            lbl_ItemDesc_SI.Text =
                                product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString() + " :" +
                                product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        // Unit
                        if (e.Row.FindControl("lbl_Unit_SI") != null)
                        {
                            var lbl_Unit_SI = e.Row.FindControl("lbl_Unit_SI") as Label;
                            lbl_Unit_SI.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        }

                        // Quantity.
                        if (e.Row.FindControl("lbl_Qty_SI") != null)
                        {
                            var lbl_Qty_SI = e.Row.FindControl("lbl_Qty_SI") as Label;
                            lbl_Qty_SI.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                            totalQty += Convert.ToDecimal(lbl_Qty_SI.Text);
                        }

                        // Unit Cost
                        if (e.Row.FindControl("lbl_UnitCost_SI") != null)
                        {
                            var lbl_UnitCost_SI = e.Row.FindControl("lbl_UnitCost_SI") as Label;
                            lbl_UnitCost_SI.Text = DataBinder.Eval(e.Row.DataItem, "UnitCost").ToString();
                        }
                    }
                    else if (Request.Params["Prefix"].ToUpper().ToString() == "SO")
                    {
                        p_ItemTIO.Visible = false;
                        p_ItemSI.Visible = false;

                        // Store Location.
                        if (e.Row.FindControl("lbl_LocationCode_SO") != null)
                        {
                            var lbl_LocationCode_SO = e.Row.FindControl("lbl_LocationCode_SO") as Label;
                            lbl_LocationCode_SO.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                        }

                        if (e.Row.FindControl("lbl_LocationName_SO") != null)
                        {
                            var lbl_LocationName_SO = e.Row.FindControl("lbl_LocationName_SO") as Label;
                            lbl_LocationName_SO.Text =
                                strLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_StoreName_SO") != null)
                        {
                            var lbl_StoreName_SO = e.Row.FindControl("lbl_StoreName_SO") as Label;
                            lbl_StoreName_SO.Text =
                                strLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        // Product 
                        if (e.Row.FindControl("lbl_ProductCode_SO") != null)
                        {
                            var lbl_ProductCode_SO = e.Row.FindControl("lbl_ProductCode_SO") as Label;
                            lbl_ProductCode_SO.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                        }

                        if (e.Row.FindControl("lbl_EnglishName_SO") != null)
                        {
                            var lbl_EnglishName_SO = e.Row.FindControl("lbl_EnglishName_SO") as Label;
                            lbl_EnglishName_SO.Text =
                                product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_LocalName_SO") != null)
                        {
                            var lbl_LocalName_SO = e.Row.FindControl("lbl_LocalName_SO") as Label;
                            lbl_LocalName_SO.Text =
                                product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        if (e.Row.FindControl("lbl_ItemDesc_SO") != null)
                        {
                            var lbl_ItemDesc_SO = e.Row.FindControl("lbl_ItemDesc_SO") as Label;
                            lbl_ItemDesc_SO.Text =
                                product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString() + " :" +
                                product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr).ToString();
                        }

                        // Unit
                        if (e.Row.FindControl("lbl_Unit_SO") != null)
                        {
                            var lbl_Unit_SO = e.Row.FindControl("lbl_Unit_SO") as Label;
                            lbl_Unit_SO.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        }

                        // Quantity.
                        if (e.Row.FindControl("lbl_Qty_SO") != null)
                        {
                            var lbl_Qty_SO = e.Row.FindControl("lbl_Qty_SO") as Label;
                            lbl_Qty_SO.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                            totalQty += Convert.ToDecimal(lbl_Qty_SO.Text);
                        }
                    }
                    else
                    {
                        p_ItemSI.Visible = false;
                        p_ItemSO.Visible = false;

                        if (e.Row.FindControl("lbl_ProductCode_TI") != null)
                        {
                            var lbl_ProductCode_TI = e.Row.FindControl("lbl_ProductCode_TI") as Label;
                            lbl_ProductCode_TI.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                        }

                        if (e.Row.FindControl("lbl_EnglishName_TI") != null)
                        {
                            var lbl_EnglishName_TI = e.Row.FindControl("lbl_EnglishName_TI") as Label;
                            lbl_EnglishName_TI.Text =
                                product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr);
                        }

                        if (e.Row.FindControl("lbl_LocalName_TI") != null)
                        {
                            var lbl_LocalName_TI = e.Row.FindControl("lbl_LocalName_TI") as Label;
                            lbl_LocalName_TI.Text =
                                product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr);
                        }

                        if (e.Row.FindControl("lbl_Unit_TI") != null)
                        {
                            var lbl_Unit_TI = e.Row.FindControl("lbl_Unit_TI") as Label;
                            lbl_Unit_TI.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        }

                        if (Request.Params["Prefix"].ToUpper().ToString() == "TO")
                        {
                            if (e.Row.FindControl("lbl_QtyAllocated_TO") != null)
                            {
                                var lbl_QtyAllocated_TO = e.Row.FindControl("lbl_QtyAllocated_TO") as Label;
                                lbl_QtyAllocated_TO.Text = String.Format("{0:0.00}",
                                    DataBinder.Eval(e.Row.DataItem, "QtyAllocate"));
                                totalQty += Convert.ToDecimal(lbl_QtyAllocated_TO.Text);
                            }

                            if (e.Row.FindControl("lbl_QtyTrOut_TO") != null)
                            {
                                var lbl_QtyTrOut_TO = e.Row.FindControl("lbl_QtyTrOut_TO") as Label;
                                lbl_QtyTrOut_TO.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                            }
                        }
                        else
                        {
                            if (e.Row.FindControl("lbl_QtyTransfer_TI") != null)
                            {
                                var lbl_QtyTransfer_TI = e.Row.FindControl("lbl_QtyTransfer_TI") as Label;
                                lbl_QtyTransfer_TI.Text = String.Format("{0:0.00}",
                                    moveMentDt.GetQty(DataBinder.Eval(e.Row.DataItem, "TrfOutId").ToString(),
                                        DataBinder.Eval(e.Row.DataItem, "TrfOutDtId").ToString(), LoginInfo.ConnStr));
                                totalQty += Convert.ToDecimal(lbl_QtyTransfer_TI.Text);
                            }

                            if (e.Row.FindControl("lbl_QtyTrIn_TI") != null)
                            {
                                var lbl_QtyTrIn_TI = e.Row.FindControl("lbl_QtyTrIn_TI") as Label;
                                lbl_QtyTrIn_TI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                            }
                        }
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var p_FooterSO = e.Row.FindControl("p_FooterSO") as Panel;
                var p_FooterSI = e.Row.FindControl("p_FooterSI") as Panel;
                var p_FooterTIO = e.Row.FindControl("p_FooterTIO") as Panel;

                if (Request.Params["Prefix"].ToUpper() == "TO" || Request.Params["Prefix"].ToUpper() == "TI")
                {
                    p_FooterSI.Visible = false;
                    p_FooterSO.Visible = false;

                    if (e.Row.FindControl("lbl_TrfTotalQty") != null)
                    {
                        var lbl_TrfTotalQty = e.Row.FindControl("lbl_TrfTotalQty") as Label;
                        lbl_TrfTotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
                else if (Request.Params["Prefix"].ToUpper() == "SI")
                {
                    p_FooterTIO.Visible = false;
                    p_FooterSO.Visible = false;

                    if (e.Row.FindControl("lbl_SITotalQty") != null)
                    {
                        var lbl_SITotalQty = e.Row.FindControl("lbl_SITotalQty") as Label;
                        lbl_SITotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
                else
                {
                    p_FooterTIO.Visible = false;
                    p_FooterSI.Visible = false;

                    if (e.Row.FindControl("lbl_SOTotalQty") != null)
                    {
                        var lbl_SOTotalQty = e.Row.FindControl("lbl_SOTotalQty") as Label;
                        lbl_SOTotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
            }
        }

        protected void grd_MovementDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (MMEditMode.ToUpper() == "NEW")
                {
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1]
                        .Delete();
                }

                if (MMEditMode.ToUpper() == "EDIT")
                {
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (MMEditMode.ToUpper() == "NEW")
                {
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1]
                        .Delete();
                }

                if (MMEditMode.ToUpper() == "EDIT")
                {
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[dsMMEdit.Tables[moveMentDt.TableName].Rows.Count - 1]
                        .CancelEdit();
                }
            }

            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = -1;
            grd_MovementDt.DataBind();

            MMEditMode = string.Empty;
        }

        protected void grd_MovementDt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = e.NewEditIndex;
            grd_MovementDt.DataBind();

            MMEditMode = "EDIT";
        }

        protected void grd_MovementDt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var tp_Information =
                grd_MovementDt.Rows[grd_MovementDt.EditIndex].FindControl("tp_Information") as ASPxPageControl;

            if (Request.Params["Prefix"].ToUpper().ToString() == "SI" ||
                Request.Params["Prefix"].ToUpper().ToString() == "SO")
            {
                var ddl_Store = tp_Information.FindControl("ddl_Store") as ASPxComboBox;
                var ddl_Product = tp_Information.FindControl("ddl_Product") as ASPxComboBox;
                var lbl_Unit = tp_Information.FindControl("lbl_Unit") as Label;
                var txt_Qty = tp_Information.FindControl("txt_Qty") as TextBox;
                var txt_UnitCost = tp_Information.FindControl("txt_UnitCost") as TextBox;
                var ddl_Debit = tp_Information.FindControl("ddl_Debit") as ASPxComboBox;
                var ddl_Credit = tp_Information.FindControl("ddl_Credit") as ASPxComboBox;
                var txt_Comment = tp_Information.FindControl("txt_Comment") as TextBox;

                //Check field cannot empty
                if (ddl_Store != null)
                {
                    if (ddl_Store.Value == null)
                    {
                        lbl_Warning.Text = "'Store' cannot empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (ddl_Product != null)
                {
                    if (ddl_Product.Value == null)
                    {
                        lbl_Warning.Text = "'SKU #' cannot empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (txt_Qty != null)
                {
                    if (txt_Qty.Text == string.Empty)
                    {
                        lbl_Warning.Text = "'Qty' cannot empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                {
                    if (txt_UnitCost != null)
                    {
                        if (txt_UnitCost.Text == string.Empty)
                        {
                            lbl_Warning.Text = "'Unit Cost' cannot empty";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }
                    }
                }

                //if (ddl_Debit != null)
                //{
                //    if (ddl_Debit.Value == string.Empty)
                //    {
                //        lbl_Warning.Text = "'Debit A/C' cannot empty";
                //        pop_Warning.ShowOnPageLoad = true;
                //        return;
                //    }
                //}

                //if (ddl_Credit != null)
                //{
                //    if (ddl_Credit.Value == string.Empty)
                //    {
                //        lbl_Warning.Text = "'Credit A/C' cannot empty";
                //        pop_Warning.ShowOnPageLoad = true;
                //        return;
                //    }
                //}

                var drUpdating =
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[
                        grd_MovementDt.Rows[grd_MovementDt.EditIndex].DataItemIndex];
                drUpdating["LocationCode"] = ddl_Store.Value;
                drUpdating["ProductCode"] = ddl_Product.Value;
                drUpdating["Qty"] = decimal.Parse(txt_Qty.Text);
                drUpdating["Unit"] = lbl_Unit.Text;

                if (Request.Params["Prefix"].ToUpper().ToString() == "SI")
                {
                    drUpdating["UnitCost"] = decimal.Parse(txt_UnitCost.Text);
                }

                drUpdating["DebitACC"] = ddl_Debit.Value;
                drUpdating["CreditACC"] = ddl_Credit.Value;
                drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;
            }
            else
            {
                var ddl_ProductCode = tp_Information.FindControl("ddl_ProductCode") as ASPxComboBox;
                var lbl_Unit_TIO = tp_Information.FindControl("lbl_Unit_TIO") as Label;
                var txt_QtyTrInOut = tp_Information.FindControl("txt_QtyTrInOut") as TextBox;
                var ddl_Debit_TIO = tp_Information.FindControl("ddl_Debit_TIO") as ASPxComboBox;
                var ddl_Credit_TIO = tp_Information.FindControl("ddl_Credit_TIO") as ASPxComboBox;
                var txt_Comment_TIO = tp_Information.FindControl("txt_Comment_TIO") as TextBox;

                if (Request.Params["Prefix"].ToUpper() == "TO")
                {
                    if (ddl_ProductCode != null)
                    {
                        if (ddl_ProductCode.Value == null)
                        {
                            lbl_Warning.Text = "'SKU #' cannot empty";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }
                    }
                }

                if (txt_QtyTrInOut != null)
                {
                    if (txt_QtyTrInOut.Text == string.Empty)
                    {
                        lbl_Warning.Text = "'Qty Requested' cannot empty";
                        pop_Warning.ShowOnPageLoad = true;
                        return;
                    }
                }

                var drUpdating =
                    dsMMEdit.Tables[moveMentDt.TableName].Rows[
                        grd_MovementDt.Rows[grd_MovementDt.EditIndex].DataItemIndex];

                if (Request.Params["Prefix"].ToUpper() == "TO")
                {
                    drUpdating["ProductCode"] = ddl_ProductCode.Value;
                }

                drUpdating["Qty"] = decimal.Parse(txt_QtyTrInOut.Text);
                drUpdating["Unit"] = lbl_Unit_TIO.Text;
                drUpdating["DebitACC"] = ddl_Debit_TIO.Value;
                drUpdating["CreditACC"] = ddl_Credit_TIO.Value;
                drUpdating["Comment"] = txt_Comment_TIO.Text == string.Empty ? null : txt_Comment_TIO.Text;
            }

            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = -1;
            grd_MovementDt.DataBind();

            MMEditMode = string.Empty;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            return;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;
                case "COMMIT":
                    Commit();
                    break;
                case "BACK":
                    Back();
                    break;
            }
        }

        protected void menu_CmdGrd_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;
                case "DELETE":
                    Delete();
                    break;
            }
        }

        protected void btn_ConfirmDelete_Yes_Click(object sender, EventArgs e)
        {
            for (var i = grd_MovementDt.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_MovementDt.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drStkOutDt = dsMMEdit.Tables[moveMentDt.TableName].Rows[i];

                    if (drStkOutDt.RowState != DataRowState.Deleted)
                    {
                        drStkOutDt.Delete();
                    }
                }
            }

            grd_MovementDt.DataSource = dsMMEdit.Tables[moveMentDt.TableName];
            grd_MovementDt.EditIndex = -1;
            grd_MovementDt.DataBind();

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmDelete_No_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
            return;
        }
    }
}