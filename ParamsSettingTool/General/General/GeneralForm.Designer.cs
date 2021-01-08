namespace ITL.General
{
    partial class GeneralForm
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
            this.components = new System.ComponentModel.Container();
            this.pnlTitle = new ITL.General.DragPanelControl(this.components);
            this.lblCaption = new ITL.General.DragLabelControl(this.components);
            this.pnlButtonContainer = new ITL.General.DragPanelControl(this.components);
            this.btnMin = new ITL.General.FlatSimpleButton(this.components);
            this.btnMax = new ITL.General.FlatSimpleButton(this.components);
            this.btnClose = new ITL.General.FlatSimpleButton(this.components);
            this.pnlButtom = new DevExpress.XtraEditors.PanelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).BeginInit();
            this.pnlButtonContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).BeginInit();
            this.pnlButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
            this.pnlTitle.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Appearance.Options.UseFont = true;
            this.pnlTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlTitle.Controls.Add(this.lblCaption);
            this.pnlTitle.Controls.Add(this.pnlButtonContainer);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.DragControl = this;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(451, 40);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblCaption
            // 
            this.lblCaption.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCaption.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblCaption.DragControl = this;
            this.lblCaption.Location = new System.Drawing.Point(48, 17);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(32, 22);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "标题";
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
            this.pnlButtonContainer.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlButtonContainer.Appearance.Options.UseBackColor = true;
            this.pnlButtonContainer.Appearance.Options.UseFont = true;
            this.pnlButtonContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlButtonContainer.Controls.Add(this.btnMin);
            this.pnlButtonContainer.Controls.Add(this.btnMax);
            this.pnlButtonContainer.Controls.Add(this.btnClose);
            this.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtonContainer.DragControl = this;
            this.pnlButtonContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlButtonContainer.Name = "pnlButtonContainer";
            this.pnlButtonContainer.Size = new System.Drawing.Size(451, 24);
            this.pnlButtonContainer.TabIndex = 1;
            // 
            // btnMin
            // 
            this.btnMin.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMin.Appearance.Options.UseBackColor = true;
            this.btnMin.Appearance.Options.UseFont = true;
            this.btnMin.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnMin.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMin.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            this.btnMin.Image = global::ITL.ParamsSettingTool.Properties.Resources.Min_16;
            this.btnMin.Location = new System.Drawing.Point(379, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(24, 24);
            this.btnMin.TabIndex = 2;
            // 
            // btnMax
            // 
            this.btnMax.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMax.Appearance.Options.UseBackColor = true;
            this.btnMax.Appearance.Options.UseFont = true;
            this.btnMax.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnMax.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMax.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            this.btnMax.Image = global::ITL.ParamsSettingTool.Properties.Resources.Max_16;
            this.btnMax.Location = new System.Drawing.Point(403, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(24, 24);
            this.btnMax.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            this.btnClose.Image = global::ITL.ParamsSettingTool.Properties.Resources.Close_16;
            this.btnClose.Location = new System.Drawing.Point(427, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.TabIndex = 0;
            // 
            // pnlButtom
            // 
            this.pnlButtom.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlButtom.Appearance.Options.UseFont = true;
            this.pnlButtom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlButtom.Controls.Add(this.btnOK);
            this.pnlButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtom.Location = new System.Drawing.Point(0, 264);
            this.pnlButtom.Name = "pnlButtom";
            this.pnlButtom.Size = new System.Drawing.Size(451, 58);
            this.pnlButtom.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(343, 18);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确  定";
            // 
            // pnlMain
            // 
            this.pnlMain.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlMain.Appearance.Options.UseFont = true;
            this.pnlMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 40);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(1);
            this.pnlMain.Size = new System.Drawing.Size(451, 224);
            this.pnlMain.TabIndex = 3;
            // 
            // GeneralForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 322);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtom);
            this.Controls.Add(this.pnlTitle);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GeneralForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GeneralForm";
            this.Load += new System.EventHandler(this.GeneralForm_Load);
            this.Shown += new System.EventHandler(this.GeneralForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).EndInit();
            this.pnlButtonContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).EndInit();
            this.pnlButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DragPanelControl pnlTitle;
        protected DevExpress.XtraEditors.PanelControl pnlButtom;
        protected DevExpress.XtraEditors.SimpleButton btnOK;
        protected DevExpress.XtraEditors.PanelControl pnlMain;
        protected DragPanelControl pnlButtonContainer;
        protected FlatSimpleButton btnMin;
        protected FlatSimpleButton btnMax;
        protected DragLabelControl lblCaption;
        protected FlatSimpleButton btnClose;
    }
}