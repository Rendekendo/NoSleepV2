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
        MessageBox.Show("Program Spustený, PC sa neuspí \n\nAk Chcete obnoviť spánok vypnite program cez System Tray Šípku ^", "NoSleepV2", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Icon = new Icon(@"IconPath"), 
            Text = "NoSleepV2 - by github.com/Rendekendo/",
            Visible = true,
            ContextMenuStrip = new ContextMenuStrip()
        };

        var exitItem = new ToolStripMenuItem("Ukončiť", null, (s, e) =>
        {
            AllowSleep();  
            trayIcon.Dispose();
            Application.Exit();
        });

        trayIcon.ContextMenuStrip.Items.Add(exitItem);

        Application.Run();
    }
}
