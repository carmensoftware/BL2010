using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Collections.Generic;
using DevExpress.Web.ASPxTreeList;
using System.Data.SqlClient;

namespace BlueLedger.PL.Option.Store
{
    public partial class StoreEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.DeliveryPoint delpoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.ADMIN.Department departCode = new Blue.BL.ADMIN.Department();
        private readonly Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();

        private DataSet dsStoreLct = new DataSet();
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
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreLct = (DataSet)Session["dsEditStoreLct"];
            }

            base.Page_Load(sender, e);
        }

        /// <summary>
        ///     Page Retrieve.
        /// </summary>
        private void Page_Retrieve()
        {
            //var dsTmp = new DataSet();
            //var dtTmp = new DataTable();
            //var MsgError = string.Empty;

            // Modified on: 01/11/2017, By: Fon
            //string locationCode = Request.Params["ID"].ToString();
            string locationCode = (Request.Params["ID"] != null)
                ? Request.Params["ID"].ToString()
                : string.Empty;
            // End Modified.

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var getStoreSchema = strLct.Get(dsStoreLct, string.Empty, LoginInfo.ConnStr);

                if (getStoreSchema)
                {
                    var drStoreLct = dsStoreLct.Tables[strLct.TableName].NewRow();
                    drStoreLct["CreatedDate"] = ServerDateTime;
                    drStoreLct["CreatedBy"] = LoginInfo.LoginName;
                    dsStoreLct.Tables[strLct.TableName].Rows.Add(drStoreLct);

                    // Added on: 06/12/2017, By: Fon
                    var getProdLoc = prodLoc.GetListByLocation(dsStoreLct, locationCode, LoginInfo.ConnStr);
                    if (!getProdLoc)
                    {
                        return;
                    }
                    // End Added.
                }
            }
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                strLct.Get(dsStoreLct, Request.Params["ID"], LoginInfo.ConnStr);

                var getProdLoc = prodLoc.GetListByLocation(dsStoreLct, locationCode, LoginInfo.ConnStr);
                if (!getProdLoc)
                {
                    return;
                }
            }

            Session["dsEditStoreLct"] = dsStoreLct;
            Page_Setting();
        }

        /// <summary>
        ///     Page setting.
        /// </summary>
        private void Page_Setting()
        {
            var drEdit = dsStoreLct.Tables[strLct.TableName].Rows[0];

            #region Mode: NEW
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                txt_Code.Enabled = true;

                ddl_DeptCode.DataSource = departCode.GetList(LoginInfo.ConnStr);
                ddl_DeptCode.ValueField = "DepCode";
                ddl_DeptCode.DataBind();
                if (ddl_DeptCode.Items.Count > 0)
                {
                    ddl_DeptCode.SelectedIndex = 0;
                }

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
            }
            #endregion
            #region Mode: EDIT
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                txt_Code.Enabled = false;

                txt_Code.Text = drEdit["LocationCode"].ToString();
                txt_Store.Text = drEdit["LocationName"].ToString();
                ddl_DeptCode.Value = drEdit["DeptCode"].ToString();
                
				ddl_DelPoint.Value = drEdit["DeliveryPoint"].ToString();
                //ddl_DelPoint.SelectedValue = drEdit["DeliveryPoint"].ToString();


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
                chk_IsActive.Checked = Convert.ToBoolean(drEdit["IsActive"].ToString());
            }
            #endregion

            // Display Module List      
            tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
            tl_ProdCate.DataBind();

            // Assign Module Selected
            // Modified on: 01/11/2017, By: Fon, For: Debug error when U just create new once.
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                foreach (DataRow drProdLoc in dsStoreLct.Tables[prodLoc.TableName].Rows)
                {
                    if (tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString()) != null)
                    {
                        var tn_ProCate = tl_ProdCate.FindNodeByKeyValue(drProdLoc["ProductCode"].ToString());
                        tn_ProCate.Selected = true;       // HiLight
                    }
                }
            }

            // Added on: 06/12/2017, By: Fon
            Session["selection"] = tl_ProdCate.GetSelectedNodes();
            // End Added.
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
                    #region Save Edit
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        // Check Require Store Name
                        if (txt_Store.Text.ToUpper() == "")
                        {
                            lbl_CheckSaveNew.Text = "Store Name is required.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }

                        if (chk_IsActive.Checked == false &&
                            chk_IsActive.Checked != Convert.ToBoolean(dsStoreLct.Tables[strLct.TableName].Rows[0]["IsActive"].ToString()))
                        {
                            //lbl_Warning.Text =
                            //    "This Store/Location is inactive.<br> All relations of this store/location will be deleted.<br> Do you want to continue?";
                            //pop_Warning.ShowOnPageLoad = true;
                            //return;

                            // Modified on: 06/12/2017, By: Fon
                            bool isInactiveLocate = true;
                            var selectedList = tl_ProdCate.GetSelectedNodes();
                            for (int i = 0; i < selectedList.Count; i++)
                            {
                                int nodeLevel = tl_ProdCate.FindNodeByKeyValue(selectedList[i].Key).Level;
                                if (nodeLevel == 4)
                                {
                                    string productCode = selectedList[i].Key;
                                    bool isAbleToInactive = IsAble_ToInactiveProductInLocation(productCode, Request.Params["ID"]);
                                    if (!isAbleToInactive)
                                    {
                                        isInactiveLocate = false;
                                        break;
                                    }
                                }
                            }

                            if (isInactiveLocate)
                            {
                                lbl_Warning.Text = string.Format(@"This Store/Location is inactive.<br> 
                                    All relations of this store/location will be deleted.<br> 
                                    Do you want to continue?");
                                pop_Warning.ShowOnPageLoad = true;
                            }
                            else
                            {
                                chk_IsActive.Checked = true;
                                lbl_CheckSaveNew.Text = string.Format(@"Cannot inactive this location, 
                                    <br/>there are some products in stock.");
                                pop_CheckSaveNew.ShowOnPageLoad = true;
                            }
                            return;
                            // End Modified.
                        }


                        // Added on: 06/12/2017
                        bool check_ProdLoc = Check_ProductInLocation(tl_ProdCate.GetSelectedNodes());
                        if (check_ProdLoc) Save_HeaderDetail();
                        else pop_SaveProdLoc.ShowOnPageLoad = true;
                        // End Added.
                    }
                    #endregion
                    #region Save New
                    else if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        //if (strLct.StoreLctCountByLocationCode2(txt_Code.Text.ToString(), LoginInfo.ConnStr) > 0)

                        // 2012-03-02 Version ที่มีการ Where isActive = true ของ Location.update แล้วทุก server แต่ยังไม่ได้
                        //update ของลูกค้า
                        if (strLct.StoreLctCountByLocationCode(txt_Code.Text, LoginInfo.ConnStr) > 0)
                        {
                            //Display Error Message
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            lbl_CheckSaveNew.Text = "Store Code already exists.";

                            return;
                        }

                        // Check Require Store Code
                        if (txt_Code.Text.ToUpper() == string.Empty)
                        {
                            lbl_CheckSaveNew.Text = "Store Code is required.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }
                        // Check Require Store Name

                        if (txt_Store.Text.ToUpper() == string.Empty)
                        {
                            lbl_CheckSaveNew.Text = "Store Name is required.";
                            pop_CheckSaveNew.ShowOnPageLoad = true;
                            return;
                        }


                        // Added on: 06/12/2017
                        bool check_ProdLoc = Check_ProductInLocation(tl_ProdCate.GetSelectedNodes());
                        if (check_ProdLoc) Save_HeaderDetail();
                        else pop_SaveProdLoc.ShowOnPageLoad = true;
                        // End Added.
                    }
                    #endregion
                    break;

                case "BACK":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("Store.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + Request.Params["ID"] +
                                          "&VID=" + Request.Params["VID"]);
                    }
                    else
                    {
                        Response.Redirect("StoreLst.aspx");
                    }
                    break;
            }
        }

        // Added on: 06/12/2017, By:Fon
        protected void Save_HeaderDetail()
        {
            var saved = Save();
            var savedProdLoc = Save_ProductOfLocation();

            if (saved && savedProdLoc)
            {
                if (Request.Params["MODE"].ToUpper() == "EDIT")
                {
                    Response.Redirect("Store.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID="
                        + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                }
                else if (Request.Params["MODE"].ToUpper() == "NEW")
                {
                    Response.Redirect("Store.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID="
                        + txt_Code.Text + "&VID=" + Request.Params["VID"]);
                }
            }
        }
        // End Added.

        //private bool SaveNew()
        //{
        //    var MsgError = string.Empty;

        //    var drEdit = dsEditStoreLct.Tables[strLct.TableName].Rows[0];

        //    drEdit["LocationName"] = txt_Store.Text;

        //    if (ddl_DeptCode.Value != null)
        //    {
        //        drEdit["DeptCode"] = Convert.ToString(ddl_DeptCode.Value).ToUpper();
        //    }

        //    if (ddl_DelPoint.Value != null)
        //    {
        //        drEdit["DeliveryPoint"] = Convert.ToInt32(ddl_DelPoint.Value);
        //    }

        //    if (ddl_Eop.Value != null)
        //    {
        //        drEdit["EOP"] = Convert.ToInt32(ddl_Eop.Value);
        //    }

        //    drEdit["IsActive"] = chk_IsActive.Checked;

        //    //Set Visible is false
        //    //drEdit["AccountNo"] = txt_AccCode.Text.ToString();

        //    drEdit["UpdatedDate"] = ServerDateTime;
        //    drEdit["UpdatedBy"] = LoginInfo.LoginName;


        //    return strLct.SaveHeader(dsEditStoreLct, ref MsgError, LoginInfo.ConnStr);
        //}

        private bool Save()
        {
            var drStoreLct = dsStoreLct.Tables[strLct.TableName].Rows[0];

            drStoreLct["LocationCode"] = txt_Code.Text;
            drStoreLct["LocationName"] = txt_Store.Text;
            drStoreLct["AccountNo"] = txt_AccCode.Text;

            if (ddl_DeptCode.Value != null)
            {
                drStoreLct["DeptCode"] = Convert.ToString(ddl_DeptCode.Value).ToUpper();
            }
            //if (ddl_DelPoint.Value != null)
            //{
            //    drStoreLct["DeliveryPoint"] = ddl_DelPoint.Value;//Convert.ToInt32(ddl_DelPoint.Value);
            //}

            if (ddl_DelPoint.Value != null)
            {
                drStoreLct["DeliveryPoint"] = ddl_DelPoint.Value.ToString();
            }
			
			//StoreLct["DeliveryPoint"] = ddl_DelPoint.SelectedValue.ToString();
			//lbl_StoreLocation_Nm.Text = ddl_DelPoint.SelectedValue.ToString();
			

            if (ddl_Eop.Value != null)
            {
                drStoreLct["EOP"] = Convert.ToInt32(ddl_Eop.Value);
            }

            drStoreLct["IsActive"] = chk_IsActive.Checked;
            drStoreLct["CreatedDate"] = ServerDateTime;
            drStoreLct["CreatedBy"] = LoginInfo.LoginName;
            drStoreLct["UpdatedDate"] = ServerDateTime;
            drStoreLct["UpdatedBy"] = LoginInfo.LoginName;

            string MsgError = string.Empty;
            return strLct.SaveHeader(dsStoreLct, ref MsgError, LoginInfo.ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_DeptCode_Load(object sender, EventArgs e)
        {
            ddl_DeptCode.DataSource = departCode.GetList(LoginInfo.ConnStr);
            ddl_DeptCode.ValueField = "DepCode";
            ddl_DeptCode.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_DelPoint_Load(object sender, EventArgs e)
        {
            ddl_DelPoint.DataSource = delpoint.GetList(LoginInfo.ConnStr);
            //ddl_DelPoint.ValueField = "DptCode";
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
            if (Save())
            {
                Response.Redirect("Store.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + Request.Params["ID"] +
                                  "&VID=" + Request.Params["VID"]);
            }
        }

        protected void btn_No_Click(object sender, EventArgs e)
        {
            chk_IsActive.Checked = true;
            pop_Warning.ShowOnPageLoad = false;
        }

        #region About TreeList
        // Modified on: 2017/09/05, By: Fon
        // Transform Store.aspx.cs

        protected void tl_ProdCate_Load(object sender, EventArgs e)
        {
            tl_ProdCate.DataSource = product.GetActiveListForTreeView(LoginInfo.ConnStr);
            tl_ProdCate.DataBind();
        }

        protected bool Save_ProductOfLocation()
        {
            var dsProdLoc = new DataSet();

            //dsProdLoc = (DataSet)Session["dsStoreLct"];
            dsProdLoc = (DataSet)Session["dsEditStoreLct"];

            // Delete all ProdLoc Data.
            if (dsProdLoc.Tables[prodLoc.TableName] != null)
            {
                foreach (DataRow drStoreCheck in dsProdLoc.Tables[prodLoc.TableName].Rows)
                {
                    if (drStoreCheck.RowState != DataRowState.Deleted)
                    {
                        drStoreCheck.Delete();
                    }
                }
            }

            var selectNodes = tl_ProdCate.GetSelectedNodes();
            lbl_Test.Text = selectNodes.Count.ToString() + " Nodes.";
            for (var i = 0; i < selectNodes.Count; i++)
            {
                switch (selectNodes[i].Level)
                {
                    case 3:
                        {
                            // Do it in SP.
                            break;
                        }
                    case 4:
                        {
                            var drNewStoreCheck = dsProdLoc.Tables[prodLoc.TableName].NewRow();

                            drNewStoreCheck["LocationCode"] = txt_Code.Text;
                            drNewStoreCheck["ProductCode"] = selectNodes[i].Key;
                            dsProdLoc.Tables[prodLoc.TableName].Rows.Add(drNewStoreCheck);
                            break;
                        }
                    default:
                        break;
                }
            }

            // Save Change to Store Location Table.
            var saveStoreLocation = prodLoc.Save(dsProdLoc, LoginInfo.ConnStr);
            if (saveStoreLocation)
            {
                // Clear value
                dsProdLoc.Clear();
                Session["dsEditStoreLct"] = null;
                return true;
            }
            else
            {
                dsProdLoc.Clear();
                return false;
            }
        }

        // Added on: 06/12/2017, By: Fon, For: Check Prouduct before take it off.
        #region
        protected List<TreeListNode> GetDifference(List<TreeListNode> smallList, List<TreeListNode> bigList)
        {
            List<TreeListNode> selectedList = new List<TreeListNode>(bigList);
            for (int i = 0; i < smallList.Count; i++)
            {
                for (int j = 0; j < selectedList.Count; j++)
                {
                    if (smallList[i].Key == selectedList[j].Key)
                    {
                        selectedList.Remove(selectedList[j]);
                        break;
                    }
                }
            }

            return selectedList;
        }

        protected bool Check_ProductInLocation(List<TreeListNode> currList)
        {
            List<TreeListNode> oriList = (List<TreeListNode>)Session["selection"];
            List<TreeListNode> diffList = new List<TreeListNode>((currList.Count > oriList.Count)
                ? GetDifference(oriList, currList)
                : GetDifference(currList, oriList));

            string warning = string.Empty;
            for (int i = 0; i < diffList.Count; i++)
            {
                int nodeLevel = tl_ProdCate.FindNodeByKeyValue(diffList[i].Key).Level;
                bool nodeChecked = tl_ProdCate.FindNodeByKeyValue(diffList[i].Key).Selected;

                // Lavel 4 == Product, Following from old style.
                if (nodeLevel == 4 && nodeChecked == false)
                {
                    string productCode = string.Format("{0}", diffList[i].Key);
                    string errMsg = string.Empty;
                    string invUnit = string.Empty;
                    decimal onhand = 0;
                    bool isAbleToInactive = IsAble_ToInactiveProductInLocation(
                        productCode, Request.Params["ID"], ref onhand, ref invUnit, ref errMsg);
                    if (!isAbleToInactive)
                    {
                        tl_ProdCate.FindNodeByKeyValue(diffList[i].Key).Selected = true;
                        warning += (onhand > 0)
                            ? string.Format("<br/>{0}: Remains {1:0.##} <{2}>,", diffList[i].Key, onhand, invUnit)
                            : string.Format("<br/>{0}: In Progress,", diffList[i].Key); // Spare
                    }
                }
            }

            if (warning != string.Empty)
            {
                warning = warning.Remove(warning.Length - 1) + ".";
                lbl_SaveStatus.Text = string.Format(@"<br/>These items{0}
                    <br/><br/>Would you like to save with skip those items?", warning);
                return false;
            }
            return true;
        }

        protected bool IsAble_ToInactiveProductInLocation(string productCode, string locationCode)
        {
            string errMsg = string.Empty;
            string invUnit = string.Empty;
            decimal onhand = 0;
            return IsAble_ToInactiveProductInLocation(
                productCode, locationCode, ref onhand, ref invUnit, ref errMsg);
        }

        protected bool IsAble_ToInactiveProductInLocation(string productCode, string locationCode, ref decimal onhand, ref string invUnit, ref string errMsg)
        {
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format("EXEC [IN].[IsableToInactiveProductInLocation] '{0}', '{1}'", productCode, locationCode);
            bool ableToInactive = false;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ableToInactive = (dt.Rows[0]["AbleToInactive"].ToString().ToLower() == "true") ? true : false;
                invUnit = Convert.ToString(dt.Rows[0]["InventoryUnit"]);
                onhand = Convert.ToDecimal(dt.Rows[0]["OnHand"]);
                conn.Close();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                conn.Close();
            }
            return ableToInactive;
        }
        #endregion
        // End Added.

        #endregion

        protected void btn_SPL_Yes_Click(object sender, EventArgs e)
        {
            // Save with active if that product cannot inactive.
            Save_HeaderDetail();
        }

        protected void btn_SPL_No_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Request.RawUrl.ToString());
            pop_SaveProdLoc.ShowOnPageLoad = false;
        }
        #endregion
    }
}