using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class StoreLoc : BasePage
    {
        #region "Attributes"

        private Blue.BL.GL.Account.Account accCode = new Blue.BL.GL.Account.Account();
        private Blue.BL.Option.Inventory.DeliveryPoint delPoint = new Blue.BL.Option.Inventory.DeliveryPoint();


        private DataSet dsStoreLct = new DataSet();
        private DataTable dtAccCode = new DataTable();
        private DataTable dtDeliPoint = new DataTable();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.Page_Retrieve();
                //tv_Location.SettingsSelection.Recursive = true;
                ListPage1.DataBind();
            }
            //else
            //{
            //    dsStoreLct = (DataSet)Session["dsStoreLct"];
            //    this.SetAspxTreeView();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        //private void Page_Retrieve()
        //{
        //    DataSet dsTmp   = new DataSet();
        //    DataTable dtTmp = new DataTable();
        //    string MsgError = string.Empty;

        //    bool getStrLct = strLct.GetList(dsTmp, ref MsgError, LoginInfo.ConnStr);

        //    if (getStrLct)
        //    {
        //        dsStoreLct = dsTmp;

        //        prodCat.GetProdCateList1(dsStoreLct, LoginInfo.ConnStr);
        //        product.GetList(dsStoreLct, LoginInfo.ConnStr);
        //        prodLoc.GetList(dsStoreLct, LoginInfo.ConnStr);
        //    }
        //    else
        //    {
        //        // Display error message
        //        //lbl_Error.Text 
        //        string Error = Resources.MsgError.ResourceManager.GetString(MsgError);

        //        return;
        //    }

        //    this.Page_Setting();

        //    Session["dsStoreLct"] = dsStoreLct;
        //}

        /// <summary>
        /// 
        /// </summary>
        //private void Page_Setting()
        //{
        //    this.SetLookup();

        //    grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //    grd_SL.DataBind();

        //    //Set tree view.                        
        //    this.SetAspxTreeView();
        //    tv_Location.Enabled = false;           
        //}

        /// <summary>
        /// Binding data to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_SL_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        // Display Store Code
        //        if (e.Row.FindControl("lbl_StoreCode") != null)
        //        {
        //            Label lbl_StoreCode = (Label)e.Row.FindControl("lbl_StoreCode");
        //            lbl_StoreCode.Text  = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
        //        }

        //        // Display Store Code
        //        if (e.Row.FindControl("txt_StoreCode") != null)
        //        {
        //            TextBox txt_StoreCode = (TextBox)e.Row.FindControl("txt_StoreCode");
        //            txt_StoreCode.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
        //        }

        //        // Display Store Name
        //        if (e.Row.FindControl("lbl_StoreName") != null)
        //        {
        //            Label lbl_ConvUnit = (Label)e.Row.FindControl("lbl_StoreName");
        //            lbl_ConvUnit.Text  = DataBinder.Eval(e.Row.DataItem, "LocationName").ToString();
        //        }

        //        // Display Store Name
        //        if (e.Row.FindControl("txt_StoreName") != null)
        //        {
        //            TextBox txt_StoreName = (TextBox)e.Row.FindControl("txt_StoreName");
        //            txt_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "LocationName").ToString();
        //        }

        //        // Display
        //        if (e.Row.FindControl("lbl_DeliPoint") != null)
        //        {
        //            Label lbl_DeliPoint = (Label)e.Row.FindControl("lbl_DeliPoint");
        //            lbl_DeliPoint.Text  = delPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliveryPoint").ToString(), LoginInfo.ConnStr);
        //        }

        //        // Display lookup baseunit
        //        if (e.Row.FindControl("ddl_DeliPoint") != null)
        //        {
        //            DropDownList ddl_DeliPoint = (DropDownList)e.Row.FindControl("ddl_DeliPoint");

        //            if (dtDeliPoint != null)
        //            {
        //                ddl_DeliPoint.DataSource     = dtDeliPoint;
        //                ddl_DeliPoint.DataTextField  = "Name";
        //                ddl_DeliPoint.DataValueField = "DptCode";
        //                ddl_DeliPoint.DataBind();
        //                ddl_DeliPoint.SelectedValue  = DataBinder.Eval(e.Row.DataItem, "DeliveryPoint").ToString();
        //            }
        //        }

        //        // Display Account code
        //        if (e.Row.FindControl("lbl_AccCode") != null)
        //        {
        //            Label lbl_AccCode = (Label)e.Row.FindControl("lbl_AccCode");
        //            //lbl_AccCode.Text  = accCode.GetName(DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString(), LoginInfo.ConnStr);
        //            lbl_AccCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
        //        }

        //        // Display lookup baseunit
        //        if (e.Row.FindControl("txt_AccCode") != null)
        //        {
        //            TextBox txt_AccCode = (TextBox)e.Row.FindControl("txt_AccCode");
        //            //txt_AccCode.Text = accCode.GetName(DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString(), LoginInfo.ConnStr);
        //            txt_AccCode.Text = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
        //            //if (dtAccCode != null)
        //            //{
        //            //ddl_AccCode.DataSource = dtAccCode;
        //            //    ddl_AccCode.DataTextField  = "Desc";
        //            //    ddl_AccCode.DataValueField = "AccCode";
        //            //    ddl_AccCode.DataBind();
        //            //    ddl_AccCode.SelectedValue  = DataBinder.Eval(e.Row.DataItem, "AccountNo").ToString();
        //            //}
        //        }

        //        // Display Account code
        //        if (e.Row.FindControl("lbl_Eop") != null)
        //        {
        //            Label lbl_Eop  = (Label)e.Row.FindControl("lbl_Eop");
        //            string strEop  = DataBinder.Eval(e.Row.DataItem, "EOP").ToString();
        //            string strName = string.Empty;
        //            //DropDownList ddl_Eop = (DropDownList)e.Row.FindControl("ddl_Eop");

        //            switch (strEop)
        //            {
        //                case "1":
        //                    {
        //                        strName = "Enter Counted Stock";
        //                        break;
        //                    }
        //                case "2":
        //                    {
        //                        strName = "Default Zero";
        //                        break;
        //                    }
        //                case "3":
        //                    {
        //                        strName = "Default System";
        //                        break;
        //                    }
        //            }

        //            lbl_Eop.Text = strName.ToString();
        //        }


        //        // Display lookup baseunit
        //        if (e.Row.FindControl("ddl_Eop") != null)
        //        {
        //            DropDownList ddl_Eop = (DropDownList)e.Row.FindControl("ddl_Eop");
        //            ddl_Eop.DataBind();
        //            ddl_Eop.SelectedValue = DataBinder.Eval(e.Row.DataItem, "EOP").ToString();
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_SL_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    //lbl_Error.Text = string.Empty;
        //    dsStoreLct.Tables[strLct.TableName].Rows[e.RowIndex].Delete();

        //    // Delete row
        //    string MsgError = string.Empty;

        //    bool deleted = strLct.Save(dsStoreLct, ref MsgError, LoginInfo.ConnStr);

        //    if (deleted)
        //    {
        //        // Binding grid
        //        grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //        grd_SL.DataBind();

        //        btn_Add.Enabled = true;
        //        //lbl_Total.Text = dsUnitConv.Tables[unitConv.TableName].Rows.Count.ToString();

        //        // Save changed to session
        //        Session["dsStoreLct"] = dsStoreLct;
        //    }
        //    else
        //    {
        //        dsStoreLct.Tables[strLct.TableName].Rows[e.RowIndex].RejectChanges();

        //        // Display error message                
        //        //lbl_Error.Text = Resources.MsgError.ResourceManager.GetString(MsgError);
        //        string error = Resources.MsgError.ResourceManager.GetString(MsgError); 

        //        return;
        //    }          
        //}

        ///// <summary>
        ///// Edit select row 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_SL_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    tv_Location.Enabled = true;

        //    this.SetLookup();

        //    grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //    grd_SL.EditIndex  = e.NewEditIndex;
        //    grd_SL.DataBind();

        //    // Set 2 fields can not edit data.
        //    TextBox txt_StoreCode = (TextBox)grd_SL.Rows[e.NewEditIndex].FindControl("txt_StoreCode");
        //    //txt_StoreCode         = (TextBox)grd_SL.Rows[e.NewEditIndex].FindControl("txt_StoreCode");
        //    txt_StoreCode.Enabled = false;


        //    //Set TreeList.
        //    tv_Location.UnselectAll();

        //    DataTable dtSelect = prodLoc.GetList(txt_StoreCode.Text.ToString(), LoginInfo.ConnStr);

        //    if (dtSelect != null)
        //    {
        //        if (dtSelect.Rows.Count > 0)
        //        {
        //            for (int r = 0; r < dtSelect.Rows.Count; r++ )
        //            {
        //                foreach (TreeListNode tlNode in tv_Location.GetVisibleNodes())
        //                {
        //                    if (tlNode.GetValue("ProductCode") != null)
        //                    {
        //                        if (dtSelect.Rows[r]["ProductCode"].ToString() == tlNode.GetValue("ProductCode").ToString())
        //                        {
        //                            tlNode.Selected = true;
        //                        }
        //                    }                            
        //                }                        
        //            }                    
        //        }
        //    }

        //    btn_Add.Enabled = false;
        //}

        /// <summary>
        /// Cancel edit row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_SL_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    if (dsStoreLct.Tables[strLct.TableName].Rows[e.RowIndex].RowState == DataRowState.Added)
        //    {
        //        dsStoreLct.Tables[strLct.TableName].Rows[e.RowIndex].Delete();
        //    }

        //    // Binding grid
        //    grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //    grd_SL.EditIndex  = -1;
        //    grd_SL.DataBind();           

        //    btn_Add.Enabled     = true;
        //    tv_Location.Enabled = false;

        //    //Clear select in treeview            
        //    tv_Location.UnselectAll();           
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_SL_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    string MsgError = string.Empty;

        //    DataRow drUpdating = dsStoreLct.Tables[strLct.TableName].Rows[e.RowIndex];

        //    TextBox txt_StoreCode      = (TextBox)grd_SL.Rows[e.RowIndex].FindControl("txt_StoreCode");
        //    TextBox txt_StoreName      = (TextBox)grd_SL.Rows[e.RowIndex].FindControl("txt_StoreName");
        //    DropDownList ddl_DeliPoint = (DropDownList)grd_SL.Rows[e.RowIndex].FindControl("ddl_DeliPoint");
        //    TextBox txt_AccCode        = (TextBox)grd_SL.Rows[e.RowIndex].FindControl("txt_AccCode");
        //    //DropDownList ddl_AccCode   = (DropDownList)grd_SL.Rows[e.RowIndex].FindControl("ddl_AccCode");
        //    DropDownList ddl_Eop       = (DropDownList)grd_SL.Rows[e.RowIndex].FindControl("ddl_Eop");

        //    // Updating for assign data.
        //    // FUnitCode
        //    drUpdating["LocationCode"]  = txt_StoreCode.Text.ToString();
        //    drUpdating["LocationName"]  = txt_StoreName.Text.ToString();
        //    drUpdating["DeliveryPoint"] = ddl_DeliPoint.SelectedValue.ToString();
        //    //drUpdating["AccountNo"]     = ddl_AccCode.SelectedValue.ToString();
        //    //dtAccCode = accCode.GetSearch(txt_AccCode.Text, LoginInfo.ConnStr);
        //    drUpdating["AccountNo"]     = txt_AccCode.Text.ToString();

        //    drUpdating["EOP"]           = ddl_Eop.SelectedValue.ToString();
        //    drUpdating["CreatedDate"]   = ServerDateTime;
        //    drUpdating["CreatedBy"]     = LoginInfo.LoginName;
        //    drUpdating["UpdatedDate"]   = ServerDateTime;
        //    drUpdating["UpdatedBy"]     = LoginInfo.LoginName;

        //    // Save data from database            
        //    bool result = strLct.Save(dsStoreLct, ref MsgError, LoginInfo.ConnStr);

        //    if (result)
        //    {
        //        // Refresh data in GridView
        //        grd_SL.EditIndex = -1;
        //        grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //        grd_SL.DataBind();

        //        btn_Add.Enabled = true;

        //        tv_Location.Enabled = false;
        //        // Save changed to session
        //        Session["dsStoreLct"] = dsStoreLct;
        //    }
        //    else
        //    {
        //        // Display error message
        //        //lbl_Error.Text = Resources.MsgError.ResourceManager.GetString(MsgError);

        //        return;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //private void SetLookup()
        //{
        //    //Set lookup delivery point.
        //    dtDeliPoint = null;
        //    dtDeliPoint = delPoint.GetList(LoginInfo.ConnStr);

        //    DataRow drBlank1 = dtDeliPoint.NewRow();
        //    dtDeliPoint.Rows.InsertAt(drBlank1, 0);

        //    //Set lookup account code.
        //    dtAccCode = null;
        //    dtAccCode = accCode.GetList(LoginInfo.ConnStr);

        //    DataRow drBlank = dtAccCode.NewRow();
        //    dtAccCode.Rows.InsertAt(drBlank, 0);
        //}

        /// <summary>
        /// 
        /// </summary>
        //private void SetAspxTreeView()
        //{
        //    string strLevelNo     = string.Empty;
        //    string strCateCode    = string.Empty;
        //    string strCateCode2   = string.Empty;
        //    string strCateCode3   = string.Empty;

        //    DataTable dtCategory  = dsStoreLct.Tables[prodCat.TableName];
        //    DataTable dtProduct   = dsStoreLct.Tables[product.TableName];

        //    if (dtCategory != null)
        //    {
        //        int row = 0;

        //        for (int i = 0; i < dtCategory.Rows.Count; i++)
        //        {
        //            strLevelNo = dtCategory.Rows[i]["LevelNo"].ToString();                

        //            if (strLevelNo == "1")
        //            {
        //                row = (row == i ? i: row + 1);

        //                TreeListNode Level1   = tv_Location.AppendNode(row, null);
        //                Level1["Name"]        = dtCategory.Rows[i]["CategoryName"].ToString();                       

        //                strCateCode = dtCategory.Rows[i]["CategoryCode"].ToString();

        //                for (int j = 0; j < dtCategory.Rows.Count; j++)
        //                {
        //                    if (dtCategory.Rows[j]["LevelNo"].ToString() == "2")
        //                    {
        //                        if (strCateCode == dtCategory.Rows[j]["ParentNo"].ToString())
        //                        {
        //                            //Level1.Expanded = true;

        //                            row += 1;
        //                            TreeListNode Level2 = tv_Location.AppendNode(row , Level1);
        //                            Level2["Name"]      = dtCategory.Rows[j]["CategoryName"].ToString();

        //                            strCateCode2         = dtCategory.Rows[j]["CategoryCode"].ToString();

        //                            for (int x = 0; x < dtCategory.Rows.Count; x++)
        //                            {
        //                                if (dtCategory.Rows[x]["LevelNo"].ToString() == "3")
        //                                {
        //                                    if (strCateCode2 == dtCategory.Rows[x]["ParentNo"].ToString())
        //                                    {
        //                                        //Level2.Expanded = true;

        //                                        row += 1;
        //                                        TreeListNode Level3 = tv_Location.AppendNode(row, Level2);
        //                                        Level3["Name"]      = dtCategory.Rows[x]["CategoryName"].ToString();

        //                                        strCateCode3         = dtCategory.Rows[x]["CategoryCode"].ToString();

        //                                        for (int y = 0; y < dtProduct.Rows.Count; y++)
        //                                        {
        //                                            if (strCateCode3 == dtProduct.Rows[y]["ProductCate"].ToString())
        //                                            {
        //                                                    //Level3.Expanded = true;

        //                                                row += 1;

        //                                                TreeListNode Level4   = tv_Location.AppendNode(row, Level3);
        //                                                Level4["Name"]        = dtProduct.Rows[y]["ProductDesc1"].ToString();
        //                                                Level4["ProductCode"] = dtProduct.Rows[y]["ProductCode"].ToString();                                                        
        //                                            }                                                   
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }                                
        //                }
        //            }
        //        }
        //    }

        //    tv_Location.SettingsSelection.Recursive = true;
        //}

        /// <summary>
        /// Click button new for add new data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_Add_Click(object sender, EventArgs e)
        //{
        //    DataRow drNew = dsStoreLct.Tables[strLct.TableName].NewRow();

        //    drNew["LocationCode"]  = DBNull.Value;
        //    drNew["LocationName"]  = DBNull.Value;
        //    drNew["DeliveryPoint"] = DBNull.Value;
        //    drNew["EOP"]           = DBNull.Value;
        //    drNew["CreatedDate"]   = ServerDateTime;
        //    drNew["CreatedBy"]     = LoginInfo.LoginName;
        //    drNew["UpdatedDate"]   = ServerDateTime;
        //    drNew["UpdatedBy"]     = LoginInfo.LoginName;

        //    // Add new row
        //    dsStoreLct.Tables[strLct.TableName].Rows.Add(drNew);

        //    // Set Lookup
        //    this.SetLookup();

        //    // Editing on new row
        //    grd_SL.DataSource = dsStoreLct.Tables[strLct.TableName];
        //    grd_SL.EditIndex  = grd_SL.Rows.Count;
        //    grd_SL.DataBind();

        //    btn_Add.Enabled     = false;
        //    tv_Location.Enabled = true;

        //    // Save changed to session
        //    Session["dsStoreLct"] = dsStoreLct;
        //}

        //protected void tv_Location_SelectionChanged(object sender, EventArgs e)
        //{
        //    //foreach (TreeListNode tl_Select in tv_Location.GetSelectedNodes())
        //    //{ 

        //    //}
        //}

        //protected void tv_Location_FocusedNodeChanged(object sender, EventArgs e)
        //{
        //    if (tv_Location.FocusedNode.GetValue("ProductCode") != null)
        //    { 
        //        string strPro         = tv_Location.FocusedNode.GetValue("ProductCode").ToString();            
        //        TextBox txt_StoreCode = (TextBox)grd_SL.Rows[grd_SL.EditIndex].FindControl("txt_StoreCode");

        //        if (tv_Location.FocusedNode.Selected)
        //        {
        //            DataRow drNew         = dsStoreLct.Tables[prodLoc.TableName].NewRow();
        //            drNew["LocationCode"] = txt_StoreCode.Text;
        //            drNew["ProductCode"]  = strPro.ToString();
        //            drNew["Onhand"]       = "0";

        //            dsStoreLct.Tables[prodLoc.TableName].Rows.Add(drNew);
        //        }
        //        else
        //        {                
        //            foreach (DataRow drSelect in dsStoreLct.Tables[prodLoc.TableName].Rows)
        //            {
        //                if(strPro.Equals(drSelect["ProductCode"].ToString()))
        //                {
        //                    if (drSelect["OnHand"].ToString() == "0")
        //                    {
        //                        drSelect.Delete();
        //                    }
        //                    else
        //                    {
        //                        tv_Location.FocusedNode.Selected = true;
        //                    }
        //                }                    
        //            }
        //        }

        //        // Save changed to session
        //        Session["dsStoreLct"] = dsStoreLct;
        //    }
        //}
    }
}