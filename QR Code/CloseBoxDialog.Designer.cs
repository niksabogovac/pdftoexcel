namespace QR_Code
{
    partial class CloseBoxDialog
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
            this.lText = new System.Windows.Forms.Label();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.bConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lText
            // 
            this.lText.AutoSize = true;
            this.lText.Location = new System.Drawing.Point(12, 16);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(46, 13);
            this.lText.TabIndex = 0;
            this.lText.Text = "Unesite ";
            // 
            // tbCode
            // 
            this.tbCode.Location = new System.Drawing.Point(172, 16);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(100, 20);
            this.tbCode.TabIndex = 1;
            // 
            // bConfirm
            // 
            this.bConfirm.Location = new System.Drawing.Point(93, 42);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(100, 23);
            this.bConfirm.TabIndex = 2;
            this.bConfirm.Text = "Potvrdi";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Click += new System.EventHandler(this.BConfirmClick);
            // 
            // CloseBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 76);
            this.Controls.Add(this.bConfirm);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.lText);
            this.Name = "CloseBoxDialog";
            this.Text = "Zatvaranje kutije";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Button bConfirm;
    }
}