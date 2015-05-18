namespace GUI
{
    partial class ChooseDiag
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbContainerCode = new System.Windows.Forms.TextBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Unesite kolonu u kojoj se nalazi Description ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Unesite kolonu u kojoj se nalazi ContainerCode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Unesite kolonu u kojoj se nalazi Year";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(271, 55);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(100, 20);
            this.tbDescription.TabIndex = 4;
            // 
            // tbContainerCode
            // 
            this.tbContainerCode.Location = new System.Drawing.Point(271, 84);
            this.tbContainerCode.Name = "tbContainerCode";
            this.tbContainerCode.Size = new System.Drawing.Size(100, 20);
            this.tbContainerCode.TabIndex = 5;
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(271, 119);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(100, 20);
            this.tbYear.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(171, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Potvrdi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChooseDiag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 216);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbYear);
            this.Controls.Add(this.tbContainerCode);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChooseDiag";
            this.Text = "ChooseDiag";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbContainerCode;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Button button1;
    }
}