namespace BlueLedger.PL.BaseClass
{
    /// <summary>
    ///     Summary description for BuInfo
    /// </summary>
    public class BuInformation
    {
        #region "Attributes"


        /// <summary>
        ///     Gets or set business unit code.
        /// </summary>
        public string BuCode { get; set; }

        /// <summary>
        ///     Gets or set business unit name.
        /// </summary>
        public string BuName { get; set; }

        public string ServerName { get; set; }

        public string DBName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        /// <summary>
        ///     Gets or set business unit group code.
        /// </summary>
        public string BuGrpCode { get; set; }

        /// <summary>
        ///     Gets or set head quater definition.
        /// </summary>
        public bool IsHQ { get; set; }

        /// <summary>
        ///     Get or set HQ Bu Code of selected business unit.
        /// </summary>
        public string HQBuCode { get; set; }

        #endregion

        #region "Operations"

        #endregion
    }
}