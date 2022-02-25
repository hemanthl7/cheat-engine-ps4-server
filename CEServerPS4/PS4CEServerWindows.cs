using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEServerPS4
{
    public partial class PS4CEServerWindows : Form
    {
        ConnectType isConnected;

        CheatEngineServer cheatEngineServer;

        delegate void SetConnectedCallback();

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        string path = "ps4.ini";
        string section = "Connection";
        string ipkey = "PS4IP";

        public PS4CEServerWindows()
        {
            InitializeComponent();
            string ip = readip();
            this.IPTextBox.Text = ip;
        }

        private void CustomCEToggle_CheckedChanged(object sender, EventArgs e)
        {
            RJToggleButton but = (RJToggleButton)sender;
            if (but.Checked)
            {
                CheatEngineConstants.isCustomCheatEngine = true;
            }
            else
            {
                CheatEngineConstants.isCustomCheatEngine = false;
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string ip = this.IPTextBox.Text;
            writeip(ip);
           
            if (!ConnectType.SUCCESS.Equals(isConnected))
            {
                try
                {
                    isConnected = ConnectType.INIT;
                    cheatEngineServer = new CheatEngineServer(ip);
                    Thread nthread = new Thread(Start);
                    nthread.Start(cheatEngineServer);
                    Thread isconnectedThread = new Thread(SetConnected);
                    isconnectedThread.Start();

                }
                catch (Exception)
                {
                    isConnected = ConnectType.FAILED;
                    MessageBox.Show("Please Check If you are loaded Payload or Ps4 Properly");
                }
            }
            else
            {
                MessageBox.Show("PS4 is Connected Already");
            }
        }

        private void Start(object cheatEngineServer)
        {
            try
            {
                ((CheatEngineServer)cheatEngineServer).StartAsync().Wait();
            }
            catch (Exception e)
            {
                if (!ConnectType.SUCCESS.Equals(isConnected))
                {
                    Trace.WriteLine("Error While Creating Tcp Client");
                    Trace.WriteLine(e);
                }
            }

        }

        public void SetConnected( )
        {
            string text;
            while (true)
            {
                if (ConnectType.SUCCESS.Equals(CheatEngineServer.isConnected))
                {
                    isConnected = ConnectType.SUCCESS;
                    text = "Connected";
                    break;
                }
                else if (ConnectType.FAILED.Equals(CheatEngineServer.isConnected))
                {
                    isConnected = ConnectType.FAILED;
                    text = "Connect";
                    break;
                }
            }
            if (this.ConnectBtn.InvokeRequired)
            {
                SetConnectedCallback d = new SetConnectedCallback(SetConnected);
                this.ConnectBtn.Invoke(d);
            }
            else
            {
                this.ConnectBtn.Text = text;
            }
        }

        string readip()
        {
            try
            {
                StringBuilder ip = new StringBuilder(255);
                GetPrivateProfileString(section, ipkey, "", ip, 255, this.path);
                return ip.ToString();

            }
            catch
            {
                return "";
            }

        }

        void writeip(string ip)
        {
            try
            {
                WritePrivateProfileString(section, ipkey, ip, this.path);
            }
            catch
            {
                Trace.WriteLine("failed reading ini file");
            }
            
        }

    }
}
