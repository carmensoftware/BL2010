namespace BlueLedger.PL.BaseClass
{
    /// <summary>
    ///     Summary description for BuFormat
    /// </summary>
    public class BuFormat
    {
        #region "Attributes"

        /// <summary>
        ///     Gets or set business unit code.
        /// </summary>
        public string BuCode { get; set; }

        /// <summary>
        ///     Gets or set business unit UI style (Theme).
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        ///     Gets or set format code.
        /// </summary>
        public string FmtCode { get; set; }

        /// <summary>
        ///     Gets or set short date format.
        /// </summary>
        public string FmtSDate { get; set; }

        /// <summary>
        ///     Gets or set long date format.
        /// </summary>
        public string FmtLDate { get; set; }

        /// <summary>
        ///     Gets or set short time format.
        /// </summary>
        public string FmtSTime { get; set; }

        /// <summary>
        ///     Gets or set long time format.
        /// </summary>
        public string FmtLTime { get; set; }

        /// <summary>
        ///     Gets or set AM symbol
        /// </summary>
        public string FmtAM { get; set; }

        /// <summary>
        ///     Gets or set PM symbol
        /// </summary>
        public string FmtPM { get; set; }

        /// <summary>
        ///     Gets or set first day of week.
        /// </summary>
        public string FmtFirstDOW { get; set; }

        /// <summary>
        ///     Gets or set decimal symbol.
        /// </summary>
        public string FmtNumDec { get; set; }

        /// <summary>
        ///     Gets or set no. of digits after decimal symbol.
        /// </summary>
        public int FmtNumDecNo { get; set; }

        /// <summary>
        ///     Gets or set grouping symbol.
        /// </summary>
        public string FmtNumDecGrp { get; set; }

        /// <summary>
        ///     Gets or set negative symbol.
        /// </summary>
        public string FmtNumNeg { get; set; }

        /// <summary>
        ///     Gets or set currency symbol.
        /// </summary>
        public string FmtCurrency { get; set; }

        /// <summary>
        ///     Gets or set currency decimal symbol.
        /// </summary>
        public string FmtCurrencyDec { get; set; }

        /// <summary>
        ///     Gets or set currency no. of digits after decimal symbol.
        /// </summary>
        public int FmtCurrencyDecNo { get; set; }

        /// <summary>
        ///     Gets or set currency decimal grouping symbol.
        /// </summary>
        public string FmtCurrencyDecGrp { get; set; }

        /// <summary>
        ///     Gets or set country code.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        ///     Gets or set UTC code.
        /// </summary>
        public string UTCCode { get; set; }

        /// <summary>
        ///     Gets or set language code.
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        ///     Gets or set other langauge code
        /// </summary>
        public string LangCodeOth { get; set; }

        /// <summary>
        ///     Gets or set flag to determine the selected langauge is match the business unit's defaul language.
        /// </summary>
        public bool IsDefaultLangCode { get; set; }


        // -------------

        /// <summary>
        ///     Gets or Set local numeric format of string ex. 1,000.50
        /// </summary>
        public string LocalNumericFormat { get; set; }

        #endregion

        #region "Operations"



        #endregion
    }
}