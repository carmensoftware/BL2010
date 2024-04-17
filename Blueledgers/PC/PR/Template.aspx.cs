using System;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PR
{
    public partial class Template : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Request.QueryString["TYPE_CODE"]);
            Response.Write(DateTime.Now.ToString());

            base.Page_Load(sender, e);
        }
    }
}