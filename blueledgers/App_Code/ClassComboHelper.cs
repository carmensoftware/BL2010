// -------------------------------------------------------
// FileName : ClassComboHelper.cs
// Modified Date : 2013/12/13 11:38:48

using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web.ASPxEditors;

/// <summary>
///     Summary description for ClassComboHelper
/// </summary>
public class ClassComboHelper
{
    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    /// <param name="Params"></param>
    public void ItemsRequestedByFilterCondition(object source,
        ListEditItemsRequestedByFilterConditionEventArgs e, Params1 Params)
    {
        var comboBox = (ASPxComboBox) source;
        var dtTemp = new DataTable();
        dtTemp.Merge(Params.DtLookUp);
        dtTemp.Clear();

        var s = string.Empty;
        for (var i = 0; i < Params.Fields.Count; i++)
        {
            var item = Params.Fields[i];
            s = string.IsNullOrEmpty(s)
                ? string.Format("{0}", item)
                : string.Format("{0} + ' ' + {1}", s, item);
        }

        var ddt = Params.DtLookUp.Select(s + " like '" + string.Format("*{0}*", e.Filter) + "'");

        if (ddt.Any())
        {
            dtTemp = ddt.CopyToDataTable();
        }

        var dtProjector = dtTemp.Rows.Count > Params.PerPage
            ? dtTemp.AsEnumerable().Skip(e.BeginIndex + 1).Take(e.EndIndex + 1).CopyToDataTable()
            : dtTemp;
        comboBox.DataSource = dtProjector;
        comboBox.DataBind();
    }

    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    /// <param name="Params"></param>
    public void ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e, Params2 Params)
    {
        var comboBox = (ASPxComboBox) source;
        var dtTemp = new DataTable();
        dtTemp.Merge(Params.DtLookUp);
        dtTemp.Clear();
        if (e.Value != null)
        {
            var ddt = Params.DtLookUp.Select(Params.Fields + " = '" + string.Format("{0}", e.Value) + "'");

            if (ddt.Any())
            {
                dtTemp = ddt.CopyToDataTable();
            }
        }

        var dtProjector = dtTemp.Rows.Count > comboBox.CallbackPageSize
            ? dtTemp.AsEnumerable().CopyToDataTable()
            : dtTemp;
        comboBox.DataSource = dtProjector;
        comboBox.DataBind();
    }

    /// <summary>
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public DataTable DataLimit(DataTable dt)
    {
        return DataLimit(dt, 1, 50);
    }

    /// <summary>
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="start"></param>
    /// <param name="record"></param>
    /// <returns></returns>
    public DataTable DataLimit(DataTable dt, int start, int record)
    {
        var t = new DataTable();
        t.Merge(dt);
        t.Clear();

        var p = t.Rows.Count > record ? t.AsEnumerable().Skip(start).Take(record).CopyToDataTable() : t;

        return p;
    }

    /// <summary>
    /// </summary>
    public class Params
    {
        /// <summary>
        ///     DataTable ก้อนใหญ่
        /// </summary>
        public DataTable DtLookUp { get; set; }

        /// <summary>
        ///     ต้องการให้ Fetch ครั้งละเท่าไร
        /// </summary>
        public int PerPage { get; set; }
    }

    /// <summary>
    ///     Field ที่ต้องการให้ search เป็น Contains
    /// </summary>
    public class Params1 : Params
    {
        /// <summary>
        ///     Field ที่ต้องการให้ search เป็น Contains
        /// </summary>
        public List<string> Fields { get; set; }
    }

    /// <summary>
    ///     Field ที่ต้องการให้ search เป็น Equal
    /// </summary>
    public class Params2 : Params
    {
        /// <summary>
        ///     Field ที่ต้องการให้ search เป็น Equal
        /// </summary>
        public string Fields { get; set; }
    }
}