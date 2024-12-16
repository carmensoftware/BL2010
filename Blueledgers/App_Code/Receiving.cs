using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlueLedger.PL.BaseClass;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Receiving functions
/// </summary>
namespace BlueLedger.PL.IN.REC
{
    public partial class RecFunc : BasePage
    {

        #region "Attributes"


        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();

        static string _connectionString = string.Empty;

        #endregion // Atttributes

        #region "Methods"

        public RecFunc()
        {
        }

        public RecFunc(string connString)
        {
            _connectionString = connString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void UpdateInventoryForCommit(DataSet dsSave, DataRow drRecDt)
        {

            string costMethod = config.GetValue("IN", "SYS", "COST", _connectionString);


            if (costMethod == "AVCO") // Average Cost
            {
                var drInv = dsSave.Tables[inv.TableName].NewRow();

                string productCode = drRecDt["ProductCode"].ToString();

                decimal currRate = Convert.ToDecimal(drRecDt["Rate"].ToString()) == 0 ? Convert.ToDecimal("1") : Convert.ToDecimal(drRecDt["Rate"].ToString());
                decimal inQty = (Convert.ToDecimal(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString())) * currRate;
                DateTime atDate = dsSave.Tables[rec.TableName].Rows[0].Field<DateTime>("CommitDate");

                drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
                drInv["InvNo"] = 1;
                drInv["ProductCode"] = productCode;
                drInv["Location"] = drRecDt["LocationCode"].ToString();
                drInv["IN"] = inQty;
                drInv["OUT"] = Convert.ToDecimal("0.00");
                drInv["Amount"] = 0;
                drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                drInv["Type"] = "RC";

                decimal extraCost = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
                drInv["PriceOnLots"] = Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCost;

                dsSave.Tables[inv.TableName].Rows.Add(drInv);

            }
            else  //FIFO Calculation
            {

                var TotalRecvQty = decimal.Parse(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString());

                var invQty = TotalRecvQty * Convert.ToDecimal(drRecDt["Rate"].ToString());
                decimal extraCostFifo = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
                var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCostFifo, 2);
                var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
                var FifoCost = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
                var diffCount = Math.Round((netPrice - (pricePerQty * (invQty == 0 ? 1 : invQty))), 2) * 100;

                if (diffCount == 0)
                {
                    var drInv = dsSave.Tables[inv.TableName].NewRow();

                    drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                    drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
                    drInv["InvNo"] = 1;
                    drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
                    drInv["Location"] = drRecDt["LocationCode"].ToString();
                    drInv["IN"] = invQty;
                    drInv["OUT"] = Convert.ToDecimal("0.00");
                    drInv["Amount"] = pricePerQty;
                    drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                    drInv["Type"] = "RC";

                    drInv["PriceOnLots"] = netPrice; // Keep NetAmt

                    dsSave.Tables[inv.TableName].Rows.Add(drInv);
                }
                else
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var drInv = dsSave.Tables[inv.TableName].NewRow();

                        drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                        drInv["DtNo"] = drRecDt["RecDtNo"];
                        drInv["InvNo"] = i + 1;
                        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
                        drInv["Location"] = drRecDt["LocationCode"].ToString();

                        if (i == 0)
                        {
                            drInv["IN"] = invQty - Math.Abs(diffCount);
                            //drInv["FIFOAudit"] = FifoCost;
                            drInv["Amount"] = pricePerQty;
                        }
                        else
                        {
                            drInv["IN"] = Math.Abs(diffCount);

                            if (diffCount < 0)
                            {
                                //case : QuantityPerUnit * Quantity > Net
                                drInv["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
                                drInv["FIFOAudit"] = FifoCost - Convert.ToDecimal("0.01");
                            }
                            else
                            {
                                //case : QuantityPerUnit * Quantity < Net
                                drInv["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
                                //drInv["FIFOAudit"] = FifoCost + Convert.ToDecimal("0.01");
                            }
                        }

                        drInv["OUT"] = 0;
                        drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                        drInv["Type"] = "RC";
                        drInv["PriceOnLots"] = Math.Round(pricePerQty * Convert.ToDecimal(drInv["IN"].ToString()), 2); // Keep NetAmt

                        dsSave.Tables[inv.TableName].Rows.Add(drInv);

                    }
                }
            }  //else (FIFO)
        }



        #endregion // Methods


        public void InsertInventory(string connectionString, string docNo, DateTime committedDate)
        {

            // Remove prevoious committed if avaiable
            var query = "DELETE FROM [IN].Inventory WHERE [Type]='RC' AND HdrNo=@HdrNo";

            ExecuteNoneQuery(connectionString, query, new SqlParameter[] { new SqlParameter("@HdrNo", docNo) });

            var queryInsertInventory = "INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots,[Type], CommittedDate)";
            var builder = new StringBuilder();

            if (IsAverageCost(connectionString))
            #region --Average--
            {
                builder.Clear();
                builder.AppendLine(queryInsertInventory);
                builder.Append(@"
SELECT
		d.RecNo,
		d.RecDtNo,
		1,
		d.ProductCode,
		d.LocationCode,
		ROUND(RecQty * ISNULL(Rate,0), app.DigitQty()),
		0,
		ROUND(NetAmt/ROUND(RecQty * Rate, app.DigitQty()), app.DigitAmt()),
		NetAmt,
		'RC',
		@CommittedDate
	FROM
		PC.REC h
		JOIN PC.RECDt d
			ON d.RecNo=h.RecNo
	WHERE
		h.RecNo = @DocNo;

EXEC [IN].[UpdateAverageCost] @DocNo
");
                var parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@DocNo", docNo));
                parameters.Add(new SqlParameter("@CommittedDate", committedDate));

                var affected = ExecuteNoneQuery(connectionString, builder.ToString(), parameters);
            }
            #endregion
            else
            #region --FIFO--
            {
                builder.Clear();
                //builder.AppendLine(queryInsertInventory);
                builder.AppendFormat(@"DECLARE @DocNo nvarchar(20)='{0}', @CommittedDate DATETIME = '{1}';", docNo, committedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.AppendLine(@"
DELETE FROM [IN].Inventory WHERE HdrNo=@docno

DECLARE 
	@DigitQty INT = APP.DigitQty(),
	@DigitAmt INT = APP.DigitAmt(),
	@Price decimal(18, 4),
	@DiffAmt decimal(18, 4),
	@DiffQty decimal(18, 3),
	@Qty1 decimal(18,3),
	@Qty2 decimal(18,3),
	@Amt1 decimal(18,4),
	@Amt2 decimal(18,4)

DECLARE 
	@HdrNo nvarchar(20),
	@DtNo INT,
	@InvNo INT,
	@ProductCode nvarchar(20),
	@LocationCode nvarchar(20),
	@Qty decimal(18,3),
	@Total decimal(18,4)


DECLARE rc_cursor CURSOR FOR
SELECT
	RecNo,
	RecDtNo,
	ProductCode,
	LocationCode,
	ROUND(RecQty * ISNULL(Rate,0), @DigitQty) as Qty,
	NetAmt + ISNULL(ExtraCost,0) as Total
FROM
	PC.RECDt
WHERE
	RecNo=@DocNo

OPEN rc_cursor
FETCH NEXT FROM rc_cursor INTO @HdrNo, @DtNo, @ProductCode, @LocationCode, @Qty, @Total

WHILE @@FETCH_STATUS=0
BEGIN
	SET @Price  = ROUND(@Total/@Qty, APP.DigitAmt())
	SET @DiffAmt = @Total - ROUND(@Price * @Qty, @DigitAmt)
	SET @DiffQty = ABS( (@DiffAmt % 1) * POWER(10, APP.DigitQty()))

	DECLARE @p decimal(18,4) = (CAST(1 as decimal(18,4))/POWER(10,@DigitAmt))

	IF @DiffAmt <= 0
	BEGIN
		SET @Qty1 = @Qty - @DiffQty
		SET @Amt1 = @Price

		SET @Qty2 = @DiffQty
		SET @Amt2 = @Price - @p
	END
	ELSE
	BEGIN
		SET @Qty1 = @DiffQty
		SET @Amt1 = @Price - @p

		SET @Qty2 = @Qty - @DiffQty
		SET @Amt2 = @Price
	END

	INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots,[Type], CommittedDate) 
	SELECT 
		@HdrNo as HdrNo, 
		@DtNo as DtNo,
		1 as InvNo,
		@ProductCode as ProductCode,
		@LocationCode as [Location],
		@Qty1 as [IN],
		0 as [OUT],
		@Amt1 as Amount,
		ROUND(@Qty1 * @Amt1, @DigitAmt) as PriceOnLots,
		'RC' as [Type],
		@CommittedDate as CommittedDate

	IF (@DiffQty <> 0)
	BEGIN
		INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots,[Type], CommittedDate) 
		SELECT 
			@HdrNo as HdrNo, 
			@DtNo as DtNo,
			2 as InvNo,
			@ProductCode as ProductCode,
			@LocationCode as [Location],
			@Qty2 as [IN],
			0 as [OUT],
			@Amt2 as Amount,
			ROUND(@Qty2 * @Amt2, @DigitAmt) as PriceOnLots,
			'RC' as [Type],
			@CommittedDate as CommittedDate
	END

	FETCH NEXT FROM rc_cursor INTO @HdrNo, @DtNo, @ProductCode, @LocationCode, @Qty, @Total
END

CLOSE rc_cursor
DEALLOCATE rc_cursor");
                //var parameters = new List<SqlParameter>();

                //parameters.Add(new SqlParameter("@DocNo", docNo));
                //parameters.Add(new SqlParameter("@CommittedDate", committedDate));
                //parameters.Add(new SqlParameter("@p_DocNo", docNo));
                //parameters.Add(new SqlParameter("@p_CommittedDate", committedDate));

                //var affected = ExecuteNoneQuery(connectionString, builder.ToString(), parameters);
                var affected = ExecuteNoneQuery(connectionString, builder.ToString());
            }
            #endregion

        }


        private bool IsAverageCost(string connectionString)
        {
            var query = "SELECT [Value] FROM APP.Config WHERE [Module]='IN' AND SubModule='SYS' AND [Key]='COST'";

            var dt = ExecuteQuery(connectionString, query);
            if (dt != null && dt.Rows.Count > 0)
            {
                var result = dt.Rows[0][0].ToString().Trim();

                return result != "FIFO";
            }
            else
                return true;
        }


        // Method(s) -- SQL
        public DataTable ExecuteQuery(string connectionString, string query, IEnumerable<SqlParameter> parameters = null)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.ToArray().Length > 0)
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

        public int ExecuteNoneQuery(string connectionString, string query, IEnumerable<SqlParameter> parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    if (parameters != null && parameters.ToArray().Length > 0)
                    {
                        foreach (var p in parameters)
                        {
                            command.Parameters.AddWithValue(p.ParameterName, p.Value);
                        }
                    }
                    command.Connection.Open();
                    return (int)command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using BlueLedger.PL.BaseClass;
//using System.Data;
//using System.Data.SqlClient;

///// <summary>
///// Receiving functions
///// </summary>
//namespace BlueLedger.PL.IN.REC
//{
//    public partial class RecFunc : BasePage
//    {

//        #region "Attributes"


//        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
//        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
//        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
//        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();

//        static string _connectionString = string.Empty;

//        #endregion // Atttributes

//        #region "Methods"

//        public RecFunc()
//        {
//        }

//        public RecFunc(string connString)
//        {
//            _connectionString = connString;
//        }

//        public void SetConnectionString(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public void UpdateInventoryForCommit(DataSet dsSave, DataRow drRecDt)
//        {

//            string costMethod = config.GetValue("IN", "SYS", "COST", _connectionString);


//            if (costMethod == "AVCO") // Average Cost
//            {
//                var drInv = dsSave.Tables[inv.TableName].NewRow();

//                string productCode = drRecDt["ProductCode"].ToString();

//                decimal currRate = Convert.ToDecimal(drRecDt["Rate"].ToString()) == 0 ? Convert.ToDecimal("1") : Convert.ToDecimal(drRecDt["Rate"].ToString());
//                decimal inQty = (Convert.ToDecimal(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString())) * currRate;
//                DateTime atDate = dsSave.Tables[rec.TableName].Rows[0].Field<DateTime>("CommitDate");

//                drInv["HdrNo"] = drRecDt["RecNo"].ToString();
//                drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
//                drInv["InvNo"] = 1;
//                drInv["ProductCode"] = productCode;
//                drInv["Location"] = drRecDt["LocationCode"].ToString();
//                drInv["IN"] = inQty;
//                drInv["OUT"] = Convert.ToDecimal("0.00");
//                drInv["Amount"] = 0;
//                drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
//                drInv["Type"] = "RC";

//                decimal extraCost = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
//                drInv["PriceOnLots"] = Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCost;  //+ Convert.ToDecimal(drRecDt["ExtraCost"]);  // Keep total net amount for calculating average cost

//                //drInv["FIFOAudit"] = System.DBNull.Value;
//                //drInv["FIFOMng"] = System.DBNull.Value;
//                //drInv["FIFOBank"] = System.DBNull.Value;
//                //drInv["MAvgAudit"] = System.DBNull.Value;
//                //drInv["MAvgMng"] = System.DBNull.Value;
//                //drInv["MAvgBank"] = System.DBNull.Value;
//                //drInv["PAvgAudit"] = System.DBNull.Value;
//                //drInv["PAvgMng"] = System.DBNull.Value;
//                //drInv["PAvgBank"] = System.DBNull.Value;
//                //drInv["RptAudit"] = System.DBNull.Value;
//                //drInv["RptMng"] = System.DBNull.Value;
//                //drInv["RptBank"] = System.DBNull.Value;

//                dsSave.Tables[inv.TableName].Rows.Add(drInv);

//            }
//            else  //FIFO Calculation
//            {

//                var TotalRecvQty = decimal.Parse(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString());

//                var invQty = TotalRecvQty * Convert.ToDecimal(drRecDt["Rate"].ToString());
//                decimal extraCostFifo = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
//                var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCostFifo, 2);
//                var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
//                var FifoCost = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
//                var diffCount = Math.Round((netPrice - (pricePerQty * (invQty == 0 ? 1 : invQty))), 2) * 100;

//                if (diffCount == 0)
//                {
//                    var drInv = dsSave.Tables[inv.TableName].NewRow();

//                    drInv["HdrNo"] = drRecDt["RecNo"].ToString();
//                    drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
//                    drInv["InvNo"] = 1;
//                    drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
//                    drInv["Location"] = drRecDt["LocationCode"].ToString();
//                    drInv["IN"] = invQty;
//                    drInv["OUT"] = Convert.ToDecimal("0.00");
//                    drInv["Amount"] = pricePerQty;
//                    drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
//                    drInv["Type"] = "RC";

//                    //drInv["PriceOnLots"] = dsSave.Tables[recDt.TableName].Rows[0]["NetAmt"]; // Keep NetAmt
//                    drInv["PriceOnLots"] = netPrice; // Keep NetAmt

//                    //drInv["FIFOAudit"] = System.DBNull.Value;
//                    //drInv["FIFOMng"] = System.DBNull.Value;
//                    //drInv["FIFOBank"] = System.DBNull.Value;
//                    //drInv["MAvgAudit"] = System.DBNull.Value;
//                    //drInv["MAvgMng"] = System.DBNull.Value;
//                    //drInv["MAvgBank"] = System.DBNull.Value;
//                    //drInv["PAvgAudit"] = System.DBNull.Value;
//                    //drInv["PAvgMng"] = System.DBNull.Value;
//                    //drInv["PAvgBank"] = System.DBNull.Value;
//                    //drInv["RptAudit"] = System.DBNull.Value;
//                    //drInv["RptMng"] = System.DBNull.Value;
//                    //drInv["RptBank"] = System.DBNull.Value;

//                    dsSave.Tables[inv.TableName].Rows.Add(drInv);


//                }
//                else
//                {
//                    for (var i = 0; i < 2; i++)
//                    {
//                        var drInv = dsSave.Tables[inv.TableName].NewRow();

//                        drInv["HdrNo"] = drRecDt["RecNo"].ToString();
//                        drInv["DtNo"] = drRecDt["RecDtNo"];
//                        drInv["InvNo"] = i + 1;
//                        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
//                        drInv["Location"] = drRecDt["LocationCode"].ToString();

//                        if (i == 0)
//                        {
//                            drInv["IN"] = invQty - Math.Abs(diffCount);
//                            //drInv["FIFOAudit"] = FifoCost;
//                            drInv["Amount"] = pricePerQty;
//                        }
//                        else
//                        {
//                            drInv["IN"] = Math.Abs(diffCount);

//                            if (diffCount < 0)
//                            {
//                                //case : QuantityPerUnit * Quantity > Net
//                                drInv["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
//                                drInv["FIFOAudit"] = FifoCost - Convert.ToDecimal("0.01");
//                            }
//                            else
//                            {
//                                //case : QuantityPerUnit * Quantity < Net
//                                drInv["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
//                                //drInv["FIFOAudit"] = FifoCost + Convert.ToDecimal("0.01");
//                            }
//                        }

//                        drInv["OUT"] = 0;
//                        drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
//                        drInv["Type"] = "RC";
//                        drInv["PriceOnLots"] = Math.Round(pricePerQty * Convert.ToDecimal(drInv["IN"].ToString()), 2); // Keep NetAmt

//                        //drInv["FIFOMng"] = System.DBNull.Value;
//                        //drInv["FIFOBank"] = System.DBNull.Value;
//                        //drInv["MAvgAudit"] = System.DBNull.Value;
//                        //drInv["MAvgMng"] = System.DBNull.Value;
//                        //drInv["MAvgBank"] = System.DBNull.Value;
//                        //drInv["PAvgAudit"] = 0;
//                        //drInv["PAvgMng"] = System.DBNull.Value;
//                        //drInv["PAvgBank"] = System.DBNull.Value;
//                        //drInv["RptAudit"] = System.DBNull.Value;
//                        //drInv["RptMng"] = System.DBNull.Value;
//                        //drInv["RptBank"] = System.DBNull.Value;

//                        dsSave.Tables[inv.TableName].Rows.Add(drInv);

//                    }
//                }
//            }  //else (FIFO)
//        }


//        #endregion // Methods
//    }
//}