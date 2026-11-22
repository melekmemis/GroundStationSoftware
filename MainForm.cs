using GMap.NET;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.Markers;
using HelixToolkit.Wpf;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Forms.DataVisualization.Charting;
using YerIstasyonu.Customs;
using static System.Windows.Forms.Cursor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Label = System.Windows.Forms.Label;

namespace YerIstasyonuV1
{
    public partial class MainForm : Form
    {
        private int currentSecond = 0;
        private Random random = new Random();
        System.Windows.Forms.Timer chartTimer;

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

        public struct SensorData
        {
            public float enlem;
            public float boylam;
            public float gpsIrtifa;
            public float irtifa;
            public int uyduSayisi;
            public int saat;    //Int16 bak
            public int dakika;
            public int saniye;
            public float Ax;
            public float Ay;
            public float Az;
            public float Gx;
            public float Gy;
            public float Gz;
            public float angleX;
            public float angleY;
            public float angleZ;
            public byte durum;
        }
        SensorData sensorData;

        public struct SensorDataGY
        {
            public float enlem;
            public float boylam;
            public float gpsIrtifa;
            public float irtifa;
            public int uyduSayisi;
            public int saat;    //Int16 bak
            public int dakika;
            public int saniye;
            public float nem;
            public float sicaklik;
        }
        SensorDataGY sensorDataGY;

        private static float rEnlem;
        private static float rBoylam;

        private static float gyEnlem;
        private static float gyBoylam;

        private float angleXKalmanOld = 0.0f, angleXCovOld = 1.0f;
        private float angleYKalmanOld = 0.0f, angleZCovOld = 1.0f;
        private float angleZKalmanOld = 0.0f, angleYCovOld = 1.0f;

        private static float angleHYI;
        private static float angleZ;
        private static float firstTime = 0;

        private static string RData;
        private static string GYData;


        //
        private ElementHost elementHost;
        private HelixViewport3D helixViewport;
        //
        private SerialPort serialPort_R = new SerialPort();
        private SerialPort serialPort_GY = new SerialPort();
        private SerialPort serialPort_HYI = new SerialPort();
        private Dictionary<CustomCombobox, Label> comboBoxLabelMapping;
        private Dictionary<CustomCombobox, float> comboBoxValueMapping;
        public event EventHandler SerialPortStateChanged;

        GMapOverlay markersOverlay;
        GMapMarker markerR;
        GMapMarker markerGY;
        System.Windows.Forms.Timer gpsUpdateTimer;


        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool maximized = false;
        private Size previousSize;
        private Point previousLocation;
        private Thread sendThread;
        private bool isSending;
        private static byte counter;

        //
        private StreamWriter fileWriter_R;
        private StreamWriter fileWriter_GY;
        private bool isLogging = false;
        private string filePath_R;
        private string filePath_GY;
        //

        public MainForm()
        {
            InitializeComponent();
            InitializeWpfControls();
            InitializeMap();
            SetupChartIrtifa();
            SetupChartIvme();
            InitializeTimer();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.DoubleBuffered = true;
            panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
            gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
            elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
            panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
            panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
            panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
            panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
            chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
            chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            serialPort_R.DataReceived += SerialPortR_DataReceived;
            serialPort_GY.DataReceived += SerialPortGY_DataReceived;
            serialPort_HYI.DataReceived += SerialPortHYI_DataReceived;
            counter = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            
            using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle,
                System.Drawing.Color.FromArgb(13, 16, 40),
                System.Drawing.Color.FromArgb(22, 27, 67),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        public SerialPort GetSerialPortR()
        {
            return serialPort_R;
        }
        public SerialPort GetSerialPortGY()
        {
            return serialPort_GY;
        }
        public SerialPort GetSerialPortHYI()
        {
            return serialPort_HYI;
        }
        protected virtual void OnSerialPortStateChanged()
        {
            SerialPortStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetSerialPortRSettings(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            if (serialPort_R.IsOpen)
            {
                serialPort_R.Close();
            }

            serialPort_R.PortName = portName;
            serialPort_R.BaudRate = baudRate;
            serialPort_R.DataBits = dataBits;
            serialPort_R.Parity = parity;
            serialPort_R.StopBits = stopBits;

        }

        public void SetSerialPortGYSettings(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            if (serialPort_GY.IsOpen)
            {
                serialPort_GY.Close();
            }

            serialPort_GY.PortName = portName;
            serialPort_GY.BaudRate = baudRate;
            serialPort_GY.DataBits = dataBits;
            serialPort_GY.Parity = parity;
            serialPort_GY.StopBits = stopBits;

        }

        public void SetSerialPortHYISettings(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            if (serialPort_HYI.IsOpen)
            {
                serialPort_HYI.Close();
            }

            serialPort_HYI.PortName = portName;
            serialPort_HYI.BaudRate = baudRate;
            serialPort_HYI.DataBits = dataBits;
            serialPort_HYI.Parity = parity;
            serialPort_HYI.StopBits = stopBits;

        }

        List<byte> rxBuffer = new List<byte>();
        List<byte> rxBufferGY = new List<byte>();

        private void SerialPortR_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort_R.BytesToRead > 0)
            {
                int b = serialPort_R.ReadByte();
                rxBuffer.Add((byte)b);

                while (rxBuffer.Count >= 70)
                {
                    if (rxBuffer[0] == 0xAA)
                    {
                        byte[] packet = rxBuffer.Take(70).ToArray();
                        //SensorData data = ParseSensorData(packet);
                        //BeginInvoke(new Action(() => UpdateUI(data)));
                        Invoke(new Action(() =>
                        {
                            ParseSensorData(packet);
                            UpdateUI(sensorData);
                        }));

                        rxBuffer.RemoveRange(0, 70);
                    }
                    else
                    {
                        rxBuffer.RemoveAt(0);
                    }
                }

                if (rxBuffer.Count > 500)
                    rxBuffer.RemoveRange(0, rxBuffer.Count - 70);
                Invoke(new Action(() => saveData(RData, fileWriter_R)));

            }
        }


        private void SerialPortGY_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //SensorDataGY

            while (serialPort_GY.BytesToRead > 0)
            {
                int b = serialPort_GY.ReadByte();
                rxBufferGY.Add((byte)b);

                while (rxBufferGY.Count >= 41)
                {
                    if (rxBufferGY[0] == 0x0B)
                    {
                        byte[] packetGY = rxBufferGY.Take(41).ToArray();
                        Invoke(new Action(() =>
                        {
                            SensorDataGY data = ParseSensorDataGY(packetGY);
                            UpdateUIGY(data);
                        }));

                        rxBufferGY.RemoveRange(0, 41);
                    }
                    else
                    {
                        rxBufferGY.RemoveAt(0);
                    }
                }

                if (rxBufferGY.Count > 500)
                    rxBufferGY.RemoveRange(0, rxBuffer.Count - 41);
                Invoke(new Action(() => saveData(GYData, fileWriter_GY)));
            }
        }

        private void SerialPortHYI_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data3 = serialPort_HYI.ReadExisting();
        }

        private void saveData(string data, StreamWriter fileWriter)
        {
            if (isLogging && fileWriter != null)
            {
                fileWriter.WriteLine(data);
                fileWriter.Flush();
            }
        }
        //

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!isLogging)
            {
                isLogging = true;

                if (filePath_R == null || filePath_GY == null)
                {
                    using (SaveFileDialog saveFileDialog_R = new SaveFileDialog())
                    {
                        saveFileDialog_R.Filter = "Text Files (*.txt)|*.txt";
                        saveFileDialog_R.Title = "Save Port R Data";
                        if (saveFileDialog_R.ShowDialog() == DialogResult.OK)
                        {
                            filePath_R = saveFileDialog_R.FileName;
                        }
                        else
                        {
                            MessageBox.Show("Port R dosya yolu seçilmedi.");
                            isLogging = false;
                            return;
                        }
                    }

                    using (SaveFileDialog saveFileDialog_GY = new SaveFileDialog())
                    {
                        saveFileDialog_GY.Filter = "Text Files (*.txt)|*.txt";
                        saveFileDialog_GY.Title = "Save Port GY Data";
                        if (saveFileDialog_GY.ShowDialog() == DialogResult.OK)
                        {
                            filePath_GY = saveFileDialog_GY.FileName;
                        }
                        else
                        {
                            MessageBox.Show("Port GY dosya yolu seçilmedi.");
                            isLogging = false;
                            filePath_R = null;
                            return;
                        }
                    }
                }

                if (fileWriter_R == null)
                {
                    fileWriter_R = new StreamWriter(filePath_R, true);
                }

                if (fileWriter_GY == null)
                {
                    fileWriter_GY = new StreamWriter(filePath_GY, true);
                }
            }
            btnKaydet.BackColor = System.Drawing.Color.FromArgb(24, 30, 54);
        }

        private void btnKayitDurdur_Click(object sender, EventArgs e)
        {
            if (isLogging)
            {
                isLogging = false;

                if (fileWriter_R != null)
                {
                    fileWriter_R.Close();
                }

                if (fileWriter_GY != null)
                {
                    fileWriter_GY.Close();
                }

                MessageBox.Show("Veri kaydetme durduruldu.");
            }

            btnKaydet.BackColor = System.Drawing.Color.FromArgb(46, 51, 73);
        }

        private SensorData ParseSensorData(byte[] data)
        {
            if (data.Length != 70)
            {
                throw new ArgumentException("Invalid data length");
            }

            sensorData.enlem = BitConverter.ToSingle(data, 1) / 1000000;
            rEnlem = sensorData.enlem;

            sensorData.boylam = BitConverter.ToSingle(data, 5) / 1000000;
            rBoylam = sensorData.boylam;

            sensorData.gpsIrtifa = BitConverter.ToSingle(data, 9);

            sensorData.irtifa = BitConverter.ToSingle(data, 13);

            sensorData.uyduSayisi = BitConverter.ToInt32(data, 17);

            sensorData.saat = BitConverter.ToInt32(data, 21);

            sensorData.dakika = BitConverter.ToInt32(data, 25);

            sensorData.saniye = BitConverter.ToInt32(data, 29);

            sensorData.Ax = BitConverter.ToSingle(data, 33);

            sensorData.Ay = BitConverter.ToSingle(data, 37);

            sensorData.Az = BitConverter.ToSingle(data, 41);

            sensorData.Gx = BitConverter.ToSingle(data, 45);

            sensorData.Gy = BitConverter.ToSingle(data, 49);

            sensorData.Gz = BitConverter.ToSingle(data, 53);

            sensorData.angleX = BitConverter.ToSingle(data, 57);
            //sensorData.angleX = KalmanFilter(sensorData.angleX, ref angleXKalmanOld, ref angleXCovOld);

            sensorData.angleY = BitConverter.ToSingle(data, 61);
            //sensorData.angleY = KalmanFilter(sensorData.angleY, ref angleYKalmanOld, ref angleYCovOld);

            sensorData.angleZ = BitConverter.ToSingle(data, 65);
            //sensorData.angleZ = KalmanFilter(sensorData.angleZ, ref angleZKalmanOld, ref angleZCovOld);

            sensorData.durum = data[69];

            if (sensorData.angleX > sensorData.angleY)
            {
                angleHYI = sensorData.angleX;
            }

            if (sensorData.angleY > sensorData.angleX)
            {
                angleHYI = sensorData.angleY;
            }

            if (firstTime == 0)
            {
                angleZ = sensorData.angleZ;
                firstTime = 1;
            }

            RData = "Enlem: " + sensorData.enlem + "Boylam: " + sensorData.boylam.ToString()+ "Satellite Count: " + sensorData.uyduSayisi.ToString() + "Altitude: " + sensorData.irtifa.ToString() + "Ax: " + sensorData.Ax.ToString() + "Ay: " + sensorData.Ay.ToString() + "Az: " + sensorData.Az.ToString() + "Gx: "+ sensorData.Gx.ToString() + "Gy: " + sensorData.Gy.ToString() + "Gz: " + sensorData.Gz.ToString();

            return sensorData;
        }

        private SensorDataGY ParseSensorDataGY(byte[] data)
        {
            if (data.Length != 41)
            {
                throw new ArgumentException("Invalid data length");
            }

            sensorDataGY.enlem = BitConverter.ToSingle(data, 1) / 1000000;
            gyEnlem = sensorDataGY.enlem;

            sensorDataGY.boylam = BitConverter.ToSingle(data, 5) / 1000000;
            gyBoylam = sensorDataGY.boylam;

            sensorDataGY.gpsIrtifa = BitConverter.ToSingle(data, 9);

            sensorDataGY.irtifa = BitConverter.ToSingle(data, 13);

            sensorDataGY.uyduSayisi = BitConverter.ToInt32(data, 17);

            sensorDataGY.saat = BitConverter.ToInt32(data, 21);

            sensorDataGY.dakika = BitConverter.ToInt32(data, 25);

            sensorDataGY.saniye = BitConverter.ToInt32(data, 29);

            sensorDataGY.nem = BitConverter.ToSingle(data, 33);

            sensorDataGY.sicaklik = BitConverter.ToSingle(data, 37);

            GYData = "Enlem: " + sensorDataGY.enlem + "Boylam: " + sensorDataGY.boylam.ToString() + "Altitude: " + sensorDataGY.irtifa.ToString() + "Satellite Count: " + sensorDataGY.uyduSayisi.ToString();

            return sensorDataGY;
        }

        private void UpdateUI(SensorData data)
        {
            Invoke(new Action(() =>
            {
                /*textBoxAlinan.AppendText($"en: {data.enlem.ToString("F6")}, " +
                $"boy: {data.boylam.ToString("F6")}, " +
                $"irt: {data.irtifa}, " +
                $"uyduSayisi: {data.uyduSayisi}," +
                $"zaman: {data.saat}:{data.dakika}:{data.saniye} \r\n");*/
                label3.Text = data.enlem.ToString("R");
                label4.Text = data.boylam.ToString("R");
                label2.Text = data.gpsIrtifa.ToString("F2");
                label1.Text = data.irtifa.ToString("F2");
                label53.Text = data.saat.ToString("F0") + ":" + data.dakika.ToString("F0") + ":" + data.saniye.ToString("F0");
                label39.Text = data.uyduSayisi.ToString("F0");
                label13.Text = data.Ax.ToString("F2");
                label14.Text = data.Ay.ToString("F2");
                label15.Text = data.Az.ToString("F2");
                label10.Text = data.Gx.ToString("F2");
                label11.Text = data.Gy.ToString("F2");
                label12.Text = data.Gz.ToString("F2");
                label16.Text = data.angleX.ToString("F2");
                label45.Text = data.angleY.ToString("F2");
                label47.Text = data.angleZ.ToString("F2");
                label17.Text = data.durum.ToString();
                RotateModel(data.angleX, data.angleY, data.angleZ);


            }));
        }

        private void UpdateUIGY(SensorDataGY data)
        {
            Invoke(new Action(() =>
            {
                /*textBoxAlinan.AppendText($"en: {data.enlem.ToString("F6")}, " +
                $"boy: {data.boylam.ToString("F6")}, " +
                $"irt: {data.irtifa}, " +
                $"uyduSayisi: {data.uyduSayisi}," +
                $"zaman: {data.saat}:{data.dakika}:{data.saniye} \r\n");*/
                label6.Text = data.enlem.ToString("R");
                label7.Text = data.boylam.ToString("R");
                label5.Text = data.gpsIrtifa.ToString("F2");
                label51.Text = data.irtifa.ToString("F2");
                //label167.Text = data.saat.ToString("F0") + ":" + data.dakika.ToString("F0") + ":" + data.saniye.ToString("F0");
                label49.Text = data.uyduSayisi.ToString("F0");


            }));
        }

        private void MFCloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MFFullscaleBtn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                panel9.Location = new Point(4, panel6.Bottom - 55);
                panel9.Height = Math.Min(this.ClientSize.Height - 20, 320);
                panel6.Height = Math.Min(this.ClientSize.Height - 20, 370);
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
                gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
                elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
                panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
                panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
                panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
                panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
                label41.Location = new Point(panel7.Location.X + 10, panel7.Top + 40);
                label42.Location = new Point(panel7.Location.X + 10, panel7.Top + 80);
                label43.Location = new Point(panel7.Location.X + 10, panel7.Top + 160);
                chart1.Width = 340;
                chart2.Location = new Point(chart1.Right + 8, chart1.Top);
                chart2.Width = 340;
                chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
                chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));

            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                panel9.Location = new Point(4, 307);
                panel9.Height = Math.Min(1172, 204);
                panel6.Height = Math.Min(1172, 298);
                panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
                gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
                elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
                panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
                panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
                panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
                panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
                chart1.Width = 248;
                chart2.Width = 248;
                chart2.Location = new Point(258, 3);
                chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
                chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));

                label41.Location = new Point(20, 20);
                label42.Location = new Point(20, 57);
                label43.Location = new Point(20, 114);
            }
        }

        private void MFMinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                maximized = true;
                previousSize = this.Size;
                previousLocation = this.Location;

                this.WindowState = FormWindowState.Normal;
                //this.Size = previousSize;
                this.Location = new Point(System.Windows.Forms.Cursor.Position.X - this.Width / 2, System.Windows.Forms.Cursor.Position.Y);
                panel9.Location = new Point(4, 307);
                panel9.Height = Math.Min(1172, 204);
                panel6.Height = Math.Min(1172, 298);
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
                gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
                elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
                panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
                panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
                panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
                panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
                chart1.Width = 248;
                chart2.Width = 248;
                chart2.Location = new Point(258, 3);
                chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
                chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));
                label41.Location = new Point(20, 20);
                label42.Location = new Point(20, 57);
                label43.Location = new Point(20, 114);
            }

            dragging = true;
            dragCursorPoint = System.Windows.Forms.Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void panelMove_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panelMove_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                panel9.Location = new Point(4, panel6.Bottom - 55);
                panel9.Height = Math.Min(this.ClientSize.Height - 20, 320);
                panel6.Height = Math.Min(this.ClientSize.Height - 20, 370);
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
                gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
                elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
                panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
                panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
                panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
                panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
                chart1.Width = 340;
                chart2.Location = new Point(chart1.Right + 8, chart1.Top);
                chart2.Width = 340;
                chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
                chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));
                label41.Location = new Point(panel7.Location.X + 10, panel7.Top + 40);
                label42.Location = new Point(panel7.Location.X + 10, panel7.Top + 80);
                label43.Location = new Point(panel7.Location.X + 10, panel7.Top + 160);
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                panel9.Location = new Point(4, 307);
                panel9.Height = Math.Min(1172, 204);
                panel6.Height = Math.Min(1172, 298);
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                panel3.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel1.Height, 20, 20));
                gmap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, gmap.Width, gmap.Height, 20, 20));
                elementHost1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, elementHost1.Width, elementHost1.Height, 20, 20));
                panel7.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
                panel1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 20, 20));
                panel2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel1.Height, 20, 20));
                panel4.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
                chart1.Width = 248;
                chart2.Width = 248;
                chart2.Location = new Point(258, 3);
                chart1.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart1.Width, chart1.Height, 20, 20));
                chart2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, chart2.Width, chart2.Height, 20, 20));
                label41.Location = new Point(20, 20);
                label42.Location = new Point(20, 57);
                label43.Location = new Point(20, 114);
            }
        }

        private void btnPortSettings_Click(object sender, EventArgs e)
        {
            PortSettings portAyar = new PortSettings(this);
            portAyar.ShowDialog();
        }

        private void InitializeWpfControls()
        {
            elementHost = elementHost1;

            helixViewport = new HelixViewport3D
            {
                Background = System.Windows.Media.Brushes.Transparent,
                ShowCoordinateSystem = false,
                ShowFieldOfView = false,
            };
            elementHost.Child = helixViewport;

            AddLights();
            LoadStlModel("roket.stl");
        }

        private void AddLights()
        {
            var lights = new Model3DGroup();
            lights.Children.Add(new AmbientLight(Colors.Gray));
            lights.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, -1, -2)));

            var lightVisual = new ModelVisual3D { Content = lights };
            helixViewport.Children.Add(lightVisual);
        }

        private void LoadStlModel(string filePath)
        {
            try
            {
                var reader = new StLReader();
                var model3d = reader.Read(filePath);

                if (model3d is Model3DGroup group)
                {
                    foreach (var child in group.Children)
                    {
                        if (child is GeometryModel3D geometry)
                        {
                            var material = new DiffuseMaterial(new SolidColorBrush(Colors.BlueViolet));
                            geometry.Material = material;
                            geometry.BackMaterial = material;
                        }
                    }
                }

                var modelVisual = new ModelVisual3D { Content = model3d };
                helixViewport.Children.Add(modelVisual);

                helixViewport.ZoomExtents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading model: {ex.Message}");
            }
        }
        private void RotateModel(double angleX, double angleY, double angleZ)
        {
            var transformGroup = new Transform3DGroup();

            var rotateTransformX = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angleX));
            var rotateTransformY = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleY));
            var rotateTransformZ = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), angleZ));

            transformGroup.Children.Add(rotateTransformX);
            transformGroup.Children.Add(rotateTransformY);
            transformGroup.Children.Add(rotateTransformZ);

            foreach (var child in helixViewport.Children)
            {
                if (child is ModelVisual3D modelVisual)
                {
                    modelVisual.Transform = transformGroup;
                }
            }
        }

        private void InitializeMap()
        {
            gmap.MapProvider = GMapProviders.GoogleMap;
            gmap.Position = new PointLatLng(38.33, 33.97);
            gmap.MinZoom = 2;
            gmap.MaxZoom = 18;
            gmap.Zoom = 15;

            markersOverlay = new GMapOverlay("markers");
            markerR = new GMarkerGoogle(gmap.Position, GMarkerGoogleType.purple_dot);
            markerGY = new GMarkerGoogle(gmap.Position, GMarkerGoogleType.yellow_dot);
            markersOverlay.Markers.Add(markerR);
            markersOverlay.Markers.Add(markerGY);
            gmap.Overlays.Add(markersOverlay);
        }

        private void InitializeTimer()
        {
            gpsUpdateTimer = new System.Windows.Forms.Timer();
            gpsUpdateTimer.Interval = 1000;
            gpsUpdateTimer.Tick += GpsUpdateTimer_Tick;
            gpsUpdateTimer.Start();

            chartTimer = new System.Windows.Forms.Timer();
            chartTimer.Interval = 1000;
            chartTimer.Tick += chartTimer_Tick;
            chartTimer.Start();
        }

        private void GpsUpdateTimer_Tick(object sender, EventArgs e)
        {

            double latitudeR = GetLatitudeFromGPSR();
            double longitudeR = GetLongitudeFromGPSR();

            double latitudeGY = GetLatitudeFromGPSGY();
            double longitudeGY = GetLongitudeFromGPSGY();

            UpdateMapPosition(markerR, latitudeR, longitudeR);
            UpdateMapPosition(markerGY, latitudeGY, longitudeGY);
        }

        private double GetLatitudeFromGPSR()
        {
            return rEnlem;
        }

        private double GetLongitudeFromGPSR()
        {
            SensorData sensorData = new SensorData();
            return rBoylam;
        }

        private double GetLatitudeFromGPSGY()
        {
            return gyEnlem;
        }

        private double GetLongitudeFromGPSGY()
        {
            return gyBoylam;
        }

        private void UpdateMapPosition(GMapMarker marker, double latitude, double longitude)
        {
            var point = new PointLatLng(latitude, longitude);
            marker.Position = point;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            panel3.BackColor = System.Drawing.Color.FromArgb(40, 0, 0, 0);
            elementHost1.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            panel7.BackColor = System.Drawing.Color.FromArgb(40, 0, 0, 0);
            panel1.BackColor = System.Drawing.Color.FromArgb(40, 0, 0, 0);
            panel4.BackColor = System.Drawing.Color.FromArgb(40, 0, 0, 0);
            chart1.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            chart2.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            helixViewport.ZoomExtents();

        }

        public struct FLOAT32_UINT8_DONUSTURUCU
        {
            public float sayi;
            public byte byte0
            {
                get { return BitConverter.GetBytes(sayi)[0]; }
            }
            public byte byte1
            {
                get { return BitConverter.GetBytes(sayi)[1]; }
            }
            public byte byte2
            {
                get { return BitConverter.GetBytes(sayi)[2]; }
            }
            public byte byte3
            {
                get { return BitConverter.GetBytes(sayi)[3]; }
            }
        }

        public static byte[] olusturalacak_paket = new byte[78];

        public static byte check_sum_hesapla()
        {
            int check_sum = 0;

            for (int i = 4; i < 75; i++)
            {
                check_sum += olusturalacak_paket[i];
            }

            return (byte)(check_sum % 256);
        }

        public static void paket_olustur(SensorData data, SensorDataGY datagy)
        {
            SensorDataGY sensorDataGY = new SensorDataGY();

            olusturalacak_paket[0] = 0xFF; // Sabit
            olusturalacak_paket[1] = 0xFF; // Sabit
            olusturalacak_paket[2] = 0x54; // Sabit
            olusturalacak_paket[3] = 0x52; // Sabit

            olusturalacak_paket[4] = 108;   // Takim ID = 0
            olusturalacak_paket[5] = counter; // Sayac degeri = 0

            FLOAT32_UINT8_DONUSTURUCU irtifa_float32_uint8_donusturucu;
            irtifa_float32_uint8_donusturucu.sayi = data.irtifa; // Irtifa degerinin atamasini yapiyoruz.
            olusturalacak_paket[6] = irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[7] = irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[8] = irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[9] = irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU roket_gps_irtifa_float32_uint8_donusturucu;
            roket_gps_irtifa_float32_uint8_donusturucu.sayi = data.gpsIrtifa; // Roket GPS Irtifa degerinin atamasini yapiyoruz.
            olusturalacak_paket[10] = roket_gps_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[11] = roket_gps_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[12] = roket_gps_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[13] = roket_gps_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU roket_enlem_float32_uint8_donusturucu;
            roket_enlem_float32_uint8_donusturucu.sayi = data.enlem; // Roket enlem degerinin atamasini yapiyoruz.
            olusturalacak_paket[14] = roket_enlem_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[15] = roket_enlem_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[16] = roket_enlem_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[17] = roket_enlem_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU roket_boylam_irtifa_float32_uint8_donusturucu;
            roket_boylam_irtifa_float32_uint8_donusturucu.sayi = data.boylam; // Roket boylam degerinin atamasini yapiyoruz.
            olusturalacak_paket[18] = roket_boylam_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[19] = roket_boylam_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[20] = roket_boylam_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[21] = roket_boylam_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU gorev_yuku_gps_irtifa_float32_uint8_donusturucu;
            gorev_yuku_gps_irtifa_float32_uint8_donusturucu.sayi = datagy.gpsIrtifa; // Gorev yuku GPS irtifa degerinin atamasini yapiyoruz.
            olusturalacak_paket[22] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[23] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[24] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[25] = gorev_yuku_gps_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU gorev_yuku_enlem_float32_uint8_donusturucu;
            gorev_yuku_enlem_float32_uint8_donusturucu.sayi = datagy.enlem; // Gorev yuku enlem degerinin atamasini yapiyoruz.
            olusturalacak_paket[26] = gorev_yuku_enlem_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[27] = gorev_yuku_enlem_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[28] = gorev_yuku_enlem_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[29] = gorev_yuku_enlem_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU gorev_yuku_boylam_irtifa_float32_uint8_donusturucu;
            gorev_yuku_boylam_irtifa_float32_uint8_donusturucu.sayi = datagy.boylam; // Gorev yuku boylam degerinin atamasini yapiyoruz.
            olusturalacak_paket[30] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[31] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[32] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[33] = gorev_yuku_boylam_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU kademe_gps_irtifa_float32_uint8_donusturucu;
            kademe_gps_irtifa_float32_uint8_donusturucu.sayi = datagy.irtifa; // Kademe GPS Irtifa degerinin atamasini yapiyoruz.
            olusturalacak_paket[34] = kademe_gps_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[35] = kademe_gps_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[36] = kademe_gps_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[37] = kademe_gps_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU kademe_enlem_float32_uint8_donusturucu;
            kademe_enlem_float32_uint8_donusturucu.sayi = 0; // Kademe enlem degerinin atamasini yapiyoruz.
            olusturalacak_paket[38] = kademe_enlem_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[39] = kademe_enlem_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[40] = kademe_enlem_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[41] = kademe_enlem_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU kademe_boylam_irtifa_float32_uint8_donusturucu;
            kademe_boylam_irtifa_float32_uint8_donusturucu.sayi = 0; // Kademe boylam degerinin atamasini yapiyoruz.
            olusturalacak_paket[42] = kademe_boylam_irtifa_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[43] = kademe_boylam_irtifa_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[44] = kademe_boylam_irtifa_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[45] = kademe_boylam_irtifa_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU jiroskop_x_float32_uint8_donusturucu;
            jiroskop_x_float32_uint8_donusturucu.sayi = data.Gx; // Jiroskop X degerinin atamasini yapiyoruz.
            olusturalacak_paket[46] = jiroskop_x_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[47] = jiroskop_x_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[48] = jiroskop_x_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[49] = jiroskop_x_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU jiroskop_y_float32_uint8_donusturucu;
            jiroskop_y_float32_uint8_donusturucu.sayi = data.Gy; // Jiroskop Y degerinin atamasini yapiyoruz.
            olusturalacak_paket[50] = jiroskop_y_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[51] = jiroskop_y_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[52] = jiroskop_y_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[53] = jiroskop_y_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU jiroskop_z_float32_uint8_donusturucu;
            jiroskop_z_float32_uint8_donusturucu.sayi = data.Gz; // Jiroskop Z degerinin atamasini yapiyoruz.
            olusturalacak_paket[54] = jiroskop_z_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[55] = jiroskop_z_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[56] = jiroskop_z_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[57] = jiroskop_z_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU ivme_x_float32_uint8_donusturucu;
            ivme_x_float32_uint8_donusturucu.sayi = data.Ax; // Ivme X degerinin atamasini yapiyoruz.
            olusturalacak_paket[58] = ivme_x_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[59] = ivme_x_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[60] = ivme_x_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[61] = ivme_x_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU ivme_y_float32_uint8_donusturucu;
            ivme_y_float32_uint8_donusturucu.sayi = data.Ay; // Ivme Y degerinin atamasini yapiyoruz.
            olusturalacak_paket[62] = ivme_y_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[63] = ivme_y_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[64] = ivme_y_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[65] = ivme_y_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU ivme_z_float32_uint8_donusturucu;
            ivme_z_float32_uint8_donusturucu.sayi = data.Az; // Ivme Z degerinin atamasini yapiyoruz.
            olusturalacak_paket[66] = ivme_z_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[67] = ivme_z_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[68] = ivme_z_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[69] = ivme_z_float32_uint8_donusturucu.byte3;

            FLOAT32_UINT8_DONUSTURUCU aci_float32_uint8_donusturucu;
            aci_float32_uint8_donusturucu.sayi = angleHYI; // Aci degerinin atamasini yapiyoruz.
            olusturalacak_paket[70] = aci_float32_uint8_donusturucu.byte0;
            olusturalacak_paket[71] = aci_float32_uint8_donusturucu.byte1;
            olusturalacak_paket[72] = aci_float32_uint8_donusturucu.byte2;
            olusturalacak_paket[73] = aci_float32_uint8_donusturucu.byte3;

            olusturalacak_paket[74] = 0; // Durum bilgisi = Iki parasut de tetiklenmedi

            olusturalacak_paket[75] = check_sum_hesapla(); // Check_sum = check_sum_hesapla();
            olusturalacak_paket[76] = 0x0D; // Sabit
            olusturalacak_paket[77] = 0x0A; // Sabit
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            if (!serialPort_HYI.IsOpen)
            {
                serialPort_HYI.Open();
            }

            if (sendThread == null || !sendThread.IsAlive)
            {
                isSending = !isSending;
                sendThread = new Thread(SendData);
                sendThread.Start();
            }
        }

        private void SendData()
        {
            while (isSending)
            {
                try
                {
                    /*for (int i = 0; i < 78; i++)
                    {
                       serialPort.Write("0x{0:X2} ", olusturalacak_paket[i]);
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");*/
                    paket_olustur(sensorData, sensorDataGY);
                    label35.Text = check_sum_hesapla().ToString();

                    if (this.WindowState == FormWindowState.Normal)
                    {
                        label42.Text = ("[" + (olusturalacak_paket[0]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[1]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[2]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[3]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[4]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[5]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[6]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[7]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[8]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[9]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[10]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[11]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[12]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[13]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[14]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[15]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[16]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[17]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[18]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[19]).ToString("X2") + "] \n" +
                                    "[" + (olusturalacak_paket[20]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[21]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[22]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[23]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[24]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[25]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[26]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[27]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[28]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[29]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[30]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[31]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[32]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[33]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[34]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[35]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[36]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[37]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[38]).ToString("X2") + "] ");

                        label43.Text = ("[" + (olusturalacak_paket[39]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[40]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[41]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[42]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[43]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[44]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[45]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[46]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[47]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[48]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[49]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[50]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[51]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[52]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[53]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[54]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[55]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[56]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[57]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[58]).ToString("X2") + "] \n" +
                                        "[" + (olusturalacak_paket[59]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[60]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[61]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[62]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[63]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[64]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[65]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[66]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[67]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[68]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[69]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[70]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[71]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[72]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[73]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[74]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[75]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[76]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[77]).ToString("X2") + "] ");
                    }
                    else
                    {
                        label42.Text = ("[" + (olusturalacak_paket[0]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[1]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[2]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[3]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[4]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[5]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[6]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[7]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[8]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[9]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[10]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[11]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[12]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[13]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[14]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[15]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[16]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[17]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[18]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[19]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[20]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[21]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[22]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[23]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[24]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[25]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[26]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[27]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[28]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[29]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[30]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[31]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[32]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[33]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[34]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[35]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[36]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[37]).ToString("X2") + "] " +
                                    "[" + (olusturalacak_paket[38]).ToString("X2") + "] ");

                        label43.Text = ("[" + (olusturalacak_paket[39]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[40]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[41]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[42]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[43]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[44]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[45]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[46]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[47]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[48]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[49]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[50]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[51]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[52]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[53]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[54]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[55]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[56]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[57]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[58]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[59]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[60]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[61]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[62]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[63]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[64]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[65]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[66]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[67]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[68]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[69]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[70]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[71]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[72]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[73]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[74]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[75]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[76]).ToString("X2") + "] " +
                                        "[" + (olusturalacak_paket[77]).ToString("X2") + "] ");
                    }

                    for (int i = 0; i < 78; i++)
                    {
                        serialPort_HYI.Write(olusturalacak_paket, 0, olusturalacak_paket.Length);
                    }
                    counter++;
                    if (counter > 255)
                    {
                        counter = 0;
                    }
                    Thread.Sleep(500);
                    label167.Text = counter.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    isSending = false;
                }
            }

        }

        private float KalmanFilter(float input, ref float kalman_old, ref float cov_old)
        {
            const float Q = 0.002f;
            const float R = 0.02f;

            cov_old += Q;
            float kalman_gain = cov_old / (cov_old + R);
            float kalman_calculated = kalman_old + kalman_gain * (input - kalman_old);
            cov_old *= (1 - kalman_gain);
            kalman_old = kalman_calculated;

            return kalman_calculated;
        }

        private void SetupChartIrtifa()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea yukseklikArea = new ChartArea("YukseklikArea");
            yukseklikArea.AxisX.Title = "Zaman (s)";
            yukseklikArea.AxisY.Title = "İrtifa (m)";
            yukseklikArea.AxisY.TitleForeColor = System.Drawing.Color.White;
            yukseklikArea.AxisX.TitleForeColor = System.Drawing.Color.White;
            yukseklikArea.Position = new ElementPosition(0, 5, 100, 95);
            yukseklikArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);

            chart1.ChartAreas.Add(yukseklikArea);

            Series roketIrtifa = new Series("Roket İrtifa");
            roketIrtifa.Color = System.Drawing.Color.Goldenrod;
            roketIrtifa.ChartType = SeriesChartType.Line;
            roketIrtifa.ChartArea = "YukseklikArea";

            Series gorevYukuIrtifa = new Series("Görev Yükü İrtifa");
            gorevYukuIrtifa.Color = System.Drawing.Color.Aquamarine;
            gorevYukuIrtifa.ChartType = SeriesChartType.Line;
            gorevYukuIrtifa.ChartArea = "YukseklikArea";

            chart1.ChartAreas["YukseklikArea"].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas["YukseklikArea"].AxisY.MajorGrid.Enabled = true;

            chart1.ChartAreas["YukseklikArea"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas["YukseklikArea"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart1.ChartAreas["YukseklikArea"].AxisY.MajorGrid.LineWidth = 1;
            chart1.ChartAreas["YukseklikArea"].AxisX.LineColor = System.Drawing.Color.White;
            chart1.ChartAreas["YukseklikArea"].AxisY.LineColor = System.Drawing.Color.White;
            chart1.ChartAreas["YukseklikArea"].AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chart1.ChartAreas["YukseklikArea"].AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;

            chart1.Series.Add(roketIrtifa);
            chart1.Series.Add(gorevYukuIrtifa);
        }

        private void SetupChartIvme()
        {
            chart2.Series.Clear();
            chart2.ChartAreas.Clear();

            ChartArea ivmeArea = new ChartArea("ivmeArea");
            ivmeArea.AxisX.Title = "Zaman (s)";
            ivmeArea.AxisY.Title = "İvme (g)";
            ivmeArea.AxisY.TitleForeColor = System.Drawing.Color.White;
            ivmeArea.AxisX.TitleForeColor = System.Drawing.Color.White;
            ivmeArea.Position = new ElementPosition(0, 5, 100, 95);
            ivmeArea.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);

            chart2.ChartAreas.Add(ivmeArea);

            Series ivmeX = new Series("ivmeX");
            ivmeX.Color = System.Drawing.Color.MediumPurple;
            ivmeX.ChartType = SeriesChartType.Line;
            ivmeX.ChartArea = "ivmeArea";

            Series ivmeY = new Series("ivmeY");
            ivmeY.Color = System.Drawing.Color.LightBlue;
            ivmeY.ChartType = SeriesChartType.Line;
            ivmeY.ChartArea = "ivmeArea";

            Series ivmeZ = new Series("ivmeZ");
            ivmeZ.Color = System.Drawing.Color.LightGreen;
            ivmeZ.ChartType = SeriesChartType.Line;
            ivmeZ.ChartArea = "ivmeArea";

            chart2.ChartAreas["ivmeArea"].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas["ivmeArea"].AxisY.MajorGrid.Enabled = true;

            chart2.ChartAreas["ivmeArea"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart2.ChartAreas["ivmeArea"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart2.ChartAreas["ivmeArea"].AxisY.MajorGrid.LineWidth = 1;
            chart2.ChartAreas["ivmeArea"].AxisX.LineColor = System.Drawing.Color.White;
            chart2.ChartAreas["ivmeArea"].AxisY.LineColor = System.Drawing.Color.White;
            chart2.ChartAreas["ivmeArea"].AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chart2.ChartAreas["ivmeArea"].AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;

            chart2.Series.Add(ivmeX);
            chart2.Series.Add(ivmeY);
            chart2.Series.Add(ivmeZ);
        }

        private void chartTimer_Tick(object sender, EventArgs e)
        {

            chart1.Series["Roket İrtifa"].Points.AddXY(currentSecond, sensorData.irtifa);
            chart1.Series["Görev Yükü İrtifa"].Points.AddXY(currentSecond, sensorDataGY.irtifa);

            chart2.Series["ivmeX"].Points.AddXY(currentSecond, sensorData.Ax);
            chart2.Series["ivmeY"].Points.AddXY(currentSecond, sensorData.Ay);
            chart2.Series["ivmeZ"].Points.AddXY(currentSecond, sensorData.Az);

            currentSecond++;

            if (currentSecond > 30)
            {
                chart1.ChartAreas["YukseklikArea"].AxisX.Minimum = currentSecond - 30;
                chart2.ChartAreas["ivmeArea"].AxisX.Minimum = currentSecond - 30;
            }
        }
    }
}
