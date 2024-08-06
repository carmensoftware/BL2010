using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.MM
{
    public partial class MoveMentDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.IN.Movement mm = new Blue.BL.IN.Movement();
        private readonly Blue.BL.IN.MovementDt mmDt = new Blue.BL.IN.MovementDt();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();

        private DataSet dsMovementDt = new DataSet();
        private int index;
        private decimal totalQty;

        private string MmType
        {
            get { return Request.Params["ID"].Substring(5, 2); }
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
                dsMovementDt = (DataSet) Session["dsMovementDt"];
            }
        }

        private void Page_Retrieve()
        {
            mm.GetList(dsMovementDt, Request.Params["ID"], LoginInfo.ConnStr);
            mmDt.GetList(dsMovementDt, Request.Params["ID"], LoginInfo.ConnStr);

            Session["dsMovementDt"] = dsMovementDt;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drMm = dsMovementDt.Tables[mm.TableName].Rows[0];

            if (drMm["Status"].ToString().ToUpper() == "COMMITTED" || drMm["Status"].ToString().ToUpper() == "VOIDED")
            {
                menu_CmdBar.Items[1].Enabled = false; // Edit
                menu_CmdBar.Items[2].Enabled = false; // Void
            }

            lbl_CreatedDate.Text = DateTime.Parse(drMm["CreatedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_CreatedDate.ToolTip = lbl_CreatedDate.Text;
            lbl_Ref.Text = drMm["RefId"].ToString();
            lbl_Ref.ToolTip = lbl_Ref.Text;
            lbl_Status.Text = drMm["Status"].ToString();
            lbl_Status.ToolTip = lbl_Status.Text;
            lbl_CommittedDate.Text = drMm["CommittedDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drMm["CommittedDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_CommittedDate.ToolTip = lbl_CommittedDate.Text;
            //lbl_FromStore.Text = drMm["FromStoreId"].ToString();
            lbl_FromStoreName.Text = locat.GetName(drMm["FromStoreId"].ToString(), LoginInfo.ConnStr);
            lbl_FromStoreName.ToolTip = lbl_FromStoreName.Text;
            lbl_DeliveryDate.Text = drMm["DeliveryDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drMm["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_DeliveryDate.ToolTip = lbl_DeliveryDate.Text;
            lbl_Desc.Text = drMm["Description"].ToString();
            lbl_Desc.ToolTip = lbl_Desc.Text;
            lbl_Type.Text = adjType.GetName(drMm["TypeCode"].ToString(), LoginInfo.ConnStr);
            lbl_Type.ToolTip = lbl_Type.Text;
            //lbl_ToStore.Text = drMm["ToStoreId"].ToString();
            lbl_ToStoreName.Text = locat.GetName(drMm["ToStoreId"].ToString(), LoginInfo.ConnStr);
            lbl_ToStoreName.ToolTip = lbl_ToStoreName.Text;

            //Set visible control
            switch (MmType)
            {
                case "TO":
                    lbl_Type_HD.Visible = false;
                    lbl_Type.Visible = false;
                    lbl_DeliveryDate_HD.Visible = true;
                    lbl_DeliveryDate.Visible = true;
                    lbl_CommittedDate_HD.Visible = false;
                    lbl_CommittedDate_SIO.Visible = false;
                    menu_CmdBar.Items[0].Visible = false; // create
                    break;

                case "TI":
                    lbl_Type_HD.Visible = false;
                    lbl_Type.Visible = false;
                    lbl_DeliveryDate_HD.Visible = false;
                    lbl_DeliveryDate.Visible = false;
                    lbl_CommittedDate_HD.Visible = false;
                    lbl_CommittedDate_SIO.Visible = false;
                    td_DeliDate_HD.Visible = false;
                    td_DeliDate_lbl.Visible = false;
                    menu_CmdBar.Items[0].Visible = false; // create
                    break;

                default:
                    lbl_Type_HD.Visible = true;
                    lbl_Type.Visible = true;
                    lbl_FromStore_HD.Visible = false;
                    lbl_ToStore_HD.Visible = false;
                    //lbl_FromStore.Visible = false;
                    td_FromStoreName_HD.Visible = false;
                    td_FromStoreName.Visible = false;
                    //lbl_FromStoreName_HD.Visible = false;
                    lbl_FromStoreName.Visible = false;
                    tr_LastRow_HD.Visible = true;
                    tr_FromToStore_HD.Visible = false;
                    //tr_LastRow.Visible = false;
                    menu_CmdBar.Items[0].Visible = true; // create
                    td_DeliDate_HD.Visible = false;
                    td_DeliDate_lbl.Visible = false;
                    break;
            }

            grd_MovementDt.DataSource = dsMovementDt.Tables[mmDt.TableName];
            grd_MovementDt.DataBind();
        }

        protected void grd_MovementDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var p_HeaderTO = e.Row.FindControl("p_HeaderTO") as Panel;
                //Panel p_HeaderTI = e.Row.FindControl("p_HeaderTI") as Panel;
                var p_HeaderSO = e.Row.FindControl("p_HeaderSO") as Panel;
                var p_HeaderSI = e.Row.FindControl("p_HeaderSI") as Panel;
                var lbl_QtyAllocate_TO_HD = p_HeaderTO.FindControl("lbl_QtyAllocate_TO_HD") as Label;
                var lbl_QtyTransfer_TI_HD = p_HeaderTO.FindControl("lbl_QtyTransfer_TI_HD") as Label;
                var lbl_QtyOut_TO_HD = p_HeaderTO.FindControl("lbl_QtyOut_TO_HD") as Label;
                var lbl_QtyIn_TI_HD = p_HeaderTO.FindControl("lbl_QtyIn_TI_HD") as Label;


                if (MmType == "TO")
                {
                    p_HeaderTO.Visible = true;
                    //p_HeaderTI.Visible = false;
                    p_HeaderSO.Visible = false;
                    p_HeaderSI.Visible = false;
                    lbl_QtyTransfer_TI_HD.Visible = false;
                    lbl_QtyIn_TI_HD.Visible = false;
                }
                else if (MmType == "TI")
                {
                    p_HeaderTO.Visible = true;
                    //p_HeaderTI.Visible = true;
                    p_HeaderSO.Visible = false;
                    p_HeaderSI.Visible = false;
                    lbl_QtyAllocate_TO_HD.Visible = false;
                    lbl_QtyOut_TO_HD.Visible = false;
                }
                else if (MmType == "SO")
                {
                    p_HeaderTO.Visible = false;
                    //p_HeaderTI.Visible = false;
                    p_HeaderSO.Visible = true;
                    p_HeaderSI.Visible = false;
                }
                else
                {
                    p_HeaderTO.Visible = false;
                    //p_HeaderTI.Visible = false;
                    p_HeaderSO.Visible = false;
                    p_HeaderSI.Visible = true;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var p_ItemTO = e.Row.FindControl("p_ItemTO") as Panel;
                //Panel p_ItemTI = e.Row.FindControl("p_ItemTI") as Panel;
                var p_ItemSO = e.Row.FindControl("p_ItemSO") as Panel;
                var p_ItemSI = e.Row.FindControl("p_ItemSI") as Panel;
                var Img_Btn = e.Row.FindControl("Img_Btn") as ImageButton;

                //set CommandArgument For ImageButton
                if (Img_Btn != null)
                {
                    Img_Btn.CommandArgument = index.ToString();
                    index = index + 1;
                }

                if (MmType == "SI")
                {
                    p_ItemTO.Visible = false;
                    //p_ItemTI.Visible = false;
                    p_ItemSO.Visible = false;
                    p_ItemSI.Visible = true;

                    if (e.Row.FindControl("lbl_StoreCode_SI") != null)
                    {
                        var lbl_StoreCode_SI = e.Row.FindControl("lbl_StoreCode_SI") as Label;
                        lbl_StoreCode_SI.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                    }

                    if (e.Row.FindControl("lbl_StoreName_SI") != null)
                    {
                        var lbl_StoreName_SI = e.Row.FindControl("lbl_StoreName_SI") as Label;
                        lbl_StoreName_SI.Text = locat.GetName(
                            DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), LoginInfo.ConnStr);
                        lbl_StoreName_SI.ToolTip = lbl_StoreName_SI.Text;
                    }

                    if (e.Row.FindControl("lbl_ItemDesc_SI") != null)
                    {
                        var lbl_ItemDesc_SI = e.Row.FindControl("lbl_ItemDesc_SI") as Label;
                        lbl_ItemDesc_SI.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) +
                            " : " +
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                        lbl_ItemDesc_SI.ToolTip = lbl_ItemDesc_SI.Text;
                    }

                    if (e.Row.FindControl("lbl_ProductCode_SI") != null)
                    {
                        var lbl_ProductCode_SI = e.Row.FindControl("lbl_ProductCode_SI") as Label;
                        lbl_ProductCode_SI.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    }

                    if (e.Row.FindControl("lbl_EnglishName_SI") != null)
                    {
                        var lbl_EnglishName_SI = e.Row.FindControl("lbl_EnglishName_SI") as Label;
                        lbl_EnglishName_SI.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_LocalName_SI") != null)
                    {
                        var lbl_LocalName_SI = e.Row.FindControl("lbl_LocalName_SI") as Label;
                        lbl_LocalName_SI.Text =
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_Unit_SI") != null)
                    {
                        var lbl_Unit_SI = e.Row.FindControl("lbl_Unit_SI") as Label;
                        lbl_Unit_SI.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                        lbl_Unit_SI.ToolTip = lbl_Unit_SI.Text;
                    }

                    if (e.Row.FindControl("lbl_Qty_SI") != null)
                    {
                        var lbl_Qty_SI = e.Row.FindControl("lbl_Qty_SI") as Label;
                        lbl_Qty_SI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                        totalQty += Convert.ToDecimal(lbl_Qty_SI.Text);
                        lbl_Qty_SI.ToolTip = lbl_Qty_SI.Text;
                    }

                    if (e.Row.FindControl("lbl_UnitCost_SI") != null && MmType == "SI")
                    {
                        var lbl_UnitCost_SI = e.Row.FindControl("lbl_UnitCost_SI") as Label;
                        lbl_UnitCost_SI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "UnitCost"));
                        lbl_UnitCost_SI.ToolTip = lbl_UnitCost_SI.Text;
                    }
                }
                else if (MmType == "SO")
                {
                    p_ItemTO.Visible = false;
                    //p_ItemTI.Visible = false;
                    p_ItemSO.Visible = true;
                    p_ItemSI.Visible = false;

                    if (e.Row.FindControl("lbl_StoreCode_SO") != null)
                    {
                        var lbl_StoreCode_SO = e.Row.FindControl("lbl_StoreCode_SO") as Label;
                        lbl_StoreCode_SO.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString();
                    }

                    if (e.Row.FindControl("lbl_StoreName_SO") != null)
                    {
                        var lbl_StoreName_SO = e.Row.FindControl("lbl_StoreName_SO") as Label;
                        lbl_StoreName_SO.Text = locat.GetName(
                            DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_ItemDesc_SO") != null)
                    {
                        var lbl_ItemDesc_SO = e.Row.FindControl("lbl_ItemDesc_SO") as Label;
                        lbl_ItemDesc_SO.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) +
                            " : " +
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                        lbl_ItemDesc_SO.ToolTip = lbl_ItemDesc_SO.Text;
                    }

                    if (e.Row.FindControl("lbl_ProductCode_SO") != null)
                    {
                        var lbl_ProductCode_SO = e.Row.FindControl("lbl_ProductCode_SO") as Label;
                        lbl_ProductCode_SO.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    }

                    if (e.Row.FindControl("lbl_EnglishName_SO") != null)
                    {
                        var lbl_EnglishName_SO = e.Row.FindControl("lbl_EnglishName_SO") as Label;
                        lbl_EnglishName_SO.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_LocalName_SO") != null)
                    {
                        var lbl_LocalName_SO = e.Row.FindControl("lbl_LocalName_SO") as Label;
                        lbl_LocalName_SO.Text =
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_Unit_SO") != null)
                    {
                        var lbl_Unit_SO = e.Row.FindControl("lbl_Unit_SO") as Label;
                        lbl_Unit_SO.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    }

                    if (e.Row.FindControl("lbl_Qty_SO") != null)
                    {
                        var lbl_Qty_SO = e.Row.FindControl("lbl_Qty_SO") as Label;
                        lbl_Qty_SO.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                        totalQty += Convert.ToDecimal(lbl_Qty_SO.Text);
                    }
                }
                else
                {
                    p_ItemTO.Visible = true;
                    //p_ItemTI.Visible = true;
                    p_ItemSO.Visible = false;
                    p_ItemSI.Visible = false;

                    if (e.Row.FindControl("lbl_ProductCode_TO") != null)
                    {
                        var lbl_ProductCode_TO = e.Row.FindControl("lbl_ProductCode_TO") as Label;
                        lbl_ProductCode_TO.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    }

                    if (e.Row.FindControl("lbl_EnglishName_TO") != null)
                    {
                        var lbl_EnglishName_TO = e.Row.FindControl("lbl_EnglishName_TO") as Label;
                        lbl_EnglishName_TO.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_ItemDesc_TO") != null)
                    {
                        var lbl_ItemDesc_TO = e.Row.FindControl("lbl_ItemDesc_TO") as Label;
                        lbl_ItemDesc_TO.Text =
                            product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr) +
                            " : " +
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                        lbl_ItemDesc_TO.ToolTip = lbl_ItemDesc_TO.Text;
                    }

                    if (e.Row.FindControl("lbl_LocalName_TO") != null)
                    {
                        var lbl_LocalName_TO = e.Row.FindControl("lbl_LocalName_TO") as Label;
                        lbl_LocalName_TO.Text =
                            product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr);
                    }

                    if (e.Row.FindControl("lbl_Unit_TO") != null)
                    {
                        var lbl_Unit_TO = e.Row.FindControl("lbl_Unit_TO") as Label;
                        lbl_Unit_TO.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    }

                    if (MmType == "TO")
                    {
                        if (e.Row.FindControl("lbl_QtyAllocate_TO") != null && MmType == "TO")
                        {
                            var lbl_QtyAllocate_TO = e.Row.FindControl("lbl_QtyAllocate_TO") as Label;
                            lbl_QtyAllocate_TO.Text = String.Format("{0:0.00}",
                                DataBinder.Eval(e.Row.DataItem, "QtyAllocate"));
                            totalQty += Convert.ToDecimal(lbl_QtyAllocate_TO.Text);
                        }

                        if (e.Row.FindControl("lbl_QtyOut_TO") != null && MmType == "TO")
                        {
                            var lbl_QtyOut_TO = e.Row.FindControl("lbl_QtyOut_TO") as Label;
                            lbl_QtyOut_TO.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                        }
                    }
                    else
                    {
                        if (e.Row.FindControl("lbl_QtyTransfer_TI") != null && MmType == "TI")
                        {
                            var lbl_QtyTransfer_TI = e.Row.FindControl("lbl_QtyTransfer_TI") as Label;
                            lbl_QtyTransfer_TI.Text = String.Format("{0:0.00}",
                                mmDt.GetQty(DataBinder.Eval(e.Row.DataItem, "TrfOutId").ToString(),
                                    DataBinder.Eval(e.Row.DataItem, "TrfOutDtId").ToString(), LoginInfo.ConnStr));
                            totalQty += Convert.ToDecimal(lbl_QtyTransfer_TI.Text);
                        }

                        if (e.Row.FindControl("lbl_QtyIn_TI") != null && MmType == "TI")
                        {
                            var lbl_QtyIn_TI = e.Row.FindControl("lbl_QtyIn_TI") as Label;
                            lbl_QtyIn_TI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                        }
                    }


                    //DataSet dsTrfOutDt = new DataSet();

                    ////p_ItemTO.Visible = false;
                    //p_ItemTI.Visible = true;
                    //p_ItemSO.Visible = false;
                    //p_ItemSI.Visible = false;

                    //if (e.Row.FindControl("lbl_ProductCode_TI") != null)
                    //{
                    //    Label lbl_ProductCode_TI = e.Row.FindControl("lbl_ProductCode_TI") as Label;
                    //    lbl_ProductCode_TI.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    //}

                    //if (e.Row.FindControl("lbl_EnglishName_TI") != null)
                    //{
                    //    Label lbl_EnglishName_TI = e.Row.FindControl("lbl_EnglishName_TI") as Label;
                    //    lbl_EnglishName_TI.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    //}

                    //if (e.Row.FindControl("lbl_LocalName_TI") != null)
                    //{
                    //    Label lbl_LocalName_TI = e.Row.FindControl("lbl_LocalName_TI") as Label;
                    //    lbl_LocalName_TI.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                    //}

                    //if (e.Row.FindControl("lbl_Unit_TI") != null)
                    //{
                    //    Label lbl_Unit_TI = e.Row.FindControl("lbl_Unit_TI") as Label;
                    //    lbl_Unit_TI.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    //}

                    //if (e.Row.FindControl("lbl_QtyTransfer_TI") != null)
                    //{
                    //    Label lbl_QtyTransfer_TI = e.Row.FindControl("lbl_QtyTransfer_TI") as Label;
                    //    //lbl_QtyTransfer_TI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "QtyAllocate"));
                    //}

                    //if (e.Row.FindControl("lbl_QtyIn_TI") != null)
                    //{
                    //    Label lbl_QtyIn_TI = e.Row.FindControl("lbl_QtyIn_TI") as Label;
                    //    lbl_QtyIn_TI.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "Qty"));
                    //}
                }

                //Expand

                var lbl_Debit = e.Row.FindControl("lbl_Debit") as Label;
                var lbl_Credit = e.Row.FindControl("lbl_Credit") as Label;
                var lbl_DebitName = e.Row.FindControl("lbl_DebitName") as Label;
                var lbl_CreditName = e.Row.FindControl("lbl_CreditName") as Label;
                var lbl_Category = e.Row.FindControl("lbl_Category") as Label;
                var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                var lbl_BaseUnit = e.Row.FindControl("lbl_BaseUnit") as Label;
                var lbl_SubCate = e.Row.FindControl("lbl_SubCate") as Label;
                var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                var lbl_ItemGroup = e.Row.FindControl("lbl_ItemGroup") as Label;
                var lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                var lbl_BarCode = e.Row.FindControl("lbl_BarCode") as Label;
                var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;

                lbl_Category.Text =
                    prodCat.GetName(
                        product.GetParentNoByCategoryCode(
                            product.GetParentNoByCategoryCode(
                                product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                lbl_Category.ToolTip = lbl_Category.Text;

                lbl_SubCate.Text =
                    prodCat.GetName(
                        product.GetParentNoByCategoryCode(
                            product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                lbl_SubCate.ToolTip = lbl_SubCate.Text;

                lbl_ItemGroup.Text =
                    prodCat.GetName(
                        product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                            LoginInfo.ConnStr), LoginInfo.ConnStr);
                lbl_ItemGroup.ToolTip = lbl_ItemGroup.Text;

                lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                lbl_ProductCode.ToolTip = lbl_ProductCode.Text;

                lbl_BaseUnit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                lbl_BaseUnit.ToolTip = lbl_BaseUnit.Text;

                lbl_EnglishName.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    LoginInfo.ConnStr);
                lbl_EnglishName.ToolTip = lbl_EnglishName.Text;

                lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    LoginInfo.ConnStr);
                lbl_LocalName.ToolTip = lbl_LocalName.Text;


                if (lbl_Debit != null)
                {
                    lbl_Debit.Text = DataBinder.Eval(e.Row.DataItem, "DebitAcc").ToString();
                    lbl_Debit.ToolTip = lbl_Debit.Text;
                }

                if (lbl_DebitName != null)
                {
                }

                if (lbl_Credit != null)
                {
                    lbl_Credit.Text = DataBinder.Eval(e.Row.DataItem, "CreditAcc").ToString();
                    lbl_Credit.ToolTip = lbl_Credit.Text;
                }

                if (lbl_CreditName != null)
                {
                }

                if (lbl_Comment != null)
                {
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment.ToolTip = lbl_Comment.Text;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var p_FooterTrf = e.Row.FindControl("p_FooterTrf") as Panel;
                var p_FooterSI = e.Row.FindControl("p_FooterSI") as Panel;
                var p_FooterSO = e.Row.FindControl("p_FooterSO") as Panel;

                if (MmType == "TO" || MmType == "TI")
                {
                    p_FooterSI.Visible = false;
                    p_FooterSO.Visible = false;

                    if (e.Row.FindControl("lbl_TrfTotalQty") != null)
                    {
                        var lbl_TrfTotalQty = e.Row.FindControl("lbl_TrfTotalQty") as Label;
                        lbl_TrfTotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
                else if (MmType == "SI")
                {
                    p_FooterTrf.Visible = false;
                    p_FooterSO.Visible = false;

                    if (e.Row.FindControl("lbl_SITotalQty") != null)
                    {
                        var lbl_SITotalQty = e.Row.FindControl("lbl_SITotalQty") as Label;
                        lbl_SITotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
                else
                {
                    p_FooterTrf.Visible = false;
                    p_FooterSI.Visible = false;

                    if (e.Row.FindControl("lbl_SOTotalQty") != null)
                    {
                        var lbl_SOTotalQty = e.Row.FindControl("lbl_SOTotalQty") as Label;
                        lbl_SOTotalQty.Text = String.Format("{0:0.00}", totalQty);
                    }
                }
            }
        }

        protected void Back()
        {
            Response.Redirect("MoveMentLst.aspx");
        }

        protected void Edit()
        {
            Response.Redirect("MoveMentEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"]
                              + "&MODE=Edit&Prefix=" + MmType + "&VID=" + Request.Params["VID"]);
        }

        protected void Create()
        {
            Response.Redirect("MoveMentEdit.aspx?MODE=New&BuCode=" + Request.Params["BuCode"] + "&Prefix=" + MmType +
                              "&VID=" + Request.Params["VID"]);
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            //switch (MmType)
            //{
            //    case "TO":
            var drVoid = dsMovementDt.Tables[mm.TableName].Rows[0];
            drVoid["Status"] = "Voided";

            var Save = mm.Save(dsMovementDt, LoginInfo.ConnStr);

            if (Save)
            {
                pop_ConfirmVoid.ShowOnPageLoad = false;
                Response.Redirect("MoveMentLst.aspx");
            }
            //        break;
            //    case "TI":
            //        break;
            //    case "SO":
            //        break;
            //    case "SI":
            //        break;
            //}
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void Void()
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;
                case "EDIT":
                    Edit();
                    break;
                case "VOID":
                    Void();
                    break;
                case "PRINT":

                    break;
                case "BACK":
                    Back();
                    break;
            }
        }
    }
}