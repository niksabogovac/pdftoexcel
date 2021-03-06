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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kutija5));
            this.lOrderNum = new System.Windows.Forms.Label();
            this.tbOrderNum = new System.Windows.Forms.TextBox();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.lNumberOfFiles = new System.Windows.Forms.Label();
            this.bOpenCloseBox = new System.Windows.Forms.Button();
            this.cbCloseBox = new System.Windows.Forms.CheckBox();
            this.lBoxCode = new System.Windows.Forms.Label();
            this.tbBoxCode = new System.Windows.Forms.TextBox();
            this.lReport = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbChange = new System.Windows.Forms.ToolStripDropDownButton();
            this.oJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brojNalogaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbDelete = new System.Windows.Forms.ToolStripDropDownButton();
            this.kutijuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qRCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pojedinačnoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izTabeleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDateTakeover = new System.Windows.Forms.Label();
            this.dtpTakeover = new System.Windows.Forms.DateTimePicker();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lOrderNum
            // 
            this.lOrderNum.AutoSize = true;
            this.lOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lOrderNum.Location = new System.Drawing.Point(8, 25);
            this.lOrderNum.Name = "lOrderNum";
            this.lOrderNum.Size = new System.Drawing.Size(180, 37);
            this.lOrderNum.TabIndex = 0;
            this.lOrderNum.Text = "Broj naloga";
            // 
            // tbOrderNum
            // 
            this.tbOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOrderNum.Location = new System.Drawing.Point(201, 24);
            this.tbOrderNum.Name = "tbOrderNum";
            this.tbOrderNum.Size = new System.Drawing.Size(329, 38);
            this.tbOrderNum.TabIndex = 1;
            // 
            // tbCode
            // 
            this.tbCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCode.Location = new System.Drawing.Point(15, 140);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(515, 38);
            this.tbCode.TabIndex = 2;
            this.tbCode.TextChanged += new System.EventHandler(this.tbCodeTextChanged);
            this.tbCode.Enter += new System.EventHandler(this.tbCodeEnter);
            // 
            // lNumberOfFiles
            // 
            this.lNumberOfFiles.AutoSize = true;
            this.lNumberOfFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lNumberOfFiles.Location = new System.Drawing.Point(8, 428);
            this.lNumberOfFiles.Name = "lNumberOfFiles";
            this.lNumberOfFiles.Size = new System.Drawing.Size(293, 37);
            this.lNumberOfFiles.TabIndex = 3;
            this.lNumberOfFiles.Text = "Broj fajlova u kutiji: ";
            // 
            // bOpenCloseBox
            // 
            this.bOpenCloseBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bOpenCloseBox.Location = new System.Drawing.Point(151, 526);
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
            this.cbCloseBox.Location = new System.Drawing.Point(181, 491);
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
            this.lBoxCode.Location = new System.Drawing.Point(8, 308);
            this.lBoxCode.Name = "lBoxCode";
            this.lBoxCode.Size = new System.Drawing.Size(277, 37);
            this.lBoxCode.TabIndex = 6;
            this.lBoxCode.Text = "Unesite kod kutije:";
            // 
            // tbBoxCode
            // 
            this.tbBoxCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBoxCode.Location = new System.Drawing.Point(15, 366);
            this.tbBoxCode.MaxLength = 16;
            this.tbBoxCode.Name = "tbBoxCode";
            this.tbBoxCode.Size = new System.Drawing.Size(515, 38);
            this.tbBoxCode.TabIndex = 3;
            // 
            // lReport
            // 
            this.lReport.AutoSize = true;
            this.lReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lReport.Location = new System.Drawing.Point(5, 194);
            this.lReport.Name = "lReport";
            this.lReport.Size = new System.Drawing.Size(0, 39);
            this.lReport.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.tsbChange,
            this.tsbDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(542, 25);
            this.toolStrip1.TabIndex = 101;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(65, 22);
            this.toolStripButton1.Text = "Izveštaj";
            this.toolStripButton1.ToolTipText = "Izveštaj";
            this.toolStripButton1.Click += new System.EventHandler(this.reportToolStripClicked);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(63, 22);
            this.toolStripButton2.Text = "Import";
            this.toolStripButton2.Click += new System.EventHandler(this.importToolStripClicked);
            // 
            // tsbChange
            // 
            this.tsbChange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oJToolStripMenuItem,
            this.brojNalogaToolStripMenuItem});
            this.tsbChange.Image = ((System.Drawing.Image)(resources.GetObject("tsbChange.Image")));
            this.tsbChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbChange.Name = "tsbChange";
            this.tsbChange.Size = new System.Drawing.Size(71, 22);
            this.tsbChange.Text = "Izmeni";
            // 
            // oJToolStripMenuItem
            // 
            this.oJToolStripMenuItem.Name = "oJToolStripMenuItem";
            this.oJToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.oJToolStripMenuItem.Text = "OJ";
            this.oJToolStripMenuItem.Click += new System.EventHandler(this.oJToolStripMenuItem_Click);
            // 
            // brojNalogaToolStripMenuItem
            // 
            this.brojNalogaToolStripMenuItem.Name = "brojNalogaToolStripMenuItem";
            this.brojNalogaToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.brojNalogaToolStripMenuItem.Text = "Broj naloga";
            this.brojNalogaToolStripMenuItem.Click += new System.EventHandler(this.brojNalogaToolStripMenuItem_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kutijuToolStripMenuItem,
            this.qRCodeToolStripMenuItem});
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(66, 22);
            this.tsbDelete.Text = "Izbriši";
            // 
            // kutijuToolStripMenuItem
            // 
            this.kutijuToolStripMenuItem.Name = "kutijuToolStripMenuItem";
            this.kutijuToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.kutijuToolStripMenuItem.Text = "Kutiju";
            this.kutijuToolStripMenuItem.Click += new System.EventHandler(this.kutijuToolStripMenuItem_Click);
            // 
            // qRCodeToolStripMenuItem
            // 
            this.qRCodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pojedinačnoToolStripMenuItem,
            this.izTabeleToolStripMenuItem});
            this.qRCodeToolStripMenuItem.Name = "qRCodeToolStripMenuItem";
            this.qRCodeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.qRCodeToolStripMenuItem.Text = "QRCode";
            // 
            // pojedinačnoToolStripMenuItem
            // 
            this.pojedinačnoToolStripMenuItem.Name = "pojedinačnoToolStripMenuItem";
            this.pojedinačnoToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.pojedinačnoToolStripMenuItem.Text = "Pojedinačno";
            this.pojedinačnoToolStripMenuItem.Click += new System.EventHandler(this.pojedinačnoToolStripMenuItem_Click);
            // 
            // izTabeleToolStripMenuItem
            // 
            this.izTabeleToolStripMenuItem.Name = "izTabeleToolStripMenuItem";
            this.izTabeleToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.izTabeleToolStripMenuItem.Text = "Iz tabele";
            this.izTabeleToolStripMenuItem.Click += new System.EventHandler(this.izTabeleToolStripMenuItem_Click);
            // 
            // lDateTakeover
            // 
            this.lDateTakeover.AutoSize = true;
            this.lDateTakeover.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDateTakeover.Location = new System.Drawing.Point(8, 76);
            this.lDateTakeover.Name = "lDateTakeover";
            this.lDateTakeover.Size = new System.Drawing.Size(308, 37);
            this.lDateTakeover.TabIndex = 102;
            this.lDateTakeover.Text = "Datum primopredaje";
            // 
            // dtpTakeover
            // 
            this.dtpTakeover.Location = new System.Drawing.Point(322, 88);
            this.dtpTakeover.Name = "dtpTakeover";
            this.dtpTakeover.Size = new System.Drawing.Size(200, 20);
            this.dtpTakeover.TabIndex = 103;
            // 
            // Kutija5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 576);
            this.Controls.Add(this.dtpTakeover);
            this.Controls.Add(this.lDateTakeover);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lReport);
            this.Controls.Add(this.tbBoxCode);
            this.Controls.Add(this.lBoxCode);
            this.Controls.Add(this.cbCloseBox);
            this.Controls.Add(this.bOpenCloseBox);
            this.Controls.Add(this.lNumberOfFiles);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.tbOrderNum);
            this.Controls.Add(this.lOrderNum);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(546, 508);
            this.Name = "Kutija5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RaiffeisenApp5";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton tsbDelete;
        private System.Windows.Forms.ToolStripMenuItem kutijuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qRCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pojedinačnoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izTabeleToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tsbChange;
        private System.Windows.Forms.ToolStripMenuItem oJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brojNalogaToolStripMenuItem;
        private System.Windows.Forms.Label lDateTakeover;
        private System.Windows.Forms.DateTimePicker dtpTakeover;
    }
}

