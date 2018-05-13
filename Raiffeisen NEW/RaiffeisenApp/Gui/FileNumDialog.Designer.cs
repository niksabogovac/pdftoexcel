namespace Gui
{
    partial class FileNumDialog
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
            this.lTextFileNumber = new System.Windows.Forms.Label();
            this.tbFileNumCode = new System.Windows.Forms.TextBox();
            this.lOrgUnit = new System.Windows.Forms.Label();
            this.tbOrgUnit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lTextFileNumber
            // 
            this.lTextFileNumber.AutoSize = true;
            this.lTextFileNumber.Location = new System.Drawing.Point(11, 51);
            this.lTextFileNumber.Name = "lTextFileNumber";
            this.lTextFileNumber.Size = new System.Drawing.Size(46, 13);
            this.lTextFileNumber.TabIndex = 5;
            this.lTextFileNumber.Text = "Unesite ";
            // 
            // tbFileNumCode
            // 
            this.tbFileNumCode.Location = new System.Drawing.Point(190, 48);
            this.tbFileNumCode.Name = "tbFileNumCode";
            this.tbFileNumCode.Size = new System.Drawing.Size(132, 20);
            this.tbFileNumCode.TabIndex = 2;
            this.tbFileNumCode.TextChanged += new System.EventHandler(this.tbFileNumCodeTextChanged);
            this.tbFileNumCode.Enter += new System.EventHandler(this.tbFileNumCodeEnter);
            // 
            // lOrgUnit
            // 
            this.lOrgUnit.AutoSize = true;
            this.lOrgUnit.Location = new System.Drawing.Point(11, 26);
            this.lOrgUnit.Name = "lOrgUnit";
            this.lOrgUnit.Size = new System.Drawing.Size(108, 13);
            this.lOrgUnit.TabIndex = 4;
            this.lOrgUnit.Text = "Unesite Org. Jedinicu";
            // 
            // tbOrgUnit
            // 
            this.tbOrgUnit.Location = new System.Drawing.Point(190, 19);
            this.tbOrgUnit.Name = "tbOrgUnit";
            this.tbOrgUnit.Size = new System.Drawing.Size(132, 20);
            this.tbOrgUnit.TabIndex = 1;
            this.tbOrgUnit.TextChanged += new System.EventHandler(this.tbOrgUnitTextChanged);
            // 
            // FileNumDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 79);
            this.Controls.Add(this.tbOrgUnit);
            this.Controls.Add(this.lOrgUnit);
            this.Controls.Add(this.tbFileNumCode);
            this.Controls.Add(this.lTextFileNumber);
            this.Name = "FileNumDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FileNumDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lTextFileNumber;
        private System.Windows.Forms.TextBox tbFileNumCode;
        private System.Windows.Forms.Label lOrgUnit;
        private System.Windows.Forms.TextBox tbOrgUnit;
    }
}