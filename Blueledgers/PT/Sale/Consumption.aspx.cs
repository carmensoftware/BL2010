using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace BlueLedger.PL.PT.Sale
{
    public partial class Consumption : BasePage
    {
        private readonly string moduleID = "4.3";
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();

        private readonly Blue.BL.IN.StockOut stockOut = new Blue.BL.IN.StockOut();

        private DateTime _Date
        {
            get { return Request.QueryString["date"] == null ? DateTime.Today : Convert.ToDateTime(Request.QueryString["date"]); }
        }

        private DataTable _dtItems
        {
            get
            {
                return Session["_Data"] == null ? null : Session["_Data"] as DataTable;

            }
            set
            {
                Session["_Data"] = value;
            }
        }

        // Event(s)

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

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
            _dtItems = GetProductItems(_Date);

            var locations = _dtItems.AsEnumerable()
                .Select(x => new
                {
                    Code = x.Field<string>("LocationCode"),
                    Name = x.Field<string>("LocationCode") + " : " + x.Field<string>("LocationName"),
                })
                .Distinct()
                .ToList();
            if (locations != null)
            {
                listbox_Location.DataTextField = "Name";
                listbox_Location.DataValueField = "Code";
                listbox_Location.DataSource = locations;
                listbox_Location.DataBind();
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "ISSUE":
                    var check = GetMissingMappingCode();

                    if (check.Outlets.Count() > 0)
                    {
                        //ShowAlert("Some outlets need to be assign location.");

                        lbl_MissingCode_Text.Text = "Some outlets need to be assigned the location.";
                        list_MissingCode.Items.Clear();
                        foreach (var item in check.Outlets)
                        {
                            list_MissingCode.Items.Add(item);
                        }
                        pop_MissingCode.ShowOnPageLoad = true;

                        return;
                    }

                    if (check.Items.Count() > 0)
                    {
                        //ShowAlert("Some items need to be assign recipe.");

                        lbl_MissingCode_Text.Text = "Some items need to be assigned the recipe.";
                        list_MissingCode.Items.Clear();
                        foreach (var item in check.Items)
                        {
                            list_MissingCode.Items.Add(item);
                        }
                        pop_MissingCode.ShowOnPageLoad = true;

                        return;
                    }

                    ShowIssueOption();
                    break;
                case "BACK":
                    Response.Redirect("SaleList.aspx?date=" + _Date.ToString("yyyy-MM-dd"));
                    break;

            }
        }

        protected void listbox_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            var locationCode = listbox.SelectedValue.ToString();


            var dt = _dtItems.AsEnumerable()
                .Where(x => x.Field<string>("LocationCode") == locationCode)
                .Select(x => new
                {
                    ProductCode = x.Field<string>("ProductCode"),
                    ProductDesc1 = x.Field<string>("ProductDesc1"),
                    ProductDesc2 = x.Field<string>("ProductDesc2"),
                    Unit = x.Field<string>("Unit"),
                    Qty = x.Field<decimal>("Qty")
                })
                .ToList();

            gv_Items.DataSource = dt;
            gv_Items.DataBind();


        }

        protected void btn_Issue_Create_Click(object sender, EventArgs e)
        {
            var docDate = _Date;
            var adjTypeId = ddl_IssueType.SelectedValue.ToString();



            if (IsPosted(docDate))
            {
                ShowAlert(string.Format("No creating. Issues have already posted.", docDate.ToString("dd/MM/yyyy")));
            }
            else
            {
                SaveConfig(adjTypeId);
                CreateStockOut(docDate, adjTypeId);
            }

            pop_Issue.ShowOnPageLoad = false;
        }

        // Private method(s)
        protected string FormatQty(object sender)
        {
            return string.Format("{0:N" + DefaultAmtDigit.ToString() + "}", Convert.ToDecimal(sender));
        }

        private void ShowAlert(string text, string headerText = null)
        {

            lbl_Alert.Text = text;
            pop_Alert.HeaderText = string.IsNullOrEmpty(headerText) ? "Alert" : headerText;
            pop_Alert.ShowOnPageLoad = true;
        }

        private DataTable GetProductItems(DateTime date)
        {
            var sql = string.Format("EXEC PT.GetConsumptionOfSale '{0}'", date.ToString("yyyy-MM-dd"));

            return bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }

        private NoMapCode GetMissingMappingCode()
        {
            var result = new NoMapCode
            {
                Outlets = new List<string>(),
                Items = new List<string>()
            };


            var sql = string.Format(@"
                    SELECT o.OutletName, ISNULL(o.LocationCode,'') LocationCode, s.ItemCode, i.ItemName, ISNULL(i.RcpCode,'') RcpCode
                    FROM PT.Sale s
                    LEFT JOIN PT.Outlet o ON o.OutletCode=s.OutletCode
                    LEFT JOIN PT.Item i ON i.ItemCode=s.ItemCode
                    WHERE SaleDate = '{0}'
                    AND (LocationCode IS NULL OR RcpCode IS NULL)", _Date.ToString("yyyy-MM-dd"));

            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var outlets = dt.AsEnumerable()
                    .Where(x => x.Field<string>("LocationCode") == "")
                    .Select(x => x.Field<string>("OutletName"))
                    .Distinct();

                var items = dt.AsEnumerable()
                    .Where(x => x.Field<string>("RcpCode") == "")
                    .Select(x => x.Field<string>("ItemCode") + " : " + x.Field<string>("ItemName"))
                    .Distinct();

                result.Outlets = outlets.ToList();
                result.Items = items.ToList();

            }



            return result;

        }

        private void ShowIssueOption()
        {

            var dt = bu.DbExecuteQuery("SELECT AdjId, AdjCode, AdjName FROM [IN].AdjType WHERE AdjType='Stock Out' ORDER BY AdjName", null, LoginInfo.ConnStr);

            ddl_IssueType.DataSource = dt;
            ddl_IssueType.DataTextField = "AdjName";
            ddl_IssueType.DataValueField = "AdjId";
            ddl_IssueType.DataBind();

            var config = new Blue.BL.APP.Config();
            var adjId = config.GetValue("PT", "Sale", "AdjId", LoginInfo.ConnStr);

            var index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["AdjId"].ToString() == adjId)
                {
                    index = i;
                    break;
                }
            }
            ddl_IssueType.SelectedIndex = index;





            pop_Issue.ShowOnPageLoad = true;
        }

        private bool IsPosted(DateTime date)
        {
            var sql = string.Format("SELECT COUNT(ID) as RecordCount FROM PT.Sale WHERE SaleDate = '{0}' AND IsPost = 1", date.ToString("yyyy-MM-dd"));
            var dt = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            var isPosted = Convert.ToInt32(dt.Rows[0][0]) > 0;

            return isPosted;
        }

        private void CreateStockOut(DateTime date, string adjTypeId)
        {
            var dt = new DataTable();
            var sql = new StringBuilder();

            var dtItems = GetProductItems(date);

            var locationCodes = dtItems.AsEnumerable()
               .Select(x => x.Field<string>("LocationCode"))
               .Distinct()
               .ToList();

            // Create Stock-Out by Type (AdjId)
            // [IN].StockOut
            var desc = "Issued by daily sale";
            var commitDate = DateTime.Now;


            foreach (var locationCode in locationCodes)
            {

                var docNo = stockOut.GetNewID(date, LoginInfo.ConnStr);

                sql.Clear();
                sql.AppendLine("INSERT INTO [IN].StockOut ([RefId], [Type], [Status], [Description], [CommitDate], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate])");
                sql.AppendFormat("VALUES ('{0}', '{1}', 'Committed', '{2}', '{3}', '{4}','{5}', '{4}','{6}' )",
                        docNo,
                        adjTypeId,
                        desc,
                        commitDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        LoginInfo.LoginName,
                        date.ToString("yyyy-MM-dd"),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    );


                //dt = bu.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);
                ExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);

                sql.Clear();

                var details = dtItems.AsEnumerable().Where(x => x.Field<string>("LocationCode") == locationCode).ToList();

                foreach (var item in details)
                {
                    var values = string.Format("('{0}', '{1}', '{2}', '{3}', {4}, 0)",
                        docNo,
                        locationCode,
                        item.Field<string>("ProductCode"),
                        item.Field<string>("Unit"),
                        item.Field<decimal>("Qty")
                    );

                    sql.AppendLine("INSERT INTO [IN].StockOutDt (RefId, StoreId, SKU, Unit, Qty, UnitCost) VALUES " + values);
                };

                //bu.DbExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);
                ExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);

                // Insert into [IN].Inventory

                dt = ExecuteQuery(string.Format("SELECT * FROM [IN].StockOutDt WHERE  RefId ='{0}'", docNo), null, LoginInfo.ConnStr);

                foreach (DataRow dr in dt.Rows)
                {

                    sql.Clear();
                    sql.Append("EXEC [IN].[InsertInventorySO] @DocNo,@DocDtNo,@LocationCode,@ProductCode,@Qty");

                    //var p = new List<Blue.DAL.DbParameter>();
                    var p = new List<SqlParameter>();

                    var docDtNo = dr["Id"].ToString();
                    var productCode = dr["SKU"].ToString();
                    var qty = Convert.ToDecimal(dr["Qty"]);

                    p.Add(new SqlParameter("@DocNo", docNo));
                    p.Add(new SqlParameter("@DocDtNo", docDtNo));
                    p.Add(new SqlParameter("@LocationCode", locationCode));
                    p.Add(new SqlParameter("@ProductCode", productCode));
                    p.Add(new SqlParameter("@Qty", qty));

                    ExecuteQuery(sql.ToString(), p.ToArray(), LoginInfo.ConnStr);
                }

                sql.Clear();
                sql.AppendFormat(@"UPDATE [IN].StockOutDt 
                            SET UnitCost = i.Amount
                            FROM 
	                            [IN].StockOutDt o
	                            JOIN [IN].Inventory i ON i.DtNo=o.Id
                            WHERE
	                            i.[Type]='SO'
	                            AND HdrNo = '{0}'", docNo);
                ExecuteQuery(sql.ToString(), null, LoginInfo.ConnStr);
            }

            ExecuteQuery(string.Format("UPDATE [PT].Sale SET IsPost=1 WHERE SaleDate = '{0}'", date.ToString("yyyy-MM-dd")), null, LoginInfo.ConnStr);
        }

        private void SaveConfig(string adjId)
        {
            var sql = string.Format("INSERT INTO [APP].[Config] (Module, SubModule, [Key], [Value], Remark, UpdatedDate, UpdatedBy) VALUES ('PT','Sale','AdjId','{0}','', GETDATE(), 'SYSTEM')", adjId);

            ExecuteQuery("DELETE FROM [APP].[Config] WHERE Module='PT' AND SubModule='Sale' AND [Key]='AdjId'", null, LoginInfo.ConnStr);
            ExecuteQuery(sql, null, LoginInfo.ConnStr);
        }


        private DataTable ExecuteQuery(string query, SqlParameter[] parameters, string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (var p in parameters)
                            {
                                da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                            }
                        }

                        var dt = new DataTable();
                        da.Fill(dt);

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        internal class NoMapCode
        {
            public IEnumerable<string> Outlets { get; set; }
            public IEnumerable<string> Items { get; set; }
        }





    }
}
