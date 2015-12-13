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
            this.lReportType = new System.Windows.Forms.Label();
            this.bReport = new System.Windows.Forms.Button();
            this.tbOrderNumber = new System.Windows.Forms.TextBox();
            this.lOrderNumber = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
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
            this.bReport.Location = new System.Drawing.Point(122, 127);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(163, 23);
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
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(319, 48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Opšti";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(319, 71);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 17);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.Text = "Detaljan";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(319, 94);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(71, 17);
            this.checkBox3.TabIndex = 11;
            this.checkBox3.Text = "Reisswolf";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // ReportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 162);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lOrderNumber);
            this.Controls.Add(this.tbOrderNumber);
            this.Controls.Add(this.bReport);
            this.Controls.Add(this.lReportType);
            this.Name = "ReportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Izveštaji";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lReportType;
        private System.Windows.Forms.Button bReport;
        private System.Windows.Forms.TextBox tbOrderNumber;
        private System.Windows.Forms.Label lOrderNumber;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}