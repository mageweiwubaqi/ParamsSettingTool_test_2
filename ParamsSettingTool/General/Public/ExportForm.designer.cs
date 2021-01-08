namespace ITL.General
{
    partial class ExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.chkAutoOpen = new DevExpress.XtraEditors.CheckEdit();
            this.edtPath = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).BeginInit();
            this.pnlButtonContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).BeginInit();
            this.pnlButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOpen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMin
            // 
            this.btnMin.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMin.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMin.Appearance.Options.UseBackColor = true;
            this.btnMin.Appearance.Options.UseFont = true;
            this.btnMin.Location = new System.Drawing.Point(407, 0);
            // 
            // lblCaption
            // 
            this.lblCaption.Appearance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblCaption.Location = new System.Drawing.Point(139, 9);
            this.lblCaption.Size = new System.Drawing.Size(64, 22);
            this.lblCaption.Text = "导出资料";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Location = new System.Drawing.Point(455, 0);
            // 
            // btnMax
            // 
            this.btnMax.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btnMax.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMax.Appearance.Options.UseBackColor = true;
            this.btnMax.Appearance.Options.UseFont = true;
            this.btnMax.Image = ((System.Drawing.Image)(resources.GetObject("btnMax.Image")));
            this.btnMax.Location = new System.Drawing.Point(431, 0);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
            this.pnlTitle.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTitle.Appearance.Options.UseBackColor = true;
            this.pnlTitle.Appearance.Options.UseFont = true;
            this.pnlTitle.Size = new System.Drawing.Size(479, 40);
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pnlButtonContainer.Appearance.Options.UseFont = true;
            this.pnlButtonContainer.Size = new System.Drawing.Size(479, 24);
            // 
            // pnlButtom
            // 
            this.pnlButtom.Location = new System.Drawing.Point(0, 149);
            this.pnlButtom.Size = new System.Drawing.Size(479, 50);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(392, 11);
            this.btnOK.Text = "导  出";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.edtPath);
            this.pnlMain.Controls.Add(this.chkAutoOpen);
            this.pnlMain.Size = new System.Drawing.Size(479, 109);
            // 
            // chkAutoOpen
            // 
            this.chkAutoOpen.Location = new System.Drawing.Point(29, 63);
            this.chkAutoOpen.Name = "chkAutoOpen";
            this.chkAutoOpen.Properties.AllowFocused = false;
            this.chkAutoOpen.Properties.Caption = "导出成功后自动打开文件";
            this.chkAutoOpen.Size = new System.Drawing.Size(242, 24);
            this.chkAutoOpen.TabIndex = 52;
            // 
            // edtPath
            // 
            this.edtPath.Location = new System.Drawing.Point(29, 22);
            this.edtPath.Name = "edtPath";
            this.edtPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ParamsSettingTool.Properties.Resources.SelectPath_24, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.edtPath.Size = new System.Drawing.Size(420, 30);
            this.edtPath.TabIndex = 53;
            this.edtPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edtPath_ButtonClick);
            // 
            // ExportForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 199);
            this.Name = "ExportForm";
            this.Text = "ExportForm";
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).EndInit();
            this.pnlButtonContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtom)).EndInit();
            this.pnlButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOpen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPath.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit edtPath;
        private DevExpress.XtraEditors.CheckEdit chkAutoOpen;
    }
}