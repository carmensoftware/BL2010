using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin
{
    public partial class Api : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
                Page_Setting();

        }

        private void Page_Setting()
        {
            var query = "SELECT AppName, BuCode, ClientID, ExpiryDate FROM BuAPI ORDER BY AppName, BuCode";
            var dt = bu.DbExecuteQuery(query, null);


           gv.DataSource = dt;
           gv.DataBind();
        }

    }


}