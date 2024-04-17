using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Option_Admin_Interface_AccountMap_AccountMappUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FileUploadControl.Attributes.Add("onchange", "FileUploadControl_Change(this)");
        if (!IsPostBack)
        {
            Page_Setting();
        }
        else
        {

        }
    }

    private void Page_Setting()
    {
        Session["fileName"] = string.Empty;
    }

    protected string uploadFile()
    {
        string fileName = string.Empty;
        if (FileUploadControl.HasFile)
        {
            fileName = Path.GetFileName(FileUploadControl.FileName);
            FileUploadControl.SaveAs(Server.MapPath("~/CSV/") + fileName);
        }
        return fileName;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Parent", "callbtnExportOfParent();", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string fileName = uploadFile();
        Session["fileName"] = fileName;
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Parent", "callbtnUploadParent();", true);
    }
}