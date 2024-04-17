using System;

namespace BlueLedger.PL.BaseClass
{
    /// <summary>
    ///     Summary description for LoginInfo
    /// </summary>
    public class LoginInformation
    {
        #region "Attributes"

        private BuFormat _buFmtInfo = new BuFormat();
        private BuInformation _buInfo = new BuInformation();

        /// <summary>
        ///     Gets or set user login name.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///     Gets or set user login first name.
        /// </summary>
        public string FName { get; set; }

        /// <summary>
        ///     Gets or set user login middle name.
        /// </summary>
        public string MName { get; set; }

        /// <summary>
        ///     Gets or set user login last name.
        /// </summary>
        public string LName { get; set; }

        /// <summary>
        ///     Gets or set user login email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or set user login email signature.
        /// </summary>
        public string EmailSign { get; set; }

        /// <summary>
        ///     Gets or set user login department.
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        ///     Gets or set user login division.
        /// </summary>
        public string DivisionCode { get; set; }

        /// <summary>
        ///     Gets or set user login section.
        /// </summary>
        public string SectionCode { get; set; }

        /// <summary>
        ///     Gets or set display langauge.
        /// </summary>
        public string DispLang { get; set; }

        /// <summary>
        ///     Gets or set application theme
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        ///     Gets or set user last login date.
        /// </summary>
        public DateTime LastLogin { get; set; }

        /// <summary>
        ///     Gets or set connection string of selected business unit.
        /// </summary>
        public string ConnStr { get; set; }

        /// <summary>
        ///     Gets or set connection string of selected Message Database.
        /// </summary>
        public string MessageConnStr { get; set; }


        /// <summary>
        ///     Gets or set connection string of selected business unit.
        /// </summary>
        public string HQConnStr { get; set; }

        /// <summary>
        ///     Gets or set business unit information.
        /// </summary>
        public BuInformation BuInfo
        {
            get { return _buInfo; }
            set { _buInfo = value; }
        }

        /// <summary>
        ///     Gets or set business unit format settings
        /// </summary>
        public BuFormat BuFmtInfo
        {
            get { return _buFmtInfo; }
            set { _buFmtInfo = value; }
        }

        #endregion

        #region "Attributes"

        /// <summary>
        ///     Gets user login display name.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return FName + " " + MName + " " + LName;
        }

        #endregion
    }
}