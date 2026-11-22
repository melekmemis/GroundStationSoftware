using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;


namespace YerIstasyonu.Customs
{
    [DefaultEvent("OnSelectedIndexChanged")]
    class CustomCombobox : UserControl
    {
        private ComboBox comboBox;

        //Fields
        private Color backColor = Color.WhiteSmoke;
        private Color iconColor = Color.MediumSlateBlue;
        private Color listBackColor = Color.FromArgb(230, 228, 245);
        private Color listTextColor = Color.DimGray;
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 1;

        //Items
        private ComboBox cmbList;
        private Label lblText;
        private Button btnIcon;

        //Properties
        // -> Appearance
        [Category("CustomCombobox - Appearance")]
        public new Color BackColor 
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                lblText.BackColor = backColor;
                btnIcon.BackColor = backColor;
            }
        }

        [Category("CustomCombobox - Appearance")]
        public Color IconColor 
        {
            get
            {
                return iconColor;
            }
            set
            {
                iconColor = value;
                btnIcon.Invalidate(); //Redraw Icon
            }
        }

        [Category("CustomCombobox - Appearance")]
        public Color ListBackColor 
        { 
            get
            {
                return listBackColor;
            }
            set
            {
                listBackColor = value;
                cmbList.BackColor = listBackColor;
            }
        }

        [Category("CustomCombobox - Appearance")]
        public Color ListTextColor 
        {
            get
            {
                return listTextColor;
            }
            set
            {
                listTextColor = value;
                cmbList.ForeColor = listTextColor;
            }
        }

        [Category("CustomCombobox - Appearance")]
        public Color BorderColor 
        { 
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                base.BackColor = borderColor; //Border Color
            }
        }

        [Category("CustomCombobox - Appearance")]
        public int BorderSize 
        { 
            get
            {
                return borderSize;
            }
            set
            {
                borderSize = value;
                this.Padding = new Padding(borderSize); //Border Size
                AdjustComboBoxDimensions();
            }
        }

        [Category("CustomCombobox - Appearance")]
        public override Color ForeColor 
        { 
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                lblText.ForeColor = value;
            }
        }

        [Category("CustomCombobox - Appearance")]
        public override Font Font 
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                lblText.Font = value;
                cmbList.Font = value; //optional
            }
        }

        [Category("CustomCombobox - Appearance")]
        public string Texts
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }

        [Category("CustomCombobox - Appearance")]
        public ComboBoxStyle DropDownStyle
        {
            get
            {
                return cmbList.DropDownStyle;
            }
            set
            {
                if (cmbList.DropDownStyle!= ComboBoxStyle.Simple)
                    cmbList.DropDownStyle = value;
            }
        }

        // -> Data
        [Category("CustomCombobox - Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        public ComboBox.ObjectCollection Items
        {
            get { return cmbList.Items; }
        }

        [Category("CustomCombobox - Data")]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource 
        { 
            get
            {
                return cmbList.DataSource;
            }
            set
            {
                cmbList.DataSource = value;
            }
        }

        [Category("CustomCombobox - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteMode.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteMode autoCompleteMode
        {
            get
            {
                return cmbList.AutoCompleteMode;
            }
            set
            {
                cmbList.AutoCompleteMode = value;
            }
        }

        [Category("CustomCombobox - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteSource.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteSource autoCompleteSource
        {
            get
            {
                return cmbList.AutoCompleteSource;
            }
            set
            {
                cmbList.AutoCompleteSource = value;
            }
        }

        [Category("CustomCombobox - Data")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Localizable(true)]
        public AutoCompleteStringCollection autoCompleteCustomSource
        {
            get
            {
                return cmbList.AutoCompleteCustomSource;
            }
            set
            {
                cmbList.AutoCompleteCustomSource = value;
            }
        }

        [Category("CustomCombobox - Data")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                return cmbList.SelectedIndex;
            }
            set
            {
                cmbList.SelectedIndex = value;
                lblText.Text = cmbList.SelectedItem != null ? cmbList.SelectedItem.ToString() : string.Empty;
            }
        }

        [Category("CustomCombobox - Data")]
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get
            {
                return cmbList.SelectedItem;
            }
            set
            {
                cmbList.SelectedItem = value;
                lblText.Text = cmbList.SelectedItem != null ? cmbList.SelectedItem.ToString() : string.Empty;
            }
        }

        //Events
        public event EventHandler OnSelectedIndexChanged; //Default event

        [Category("Behavior")]
        [Description("Occurs when the drop-down portion of the ComboBox is shown.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event EventHandler DropDown;

        //Constructor
        public CustomCombobox()
        {
            cmbList = new ComboBox();
            lblText = new Label();
            btnIcon = new Button();
            this.SuspendLayout();

            //Combobox: Dropdown List
            cmbList.BackColor = listBackColor;
            cmbList.Font = new Font(this.Font.Name, 10F);
            cmbList.ForeColor = listTextColor;
            cmbList.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged); //Default event
            cmbList.TextChanged += new EventHandler(ComboBox_TextChanged); //Refresh Text
            cmbList.DropDown += new EventHandler(ComboBox_DropDown);

            //Button : Icon
            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = backColor;
            btnIcon.Size = new Size(20, 20);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += new EventHandler(Icon_Click); //Open Dropdown List
            btnIcon.Paint += new PaintEventHandler(Icon_Paint); //Draw icon

            //Label: Text
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = backColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, 0, 0); // 8'i değiştir
            lblText.Font = new Font(this.Font.Name, 9F);
            lblText.Click += new EventHandler(Surface_Click); //Select combobox
            lblText.MouseLeave += new EventHandler(Mouse_Leave);
            lblText.MouseEnter += new EventHandler(Mouse_Enter);

            //User Control
            this.Controls.Add(lblText);//2
            this.Controls.Add(btnIcon);//1
            this.Controls.Add(cmbList);//0
            this.MinimumSize = new Size(40, 13); //değiştir
            this.Size = new Size(40,10);
            this.ForeColor = Color.DimGray;
            this.Padding = new Padding(borderSize); //Border Size
            base.BackColor = borderColor; //Border Color
            this.ResumeLayout();
            AdjustComboBoxDimensions();
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            OnDropDown(EventArgs.Empty);
        }

        public ComboBox ComboBox
        {
            get { return comboBox; }
        }

        //Private Methods
        private void AdjustComboBoxDimensions()
        {
            cmbList.Width = lblText.Width;
            cmbList.Location = new Point()
            {
                X = lblText.Left,
                Y = lblText.Top - cmbList.Height
            };
        }
        //Event Methods

        // -> Default Event
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged.Invoke(sender, e);
            //Refresh Text
            lblText.Text = cmbList.SelectedItem != null ? cmbList.SelectedItem.ToString() : string.Empty;
        }
        

        //-> Draw Icon
        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            //Fields
            int iconWidht = 7;
            int iconHeight = 3;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidht)/2, (btnIcon.Height - iconHeight)/2, iconWidht, iconHeight);
            Graphics graph = e.Graphics;

            //Draw arrow down icon
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidht / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidht / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        //-> Items Actions
        private void Surface_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
            //Select ComboBox
            cmbList.Select();
            if (cmbList.DropDownStyle == ComboBoxStyle.DropDownList)
                cmbList.DroppedDown = true; //Open Dropdown List
        }
        
        private void Icon_Click(object sender, EventArgs e)
        {
            //Open Dropdown List
            cmbList.Select();
            cmbList.DroppedDown = true;
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            //Refresh Text
            lblText.Text = cmbList.Text;
        }

        private void Mouse_Enter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void Mouse_Leave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            DropDown?.Invoke(this, e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
        }
    }
}
