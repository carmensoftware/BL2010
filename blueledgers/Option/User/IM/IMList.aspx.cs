using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IM
{
    public partial class IMList : BasePage
    {
        #region "Attributies"

        private DataSet dsReceiptList = new DataSet();
        private Blue.BL.Application.Field field = new Blue.BL.Application.Field();
        private Blue.BL.ProjectAdmin.Menu menu = new Blue.BL.ProjectAdmin.Menu();
        private Blue.BL.AR.Receipt receipt = new Blue.BL.AR.Receipt();
        private Blue.BL.AR.ReceiptAttachment receiptAttachment = new Blue.BL.AR.ReceiptAttachment();
        private Blue.BL.AR.ReceiptAuto receiptAuto = new Blue.BL.AR.ReceiptAuto();
        private Blue.BL.AR.ReceiptCash receiptCash = new Blue.BL.AR.ReceiptCash();
        private Blue.BL.AR.ReceiptCheq receiptCheq = new Blue.BL.AR.ReceiptCheq();
        private Blue.BL.AR.ReceiptCheqDetail receiptCheqDetail = new Blue.BL.AR.ReceiptCheqDetail();
        private Blue.BL.AR.ReceiptComment receiptComment = new Blue.BL.AR.ReceiptComment();
        private Blue.BL.AR.ReceiptCredit receiptCredit = new Blue.BL.AR.ReceiptCredit();
        private Blue.BL.AR.ReceiptDetail receiptDetail = new Blue.BL.AR.ReceiptDetail();
        private Blue.BL.AR.ReceiptDetailAmt receiptDetailAmt = new Blue.BL.AR.ReceiptDetailAmt();
        private Blue.BL.AR.ReceiptLog receiptLog = new Blue.BL.AR.ReceiptLog();
        private Blue.BL.AR.ReceiptMisc receiptMisc = new Blue.BL.AR.ReceiptMisc();
        private Blue.BL.AR.ReceiptTrans receiptTrans = new Blue.BL.AR.ReceiptTrans();
        private Blue.BL.AR.ReceiptView receiptView = new Blue.BL.AR.ReceiptView();
        private Blue.BL.AR.ReceiptViewColumn receiptViewColumn = new Blue.BL.AR.ReceiptViewColumn();
        private Blue.BL.AR.ReceiptWHT receiptWHT = new Blue.BL.AR.ReceiptWHT();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Get data from database
        /// </summary>
        private void Page_Retrieve()
        {
        }

        /// <summary>
        ///     Binding controls
        /// </summary>
        private void Page_Setting()
        {
            img_Imp1.ImageUrl = "../App_Themes/default/pics/important_icon.gif";
            img_Reply1.ImageUrl = "../App_Themes/default/pics/reply.gif";
            img_For1.ImageUrl = "../App_Themes/Default/Images/blank.gif";

            img_Imp2.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_Reply2.ImageUrl = "../App_Themes/default/pics/reply.gif";
            img_For2.ImageUrl = "../App_Themes/Default/Images/blank.gif";

            img_Imp3.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_Reply3.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_For3.ImageUrl = "../App_Themes/default/pics/forward.gif";

            img_Imp4.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_Reply4.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_For4.ImageUrl = "../App_Themes/Default/Images/blank.gif";

            img_Imp5.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_Reply5.ImageUrl = "../App_Themes/Default/Images/blank.gif";
            img_For5.ImageUrl = "../App_Themes/default/pics/forward.gif";
        }

        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Checking login session 
            base.Page_Load(sender, e);

            // Retrieve data and binding with controls
            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
        }

        protected void lnkb_SendMsg_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMSendMsg.aspx");
        }

        protected void lnkb_1_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMReadMsg.aspx?Line=1");
        }

        protected void lnkb_2_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMReadMsg.aspx?Line=2");
        }

        protected void lnkb_3_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMReadMsg.aspx?Line=3");
        }

        protected void lnkb_4_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMReadMsg.aspx?Line=4");
        }

        protected void lnkb_5_Click(object sender, EventArgs e)
        {
            Response.Redirect("IMReadMsg.aspx?Line=5");
        }

        #endregion
    }
}