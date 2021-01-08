namespace ITL.ParamsSettingTool
{
    partial class GeneralDeviceUserControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gpcDevices = new DevExpress.XtraEditors.GroupControl();
            this.gcDevices = new DevExpress.XtraGrid.GridControl();
            this.gvDevices = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlContainer = new DevExpress.XtraEditors.PanelControl();
            this.gpcOperateArea = new DevExpress.XtraEditors.GroupControl();
            this.Btn_Export = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_BatchSet = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_QuickSet = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_DownLoadDev = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_SaveSet = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadLocalFloorTable = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadCloudFloorTable = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_DownLoadParm = new DevExpress.XtraEditors.SimpleButton();
            this.pnlOperateArea = new DevExpress.XtraEditors.PanelControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tmrCommunication = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gpcDevices)).BeginInit();
            this.gpcDevices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContainer)).BeginInit();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpcOperateArea)).BeginInit();
            this.gpcOperateArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlOperateArea)).BeginInit();
            this.SuspendLayout();
            // 
            // gpcDevices
            // 
            this.gpcDevices.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.gpcDevices.Controls.Add(this.gcDevices);
            this.gpcDevices.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("搜索设备", null)});
            this.gpcDevices.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.gpcDevices.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpcDevices.Location = new System.Drawing.Point(0, 0);
            this.gpcDevices.Name = "gpcDevices";
            this.gpcDevices.Size = new System.Drawing.Size(1153, 253);
            this.gpcDevices.TabIndex = 2;
            this.gpcDevices.Text = "设备列表";
            // 
            // gcDevices
            // 
            this.gcDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDevices.Location = new System.Drawing.Point(2, 33);
            this.gcDevices.MainView = this.gvDevices;
            this.gcDevices.Name = "gcDevices";
            this.gcDevices.Size = new System.Drawing.Size(1149, 218);
            this.gcDevices.TabIndex = 2;
            this.gcDevices.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDevices});
            // 
            // gvDevices
            // 
            this.gvDevices.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDevices.Appearance.Empty.BackColor2 = System.Drawing.Color.White;
            this.gvDevices.Appearance.Empty.Options.UseBackColor = true;
            this.gvDevices.GridControl = this.gcDevices;
            this.gvDevices.Name = "gvDevices";
            // 
            // pnlContainer
            // 
            this.pnlContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlContainer.Controls.Add(this.gpcOperateArea);
            this.pnlContainer.Controls.Add(this.splitter1);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 253);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(1153, 463);
            this.pnlContainer.TabIndex = 3;
            // 
            // gpcOperateArea
            // 
            this.gpcOperateArea.Controls.Add(this.Btn_Export);
            this.gpcOperateArea.Controls.Add(this.Btn_BatchSet);
            this.gpcOperateArea.Controls.Add(this.Btn_QuickSet);
            this.gpcOperateArea.Controls.Add(this.Btn_DownLoadDev);
            this.gpcOperateArea.Controls.Add(this.Btn_SaveSet);
            this.gpcOperateArea.Controls.Add(this.Btn_ReadLocalFloorTable);
            this.gpcOperateArea.Controls.Add(this.Btn_ReadCloudFloorTable);
            this.gpcOperateArea.Controls.Add(this.Btn_DownLoadParm);
            this.gpcOperateArea.Controls.Add(this.pnlOperateArea);
            this.gpcOperateArea.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton("下载参数", null, -1, DevExpress.XtraEditors.ButtonPanel.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, false, null, null, -1)});
            this.gpcOperateArea.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.gpcOperateArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpcOperateArea.Location = new System.Drawing.Point(0, 0);
            this.gpcOperateArea.Name = "gpcOperateArea";
            this.gpcOperateArea.Size = new System.Drawing.Size(1153, 455);
            this.gpcOperateArea.TabIndex = 2;
            this.gpcOperateArea.Text = "参数设置";
            // 
            // Btn_Export
            // 
            this.Btn_Export.Location = new System.Drawing.Point(1045, 413);
            this.Btn_Export.Name = "Btn_Export";
            this.Btn_Export.Size = new System.Drawing.Size(90, 35);
            this.Btn_Export.TabIndex = 8;
            this.Btn_Export.Text = "导出对应表";
            this.Btn_Export.Visible = false;
            // 
            // Btn_BatchSet
            // 
            this.Btn_BatchSet.Location = new System.Drawing.Point(924, 413);
            this.Btn_BatchSet.Name = "Btn_BatchSet";
            this.Btn_BatchSet.Size = new System.Drawing.Size(90, 35);
            this.Btn_BatchSet.TabIndex = 7;
            this.Btn_BatchSet.Text = "批量设置";
            this.Btn_BatchSet.Visible = false;
            // 
            // Btn_QuickSet
            // 
            this.Btn_QuickSet.Location = new System.Drawing.Point(779, 413);
            this.Btn_QuickSet.Name = "Btn_QuickSet";
            this.Btn_QuickSet.Size = new System.Drawing.Size(90, 35);
            this.Btn_QuickSet.TabIndex = 6;
            this.Btn_QuickSet.Text = "快速设置";
            this.Btn_QuickSet.Visible = false;
            // 
            // Btn_DownLoadDev
            // 
            this.Btn_DownLoadDev.Location = new System.Drawing.Point(422, 413);
            this.Btn_DownLoadDev.Name = "Btn_DownLoadDev";
            this.Btn_DownLoadDev.Size = new System.Drawing.Size(140, 35);
            this.Btn_DownLoadDev.TabIndex = 4;
            this.Btn_DownLoadDev.Text = "下载到设备";
            this.Btn_DownLoadDev.Visible = false;
            this.Btn_DownLoadDev.Click += new System.EventHandler(this.Btn_DownLoadDev_Click);
            // 
            // Btn_SaveSet
            // 
            this.Btn_SaveSet.Location = new System.Drawing.Point(623, 413);
            this.Btn_SaveSet.Name = "Btn_SaveSet";
            this.Btn_SaveSet.Size = new System.Drawing.Size(90, 35);
            this.Btn_SaveSet.TabIndex = 5;
            this.Btn_SaveSet.Text = "保存设置";
            this.Btn_SaveSet.Visible = false;
            // 
            // Btn_ReadLocalFloorTable
            // 
            this.Btn_ReadLocalFloorTable.Location = new System.Drawing.Point(211, 413);
            this.Btn_ReadLocalFloorTable.Name = "Btn_ReadLocalFloorTable";
            this.Btn_ReadLocalFloorTable.Size = new System.Drawing.Size(140, 35);
            this.Btn_ReadLocalFloorTable.TabIndex = 3;
            this.Btn_ReadLocalFloorTable.Text = "读取本地楼层对应表";
            this.Btn_ReadLocalFloorTable.Visible = false;
            this.Btn_ReadLocalFloorTable.Click += new System.EventHandler(this.Btn_ReadLocalFloorTable_Click);
            // 
            // Btn_ReadCloudFloorTable
            // 
            this.Btn_ReadCloudFloorTable.Location = new System.Drawing.Point(5, 413);
            this.Btn_ReadCloudFloorTable.Name = "Btn_ReadCloudFloorTable";
            this.Btn_ReadCloudFloorTable.Size = new System.Drawing.Size(140, 35);
            this.Btn_ReadCloudFloorTable.TabIndex = 2;
            this.Btn_ReadCloudFloorTable.Text = "读取云端楼层对应表";
            this.Btn_ReadCloudFloorTable.Visible = false;
            this.Btn_ReadCloudFloorTable.Click += new System.EventHandler(this.Btn_ReadCloudFloorTable_Click);
            // 
            // Btn_DownLoadParm
            // 
            this.Btn_DownLoadParm.Location = new System.Drawing.Point(409, 414);
            this.Btn_DownLoadParm.Name = "Btn_DownLoadParm";
            this.Btn_DownLoadParm.Size = new System.Drawing.Size(90, 35);
            this.Btn_DownLoadParm.TabIndex = 1;
            this.Btn_DownLoadParm.Text = "下载参数";
            this.Btn_DownLoadParm.Click += new System.EventHandler(this.Btn_DownLoadParm_Click);
            // 
            // pnlOperateArea
            // 
            this.pnlOperateArea.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlOperateArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOperateArea.Location = new System.Drawing.Point(2, 27);
            this.pnlOperateArea.Name = "pnlOperateArea";
            this.pnlOperateArea.Size = new System.Drawing.Size(1149, 380);
            this.pnlOperateArea.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 455);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1153, 8);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // tmrCommunication
            // 
            this.tmrCommunication.Tick += new System.EventHandler(this.tmrCommunication_Tick);
            // 
            // GeneralDeviceUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.gpcDevices);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GeneralDeviceUserControl";
            this.Size = new System.Drawing.Size(1153, 716);
            ((System.ComponentModel.ISupportInitialize)(this.gpcDevices)).EndInit();
            this.gpcDevices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContainer)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gpcOperateArea)).EndInit();
            this.gpcOperateArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlOperateArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gpcDevices;
        protected DevExpress.XtraEditors.PanelControl pnlContainer;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDevices;
        protected DevExpress.XtraGrid.GridControl gcDevices;
        private DevExpress.XtraEditors.GroupControl gpcOperateArea;
        protected System.Windows.Forms.Timer tmrCommunication;
        protected DevExpress.XtraEditors.PanelControl pnlOperateArea;
        private System.Windows.Forms.Splitter splitter1;
        //private DevExpress.XtraEditors.SimpleButton Btn_DownLoadParm;
        public DevExpress.XtraEditors.SimpleButton Btn_DownLoadParm;
        public DevExpress.XtraEditors.SimpleButton Btn_ReadLocalFloorTable;
        public DevExpress.XtraEditors.SimpleButton Btn_ReadCloudFloorTable;
        public DevExpress.XtraEditors.SimpleButton Btn_QuickSet;
        public DevExpress.XtraEditors.SimpleButton Btn_SaveSet;
        public DevExpress.XtraEditors.SimpleButton Btn_DownLoadDev;
        public DevExpress.XtraEditors.SimpleButton Btn_Export;
        public DevExpress.XtraEditors.SimpleButton Btn_BatchSet;
    }
}
