using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEServerPS4
{
    public partial class PS4CEServerWindows : Form
    {
        bool isConnected;
        public PS4CEServerWindows()
        {
            InitializeComponent();
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
            if (!isConnected)
            {
                try
                {
                    CheatEngineServer cheatEngineServer = new CheatEngineServer(ip);
                    isConnected = true;
                    Thread nthread = new Thread(Start) { IsBackground = true };
                    nthread.Start(cheatEngineServer);
                    this.ConnectBtn.Text = "Connected";
                }
                catch (Exception)
                {
                    isConnected = false;
                    MessageBox.Show("Please Check If you are loaded Payload or Ps4 Properly");
                }
            }
            else
            {
                MessageBox.Show("PS4 is Connected Already");
            }
        }

        private void Start(Object cheatEngineServer)
        {
            ((CheatEngineServer)cheatEngineServer).StartAsync().Wait();
        }
    }
}
