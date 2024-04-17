using System;
using System.Globalization;

namespace BlueLedger.PL.BaseClass
{
    /// <summary>
    ///     Initial neccessary variable which widely used in presentation layer
    /// </summary>
    public class BaseUserControl : System.Web.UI.UserControl
    {
        #region "Attributes"        

        /// <summary>
        ///     Attach from file
        /// </summary>
        public enum AttachFromFile
        {
            JournalVourcher = 1,
            Budget = 2,
            Account = 3,
            StandartVourcher = 4,
            AccountConsolidate = 5,
            Vendor = 6,
            Debtor = 7,
            Invoice = 8,
            Payment = 9,
            Receipt = 10,
            AccountReconcile = 11
        }

        /// <summary>
        ///     Mode for user control
        /// </summary>
        public enum AttachMode
        {
            Display = 1,
            Edit = 2
        }

        /// <summary>
        /// </summary>
        public enum OccursEveryType
        {
            Hour = 0,
            Minute = 1
        }

        /// <summary>
        ///     Flag to determine the action of detail page. (New/Edit transaction)
        /// </summary>
        public enum PageAction
        {
            New,
            Edit
        }

        /// <summary>
        ///     Status of datarow to determine command type (update, insert or delete)
        /// </summary>
        public enum RowState
        {
            Unchanged,
            Modified,
            Added,
            Deleted
        }

        /// <summary>
        ///     Data of Schedule Type
        /// </summary>
        public enum ScheduleType
        {
            OneTime = 0,
            Recurring = 1
        }

        /// <summary>
        ///     Detail of Standard Voucher Type.
        /// </summary>
        public enum StandardVoucherType
        {
            AllocationAmount = 1,
            AllocationPercentage = 2,
            AllocationAverage = 3,
            Recurring = 4
        }

        private readonly Blue.BL.ProjectAdmin.SysParameter _sysParameter = new Blue.BL.ProjectAdmin.SysParameter();

        /// <summary>
        ///     Gets current login user information.
        /// </summary>
        protected LoginInformation LoginInfo
        {
            get { return (LoginInformation) Session["LoginInfo"]; }
        }

        /// <summary>
        ///     Gets current date time base on server datetime and current business regional setting.
        /// </summary>
        protected DateTime ServerDateTime
        {
            get { return Blue.BL.GnxLib.GetSysDate(string.Empty); }
        }

        /// <summary>
        ///     DateTime format depend on culture or manual configuration.
        /// </summary>
        protected string DateTimeFormat
        {
            get
            {
                var culture = new CultureInfo(_sysParameter.GetValue("System", "Culture", LoginInfo.ConnStr));
                var dateTimePattern = _sysParameter.GetValue("System", "DateTimePattern", LoginInfo.ConnStr);

                return (dateTimePattern != string.Empty ? dateTimePattern : culture.DateTimeFormat.ShortDatePattern);
            }
        }

        /// <summary>
        ///     Project name.
        /// </summary>
        protected string AppName
        {
            get { return _sysParameter.GetValue("System", "AppName", LoginInfo.ConnStr); }
        }

        #endregion

        #region "Operations"

        #endregion
    }
}