namespace ITL.ParamsSettingTool
{
    partial class ReadCloudFloorTableForm
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
            this.pnl_ReadCloudFloorTable = new DevExpress.XtraEditors.PanelControl();
            this.comboBox_FloorName = new System.Windows.Forms.ComboBox();
            this.Btn_ReadcloudFloorTable_GetData = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadcloudFloorTable_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_ReadcloudFloorTable_OK = new DevExpress.XtraEditors.SimpleButton();
            this.Lbc_Elevator = new DevExpress.XtraEditors.LabelControl();
            this.Lbc_ProNo = new DevExpress.XtraEditors.LabelControl();
            this.TextEdit_ProNo = new DevExpress.XtraEditors.TextEdit();
            this.grc_ReadCloudFloorTableTitle = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).BeginInit();
            this.pnl_ReadCloudFloorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_ProNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_ReadCloudFloorTable
            // 
            this.pnl_ReadCloudFloorTable.Controls.Add(this.comboBox_FloorName);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_GetData);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_Cancel);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_ReadcloudFloorTable_OK);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_Elevator);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_ProNo);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.TextEdit_ProNo);
            this.pnl_ReadCloudFloorTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ReadCloudFloorTable.Location = new System.Drawing.Point(0, 27);
            this.pnl_ReadCloudFloorTable.Name = "pnl_ReadCloudFloorTable";
            this.pnl_ReadCloudFloorTable.Size = new System.Drawing.Size(314, 195);
            this.pnl_ReadCloudFloorTable.TabIndex = 3;
            // 
            // comboBox_FloorName
            // 
            this.comboBox_FloorName.FormattingEnabled = true;
            this.comboBox_FloorName.Location = new System.Drawing.Point(110, 89);
            this.comboBox_FloorName.Name = "comboBox_FloorName";
            this.comboBox_FloorName.Size = new System.Drawing.Size(133, 28);
            this.comboBox_FloorName.TabIndex = 7;
            // 
            // Btn_ReadcloudFloorTable_GetData
            // 
            this.Btn_ReadcloudFloorTable_GetData.Location = new System.Drawing.Point(258, 33);
            this.Btn_ReadcloudFloorTable_GetData.Name = "Btn_ReadcloudFloorTable_GetData";
            this.Btn_ReadcloudFloorTable_GetData.Size = new System.Drawing.Size(45, 25);
            this.Btn_ReadcloudFloorTable_GetData.TabIndex = 6;
            this.Btn_ReadcloudFloorTable_GetData.Text = "获取";
            this.Btn_ReadcloudFloorTable_GetData.Click += new System.EventHandler(this.Btn_ReadcloudFloorTable_GetData_Click);
            // 
            // Btn_ReadcloudFloorTable_Cancel
            // 
            this.Btn_ReadcloudFloorTable_Cancel.Location = new System.Drawing.Point(191, 154);
            this.Btn_ReadcloudFloorTable_Cancel.Name = "Btn_ReadcloudFloorTable_Cancel";
            this.Btn_ReadcloudFloorTable_Cancel.Size = new System.Drawing.Size(75, 30);
            this.Btn_ReadcloudFloorTable_Cancel.TabIndex = 5;
            this.Btn_ReadcloudFloorTable_Cancel.Text = "取消";
            this.Btn_ReadcloudFloorTable_Cancel.Click += new System.EventHandler(this.Btn_ReadcloudFloorTable_Cancel_Click);
            // 
            // Btn_ReadcloudFloorTable_OK
            // 
            this.Btn_ReadcloudFloorTable_OK.Location = new System.Drawing.Point(39, 154);
            this.Btn_ReadcloudFloorTable_OK.Name = "Btn_ReadcloudFloorTable_OK";
            this.Btn_ReadcloudFloorTable_OK.Size = new System.Drawing.Size(75, 30);
            this.Btn_ReadcloudFloorTable_OK.TabIndex = 4;
            this.Btn_ReadcloudFloorTable_OK.Text = "确定";
            this.Btn_ReadcloudFloorTable_OK.Click += new System.EventHandler(this.Btn_ReadcloudFloorTable_OK_Click);
            // 
            // Lbc_Elevator
            // 
            this.Lbc_Elevator.Location = new System.Drawing.Point(30, 89);
            this.Lbc_Elevator.Name = "Lbc_Elevator";
            this.Lbc_Elevator.Size = new System.Drawing.Size(31, 20);
            this.Lbc_Elevator.TabIndex = 2;
            this.Lbc_Elevator.Text = "电梯:";
            // 
            // Lbc_ProNo
            // 
            this.Lbc_ProNo.Location = new System.Drawing.Point(30, 35);
            this.Lbc_ProNo.Name = "Lbc_ProNo";
            this.Lbc_ProNo.Size = new System.Drawing.Size(59, 20);
            this.Lbc_ProNo.TabIndex = 1;
            this.Lbc_ProNo.Text = "项目编号:";
            // 
            // TextEdit_ProNo
            // 
            this.TextEdit_ProNo.Location = new System.Drawing.Point(110, 32);
            this.TextEdit_ProNo.Name = "TextEdit_ProNo";
            this.TextEdit_ProNo.Size = new System.Drawing.Size(133, 26);
            this.TextEdit_ProNo.TabIndex = 0;
            // 
            // grc_ReadCloudFloorTableTitle
            // 
            this.grc_ReadCloudFloorTableTitle.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grc_ReadCloudFloorTableTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.grc_ReadCloudFloorTableTitle.Location = new System.Drawing.Point(0, 0);
            this.grc_ReadCloudFloorTableTitle.Name = "grc_ReadCloudFloorTableTitle";
            this.grc_ReadCloudFloorTableTitle.Size = new System.Drawing.Size(314, 27);
            this.grc_ReadCloudFloorTableTitle.TabIndex = 2;
            this.grc_ReadCloudFloorTableTitle.Text = "读取楼层对应表";
            // 
            // ReadCloudFloorTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 222);
            this.Controls.Add(this.pnl_ReadCloudFloorTable);
            this.Controls.Add(this.grc_ReadCloudFloorTableTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReadCloudFloorTableForm";
            this.Text = "ReadCloudFloorTableForm";
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).EndInit();
            this.pnl_ReadCloudFloorTable.ResumeLayout(false);
            this.pnl_ReadCloudFloorTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_ProNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnl_ReadCloudFloorTable;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_GetData;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_Cancel;
        private DevExpress.XtraEditors.SimpleButton Btn_ReadcloudFloorTable_OK;
        private DevExpress.XtraEditors.LabelControl Lbc_Elevator;
        private DevExpress.XtraEditors.LabelControl Lbc_ProNo;
        private DevExpress.XtraEditors.TextEdit TextEdit_ProNo;
        private DevExpress.XtraEditors.GroupControl grc_ReadCloudFloorTableTitle;
        private System.Windows.Forms.ComboBox comboBox_FloorName;
    }
}