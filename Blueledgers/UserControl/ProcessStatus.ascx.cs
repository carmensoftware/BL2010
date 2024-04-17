using System;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class ProcessStatus : BaseUserControl
    {
        #region "Attributies"

        private string _apprStatus = string.Empty;

        #endregion

        #region "Operations"

        public string ApprStatus
        {
            get
            {
                if (ViewState["ApprStatus"] == null)
                {
                    return _apprStatus;
                }

                return ViewState["ApprStatus"].ToString();
            }
            set
            {
                _apprStatus = value;
                ViewState["ApprStatus"] = _apprStatus;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
            for (var i = 0; i < ApprStatus.Length; i++)
            {
                var imgWF = new Image();

                switch (ApprStatus.Substring(i, 1))
                {
                    case "_": // Wait for approve
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/NA.gif";
                        break;

                    case "A": // Approve
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/APP.gif";
                        break;

                    case "R": // Reject
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/REJ.gif";
                        break;

                    case "P": // Partial Approve
                        imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/PAR.gif";
                        break;
                }

                Panel_ProcessStatus.Controls.Add(imgWF);
            }
        }

        #endregion
    }
}