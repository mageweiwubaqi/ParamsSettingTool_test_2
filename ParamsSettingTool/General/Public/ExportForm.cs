using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.Public;
using System.IO;
using ITL.Framework;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;

namespace ITL.General
{
    public partial class ExportForm : GeneralForm
    {
        private string f_ExportPath = string.Empty;
        private bool f_IsAutoOpenPath = false;

        public string ExportPath
        {
            get
            {
                return f_ExportPath;
            }
            set
            {
                f_ExportPath = value;
            }
        }

        public bool IsAutoOpenPath
        {
            get
            {
                return f_IsAutoOpenPath;
            }
            set
            {
                f_IsAutoOpenPath = value;
            }
        }

        public ExportForm()
        {
            InitializeComponent();
        }

        protected override void InitUIOnShown()
        {
            base.InitUIOnShown();

            ControlUtilityTool.SetITLTextEditStyle(this.edtPath);

            this.ReadXMLConfig();
            this.RefreshUIByConfig();
            ControlUtilityTool.SetFocus(this.edtPath);
        }

        private void RefreshUIByConfig()
        {
            if((string.IsNullOrWhiteSpace(this.ExportPath)) || (!Directory.Exists(this.ExportPath)))
            {
                this.ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            this.edtPath.Text = this.ExportPath;
            this.chkAutoOpen.Checked = this.IsAutoOpenPath;
        }

        private void ReadXMLConfig()
        {
            this.ExportPath = ExportXMLConfig.Singleton[ExportXMLConfig.EXPORT_PATH].ToString().Trim();
            this.IsAutoOpenPath = ExportXMLConfig.Singleton[ExportXMLConfig.AUTO_OPEN].ToString().Trim() == "1";
        }

        private void SaveXMLConfig()
        {
            ExportXMLConfig.Singleton[ExportXMLConfig.EXPORT_PATH] = this.ExportPath;
            ExportXMLConfig.Singleton[ExportXMLConfig.AUTO_OPEN] = this.IsAutoOpenPath ? "1" : "0";
            ExportXMLConfig.Singleton.Save();
        }

        private void ShowFolderBrowerDialog()
        {
            FolderBrowserDialogEx cfbd = new FolderBrowserDialogEx();
            cfbd.Title = "导出资料";
            cfbd.SelectedPath = Directory.Exists(this.ExportPath) ? this.ExportPath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            cfbd.ShowEditbox = true;
            cfbd.ShowNewFolderButton = true;
            cfbd.RootFolder = Environment.SpecialFolder.MyComputer;
            cfbd.StartPosition = FormStartPosition.CenterScreen;
            cfbd.OKButtonCaption = "确定";
            cfbd.CancelButtonCaption = "取消";
            cfbd.LblFolederText = "路径";
            DialogResult dr = cfbd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.edtPath.Text = cfbd.SelectedPath;
                this.edtPath.SelectionStart = this.edtPath.Text.Length;
            }
        }

        private bool CheckUIValid()
        {
            string exportPath = this.edtPath.Text.Trim();
            if(!Directory.Exists(exportPath))
            {
                HintProvider.ShowAutoCloseDialog(this, "请选择合法的导出路径!");
                return false;
            }
            return true;
        }

        private void GetUIInfo()
        {
            this.ExportPath = this.edtPath.Text.Trim();
            this.IsAutoOpenPath = this.chkAutoOpen.Checked;
        }

        protected override void ExcuteOKPerformClick()
        {
            if(!this.CheckUIValid())
            {
                return;
            }
            this.GetUIInfo();
            this.SaveXMLConfig();
            this.DialogResult = DialogResult.OK;
        }

        private void edtPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if(e.Button.Index != 0)
            {
                return;
            }
            this.ShowFolderBrowerDialog();
        }

        public static bool ExportData(GridControl grdDataSource, string reportName)
        {
            ExportForm oneForm = new ExportForm();
            oneForm.ShowDialog();
            if (oneForm.DialogResult != DialogResult.OK)
            {
                return false;

            }
            string reportFile = Path.Combine(oneForm.ExportPath,
                string.Format("{0}_{1}.xls", reportName, DateTime.Now.ToString("yyyyMMddHHmmss")));

            XlsExportOptions options = new XlsExportOptions();
            options.SheetName = reportName;
            grdDataSource.ExportToXls(reportFile, options);  //,DevExpress.XtraPrinting.XlsExportOptions.;
            if (oneForm.IsAutoOpenPath)
            {
                FileInfo fi = new FileInfo(reportFile);
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start(reportFile);
                }
                else
                {
                    //file doesn't exist
                }
            }
            return true;
        }
    }
}
