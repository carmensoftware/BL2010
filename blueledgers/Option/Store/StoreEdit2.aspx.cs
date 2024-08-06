using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Store
{
    public partial class StoreEdit2 : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.DeliveryPoint delpoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.IN.StoreGrp storeGrp = new Blue.BL.IN.StoreGrp();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();

        private DataSet dsEditStoreLct = new DataSet();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Statement here.
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsEditStoreLct = (DataSet) Session["dsEditStoreLct"];
            }

            base.Page_Load(sender, e);
        }

        /// <summary>
        ///     Page Retrieve.
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var dtTmp = new DataTable();
            var MsgError = string.Empty;

            //string locationCode = Request.Params["ID"].ToString();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // 2012-03-02 Version update แล้วทุก server แต่ยังไม่ได้ update ของลูกค้า
                //strLct.Get2(dsEditStoreLct, string.Empty, LoginInfo.ConnStr);
                var getStoreSchema = strLct.Get(dsEditStoreLct, string.Empty, LoginInfo.ConnStr);

                if (getStoreSchema)
                {
                    var drStore = dsEditStoreLct.Tables[strLct.TableName].NewRow();
                    drStore["CreatedDate"] = ServerDateTime;
                    drStore["CreatedBy"] = LoginInfo.LoginName;
                    dsEditStoreLct.Tables[strLct.TableName].Rows.Add(drStore);
                }
            }
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                strLct.Get(dsEditStoreLct, Request.Params["ID"], LoginInfo.ConnStr);

                // 2012-03-02 Version ที่มีการ Where isActive = true ของ Location.update แล้วทุก server แต่ยังไม่ได้
                //update ของลูกค้า
                //strLct.Get2(dsEditStoreLct, Request.Params["ID"], LoginInfo.ConnStr);
            }

            Session["dsEditStoreLct"] = dsEditStoreLct;
            Page_Setting();
        }

        /// <summary>
        ///     Page setting.
        /// </summary>
        private void Page_Setting()
        {
            //hf_ConnStr.Value = LoginInfo.ConnStr;

            if (dsEditStoreLct.Tables[strLct.TableName].Rows.Count > 0)
            {
                var drEdit = dsEditStoreLct.Tables[strLct.TableName].Rows[0];

                if (Request.Params["MODE"].ToUpper() == "NEW")
                {
                    txt_Code.Enabled = true;
                    txt_Store.Enabled = true;

                    ddl_DelPoint.DataSource = delpoint.GetList(LoginInfo.ConnStr);
                    ddl_DelPoint.DataBind();
                    if (ddl_DelPoint.Items.Count > 0)
                    {
                        ddl_DelPoint.SelectedIndex = 0;
                    }

                    if (ddl_Eop.Items.Count > 0)
                    {
                        ddl_Eop.SelectedIndex = 0;
                    }
                    //txt_AccCode.Enabled  = true;
                    //ddl_DelPoint.Value   = delpoint.GetName(drEdit["DeliveryPoint"].ToString(), LoginInfo.ConnStr);
                    //ddl_DelPoint.Enabled = true;
                    //ddl_Eop.Enabled      = true;
                }
                else if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    txt_Code.Text = drEdit["LocationCode"].ToString();
                    txt_Code.Enabled = false;
                    txt_Store.Text = drEdit["LocationName"].ToString();
                    chk_IsActive.Checked = Convert.ToBoolean(drEdit["IsActive"].ToString());

                    //txt_AccCode.Text    = drEdit["AccountNo"].ToString();
                    //txt_AccCode.Enabled = true;
                    //ddl_DelPoint.Value  = delpoint.GetName(drEdit["DeliveryPoint"].ToString(), LoginInfo.ConnStr);

                    ddl_DelPoint.Value = drEdit["DeliveryPoint"].ToString();
                    ddl_StoreGrp.Value = drEdit["StoreGrp"].ToString();
                    ddl_Eop.Value = drEdit["EOP"].ToString();

                    if (drEdit["EOP"].ToString() == "1")
                    {
                        ddl_Eop.Text = "Enter Counted Stock";
                    }
                    else if (drEdit["EOP"].ToString() == "2")
                    {
                        ddl_Eop.Text = "Default Zero";
                    }
                    else if (drEdit["EOP"].ToString() == "3")
                    {
                        ddl_Eop.Text = "Default System";
                    }

                    //tl_ProdCate.DataSource = strLct.GetActiveProdCate(LoginInfo.ConnStr);
                    //tl_ProdCate.DataBind();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        // Check Require Store Name
                        if (txt_Store.Text.ToUpper() == "")
                        {
                            lbl_CheckSaveNew.Text = "Store Name can not be empty.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }

                        if (chk_IsActive.Checked == false &&
                            chk_IsActive.Checked !=
                            Convert.ToBoolean(dsEditStoreLct.Tables[strLct.TableName].Rows[0]["IsActive"].ToString()))
                        {
                            lbl_Warning.Text =
                                "The Store Location active status setted false.<br> It will be delete all relation with this location.<br> Do you want to continue?";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }

                        if (ddl_StoreGrp.Value == null)
                        {
                            lbl_CheckSaveNew.Text = "Store Group can not be empty.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }

                        var saved = SaveNew();

                        if (saved)
                        {
                            Response.Redirect("Store2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" +
                                              Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                        }
                    }
                    else if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        //if (strLct.StoreLctCountByLocationCode2(txt_Code.Text.ToString(), LoginInfo.ConnStr) > 0)

                        // 2012-03-02 Version ที่มีการ Where isActive = true ของ Location.update แล้วทุก server แต่ยังไม่ได้
                        //update ของลูกค้า
                        if (strLct.StoreLctCountByLocationCode(txt_Code.Text, LoginInfo.ConnStr) > 0)
                        {
                            //Display Error Message
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            lbl_CheckSaveNew.Text = "Code is already Exist.";

                            return;
                        }

                        // Check Require Store Code
                        if (txt_Code.Text.ToUpper() == "")
                        {
                            lbl_CheckSaveNew.Text = "Store Code can not be empty.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }
                        // Check Require Store Name

                        if (txt_Store.Text.ToUpper() == "")
                        {
                            lbl_CheckSaveNew.Text = "Store Name can not be empty.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }

                        if (ddl_StoreGrp.Value == null)
                        {
                            lbl_CheckSaveNew.Text = "Store Group can not be empty.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }

                        var saved = Update();

                        if (saved)
                        {
                            Response.Redirect("Store2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + txt_Code.Text +
                                              "&VID=" + Request.Params["VID"]);
                        }
                    }
                    break;

                case "BACK":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("Store2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" +
                                          Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                    }
                    else
                    {
                        Response.Redirect("StoreLst2.aspx");
                    }
                    break;
            }
        }

        private bool SaveNew()
        {
            var MsgError = string.Empty;

            var drEdit = dsEditStoreLct.Tables[strLct.TableName].Rows[0];
            drEdit["LocationName"] = txt_Store.Text;

            if (ddl_DelPoint.Value != null)
            {
                drEdit["DeliveryPoint"] = Convert.ToInt32(ddl_DelPoint.Value);
            }
            if (ddl_Eop.Value != null)
            {
                drEdit["EOP"] = Convert.ToInt32(ddl_Eop.Value);
            }

            drEdit["IsActive"] = chk_IsActive.Checked;
            drEdit["StoreGrp"] = ddl_StoreGrp.Value.ToString();

            //Set Visible is false
            //drEdit["AccountNo"] = txt_AccCode.Text.ToString();

            drEdit["UpdatedDate"] = ServerDateTime;
            drEdit["UpdatedBy"] = LoginInfo.LoginName;

            return strLct.SaveHeader(dsEditStoreLct, ref MsgError, LoginInfo.ConnStr);
        }

        private bool Update()
        {
            var MsgError = string.Empty;

            var drNew = dsEditStoreLct.Tables[strLct.TableName].Rows[0];
            drNew["LocationCode"] = txt_Code.Text;
            drNew["LocationName"] = txt_Store.Text;
            drNew["AccountNo"] = txt_AccCode.Text;

            if (ddl_DelPoint.Value != null)
            {
                drNew["DeliveryPoint"] = Convert.ToInt32(ddl_DelPoint.Value);
            }
            if (ddl_Eop.Value != null)
            {
                drNew["EOP"] = Convert.ToInt32(ddl_Eop.Value);
            }

            drNew["IsActive"] = chk_IsActive.Checked;
            drNew["StoreGrp"] = ddl_StoreGrp.Value.ToString();
            drNew["CreatedDate"] = ServerDateTime;
            drNew["CreatedBy"] = LoginInfo.LoginName;
            drNew["UpdatedDate"] = ServerDateTime;
            drNew["UpdatedBy"] = LoginInfo.LoginName;

            return strLct.SaveHeader(dsEditStoreLct, ref MsgError, LoginInfo.ConnStr);
        }


        //protected void tl_ProdCate_Load(object sender, EventArgs e)
        //{
        //    //tl_ProdCate.DataSource = strLct.GetActiveProdCate(LoginInfo.ConnStr);
        //    //tl_ProdCate.DataBind();
        //}

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_DelPoint_Load(object sender, EventArgs e)
        {
            ddl_DelPoint.DataSource = delpoint.GetList(LoginInfo.ConnStr);
            ddl_DelPoint.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Eop_Load(object sender, EventArgs e)
        {
            //ddl_Eop.DataSource = strLct.Get(dsStoreLct, Request.Params["ID"].ToString(), LoginInfo.ConnStr);
            //ddl_Eop.DataBind();
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            var Save = SaveNew();

            if (Save)
            {
                //strLct.DeleteRelation(txt_Code.Text.Trim(), LoginInfo.ConnStr);
                Response.Redirect("Store2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + Request.Params["ID"] +
                                  "&VID=" + Request.Params["VID"]);
            }
        }

        protected void btn_No_Click(object sender, EventArgs e)
        {
            chk_IsActive.Checked = true;
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void ddl_StoreGrp_Load(object sender, EventArgs e)
        {
            ddl_StoreGrp.DataSource = storeGrp.GetList(LoginInfo.ConnStr);
            ddl_StoreGrp.DataBind();
        }

        #endregion
    }
}