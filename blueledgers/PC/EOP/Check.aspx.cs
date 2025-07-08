using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace BlueLedger.PL.PC.EOP
{
    public partial class Check : BasePage
    {

        private DataTable _dtDetail
        {
            get { return ViewState["_dtDetail"] as DataTable; }
            set { ViewState["_dtDetail"] = value; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Setting();
            }
        }


        private void Page_Setting()
        {
            var query = @"
;WITH
d AS(
	SELECT DISTINCT StartDate FROM [IN].Eop
)
SELECT
	convert(varchar, StartDate, 23) as StartDate
FROM
	d
ORDER BY
	StartDate
";
            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var dt = sql.ExecuteQuery(query);

            var items = dt.AsEnumerable()
                .Select(x => new ListItem
                {
                    Value = x.Field<string>("StartDate"),
                    Text = x.Field<string>("StartDate")
                })
                .ToArray();

            ddl_Period.Items.Clear();
            ddl_Period.Items.AddRange(items);
            ddl_Period.Items.Add(new ListItem { Text = "--Select Period--", Value = "" });

            ddl_Period.SelectedIndex = ddl_Period.Items.Count - 1;
        }


        protected void ddl_Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;

            var value = ddl.SelectedValue;

            if (string.IsNullOrEmpty(value))
            {
                gv1.DataSource = null;
                gv1.DataBind();

                gv2.DataSource = null;
                gv2.DataBind();

                _dtDetail = null;
            }
            else
            {
                var startDate = Convert.ToDateTime(value);

                LoadData(startDate);
            }
        }



        private void LoadData(DateTime date)
        {
            #region --Query--
            var query = @"
declare @enddate date = eomonth(@startdate)

IF OBJECT_ID('tempdb..#loc') IS NOT NULL DROP TABLE #loc
IF OBJECT_ID('tempdb..#eop') IS NOT NULL DROP TABLE #eop
IF OBJECT_ID('tempdb..#locprd') IS NOT NULL DROP TABLE #locprd

CREATE TABLE #locprd (
	LocationCode nvarchar(20) NOT NULL,
	ProductCode nvarchar(20) NOT NULL,
	Onhand decimal(18,3) NULL,
	EopQty decimal(18,3) NULL,
	AdjIn decimal(18,3) NULL,
	AdjOut decimal(18,3) NULL,
	EopId int null,
	EopDtId int null,

	PRIMARY KEY (LocationCode, ProductCode),
	INDEX idx_eopid (EopId),
	INDEX idx_eopdtid (EopDtId)
)

SELECT 
	DISTINCT LocationCode
INTO
	#loc
FROM 
	[IN].Eop 
	JOIN [IN].StoreLocation l ON l.LocationCode=eop.StoreId AND l.EOP=1
WHERE 
	EndDate = EOMONTH(@StartDate)

CREATE INDEX idx_tmp_loc ON #loc(LocationCode)

;WITH
eop AS(
SELECT
	eopdt.EopId,
	eopdt.EopDtId,
	StoreId as LocationCode,
	ProductCode,
	Qty
FROM
	[IN].Eop
	JOIN [IN].EopDt ON eopdt.EopId=eop.EopId
WHERE
	EndDate = @EndDate
	AND eop.StoreId IN (SELECT DISTINCT LocationCode FROM #loc)
),
i AS(
	SELECT
		HdrNo,
		DtNo,
		SUM([IN]) as [IN],
		SUM([OUT]) as [OUT]
	FROM
		[IN].Inventory i
		JOIN eop ON i.HdrNo=CAST(eop.EopId as nvarchar(20)) AND i.DtNo=eop.EopDtId
	GROUP BY
		HdrNo,
		DtNo
)
SELECT
	eop.*,
	i.[IN] as AdjIn,
	i.[OUT] as AdjOut
INTO
	#eop
FROM
	eop
	LEFT JOIN i ON i.HdrNo=CAST(eop.EopId as nvarchar(20)) AND i.DtNo=eop.EopDtId


CREATE INDEX idx_tmp_eop_loc_prd ON #eop (LocationCode, ProductCode)



;WITH
locprd AS(
SELECT 
	LocationCode,
	ProductCode,
	0 as onhand
FROM 
	[IN].Eop 
	JOIN [IN].EopDt ON eop.EopId=eopdt.EopId
	JOIN [IN].StoreLocation l ON l.LocationCode=eop.StoreId AND l.EOP=1
WHERE 
	EndDate = EOMONTH(@StartDate)
GROUP BY
	LocationCode,
	ProductCode
UNION ALL
SELECT
	[LocationCode],
	ProductCode,
	SUM([IN]-[OUT]) as Onhand
FROM
	[IN].Inventory i
	JOIN #loc l ON l.LocationCode=i.Location
WHERE
	ISNULL(ProductCode,'') <> ''
	AND ISNULL(Location,'') <> ''
	AND i.CommittedDate < DATEADD(day,1,@enddate)
GROUP BY
	[LocationCode],
	[ProductCode]
)
INSERT INTO #locprd (LocationCode, ProductCode, Onhand)
SELECT
	LocationCode,
	ProductCode,
	SUM(Onhand) as Onhand
FROM
	locprd	
GROUP BY
	LocationCode,
	ProductCode


UPDATE
	#locprd
SET
	EopQty=eop.Qty,
	AdjIn = eop.AdjIn,
	AdjOut = eop.AdjOut,
	EopId = eop.EopId,
	EopDtId = eop.EopDtId
FROM
	#locprd lp
	JOIN #eop eop ON eop.LocationCode=lp.LocationCode AND eop.ProductCode=lp.ProductCode

DELETE FROM #locprd WHERE Onhand = EopQty

SELECT
	LocationCode,
	ProductCode,
	EopQty,
	CASE 
		WHEN EopQty is not null THEN Onhand - ISNULL(AdjIn,0) + ISNULL(AdjOut,0)
		ELSE NULL
	END as OnhandBeforeEop,
	AdjIn,
	AdjOut,
	Onhand,
	EopId,
	EopDtId,
	CASE WHEN EopQty IS NULL THEN 0 ELSE 1 END IsEop

FROM
	#locprd lp
ORDER BY
	LocationCode,
	ProductCode";
            //            var query = @"
            //declare @enddate date = eomonth(@startdate)
            //
            //IF OBJECT_ID('tempdb..#loc') IS NOT NULL DROP TABLE #loc
            //IF OBJECT_ID('tempdb..#prodloc') IS NOT NULL DROP TABLE #prodloc
            //IF OBJECT_ID('tempdb..#eop') IS NOT NULL DROP TABLE #eop
            //
            //SELECT 
            //	DISTINCT LocationCode
            //INTO
            //	#loc
            //FROM 
            //	[IN].Eop 
            //	JOIN [IN].StoreLocation l ON l.LocationCode=eop.StoreId AND l.EOP=1
            //WHERE 
            //	EndDate = EOMONTH(@StartDate)
            //
            //CREATE INDEX idx_tmp_loc ON #loc(LocationCode)
            //
            //
            //SELECT
            //	[LocationCode],
            //	ProductCode,
            //	SUM([IN]-[OUT]) as Onhand
            //INTO
            //	#prodloc
            //FROM
            //	[IN].Inventory i
            //	JOIN #loc l ON l.LocationCode=i.Location
            //WHERE
            //	ISNULL(ProductCode,'') <> ''
            //	AND ISNULL(Location,'') <> ''
            //	AND i.CommittedDate < DATEADD(day,1,@enddate)
            //GROUP BY
            //	[LocationCode],
            //	[ProductCode]
            //
            //CREATE INDEX idx_tmp_prodloc_loc_prd ON #prodloc (LocationCode, ProductCode)
            //
            //SELECT
            //	eopdt.EopId,
            //	eopdt.EopDtId,
            //	StoreId as LocationCode,
            //	ProductCode,
            //	Qty
            //INTO
            //	#eop
            //FROM
            //	[IN].Eop
            //	JOIN [IN].EopDt ON eopdt.EopId=eop.EopId
            //WHERE
            //	EndDate = @EndDate
            //
            //CREATE INDEX idx_tmp_eop_loc_prd ON #eop (LocationCode, ProductCode)
            //
            //;WITH
            //r AS(
            //	SELECT
            //		eop.LocationCode,
            //		eop.ProductCode,
            //		eop.Qty,
            //		pl.Onhand,
            //		eop.EopId,
            //		eop.EopDtId
            //	FROM
            //		#eop eop
            //		JOIN #prodloc pl ON pl.ProductCode=eop.ProductCode AND pl.LocationCode=eop.LocationCode
            //	WHERE
            //		Qty <> Onhand
            //
            //	UNION ALL
            //	SELECT
            //		pl.LocationCode,
            //		pl.ProductCode,
            //		NULL as Qty,
            //		pl.Onhand,
            //		NULL as EopId,
            //		NULL as EopDtId
            //	FROM
            //		#prodloc pl
            //		LEFT JOIN #eop eop ON pl.ProductCode=eop.ProductCode AND pl.LocationCode=eop.LocationCode
            //	WHERE
            //		eop.EopId IS NULL
            //		AND pl.Onhand <> 0
            //),
            //r1 AS(
            //	SELECT
            //		*,
            //		CASE WHEN Qty > Onhand THEN Qty-Onhand ELSE 0 END as [IN],
            //		CASE WHEN Qty < Onhand THEN Onhand-Qty ELSE 0 END as [OUT]
            //	FROM
            //		r
            //)
            //SELECT
            //	*,
            //	CASE 
            //        WHEN Qty IS NULL THEN 'Not found this product on EOP'
            //        ELSE ''
            //    END as Remark
            //FROM
            //	r1
            //ORDER BY
            //	LocationCode,
            //	ProductCode
            //
            //
            //IF OBJECT_ID('tempdb..#loc') IS NOT NULL DROP TABLE #loc
            //IF OBJECT_ID('tempdb..#prodloc') IS NOT NULL DROP TABLE #prodloc
            //IF OBJECT_ID('tempdb..#eop') IS NOT NULL DROP TABLE #eop
            //";
            #endregion
            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@startdate", date) });

            _dtDetail = dt;

            gv1.DataSource = _dtDetail.AsEnumerable()
                .Where(x => x.Field<int>("IsEop") == 1)
                .Select(x => new
                {
                    LocationCode = x.Field<string>("LocationCode"),
                    ProductCode = x.Field<string>("ProductCode"),
                    EopQty = x.Field<Nullable<decimal>>("EopQty"),
                    OnhandBeforeEop = x.Field<Nullable<decimal>>("OnhandBeforeEop"),
                    AdjIn = x.Field<Nullable<decimal>>("AdjIn"),
                    AdjOut = x.Field<Nullable<decimal>>("AdjOut"),
                    Onhand = x.Field<Nullable<decimal>>("Onhand"),
                    EopId = x.Field<Nullable<int>>("EopId"),
                    EopDtId = x.Field<Nullable<int>>("EopDtId"),
                })
                .ToArray();
            gv1.DataBind();

            gv2.DataSource = _dtDetail.AsEnumerable()
                .Where(x => x.Field<int>("IsEop") == 0)
                .Select(x => new
                {
                    LocationCode = x.Field<string>("LocationCode"),
                    ProductCode = x.Field<string>("ProductCode"),
                    Onhand = x.Field<Nullable<decimal>>("Onhand"),
                })
                .ToArray();
            gv2.DataBind();
        }


        protected void btn_Export_Click(object sender, EventArgs e)
        {
            if (_dtDetail != null)
            {
                var id = Guid.NewGuid().ToString();
                var text = DataTableToCsv(_dtDetail);

                var path = Path.GetTempPath();
                var fileId = Path.Combine(path, id);
                File.WriteAllText(fileId, text);



                var filename = "EOP-" + ddl_Period.SelectedValue.ToString() + ".csv";

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + filename);
                //Response.TransmitFile(Server.MapPath("~/Application/FileUploads/") + filename);
                Response.TransmitFile(fileId);
                Response.End();
            }
        }

        private string DataTableToCsv(DataTable dataTable)
        {

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }



            return sb.ToString();
        }
    }
}