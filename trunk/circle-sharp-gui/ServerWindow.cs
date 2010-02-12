using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace circle_sharp_gui
{
    public partial class ServerWindow : Form
    {
		delegate void PrintText(string text);

		private bool _enabled = false;

		public bool Enabled { get { return _enabled; } set { _enabled = value; } }

        public ServerWindow()
        {
            InitializeComponent();
        }

		private void ServerWindow_Load (object sender, EventArgs e)
		{

		}

		public void Log(string text)
		{
			if (this.InvokeRequired)
			{
				PrintText d = new PrintText (Log);

				this.Invoke (d, new object[] { text });
			}
			else
			{
                if (!outputTextBox.Disposing)
                {

					try
					{
						outputTextBox.SelectionStart = outputTextBox.Text.Length;
						outputTextBox.AppendText(text + "\n");
						outputTextBox.SelectionStart = outputTextBox.Text.Length;
						outputTextBox.ScrollToCaret();
					}
					catch
					{

					}
                }
			}
		}

		private void ServerWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_enabled)
			{
				e.Cancel = true;
				this.Hide ();
			}
		}

    }
}