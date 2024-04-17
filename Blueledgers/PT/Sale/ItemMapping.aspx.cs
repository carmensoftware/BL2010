using System;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;

namespace BlueLedger.PL.PT.Sale
{
    public partial class ItemMapping : BasePage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            BindItemData();
        }


        protected void gv_Item_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var gv = sender as GridView;

            gv.PageIndex = e.NewPageIndex;

            BindItemData();

        }

        protected void gv_Item_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var textUtils = new Helpers.TextUtils();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("ddl_Recipe") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Recipe") as ASPxComboBox;

                    var itemName = DataBinder.Eval(e.Row.DataItem, "ItemName").ToString();
                    var dt = GetRecipe();


                    var list = new List<ItemScore>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        var code = dr["RcpDesc1"].ToString();
                        var name = dr["RcpName"].ToString();
                        var score = textUtils.GetCompareScore(itemName, code);

                        list.Add(new ItemScore
                        {
                            Code = code,
                            Name = name + "("+ score.ToString()+")",
                            Score = score
                        });
                    }

                    ddl.DataSource = list.OrderByDescending(x => x.Score).OrderBy(x=>x.Code).ToList();
                    ddl.ValueField = "Code";
                    ddl.TextField = "Name";
                    ddl.DataBind();

                    if (list.Count() > 0)
                    {
                        if (list[0].Score > 0)
                            ddl.SelectedIndex = 0;
                    }
                }
            }
        }

        // private method(s)
        private void BindItemData()
        {
            var dt = GetItems();

            gv_Item.DataSource = dt;
            gv_Item.DataBind();

        }


        private DataTable GetRecipe()
        {
            var query = "SELECT RcpCode, CONCAT(RcpCode,' : ',RcpDesc1) as RcpName, RcpDesc1  FROM PT.Rcp ORDER BY RcpCode";

            return new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query);
        }



        private DataTable GetItems()
        {
            var query = "SELECT * FROM PT.Item WHERE ISNULL(RcpCode,'')='' ";

            return new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query);
        }


        public class ItemScore
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
        }

    }
}

