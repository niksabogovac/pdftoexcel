namespace QR_Code
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.pUpper = new System.Windows.Forms.Panel();
            this.lNotification = new System.Windows.Forms.Label();
            this.bAddData = new System.Windows.Forms.Button();
            this.tbQr = new System.Windows.Forms.TextBox();
            this.lQr = new System.Windows.Forms.Label();
            this.tbOrderNum = new System.Windows.Forms.TextBox();
            this.lOrderNum = new System.Windows.Forms.Label();
            this.lWorker = new System.Windows.Forms.Label();
            this.mMain = new System.Windows.Forms.MenuStrip();
            this.miOption = new System.Windows.Forms.ToolStripMenuItem();
            this.dodajToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doctypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.korisnikaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izbrišiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qRCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pojedinacnoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izTabeleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kutijuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izveštajiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.izmeniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brojNalogaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpLower = new System.Windows.Forms.TableLayoutPanel();
            this.pGreen = new System.Windows.Forms.Panel();
            this.cbCloseGreen = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lGreenTableName = new System.Windows.Forms.Label();
            this.lNumFilesGreen = new System.Windows.Forms.Label();
            this.lStatusGreen = new System.Windows.Forms.Label();
            this.lGreen = new System.Windows.Forms.Label();
            this.tbGreen = new System.Windows.Forms.TextBox();
            this.bCloseGreen = new System.Windows.Forms.Button();
            this.pRed = new System.Windows.Forms.Panel();
            this.cbCloseRed = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lRedTableName = new System.Windows.Forms.Label();
            this.lNumFilesRed = new System.Windows.Forms.Label();
            this.lStatusRed = new System.Windows.Forms.Label();
            this.tbRed = new System.Windows.Forms.TextBox();
            this.lRed = new System.Windows.Forms.Label();
            this.bCloseRed = new System.Windows.Forms.Button();
            this.pYellow = new System.Windows.Forms.Panel();
            this.cbCloseYellow = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lYellowTableName = new System.Windows.Forms.Label();
            this.lNumFilesYellow = new System.Windows.Forms.Label();
            this.lStatusYellow = new System.Windows.Forms.Label();
            this.lYellow = new System.Windows.Forms.Label();
            this.tbYellow = new System.Windows.Forms.TextBox();
            this.bCloseYellow = new System.Windows.Forms.Button();
            this.pBlue = new System.Windows.Forms.Panel();
            this.cbCloseBlue = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lBlueTableName = new System.Windows.Forms.Label();
            this.lNumFilesBlue = new System.Windows.Forms.Label();
            this.lStatusBlue = new System.Windows.Forms.Label();
            this.tbBlue = new System.Windows.Forms.TextBox();
            this.lBlue = new System.Windows.Forms.Label();
            this.bCloseBlue = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pUpper.SuspendLayout();
            this.mMain.SuspendLayout();
            this.tlpLower.SuspendLayout();
            this.pGreen.SuspendLayout();
            this.pRed.SuspendLayout();
            this.pYellow.SuspendLayout();
            this.pBlue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pUpper
            // 
            this.pUpper.Controls.Add(this.lNotification);
            this.pUpper.Controls.Add(this.bAddData);
            this.pUpper.Controls.Add(this.tbQr);
            this.pUpper.Controls.Add(this.lQr);
            this.pUpper.Controls.Add(this.tbOrderNum);
            this.pUpper.Controls.Add(this.lOrderNum);
            this.pUpper.Controls.Add(this.lWorker);
            this.pUpper.Dock = System.Windows.Forms.DockStyle.Top;
            this.pUpper.Location = new System.Drawing.Point(0, 24);
            this.pUpper.Name = "pUpper";
            this.pUpper.Size = new System.Drawing.Size(1017, 124);
            this.pUpper.TabIndex = 0;
            // 
            // lNotification
            // 
            this.lNotification.AutoSize = true;
            this.lNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lNotification.Location = new System.Drawing.Point(535, 71);
            this.lNotification.Name = "lNotification";
            this.lNotification.Size = new System.Drawing.Size(0, 31);
            this.lNotification.TabIndex = 12;
            // 
            // bAddData
            // 
            this.bAddData.Location = new System.Drawing.Point(414, 41);
            this.bAddData.Name = "bAddData";
            this.bAddData.Size = new System.Drawing.Size(103, 23);
            this.bAddData.TabIndex = 11;
            this.bAddData.Text = "Unesi";
            this.bAddData.UseVisualStyleBackColor = true;
            this.bAddData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddDataMouseClick);
            // 
            // tbQr
            // 
            this.tbQr.Location = new System.Drawing.Point(541, 14);
            this.tbQr.Multiline = true;
            this.tbQr.Name = "tbQr";
            this.tbQr.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tbQr.Size = new System.Drawing.Size(464, 50);
            this.tbQr.TabIndex = 10;
            this.tbQr.TextChanged += new System.EventHandler(this.QrCodeValueChanged);
            this.tbQr.Enter += new System.EventHandler(this.QrCodeEntered);
            // 
            // lQr
            // 
            this.lQr.AutoSize = true;
            this.lQr.Location = new System.Drawing.Point(411, 13);
            this.lQr.Name = "lQr";
            this.lQr.Size = new System.Drawing.Size(106, 13);
            this.lQr.TabIndex = 5;
            this.lQr.Text = "Unesite QR Kod fajla";
            // 
            // tbOrderNum
            // 
            this.tbOrderNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOrderNum.Location = new System.Drawing.Point(182, 13);
            this.tbOrderNum.Name = "tbOrderNum";
            this.tbOrderNum.Size = new System.Drawing.Size(226, 51);
            this.tbOrderNum.TabIndex = 1;
            // 
            // lOrderNum
            // 
            this.lOrderNum.AutoSize = true;
            this.lOrderNum.Location = new System.Drawing.Point(13, 43);
            this.lOrderNum.Name = "lOrderNum";
            this.lOrderNum.Size = new System.Drawing.Size(163, 13);
            this.lOrderNum.TabIndex = 3;
            this.lOrderNum.Text = "Unesite broj naloga/primopredaje";
            // 
            // lWorker
            // 
            this.lWorker.AutoSize = true;
            this.lWorker.Location = new System.Drawing.Point(12, 13);
            this.lWorker.Name = "lWorker";
            this.lWorker.Size = new System.Drawing.Size(50, 13);
            this.lWorker.TabIndex = 0;
            this.lWorker.Text = "Radnik : ";
            // 
            // mMain
            // 
            this.mMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOption,
            this.izveštajiToolStripMenuItem,
            this.izmeniToolStripMenuItem});
            this.mMain.Location = new System.Drawing.Point(0, 0);
            this.mMain.Name = "mMain";
            this.mMain.Size = new System.Drawing.Size(1017, 24);
            this.mMain.TabIndex = 1;
            this.mMain.Text = "mGlavni";
            // 
            // miOption
            // 
            this.miOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dodajToolStripMenuItem,
            this.izbrišiToolStripMenuItem});
            this.miOption.Name = "miOption";
            this.miOption.Size = new System.Drawing.Size(53, 20);
            this.miOption.Text = "Opcije";
            // 
            // dodajToolStripMenuItem
            // 
            this.dodajToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doctypeToolStripMenuItem,
            this.korisnikaToolStripMenuItem});
            this.dodajToolStripMenuItem.Name = "dodajToolStripMenuItem";
            this.dodajToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.dodajToolStripMenuItem.Text = "Dodaj";
            // 
            // doctypeToolStripMenuItem
            // 
            this.doctypeToolStripMenuItem.Name = "doctypeToolStripMenuItem";
            this.doctypeToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.doctypeToolStripMenuItem.Text = "Doctype";
            this.doctypeToolStripMenuItem.Click += new System.EventHandler(this.DoctypeToolStripMenuItem_Click);
            // 
            // korisnikaToolStripMenuItem
            // 
            this.korisnikaToolStripMenuItem.Name = "korisnikaToolStripMenuItem";
            this.korisnikaToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.korisnikaToolStripMenuItem.Text = "Korisnika";
            this.korisnikaToolStripMenuItem.Click += new System.EventHandler(this.korisnikaToolStripMenuItem_Click);
            // 
            // izbrišiToolStripMenuItem
            // 
            this.izbrišiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qRCodeToolStripMenuItem,
            this.kutijuToolStripMenuItem});
            this.izbrišiToolStripMenuItem.Name = "izbrišiToolStripMenuItem";
            this.izbrišiToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.izbrišiToolStripMenuItem.Text = "Izbriši";
            // 
            // qRCodeToolStripMenuItem
            // 
            this.qRCodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pojedinacnoToolStripMenuItem,
            this.izTabeleToolStripMenuItem});
            this.qRCodeToolStripMenuItem.Name = "qRCodeToolStripMenuItem";
            this.qRCodeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.qRCodeToolStripMenuItem.Text = "QRCode";
            // 
            // pojedinacnoToolStripMenuItem
            // 
            this.pojedinacnoToolStripMenuItem.Name = "pojedinacnoToolStripMenuItem";
            this.pojedinacnoToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.pojedinacnoToolStripMenuItem.Text = "Pojedinacno";
            this.pojedinacnoToolStripMenuItem.Click += new System.EventHandler(this.pojedinacnoToolStripMenuItem_Click);
            // 
            // izTabeleToolStripMenuItem
            // 
            this.izTabeleToolStripMenuItem.Name = "izTabeleToolStripMenuItem";
            this.izTabeleToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.izTabeleToolStripMenuItem.Text = "Iz tabele";
            this.izTabeleToolStripMenuItem.Click += new System.EventHandler(this.izTabeleToolStripMenuItem_Click);
            // 
            // kutijuToolStripMenuItem
            // 
            this.kutijuToolStripMenuItem.Name = "kutijuToolStripMenuItem";
            this.kutijuToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.kutijuToolStripMenuItem.Text = "Kutiju";
            this.kutijuToolStripMenuItem.Click += new System.EventHandler(this.kutijuToolStripMenuItem_Click);
            // 
            // izveštajiToolStripMenuItem
            // 
            this.izveštajiToolStripMenuItem.Name = "izveštajiToolStripMenuItem";
            this.izveštajiToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.izveštajiToolStripMenuItem.Text = "Izveštaji";
            this.izveštajiToolStripMenuItem.Click += new System.EventHandler(this.ReportMenuStripItemClick);
            // 
            // izmeniToolStripMenuItem
            // 
            this.izmeniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brojNalogaToolStripMenuItem});
            this.izmeniToolStripMenuItem.Name = "izmeniToolStripMenuItem";
            this.izmeniToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.izmeniToolStripMenuItem.Text = "Izmeni";
            // 
            // brojNalogaToolStripMenuItem
            // 
            this.brojNalogaToolStripMenuItem.Name = "brojNalogaToolStripMenuItem";
            this.brojNalogaToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.brojNalogaToolStripMenuItem.Text = "Broj naloga";
            this.brojNalogaToolStripMenuItem.Click += new System.EventHandler(this.brojNalogaToolStripMenuItem_Click);
            // 
            // tlpLower
            // 
            this.tlpLower.ColumnCount = 4;
            this.tlpLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpLower.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpLower.Controls.Add(this.pGreen, 0, 0);
            this.tlpLower.Controls.Add(this.pRed, 1, 0);
            this.tlpLower.Controls.Add(this.pYellow, 2, 0);
            this.tlpLower.Controls.Add(this.pBlue, 3, 0);
            this.tlpLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLower.Location = new System.Drawing.Point(0, 148);
            this.tlpLower.Name = "tlpLower";
            this.tlpLower.RowCount = 1;
            this.tlpLower.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLower.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 396F));
            this.tlpLower.Size = new System.Drawing.Size(1017, 385);
            this.tlpLower.TabIndex = 2;
            // 
            // pGreen
            // 
            this.pGreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pGreen.Controls.Add(this.cbCloseGreen);
            this.pGreen.Controls.Add(this.label1);
            this.pGreen.Controls.Add(this.lGreenTableName);
            this.pGreen.Controls.Add(this.lNumFilesGreen);
            this.pGreen.Controls.Add(this.lStatusGreen);
            this.pGreen.Controls.Add(this.lGreen);
            this.pGreen.Controls.Add(this.tbGreen);
            this.pGreen.Controls.Add(this.bCloseGreen);
            this.pGreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGreen.Location = new System.Drawing.Point(3, 3);
            this.pGreen.Name = "pGreen";
            this.pGreen.Size = new System.Drawing.Size(248, 379);
            this.pGreen.TabIndex = 0;
            // 
            // cbCloseGreen
            // 
            this.cbCloseGreen.AutoSize = true;
            this.cbCloseGreen.Enabled = false;
            this.cbCloseGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCloseGreen.Location = new System.Drawing.Point(13, 311);
            this.cbCloseGreen.Name = "cbCloseGreen";
            this.cbCloseGreen.Size = new System.Drawing.Size(85, 28);
            this.cbCloseGreen.TabIndex = 7;
            this.cbCloseGreen.Text = "Zatvori";
            this.cbCloseGreen.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 51);
            this.label1.TabIndex = 6;
            this.label1.Text = "1";
            // 
            // lGreenTableName
            // 
            this.lGreenTableName.AutoSize = true;
            this.lGreenTableName.Location = new System.Drawing.Point(121, 9);
            this.lGreenTableName.Name = "lGreenTableName";
            this.lGreenTableName.Size = new System.Drawing.Size(55, 13);
            this.lGreenTableName.TabIndex = 5;
            this.lGreenTableName.Text = "Pozajmice";
            // 
            // lNumFilesGreen
            // 
            this.lNumFilesGreen.AutoSize = true;
            this.lNumFilesGreen.Location = new System.Drawing.Point(15, 259);
            this.lNumFilesGreen.Name = "lNumFilesGreen";
            this.lNumFilesGreen.Size = new System.Drawing.Size(0, 13);
            this.lNumFilesGreen.TabIndex = 4;
            // 
            // lStatusGreen
            // 
            this.lStatusGreen.AutoSize = true;
            this.lStatusGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatusGreen.Location = new System.Drawing.Point(9, 284);
            this.lStatusGreen.Name = "lStatusGreen";
            this.lStatusGreen.Size = new System.Drawing.Size(154, 24);
            this.lStatusGreen.TabIndex = 3;
            this.lStatusGreen.Text = "Status: Zatvorena";
            // 
            // lGreen
            // 
            this.lGreen.AutoSize = true;
            this.lGreen.Location = new System.Drawing.Point(9, 46);
            this.lGreen.Name = "lGreen";
            this.lGreen.Size = new System.Drawing.Size(109, 13);
            this.lGreen.TabIndex = 1;
            this.lGreen.Text = "Unesite kod za kutiju:";
            // 
            // tbGreen
            // 
            this.tbGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGreen.Location = new System.Drawing.Point(13, 62);
            this.tbGreen.Name = "tbGreen";
            this.tbGreen.Size = new System.Drawing.Size(178, 29);
            this.tbGreen.TabIndex = 2;
            // 
            // bCloseGreen
            // 
            this.bCloseGreen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bCloseGreen.Location = new System.Drawing.Point(0, 344);
            this.bCloseGreen.Name = "bCloseGreen";
            this.bCloseGreen.Size = new System.Drawing.Size(248, 35);
            this.bCloseGreen.TabIndex = 3;
            this.bCloseGreen.TabStop = false;
            this.bCloseGreen.Text = "Otvori";
            this.bCloseGreen.UseVisualStyleBackColor = true;
            this.bCloseGreen.Click += new System.EventHandler(this.OpenCloseGreenBoxMouseClick);
            // 
            // pRed
            // 
            this.pRed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.pRed.Controls.Add(this.cbCloseRed);
            this.pRed.Controls.Add(this.label2);
            this.pRed.Controls.Add(this.lRedTableName);
            this.pRed.Controls.Add(this.lNumFilesRed);
            this.pRed.Controls.Add(this.lStatusRed);
            this.pRed.Controls.Add(this.tbRed);
            this.pRed.Controls.Add(this.lRed);
            this.pRed.Controls.Add(this.bCloseRed);
            this.pRed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pRed.Location = new System.Drawing.Point(257, 3);
            this.pRed.Name = "pRed";
            this.pRed.Size = new System.Drawing.Size(248, 379);
            this.pRed.TabIndex = 1;
            // 
            // cbCloseRed
            // 
            this.cbCloseRed.AutoSize = true;
            this.cbCloseRed.Enabled = false;
            this.cbCloseRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCloseRed.Location = new System.Drawing.Point(19, 311);
            this.cbCloseRed.Name = "cbCloseRed";
            this.cbCloseRed.Size = new System.Drawing.Size(85, 28);
            this.cbCloseRed.TabIndex = 9;
            this.cbCloseRed.Text = "Zatvori";
            this.cbCloseRed.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 51);
            this.label2.TabIndex = 8;
            this.label2.Text = "2";
            // 
            // lRedTableName
            // 
            this.lRedTableName.AutoSize = true;
            this.lRedTableName.Location = new System.Drawing.Point(128, 9);
            this.lRedTableName.Name = "lRedTableName";
            this.lRedTableName.Size = new System.Drawing.Size(36, 13);
            this.lRedTableName.TabIndex = 7;
            this.lRedTableName.Text = "Krediti";
            // 
            // lNumFilesRed
            // 
            this.lNumFilesRed.AutoSize = true;
            this.lNumFilesRed.Location = new System.Drawing.Point(21, 259);
            this.lNumFilesRed.Name = "lNumFilesRed";
            this.lNumFilesRed.Size = new System.Drawing.Size(0, 13);
            this.lNumFilesRed.TabIndex = 6;
            // 
            // lStatusRed
            // 
            this.lStatusRed.AutoSize = true;
            this.lStatusRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatusRed.Location = new System.Drawing.Point(15, 284);
            this.lStatusRed.Name = "lStatusRed";
            this.lStatusRed.Size = new System.Drawing.Size(154, 24);
            this.lStatusRed.TabIndex = 5;
            this.lStatusRed.Text = "Status: Zatvorena";
            // 
            // tbRed
            // 
            this.tbRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRed.Location = new System.Drawing.Point(19, 62);
            this.tbRed.Name = "tbRed";
            this.tbRed.Size = new System.Drawing.Size(178, 29);
            this.tbRed.TabIndex = 4;
            // 
            // lRed
            // 
            this.lRed.AutoSize = true;
            this.lRed.Location = new System.Drawing.Point(16, 46);
            this.lRed.Name = "lRed";
            this.lRed.Size = new System.Drawing.Size(109, 13);
            this.lRed.TabIndex = 3;
            this.lRed.Text = "Unesite kod za kutiju:";
            // 
            // bCloseRed
            // 
            this.bCloseRed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bCloseRed.Location = new System.Drawing.Point(0, 344);
            this.bCloseRed.Name = "bCloseRed";
            this.bCloseRed.Size = new System.Drawing.Size(248, 35);
            this.bCloseRed.TabIndex = 5;
            this.bCloseRed.TabStop = false;
            this.bCloseRed.Text = "Otvori";
            this.bCloseRed.UseVisualStyleBackColor = true;
            this.bCloseRed.Click += new System.EventHandler(this.OpenCloseRedBoxMouseClick);
            // 
            // pYellow
            // 
            this.pYellow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pYellow.Controls.Add(this.cbCloseYellow);
            this.pYellow.Controls.Add(this.label3);
            this.pYellow.Controls.Add(this.lYellowTableName);
            this.pYellow.Controls.Add(this.lNumFilesYellow);
            this.pYellow.Controls.Add(this.lStatusYellow);
            this.pYellow.Controls.Add(this.lYellow);
            this.pYellow.Controls.Add(this.tbYellow);
            this.pYellow.Controls.Add(this.bCloseYellow);
            this.pYellow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pYellow.Location = new System.Drawing.Point(511, 3);
            this.pYellow.Name = "pYellow";
            this.pYellow.Size = new System.Drawing.Size(248, 379);
            this.pYellow.TabIndex = 2;
            // 
            // cbCloseYellow
            // 
            this.cbCloseYellow.AutoSize = true;
            this.cbCloseYellow.Enabled = false;
            this.cbCloseYellow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCloseYellow.Location = new System.Drawing.Point(15, 311);
            this.cbCloseYellow.Name = "cbCloseYellow";
            this.cbCloseYellow.Size = new System.Drawing.Size(85, 28);
            this.cbCloseYellow.TabIndex = 11;
            this.cbCloseYellow.Text = "Zatvori";
            this.cbCloseYellow.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 51);
            this.label3.TabIndex = 10;
            this.label3.Text = "3";
            // 
            // lYellowTableName
            // 
            this.lYellowTableName.AutoSize = true;
            this.lYellowTableName.Location = new System.Drawing.Point(121, 9);
            this.lYellowTableName.Name = "lYellowTableName";
            this.lYellowTableName.Size = new System.Drawing.Size(41, 13);
            this.lYellowTableName.TabIndex = 9;
            this.lYellowTableName.Text = "Računi";
            // 
            // lNumFilesYellow
            // 
            this.lNumFilesYellow.AutoSize = true;
            this.lNumFilesYellow.Location = new System.Drawing.Point(17, 259);
            this.lNumFilesYellow.Name = "lNumFilesYellow";
            this.lNumFilesYellow.Size = new System.Drawing.Size(0, 13);
            this.lNumFilesYellow.TabIndex = 8;
            // 
            // lStatusYellow
            // 
            this.lStatusYellow.AutoSize = true;
            this.lStatusYellow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatusYellow.Location = new System.Drawing.Point(11, 284);
            this.lStatusYellow.Name = "lStatusYellow";
            this.lStatusYellow.Size = new System.Drawing.Size(154, 24);
            this.lStatusYellow.TabIndex = 7;
            this.lStatusYellow.Text = "Status: Zatvorena";
            // 
            // lYellow
            // 
            this.lYellow.AutoSize = true;
            this.lYellow.Location = new System.Drawing.Point(12, 46);
            this.lYellow.Name = "lYellow";
            this.lYellow.Size = new System.Drawing.Size(109, 13);
            this.lYellow.TabIndex = 3;
            this.lYellow.Text = "Unesite kod za kutiju:";
            // 
            // tbYellow
            // 
            this.tbYellow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbYellow.Location = new System.Drawing.Point(15, 62);
            this.tbYellow.Name = "tbYellow";
            this.tbYellow.Size = new System.Drawing.Size(178, 29);
            this.tbYellow.TabIndex = 6;
            // 
            // bCloseYellow
            // 
            this.bCloseYellow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bCloseYellow.Location = new System.Drawing.Point(0, 344);
            this.bCloseYellow.Name = "bCloseYellow";
            this.bCloseYellow.Size = new System.Drawing.Size(248, 35);
            this.bCloseYellow.TabIndex = 7;
            this.bCloseYellow.TabStop = false;
            this.bCloseYellow.Text = "Otvori";
            this.bCloseYellow.UseVisualStyleBackColor = true;
            this.bCloseYellow.Click += new System.EventHandler(this.OpenCloseYellowBoxMouseClick);
            // 
            // pBlue
            // 
            this.pBlue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.pBlue.Controls.Add(this.cbCloseBlue);
            this.pBlue.Controls.Add(this.label4);
            this.pBlue.Controls.Add(this.lBlueTableName);
            this.pBlue.Controls.Add(this.lNumFilesBlue);
            this.pBlue.Controls.Add(this.lStatusBlue);
            this.pBlue.Controls.Add(this.tbBlue);
            this.pBlue.Controls.Add(this.lBlue);
            this.pBlue.Controls.Add(this.bCloseBlue);
            this.pBlue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBlue.Location = new System.Drawing.Point(765, 3);
            this.pBlue.Name = "pBlue";
            this.pBlue.Size = new System.Drawing.Size(249, 379);
            this.pBlue.TabIndex = 3;
            // 
            // cbCloseBlue
            // 
            this.cbCloseBlue.AutoSize = true;
            this.cbCloseBlue.Enabled = false;
            this.cbCloseBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCloseBlue.Location = new System.Drawing.Point(18, 311);
            this.cbCloseBlue.Name = "cbCloseBlue";
            this.cbCloseBlue.Size = new System.Drawing.Size(85, 28);
            this.cbCloseBlue.TabIndex = 12;
            this.cbCloseBlue.Text = "Zatvori";
            this.cbCloseBlue.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 51);
            this.label4.TabIndex = 11;
            this.label4.Text = "4";
            // 
            // lBlueTableName
            // 
            this.lBlueTableName.AutoSize = true;
            this.lBlueTableName.Location = new System.Drawing.Point(109, 9);
            this.lBlueTableName.Name = "lBlueTableName";
            this.lBlueTableName.Size = new System.Drawing.Size(50, 13);
            this.lBlueTableName.TabIndex = 10;
            this.lBlueTableName.Text = "Oročenja";
            // 
            // lNumFilesBlue
            // 
            this.lNumFilesBlue.AutoSize = true;
            this.lNumFilesBlue.Location = new System.Drawing.Point(20, 259);
            this.lNumFilesBlue.Name = "lNumFilesBlue";
            this.lNumFilesBlue.Size = new System.Drawing.Size(0, 13);
            this.lNumFilesBlue.TabIndex = 8;
            // 
            // lStatusBlue
            // 
            this.lStatusBlue.AutoSize = true;
            this.lStatusBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatusBlue.Location = new System.Drawing.Point(14, 284);
            this.lStatusBlue.Name = "lStatusBlue";
            this.lStatusBlue.Size = new System.Drawing.Size(154, 24);
            this.lStatusBlue.TabIndex = 7;
            this.lStatusBlue.Text = "Status: Zatvorena";
            // 
            // tbBlue
            // 
            this.tbBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBlue.Location = new System.Drawing.Point(18, 62);
            this.tbBlue.Name = "tbBlue";
            this.tbBlue.Size = new System.Drawing.Size(178, 29);
            this.tbBlue.TabIndex = 8;
            // 
            // lBlue
            // 
            this.lBlue.AutoSize = true;
            this.lBlue.Location = new System.Drawing.Point(15, 46);
            this.lBlue.Name = "lBlue";
            this.lBlue.Size = new System.Drawing.Size(109, 13);
            this.lBlue.TabIndex = 3;
            this.lBlue.Text = "Unesite kod za kutiju:";
            // 
            // bCloseBlue
            // 
            this.bCloseBlue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bCloseBlue.Location = new System.Drawing.Point(0, 344);
            this.bCloseBlue.Name = "bCloseBlue";
            this.bCloseBlue.Size = new System.Drawing.Size(249, 35);
            this.bCloseBlue.TabIndex = 9;
            this.bCloseBlue.TabStop = false;
            this.bCloseBlue.Text = "Otvori";
            this.bCloseBlue.UseVisualStyleBackColor = true;
            this.bCloseBlue.Click += new System.EventHandler(this.OpenCloseBlueBoxMouseClick);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 533);
            this.Controls.Add(this.tlpLower);
            this.Controls.Add(this.pUpper);
            this.Controls.Add(this.mMain);
            this.MainMenuStrip = this.mMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Program";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pUpper.ResumeLayout(false);
            this.pUpper.PerformLayout();
            this.mMain.ResumeLayout(false);
            this.mMain.PerformLayout();
            this.tlpLower.ResumeLayout(false);
            this.pGreen.ResumeLayout(false);
            this.pGreen.PerformLayout();
            this.pRed.ResumeLayout(false);
            this.pRed.PerformLayout();
            this.pYellow.ResumeLayout(false);
            this.pYellow.PerformLayout();
            this.pBlue.ResumeLayout(false);
            this.pBlue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pUpper;
        private System.Windows.Forms.MenuStrip mMain;
        private System.Windows.Forms.ToolStripMenuItem miOption;
        private System.Windows.Forms.TableLayoutPanel tlpLower;
        private System.Windows.Forms.Panel pGreen;
        private System.Windows.Forms.Button bCloseGreen;
        private System.Windows.Forms.Panel pRed;
        private System.Windows.Forms.Button bCloseRed;
        private System.Windows.Forms.Panel pYellow;
        private System.Windows.Forms.Button bCloseYellow;
        private System.Windows.Forms.Panel pBlue;
        private System.Windows.Forms.Button bCloseBlue;
        private System.Windows.Forms.TextBox tbGreen;
        private System.Windows.Forms.Label lGreen;
        private System.Windows.Forms.TextBox tbRed;
        private System.Windows.Forms.Label lRed;
        private System.Windows.Forms.TextBox tbYellow;
        private System.Windows.Forms.Label lYellow;
        private System.Windows.Forms.TextBox tbBlue;
        private System.Windows.Forms.Label lBlue;
        private System.Windows.Forms.Label lNumFilesGreen;
        private System.Windows.Forms.Label lStatusGreen;
        private System.Windows.Forms.Label lNumFilesRed;
        private System.Windows.Forms.Label lStatusRed;
        private System.Windows.Forms.Label lNumFilesYellow;
        private System.Windows.Forms.Label lStatusYellow;
        private System.Windows.Forms.Label lNumFilesBlue;
        private System.Windows.Forms.Label lStatusBlue;
        private System.Windows.Forms.Label lWorker;
        private System.Windows.Forms.TextBox tbOrderNum;
        private System.Windows.Forms.Label lOrderNum;
        private System.Windows.Forms.TextBox tbQr;
        private System.Windows.Forms.Label lQr;
        private System.Windows.Forms.Button bAddData;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lGreenTableName;
        private System.Windows.Forms.Label lRedTableName;
        private System.Windows.Forms.Label lYellowTableName;
        private System.Windows.Forms.Label lBlueTableName;
        private System.Windows.Forms.Label lNotification;
        private System.Windows.Forms.ToolStripMenuItem izveštajiToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem dodajToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doctypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem korisnikaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izbrišiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qRCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kutijuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izmeniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brojNalogaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pojedinacnoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izTabeleToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbCloseGreen;
        private System.Windows.Forms.CheckBox cbCloseRed;
        private System.Windows.Forms.CheckBox cbCloseYellow;
        private System.Windows.Forms.CheckBox cbCloseBlue;
    }
}

