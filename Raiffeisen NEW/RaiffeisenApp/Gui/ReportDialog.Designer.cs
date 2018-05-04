namespace Gui
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbBoxCode = new System.Windows.Forms.RadioButton();
            this.rbOrderNum = new System.Windows.Forms.RadioButton();
            this.rbDateTime = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Odaberite tip izveštaja";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Unesite vrednost";
            // 
            // tbValue
            // 
            this.tbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValue.Location = new System.Drawing.Point(184, 86);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(362, 26);
            this.tbValue.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(199, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Napravi izveštaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDateTime);
            this.groupBox1.Controls.Add(this.rbBoxCode);
            this.groupBox1.Controls.Add(this.rbOrderNum);
            this.groupBox1.Location = new System.Drawing.Point(184, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 50);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // rbBoxCode
            // 
            this.rbBoxCode.AutoSize = true;
            this.rbBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBoxCode.Location = new System.Drawing.Point(172, 12);
            this.rbBoxCode.Name = "rbBoxCode";
            this.rbBoxCode.Size = new System.Drawing.Size(66, 24);
            this.rbBoxCode.TabIndex = 8;
            this.rbBoxCode.TabStop = true;
            this.rbBoxCode.Text = "Kutija";
            this.rbBoxCode.UseVisualStyleBackColor = true;
            // 
            // rbOrderNum
            // 
            this.rbOrderNum.AutoSize = true;
            this.rbOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbOrderNum.Location = new System.Drawing.Point(6, 12);
            this.rbOrderNum.Name = "rbOrderNum";
            this.rbOrderNum.Size = new System.Drawing.Size(112, 24);
            this.rbOrderNum.TabIndex = 7;
            this.rbOrderNum.TabStop = true;
            this.rbOrderNum.Text = "Radni nalog";
            this.rbOrderNum.UseVisualStyleBackColor = true;
            // 
            // rbDateTime
            // 
            this.rbDateTime.AutoSize = true;
            this.rbDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDateTime.Location = new System.Drawing.Point(287, 12);
            this.rbDateTime.Name = "rbDateTime";
            this.rbDateTime.Size = new System.Drawing.Size(75, 24);
            this.rbDateTime.TabIndex = 10;
            this.rbDateTime.TabStop = true;
            this.rbDateTime.Text = "Datum";
            this.rbDateTime.UseVisualStyleBackColor = true;
            // 
            // ReportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 166);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Izveštaj";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbBoxCode;
        private System.Windows.Forms.RadioButton rbOrderNum;
        private System.Windows.Forms.RadioButton rbDateTime;
    }
}