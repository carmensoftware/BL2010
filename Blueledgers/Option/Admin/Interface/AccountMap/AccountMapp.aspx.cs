using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using BlueLedger.PL.BaseClass;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();

        #endregion

        //private string _ViewId
        //{
        //    get { return Request.QueryString["id"] == null ? "" : Request.QueryString["id"].ToString(); }
        //}

        //private string _TextSearch
        //{
        //    get { return Request.QueryString["search"] == null ? "" : Request.QueryString["search"].ToString(); }
        //}


        protected override void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Page_Retrieve();
            }




        }

        private void Page_Retrieve()
        {
            var dtView = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("SELECT ID, ViewName FROM [ADMIN].[AccountMappView] ORDER BY Id");

            ddl_View.Items.Clear();
            ddl_View.Items.AddRange(dtView.AsEnumerable()
                .Select(x => new ListItem
                {
                    Value = x.Field<int>("Id").ToString(),
                    Text = x.Field<string>("ViewName")
                })
                .ToArray());



            Page_Setting();
        }

        private void Page_Setting()
        {
        }

        // Event(s)
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "EDIT": //2016-10-05 Add case Edit on Menu_CmdBar
                        break;

                    case "SAVE":
                        break;

                    case "PRINT":
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                        break;

                    case "BACK":
                        break;

                    case "IMPORT":
                        break;

                    // Added on: 23/11/2017, For: New feature(Get new account-Mapping)
                    case "GETNEW":
                        break;
                    // End Added.
                }
            }
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btn_CreateView_Click(object sender, EventArgs e)
        {
        }

        protected void btn_EditView_Click(object sender, EventArgs e)
        {
        }


        protected void btn_Search_Click(object sender, EventArgs e)
        {
            //var id = _ViewId;
            //var search = txt_Search.Text;
            //var url = string.Format("AccountMapp.aspx?id={0}&search={1}", id, search);

            //Response.Redirect(url);
        }

        // GridView
        protected void gv_AccMap_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gv_AccMap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_AccMap_Sorting(object sender, GridViewSortEventArgs e)
        {
        }


        #region -- Method(s)--
        private void BindData()
        {
            var id = ddl_View.SelectedItem.Value.ToString();
            var searchText = txt_Search.Text;
            var sql = new Helpers.SQL(LoginInfo.ConnStr);

            var dtView = sql.ExecuteQuery("SELECT * FROM [ADMIN].[AccountMappView] WHERE ID =" + id);
            var view = dtView.AsEnumerable()
                .Select(x => new ViewItem
                {
                    Id = x.Field<int>("Id"),
                    ViewName = x.Field<string>("ViewName"),
                    StoreCode = x.Field<bool>("StoreCode"),
                    CategoryCode = x.Field<bool>("CategoryCode"),
                    SubCategoryCode = x.Field<bool>("SubCategoryCode"),
                    itemGroupCode = x.Field<bool>("itemGroupCode"),

                    KeyA1 = x.Field<bool>("KeyA1"),
                    KeyA2 = x.Field<bool>("KeyA2"),
                    KeyA3 = x.Field<bool>("KeyA3"),
                    KeyA4 = x.Field<bool>("KeyA4"),
                    KeyA5 = x.Field<bool>("KeyA5"),
                    KeyA6 = x.Field<bool>("KeyA6"),
                    KeyA7 = x.Field<bool>("KeyA7"),
                    KeyA8 = x.Field<bool>("KeyA8"),
                    KeyA9 = x.Field<bool>("KeyA9"),

                    DescKeyA1 = x.Field<string>("DescKeyA1"),
                    DescKeyA2 = x.Field<string>("DescKeyA2"),
                    DescKeyA3 = x.Field<string>("DescKeyA3"),
                    DescKeyA4 = x.Field<string>("DescKeyA4"),
                    DescKeyA5 = x.Field<string>("DescKeyA5"),
                    DescKeyA6 = x.Field<string>("DescKeyA6"),
                    DescKeyA7 = x.Field<string>("DescKeyA7"),
                    DescKeyA8 = x.Field<string>("DescKeyA8"),
                    DescKeyA9 = x.Field<string>("DescKeyA9"),


                    A1 = x.Field<bool>("A1"),
                    A2 = x.Field<bool>("A2"),
                    A3 = x.Field<bool>("A3"),
                    A4 = x.Field<bool>("A4"),
                    A5 = x.Field<bool>("A5"),
                    A6 = x.Field<bool>("A6"),
                    A7 = x.Field<bool>("A7"),
                    A8 = x.Field<bool>("A8"),
                    A9 = x.Field<bool>("A9"),

                    DescA1 = x.Field<string>("DescA1"),
                    DescA2 = x.Field<string>("DescA2"),
                    DescA3 = x.Field<string>("DescA3"),
                    DescA4 = x.Field<string>("DescA4"),
                    DescA5 = x.Field<string>("DescA5"),
                    DescA6 = x.Field<string>("DescA6"),
                    DescA7 = x.Field<string>("DescA7"),
                    DescA8 = x.Field<string>("DescA8"),
                    DescA9 = x.Field<string>("DescA9"),

                    TypeA1 = x.Field<string>("TypeA1"),
                    TypeA2 = x.Field<string>("TypeA2"),
                    TypeA3 = x.Field<string>("TypeA3"),
                    TypeA4 = x.Field<string>("TypeA4"),
                    TypeA5 = x.Field<string>("TypeA5"),
                    TypeA6 = x.Field<string>("TypeA6"),
                    TypeA7 = x.Field<string>("TypeA7"),
                    TypeA8 = x.Field<string>("TypeA8"),
                    TypeA9 = x.Field<string>("TypeA9"),

                    PostType = x.Field<string>("PostType")


                })
                .FirstOrDefault();

            var queries = new StringBuilder();
            var select = new StringBuilder();

            if (view.StoreCode)
                select.Append("StoreCode,");
            if (view.CategoryCode)
                select.Append("CategoryCode,");
            if (view.SubCategoryCode)
                select.Append("SubCategoryCode,");
            if (view.itemGroupCode)
                select.Append("ItemGroupCode,");
            
            if (view.KeyA1)
                select.Append("A1 as KeyA1,");
            if (view.KeyA2)
                select.Append("A2 as KeyA2,");
            if (view.KeyA3)
                select.Append("A3 as KeyA3,");
            if (view.KeyA4)
                select.Append("A4 as KeyA4,");
            if (view.KeyA5)
                select.Append("A5 as KeyA5,");
            if (view.KeyA6)
                select.Append("A6 as KeyA6,");
            if (view.KeyA7)
                select.Append("A7 as KeyA7,");
            if (view.KeyA8)
                select.Append("A8 as KeyA8,");
            if (view.KeyA9)
                select.Append("A9 as KeyA9,");

            if (view.A1)
                select.Append("A1 as A1,");
            if (view.A2)
                select.Append("A2 as A2,");
            if (view.A3)
                select.Append("A3 as A3,");
            if (view.A4)
                select.Append("A4 as A4,");
            if (view.A5)
                select.Append("A5 as A5,");
            if (view.A6)
                select.Append("A6 as A6,");
            if (view.A7)
                select.Append("A7 as A7,");
            if (view.A8)
                select.Append("A8 as A8,");
            if (view.A9)
                select.Append("A9 as A9,");
            
            queries.AppendLine("SELECT");
            queries.AppendLine(select.ToString().TrimEnd(','));
            queries.AppendLine("FROM [ADMIN].[AccountMapp]");
            queries.AppendLine("WHERE PostType = @type");


            var dt = sql.ExecuteQuery(queries.ToString(), new SqlParameter[] { new SqlParameter("type", view.PostType) });

            gv_AccMap.DataSource = dt;
            gv_AccMap.DataBind();

        }

        #endregion

        public class ViewItem
        {
            public int Id { get; set; }
            public string ViewName { get; set; }
            //public bool BusinessUnitCode { get; set; }
            public bool StoreCode { get; set; }
            public bool CategoryCode { get; set; }
            public bool SubCategoryCode { get; set; }
            public bool itemGroupCode { get; set; }

            public bool KeyA1 { get; set; }
            public bool KeyA2 { get; set; }
            public bool KeyA3 { get; set; }
            public bool KeyA4 { get; set; }
            public bool KeyA5 { get; set; }
            public bool KeyA6 { get; set; }
            public bool KeyA7 { get; set; }
            public bool KeyA8 { get; set; }
            public bool KeyA9 { get; set; }

            public string DescKeyA1 { get; set; }
            public string DescKeyA2 { get; set; }
            public string DescKeyA3 { get; set; }
            public string DescKeyA4 { get; set; }
            public string DescKeyA5 { get; set; }
            public string DescKeyA6 { get; set; }
            public string DescKeyA7 { get; set; }
            public string DescKeyA8 { get; set; }
            public string DescKeyA9 { get; set; }

            public bool A1 { get; set; }
            public bool A2 { get; set; }
            public bool A3 { get; set; }
            public bool A4 { get; set; }
            public bool A5 { get; set; }
            public bool A6 { get; set; }
            public bool A7 { get; set; }
            public bool A8 { get; set; }
            public bool A9 { get; set; }

            public string DescA1 { get; set; }
            public string DescA2 { get; set; }
            public string DescA3 { get; set; }
            public string DescA4 { get; set; }
            public string DescA5 { get; set; }
            public string DescA6 { get; set; }
            public string DescA7 { get; set; }
            public string DescA8 { get; set; }
            public string DescA9 { get; set; }

            public string TypeA1 { get; set; }
            public string TypeA2 { get; set; }
            public string TypeA3 { get; set; }
            public string TypeA4 { get; set; }
            public string TypeA5 { get; set; }
            public string TypeA6 { get; set; }
            public string TypeA7 { get; set; }
            public string TypeA8 { get; set; }
            public string TypeA9 { get; set; }

            public string PostType { get; set; }
        }


    }



}