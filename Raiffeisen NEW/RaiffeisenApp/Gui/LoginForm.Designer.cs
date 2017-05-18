namespace Gui
{
    partial class LoginForm
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.lPass = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(182, 83);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 11;
            this.bCancel.Text = "Otkaži";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(15, 83);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 10;
            this.bOK.Text = "Potvrdi";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(157, 49);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(100, 20);
            this.tbPass.TabIndex = 9;
            // 
            // lPass
            // 
            this.lPass.AutoSize = true;
            this.lPass.Location = new System.Drawing.Point(12, 49);
            this.lPass.Name = "lPass";
            this.lPass.Size = new System.Drawing.Size(93, 13);
            this.lPass.TabIndex = 8;
            this.lPass.Text = "Unesite svoju šifru";
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(12, 9);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(90, 13);
            this.lName.TabIndex = 7;
            this.lName.Text = "Unesite svoje ime";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(157, 6);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(100, 20);
            this.tbName.TabIndex = 6;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 120);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.lPass);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.tbName);
            this.MaximumSize = new System.Drawing.Size(290, 159);
            this.MinimumSize = new System.Drawing.Size(290, 159);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        public System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label lPass;
        private System.Windows.Forms.Label lName;
        public System.Windows.Forms.TextBox tbName;
    }
}