﻿namespace Gui
{
    partial class Kutija5
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
            this.lOrderNum = new System.Windows.Forms.Label();
            this.tbOrderNum = new System.Windows.Forms.TextBox();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.lNumberOfFiles = new System.Windows.Forms.Label();
            this.bOpenCloseBox = new System.Windows.Forms.Button();
            this.cbCloseBox = new System.Windows.Forms.CheckBox();
            this.lBoxCode = new System.Windows.Forms.Label();
            this.tbBoxCode = new System.Windows.Forms.TextBox();
            this.lReport = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.izvestajiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lOrderNum
            // 
            this.lOrderNum.AutoSize = true;
            this.lOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOrderNum.Location = new System.Drawing.Point(12, 27);
            this.lOrderNum.Name = "lOrderNum";
            this.lOrderNum.Size = new System.Drawing.Size(180, 37);
            this.lOrderNum.TabIndex = 0;
            this.lOrderNum.Text = "Broj naloga";
            // 
            // tbOrderNum
            // 
            this.tbOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOrderNum.Location = new System.Drawing.Point(201, 27);
            this.tbOrderNum.Name = "tbOrderNum";
            this.tbOrderNum.Size = new System.Drawing.Size(317, 38);
            this.tbOrderNum.TabIndex = 1;
            // 
            // tbCode
            // 
            this.tbCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCode.Location = new System.Drawing.Point(15, 86);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(503, 38);
            this.tbCode.TabIndex = 2;
            this.tbCode.TextChanged += new System.EventHandler(this.tbCodeTextChanged);
            this.tbCode.Enter += new System.EventHandler(this.tbCodeEnter);
            // 
            // lNumberOfFiles
            // 
            this.lNumberOfFiles.AutoSize = true;
            this.lNumberOfFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lNumberOfFiles.Location = new System.Drawing.Point(5, 320);
            this.lNumberOfFiles.Name = "lNumberOfFiles";
            this.lNumberOfFiles.Size = new System.Drawing.Size(293, 37);
            this.lNumberOfFiles.TabIndex = 3;
            this.lNumberOfFiles.Text = "Broj fajlova u kutiji: ";
            // 
            // bOpenCloseBox
            // 
            this.bOpenCloseBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bOpenCloseBox.Location = new System.Drawing.Point(148, 418);
            this.bOpenCloseBox.Name = "bOpenCloseBox";
            this.bOpenCloseBox.Size = new System.Drawing.Size(206, 38);
            this.bOpenCloseBox.TabIndex = 4;
            this.bOpenCloseBox.Text = "Otvori kutiju";
            this.bOpenCloseBox.UseVisualStyleBackColor = true;
            this.bOpenCloseBox.Click += new System.EventHandler(this.bOpenCloseBoxClick);
            // 
            // cbCloseBox
            // 
            this.cbCloseBox.AutoSize = true;
            this.cbCloseBox.Enabled = false;
            this.cbCloseBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCloseBox.Location = new System.Drawing.Point(178, 383);
            this.cbCloseBox.Name = "cbCloseBox";
            this.cbCloseBox.Size = new System.Drawing.Size(154, 29);
            this.cbCloseBox.TabIndex = 99;
            this.cbCloseBox.TabStop = false;
            this.cbCloseBox.Text = "Zatvori kutiju";
            this.cbCloseBox.UseVisualStyleBackColor = true;
            // 
            // lBoxCode
            // 
            this.lBoxCode.AutoSize = true;
            this.lBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBoxCode.Location = new System.Drawing.Point(5, 200);
            this.lBoxCode.Name = "lBoxCode";
            this.lBoxCode.Size = new System.Drawing.Size(277, 37);
            this.lBoxCode.TabIndex = 6;
            this.lBoxCode.Text = "Unesite kod kutije:";
            // 
            // tbBoxCode
            // 
            this.tbBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBoxCode.Location = new System.Drawing.Point(12, 258);
            this.tbBoxCode.MaxLength = 15;
            this.tbBoxCode.Name = "tbBoxCode";
            this.tbBoxCode.Size = new System.Drawing.Size(506, 38);
            this.tbBoxCode.TabIndex = 3;
            this.tbBoxCode.Text = "RSRFBA01C-00001";
            // 
            // lReport
            // 
            this.lReport.AutoSize = true;
            this.lReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lReport.Location = new System.Drawing.Point(5, 143);
            this.lReport.Name = "lReport";
            this.lReport.Size = new System.Drawing.Size(0, 39);
            this.lReport.TabIndex = 8;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.izvestajiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(530, 24);
            this.menuStrip1.TabIndex = 100;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // izvestajiToolStripMenuItem
            // 
            this.izvestajiToolStripMenuItem.Name = "izvestajiToolStripMenuItem";
            this.izvestajiToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.izvestajiToolStripMenuItem.Text = "Izvestaji";
            this.izvestajiToolStripMenuItem.Click += new System.EventHandler(this.izvestajiToolStripMenuItemClick);
            // 
            // Kutija5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 469);
            this.Controls.Add(this.lReport);
            this.Controls.Add(this.tbBoxCode);
            this.Controls.Add(this.lBoxCode);
            this.Controls.Add(this.cbCloseBox);
            this.Controls.Add(this.bOpenCloseBox);
            this.Controls.Add(this.lNumberOfFiles);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.tbOrderNum);
            this.Controls.Add(this.lOrderNum);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Kutija5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kutija5";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lOrderNum;
        private System.Windows.Forms.TextBox tbOrderNum;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Label lNumberOfFiles;
        private System.Windows.Forms.Button bOpenCloseBox;
        private System.Windows.Forms.CheckBox cbCloseBox;
        private System.Windows.Forms.Label lBoxCode;
        private System.Windows.Forms.TextBox tbBoxCode;
        private System.Windows.Forms.Label lReport;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem izvestajiToolStripMenuItem;
    }
}
