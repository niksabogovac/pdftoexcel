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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.lFileNum = new System.Windows.Forms.Label();
            this.tbFileNumCode = new System.Windows.Forms.TextBox();
            this.tbOrgUnit = new System.Windows.Forms.TextBox();
            this.lOrgUnit = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lFileNum
            // 
            this.lFileNum.AutoSize = true;
            this.lFileNum.Location = new System.Drawing.Point(13, 46);
            this.lFileNum.Name = "lFileNum";
            this.lFileNum.Size = new System.Drawing.Size(46, 13);
            this.lFileNum.TabIndex = 0;
            this.lFileNum.Text = "Unesite ";
            // 
            // tbFileNumCode
            // 
            this.tbFileNumCode.Location = new System.Drawing.Point(189, 43);
            this.tbFileNumCode.Name = "tbFileNumCode";
            this.tbFileNumCode.Size = new System.Drawing.Size(132, 20);
            this.tbFileNumCode.TabIndex = 1;
            this.tbFileNumCode.TextChanged += new System.EventHandler(this.tbFileNumCodeTextChanged);
            this.tbFileNumCode.Enter += new System.EventHandler(this.tbFileNumCodeEnter);
            // 
            // tbOrgUnit
            // 
            this.tbOrgUnit.Location = new System.Drawing.Point(189, 16);
            this.tbOrgUnit.Name = "tbOrgUnit";
            this.tbOrgUnit.Size = new System.Drawing.Size(132, 20);
            this.tbOrgUnit.TabIndex = 5;
            this.tbOrgUnit.TextChanged += new System.EventHandler(this.tbOrgUnitTextChanged);
            // 
            // lOrgUnit
            // 
            this.lOrgUnit.AutoSize = true;
            this.lOrgUnit.Location = new System.Drawing.Point(13, 19);
            this.lOrgUnit.Name = "lOrgUnit";
            this.lOrgUnit.Size = new System.Drawing.Size(108, 13);
            this.lOrgUnit.TabIndex = 6;
            this.lOrgUnit.Text = "Unesite Org. Jedinicu";
            // 
            // CloseBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 76);
            this.Controls.Add(this.tbOrgUnit);
            this.Controls.Add(this.lOrgUnit);
            this.Controls.Add(this.tbFileNumCode);
            this.Controls.Add(this.lFileNum);
            this.Name = "CloseBoxDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zatvaranje kutije";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lFileNum;
        private System.Windows.Forms.TextBox tbFileNumCode;
        private System.Windows.Forms.TextBox tbOrgUnit;
        private System.Windows.Forms.Label lOrgUnit;
    }
}