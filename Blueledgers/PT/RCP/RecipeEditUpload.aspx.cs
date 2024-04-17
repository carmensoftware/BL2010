using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PT_RCP_ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fileUploadImg.Attributes.Add("onchange", "FileUploadControl_Change(this)");
        if (!IsPostBack)
        {
            Page_Setting();
        }
        else
        {
        }
    }

    protected void Page_Setting()
    {
        Session["ImgBytes"] = null;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string filePath = fileUploadImg.PostedFile.FileName;
        string filename = Path.GetFileName(filePath);
        string ext = Path.GetExtension(filename);
        //string contenttype = string.Empty;

        if (ext.ToLower() == ".jpg"
            || ext.ToLower() == ".jpeg"
            || ext.ToLower() == ".png"
            || ext.ToLower() == ".gif")
        {
            string contentType = "data:image/" + ext.ToLower().Substring(1) + ";base64,";

            Stream fs = fileUploadImg.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string imgUrl = contentType + Convert.ToBase64String(bytes, 0, bytes.Length);

            lblMessage.Text = "";
            Session["ImgBytes"] = (string)imgUrl;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Parent", "callParentbtnUpload_Hide()", true);
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "File not supported.";
        }
    }
}