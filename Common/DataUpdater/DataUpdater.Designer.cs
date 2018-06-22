namespace Common
{
    partial class DataUpdater
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
            this.bConfirm = new System.Windows.Forms.Button();
            this.bChoose = new System.Windows.Forms.Button();
            this.lFile = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // bConfirm
            // 
            this.bConfirm.Location = new System.Drawing.Point(90, 57);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(93, 23);
            this.bConfirm.TabIndex = 5;
            this.bConfirm.Text = "Potvrdi";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Click += new System.EventHandler(this.bConfirmClick);
            // 
            // bChoose
            // 
            this.bChoose.Location = new System.Drawing.Point(179, 19);
            this.bChoose.Name = "bChoose";
            this.bChoose.Size = new System.Drawing.Size(93, 23);
            this.bChoose.TabIndex = 4;
            this.bChoose.Text = "Odaberi";
            this.bChoose.UseVisualStyleBackColor = true;
            this.bChoose.Click += new System.EventHandler(this.bChooseClick);
            // 
            // lFile
            // 
            this.lFile.AutoSize = true;
            this.lFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFile.Location = new System.Drawing.Point(12, 19);
            this.lFile.Name = "lFile";
            this.lFile.Size = new System.Drawing.Size(97, 17);
            this.lFile.TabIndex = 3;
            this.lFile.Text = "Odaberite fajl:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            // 
            // DataUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 92);
            this.Controls.Add(this.bConfirm);
            this.Controls.Add(this.bChoose);
            this.Controls.Add(this.lFile);
            this.Name = "DataUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bConfirm;
        private System.Windows.Forms.Button bChoose;
        private System.Windows.Forms.Label lFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}