namespace ITL.ParamsSettingTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.nbNavigation = new DevExpress.XtraNavBar.NavBarControl();
            this.nbgDeviceList = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbItemCloudElevator = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemCloudIntercom = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemLinkageCtrl = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemCloudGroupLinkage = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemGroupLinkControl = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemCouldEntrance = new DevExpress.XtraNavBar.NavBarItem();
            this.nbItemGroupLinkage = new DevExpress.XtraNavBar.NavBarItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlReportContainer = new DevExpress.XtraEditors.PanelControl();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem3 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem4 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem5 = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).BeginInit();
            this.pnlButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).BeginInit();
            this.pnlButtonContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbNavigation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlReportContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
            this.pnlTitle.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Appearance.Options.UseFont = true;
            this.pnlTitle.Size = new System.Drawing.Size(1287, 40);
            // 
            // pnlButtom
            // 
            this.pnlButtom.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlButtom.Appearance.Options.UseFont = true;
            this.pnlButtom.Location = new System.Drawing.Point(0, 850);
            this.pnlButtom.Size = new System.Drawing.Size(1287, 10);
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(925, 18);
            // 
            // pnlMain
            // 
            this.pnlMain.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlMain.Appearance.Options.UseFont = true;
            this.pnlMain.Controls.Add(this.pnlReportContainer);
            this.pnlMain.Controls.Add(this.splitter1);
            this.pnlMain.Controls.Add(this.nbNavigation);
            this.pnlMain.Size = new System.Drawing.Size(1287, 810);
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
            this.pnlButtonContainer.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlButtonContainer.Appearance.Options.UseBackColor = true;
            this.pnlButtonContainer.Appearance.Options.UseFont = true;
            this.pnlButtonContainer.Size = new System.Drawing.Size(1287, 24);
            // 
            // btnMin
            // 
            this.btnMin.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMin.Appearance.Options.UseBackColor = true;
            this.btnMin.Appearance.Options.UseFont = true;
            this.btnMin.Location = new System.Drawing.Point(1215, 0);
            // 
            // btnMax
            // 
            this.btnMax.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMax.Appearance.Options.UseBackColor = true;
            this.btnMax.Appearance.Options.UseFont = true;
            this.btnMax.Location = new System.Drawing.Point(1239, 0);
            // 
            // lblCaption
            // 
            this.lblCaption.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCaption.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblCaption.Location = new System.Drawing.Point(48, 11);
            this.lblCaption.Size = new System.Drawing.Size(200, 27);
            this.lblCaption.Text = "网络设备参数设置工具";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.HotBackColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(1263, 0);
            // 
            // nbNavigation
            // 
            this.nbNavigation.ActiveGroup = this.nbgDeviceList;
            this.nbNavigation.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.nbNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.nbNavigation.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nbNavigation.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.nbgDeviceList});
            this.nbNavigation.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nbItemCloudElevator,
            this.nbItemCloudIntercom,
            this.nbItemLinkageCtrl,
            this.nbItemCloudGroupLinkage,
            this.nbItemGroupLinkControl,
            this.nbItemCouldEntrance,
            this.nbItemGroupLinkage});
            this.nbNavigation.Location = new System.Drawing.Point(1, 1);
            this.nbNavigation.MaximumSize = new System.Drawing.Size(200, 0);
            this.nbNavigation.Name = "nbNavigation";
            this.nbNavigation.OptionsNavPane.ExpandedWidth = 110;
            this.nbNavigation.OptionsNavPane.ShowOverflowButton = false;
            this.nbNavigation.OptionsNavPane.ShowOverflowPanel = false;
            this.nbNavigation.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.nbNavigation.Size = new System.Drawing.Size(110, 808);
            this.nbNavigation.TabIndex = 2;
            this.nbNavigation.Text = "navBarControl1";
            // 
            // nbgDeviceList
            // 
            this.nbgDeviceList.Caption = "设备类型";
            this.nbgDeviceList.Expanded = true;
            this.nbgDeviceList.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
            this.nbgDeviceList.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemCloudElevator),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemCloudIntercom),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemLinkageCtrl),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemCloudGroupLinkage),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemGroupLinkControl),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemCouldEntrance),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbItemGroupLinkage)});
            this.nbgDeviceList.Name = "nbgDeviceList";
            // 
            // nbItemCloudElevator
            // 
            this.nbItemCloudElevator.Caption = "云电梯";
            this.nbItemCloudElevator.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemCloudElevator.Name = "nbItemCloudElevator";
            // 
            // nbItemCloudIntercom
            // 
            this.nbItemCloudIntercom.Caption = "云对讲";
            this.nbItemCloudIntercom.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemCloudIntercom.Name = "nbItemCloudIntercom";
            // 
            // nbItemLinkageCtrl
            // 
            this.nbItemLinkageCtrl.Caption = "协议控制器";
            this.nbItemLinkageCtrl.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemLinkageCtrl.Name = "nbItemLinkageCtrl";
            // 
            // nbItemCloudGroupLinkage
            // 
            this.nbItemCloudGroupLinkage.Caption = "云群控器";
            this.nbItemCloudGroupLinkage.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemCloudGroupLinkage.Name = "nbItemCloudGroupLinkage";
            // 
            // nbItemGroupLinkControl
            // 
            this.nbItemGroupLinkControl.Caption = "云联动器";
            this.nbItemGroupLinkControl.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemGroupLinkControl.Name = "nbItemGroupLinkControl";
            // 
            // nbItemCouldEntrance
            // 
            this.nbItemCouldEntrance.Caption = "云门禁";
            this.nbItemCouldEntrance.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemCouldEntrance.Name = "nbItemCouldEntrance";
            // 
            // nbItemGroupLinkage
            // 
            this.nbItemGroupLinkage.Caption = "群控器";
            this.nbItemGroupLinkage.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.nbItemGroupLinkage.Name = "nbItemGroupLinkage";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(111, 1);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 808);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // pnlReportContainer
            // 
            this.pnlReportContainer.AutoSize = true;
            this.pnlReportContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlReportContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReportContainer.Location = new System.Drawing.Point(114, 1);
            this.pnlReportContainer.Name = "pnlReportContainer";
            this.pnlReportContainer.Size = new System.Drawing.Size(1172, 808);
            this.pnlReportContainer.TabIndex = 4;
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "云联动器";
            this.navBarItem1.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.navBarItem1.Name = "navBarItem1";
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "云门禁";
            this.navBarItem2.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.navBarItem2.Name = "navBarItem2";
            // 
            // navBarItem3
            // 
            this.navBarItem3.Caption = "云门禁";
            this.navBarItem3.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.navBarItem3.Name = "navBarItem3";
            // 
            // navBarItem4
            // 
            this.navBarItem4.Caption = "云门禁";
            this.navBarItem4.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.navBarItem4.Name = "navBarItem4";
            // 
            // navBarItem5
            // 
            this.navBarItem5.Caption = "云门禁";
            this.navBarItem5.LargeImage = global::ITL.ParamsSettingTool.Properties.Resources.NbgIcon_48;
            this.navBarItem5.Name = "navBarItem5";
            // 
            // MainForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1287, 860);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowInTaskbar = true;
            this.Text = "网络设备参数设置工具";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).EndInit();
            this.pnlButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).EndInit();
            this.pnlButtonContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbNavigation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlReportContainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl nbNavigation;
        private DevExpress.XtraNavBar.NavBarGroup nbgDeviceList;
        private DevExpress.XtraNavBar.NavBarItem nbItemCloudElevator;
        private DevExpress.XtraNavBar.NavBarItem nbItemCloudIntercom;
        private DevExpress.XtraNavBar.NavBarItem nbItemLinkageCtrl;
        private DevExpress.XtraEditors.PanelControl pnlReportContainer;
        private System.Windows.Forms.Splitter splitter1;
        private DevExpress.XtraNavBar.NavBarItem nbItemCloudGroupLinkage;
        private DevExpress.XtraNavBar.NavBarItem nbItemGroupLinkControl;
        private DevExpress.XtraNavBar.NavBarItem nbItemCouldEntrance;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem nbItemGroupLinkage;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private DevExpress.XtraNavBar.NavBarItem navBarItem3;
        private DevExpress.XtraNavBar.NavBarItem navBarItem4;
        private DevExpress.XtraNavBar.NavBarItem navBarItem5;
    }
}