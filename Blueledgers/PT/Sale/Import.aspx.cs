using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.IO;
using System.Data;

namespace BlueLedger.PL.PT.Sale
{
    public partial class Import : BasePage
    {
        private const char _COMMA = ',';
        private const char _TAB = '\t';



        protected FileImport _FileImport
        {
            get { return ViewState["_FileImport"] as FileImport; }
            set
            {
                if (ViewState["_FileImport"] == null)
                    ViewState["_FileImport"] = new FileImport();

                ViewState["_FileImport"] = value;
            }
        }



        // Event(s)
        protected override void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Title = "Recipe - Blueledgers";

                hf_FileName.Value = "";
                hf_FilePath.Value = "";
            }

        }

        protected void btn_UploadFile_Click(object sender, EventArgs e)
        {
            UploadFile();

            //if (!FileUpload.HasFile)
            //{
            //    ShowAlert("Please select a file.");
            //}

            //var file = FileUpload.PostedFile.FileName;
            ////var fileName = Path.GetFileName(file);
            //var fileExt = Path.GetExtension(file).ToLower();

            //var dt = new DataTable();
            //var option = new ConfigFile();

            //option.HeaderLine = 0;
            //option.StartLine = 1;
            //option.NullAsEmpty = true;


            //switch (fileExt)
            //{
            //    case ".xls":
            //    case ".xlsx":
            //        break;
            //    case ".csv":
            //        dt = GetDataByDelimiter(file, _COMMA);
            //        break;
            //    case ".txt":
            //        dt = GetDataByDelimiter(file, _TAB);
            //        break;
            //    default:
            //        ShowAlert(string.Format("{0} is not supported.", file));
            //        return;
            //}

            //gv_Data.DataSource = dt;
            //gv_Data.DataBind();

            //var length = dt.Columns.Count;

            //SetDropdownColumn(ddl_Date, length);
            //SetDropdownColumn(ddl_RevCode, length);
            //SetDropdownColumn(ddl_OutletCode, length);
            //SetDropdownColumn(ddl_DepCode, length);
            //SetDropdownColumn(ddl_ItemCode, length);
            //SetDropdownColumn(ddl_Qty, length);
            //SetDropdownColumn(ddl_Price, length);
            //SetDropdownColumn(ddl_Total, length);

        }

        protected void btn_Setting_Click(object sender, EventArgs e)
        {
            Setting();
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        protected void btn_Import_Click(object sender, EventArgs e)
        {
        }

        protected void chk_ShowAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Private
        private void ShowAlert(string text, string headerText = null)
        {

            lbl_Alert.Text = text;
            pop_Alert.HeaderText = string.IsNullOrEmpty(headerText) ? "Alert" : headerText;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void UploadFile()
        {
            if (FileUpload.HasFile)
            {
                var file = FileUpload.PostedFile.FileName;
                var fileName = Path.GetFileName(file);
                var fileExt = Path.GetExtension(file).ToLower();

                switch (fileExt)
                {
                    case ".xls":
                    case ".xlsx":
                    case ".csv":
                    case ".txt":
                        break;
                    default:
                        ShowAlert(string.Format("{0} is not supported.", file));
                        return;
                }


                var tempPath = Path.GetTempPath();
                var filePath = string.Format("{0}{1}", tempPath, Guid.NewGuid());

                // Save file to temporary path (system)
                FileUpload.SaveAs(filePath);

                hf_FileName.Value = fileName;
                hf_FilePath.Value = filePath;

                //_FileImport.FileName = fileName;
                //_FileImport.FilePath = filePath;

            }
            else
                ShowAlert("Please select a file.");

        }

        private void Preview(bool showAll = true)
        {
            var dt = new DataTable();
            var option = new ConfigFile();

            option.HeaderLine = 0;
            option.StartLine = string.IsNullOrEmpty(txt_StartLine.Text) ? 1 : int.Parse(txt_StartLine.Text);
            option.NullAsEmpty = true;


            var filePath = hf_FilePath.Value;
            var fileName = hf_FileName.Value;
            var ext = Path.GetExtension(fileName).ToLower();

            switch (ext)
            {
                case ".xls":
                case ".xlsx":
                    break;
                case ".csv":
                    dt = GetDataByDelimiter(filePath, _COMMA, showAll);
                    break;
                case ".txt":
                    dt = GetDataByDelimiter(filePath, _TAB, showAll);
                    break;
            }


            gv_Data.DataSource = dt;
            gv_Data.DataBind();
        }

        private void Setting()
        {
            var dt = new DataTable();
            var option = new ConfigFile();

            option.HeaderLine = 0;
            option.StartLine = string.IsNullOrEmpty(txt_StartLine.Text) ? 1 : int.Parse(txt_StartLine.Text);
            option.NullAsEmpty = true;


            var filePath = hf_FilePath.Value;
            var fileName = hf_FileName.Value;
            var ext = Path.GetExtension(fileName).ToLower();

            var showAll = chk_ShowAll.Checked;

            switch (ext)
            {
                case ".xls":
                case ".xlsx":
                    break;
                case ".csv":
                    dt = GetDataByDelimiter(filePath, _COMMA, showAll);
                    break;
                case ".txt":
                    dt = GetDataByDelimiter(filePath, _TAB, showAll);
                    break;
            }


            gv_Data.DataSource = dt;
            gv_Data.DataBind();
        }



        private DataTable GetDataByDelimiter(string file, char delimiter, bool showAll = true)
        {
            string[] lines = File.ReadAllLines(file);
            var columns = lines[0].Split(delimiter);

            var dt = new DataTable();
            var ch = 65; // A

            dt.Columns.Add("Line", typeof(int));

            for (int i = 0; i < columns.Length; i++)
            {
                var colName = (char)ch++;

                dt.Columns.Add(colName.ToString(), typeof(string));
            }

            var rows = lines.Length;

            if (!showAll)
            {
                rows = rows < 5 ? rows : 5;
            }

            // data
            for (int lineNo = 0; lineNo < rows; lineNo++)
            {
                var line = lines[lineNo].Split(delimiter);

                var dr = dt.NewRow();

                dr[0] = lineNo + 1;

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    var value = line[i - 1].ToString();

                    dr[i] = value;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private void SetDropdownColumn(DropDownList dropdown, int length)
        {
            var ch = 65;

            dropdown.Items.Clear();
            dropdown.Items.Add(new ListItem
            {
                Value = "",
                Text = "Not Set"
            });

            for (var i = 0; i < length - 1; i++)
            {
                var value = (char)ch++;
                dropdown.Items.Add(new ListItem
                {
                    Value = value.ToString(),
                    Text = value.ToString()
                });
            }

        }

        protected class ConfigFile
        {
            public int HeaderLine { get; set; }

            public int StartLine { get; set; }

            public bool NullAsEmpty { get; set; }
        }

        protected class FileImport
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }

    }
}