using Blue.DAL;

namespace Blue.BL.Setup.SysAdmin.BU
{
    public class Currency : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Currency()
        {
            SelectCommand = "SELECT * FROM GL.Currency";
            TableName = "Currency";
        }

        #endregion
    }
}