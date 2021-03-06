﻿namespace ITL.ParamsSettingTool
{
    partial class CloudElevatorItemUserControl
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
            this.lcItem = new DevExpress.XtraLayout.LayoutControl();
            this.lblConStatues = new DevExpress.XtraEditors.LabelControl();
            this.cmbCtrlProperties = new DevExpress.XtraEditors.ComboBoxEdit();
            this.edtDevIp = new DevExpress.XtraEditors.TextEdit();
            this.lgItem = new DevExpress.XtraLayout.LayoutControlGroup();
            this.liDevIp = new DevExpress.XtraLayout.LayoutControlItem();
            this.liCtrlProperties = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.pnlContainer = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lcItem)).BeginInit();
            this.lcItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCtrlProperties.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDevIp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.liDevIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.liCtrlProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContainer)).BeginInit();
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lcItem
            // 
            this.lcItem.Controls.Add(this.lblConStatues);
            this.lcItem.Controls.Add(this.cmbCtrlProperties);
            this.lcItem.Controls.Add(this.edtDevIp);
            this.lcItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcItem.Location = new System.Drawing.Point(1, 1);
            this.lcItem.Name = "lcItem";
            this.lcItem.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(849, 134, 250, 350);
            this.lcItem.Root = this.lgItem;
            this.lcItem.Size = new System.Drawing.Size(530, 39);
            this.lcItem.TabIndex = 0;
            this.lcItem.Text = "layoutControl1";
            // 
            // lblConStatues
            // 
            this.lblConStatues.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblConStatues.Location = new System.Drawing.Point(476, 5);
            this.lblConStatues.Name = "lblConStatues";
            this.lblConStatues.Size = new System.Drawing.Size(49, 29);
            this.lblConStatues.StyleController = this.lcItem;
            this.lblConStatues.TabIndex = 19;
            this.lblConStatues.Text = "未连接";
            // 
            // cmbCtrlProperties
            // 
            this.cmbCtrlProperties.Location = new System.Drawing.Point(332, 5);
            this.cmbCtrlProperties.MaximumSize = new System.Drawing.Size(140, 0);
            this.cmbCtrlProperties.MinimumSize = new System.Drawing.Size(140, 0);
            this.cmbCtrlProperties.Name = "cmbCtrlProperties";
            this.cmbCtrlProperties.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCtrlProperties.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbCtrlProperties.Size = new System.Drawing.Size(140, 26);
            this.cmbCtrlProperties.StyleController = this.lcItem;
            this.cmbCtrlProperties.TabIndex = 5;
            // 
            // edtDevIp
            // 
            this.edtDevIp.EditValue = "";
            this.edtDevIp.Location = new System.Drawing.Point(113, 5);
            this.edtDevIp.MaximumSize = new System.Drawing.Size(140, 0);
            this.edtDevIp.MinimumSize = new System.Drawing.Size(140, 0);
            this.edtDevIp.Name = "edtDevIp";
            this.edtDevIp.Size = new System.Drawing.Size(140, 26);
            this.edtDevIp.StyleController = this.lcItem;
            this.edtDevIp.TabIndex = 4;
            // 
            // lgItem
            // 
            this.lgItem.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lgItem.GroupBordersVisible = false;
            this.lgItem.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.liDevIp,
            this.liCtrlProperties,
            this.layoutControlItem1});
            this.lgItem.Location = new System.Drawing.Point(0, 0);
            this.lgItem.Name = "Root";
            this.lgItem.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.lgItem.Size = new System.Drawing.Size(530, 39);
            this.lgItem.TextVisible = false;
            // 
            // liDevIp
            // 
            this.liDevIp.Control = this.edtDevIp;
            this.liDevIp.Location = new System.Drawing.Point(0, 0);
            this.liDevIp.Name = "liDevIp";
            this.liDevIp.Size = new System.Drawing.Size(252, 33);
            this.liDevIp.Text = "1号云电梯IP地址";
            this.liDevIp.TextSize = new System.Drawing.Size(105, 20);
            // 
            // liCtrlProperties
            // 
            this.liCtrlProperties.Control = this.cmbCtrlProperties;
            this.liCtrlProperties.Location = new System.Drawing.Point(252, 0);
            this.liCtrlProperties.MaxSize = new System.Drawing.Size(219, 33);
            this.liCtrlProperties.MinSize = new System.Drawing.Size(219, 33);
            this.liCtrlProperties.Name = "liCtrlProperties";
            this.liCtrlProperties.Size = new System.Drawing.Size(219, 33);
            this.liCtrlProperties.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.liCtrlProperties.Text = "控制器属性";
            this.liCtrlProperties.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.liCtrlProperties.TextSize = new System.Drawing.Size(70, 20);
            this.liCtrlProperties.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lblConStatues;
            this.layoutControlItem1.Location = new System.Drawing.Point(471, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(46, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(53, 33);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // pnlContainer
            // 
            this.pnlContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlContainer.Controls.Add(this.lcItem);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(1);
            this.pnlContainer.Size = new System.Drawing.Size(532, 41);
            this.pnlContainer.TabIndex = 6;
            // 
            // CloudElevatorItemUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CloudElevatorItemUserControl";
            this.Size = new System.Drawing.Size(532, 41);
            ((System.ComponentModel.ISupportInitialize)(this.lcItem)).EndInit();
            this.lcItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCtrlProperties.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDevIp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lgItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.liDevIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.liCtrlProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContainer)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl lcItem;
        private DevExpress.XtraLayout.LayoutControlGroup lgItem;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCtrlProperties;
        private DevExpress.XtraEditors.TextEdit edtDevIp;
        private DevExpress.XtraLayout.LayoutControlItem liDevIp;
        private DevExpress.XtraLayout.LayoutControlItem liCtrlProperties;
        private DevExpress.XtraEditors.PanelControl pnlContainer;
        private DevExpress.XtraEditors.LabelControl lblConStatues;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
