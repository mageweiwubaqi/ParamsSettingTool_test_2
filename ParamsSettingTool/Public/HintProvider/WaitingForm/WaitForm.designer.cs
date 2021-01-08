namespace ITL.Public
{
    partial class WaitForm
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
            this.pnlText = new System.Windows.Forms.Panel();
            this.lblDescription = new DevExpress.XtraEditors.LabelControl();
            this.lblCaption = new DevExpress.XtraEditors.LabelControl();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.picWaiting = new System.Windows.Forms.PictureBox();
            this.pnlText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlText
            // 
            this.pnlText.BackColor = System.Drawing.Color.White;
            this.pnlText.Controls.Add(this.lblDescription);
            this.pnlText.Controls.Add(this.lblCaption);
            this.pnlText.Controls.Add(this.picClose);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlText.Location = new System.Drawing.Point(58, 0);
            this.pnlText.Name = "pnlText";
            this.pnlText.Size = new System.Drawing.Size(266, 89);
            this.pnlText.TabIndex = 3;
            this.pnlText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseDown);
            this.pnlText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseMove);
            this.pnlText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseUp);
            // 
            // lblDescription
            // 
            this.lblDescription.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription.Location = new System.Drawing.Point(4, 50);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(238, 19);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "等待中。。。";
            this.lblDescription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseDown);
            this.lblDescription.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseMove);
            this.lblDescription.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseUp);
            // 
            // lblCaption
            // 
            this.lblCaption.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCaption.Location = new System.Drawing.Point(4, 20);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(238, 23);
            this.lblCaption.TabIndex = 5;
            this.lblCaption.Text = "请稍后";
            this.lblCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseDown);
            this.lblCaption.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseMove);
            this.lblCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseUp);
            // 
            // picClose
            // 
            this.picClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picClose.Image = ParamsSettingTool.Properties.Resources.Close_gray_16;
            this.picClose.Location = new System.Drawing.Point(247, 3);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(16, 16);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picClose.TabIndex = 4;
            this.picClose.TabStop = false;
            this.picClose.Visible = false;
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlContainer.Controls.Add(this.pnlText);
            this.pnlContainer.Controls.Add(this.picWaiting);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(3, 3);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(324, 89);
            this.pnlContainer.TabIndex = 5;
            // 
            // picWaiting
            // 
            this.picWaiting.BackColor = System.Drawing.Color.White;
            this.picWaiting.Dock = System.Windows.Forms.DockStyle.Left;
            this.picWaiting.Image = ParamsSettingTool.Properties.Resources.loading_32;
            this.picWaiting.Location = new System.Drawing.Point(0, 0);
            this.picWaiting.Name = "picWaiting";
            this.picWaiting.Size = new System.Drawing.Size(58, 89);
            this.picWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWaiting.TabIndex = 0;
            this.picWaiting.TabStop = false;
            this.picWaiting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseDown);
            this.picWaiting.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseMove);
            this.picWaiting.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaiting_MouseUp);
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(330, 95);
            this.Controls.Add(this.pnlContainer);
            this.Name = "WaitForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "WaitForm";
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picWaiting;
        private System.Windows.Forms.Panel pnlText;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.Panel pnlContainer;
        private DevExpress.XtraEditors.LabelControl lblCaption;
        private DevExpress.XtraEditors.LabelControl lblDescription;
    }
}