using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

class NoSleepApp
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern uint SetThreadExecutionState(uint esFlags);

    private static NotifyIcon trayIcon;

    static void PreventSleep()
    {
        SetThreadExecutionState(0x80000002);
    }

    static void AllowSleep()
    {
        SetThreadExecutionState(0x80000000);
    }

    static void ShowSplashScreen()
    {
        MessageBox.Show("Program Started, PC will not sleep \n\nTo restore sleep behavior, turn off the program from the System Tray ^", "NoSleepV2", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    [STAThread]
    static void Main()
    {
        Thread splashThread = new Thread(new ThreadStart(ShowSplashScreen));
        splashThread.Start();

        Thread.Sleep(2000);

        PreventSleep();

        splashThread.Abort();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        trayIcon = new NotifyIcon()
        {
            Icon = SystemIcons.Information,  
            Text = "NoSleepV2 - by github.com/Rendekendo/",
            Visible = true,
            ContextMenuStrip = new ContextMenuStrip()
        };

        var programNameItem = new ToolStripMenuItem("NoSleepV2")
        {
            Enabled = false  
        };

        var exitItem = new ToolStripMenuItem("Exit", null, (s, e) =>
        {
            AllowSleep();  
            trayIcon.Dispose();
            Application.Exit();
        });

        trayIcon.ContextMenuStrip.Items.Add(programNameItem);  
        trayIcon.ContextMenuStrip.Items.Add(exitItem);         

        Application.Run();
    }
}
