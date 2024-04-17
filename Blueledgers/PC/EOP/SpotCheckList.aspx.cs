using System;

using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.EOP
{
    public partial class SpotCheckList : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListPage.DataBind();

                ListPage.CreateItems.NavigateUrl = "~/PC/EOP/EOPEdit.aspx?MODE=new&BuCode=" +
                                                   LoginInfo.BuInfo.BuCode + "&VID=" +
                                                   Request.Cookies["[IN].[vEOPList]"].Value;
            }

            base.Page_Load(sender, e);
        }
    }
}
