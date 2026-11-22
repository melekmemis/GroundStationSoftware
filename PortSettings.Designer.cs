namespace YerIstasyonuV1
{
    partial class PortSettings
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
            this.buttonHYIKapa = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBoxHYIBaudrate = new YerIstasyonu.Customs.CustomCombobox();
            this.comboBoxHYIPorts = new YerIstasyonu.Customs.CustomCombobox();
            this.buttonHYIAc = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBoxGYBaudrate = new YerIstasyonu.Customs.CustomCombobox();
            this.buttonGYKapa = new System.Windows.Forms.Button();
            this.comboBoxGYPorts = new YerIstasyonu.Customs.CustomCombobox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonGYAc = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxRBaudrate = new YerIstasyonu.Customs.CustomCombobox();
            this.comboBoxRPorts = new YerIstasyonu.Customs.CustomCombobox();
            this.buttonRKapa = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRAc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SFpanelMove = new System.Windows.Forms.Panel();
            this.SFMinimizeBtn = new System.Windows.Forms.Button();
            this.SFCloseBtn = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SFpanelMove.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonHYIKapa
            // 
            this.buttonHYIKapa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonHYIKapa.FlatAppearance.BorderSize = 0;
            this.buttonHYIKapa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonHYIKapa.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHYIKapa.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonHYIKapa.Location = new System.Drawing.Point(117, 115);
            this.buttonHYIKapa.Name = "buttonHYIKapa";
            this.buttonHYIKapa.Size = new System.Drawing.Size(120, 29);
            this.buttonHYIKapa.TabIndex = 38;
            this.buttonHYIKapa.Text = "Bağlantıyı Kes";
            this.buttonHYIKapa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonHYIKapa.UseVisualStyleBackColor = true;
            this.buttonHYIKapa.Click += new System.EventHandler(this.buttonHYIKapa_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(8, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 20);
            this.label6.TabIndex = 35;
            this.label6.Text = "COM Ports :";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(8, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 20);
            this.label7.TabIndex = 36;
            this.label7.Text = "Baudrate :";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 20);
            this.label8.TabIndex = 41;
            this.label8.Text = "HYI";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(44)))), ((int)(((byte)(69)))));
            this.panel3.Controls.Add(this.comboBoxHYIBaudrate);
            this.panel3.Controls.Add(this.buttonHYIKapa);
            this.panel3.Controls.Add(this.comboBoxHYIPorts);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.buttonHYIAc);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Location = new System.Drawing.Point(522, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(249, 157);
            this.panel3.TabIndex = 62;
            // 
            // comboBoxHYIBaudrate
            // 
            this.comboBoxHYIBaudrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxHYIBaudrate.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxHYIBaudrate.BorderSize = 2;
            this.comboBoxHYIBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxHYIBaudrate.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxHYIBaudrate.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxHYIBaudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "115200"});
            this.comboBoxHYIBaudrate.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxHYIBaudrate.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxHYIBaudrate.Location = new System.Drawing.Point(117, 72);
            this.comboBoxHYIBaudrate.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxHYIBaudrate.Name = "comboBoxHYIBaudrate";
            this.comboBoxHYIBaudrate.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxHYIBaudrate.Size = new System.Drawing.Size(120, 24);
            this.comboBoxHYIBaudrate.TabIndex = 50;
            this.comboBoxHYIBaudrate.Texts = "";
            // 
            // comboBoxHYIPorts
            // 
            this.comboBoxHYIPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxHYIPorts.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxHYIPorts.BorderSize = 2;
            this.comboBoxHYIPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxHYIPorts.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxHYIPorts.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxHYIPorts.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxHYIPorts.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxHYIPorts.Location = new System.Drawing.Point(117, 42);
            this.comboBoxHYIPorts.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxHYIPorts.Name = "comboBoxHYIPorts";
            this.comboBoxHYIPorts.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxHYIPorts.Size = new System.Drawing.Size(120, 24);
            this.comboBoxHYIPorts.TabIndex = 49;
            this.comboBoxHYIPorts.Texts = "";
            this.comboBoxHYIPorts.DropDown += new System.EventHandler(this.comboBoxHYIPorts_DropDown);
            // 
            // buttonHYIAc
            // 
            this.buttonHYIAc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonHYIAc.FlatAppearance.BorderSize = 0;
            this.buttonHYIAc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonHYIAc.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHYIAc.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonHYIAc.Location = new System.Drawing.Point(12, 115);
            this.buttonHYIAc.Name = "buttonHYIAc";
            this.buttonHYIAc.Size = new System.Drawing.Size(99, 29);
            this.buttonHYIAc.TabIndex = 37;
            this.buttonHYIAc.Text = "Bağlan";
            this.buttonHYIAc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonHYIAc.UseVisualStyleBackColor = true;
            this.buttonHYIAc.Click += new System.EventHandler(this.buttonHYIAc_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(44)))), ((int)(((byte)(69)))));
            this.panel2.Controls.Add(this.comboBoxGYBaudrate);
            this.panel2.Controls.Add(this.buttonGYKapa);
            this.panel2.Controls.Add(this.comboBoxGYPorts);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.buttonGYAc);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(267, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(249, 157);
            this.panel2.TabIndex = 61;
            // 
            // comboBoxGYBaudrate
            // 
            this.comboBoxGYBaudrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxGYBaudrate.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxGYBaudrate.BorderSize = 2;
            this.comboBoxGYBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxGYBaudrate.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxGYBaudrate.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxGYBaudrate.Items.AddRange(new object[] {
            "9600",
            "115200",
            "38400"});
            this.comboBoxGYBaudrate.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxGYBaudrate.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxGYBaudrate.Location = new System.Drawing.Point(117, 72);
            this.comboBoxGYBaudrate.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxGYBaudrate.Name = "comboBoxGYBaudrate";
            this.comboBoxGYBaudrate.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxGYBaudrate.Size = new System.Drawing.Size(120, 24);
            this.comboBoxGYBaudrate.TabIndex = 48;
            this.comboBoxGYBaudrate.Texts = "";
            // 
            // buttonGYKapa
            // 
            this.buttonGYKapa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonGYKapa.FlatAppearance.BorderSize = 0;
            this.buttonGYKapa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGYKapa.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGYKapa.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonGYKapa.Location = new System.Drawing.Point(117, 115);
            this.buttonGYKapa.Name = "buttonGYKapa";
            this.buttonGYKapa.Size = new System.Drawing.Size(120, 29);
            this.buttonGYKapa.TabIndex = 38;
            this.buttonGYKapa.Text = "Bağlantıyı Kes";
            this.buttonGYKapa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonGYKapa.UseVisualStyleBackColor = true;
            this.buttonGYKapa.Click += new System.EventHandler(this.buttonGYKapa_Click);
            // 
            // comboBoxGYPorts
            // 
            this.comboBoxGYPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxGYPorts.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxGYPorts.BorderSize = 2;
            this.comboBoxGYPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxGYPorts.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxGYPorts.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxGYPorts.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxGYPorts.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxGYPorts.Location = new System.Drawing.Point(117, 42);
            this.comboBoxGYPorts.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxGYPorts.Name = "comboBoxGYPorts";
            this.comboBoxGYPorts.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxGYPorts.Size = new System.Drawing.Size(120, 24);
            this.comboBoxGYPorts.TabIndex = 47;
            this.comboBoxGYPorts.Texts = "";
            this.comboBoxGYPorts.DropDown += new System.EventHandler(this.comboBoxGYPorts_DropDown);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(8, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 35;
            this.label3.Text = "COM Ports :";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 36;
            this.label4.Text = "Baudrate :";
            // 
            // buttonGYAc
            // 
            this.buttonGYAc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonGYAc.FlatAppearance.BorderSize = 0;
            this.buttonGYAc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGYAc.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGYAc.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonGYAc.Location = new System.Drawing.Point(12, 115);
            this.buttonGYAc.Name = "buttonGYAc";
            this.buttonGYAc.Size = new System.Drawing.Size(99, 29);
            this.buttonGYAc.TabIndex = 37;
            this.buttonGYAc.Text = "Bağlan";
            this.buttonGYAc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonGYAc.UseVisualStyleBackColor = true;
            this.buttonGYAc.Click += new System.EventHandler(this.buttonGYAc_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 41;
            this.label5.Text = "Görev Yükü";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(44)))), ((int)(((byte)(69)))));
            this.panel1.Controls.Add(this.comboBoxRBaudrate);
            this.panel1.Controls.Add(this.comboBoxRPorts);
            this.panel1.Controls.Add(this.buttonRKapa);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonRAc);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(249, 157);
            this.panel1.TabIndex = 60;
            // 
            // comboBoxRBaudrate
            // 
            this.comboBoxRBaudrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxRBaudrate.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxRBaudrate.BorderSize = 2;
            this.comboBoxRBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxRBaudrate.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxRBaudrate.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxRBaudrate.Items.AddRange(new object[] {
            "9600",
            "115200",
            "38400"});
            this.comboBoxRBaudrate.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxRBaudrate.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxRBaudrate.Location = new System.Drawing.Point(117, 72);
            this.comboBoxRBaudrate.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxRBaudrate.Name = "comboBoxRBaudrate";
            this.comboBoxRBaudrate.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxRBaudrate.Size = new System.Drawing.Size(120, 24);
            this.comboBoxRBaudrate.TabIndex = 46;
            this.comboBoxRBaudrate.Texts = "";
            // 
            // comboBoxRPorts
            // 
            this.comboBoxRPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxRPorts.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxRPorts.BorderSize = 2;
            this.comboBoxRPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxRPorts.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxRPorts.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.comboBoxRPorts.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.comboBoxRPorts.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxRPorts.Location = new System.Drawing.Point(117, 42);
            this.comboBoxRPorts.MinimumSize = new System.Drawing.Size(40, 13);
            this.comboBoxRPorts.Name = "comboBoxRPorts";
            this.comboBoxRPorts.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxRPorts.Size = new System.Drawing.Size(120, 24);
            this.comboBoxRPorts.TabIndex = 45;
            this.comboBoxRPorts.Texts = "";
            this.comboBoxRPorts.DropDown += new System.EventHandler(this.comboBoxRPorts_DropDown);
            // 
            // buttonRKapa
            // 
            this.buttonRKapa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRKapa.FlatAppearance.BorderSize = 0;
            this.buttonRKapa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRKapa.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRKapa.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonRKapa.Location = new System.Drawing.Point(117, 115);
            this.buttonRKapa.Name = "buttonRKapa";
            this.buttonRKapa.Size = new System.Drawing.Size(120, 29);
            this.buttonRKapa.TabIndex = 38;
            this.buttonRKapa.Text = "Bağlantıyı Kes";
            this.buttonRKapa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonRKapa.UseVisualStyleBackColor = true;
            this.buttonRKapa.Click += new System.EventHandler(this.buttonRKapa_Click);
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.Control;
            this.label20.Location = new System.Drawing.Point(8, 42);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(91, 20);
            this.label20.TabIndex = 35;
            this.label20.Text = "COM Ports :";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(8, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Baudrate :";
            // 
            // buttonRAc
            // 
            this.buttonRAc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRAc.FlatAppearance.BorderSize = 0;
            this.buttonRAc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRAc.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRAc.ForeColor = System.Drawing.Color.MediumPurple;
            this.buttonRAc.Location = new System.Drawing.Point(12, 115);
            this.buttonRAc.Name = "buttonRAc";
            this.buttonRAc.Size = new System.Drawing.Size(99, 29);
            this.buttonRAc.TabIndex = 37;
            this.buttonRAc.Text = "Bağlan";
            this.buttonRAc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonRAc.UseVisualStyleBackColor = true;
            this.buttonRAc.Click += new System.EventHandler(this.buttonRAc_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 20);
            this.label2.TabIndex = 41;
            this.label2.Text = "Roketten Gelen Veriler";
            // 
            // SFpanelMove
            // 
            this.SFpanelMove.Controls.Add(this.SFMinimizeBtn);
            this.SFpanelMove.Controls.Add(this.SFCloseBtn);
            this.SFpanelMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.SFpanelMove.Location = new System.Drawing.Point(0, 0);
            this.SFpanelMove.Name = "SFpanelMove";
            this.SFpanelMove.Size = new System.Drawing.Size(786, 29);
            this.SFpanelMove.TabIndex = 59;
            this.SFpanelMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SFpanelMove_MouseDown);
            this.SFpanelMove.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SFpanelMove_MouseMove);
            this.SFpanelMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SFpanelMove_MouseUp);
            // 
            // SFMinimizeBtn
            // 
            this.SFMinimizeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SFMinimizeBtn.BackgroundImage = global::YerIstasyonuV1.Properties.Resources.minimize_24dp_FILL0_wght400_GRAD0_opsz241;
            this.SFMinimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SFMinimizeBtn.FlatAppearance.BorderSize = 0;
            this.SFMinimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SFMinimizeBtn.Location = new System.Drawing.Point(728, 1);
            this.SFMinimizeBtn.Name = "SFMinimizeBtn";
            this.SFMinimizeBtn.Size = new System.Drawing.Size(27, 27);
            this.SFMinimizeBtn.TabIndex = 4;
            this.SFMinimizeBtn.UseVisualStyleBackColor = true;
            this.SFMinimizeBtn.Click += new System.EventHandler(this.SFMinimizeBtn_Click);
            // 
            // SFCloseBtn
            // 
            this.SFCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SFCloseBtn.BackgroundImage = global::YerIstasyonuV1.Properties.Resources.close_24dp_FILL0_wght400_GRAD0_opsz241;
            this.SFCloseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SFCloseBtn.FlatAppearance.BorderSize = 0;
            this.SFCloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SFCloseBtn.Location = new System.Drawing.Point(758, 1);
            this.SFCloseBtn.Name = "SFCloseBtn";
            this.SFCloseBtn.Size = new System.Drawing.Size(27, 27);
            this.SFCloseBtn.TabIndex = 2;
            this.SFCloseBtn.UseVisualStyleBackColor = true;
            this.SFCloseBtn.Click += new System.EventHandler(this.SFCloseBtn_Click);
            // 
            // PortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(16)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(786, 204);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SFpanelMove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PortSettings";
            this.Text = "PortSettings";
            this.Load += new System.EventHandler(this.PortSettings_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.SFpanelMove.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonHYIKapa;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonHYIAc;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonGYKapa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonGYAc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonRKapa;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRAc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SFMinimizeBtn;
        private System.Windows.Forms.Button SFCloseBtn;
        private System.Windows.Forms.Panel SFpanelMove;
        private YerIstasyonu.Customs.CustomCombobox comboBoxHYIBaudrate;
        private YerIstasyonu.Customs.CustomCombobox comboBoxHYIPorts;
        private YerIstasyonu.Customs.CustomCombobox comboBoxGYBaudrate;
        private YerIstasyonu.Customs.CustomCombobox comboBoxGYPorts;
        private YerIstasyonu.Customs.CustomCombobox comboBoxRBaudrate;
        private YerIstasyonu.Customs.CustomCombobox comboBoxRPorts;
    }
}