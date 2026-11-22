using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YerIstasyonu.Customs;

namespace YerIstasyonuV1
{
    public partial class PortSettings : Form
    {
        private MainForm mainForm;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool maximized = false;
        private Size previousSize;
        private Point previousLocation;

        public PortSettings(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void UpdateButtonStates()
        {
            if (mainForm.GetSerialPortR().IsOpen)
            {
                buttonRAc.Enabled = false;
                buttonRKapa.Enabled = true;
            }
            else
            {
                buttonRAc.Enabled = true;
                buttonRKapa.Enabled = false;
            }

            if (mainForm.GetSerialPortGY().IsOpen)
            {
                buttonGYAc.Enabled = false;
                buttonGYKapa.Enabled = true;
            }
            else
            {
                buttonGYAc.Enabled = true;
                buttonGYKapa.Enabled = false;
            }

            if (mainForm.GetSerialPortHYI().IsOpen)
            {
                buttonHYIAc.Enabled = false;
                buttonHYIKapa.Enabled = true;
            }
            else
            {
                buttonHYIAc.Enabled = true;
                buttonHYIKapa.Enabled = false;
            }
        }

        private void PortSettings_Load(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void MainForm_SerialPortStateChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void comboBoxRPorts_DropDown(object sender, EventArgs e)
        {
            string[] portlists = SerialPort.GetPortNames();
            comboBoxRPorts.Items.Clear();
            comboBoxRPorts.Items.AddRange(portlists);
        }

        private void comboBoxGYPorts_DropDown(object sender, EventArgs e)
        {
            string[] portlists = SerialPort.GetPortNames();
            comboBoxGYPorts.Items.Clear();
            comboBoxGYPorts.Items.AddRange(portlists);
        }

        private void comboBoxHYIPorts_DropDown(object sender, EventArgs e)
        {
            string[] portlists = SerialPort.GetPortNames();
            comboBoxHYIPorts.Items.Clear();
            comboBoxHYIPorts.Items.AddRange(portlists);
        }


        private void buttonRAc_Click(object sender, EventArgs e)
        {
            string portName = comboBoxRPorts.Texts;
            int baudRate = Convert.ToInt32(comboBoxRBaudrate.Texts);
            int dataBits = 8;
            Parity parity = (Parity)Enum.Parse(typeof(Parity), "None");
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");

            mainForm.SetSerialPortRSettings(portName, baudRate, dataBits, parity, stopBits);
            SerialPort serialPort_R = mainForm.GetSerialPortR();
            try
            {

                serialPort_R.Open();
                serialPort_R.Write("#");

                UpdateButtonStates();
                MessageBox.Show("Connected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void buttonGYAc_Click(object sender, EventArgs e)
        {

            string portName = comboBoxGYPorts.Texts;
            int baudRate = Convert.ToInt32(comboBoxGYBaudrate.Texts);
            int dataBits = 8;
            Parity parity = (Parity)Enum.Parse(typeof(Parity), "None");
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            mainForm.SetSerialPortGYSettings(portName, baudRate, dataBits, parity, stopBits);
            SerialPort serialPort_GY = mainForm.GetSerialPortGY();
            try
            {
                serialPort_GY.Open();
                serialPort_GY.Write("#");

                UpdateButtonStates();
                MessageBox.Show("Connected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void buttonHYIAc_Click(object sender, EventArgs e)
        {
            string portName = comboBoxHYIPorts.Texts;
            int baudRate = Convert.ToInt32(comboBoxHYIBaudrate.Texts);
            int dataBits = 8;
            Parity parity = (Parity)Enum.Parse(typeof(Parity), "None");
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            mainForm.SetSerialPortHYISettings(portName, baudRate, dataBits, parity, stopBits);
            SerialPort serialPort_HYI = mainForm.GetSerialPortHYI();

            try
            {
                serialPort_HYI.Open();
                serialPort_HYI.Write("#");

                UpdateButtonStates();
                MessageBox.Show("Connected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void buttonRKapa_Click(object sender, EventArgs e)
        {
            SerialPort serialPort_R = mainForm.GetSerialPortR();
            if (serialPort_R.IsOpen)
                try
                {
                    serialPort_R.Close();
                    UpdateButtonStates();
                    MessageBox.Show("Disconnected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
        }

        private void buttonGYKapa_Click(object sender, EventArgs e)
        {
            SerialPort serialPort_GY = mainForm.GetSerialPortGY();
            if (serialPort_GY.IsOpen)
                try
                {
                    serialPort_GY.Close();
                    UpdateButtonStates();
                    MessageBox.Show("Disconnected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
        }

        private void buttonHYIKapa_Click(object sender, EventArgs e)
        {
            SerialPort serialPort_HYI = mainForm.GetSerialPortHYI();
            if (serialPort_HYI.IsOpen)
                try
                {
                    serialPort_HYI.Close();
                    UpdateButtonStates();
                    MessageBox.Show("Disconnected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
        }

        private void Port_Ayarları_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*SerialPort serialPort_R = mainForm.GetSerialPortR();
            if (serialPort_R.IsOpen)
                try
                {
                    serialPort_R.Close();
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

            if (serialPort_GY.IsOpen)
                try
                {
                    serialPort_GY.Close();
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

            if (serialPort_HYI.IsOpen)
                try
                {
                    serialPort_HYI.Close();
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }*/
        }

        private void SFCloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SFMinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SFpanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                maximized = true;
                previousSize = this.Size;
                previousLocation = this.Location;

                this.WindowState = FormWindowState.Normal;
                //this.Size = previousSize;
                this.Location = new Point(Cursor.Position.X - this.Width / 2, Cursor.Position.Y);
            }

            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void SFpanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void SFpanelMove_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
