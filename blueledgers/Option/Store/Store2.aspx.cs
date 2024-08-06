using System;
using System.Collections;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxTreeList;

namespace BlueLedger.PL.Option.Store
{
    public partial class Store2 : BasePage
    {
        private readonly Blue.BL.ADMIN.AccountMapp accCode = new Blue.BL.ADMIN.AccountMapp();
        private readonly Blue.BL.PC.CN.CnDt cndt = new Blue.BL.PC.CN.CnDt();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint delpoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prdt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.ADMIN.UserStore userStore = new Blue.BL.ADMIN.UserStore();
        private DataSet dsDelPoint = new DataSet();
        private DataSet dsStoreLct = new DataSet();
        private DataSet dsTemplate = new DataSet();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreLct = (DataSet) Session["dsStoreLct"];
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        private void Page_Retrieve()
        {
            var locationCode = Request.Params["ID"];


            // 2012-03-02 Version update แล้วทุก server แต่ยังไม่ได้ update ของลูกค้า
            //strLct.Get2(dsStoreLct, locationCode, LoginInfo.ConnStr);
            var getStore = strLct.Get(dsStoreLct, locationCode, LoginInfo.ConnStr);

            if (!getStore)
            {
                return;
            }

            var getProdLoc = prodLoc.GetListByLocation(dsStoreLct, locationCode, LoginInfo.ConnStr);

            if (!getProdLoc)
            {
                return;
            }

            Session["dsStoreLct"] = dsStoreLct;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drStore = dsStoreLct.Tables[strLct.TableName].Rows[0];

            lbl_Code.Text = drStore["LocationCode"].ToString();
            lbl_Store.Text = drStore["LocationName"].ToString();
            lbl_DelPoint.Text = delpoint.GetName(drStore["DeliveryPoint"].ToString(), LoginInfo.ConnStr);
            lbl_AccCode.Text = drStore["AccountNo"].ToString();
            chk_IsActive.Checked = Convert.ToBoolean(drStore["IsActive"].ToString());
            lbl_StoreGrp.Text = drStore["StoreGrp"].ToString();

            if (drStore["EOP"].ToString() == "1")
            {
                lbl_Eop.Text = "Enter Counted Stock";
            }
            else if (drStore["EOP"].ToString() == "2")
            {
                lbl_Eop.Text = "Default Zero";
            }
            else if (drStore["EOP"].ToString() == "3")
            {
                lbl_Eop.Text = "Default System";
            }

            // Display Module List
            tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
            tl_ProdCate.DataBind();

            // Assign Module Selected
            foreach (DataRow drProdLoc in dsStoreLct.Tables[prodLoc.TableName].Rows)
            {
                if (tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString()) != null)
                {
                    var tn_ProCate = tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString());
                    tn_ProCate.Selected = true;
                    NodeRecursiveExpand(tn_ProCate);
                }
            }
        }

        private void NodeRecursiveExpand(TreeListNode tn_ProCate)
        {
            tn_ProCate.Expanded = true;

            if (tn_ProCate.ParentNode != null)
            {
                NodeRecursiveExpand(tn_ProCate.ParentNode);
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("StoreEdit2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=New&VID=" +
                                      Request.Params["VID"]);
                    break;

                case "EDIT":
                    Response.Redirect("StoreEdit2.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=Edit&ID=" +
                                      Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                    break;

                case "DELETE":
                    var StoreLct = dsStoreLct.Tables[strLct.TableName].Rows[0]["LocationCode"].ToString();

                    // Check LocationCode used in Account Mapping
                    if (accCode.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in Account Mapping. Can not Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in User Assigned Store   
                    if (userStore.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in User Assigned Store, Can not Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Market List or Standard Order
                    if (template.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        // Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in Market List or Standard Order, Can not Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Purchase Request
                    if (pr.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0 ||
                        prdt.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        // Display Error Message
                        lbl_CheckDelete.Text = "Store has been used Purchase Request, Can not Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Credit Note
                    if (cndt.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message.
                        lbl_CheckDelete.Text = "Store has used in Credit Note, Can not Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;
                case "PRINT":
                    // Send LocationCode  to Session            
                    var objArrList = new ArrayList();
                    objArrList.Add(Request.Params["ID"]);
                    Session["s_arrNo"] = objArrList;

                    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=201" + "&BuCode=" +
                                     Request.Params["BuCode"];
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");

                    break;
                case "BACK":
                    Response.Redirect("StoreLst2.aspx");
                    break;
            }
        }

        protected void tl_ProdCate_Load(object sender, EventArgs e)
        {
            tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
            tl_ProdCate.DataBind();
        }

        protected void menu_Module_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    var dsProdLoc = new DataSet();

                    dsProdLoc = (DataSet) Session["dsStoreLct"];

                    // Delete all ProdLoc Data.                    
                    foreach (DataRow drStoreCheck in dsProdLoc.Tables[prodLoc.TableName].Rows)
                    {
                        if (drStoreCheck.RowState != DataRowState.Deleted)
                        {
                            drStoreCheck.Delete();
                        }
                    }

                    // Insert New RoleStoreCheck.
                    var selectNodes = tl_ProdCate.GetSelectedNodes();

                    for (var i = 0; i < selectNodes.Count; i++)
                    {
                        if (selectNodes[i].Level == 4)
                        {
                            var drNewStoreCheck = dsProdLoc.Tables[prodLoc.TableName].NewRow();

                            drNewStoreCheck["LocationCode"] = lbl_Code.Text;
                            drNewStoreCheck["ProductCode"] = selectNodes[i].Key;
                            dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drNewStoreCheck);
                        }
                    }

                    // Save Change to Store Location Table.
                    var saveStoreLocation = prodLoc.Save(dsProdLoc, LoginInfo.ConnStr);

                    if (saveStoreLocation)
                    {
                        // Clear value
                        dsProdLoc.Clear();
                        Session["dsStoreLct"] = null;

                        // Refresh all Role Data.
                        Page_Retrieve();

                        // Display Successfully Message
                        pop_RoleStoreLocationSave.ShowOnPageLoad = true;
                    }
                    else
                    {
                        dsProdLoc.Clear();
                    }

                    break;
            }
        }

        /// <summary>
        ///     Delete Select Store Location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var StoreLct = dsStoreLct.Tables[strLct.TableName].Rows[0]["LocationCode"].ToString();
            var MsgError = string.Empty;

            // Delete StoreLocation data.            
            dsStoreLct.Tables[strLct.TableName].Rows[0].Delete();

            // Delete Location's Product data.
            foreach (DataRow drProdLoc in dsStoreLct.Tables[prodLoc.TableName].Rows)
            {
                drProdLoc.Delete();
            }

            //Save To DataBase.
            var saveStore = strLct.Delete(dsStoreLct, ref MsgError, LoginInfo.ConnStr);

            if (saveStore)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;
                Response.Redirect("StoreLst2.aspx?ID=" + StoreLct);
            }
        }

        private void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
            Response.Redirect("Storelst2.aspx");
        }

        protected void btn_ok_alert_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }

        protected void btn_OKsave_Click(object sender, EventArgs e)
        {
            pop_RoleStoreLocationSave.ShowOnPageLoad = false;
        }

        protected void btn_OK_Delete_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
        }

        protected void btn_OK_ChkDelete_Click(object sender, EventArgs e)
        {
            pop_CheckDelete.ShowOnPageLoad = false;
        }
    }
}