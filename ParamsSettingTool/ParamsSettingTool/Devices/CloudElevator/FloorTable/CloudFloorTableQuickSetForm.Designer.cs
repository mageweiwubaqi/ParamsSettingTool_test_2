namespace ITL.ParamsSettingTool
{
    partial class CloudFloorTableQuickSetForm
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
            this.rdogrpSetType = new DevExpress.XtraEditors.RadioGroup();
            this.cbbE_Start = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Lbc_Choice = new DevExpress.XtraEditors.LabelControl();
            this.lbc_StartNameNo = new DevExpress.XtraEditors.LabelControl();
            this.cbbE_StartAuthFlag = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Btn_QsetcloudFloorTable_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_QSetcloudFloorTable_OK = new DevExpress.XtraEditors.SimpleButton();
            this.cBbE_EndAuthFlag = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Lbc_EndAuthFlag = new DevExpress.XtraEditors.LabelControl();
            this.Lbc_StartAuthFlag = new DevExpress.XtraEditors.LabelControl();
            this.grc_ReadCloudFloorTableTitle = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).BeginInit();
            this.pnl_ReadCloudFloorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdogrpSetType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbE_Start.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbE_StartAuthFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBbE_EndAuthFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_ReadCloudFloorTable
            // 
            this.pnl_ReadCloudFloorTable.Controls.Add(this.rdogrpSetType);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.cbbE_Start);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_Choice);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.lbc_StartNameNo);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.cbbE_StartAuthFlag);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_QsetcloudFloorTable_Cancel);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Btn_QSetcloudFloorTable_OK);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.cBbE_EndAuthFlag);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_EndAuthFlag);
            this.pnl_ReadCloudFloorTable.Controls.Add(this.Lbc_StartAuthFlag);
            this.pnl_ReadCloudFloorTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ReadCloudFloorTable.Location = new System.Drawing.Point(0, 27);
            this.pnl_ReadCloudFloorTable.Name = "pnl_ReadCloudFloorTable";
            this.pnl_ReadCloudFloorTable.Size = new System.Drawing.Size(506, 307);
            this.pnl_ReadCloudFloorTable.TabIndex = 5;
            // 
            // rdogrpSetType
            // 
            this.rdogrpSetType.Location = new System.Drawing.Point(32, 38);
            this.rdogrpSetType.Name = "rdogrpSetType";
            this.rdogrpSetType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("FloorName", "按键名称", true, ((short)(1))),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("CheckFloor", "检测楼层", true, ((short)(2))),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("FloorTerminalNo", "端子号", true, ((short)(3))),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("TerminalNumSlave1", "第一副操纵盘", true, ((short)(4))),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("TerminalNumSlave2", "第二副操纵盘", true, ((short)(5)))});
            this.rdogrpSetType.Size = new System.Drawing.Size(127, 187);
            this.rdogrpSetType.TabIndex = 11;
            this.rdogrpSetType.SelectedIndexChanged += new System.EventHandler(this.rdogrpSetType_SelectedIndexChanged);
            // 
            // cbbE_Start
            // 
            this.cbbE_Start.Location = new System.Drawing.Point(371, 186);
            this.cbbE_Start.Name = "cbbE_Start";
            this.cbbE_Start.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbE_Start.Size = new System.Drawing.Size(119, 26);
            this.cbbE_Start.TabIndex = 8;
            // 
            // Lbc_Choice
            // 
            this.Lbc_Choice.Location = new System.Drawing.Point(34, 15);
            this.Lbc_Choice.Name = "Lbc_Choice";
            this.Lbc_Choice.Size = new System.Drawing.Size(42, 20);
            this.Lbc_Choice.TabIndex = 10;
            this.Lbc_Choice.Text = "类别：";
            // 
            // lbc_StartNameNo
            // 
            this.lbc_StartNameNo.Location = new System.Drawing.Point(188, 189);
            this.lbc_StartNameNo.Name = "lbc_StartNameNo";
            this.lbc_StartNameNo.Size = new System.Drawing.Size(73, 20);
            this.lbc_StartNameNo.TabIndex = 7;
            this.lbc_StartNameNo.Text = "请选择类别:";
            // 
            // cbbE_StartAuthFlag
            // 
            this.cbbE_StartAuthFlag.Location = new System.Drawing.Point(371, 48);
            this.cbbE_StartAuthFlag.Name = "cbbE_StartAuthFlag";
            this.cbbE_StartAuthFlag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbE_StartAuthFlag.Size = new System.Drawing.Size(119, 26);
            this.cbbE_StartAuthFlag.TabIndex = 6;
            // 
            // Btn_QsetcloudFloorTable_Cancel
            // 
            this.Btn_QsetcloudFloorTable_Cancel.Location = new System.Drawing.Point(314, 249);
            this.Btn_QsetcloudFloorTable_Cancel.Name = "Btn_QsetcloudFloorTable_Cancel";
            this.Btn_QsetcloudFloorTable_Cancel.Size = new System.Drawing.Size(75, 30);
            this.Btn_QsetcloudFloorTable_Cancel.TabIndex = 5;
            this.Btn_QsetcloudFloorTable_Cancel.Text = "取消";
            this.Btn_QsetcloudFloorTable_Cancel.Click += new System.EventHandler(this.Btn_QsetcloudFloorTable_Cancel_Click);
            // 
            // Btn_QSetcloudFloorTable_OK
            // 
            this.Btn_QSetcloudFloorTable_OK.Location = new System.Drawing.Point(133, 249);
            this.Btn_QSetcloudFloorTable_OK.Name = "Btn_QSetcloudFloorTable_OK";
            this.Btn_QSetcloudFloorTable_OK.Size = new System.Drawing.Size(75, 30);
            this.Btn_QSetcloudFloorTable_OK.TabIndex = 4;
            this.Btn_QSetcloudFloorTable_OK.Text = "确定";
            this.Btn_QSetcloudFloorTable_OK.Click += new System.EventHandler(this.Btn_QSetcloudFloorTable_OK_Click);
            // 
            // cBbE_EndAuthFlag
            // 
            this.cBbE_EndAuthFlag.Location = new System.Drawing.Point(371, 122);
            this.cBbE_EndAuthFlag.Name = "cBbE_EndAuthFlag";
            this.cBbE_EndAuthFlag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cBbE_EndAuthFlag.Size = new System.Drawing.Size(119, 26);
            this.cBbE_EndAuthFlag.TabIndex = 3;
            // 
            // Lbc_EndAuthFlag
            // 
            this.Lbc_EndAuthFlag.Location = new System.Drawing.Point(188, 125);
            this.Lbc_EndAuthFlag.Name = "Lbc_EndAuthFlag";
            this.Lbc_EndAuthFlag.Size = new System.Drawing.Size(112, 20);
            this.Lbc_EndAuthFlag.TabIndex = 2;
            this.Lbc_EndAuthFlag.Text = "结束权限标识号：";
            // 
            // Lbc_StartAuthFlag
            // 
            this.Lbc_StartAuthFlag.Location = new System.Drawing.Point(188, 51);
            this.Lbc_StartAuthFlag.Name = "Lbc_StartAuthFlag";
            this.Lbc_StartAuthFlag.Size = new System.Drawing.Size(112, 20);
            this.Lbc_StartAuthFlag.TabIndex = 1;
            this.Lbc_StartAuthFlag.Text = "开始权限标识号：";
            // 
            // grc_ReadCloudFloorTableTitle
            // 
            this.grc_ReadCloudFloorTableTitle.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grc_ReadCloudFloorTableTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.grc_ReadCloudFloorTableTitle.Location = new System.Drawing.Point(0, 0);
            this.grc_ReadCloudFloorTableTitle.Name = "grc_ReadCloudFloorTableTitle";
            this.grc_ReadCloudFloorTableTitle.Size = new System.Drawing.Size(506, 27);
            this.grc_ReadCloudFloorTableTitle.TabIndex = 4;
            this.grc_ReadCloudFloorTableTitle.Text = "快速设置";
            // 
            // CloudFloorTableQuickSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 334);
            this.Controls.Add(this.pnl_ReadCloudFloorTable);
            this.Controls.Add(this.grc_ReadCloudFloorTableTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CloudFloorTableQuickSetForm";
            this.Text = "快速设置";
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ReadCloudFloorTable)).EndInit();
            this.pnl_ReadCloudFloorTable.ResumeLayout(false);
            this.pnl_ReadCloudFloorTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdogrpSetType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbE_Start.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbE_StartAuthFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBbE_EndAuthFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grc_ReadCloudFloorTableTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnl_ReadCloudFloorTable;
        private DevExpress.XtraEditors.ComboBoxEdit cbbE_StartAuthFlag;
        private DevExpress.XtraEditors.SimpleButton Btn_QsetcloudFloorTable_Cancel;
        private DevExpress.XtraEditors.SimpleButton Btn_QSetcloudFloorTable_OK;
        private DevExpress.XtraEditors.ComboBoxEdit cBbE_EndAuthFlag;
        private DevExpress.XtraEditors.LabelControl Lbc_EndAuthFlag;
        private DevExpress.XtraEditors.LabelControl Lbc_StartAuthFlag;
        private DevExpress.XtraEditors.GroupControl grc_ReadCloudFloorTableTitle;
        private DevExpress.XtraEditors.ComboBoxEdit cbbE_Start;
        private DevExpress.XtraEditors.LabelControl lbc_StartNameNo;
        private DevExpress.XtraEditors.LabelControl Lbc_Choice;
        private DevExpress.XtraEditors.RadioGroup rdogrpSetType;
    }
}