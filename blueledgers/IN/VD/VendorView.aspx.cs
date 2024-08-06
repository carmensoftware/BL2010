using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class VendorView : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListPageCuz.DataBind();
            }

            base.Page_Load(sender, e);
        }
    }
}