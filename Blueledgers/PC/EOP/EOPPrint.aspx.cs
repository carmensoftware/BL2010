using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;

namespace BlueLedger.PL.PC.EOP
{
    public partial class EOPPrint : BasePage
    {
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();

        protected DataTable _dtEop = new DataTable();
        protected DataTable _dtEopDt = new DataTable();

        protected int _digitQty { get { return (int)ViewState["_digitQty"]; } set { ViewState["_digitQty"] = value; } }

        private string _eopId { get { return Request.QueryString["ID"].ToString() ?? "0"; } }


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                var value = _config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);

                _digitQty = string.IsNullOrEmpty(value) ? 2 : Convert.ToInt32(value);


                var sql = new Helpers.SQL(LoginInfo.ConnStr);
                var query = "";


                // IN.Eop
                query = "SELECT eop.*, l.LocationName FROM [IN].Eop JOIN [IN].StoreLocation l ON l.LocationCode=eop.StoreId WHERE EopId=@id";
                _dtEop = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", _eopId) });

                // IN.EopDt
                query = "SELECT * FROM [IN].EopDt WHERE EopId=@id ORDER BY EopDtId";
                _dtEopDt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", _eopId) });



            }
        }

    }
}