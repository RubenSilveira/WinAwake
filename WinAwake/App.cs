using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

[assembly: AssemblyTitle("WinAwake")]
[assembly: AssemblyDescription("WinAwake")]
[assembly: AssemblyCompany("WinAwake")]
[assembly: AssemblyCopyright("Ruben Silveira")]
[assembly: AssemblyProduct("WinAwake")]
[assembly: Guid("FE3D24DB-5C73-4AC0-AB1F-C4E76A45D5C3")]

[assembly: AssemblyVersion("1.23.7.16")]

class App : ApplicationContext
{
    private static readonly Icon MainIcon;
    private static readonly Icon IdleIcon;

    private readonly SysTray sysTray;
    private readonly Worker worker;

    static App()
    {
        using (Stream strem = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAwake.Main.ico"))
        {
            MainIcon = new Icon(strem);
        }

        using (Stream strem = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAwake.Idle.ico"))
        {
            IdleIcon = new Icon(strem);
        }
    }

    [STAThread]
    static int Main()
    {
        try
        {
            _ = new Mutex(true,
                (WindowsIdentity.GetCurrent().User?.ToString() ?? string.Empty)
                + ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value,
                out bool isNewMutex);
            if (isNewMutex)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new App());
            }
        }
        catch
        {
            return 1;
        }

        return 0;
    }

    public App()
    {
        sysTray = new SysTray("Idle", IdleIcon);
        worker = new Worker();

        sysTray.DoubleClicked += SysTray_DoubleClicked;
        sysTray.ExitActivated += SysTray_ExitActivated;

        if (0 != (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\WinAwake", "ActiveOnStart", 0))
        {
            SysTray_DoubleClicked(null, null);
        }
    }

    private void SysTray_DoubleClicked(object sender, EventArgs e)
    {
        if (worker.Enabled)
        {
            worker.Enabled = false;
            sysTray.AppendedTitleText = "Idle";
            sysTray.Icon = IdleIcon;
        }
        else
        {
            worker.Enabled = true;
            sysTray.AppendedTitleText = "Active";
            sysTray.Icon = MainIcon;
        }
    }

    private void SysTray_ExitActivated(object sender, EventArgs e)
    {
        worker.Dispose();
        sysTray.Dispose();

        ExitThread();
    }
}