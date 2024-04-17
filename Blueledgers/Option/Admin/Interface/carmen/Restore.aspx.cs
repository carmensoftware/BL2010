using System;
using System.Collections;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.carmen
{
    public partial class Restore : BasePage
    {
        private readonly Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();
            }
        }

        private void Page_Setting()
        {
            //Default FFDate and TTDate
            txt_FromDate.Date = ServerDateTime.Date;
            txt_ToDate.Date = ServerDateTime.Date;
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            ViewRestore();
        }

        protected void btn_Restore_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = true;
        }

        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = false;
            UpdateRestoreStatus();
            ViewRestore();
        }

        /// <summary>
        ///     Update export status to true/false
        /// </summary>
        /// <param name="exportStatus"></param>
        private void UpdateRestoreStatus()
        {
            var dbHandler = new Blue.DAL.DbHandler();
            var dbParams = new Blue.DAL.DbParameter[2];

            var cmd =
                " UPDATE [PC].[REC] SET ExportStatus = 'False' WHERE RecDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND RecDate <= DATEADD(s,86399,@ToDate) " +
                " UPDATE [PC].[RECDt] SET ExportStatus = 'False' WHERE RecNo in (SELECT RecNo FROM [PC].[REC] WHERE ExportStatus = 'False' AND RecDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND RecDate <= DATEADD(s,86399,@ToDate)) " +
                " UPDATE [PC].[CN] SET ExportStatus = 'False' WHERE CnDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND CnDate <= DATEADD(s,86399,@ToDate)";

            dbParams[0] = new Blue.DAL.DbParameter("@FromDate", txt_FromDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new Blue.DAL.DbParameter("@ToDate", txt_ToDate.Date.ToString("yyyy-MM-dd"));
            //dbParams[2] = new DAL.DbParameter("@StoreGrp", ddl_StoreGrp.SelectedItem.Value);

            //dbHandler.DbExecuteNonQuery(cmd, dbParams, LoginInfo.ConnStr);
            dbHandler.DbExecuteQuery(cmd, dbParams, LoginInfo.ConnStr);
        }

        private void ViewRestore()
        {
            grd_Preview.DataSource = accountMapp.GetPreviewRestore(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);
            grd_Preview.DataBind();
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = false;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "PRINT":
                        var objArrList = new ArrayList();
                        objArrList.Add(txt_FromDate.Value);
                        objArrList.Add(txt_ToDate.Value);
                        Session["s_arrNo"] = objArrList;

                        var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=170" + "&BuCode=" +
                                         LoginInfo.BuInfo.BuCode;
                        Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("<script>");
                        //Response.Write("window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("</script>");
                        break;
                }
            }
        }
    }
}