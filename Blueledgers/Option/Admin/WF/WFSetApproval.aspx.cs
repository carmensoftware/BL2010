using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;

public partial class Option_Admin_WF_WFSetApproval : System.Web.UI.Page
{
    private static DataTable dtSr = new DataTable();

    private void FillUsers(ASPxComboBox combo, string locationCode)
    {
        string connStr = (string)Session["ConnectionString"];

        SqlConnection conn = new SqlConnection(connStr);
        conn.Open();

        string sql = string.Empty;
        sql += " SELECT LoginName";
        sql += " FROM [ADMIN].UserStore";
        sql += " WHERE LocationCode = '" + locationCode + "'";
        sql += " ORDER BY LoginName";
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader reader = cmd.ExecuteReader();

        combo.Items.Clear();
        combo.Items.Add("");
        while (reader.Read())
        {
            combo.Items.Add(reader["LoginName"].ToString());
        }

        reader.Close();
        conn.Close();

    }

    private void OpenDataSetForSR()
    {
        string connStr = (string)Session["ConnectionString"];
        //SqlConnection conn = new SqlConnection(connStr);
        //conn.Open();

        #region Old ver.
        /*string sqlSelect = string.Empty;
        sqlSelect = string.Format(@"SELECT h.Id, h.LocationCode, l.LocationName, ApproveSRLv1, ApproveSRLv2, ApproveSRLv3,
            FROM [IN].HeadofLocation h
            LEFT JOIN [IN].StoreLocation l ON l.LocationCode = h.LocationCode
            ORDER BY LocationCode");

        //SqlCommand cmd = new SqlCommand(sqlSelect, conn);
        //SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        ////DataTable dt = new DataTable();
        //dtSr.Clear();
        //adapter.Fill(dtSr);
        //conn.Close();

        string sqlUpdate = string.Empty;
        sqlUpdate = string.Format(@"UPDATE [IN].HeadofLocation
            SET ApproveSRLv1 = @ApproveSRLv1, ApproveSRLv2 = @ApproveSRLv2, ApproveSRLv3 = @ApproveSRLv3
            WHERE Id = @Id");
         
        DataSourceSR.ConnectionString = connStr;
        DataSourceSR.SelectCommand = sqlSelect;
        DataSourceSR.UpdateCommand = sqlUpdate;

        grid.SettingsPager.PageSize = 22;
        grid.DataSourceID = "DataSourceSR";
        //grid.StartEdit(-1); 
        */
        #endregion

        // Modified on: 27/11/2017, By:Fon, For: New field.
        string sqlSelect = string.Empty;
        string sqlUpdate = string.Empty;


        if (ddl_ApprModule.SelectedValue != null)
        {
            sqlSelect = string.Format(@"SELECT h.Id, h.LocationCode, l.LocationName, 
                ApproveSRLv1, ApproveSRLv2, ApproveSRLv3,
                IssueSRLv1, IssueSRLv2, IssueSRLv3
            
                FROM [IN].HeadofLocation h
                LEFT JOIN [IN].StoreLocation l ON l.LocationCode = h.LocationCode
                ORDER BY LocationCode");

            sqlUpdate = string.Format(@"UPDATE [IN].HeadofLocation
                SET ApproveSRLv1=@ApproveSRLv1, ApproveSRLv2=@ApproveSRLv2, ApproveSRLv3=@ApproveSRLv3,
                IssueSRLv1=@IssueSRLv1, IssueSRLv2=@IssueSRLv2, IssueSRLv3=@IssueSRLv3
                WHERE Id = @Id");
        }

        DataSourceSR.ConnectionString = connStr;
        DataSourceSR.SelectCommand = sqlSelect;
        DataSourceSR.UpdateCommand = sqlUpdate;

        grid.SettingsPager.PageSize = 22;
        grid.DataSourceID = "DataSourceSR";
        // End Modified.
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        // initialize SomeDataTable
        //Session["ConnectionString"] = @"Data Source=.\SQL2008R2; Initial Catalog=IMPACT_BL; User ID=sa; Password=sa@genex;";
        //OpenDataSetForSR();

        // Modified on: 28/11/2017, By: Fon
        OpenDataSetForPR();
        OpenDataSetForSR();
        // End Modified.
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Added on: 28/11/2017, By: Fon
            Control_gridWith_ddl_AppeModule(ddl_ApprModule.SelectedValue);
            // End Added.
        }
    }

    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!grid.IsEditing) return;
        //if (e.Column.FieldName == "ApproveSRLv1" || e.Column.FieldName == "ApproveSRLv2" || e.Column.FieldName == "ApproveSRLv3")
        if (e.Column.FieldName.Contains("SRLv"))
        {
            if (e.KeyValue == DBNull.Value || e.KeyValue == null) return;
            object val = grid.GetRowValuesByKeyValue(e.KeyValue, "LocationCode");
            if (val == DBNull.Value) return;
            string locationCode = (string)val;

            ASPxComboBox combo = e.Editor as ASPxComboBox;
            FillUsers(combo, locationCode);

            //combo.Callback += new CallbackEventHandlerBase(ComboBox_OnCallback);
        }
    }

    //private void ComboBox_OnCallback(object source, CallbackEventArgsBase e)
    //{
    //    FillUsers(source as ASPxComboBox, e.Parameter);
    //}


    //protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    //{
    //    int index = -1;
    //    if (int.TryParse(e.Parameters, out index))
    //        grid.SettingsEditing.Mode = (GridViewEditingMode)index;
    //}

    #region
    // Added on: 21/11/2017, By: Fon, For: PR, SR Issue.
    /* Note
     * On: 28/11/2017, I try to show-hide in one GridView but failed. So I make it 2 parts.
     */
    protected void ddl_ApprModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl_AppeModule = (DropDownList)sender;
        Control_gridWith_ddl_AppeModule(ddl_ApprModule.SelectedValue.ToString());
    }

    protected void Control_gridWith_ddl_AppeModule(string ddl_value)
    {
        #region
        //foreach (GridViewColumn col in grid.Columns)
        //{
        //    if (col is GridViewDataColumn)
        //    {
        //        GridViewDataColumn dataCol = (GridViewDataColumn)col;
        //        if (dataCol.FieldName.Contains("PRLv") || dataCol.FieldName.Contains("SRLv"))
        //            col.Visible = false;

        //        if (dataCol.FieldName.Contains("PRLv") && ddl_value.Contains("PR")) col.Visible = true;
        //        else if (dataCol.FieldName.Contains("SRLv") && ddl_value.Contains("SR")) col.Visible = true;
        //    }
        //}
        #endregion

        grid_PR.Visible = (ddl_value.Contains("PR")) ? true : false;
        grid.Visible = (ddl_value.Contains("SR")) ? true : false;

    }

    private void OpenDataSetForPR()
    {
        string connStr = (string)Session["ConnectionString"];
        string sqlSelect = string.Empty;
        string sqlUpdate = string.Empty;

        if (ddl_ApprModule.SelectedValue != null)
        {
            if (ddl_ApprModule.SelectedValue == "PR")
            {
                sqlSelect = string.Format(@"SELECT h.Id, h.LocationCode, l.LocationName, 
                            PermitPRLv1, PermitPRLv2, PermitPRLv3,
                        	ApprovePRLv1, ApprovePRLv2, ApprovePRLv3
                        
                            FROM [IN].HeadofLocation h
                            LEFT JOIN [IN].StoreLocation l ON l.LocationCode = h.LocationCode
                            ORDER BY LocationCode");

                sqlUpdate = string.Format(@"UPDATE [IN].HeadofLocation
                            SET PermitPRLv1=@PermitPRLv1, PermitPRLv2=@PermitPRLv2, PermitPRLv3=@PermitPRLv3,
                            ApprovePRLv1=@ApprovePRLv1, ApprovePRLv2=@ApprovePRLv2, ApprovePRLv3=@ApprovePRLv3
                            WHERE Id = @Id");
            }
        }

        DataSourcePR.ConnectionString = connStr;
        DataSourcePR.SelectCommand = sqlSelect;
        DataSourcePR.UpdateCommand = sqlUpdate;

        grid_PR.SettingsPager.PageSize = 22;
        grid_PR.DataSourceID = "DataSourcePR";
    }

    protected void grid_PR_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!grid_PR.IsEditing) return;
        if (e.Column.FieldName.Contains("PRLv"))
        {
            if (e.KeyValue == DBNull.Value || e.KeyValue == null) return;
            object val = grid_PR.GetRowValuesByKeyValue(e.KeyValue, "LocationCode");
            if (val == DBNull.Value) return;
            string locationCode = (string)val;

            ASPxComboBox combo = e.Editor as ASPxComboBox;
            FillUsers(combo, locationCode);
        }
    }
    #endregion
}