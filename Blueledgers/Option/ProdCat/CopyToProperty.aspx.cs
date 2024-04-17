using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.ProdCat
{
    public partial class CopyToProperty : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        //private DataSet dsBu                        = new DataSet();
        private readonly DataSet dsCoppy = new DataSet();
        private readonly DataSet dsSave = new DataSet();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private string MsgError = string.Empty;
        private string buCode = string.Empty;
        private DataSet dsProdCat = new DataSet();

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(LoginInfo.BuInfo.BuCode);

            if (!IsPostBack)
            {
                Page_Retrieve();

                dsProdCat = (DataSet) Session["dsProdCat"];
            }
            else
            {
                dsProdCat = (DataSet) Session["dsProdCat"];
            }
        }

        /// <summary>
        ///     Get Product Category data.
        /// </summary>
        private void Page_Retrieve()
        {
            prodCat.GetList(dsProdCat, ref MsgError, hf_ConnStr.Value);
            var Get = bu.GetList(dsProdCat, LoginInfo.BuInfo.BuGrpCode);

            if (Get)
            {
                foreach (DataRow drProdCat in dsProdCat.Tables[bu.TableName].Rows)
                {
                    if (drProdCat.RowState != DataRowState.Deleted)
                    {
                        if (drProdCat["BuCode"].ToString().ToUpper() == LoginInfo.BuInfo.BuCode.ToUpper())
                        {
                            drProdCat.Delete();
                        }
                    }
                }
            }

            Session["dsProdCat"] = dsProdCat;

            Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            tl_Category.DataSource = dsProdCat.Tables[prodCat.TableName];
            tl_Category.DataBind();

            tl_Bu.DataSource = dsProdCat.Tables[bu.TableName];
            tl_Bu.DataBind();
        }

        protected void tl_Category_Load(object sender, EventArgs e)
        {
            tl_Category.DataSource = dsProdCat.Tables[prodCat.TableName];
            tl_Category.DataBind();
        }

        protected void tl_Bu_Load(object sender, EventArgs e)
        {
            tl_Bu.DataSource = dsProdCat.Tables[bu.TableName];
            tl_Bu.DataBind();
        }

        #region "Buttons"

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_ConfirmCoppy.ShowOnPageLoad = true;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
            pop_ConfirmCoppy.ShowOnPageLoad = false;

            tl_Bu.UnselectAll();
            tl_Category.UnselectAll();
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            var count = 0;

            var selectBuNodes = tl_Bu.GetSelectedNodes();

            //Check Category in each BU
            for (var i = 0; i < selectBuNodes.Count; i++)
            {
                hf_ConnStr.Value = bu.GetConnectionString(selectBuNodes[i].GetValue("BuCode").ToString());

                count = prodCat.GetCount(hf_ConnStr.Value);

                if (count == 9)
                {
                    if (buCode == string.Empty)
                    {
                        buCode = buCode + selectBuNodes[i].GetValue("BuName");
                    }
                    else
                    {
                        buCode = buCode + ", " + selectBuNodes[i].GetValue("BuName");
                    }
                }
            }

            if (buCode != string.Empty)
            {
                lbl_Warning.Text = "Cannot coppy to property because product category in " + buCode + " full.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            var selectCategoryNodes = tl_Category.GetSelectedNodes();

            for (var i = 0; i < selectBuNodes.Count; i++)
            {
                var parentSNo = 0;
                var parentINo = 0;
                var countSubCat = 0;
                var countItem = 0;
                count = 0;

                for (var j = 0; j < selectCategoryNodes.Count; j++)
                {
                    hf_ConnStr.Value = bu.GetConnectionString(selectBuNodes[i].GetValue("BuCode").ToString());
                    var catCode = 0;

                    //Get data from HQ
                    prodCat.GetListByCategoryCode(dsCoppy, selectCategoryNodes[j].GetValue("CategoryCode").ToString(),
                        LoginInfo.ConnStr);
                    prodCat.GetStructure(dsSave, hf_ConnStr.Value);

                    var drCoppy = dsCoppy.Tables[prodCat.TableName].Rows[j];

                    var drSave = dsSave.Tables[prodCat.TableName].NewRow();
                    drSave["LevelNo"] = drCoppy["LevelNo"];

                    if (int.Parse(drSave["LevelNo"].ToString()) == 1)
                    {
                        drSave["ParentNo"] = 0;
                        catCode =
                            prodCat.GetNewCategoryID(drCoppy["ParentNo"].ToString(), drSave["LevelNo"].ToString(),
                                hf_ConnStr.Value) + count;

                        if (catCode > 9)
                        {
                            lbl_Warning.Text = "Category Code cannot more than 9.";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }

                        drSave["CategoryCode"] = catCode;
                        parentSNo = catCode;
                        count++;
                    }
                    else if (int.Parse(drSave["LevelNo"].ToString()) == 2)
                    {
                        drSave["ParentNo"] = parentSNo;
                        catCode =
                            prodCat.GetNewCategoryID(drSave["ParentNo"].ToString(), drSave["LevelNo"].ToString(),
                                hf_ConnStr.Value) + countSubCat;

                        if (catCode > int.Parse(parentSNo + "9"))
                        {
                            lbl_Warning.Text = "Category Code cannot more than " + (catCode - countSubCat) + ".";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }

                        drSave["CategoryCode"] = catCode;
                        parentINo = catCode;
                        countSubCat++;
                    }
                    else if (int.Parse(drSave["LevelNo"].ToString()) == 3)
                    {
                        drSave["ParentNo"] = parentINo;
                        catCode =
                            prodCat.GetNewCategoryID(drSave["ParentNo"].ToString(), drSave["LevelNo"].ToString(),
                                hf_ConnStr.Value) + countItem;

                        if (catCode > int.Parse(parentINo + "99"))
                        {
                            lbl_Warning.Text = "Category Code cannot more than " + (catCode - countItem) + ".";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }

                        drSave["CategoryCode"] = catCode;
                        countItem++;
                    }

                    drSave["CategoryName"] = drCoppy["CategoryName"];
                    drSave["CategoryType"] = drCoppy["CategoryType"];
                    drSave["IsActive"] = drCoppy["IsActive"];
                    drSave["TaxAccCode"] = drCoppy["TaxAccCode"];
                    drSave["AuthRules"] = drCoppy["AuthRules"];
                    drSave["ApprovalLevel"] = drCoppy["ApprovalLevel"];
                    drSave["CreatedDate"] = ServerDateTime.Date;
                    drSave["CreatedBy"] = LoginInfo.LoginName;
                    drSave["UpdatedDate"] = ServerDateTime.Date;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;

                    dsSave.Tables[prodCat.TableName].Rows.Add(drSave);
                }

                prodCat.Save(dsSave, hf_ConnStr.Value);
                dsSave.Clear();
            }

            pop_ConfirmCoppy.ShowOnPageLoad = false;
            Response.Redirect("ProdCatLst.aspx");
            //hf_ConnStr.Value = bu.GetConnectionString(LoginInfo.BuInfo.BuCode);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProdCatLst.aspx");
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_ConfirmCoppy.ShowOnPageLoad = false;
        }

        #endregion
    }
}