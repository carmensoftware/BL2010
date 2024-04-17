using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class AdjTypeDt : BasePage
    {
        private DataSet dsAdjType = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsAdjType = (DataSet) Session["dsAdjType"];
            }
        }

        private void Page_Retrieve()
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
        }
    }
}