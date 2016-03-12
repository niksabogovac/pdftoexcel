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
            this.lValue = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.lChoose = new System.Windows.Forms.Label();
            this.cbChoose = new System.Windows.Forms.ComboBox();
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimeUntil = new System.Windows.Forms.DateTimePicker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lReportType
            // 
            this.lReportType.AutoSize = true;
            this.lReportType.Location = new System.Drawing.Point(15, 93);
            this.lReportType.Name = "lReportType";
            this.lReportType.Size = new System.Drawing.Size(117, 13);
            this.lReportType.TabIndex = 5;
            this.lReportType.Text = "Izaberite vrstu izveštaja";
            // 
            // bReport
            // 
            this.bReport.Location = new System.Drawing.Point(123, 155);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(163, 23);
            this.bReport.TabIndex = 6;
            this.bReport.Text = "Kreiraj";
            this.bReport.UseVisualStyleBackColor = true;
            this.bReport.Click += new System.EventHandler(this.BReport_Click);
            // 
            // tbOrderNumber
            // 
            this.tbOrderNumber.Location = new System.Drawing.Point(184, 39);
            this.tbOrderNumber.Name = "tbOrderNumber";
            this.tbOrderNumber.Size = new System.Drawing.Size(200, 20);
            this.tbOrderNumber.TabIndex = 7;
            this.tbOrderNumber.Visible = false;
            // 
            // lValue
            // 
            this.lValue.AutoSize = true;
            this.lValue.Location = new System.Drawing.Point(15, 39);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(98, 13);
            this.lValue.TabIndex = 8;
            this.lValue.Text = "Unesite broj naloga";
            this.lValue.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(313, 92);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Opšti";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(313, 122);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 17);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.Text = "Detaljan";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(313, 155);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(71, 17);
            this.checkBox3.TabIndex = 11;
            this.checkBox3.Text = "Reisswolf";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // lChoose
            // 
            this.lChoose.AutoSize = true;
            this.lChoose.Location = new System.Drawing.Point(15, 9);
            this.lChoose.Name = "lChoose";
            this.lChoose.Size = new System.Drawing.Size(135, 13);
            this.lChoose.TabIndex = 12;
            this.lChoose.Text = "Izaberite kriterijum izveštaja";
            // 
            // cbChoose
            // 
            this.cbChoose.AllowDrop = true;
            this.cbChoose.FormattingEnabled = true;
            this.cbChoose.Items.AddRange(new object[] {
            "Broj naloga",
            "Datum"});
            this.cbChoose.Location = new System.Drawing.Point(263, 12);
            this.cbChoose.Name = "cbChoose";
            this.cbChoose.Size = new System.Drawing.Size(121, 21);
            this.cbChoose.TabIndex = 13;
            this.cbChoose.SelectedIndexChanged += new System.EventHandler(this.cbChoose_SelectedIndexChanged);
            // 
            // dateTimeFrom
            // 
            this.dateTimeFrom.Location = new System.Drawing.Point(184, 39);
            this.dateTimeFrom.Name = "dateTimeFrom";
            this.dateTimeFrom.Size = new System.Drawing.Size(200, 20);
            this.dateTimeFrom.TabIndex = 14;
            this.dateTimeFrom.Visible = false;
            // 
            // dateTimeUntil
            // 
            this.dateTimeUntil.Location = new System.Drawing.Point(184, 65);
            this.dateTimeUntil.Name = "dateTimeUntil";
            this.dateTimeUntil.Size = new System.Drawing.Size(200, 20);
            this.dateTimeUntil.TabIndex = 15;
            this.dateTimeUntil.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(18, 116);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(268, 23);
            this.progressBar1.TabIndex = 16;
            // 
            // ReportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 190);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dateTimeUntil);
            this.Controls.Add(this.dateTimeFrom);
            this.Controls.Add(this.cbChoose);
            this.Controls.Add(this.lChoose);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lValue);
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
        private System.Windows.Forms.Label lValue;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label lChoose;
        private System.Windows.Forms.ComboBox cbChoose;
        private System.Windows.Forms.DateTimePicker dateTimeFrom;
        private System.Windows.Forms.DateTimePicker dateTimeUntil;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}