using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.StockLevel
{
    public partial class StockLev : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsstockLev = new DataSet();
        private readonly Blue.BL.Option.Inventory.Season season = new Blue.BL.Option.Inventory.Season();
        private readonly Blue.BL.Option.Inventory.StockLev stockLev = new Blue.BL.Option.Inventory.StockLev();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private DataSet dsStoreLct = new DataSet();
        private DataSet dsprodcat = new DataSet();
        private DataTable dtSeason = new DataTable();
        private DataTable dtStoreLocation = new DataTable();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        #endregion

        private void SetAspxTreeView()
        {
            //string strLevelNo = string.Empty;
            //string strCateCode = string.Empty;
            //string strCateCode2 = string.Empty;
            //string strCateCode3 = string.Empty;

            //DataTable dtCategory = dsStoreLct.Tables[prodCat.TableName];
            //DataTable dtProduct = dsStoreLct.Tables[product.TableName];

            //if (dtCategory != null)
            //{
            //    int row = 0;

            //    for (int i = 0; i < dtCategory.Rows.Count; i++)
            //    {
            //        strLevelNo = dtCategory.Rows[i]["LevelNo"].ToString();

            //        if (strLevelNo == "1")
            //        {
            //            row = (row == i ? i : row + 1);

            //            TreeListNode Level1 = tv_Product.AppendNode(row, null);
            //            Level1["Name"] = dtCategory.Rows[i]["CategoryName"].ToString();

            //            strCateCode = dtCategory.Rows[i]["CategoryCode"].ToString();

            //            for (int j = 0; j < dtCategory.Rows.Count; j++)
            //            {
            //                if (dtCategory.Rows[j]["LevelNo"].ToString() == "2")
            //                {
            //                    if (strCateCode == dtCategory.Rows[j]["ParentNo"].ToString())
            //                    {
            //                        Level1.Expanded = true;

            //                        row += 1;
            //                        TreeListNode Level2 = tv_Product.AppendNode(row, Level1);
            //                        Level2["Name"] = dtCategory.Rows[j]["CategoryName"].ToString();

            //                        strCateCode2 = dtCategory.Rows[j]["CategoryCode"].ToString();

            //                        for (int x = 0; x < dtCategory.Rows.Count; x++)
            //                        {
            //                            if (dtCategory.Rows[x]["LevelNo"].ToString() == "3")
            //                            {
            //                                if (strCateCode2 == dtCategory.Rows[x]["ParentNo"].ToString())
            //                                {
            //                                    Level2.Expanded = true;

            //                                    row += 1;
            //                                    TreeListNode Level3 = tv_Product.AppendNode(row, Level2);
            //                                    Level3["Name"] = dtCategory.Rows[x]["CategoryName"].ToString();

            //                                    strCateCode3 = dtCategory.Rows[x]["CategoryCode"].ToString();

            //                                    for (int y = 0; y < dtProduct.Rows.Count; y++)
            //                                    {
            //                                        if (strCateCode3 == dtProduct.Rows[y]["ProductSubCate"].ToString())
            //                                        {
            //                                            Level3.Expanded = true;

            //                                            row += 1;

            //                                            TreeListNode Level4 = tv_Product.AppendNode(row, Level3);
            //                                            Level4["Name"] = dtProduct.Rows[y]["ProductDesc1"].ToString();
            //                                            Level4["ProductCode"] = dtProduct.Rows[y]["ProductCode"].ToString();
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
            //}

            //tv_Product.SettingsSelection.Recursive = true;
        }

        #region "Operations"

        protected override void Page_Load(object sender, EventArgs e)
        {
            //base.Page_Load(sender, e);
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
                //tv_Location.SettingsSelection.Recursive = true;
            }
            else
            {
                dsStoreLct = (DataSet) Session["dsStoreLct"];
                SetAspxTreeView();
            }
        }

        private void Page_Retrieve()
        {
            //string MsgError = string.Empty;

            //bool getStockLevel = stockLev.GetList(dsStoreLct, ref MsgError, LoginInfo.ConnStr);

            //if (getStockLevel)
            //{

            //}
            //else
            //{
            //    // Display error message
            //    //lbl_Error.Text 
            //    string Error = Resources.MsgError.ResourceManager.GetString(MsgError);

            //    return;
            //}

            var getStockLev = stockLev.GetList(dsstockLev, Request.Params["ID"], LoginInfo.ConnStr);

            if (getStockLev)
            {
                Page_Setting();
            }

            Session["dsStoreLct"] = dsStoreLct;
        }

        private void Page_Setting()
        {
            // Page Title
            lbl_Title.Text = "Stock Level";

            SetLookup();

            //Grd_StockLevel.DataSource = dsstockLev.Tables[stockLev.TableName];
            //Grd_StockLevel.DataBind();

            grd_StockLevel1.DataSource = dsstockLev.Tables[stockLev.TableName];
            grd_StockLevel1.DataBind();
        }

        private void SetLookup()
        {
            dtStoreLocation = null;
            dtStoreLocation = storeLct.GetList(LoginInfo.ConnStr);

            // Insert Null row in first drop downlist
            //DataRow drStockLevel = dtStoreLocation.NewRow();
            //dtStoreLocation.Rows.InsertAt(drStockLevel, 0);

            //// Store Location
            //ddl_StoreLocation.DataSource = dtStoreLocation;
            //ddl_StoreLocation.DataTextField = "LocationName";
            //ddl_StoreLocation.DataValueField = "LocationCode";
            //ddl_StoreLocation.DataBind();

            dtSeason = null;
            dtSeason = season.GetList(LoginInfo.ConnStr);

            // DataRow drSeason = dtSeason.NewRow();
            // dtSeason.Rows.InsertAt(drSeason, 0);

            // Season
            ddl_Season.DataSource = dtSeason;
            ddl_Season.DataTextField = "SeasonName";
            ddl_Season.DataValueField = "SeasonCode";
            ddl_Season.DataBind();
        }

        //protected void ddl_StoreLocation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //tv_Product.Nodes.Clear();
        //    //string LocaCode = Request.Params["LocaCode"].ToString().ToUpper();
        //    string LocaCode = ddl_StoreLocation.SelectedItem.Value.ToString();

        //    prodCat.GetProdCateList1(dsStoreLct, LoginInfo.ConnStr);
        //    product.GetList(dsStoreLct, LoginInfo.ConnStr);
        //    prodLoc.GetList(dsStoreLct, LoginInfo.ConnStr);
        //    Session["dsStoreLct"] = dsStoreLct;

        //    tv_Product.ClearNodes();
        //    this.SetAspxTreeView();

        //    //TreeNode childNode = new TreeNode();

        //    //DataTable tableProdCategory = prodCat.GetList(LocaCode, LoginInfo.ConnStr);
        //    //int i = 0;

        //    //if (tableProdCategory != null)
        //    //{
        //    //    foreach (DataRow row in tableProdCategory.Rows)
        //    //    {
        //    //        TreeNode childNode_parent = new TreeNode();
        //    //        childNode_parent.Text = row[0].ToString() + " " + row[1].ToString();
        //    //        childNode_parent.ShowCheckBox = true;
        //    //        tv_Product.Nodes.Add(childNode_parent);

        //    //        if (row[0].ToString() != "")
        //    //        {
        //    //            string cateCode = row[0].ToString();
        //    //            int j = 0;

        //    //            DataTable tableProduct = prodLoc.GetProdName_LocaCode(LocaCode, cateCode, LoginInfo.ConnStr);

        //    //            if (tableProduct != null)
        //    //            {
        //    //                foreach (DataRow rowDiv in tableProduct.Rows)
        //    //                {
        //    //                    TreeNode childNode_Dep = new TreeNode();
        //    //                    childNode_Dep.Text = rowDiv[0].ToString() + " " + rowDiv[1].ToString();
        //    //                    childNode_Dep.ShowCheckBox = true;
        //    //                    tv_Product.Nodes[i].ChildNodes.AddAt(j, childNode_Dep);
        //    //                    j++;
        //    //                }
        //    //            }
        //    //        }

        //    //        i++;
        //    //    }
        //    //}
        //}

        protected void btn_ok_Click(object sender, EventArgs e)
        {
            // string strQOH = "";
            // string arr_Product = "";
            // int number = 0;

            // for (int i = 0; i < tv_Product.Nodes.Count; i++)
            // {
            //     for (int j = 0; j < tv_Product.Nodes[i].ChildNodes.Count; j++)
            //     {
            //         if (tv_Product.Nodes[i].Selected == false)
            //         {
            //             if (tv_Product.Nodes[i].ChildNodes[j].Selected == true)
            //             {
            //                 int x = tv_Product.Nodes[i].ChildNodes[j].ToString().IndexOf(' ');
            //                 // Get Onhand old concept
            //                 //DataTable tableProd = product.GetProductOnHand(ddl_StoreLocation.SelectedItem.Value.ToString(), 
            //                 //                                               tv_Product.Nodes[i].ChildNodes[j].Text.Substring(0, x), LoginInfo.ConnStr);
            //                 //int onhand = int.Parse(tableProd.Rows[0]["OnHand"].ToString());

            //                 //TODO: Get Onhand 
            //                 int onhand = 10;

            //                 if (number > 0)
            //                 {
            //                     arr_Product = arr_Product + "," + "'" + tv_Product.Nodes[i].ChildNodes[j].ToString().Substring(0, x) + "'";
            //                 }
            //                 else
            //                 {
            //                     arr_Product = "'" + tv_Product.Nodes[i].ChildNodes[j].ToString().Substring(0, x) + "'";
            //                 }
            //                 number++; 


            //                 // Get Reorder Point , ReStock level
            //                 //DataTable tablestock = stockLev.GetStocLevList(ddl_StoreLocation.SelectedItem.Value.ToString(),
            //                 //                                               tv_Product.Nodes[i].ChildNodes[j].ToString().Substring(0, x), LoginInfo.ConnStr);
            //                 //int StkReorderPoint = 0;
            //                 //int StkRestockLevel = 0;

            //                 //if (tablestock.Rows.Count > 0 )
            //                 //{
            //                 //     StkReorderPoint = int.Parse(tablestock.Rows[0]["StkReorderPoint"].ToString());
            //                 //     StkRestockLevel = int.Parse(tablestock.Rows[0]["StkRestockLevel"].ToString());
            //                 //}


            //                 // QOH
            //                 switch (int.Parse(ddl_QOHFilter.SelectedItem.Value))
            //                 {
            //                     // All Product
            //                     case 1:

            //                         strQOH = "";
            //                         break;

            //                     // OnHand < Reorder Point
            //                     case 2:
            //                         strQOH = "";
            //                         break;

            //                     // OnHand > ReStock Level  and  OnHand < ReOrder Point 
            //                     case 3:
            //                         strQOH = "";
            //                         break;

            //                     // OnHand > ReStock Level
            //                     case 4:
            //                         strQOH = "";
            //                         break;

            //                     // OnHand >= Reorder Point  and <= ReOrder Point
            //                     case 5:
            //                         strQOH = "";
            //                         break;
            //                 }
            //             }
            //         }
            //     }
            // }

            //// 1. Product
            //// arr_Product;

            // //2.Hide Zero  เว้นไว้ก่อนเพราะว่าต้องไปเช็คกับ Onhand
            //// chk_hideZero.Checked;


            // //3.QOH

            //// 4.Season


            // // GetList_ByCriteria with 4 parameter 
            // // 1. array Product  2.QOH 3.Season 4.Hide Zero 5. ConStr
            //// DataTable tbAllCriteria = stockLev.StockLev_GetListAllCriteria(arr_Product, strQOH , LoginInfo.ConnStr);

            // string MsgError = string.Empty;
            // bool IsTrue = stockLev.StockLev_GetListAllCriteria(dsstockLev,arr_Product, strQOH, ref MsgError, LoginInfo.ConnStr);

            // if (IsTrue == true)
            // {
            //     // Refresh  list
            //     //Grd_StockLevel.DataSource = dsstockLev.Tables[stockLev.TableName];
            //     //Grd_StockLevel.DataBind();

            //     // Save changed to session
            //     Session["dsstockLev"] = dsstockLev;               

            // }
        }

        protected void btn_edit_Click(object sender, EventArgs e)
        {
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            // Save changed to database.
            var MsgError = string.Empty;
            var saved = stockLev.Save(dsstockLev, ref MsgError, LoginInfo.ConnStr);

            if (saved)
            {
                //Response.Redirect("stocLev.aspx");
            }
        }

        protected void btn_order_Click(object sender, EventArgs e)
        {
            //TODO: Create Order
        }

        protected void btn_requisition_Click(object sender, EventArgs e)
        {
            //TODO: Create requisition
        }

        protected void Grd_StockLevel_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // Find deleting row.
            var drDeleting = dsstockLev.Tables[stockLev.TableName].Rows.Find(e.Keys[0]);
            drDeleting.Delete();

            // Save change to database.
            var MsgError = string.Empty;
            var saved = stockLev.Save(dsstockLev, ref MsgError, LoginInfo.ConnStr);
            if (saved)
            {
                Session["dsstockLev"] = dsstockLev;
                e.Cancel = true;
                //Grd_StockLevel.DataSource = dsstockLev;
                //Grd_StockLevel.DataBind();
            }
        }

        protected void Grd_StockLevel_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var ServerDateTimeTime = ServerDateTime;

            var drInserting = dsstockLev.Tables[stockLev.TableName].NewRow();
            drInserting["SeasonCode"] = e.NewValues["SeasonCode"];
            drInserting["SeasonName"] = e.NewValues["SeasonName"];

            drInserting["CreatedDate"] = ServerDateTimeTime;
            drInserting["CreatedBy"] = LoginInfo.LoginName;
            drInserting["UpdatedDate"] = ServerDateTimeTime;
            drInserting["UpdatedBy"] = LoginInfo.LoginName;
            dsstockLev.Tables[stockLev.TableName].Rows.Add(drInserting);

            // Save change to database.
            var MsgError = string.Empty;
            var saved = stockLev.Save(dsstockLev, ref MsgError, LoginInfo.ConnStr);
            if (saved)
            {
                Session["dsstockLev"] = dsstockLev;
                e.Cancel = true;
                //Grd_StockLevel.DataSource = dsstockLev;
                //Grd_StockLevel.CancelEdit();
                //Grd_StockLevel.DataBind();
            }
            else
            {
                e.Cancel = false;
            }
        }

        protected void Grd_StockLevel_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            // Find updating row.
            var drUpdating = dsstockLev.Tables[stockLev.TableName].Rows.Find(e.Keys[0]);
            drUpdating["SeasonName"] = e.NewValues["SeasonName"];

            drUpdating["UpdatedDate"] = ServerDateTime;
            drUpdating["UpdatedBy"] = LoginInfo.LoginName;

            // Save change to database.
            var MsgError = string.Empty;
            var saved = stockLev.Save(dsstockLev, ref MsgError, LoginInfo.ConnStr);

            if (saved)
            {
                Session["dsSeason"] = dsstockLev;
                e.Cancel = true;
                //Grd_StockLevel.DataSource = dsstockLev;
                //Grd_StockLevel.CancelEdit();
                //Grd_StockLevel.DataBind();
            }
            else
            {
                e.Cancel = false;
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("stockLevEdit.aspx?MODE=New");
                    break;

                case "EDIT":
                    Response.Redirect("stockLevEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"]);
                    break;

                case "DELETE":

                    //this.MessageBox("Are you sure wanted to delete !");

                    //dsProduct.Tables[product.TableName].Rows[0].Delete();

                    //bool result = product.Save(dsProduct, LoginInfo.ConnStr);

                    //if (result)
                    //{
                    //    Response.Redirect("ProdList.aspx");
                    //}
                    //else
                    //{
                    //    // show error message

                    //}

                    break;

                case "PRINT":
                    break;

                case "BACK":
                    //if (Request.Params["MODE"].ToUpper() == "EDIT")
                    //{
                    //    Response.Redirect("MLT.aspx?ID=" + Request.Params["ID"].ToString());
                    //}
                    //else if (Request.Params["MODE"].ToUpper() == "NEW")
                    //{
                    Response.Redirect("StockLevLst.aspx");
                    //}

                    break;
            }
        }

        #endregion
    }
}