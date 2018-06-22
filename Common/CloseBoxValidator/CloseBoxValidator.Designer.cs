namespace Common
{
    partial class CloseBoxValidator
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
            this.tbFileNumCode = new System.Windows.Forms.TextBox();
            this.lTextFileNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbFileNumCode
            // 
            this.tbFileNumCode.Location = new System.Drawing.Point(183, 9);
            this.tbFileNumCode.Name = "tbFileNumCode";
            this.tbFileNumCode.Size = new System.Drawing.Size(132, 20);
            this.tbFileNumCode.TabIndex = 6;
            this.tbFileNumCode.TextChanged += new System.EventHandler(this.TbFileNumCodeTextChanged);
            // 
            // lTextFileNumber
            // 
            this.lTextFileNumber.AutoSize = true;
            this.lTextFileNumber.Location = new System.Drawing.Point(12, 16);
            this.lTextFileNumber.Name = "lTextFileNumber";
            this.lTextFileNumber.Size = new System.Drawing.Size(118, 13);
            this.lTextFileNumber.TabIndex = 7;
            this.lTextFileNumber.Text = "Broj preostalih kodova: ";
            // 
            // CloseBoxValidator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 38);
            this.Controls.Add(this.tbFileNumCode);
            this.Controls.Add(this.lTextFileNumber);
            this.MaximumSize = new System.Drawing.Size(343, 77);
            this.MinimumSize = new System.Drawing.Size(343, 77);
            this.Name = "CloseBoxValidator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kontrola zatvaranja kutije";
            this.Shown += new System.EventHandler(this.CloseBoxValidatorShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFileNumCode;
        private System.Windows.Forms.Label lTextFileNumber;
    }
}