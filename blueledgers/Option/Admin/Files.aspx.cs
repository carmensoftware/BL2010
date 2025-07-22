using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.IO;

namespace BlueLedger.PL.Option.Admin
{
    public partial class Files : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();


        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Settings();
            }

        }


        private void Page_Settings()
        {
            UpdateAllSize();
        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            UpdateAllSize();
        }



        #region --Method(s)--

        private string SizeToText(decimal sizeByte)
        {

            var unit = "Byte";
            decimal size = sizeByte;

            if (size > 1024)
            {
                size = size / 1024;
                unit = "KB";
            }

            if (size > 1024)
            {
                size = size / 1024;
                unit = "MB";
            }

            if (size > 1024)
            {
                size = size / 1024;
                unit = "GB";
            }

            if (size > 1024)
            {
                size = size / 1024;
                unit = "TB";
            }

            return string.Format("{0:N} {1}", size, unit);
        }

        private long FolderSize(DirectoryInfo folder)
        {
            long totalSizeOfDir = 0;

            // Get all files into the directory
            FileInfo[] allFiles = folder.GetFiles();

            // Loop through every file and get size of it
            foreach (FileInfo file in allFiles)
            {
                totalSizeOfDir += file.Length;
            }

            // Find all subdirectories
            DirectoryInfo[] subFolders = folder.GetDirectories();

            // Loop through every subdirectory and get size of each
            foreach (DirectoryInfo dir in subFolders)
            {
                totalSizeOfDir += FolderSize(dir);
            }

            // Return the total size of folder
            return totalSizeOfDir;
        }

        private long SubFolderSize(DirectoryInfo[] subFolders)
        {
            long totalSizeOfDir = 0;

            // Loop through every subdirectory and get size of each
            foreach (DirectoryInfo dir in subFolders)
            {
                totalSizeOfDir += FolderSize(dir);
            }

            // Return the total size of folder
            return totalSizeOfDir;
        }

        private string getAttachPath()
        {
            var buCode = LoginInfo.BuInfo.BuCode;

            string attachmentPath = config.GetValue("SYS", "ATTACH", "Path", LoginInfo.ConnStr);

            return @Server.MapPath(attachmentPath + "\\" + buCode);
        }

        private void UpdateAllSize()
        {
            var path = getAttachPath();
            var folder = new DirectoryInfo(path);
            
            decimal folderSize = FolderSize(folder);

            var dtDbSize = config.DbExecuteQuery("SELECT CAST( (SUM(size) * (8.0 * 1024) ) as decimal(18,2)) as Bytes FROM sys.master_files WITH(NOWAIT) WHERE database_id = DB_ID() GROUP BY database_id", null, LoginInfo.ConnStr);
            decimal dataSize = Convert.ToDecimal(dtDbSize.Rows[0][0].ToString());


            lbl_Total.Text = SizeToText(folderSize + dataSize);

            lbl_Data.Text = SizeToText(dataSize);

            lbl_Folder.Text = SizeToText(folderSize);

            var pr = SubFolderSize(folder.GetDirectories("PR*"));
            var po = SubFolderSize(folder.GetDirectories("PO*"));
            var rc = SubFolderSize(folder.GetDirectories("RC*"));
            var cn = SubFolderSize(folder.GetDirectories("CN*"));
            var si = SubFolderSize(folder.GetDirectories("SI*"));
            var so = SubFolderSize(folder.GetDirectories("SO*"));
            var sr = SubFolderSize(folder.GetDirectories("SR*"));
            var other = folderSize - pr - po - rc - cn - si - so - sr;

            other = other < 0 ? 0 : other;

            lbl_PR.Text = SizeToText(pr);
            lbl_PO.Text = SizeToText(po);
            lbl_RC.Text = SizeToText(rc);
            lbl_CN.Text = SizeToText(cn);
            lbl_SI.Text = SizeToText(si);
            lbl_SO.Text = SizeToText(so);
            lbl_SR.Text = SizeToText(sr);
            lbl_Others.Text = SizeToText(other);
        }

        #endregion
    }
}