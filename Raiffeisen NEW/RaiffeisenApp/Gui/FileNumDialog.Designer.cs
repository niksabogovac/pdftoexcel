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
            this.lText = new System.Windows.Forms.Label();
            this.bConfirm = new System.Windows.Forms.Button();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lText
            // 
            this.lText.AutoSize = true;
            this.lText.Location = new System.Drawing.Point(12, 15);
            this.lText.Name = "lText";
            this.lText.Size = new System.Drawing.Size(46, 13);
            this.lText.TabIndex = 1;
            this.lText.Text = "Unesite ";
            // 
            // bConfirm
            // 
            this.bConfirm.Enabled = false;
            this.bConfirm.Location = new System.Drawing.Point(93, 38);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(100, 23);
            this.bConfirm.TabIndex = 4;
            this.bConfirm.Text = "Potvrdi";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Visible = false;
            // 
            // tbCode
            // 
            this.tbCode.Location = new System.Drawing.Point(191, 12);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(132, 20);
            this.tbCode.TabIndex = 3;
            this.tbCode.TextChanged += new System.EventHandler(this.tbCodeTextChanged);
            // 
            // FileNumDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 74);
            this.Controls.Add(this.bConfirm);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.lText);
            this.Name = "FileNumDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FileNumDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lText;
        private System.Windows.Forms.Button bConfirm;
        private System.Windows.Forms.TextBox tbCode;
    }
}