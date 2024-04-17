using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
    public Report()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void PrintForm(System.Web.UI.Control control, string pagePath, string id, string reportName)
    {
        string script = "<script> window.open('" + pagePath+ "?ID=" + id + "&Report=" + reportName + "')</script>";
        System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "newForm", script, false);

    }

}