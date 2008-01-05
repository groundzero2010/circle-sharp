using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using SharpMUD;

namespace circle_sharp_gui
{
    class ServerApp
    {
        private NotifyIcon _notifyIcon;
        private ContextMenu _notificationMenu;
        private ServerWindow _serverWindow;
        private SharpCore _serverEngine;
        private System.Threading.Thread _engineThread;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isFirstInstance;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (Mutex mtx = new Mutex(true, "SharpMUDServer", out isFirstInstance))
            {
                if (isFirstInstance)
                {
                    ServerApp app = new ServerApp();

                    Application.Run();

                    app.Dispose();
                }
                else
                {
                    // The application is already running
                    // TODO: Display message box or change focus to existing application instance
                }
            }
        }

        public ServerApp()
        {
            // Create the notification tray icon that will be used to access features.

            _notifyIcon = new NotifyIcon();
            _notificationMenu = new ContextMenu(InitializeMenu());

            _notifyIcon.DoubleClick += IconDoubleClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (ServerResources));
            _notifyIcon.Icon = (Icon)resources.GetObject("ServerAppIcon");
            _notifyIcon.ContextMenu = _notificationMenu;

            _notifyIcon.Visible = true;

            // Create the window that we will use to control the server.

            _serverWindow = new ServerWindow();

            // Now we want to create the server engine and attach the events required.

            _serverEngine = new SharpCore();

            // Attach the events of the engine we will need.
            _serverEngine.Started += EngineStarted;
            _serverEngine.Stopped += EngineStopped;

            _engineThread = new Thread(new ThreadStart(_serverEngine.Start));
            _engineThread.Start();
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }

        private MenuItem[] InitializeMenu()
        {
            MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};

            return menu;
        }

        private void menuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("About This Application");
        }

        private void menuExitClick(object sender, EventArgs e)
        {
            _serverEngine.Stop();

            Application.Exit();
        }

        private void IconDoubleClick(object sender, EventArgs e)
        {
            _serverWindow.Visible = !_serverWindow.Visible;
        }

        private void EngineStarted(object sender, EventArgs args)
        {
            MessageBox.Show("Engine Started");
        }

        private void EngineStopped(object sender, EventArgs args)
        {
            MessageBox.Show("Engine Stopped");
        }
    }
}