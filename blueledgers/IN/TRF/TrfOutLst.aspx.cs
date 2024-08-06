using System;
using System.Collections;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfOutLst : BasePage
    {
        #region "Attributes"

        //BL.IN.StandardRequistion stdReq             = new BL.IN.StandardRequistion();
        //BL.IN.StandardRequisitionDetail stdReqDt    = new BL.IN.StandardRequisitionDetail();
        //BL.IN.storeRequisition storeReq = new BL.IN.storeRequisition();
        //BL.IN.StoreRequisitionDetail storeReqDt = new BL.IN.StoreRequisitionDetail();
        //DataSet dsStoreReq                          = new DataSet();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!Page.IsPostBack)
            {
                ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create from Store Requisition",
                    "CFS"));

                ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Transfer Out List", "SL"));


                ListPage2.DataBind();
            }

            ListPage2.CreateItems.Menu.ItemClick += menu_ItemClick;
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CFS":
                    Response.Redirect("TrfOutCreate.aspx");
                    break;
                case "SL":

                    var objArrList = new ArrayList();
                    objArrList.Add(ListPage2.ListKeys);
                    Session["s_arrNo"] = objArrList;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=325";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }
    }
}