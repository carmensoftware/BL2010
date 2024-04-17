using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.ProdCat
{
    public partial class ProdCat : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Admin.Interface.AccountMapp AccMapp =
            new Blue.BL.Option.Admin.Interface.AccountMapp();

        private readonly Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();

        private readonly DataSet dsProdCat = new DataSet();
        private readonly DataSet dsProduct = new DataSet();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        private readonly Blue.BL.Option.Inventory.ProdCateType prodCateType =
            new Blue.BL.Option.Inventory.ProdCateType();

        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private DataSet dsAccMapp = new DataSet();

        #endregion

        #region "Operations"

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
        }

        private void Page_Retrieve()
        {
            //Get list by Product Category Code
            var ProdCatID = Request.Params["ID"];
            var prodcatID = ProdCatID.Split(',');

            prodCat.GetListByCategoryCode(dsProdCat, prodcatID[0], LoginInfo.ConnStr);
            Session["dsProdCat"] = dsProdCat;

            Page_Setting();
        }

        private void Page_Setting()
        {
            //if (LoginInfo.BuInfo.BuCode.ToUpper() == "IBIS_ICC")
            //{
            //    menu_CmdBar.Items.FindByName("Coppy").Visible = true;    
            //}

            var drProdCat = dsProdCat.Tables[prodCat.TableName].Rows[0];

            var ProdCatID = Request.Params["ID"];
            var prodcatID = ProdCatID.Split(',');
            switch (prodcatID[1])
            {
                case "1":

                    Panel1.Visible = true;
                    Panel2.Visible = false;
                    Panel3.Visible = false;

                    lbl_CatCode.Text = drProdCat["CategoryCode"].ToString();
                    lbl_CatName.Text = drProdCat["CategoryName"].ToString();
                    lbl_TaxAccCode.Text = drProdCat["TaxAccCode"].ToString();
                    lbl_AppLv.Text = drProdCat["ApprovalLevel"] + " : " +
                                     apprLv.GetName(drProdCat["ApprovalLevel"].ToString(), LoginInfo.ConnStr);
                    lbl_CateType.Text = drProdCat["CategoryType"] + " : " +
                                        prodCateType.GetName(drProdCat["CategoryType"].ToString(), LoginInfo.ConnStr);

                    break;
                case "2":
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    Panel3.Visible = false;

                    lbl_SCatCode.Text = drProdCat["ParentNo"] + " " +
                                        prodCat.GetName(drProdCat["ParentNo"].ToString(), LoginInfo.ConnStr);
                    lbl_SubCatCode.Text = drProdCat["CategoryCode"].ToString();
                    lbl_SubCatName.Text = drProdCat["CategoryName"].ToString();
                    lbl_STaxAccCode.Text = drProdCat["TaxAccCode"].ToString();
                    lbl_SAppLv.Text = drProdCat["ApprovalLevel"] + " : " +
                                      apprLv.GetName(drProdCat["ApprovalLevel"].ToString(), LoginInfo.ConnStr);

                    break;
                case "3":
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                    Panel3.Visible = true;

                    lbl_ICatCode.Text =
                        product.GetParentNoByCategoryCode(drProdCat["ParentNo"].ToString(), LoginInfo.ConnStr) + " " +
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(drProdCat["ParentNo"].ToString(), LoginInfo.ConnStr),
                            LoginInfo.ConnStr);
                    lbl_ISubCatCode.Text = drProdCat["ParentNo"] + " " +
                                           prodCat.GetName(drProdCat["ParentNo"].ToString(), LoginInfo.ConnStr);
                    lbl_IGroupCode.Text = drProdCat["CategoryCode"].ToString();
                    lbl_IGroupName.Text = drProdCat["CategoryName"].ToString();
                    lbl_ITaxAccCode.Text = drProdCat["TaxAccCode"].ToString();
                    lbl_IAppLv.Text = drProdCat["ApprovalLevel"] + " : " +
                                      apprLv.GetName(drProdCat["ApprovalLevel"].ToString(), LoginInfo.ConnStr);

                    break;
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ProdCatEdit.aspx?MODE=New");
                    break;

                case "SC":
                    Response.Redirect("ProdCatEdit.aspx?MODE=New&LvNo=2");
                    break;

                case "IG":
                    Response.Redirect("ProdCatEdit.aspx?MODE=New&LvNo=3");
                    break;

                case "EDIT":
                    var ProdCatID = Request.Params["ID"];
                    var prodcatID = ProdCatID.Split(',');
                    Response.Redirect("ProdCatEdit.aspx?MODE=Edit&LvNo=" + prodcatID[1] + "&ID=" + prodcatID[0]);
                    break;

                case "DELETE":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    break;

                case "BACK":
                    Response.Redirect("ProdCatLst.aspx");
                    break;

                case "COPPY":
                    Response.Redirect("CoppyToProperty.aspx");
                    break;
            }
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var ProductCate = Request.Params["ID"];
            var productCate = ProductCate.Split(',');

            var Child = prodCat.GetChild(productCate[0], LoginInfo.ConnStr);

            if (int.Parse(Child) == 1)
            {
                var count = 0;

                product.GetList(dsProduct, LoginInfo.ConnStr);

                foreach (DataRow drDeleting in dsProduct.Tables[product.TableName].Rows)
                {
                    if (drDeleting["ProductCate"].ToString() == productCate[0])
                    {
                        count = count + 1;
                    }
                }

                foreach (DataRow drDeleting in AccMapp.GetList(LoginInfo.ConnStr).Rows)
                {
                    if (drDeleting["CategoryCode"].ToString() == productCate[0])
                    {
                        count = count + 1;
                    }
                    else if (drDeleting["SubCategoryCode"].ToString() == productCate[0])
                    {
                        count = count + 1;
                    }
                    else if (drDeleting["ItemGroupCode"].ToString() == productCate[0])
                    {
                        count = count + 1;
                    }
                }

                if (count > 0)
                {
                    pop_ConfrimDelete.ShowOnPageLoad = false;
                    pop_WarningDelete.ShowOnPageLoad = true;
                }
                else
                {
                    prodCat.GetListByCategoryCode(dsProdCat, productCate[0], LoginInfo.ConnStr);
                    var drProdCat = dsProdCat.Tables[prodCat.TableName].Rows[0];
                    drProdCat.Delete();

                    // Save to database
                    var Save = prodCat.Save(dsProdCat, LoginInfo.ConnStr);

                    if (Save)
                    {
                        pop_ConfrimDelete.ShowOnPageLoad = false;

                        //this.Page_Retrieve();
                    }

                    Response.Redirect("ProdCatLst.aspx");
                }
            }
            else
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;
                pop_WarningDelete.ShowOnPageLoad = true;
            }
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_WarningDelete.ShowOnPageLoad = false;
            Response.Redirect("ProdCatLst.aspx");
        }
    }

    #endregion
}