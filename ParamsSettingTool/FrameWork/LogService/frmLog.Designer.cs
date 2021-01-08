namespace ITL.Framework
{
    partial class frmLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLog));
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.btnStopScroll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbInfo
            // 
            this.tbInfo.BackColor = System.Drawing.Color.Black;
            this.tbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbInfo.ForeColor = System.Drawing.Color.Yellow;
            this.tbInfo.Location = new System.Drawing.Point(0, 0);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInfo.Size = new System.Drawing.Size(845, 403);
            this.tbInfo.TabIndex = 0;
            // 
            // btnStopScroll
            // 
            this.btnStopScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopScroll.Image = global::ITL.ParamsSettingTool.Properties.Resources.Close_16;
            this.btnStopScroll.Location = new System.Drawing.Point(769, 0);
            this.btnStopScroll.Name = "btnStopScroll";
            this.btnStopScroll.Size = new System.Drawing.Size(55, 30);
            this.btnStopScroll.TabIndex = 1;
            this.btnStopScroll.UseVisualStyleBackColor = true;
            this.btnStopScroll.Click += new System.EventHandler(this.btnStopScroll_Click);
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 403);
            this.Controls.Add(this.btnStopScroll);
            this.Controls.Add(this.tbInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Log Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLog_FormClosing);
            this.Shown += new System.EventHandler(this.frmLog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Button btnStopScroll;
    }
}