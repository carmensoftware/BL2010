using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PO
{
    public partial class PoChangeDocStatus : BasePage
    {
        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private DataSet dsPo = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            dsPo = (DataSet) Session["dsPo"];

            if (dsPo.Tables[po.TableName].Rows[0]["DocStatus"].ToString() == "Approved")
            {
                dsPo.Tables[po.TableName].Rows[0]["DocStatus"] = "Printed";
            }

            // Commit change to database
            var resultSave = po.SaveOnlyPO(dsPo, LoginInfo.ConnStr);

            if (resultSave)
            {
            }

            Session["dsPo"] = dsPo;

            Response.Redirect("~/RPT/Default.aspx?PoNo=" + dsPo.Tables[po.TableName].Rows[0]["PoNo"]);


            //}
            //else
            //{
            //    dsPo = (DataSet)Session["dsPo"];
            //}
        }
    }
}