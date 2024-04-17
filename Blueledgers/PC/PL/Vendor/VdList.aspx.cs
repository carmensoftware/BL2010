using System;
using System.Text;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;


namespace BlueLedger.PL.PC.PL.Vendor
{
    public partial class VdList : BasePage
    {
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.9.1";

        protected override void Page_Load(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            if (!IsPostBack)
            {

                ListPage.CreateItems.NavigateUrl = "~/PC/PL/Vendor/VdList.aspx";
                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Manually", "Manually"));
                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create by Vendor", "ByVendor"));

                //ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Price List", "PLV"));
                //ListPage.DataBind();

                //// Added on: 27/09/2017, By: Fon
                ListPage.menuItems.Add("Export", "EX");
                ListPage.menuItems.FindByName("EX").ItemStyle.HoverStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8D8D8");
                ListPage.menuItems.FindByName("EX").ItemStyle.ForeColor = System.Drawing.Color.White;
                ListPage.menuItems.FindByName("EX").ItemStyle.Font.Size = 7;
                ListPage.menuItems.FindByName("EX").ItemStyle.VerticalAlign = VerticalAlign.Bottom;

                ListPage.menuItems.Add("Import", "IM");
                ListPage.menuItems.FindByName("IM").ItemStyle.HoverStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8D8D8");
                ListPage.menuItems.FindByName("IM").ItemStyle.ForeColor = System.Drawing.Color.White;
                ListPage.menuItems.FindByName("IM").ItemStyle.Font.Size = 7;
                ListPage.menuItems.FindByName("IM").ItemStyle.VerticalAlign = VerticalAlign.Bottom;
                ListPage.DataBind();

                ListPage.menuItems.FindByName("EX").VisibleIndex = 0;
                ListPage.menuItems.FindByName("IM").VisibleIndex = 1;
                // End Added.

                ddl_vendor.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                ddl_vendor.SelectedIndex = 0;
            }

            Control_HeaderMenuBar();
            ListPage.CreateItems.Menu.ItemClick += menu_ItemClick;

            base.Page_Load(sender, e);
        }

        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage.CreateItems.Visible = (pagePermiss >= 3) ? ListPage.CreateItems.Visible : false;
            ListPage.menuItems.FindByName("IM").Visible = (pagePermiss >= 3) ? ListPage.menuItems.FindByName("IM").Visible : false;
            ListPage.menuItems.FindByName("EX").Visible = (pagePermiss >= 3) ? ListPage.menuItems.FindByName("EX").Visible : false;
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "MANUALLY":
                    Response.Redirect("VdEdit.aspx?MODE=new");
                    break;

                case "BYVENDOR":
                    popup_vendor.ShowOnPageLoad = true;
                    break;

                case "PLV":
                    Session["dtBuKeys"] = ListPage.dtBuKeys;

                    var reportLink = "../../../RPT/ReportCriteria.aspx?category=001&reportid=147";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;

                case "IM":
                    Response.Redirect("~/PC/PL/ByVdImp.aspx");
                    break;

                case "EX":
                    Response.Redirect("~/PC/PL/ByVdExp.aspx");
                    break;
            }
        }

        private String GetArrPLVNo()
        {
            var sb = new StringBuilder();


            var grdd = (GridView)ListPage.FindControl("grd_BU");
            var grdTran = (GridView)grdd.Rows[0].FindControl("grd_Trans");


            for (var i = 0; i < grdTran.Rows.Count; i++)
            {
                var prId = grdTran.Rows[i].Cells[2].Text;

                sb.Append("'" + prId + "',");
            }
            if (sb.Length > 0)
            {
                return sb.ToString().Substring(0, sb.Length - 1);
            }
            return "'*'";
        }

        #region "PopUpChooseVendor"
        protected void btn_OK_Click(object sender, EventArgs e)
        {
            //if (ddl_Vendor.Value != null)
            //{
            //    //ddl_Vendor.SelectedItem.Value = get hold line
            //    string[] vendorCode = ddl_Vendor.Value.ToString().Split(':');
            //    Response.Redirect("VdEdit.aspx?MODE=new&VENDOR=" + vendorCode[0].Trim());
            //}
            if (ddl_vendor.SelectedValue != null)
                Response.Redirect("VdEdit.aspx?MODE=new&VENDOR=" + ddl_vendor.SelectedValue.ToString());

        }
        #endregion

    }
}