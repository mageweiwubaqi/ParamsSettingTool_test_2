namespace ITL.ParamsSettingTool.FloorTable
{
    partial class ReadCloudFloorTableForm
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
            this.grc_ReadCloudFloorTableTitle = new DevExpress.XtraEditors.GroupControl();
            this.pnl_ReadCloudFloorTable = new DevExpress.XtraEditors.PanelControl();
            this.TextEdit_ProNo = new DevExpress.XtraEditors.TextEdit();
            this.Lbc_ProNo = new DevExpress.XtraEditors.LabelControl();
            this.Lbc_Elevator = new DevExpress.XtraEditors.LabelControl();
            this.cBbE_Elevator = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Btn_ReadcloudFloorTable_OK = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadcloudFloorTable_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadcloudFloorTable_GetData = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).BeginInit();
            this.pnl_ReadCloudFloorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_ProNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBbE_Elevator.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grc_ReadCloudFloorTableTitle
            // 
            this.grc_ReadCloudFloorTableTitle.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grc_ReadCloudFloorTableTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.grc_ReadCloudFloorTableTitle.Location = new System.Drawing.Point(0, 0);
            this.grc_ReadCloudFloorTableTitle.Name = "grc_ReadCloudFloorTableTitle";
            this.grc_ReadCloudFloorTableTitle.Size = new System.Drawing.Size(350, 27);
            this.grc_ReadCloudFloorTableTitle.TabIndex = 0;
            this.grc_ReadCloudFloorTableTitle.Text = "读取楼层对应表";
            // 
            // pnl_ReadCloudFloorTable
            // 
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_GetData);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_Cancel);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_OK);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.cBbE_Elevator);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_Elevator);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_ProNo);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.TextEdit_ProNo);
            this.pnl_ReadCloudFloorTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ReadCloudFloorTable.Location = new System.Drawing.Point(0, 27);
            this.pnl_ReadCloudFloorTable.Name = "pnl_ReadCloudFloorTable";
            this.pnl_ReadCloudFloorTable.Size = new System.Drawing.Size(350, 213);
            this.pnl_ReadCloudFloorTable.TabIndex = 1;
            // 
            // TextEdit_ProNo
            // 
            this.TextEdit_ProNo.Location = new System.Drawing.Point(143, 32);
            this.TextEdit_ProNo.Name = "TextEdit_ProNo";
            this.TextEdit_ProNo.Size = new System.Drawing.Size(100, 26);
            this.TextEdit_ProNo.TabIndex = 0;
            // 
            // Lbc_ProNo
            // 
            this.Lbc_ProNo.Location = new System.Drawing.Point(30, 35);
            this.Lbc_ProNo.Name = "Lbc_ProNo";
            this.Lbc_ProNo.Size = new System.Drawing.Size(59, 20);
            this.Lbc_ProNo.TabIndex = 1;
            this.Lbc_ProNo.Text = "项目编号:";
            // 
            // Lbc_Elevator
            // 
            this.Lbc_Elevator.Location = new System.Drawing.Point(30, 89);
            this.Lbc_Elevator.Name = "Lbc_Elevator";
            this.Lbc_Elevator.Size = new System.Drawing.Size(31, 20);
            this.Lbc_Elevator.TabIndex = 2;
            this.Lbc_Elevator.Text = "电梯:";
            // 
            // cBbE_Elevator
            // 
            this.cBbE_Elevator.Location = new System.Drawing.Point(143, 89);
            this.cBbE_Elevator.Name = "cBbE_Elevator";
            this.cBbE_Elevator.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cBbE_Elevator.Size = new System.Drawing.Size(100, 26);
            this.cBbE_Elevator.TabIndex = 3;
            // 
            // Btn_ReadcloudFloorTable_OK
            // 
            this.Btn_ReadcloudFloorTable_OK.Location = new System.Drawing.Point(39, 154);
            this.Btn_ReadcloudFloorTable_OK.Name = "Btn_ReadcloudFloorTable_OK";
            this.Btn_ReadcloudFloorTable_OK.Size = new System.Drawing.Size(75, 30);
            this.Btn_ReadcloudFloorTable_OK.TabIndex = 4;
            this.Btn_ReadcloudFloorTable_OK.Text = "确定";
            // 
            // Btn_ReadcloudFloorTable_Cancel
            // 
            this.Btn_ReadcloudFloorTable_Cancel.Location = new System.Drawing.Point(191, 154);
            this.Btn_ReadcloudFloorTable_Cancel.Name = "Btn_ReadcloudFloorTable_Cancel";
            this.Btn_ReadcloudFloorTable_Cancel.Size = new System.Drawing.Size(75, 30);
            this.Btn_ReadcloudFloorTable_Cancel.TabIndex = 5;
            this.Btn_ReadcloudFloorTable_Cancel.Text = "取消";
            // 
            // Btn_ReadcloudFloorTable_GetData
            // 
            this.Btn_ReadcloudFloorTable_GetData.Location = new System.Drawing.Point(258, 33);
            this.Btn_ReadcloudFloorTable_GetData.Name = "Btn_ReadcloudFloorTable_GetData";
            this.Btn_ReadcloudFloorTable_GetData.Size = new System.Drawing.Size(45, 25);
            this.Btn_ReadcloudFloorTable_GetData.TabIndex = 6;
            this.Btn_ReadcloudFloorTable_GetData.Text = "获取";
            // 
            // ReadCloudFloorTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_ReadCloudFloorTable);
            this.Controls.Add(this.grc_ReadCloudFloorTableTitle);
            this.Name = "ReadCloudFloorTableForm";
            this.Size = new System.Drawing.Size(350, 240);
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).EndInit();
            this.pnl_ReadCloudFloorTable.ResumeLayout(false);
            this.pnl_ReadCloudFloorTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_ProNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBbE_Elevator.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grc_ReadCloudFloorTableTitle;
        private DevExpress.XtraEditors.PanelControl pnl_ReadCloudFloorTable;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_GetData;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_Cancel;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_OK;
        private DevExpress.XtraEditors.ComboBoxEdit cBbE_Elevator;
        private DevExpress.XtraEditors.LabelControl Lbc_Elevator;
        private DevExpress.XtraEditors.LabelControl Lbc_ProNo;
        private DevExpress.XtraEditors.TextEdit TextEdit_ProNo;
    }
}
