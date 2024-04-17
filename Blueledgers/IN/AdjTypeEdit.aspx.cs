using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN
{
    public partial class AdjTypeEdit : BasePage
    {
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();

        private DataSet dsAdjType = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();

                dsAdjType = (DataSet) Session["dsAdjType"];
            }
            else
            {
                dsAdjType = (DataSet) Session["dsAdjType"];
            }
        }

        private void Page_Retrieve()
        {
            var get = adjType.GetSchema(dsAdjType, LoginInfo.ConnStr);

            if (!get)
            {
                return;
            }

            Session["dsAdjType"] = dsAdjType;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drAdjType = dsAdjType.Tables[adjType.TableName].Rows[0];

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var drSave = dsAdjType.Tables[adjType.TableName].NewRow();
            drSave["AdjType"] = ddl_AdjType.Value.ToString();
            drSave["AdjCode"] = txt_AdjCode.Text;
            drSave["AdjName"] = txt_AdjName.Text;
            drSave["Description"] = txt_Desc.Text;
            drSave["CreateBy"] = LoginInfo.LoginName;
            drSave["CreateDate"] = ServerDateTime;
            drSave["UpdateDate"] = ServerDateTime;
            drSave["UpdateBy"] = LoginInfo.LoginName;

            dsAdjType.Tables[adjType.TableName].Rows.Add(drSave);

            var save = adjType.Save(dsAdjType, LoginInfo.ConnStr);

            if (save)
            {
                Response.Redirect("AdjTypeDt.aspx?ID=");
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdjTypeLst.aspx");
        }
    }
}