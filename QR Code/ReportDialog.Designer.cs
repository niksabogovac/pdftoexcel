namespace QR_Code
{
    partial class ReportDialog
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
            this.cbReport = new System.Windows.Forms.ComboBox();
            this.lReportType = new System.Windows.Forms.Label();
            this.bReport = new System.Windows.Forms.Button();
            this.tbOrderNumber = new System.Windows.Forms.TextBox();
            this.lOrderNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbReport
            // 
            this.cbReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReport.FormattingEnabled = true;
            this.cbReport.Items.AddRange(new object[] {
            "Opšti",
            "Detaljan",
            "Reisswolf"});
            this.cbReport.Location = new System.Drawing.Point(199, 71);
            this.cbReport.Name = "cbReport";
            this.cbReport.Size = new System.Drawing.Size(200, 21);
            this.cbReport.TabIndex = 4;
            // 
            // lReportType
            // 
            this.lReportType.AutoSize = true;
            this.lReportType.Location = new System.Drawing.Point(15, 78);
            this.lReportType.Name = "lReportType";
            this.lReportType.Size = new System.Drawing.Size(117, 13);
            this.lReportType.TabIndex = 5;
            this.lReportType.Text = "Izaberite vrstu izveštaja";
            // 
            // bReport
            // 
            this.bReport.Location = new System.Drawing.Point(167, 127);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(75, 23);
            this.bReport.TabIndex = 6;
            this.bReport.Text = "Kreiraj";
            this.bReport.UseVisualStyleBackColor = true;
            this.bReport.Click += new System.EventHandler(this.BReport_Click);
            // 
            // tbOrderNumber
            // 
            this.tbOrderNumber.Location = new System.Drawing.Point(199, 22);
            this.tbOrderNumber.Name = "tbOrderNumber";
            this.tbOrderNumber.Size = new System.Drawing.Size(200, 20);
            this.tbOrderNumber.TabIndex = 7;
            // 
            // lOrderNumber
            // 
            this.lOrderNumber.AutoSize = true;
            this.lOrderNumber.Location = new System.Drawing.Point(15, 25);
            this.lOrderNumber.Name = "lOrderNumber";
            this.lOrderNumber.Size = new System.Drawing.Size(98, 13);
            this.lOrderNumber.TabIndex = 8;
            this.lOrderNumber.Text = "Unesite broj naloga";
            // 
            // ReportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 162);
            this.Controls.Add(this.lOrderNumber);
            this.Controls.Add(this.tbOrderNumber);
            this.Controls.Add(this.bReport);
            this.Controls.Add(this.lReportType);
            this.Controls.Add(this.cbReport);
            this.Name = "ReportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Izveštaji";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbReport;
        private System.Windows.Forms.Label lReportType;
        private System.Windows.Forms.Button bReport;
        private System.Windows.Forms.TextBox tbOrderNumber;
        private System.Windows.Forms.Label lOrderNumber;
    }
}