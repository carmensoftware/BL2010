using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Text;

public partial class Option_ProdCat_ProdCatLstUpload : System.Web.UI.Page
{
    private DataSet dsMainProductcategory = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        FileUploadControl.Attributes.Add("onchange", "FileUploadControl_Change(this)");
        if (!IsPostBack)
        {
            Page_Setting();
        }
        else
        {
            dsMainProductcategory = (DataSet)Session["dsMainProductcategory"];
        }
    }

    private void Page_Setting()
    {
        Session["fileName"] = string.Empty;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string header = string.Format("{0},{1},{2},{3},{4}",
                "Category Code", "Sub-Category Code", "Itemgroup Code", "Description", "Category Type", "TaxAccount");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename = product_category.csv");
            HttpContext.Current.Response.ContentType = "text/csv";
            //HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.Write(header);
            HttpContext.Current.Response.End();
            Response.Flush();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string fileName = uploadFile();
        Session["fileName"] = fileName;

        Page.ClientScript.RegisterStartupScript(this.GetType(), "Parent", "callParent();", true);
    }

    protected string uploadFile()
    {
        string fileName = string.Empty;

        if (FileUploadControl.HasFile)
        {
            fileName = Path.GetFileName(FileUploadControl.FileName);
            FileUploadControl.SaveAs(Server.MapPath("~/Tmp/") + fileName);
        }

        return fileName;
    }
}