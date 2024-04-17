using System.Data;
using System.Globalization;
using System.Text;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class ViewHandlerOrder : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public ViewHandlerOrder()
        {
            SelectCommand = "SELECT * FROM APP.ViewHandlerOrder";
            TableName = "ViewHandlerOrder";
        }

        public bool GetList(DataSet dsViewHandlerOrder, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            return DbRetrieve("APP.GetViewHandlerOrderListByViewNo", dsViewHandlerOrder, dbParams, TableName, connStr);
        }

        public string GetSortingList(int viewNo, string connStr)
        {
            var dsViewHandlerOrder = new DataSet();
            var sbSortingList = new StringBuilder();

            var result = GetList(dsViewHandlerOrder, viewNo, connStr);

            if (result)
            {
                foreach (DataRow drViewHandlerOrder in dsViewHandlerOrder.Tables[TableName].Rows)
                {
                    sbSortingList.Append((sbSortingList.ToString() == "" ? "" : ", "));
                    sbSortingList.AppendFormat("[{0}] ", drViewHandlerOrder["FieldName"]);
                    sbSortingList.Append(drViewHandlerOrder["OrderType"]);
                }

                return sbSortingList.ToString();
            }

            return string.Empty;
        }

        #endregion
    }
}