using System;
using System.Data;

/// <summary>
///     Function for cal FIFO or Average {ยังคิดไม่ออก}
///     Modify by "OP"
/// </summary>
public class CalcCostType
{
    #region "Attributes"

    //BlueLedger.BL.IN.ProdUnit prodUnit = new BlueLedger.BL.IN.ProdUnit();
    //BlueLedger.BL.IN.Inventory inv = new BlueLedger.BL.IN.Inventory();
    //public BlueLedger.BL.IN.Inventory Inv { get; set; }

    #endregion "Attributes"

    /// <summary>
    /// 
    /// </summary>
    public enum CalcType
    {
        Average = 0,
        FIFO = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MoveType
    {
        RC,
        SR,
        TR,
        SO,
        SI,
        EOF,
        CN
    };

    public CalcType CostType { get; set; }
    public DataSet DsSave { get; set; }

    public DocLedger Ledgers { get; set; }
    public string Constr { get; set; }
    public string ErrorMsg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class DocLedger
    {
        public string StoreLocCode { get; set; }
        public string ItemGroup { get; set; }
        public string ProductCode { get; set; }
        public MoveType MovementType { get; set; }
        public decimal ReqQty { get; set; }
    }
}

public class CalcCostTypeAvg : CalcCostType
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GenLedger()
    {
        return GenLedger(DsSave, Ledgers, Constr);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dsSave"></param>
    /// <param name="ledger"></param>
    /// <param name="conStr"></param>
    /// <returns></returns>
    public bool GenLedger(DataSet dsSave, DocLedger ledger, string conStr)
    {
        try
        {
            var item = new CalcCostType
            {
                DsSave = dsSave,
                Ledgers = new DocLedger
                {
                    ItemGroup = ledger.ItemGroup,
                    StoreLocCode = ledger.StoreLocCode,
                    ProductCode = ledger.ProductCode,
                    MovementType = ledger.MovementType,
                    ReqQty = ledger.ReqQty,
                },
                Constr = conStr
            };

            // --- Remove Error //
            item.Constr = "";

            return true;
        }
        catch (Exception e)
        {
            LogManager.Error(e);
            ErrorMsg = e.Message;
            return false;
        }
    }
}

/*
public class CalcCostTypeFifo : CalcCostType
{
    public bool GenLedger()
    {
        return GenLedger(this.DsSave, this.StoreLocCode, this.ProductCode, this.MovementType, this.ReqQty, this.Constr);
    }

    public bool GenLedger(DataSet dsSave, string storeLocCode, string productCode, MoveType movementType, decimal reqQty, string conStr)
    {
        try
        {
            var item = new CalcCostType
            {
                DsSave = dsSave,
                StoreLocCode = storeLocCode,
                ProductCode = productCode,
                MovementType = movementType,
                ReqQty = reqQty,
                Constr = conStr
            };



            return true;
        }
        catch (Exception e)
        {
            LogManager.Error(e);
            this.ErrorMsg = e.Message;
            return false;
        }
    }

}
*/