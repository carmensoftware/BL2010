using System;
using System.Collections;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxTreeList;

using System.Web.UI;

namespace BlueLedger.PL.Option.Store
{
    public partial class Store : BasePage
    {
        private readonly Blue.BL.ADMIN.AccountMapp accCode = new Blue.BL.ADMIN.AccountMapp();

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp accountMapp =
            new Blue.BL.Option.Admin.Interface.AccountMapp(); // add by mai

        private readonly Blue.BL.PC.CN.CnDt cndt = new Blue.BL.PC.CN.CnDt();

        private readonly Blue.BL.Option.Inventory.DeliveryPoint delpoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.ADMIN.Department deptCode = new Blue.BL.ADMIN.Department();
        //private readonly DataSet dsAccountMap = new DataSet();
        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prdt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.ADMIN.UserStore userStore = new Blue.BL.ADMIN.UserStore();
        private DataSet dsDelPoint = new DataSet();
        private DataSet dsDept = new DataSet();
        private DataSet dsStoreLct = new DataSet();
        private DataSet dsTemplate = new DataSet();

        private DataSet dsProduct = new DataSet();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreLct = (DataSet)Session["dsStoreLct"];
                dsProduct = (DataSet)Session["dsProduct"];
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        private void Page_Retrieve()
        {
            var locationCode = Request.Params["ID"];

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
            Session["dsProduct"] = dsProduct;
            Page_Setting();
        }

        private void Page_Setting()
        {
            if (dsStoreLct.Tables[strLct.TableName].Rows.Count > 0)
            {
                var drStore = dsStoreLct.Tables[strLct.TableName].Rows[0];

                lbl_Code.Text = drStore["LocationCode"].ToString();
                lbl_Store.Text = drStore["LocationName"].ToString();
                //lbl_Dept.Text           = drStore["DepCode"].ToString();
                lbl_Dept.Text = deptCode.GetName(drStore["DeptCode"].ToString(), LoginInfo.ConnStr);
                lbl_DelPoint.Text = delpoint.GetName(drStore["DeliveryPoint"].ToString(), LoginInfo.ConnStr);
                lbl_AccCode.Text = drStore["AccountNo"].ToString();
                chk_IsActive.Checked = Convert.ToBoolean(drStore["IsActive"].ToString());

                //Add by mai
                //accountMapp.GetList(dsAccountMap, drStore["LocationCode"].ToString(), "ItemGroup", LoginInfo.ConnStr);
                //Session["dsAccountMap"] = dsAccountMap;

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
                //tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
                //tl_ProdCate.DataBind();

                // Assign Module Selected
                //foreach (DataRow drProdLoc in dsStoreLct.Tables[prodLoc.TableName].Rows)
                //{
                //    if (tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString()) != null)
                //    {
                //        var tn_ProCate = tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString());
                //        tn_ProCate.Selected = true;       // HiLight
                //        NodeRecursiveExpand(tn_ProCate);  // Control-Expand
                //    }
                //}

                // Modified on: 09/05/2017, By: Fon
                string locationCode = Request.Params["ID"];
                tl_ProdCate.DataSource = product.GetProductListForTreeView(locationCode, LoginInfo.ConnStr);
                tl_ProdCate.DataBind();
            }
        }

        #region comment
        //private void NodeRecursiveExpand(TreeListNode tn_ProCate)
        //{
        //    tn_ProCate.Expanded = true;

        //    if (tn_ProCate.ParentNode != null)
        //    {
        //        NodeRecursiveExpand(tn_ProCate.ParentNode);
        //    }
        //}
        #endregion

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("StoreEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=New&VID=" +
                                      Request.Params["VID"]);
                    break;

                case "EDIT":
                    Response.Redirect("StoreEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=Edit&ID=" +
                                      Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                    break;

                case "DELETE":
                    var StoreLct = dsStoreLct.Tables[strLct.TableName].Rows[0]["LocationCode"].ToString();

                    // Check LocationCode used in Account Mapping
                    if (accCode.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in Account Mapping. Cannot Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in User Assigned Store   
                    if (userStore.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in User Assigned Store, Cannot Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Market List or Standard Order
                    if (template.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        // Display Error Message
                        lbl_CheckDelete.Text = "Store has been used in Market List or Standard Order, Cannot Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Purchase Request
                    if (pr.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0 ||
                        prdt.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        // Display Error Message
                        lbl_CheckDelete.Text = "Store has been used Purchase Request, Cannot Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    // Check LocationCode used in Credit Note
                    if (cndt.CountByLocationCode(StoreLct, LoginInfo.ConnStr) > 0)
                    {
                        //Display Error Message.
                        lbl_CheckDelete.Text = "Store has used in Credit Note, Cannot Delete.";
                        pop_CheckDelete.ShowOnPageLoad = true;
                        return;
                    }

                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;
                case "PRINT":
                    // Send LocationCode  to Session            
                    //var objArrList = new ArrayList();
                    //objArrList.Add(Request.Params["ID"]);
                    //Session["s_arrNo"] = objArrList;

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=201" + "&BuCode=" +
                    //                 Request.Params["BuCode"];
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
                case "BACK":
                    Response.Redirect("StoreLst.aspx");
                    break;
            }
        }

        protected void tl_ProdCate_Load(object sender, EventArgs e)
        {
            //tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
            //tl_ProdCate.DataBind();

            //Modified on: 2017/05/09, By: Fon
            string locationCode = Request.Params["ID"];
            tl_ProdCate.DataSource = product.GetProductListForTreeView(locationCode, 1, LoginInfo.ConnStr);
            tl_ProdCate.DataBind();
        }

        #region Save product of Location
        /// Move to: StoreEdit.aspx.cs, On: 2017/05/09, By: Fon
        //protected void menu_Module_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        //{
        //    switch (e.Item.Name.ToUpper())
        //    {
        //        case "SAVE":
        //            #region Begin of Save Process
        //            var dsProdLoc = new DataSet();

        //            dsProdLoc = (DataSet)Session["dsStoreLct"];

        //            // Delete all ProdLoc Data.                    
        //            foreach (DataRow drStoreCheck in dsProdLoc.Tables[prodLoc.TableName].Rows)
        //            {
        //                if (drStoreCheck.RowState != DataRowState.Deleted)
        //                {
        //                    drStoreCheck.Delete();
        //                }
        //            }

        //            //Add by mai
        //            //var dsAccountMapDel = (DataSet)Session["dsAccountMap"];
        //            //foreach (DataRow dr in dsAccountMapDel.Tables[accountMapp.TableName].Rows)
        //            //{
        //            //    if (dr.RowState != DataRowState.Deleted)
        //            //    {
        //            //        dr.Delete();
        //            //    }
        //            //}

        //            //Session["dsAccountMap"] = null;

        //            // Insert New RoleStoreCheck.
        //            var selectNodes = tl_ProdCate.GetSelectedNodes();

        //            //Add by mai
        //            //var dsAccountMapp = new DataSet(accountMapp.TableName);
        //            //accountMapp.GetStructure(dsAccountMapp, LoginInfo.ConnStr);

        //            for (var i = 0; i < selectNodes.Count; i++)
        //            {
        //                switch (selectNodes[i].Level)
        //                {
        //                    case 3:
        //                        {
        //                            //DataTable dt = dsAccountMap.Tables[accountMapp.TableName];

        //                            //DataRow[] dr = dt.Select("StoreCode= '" + lbl_Code.Text + "' and " + "ItemGroupCode = '"
        //                            //    + selectNodes[i].Key + "' and PostType = '" + Blue.BL.Option.Admin.Interface.PostType.AP.ToString() + "'");
        //                            //if (dr == null)
        //                            //{
        //                            //    var drNewAccountMapp = dsAccountMapp.Tables[accountMapp.TableName].NewRow();

        //                            //    drNewAccountMapp["ID"] = Guid.NewGuid();
        //                            //    drNewAccountMapp["BusinessUnitCode"] = LoginInfo.BuInfo.BuCode;
        //                            //    drNewAccountMapp["StoreCode"] = lbl_Code.Text;
        //                            //    drNewAccountMapp["ItemGroupCode"] = selectNodes[i].Key;
        //                            //    drNewAccountMapp["PostType"] = Blue.BL.Option.Admin.Interface.PostType.AP.ToString();
        //                            //    dsAccountMapp.Tables[accountMapp.TableName].Rows.Add(drNewAccountMapp);
        //                            //}
        //                            break;
        //                        }
        //                    case 4:
        //                        {
        //                            var drNewStoreCheck = dsProdLoc.Tables[prodLoc.TableName].NewRow();

        //                            drNewStoreCheck["LocationCode"] = lbl_Code.Text;
        //                            drNewStoreCheck["ProductCode"] = selectNodes[i].Key;
        //                            dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drNewStoreCheck);
        //                            break;
        //                        }
        //                    default:
        //                        break;
        //                }
        //            }

        //            // Save Change to Store Location Table.
        //            var saveStoreLocation = prodLoc.Save(dsProdLoc, LoginInfo.ConnStr);
        //            //var saveAccountMap = false;

        //            //if (saveStoreLocation)
        //            //    saveAccountMap = accountMapp.Save(dsAccountMapp, LoginInfo.ConnStr);


        //            if (saveStoreLocation)
        //            {
        //                // Clear value
        //                dsProdLoc.Clear();
        //                Session["dsStoreLct"] = null;

        //                // Refresh all Role Data.
        //                Page_Retrieve();

        //                // Display Successfully Message
        //                pop_RoleStoreLocationSave.ShowOnPageLoad = true;
        //            }
        //            else
        //            {
        //                dsProdLoc.Clear();
        //            }
        //            #endregion
        //            break;
        //    }
        //}
        #endregion

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
                Response.Redirect("StoreLst.aspx?ID=" + StoreLct);
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
            Response.Redirect("Storelst.aspx");
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