using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.ProdCat
{
    public partial class ProdCatEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        private readonly Blue.BL.Option.Inventory.ProdCateType prodCateType =
            new Blue.BL.Option.Inventory.ProdCateType();

        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        private DataSet dsProdCatEdit = new DataSet();
        private string iGroupCode = string.Empty;
        private string iGroupName = string.Empty;

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
                dsProdCatEdit = (DataSet) Session["dsProdCatEdit"];
            }
            else
            {
                dsProdCatEdit = (DataSet) Session["dsProdCatEdit"];
            }
        }

        /// <summary>
        ///     Get Product Category data.
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var getProdCatSchema = prodCat.GetListByCategoryCode(dsProdCatEdit, string.Empty, LoginInfo.ConnStr);

                if (getProdCatSchema)
                {
                    // Assign Default Value for New Category.
                    var drNew = dsProdCatEdit.Tables[prodCat.TableName].NewRow();

                    if (Request.Params["LvNo"] == null)
                    {
                        drNew["LevelNo"] = 1;
                    }
                    else
                    {
                        drNew["LevelNo"] = int.Parse(Request.Params["LvNo"]);
                    }

                    drNew["IsActive"] = true;
                    drNew["CreatedDate"] = ServerDateTime;
                    drNew["CreatedBy"] = LoginInfo.LoginName;
                    drNew["UpdatedDate"] = ServerDateTime;
                    drNew["UpdatedBy"] = LoginInfo.LoginName;

                    dsProdCatEdit.Tables[prodCat.TableName].Rows.Add(drNew);
                }
            }
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                //Get list by CategoryCode
                prodCat.GetListByCategoryCode(dsProdCatEdit, Request.Params["ID"], LoginInfo.ConnStr);
            }

            Session["dsProdCatEdit"] = dsProdCatEdit;

            Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            var LevelNo = 1;

            if (Request.Params["LvNo"] != null)
            {
                LevelNo = int.Parse(Request.Params["LvNo"]);
            }

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                switch (LevelNo)
                {
                    case 1:
                        Panel1.Visible = true;
                        Panel2.Visible = false;
                        Panel3.Visible = false;

                        if (prodCat.ProdCat_GeLevelOneNewCategoryCode(LoginInfo.ConnStr) > 9)
                        {
                            txt_CatCode.Enabled = false;
                            txt_CatName.Enabled = false;
                            txt_TaxAccCode.Enabled = false;
                            spe_AppLv.Enabled = false;
                            ddl_CatType.Enabled = false;

                            //Set save button
                            menu_CmdBar.Items.FindByName("Save").Enabled = false;


                            pop_Warning.ShowOnPageLoad = true;
                            lbl_warning.Text = "No Additional Category can be created";
                        }
                        else
                        {
                            //Check Field's Number
                            var dsCheckField = new DataSet();
                            var parentNo = 0;
                            prodCat.GetListByParentNo(dsCheckField, parentNo.ToString(), LoginInfo.ConnStr);

                            //DataRow drList = dsCheckField.Tables[prodCat.TableName].Rows;

                            //Check Empty Field between created field
                            foreach (DataRow drList in dsCheckField.Tables[prodCat.TableName].Rows)
                            {
                                var SrNo = drList["SrNo"].ToString();
                                var SrNo1 = decimal.Parse(SrNo);
                                var CatNo = drList["CategoryCode"].ToString();
                                var CatNo1 = decimal.Parse(CatNo);

                                if (SrNo1 - CatNo1 != 0)
                                {
                                    txt_CatCode.Text = SrNo;
                                    break;
                                }
                            }

                            //Generate New ID for Category
                            if (txt_CatCode.Text == "")
                            {
                                txt_CatCode.Text =
                                    prodCat.ProdCat_GeLevelOneNewCategoryCode(LoginInfo.ConnStr).ToString();
                            }

                            txt_CatCode.Enabled = false;
                        }

                        break;

                    case 2:
                        Panel1.Visible = false;
                        Panel2.Visible = true;
                        Panel3.Visible = false;

                        txt_SubCatCode.Enabled = false;

                        break;

                    case 3:
                        Panel1.Visible = false;
                        Panel2.Visible = false;
                        Panel3.Visible = true;

                        txt_IGroupCode.Enabled = false;

                        break;
                }
            }
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                var drEdit = dsProdCatEdit.Tables[prodCat.TableName].Rows[0];

                switch (LevelNo)
                {
                    case 1:
                        Panel1.Visible = true;
                        Panel2.Visible = false;
                        Panel3.Visible = false;

                        txt_CatCode.Text = drEdit["CategoryCode"].ToString();
                        txt_CatCode.Enabled = false;
                        txt_CatName.Text = drEdit["CategoryName"].ToString();
                        txt_TaxAccCode.Text = drEdit["TaxAccCode"].ToString();
                        spe_AppLv.Value = drEdit["ApprovalLevel"] + " : " +
                                          apprLv.GetName(drEdit["ApprovalLevel"].ToString(), LoginInfo.ConnStr);
                        ddl_CatType.Value = drEdit["CategoryType"] + " : " +
                                            prodCateType.GetName(drEdit["CategoryType"].ToString(), LoginInfo.ConnStr);

                        break;

                    case 2:
                        Panel1.Visible = false;
                        Panel2.Visible = true;
                        Panel3.Visible = false;

                        ddl_SCatCode.Value = product.GetParentNoByCategoryCode(drEdit["CategoryCode"].ToString(),
                            LoginInfo.ConnStr);
                        ddl_SCatCode.Enabled = false;
                        txt_SubCatCode.Text = drEdit["CategoryCode"].ToString();
                        txt_SubCatCode.Enabled = false;
                        txt_SubCatName.Text = drEdit["CategoryName"].ToString();
                        txt_STaxAccCode.Text = drEdit["TaxAccCode"].ToString();
                        spe_SAppLv.Value = drEdit["ApprovalLevel"] + " : " +
                                           apprLv.GetName(drEdit["ApprovalLevel"].ToString(), LoginInfo.ConnStr);

                        break;

                    case 3:
                        Panel1.Visible = false;
                        Panel2.Visible = false;
                        Panel3.Visible = true;

                        ddl_ISubCatCode.DataSource = prodCat.GetList(LoginInfo.ConnStr);
                        ddl_ISubCatCode.DataBind();
                        ddl_ISubCatCode.Value = product.GetParentNoByCategoryCode(drEdit["CategoryCode"].ToString(),
                            LoginInfo.ConnStr);

                        ddl_ISubCatCode.Enabled = false;
                        ddl_ICatCode.Value = product.GetParentNoByCategoryCode(ddl_ISubCatCode.Value.ToString(),
                            LoginInfo.ConnStr);
                        ddl_ICatCode.Enabled = false;
                        txt_IGroupCode.Text = drEdit["CategoryCode"].ToString();
                        txt_IGroupCode.Enabled = false;
                        txt_IGroupName.Text = drEdit["CategoryName"].ToString();
                        txt_ITaxAccCode.Text = drEdit["TaxAccCode"].ToString();
                        spe_IAppLv.Value = drEdit["ApprovalLevel"] + " : " +
                                           apprLv.GetName(drEdit["ApprovalLevel"].ToString(), LoginInfo.ConnStr);

                        break;
                }
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            var LevelNo = 1;

            if (Request.Params["LvNo"] != null)
            {
                LevelNo = int.Parse(Request.Params["LvNo"]);
            }

            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        var drEdit = dsProdCatEdit.Tables[prodCat.TableName].Rows[0];

                        switch (int.Parse(Request.Params["LvNo"]))
                        {
                            case 1:
                                drEdit["CategoryName"] = txt_CatName.Text;
                                drEdit["TaxAccCode"] = txt_TaxAccCode.Text;
                                drEdit["ApprovalLevel"] = Convert.ToInt32(spe_AppLv.Value.ToString().Split(':')[0]);

                                var Get = prodCat.GetList(dsProdCatEdit, txt_CatCode.Text, LoginInfo.ConnStr);

                                if (Get)
                                {
                                    for (var i = 0; i < dsProdCatEdit.Tables[prodCat.TableName].Rows.Count; i++)
                                    {
                                        dsProdCatEdit.Tables[prodCat.TableName].Rows[i]["CategoryType"] =
                                            Convert.ToInt32(ddl_CatType.Value.ToString().Split(':')[0]);
                                    }
                                }

                                break;

                            case 2:
                                drEdit["CategoryName"] = txt_SubCatName.Text;
                                drEdit["TaxAccCode"] = txt_STaxAccCode.Text;
                                drEdit["ApprovalLevel"] = Convert.ToInt32(spe_SAppLv.Value.ToString().Split(':')[0]);

                                if (drEdit["CategoryType"].ToString() == string.Empty)
                                {
                                    drEdit["CategoryType"] = prodCat.GetCategoryType(drEdit["ParentNo"].ToString(),
                                        LoginInfo.ConnStr);
                                }

                                break;

                            case 3:
                                drEdit["CategoryName"] = txt_IGroupName.Text;
                                drEdit["TaxAccCode"] = txt_ITaxAccCode.Text;
                                drEdit["ApprovalLevel"] = Convert.ToInt32(spe_IAppLv.Value.ToString().Split(':')[0]);

                                if (drEdit["CategoryType"].ToString() == string.Empty)
                                {
                                    drEdit["CategoryType"] = prodCat.GetCategoryType(ddl_ICatCode.Value.ToString(),
                                        LoginInfo.ConnStr);
                                }

                                break;
                        }

                        var result = prodCat.Save(dsProdCatEdit, LoginInfo.ConnStr);

                        if (result)
                        {
                            Response.Redirect("ProdCat.aspx?ID=" + Request.Params["ID"] + "," + Request.Params["LvNo"]);
                        }
                    }

                    else if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        var drNew = dsProdCatEdit.Tables[prodCat.TableName].Rows[0];

                        switch (LevelNo)
                        {
                            case 1:
                                drNew["CategoryCode"] = txt_CatCode.Text;
                                if (txt_CatName.Text == "")
                                {
                                    pop_Warning.ShowOnPageLoad = true;
                                    lbl_warning.Text = "Please insert Category Name";
                                }
                                else
                                {
                                    drNew["CategoryName"] = txt_CatName.Text;
                                }
                                drNew["TaxAccCode"] = txt_TaxAccCode.Text;

                                var AppLv = spe_AppLv.Value.ToString();
                                var ApprSave = AppLv.Split(':');
                                //drNew["ApprovalLevel"]  = Convert.ToInt32(spe_AppLv.Value.ToString());
                                drNew["ApprovalLevel"] = Convert.ToInt32(ApprSave[0]);
                                drNew["ParentNo"] = "0";
                                drNew["CategoryType"] = Convert.ToInt32(ddl_CatType.Value.ToString().Split(':')[0]);

                                break;

                            case 2:

                                //Get parent number
                                var SCatCode_Value = ddl_SCatCode.Value.ToString();
                                var Value = SCatCode_Value.Split(':');

                                if (txt_SubCatCode.Text == "")
                                {
                                    pop_Warning.ShowOnPageLoad = true;
                                    lbl_warning.Text = "Please select Category";
                                }
                                else
                                {
                                    drNew["CategoryCode"] = txt_SubCatCode.Text;
                                }

                                if (txt_SubCatName.Text == "")
                                {
                                    pop_Warning.ShowOnPageLoad = true;
                                    lbl_warning.Text = "Please insert SubCategory Name";
                                }
                                else
                                {
                                    drNew["CategoryName"] = txt_SubCatName.Text;
                                }

                                drNew["TaxAccCode"] = txt_STaxAccCode.Text;

                                var SAppLv = spe_SAppLv.Value.ToString();
                                var SAppLvSave = SAppLv.Split(':');
                                drNew["ApprovalLevel"] = Convert.ToInt32(SAppLvSave[0]);
                                //drNew["ApprovalLevel"]  = Convert.ToInt32(spe_SAppLv.Value.ToString());
                                drNew["ParentNo"] = Value[0].Trim();
                                drNew["CategoryType"] = prodCat.GetCategoryType(Value[0], LoginInfo.ConnStr);
                                break;

                            case 3:

                                //Get parent number
                                var ISubCatCode_Value = ddl_ISubCatCode.Value.ToString();
                                Value = ISubCatCode_Value.Split(':');

                                if (txt_IGroupCode.Text == "")
                                {
                                    pop_Warning.ShowOnPageLoad = true;
                                    lbl_warning.Text = "Please select Category and SubCategory";
                                }
                                else
                                {
                                    drNew["CategoryCode"] = txt_IGroupCode.Text;
                                }

                                if (txt_IGroupName.Text == "")
                                {
                                    pop_Warning.ShowOnPageLoad = true;
                                    lbl_warning.Text = "Please insert Item Group Name";
                                }
                                else
                                {
                                    drNew["CategoryName"] = txt_IGroupName.Text;
                                }
                                drNew["TaxAccCode"] = txt_ITaxAccCode.Text;

                                var IAppLv = spe_IAppLv.Value.ToString();
                                var IAppLvSave = IAppLv.Split(':');
                                drNew["ApprovalLevel"] = Convert.ToInt32(IAppLvSave[0]);
                                //drNew["ApprovalLevel"]  = Convert.ToInt32(spe_IAppLv.Value.ToString());
                                drNew["ParentNo"] = Value[0].Trim();
                                drNew["CategoryType"] = prodCat.GetCategoryType(Value[0], LoginInfo.ConnStr);

                                break;
                        }

                        var result = prodCat.Save(dsProdCatEdit, LoginInfo.ConnStr);

                        if (result)
                        {
                            if (Request.Params["LvNo"] == null)
                            {
                                Response.Redirect("ProdCat.aspx?ID=" + txt_CatCode.Text.Trim() + ",1");
                            }
                            else if (int.Parse(Request.Params["LvNo"]) == 2)
                            {
                                Response.Redirect("ProdCat.aspx?ID=" + txt_SubCatCode.Text.Trim() + "," +
                                                  Request.Params["LvNo"]);
                            }
                            else if (int.Parse(Request.Params["LvNo"]) == 3)
                            {
                                Response.Redirect("ProdCat.aspx?ID=" + txt_IGroupCode.Text.Trim() + "," +
                                                  Request.Params["LvNo"]);
                            }
                        }
                    }

                    break;

                case "BACK":

                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("ProdCat.aspx?ID=" + Request.Params["ID"] + "," + Request.Params["LvNo"]);
                    }
                    else
                    {
                        Response.Redirect("ProdCatLst.aspx");
                    }

                    break;
            }
        }


        protected void ddl_ICatCode_ValueChanged(object sender, EventArgs e)
        {
            iGroupCode = txt_IGroupCode.Text;
            iGroupName = txt_IGroupName.Text;

            txt_IGroupCode.Text = string.Empty;
            txt_IGroupName.Text = string.Empty;
            txt_ITaxAccCode.Text = string.Empty;
            spe_IAppLv.Value = null;

            ddl_ISubCatCode.Value = ":";

            var ICatCode_Value = ddl_ICatCode.Value.ToString();
            var Value = ICatCode_Value.Split(':');

            if (Value[0] != "" && Value[0] != " ")
            {
                ddl_ISubCatCode.DataSource = prodCat.GetList(int.Parse(Value[0]), LoginInfo.ConnStr);
                ddl_ISubCatCode.TextField = "CategoryName";
                ddl_ISubCatCode.ValueField = "CategoryCode";
                ddl_ISubCatCode.DataBind();
            }
        }

        protected void ddl_ISubCatCode_ValueChanged(object sender, EventArgs e)
        {
            //iGroupCode = txt_IGroupCode.Text;
            //iGroupName = txt_IGroupName.Text;

            //set empty
            txt_IGroupName.Text = string.Empty;
            txt_ITaxAccCode.Text = string.Empty;
            spe_IAppLv.Value = null;

            var ISubCatCode_Value = ddl_ISubCatCode.Value.ToString();
            var Value = ISubCatCode_Value.Split(':');
            var chkOldTaxAcc = txt_ITaxAccCode.Text;
            //decimal chkOldAppLv      = spe_IAppLv.Number;
            //string chkOldAppLv      = spe_IAppLv.Value.ToString();
            var chkOldAppLv = (spe_IAppLv.Value != null ? spe_IAppLv.Value.ToString() : "0");

            if (Value[0] != "")
            {
                if (prodCat.IN_ProductCategory_GetNewItemGroupCode(Convert.ToInt32(Value[0]), LoginInfo.ConnStr) <=
                    int.Parse(Value[0] + "99"))
                {
                    //Check Field's Number
                    var dsCheckField = new DataSet();
                    prodCat.GetListByParentNo(dsCheckField, Value[0], LoginInfo.ConnStr);

                    foreach (DataRow drList in dsCheckField.Tables[prodCat.TableName].Rows)
                    {
                        var SrNo = drList["SrNo"].ToString();
                        var SrNo1 = decimal.Parse(SrNo) - 1;
                        var CatNo =
                            drList["CategoryCode"].ToString().Substring(drList["CategoryCode"].ToString().Length - 2, 2);
                        var CatNo1 = decimal.Parse(CatNo);

                        if (SrNo1 - CatNo1 != 0)
                        {
                            txt_IGroupCode.Text = drList["ParentNo"] + SrNo1.ToString().PadLeft(2, '0');
                            break;
                        }
                    }

                    if (txt_IGroupCode.Text == "")
                    {
                        txt_IGroupCode.Text =
                            prodCat.IN_ProductCategory_GetNewItemGroupCode(Convert.ToInt32(Value[0]), LoginInfo.ConnStr)
                                .ToString();
                    }


                    var strTaxNewValue = prodCat.ProdCat_GetTaxAccCodeByCategoryCode(int.Parse(Value[0]),
                        LoginInfo.ConnStr);

                    if (chkOldTaxAcc == string.Empty)
                    {
                        txt_ITaxAccCode.Text = strTaxNewValue;
                    }
                    else if (chkOldTaxAcc != strTaxNewValue)
                    {
                        txt_ITaxAccCode.Text = chkOldTaxAcc;
                    }

                    //decimal strAppNewValue = decimal.Parse(prodCat.ProdCat_GeApprovalLevelByCategoryCode(int.Parse(Value[0]), LoginInfo.ConnStr).ToString());
                    var strAppNewValue =
                        prodCat.ProdCat_GeApprovalLevelByCategoryCode((Value[0]), LoginInfo.ConnStr).ToString();

                    if (chkOldAppLv == "0")
                    {
                        //spe_IAppLv.Number = strAppNewValue;
                        spe_IAppLv.Value = strAppNewValue;
                    }
                    else if (chkOldAppLv != strAppNewValue)
                    {
                        spe_IAppLv.Value = chkOldAppLv;
                    }
                    txt_IGroupCode.Enabled = false;

                    if (iGroupCode == txt_IGroupCode.Text)
                    {
                        txt_IGroupName.Text = iGroupName;
                    }
                }
                else
                {
                    lbl_warning.Text = "No Additional Item Group can be created under this Sub Category";
                    pop_Warning.ShowOnPageLoad = true;
                }
            }
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;

            if (Request.Params["MODE"].ToUpper() == "NEW" && Request.Params["LvNo"] == null)
            {
                Response.Redirect("ProdCatLst.aspx");
            }
        }

        protected void ddl_SCatCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var subCatName = txt_SubCatName.Text;
            var subCatCode = txt_SubCatCode.Text;

            //set sub category code to empty
            txt_SubCatCode.Text = string.Empty;
            txt_SubCatName.Text = string.Empty;
            txt_STaxAccCode.Text = string.Empty;
            spe_SAppLv.Value = null;


            var SCatCode_Value = ddl_SCatCode.Value.ToString();
            var Value = SCatCode_Value.Split(':');
            var chkOldTaxAcc = txt_STaxAccCode.Text;
            //decimal chkOldAppLv     = spe_SAppLv.Number;
            var chkOldAppLv = (spe_SAppLv.Value != null ? spe_SAppLv.Value.ToString() : "0");


            if (Value[0] != "" && Value[0] != " ")
            {
                //if (prodCat.ProdCat_GeLevelTwoNewCategoryCode(Convert.ToInt32(Value[0]), LoginInfo.ConnStr) <= int.Parse(Value[0] + "9"))
                if (prodCat.ProdCat_GeLevelTwoNewCategoryCode(Convert.ToInt32(Value[0]), LoginInfo.ConnStr) <=
                    int.Parse(Convert.ToInt32(Value[0]) + "9"))
                {
                    //Check Field's Number
                    var dsCheckField = new DataSet();
                    //int parentNo = 0;
                    prodCat.GetListByParentNo(dsCheckField, Value[0], LoginInfo.ConnStr);

                    //DataRow drList = dsCheckField.Tables[prodCat.TableName].Rows;
                    foreach (DataRow drList in dsCheckField.Tables[prodCat.TableName].Rows)
                    {
                        var SrNo = drList["SrNo"].ToString();
                        var SrNo1 = decimal.Parse(SrNo);
                        var CatNo =
                            drList["CategoryCode"].ToString().Substring(drList["CategoryCode"].ToString().Length - 1, 1);
                        var CatNo1 = decimal.Parse(CatNo);

                        if (SrNo1 - 1 - CatNo1 != 0)
                        {
                            txt_SubCatCode.Text = drList["ParentNo"].ToString() + (SrNo1 - 1);
                            break;
                        }
                    }


                    if (txt_SubCatCode.Text == "")
                    {
                        txt_SubCatCode.Text =
                            prodCat.ProdCat_GeLevelTwoNewCategoryCode(Convert.ToInt32(Value[0]), LoginInfo.ConnStr)
                                .ToString();
                    }

                    var strTaxNewValue = prodCat.ProdCat_GetTaxAccCodeByCategoryCode(int.Parse(Value[0]),
                        LoginInfo.ConnStr);

                    if (chkOldTaxAcc == string.Empty)
                    {
                        txt_STaxAccCode.Text = strTaxNewValue;
                    }
                    else if (chkOldTaxAcc != strTaxNewValue)
                    {
                        txt_STaxAccCode.Text = chkOldTaxAcc;
                    }

                    //decimal strAppNewValue = decimal.Parse(prodCat.ProdCat_GeApprovalLevelByCategoryCode(int.Parse(Value[0]), LoginInfo.ConnStr).ToString());
                    var strAppNewValue =
                        prodCat.ProdCat_GeApprovalLevelByCategoryCode((Value[0]), LoginInfo.ConnStr).ToString();

                    if (chkOldAppLv == "0")
                    {
                        //spe_SAppLv.Number = strAppNewValue;
                        spe_SAppLv.Value = strAppNewValue;
                    }
                    else if (chkOldAppLv != strAppNewValue)
                    {
                        //spe_SAppLv.Number = chkOldAppLv;
                        spe_SAppLv.Value = chkOldAppLv;
                    }

                    txt_SubCatCode.Enabled = false;

                    if (subCatCode == txt_SubCatCode.Text)
                    {
                        txt_SubCatName.Text = subCatName;
                    }
                }
                else
                {
                    lbl_warning.Text = "No Additonal Sub Category can be created under this Category";
                    pop_Warning.ShowOnPageLoad = true;
                }
            }
        }
    }
}