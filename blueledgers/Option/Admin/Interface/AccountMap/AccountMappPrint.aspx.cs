using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMappPrint : BasePage
    {
        protected DataTable _dtDataPrint;

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _dtDataPrint = Session["AccountMappPrint"] as DataTable;
                Session.Remove("AccountMappPrint");
            }


        }
    }
}
