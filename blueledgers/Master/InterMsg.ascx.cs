using System;

namespace BlueLedger.PL.Master
{
    public partial class InterMsg : BaseClass.BaseUserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}