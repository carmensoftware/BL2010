using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

public partial class Option_LocalConfig_User_ActiveUserList : System.Web.UI.Page
{


    private void OpenDataSet()
    {
        string connStr = (string)Session["ConnectionString"];


        string sqlSelect = string.Empty;
        sqlSelect += " SELECT LoginName, FName + ' ' + MName + ' ' + LName as FullName, JobTitle, LastLogin, IsActived";
        sqlSelect += " FROM dbo.[User]";
        sqlSelect += " ORDER BY LoginName";

        string sqlUpdate = string.Empty;
        sqlUpdate += " UPDATE dbo.[User]";
        sqlUpdate += " SET IsActived = @IsActived";
        sqlUpdate += " WHERE LoginName = @LoginName";

        aspDsUser.ConnectionString = connStr;
        aspDsUser.SelectCommand = sqlSelect;
        aspDsUser.UpdateCommand = sqlUpdate;


        aspxGrid.SettingsPager.PageSize = 22;
        aspxGrid.DataSourceID = "aspDsUser";
        //grid.StartEdit(-1);


    }

    // ------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        OpenDataSet();
    }


}