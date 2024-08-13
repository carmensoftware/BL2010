using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;


namespace BlueLedger.PL.IN.REC
{
    public partial class RECEdit : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();


        #region --URL Parameters--
        private string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        private string _VID { get { return Request.Params["VID"].ToString() ?? ""; } }
        private string _ID { get { return Request.Params["ID"].ToString() ?? ""; } }

        #endregion


        #region --Event(s)--

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
        }

        private void Page_Retrieve()
        {

        }

        private void Page_Setting()
        {

        }

        // Title / Action bar
        protected void btn_Save_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var MODE = Request.QueryString["MODE"];


            if (MODE.ToUpper() == "EDIT")
            {
                var id = _ID;
                var buCode = _BuCode;
                var vid = _VID;
                Response.Redirect(string.Format("Rec.aspx?ID={0}&BuCode={1}&Vid={2}", id, buCode, vid));
            }
            else
            {
                Response.Redirect("RecLst.aspx");
            }
        }


        // Header
        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btn_AllocateExtraCost_Click(object sender, EventArgs e)
        {
        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {
        }

        // Add Item / PO
        protected void btn_AddPo_Click(object sender, EventArgs e)
        {
        }

        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }

}