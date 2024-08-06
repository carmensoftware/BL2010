using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.CA
{
    public partial class CALst : BasePage
    {
        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListPage.DataBind();
            }

            base.Page_Load(sender, e);
        }
    }
}