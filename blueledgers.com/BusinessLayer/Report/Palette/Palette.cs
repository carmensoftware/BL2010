using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class Palette : DbHandler
    {
        #region "Operations"

        public Palette()
        {
            SelectCommand = "SELECT * FROM Report.Palette";
            TableName = "Palette";
        }

        /// <summary>
        ///     Get Palette data by PaletteCode.
        /// </summary>
        /// <param name="dsPalette"></param>
        /// <param name="PaletteCode"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Get(DataSet dsPalette, string PaletteCode, string ConnectionString)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@PaletteCode", PaletteCode);

            // Get data
            return DbRetrieve("Report.GetPaletteByPaletteCode", dsPalette, dbParams, TableName, ConnectionString);
        }

        /// <summary>
        ///     Get palette name by PaletteCode.
        /// </summary>
        /// <param name="PaletteCode"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public string GetName(string PaletteCode, string ConnectionString)
        {
            var dsPalette = new DataSet();
            var PaletteName = string.Empty;

            // Get data
            var retireved = Get(dsPalette, PaletteCode, ConnectionString);

            if (retireved)
            {
                if (dsPalette.Tables[TableName] != null)
                {
                    if (dsPalette.Tables[TableName].Rows.Count > 0)
                    {
                        PaletteName = dsPalette.Tables[TableName].Rows[0]["Name"].ToString();
                    }
                }
            }

            return PaletteName;
        }

        /// <summary>
        ///     Get only active c data.
        /// </summary>
        /// <returns></returns>
        public DataTable GetActiveList(string constr)
        {
            //return this.DbRead("Report.GetPaletteActiveList", null, userName, buID);
            return DbRead("Report.GetPaletteActiveList", null, constr);
        }

        #endregion
    }
}