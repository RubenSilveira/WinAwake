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

[assembly: AssemblyVersion("2.23.7.16")]

class App : ApplicationContext
{
    private static readonly Icon MainIcon;
    private static readonly Icon IdleIcon;

    private readonly Worker worker;
    private readonly SysTray sysTray;

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
        worker = new Worker();

        sysTray = new SysTray("Idle", false, IdleIcon)
        {
            ActiveOnStartChecked = Settings.ActiveOnStart
        };

        sysTray.ActiveActivated += SysTray_ActiveActivated;
        sysTray.ActiveOnStartActivated += SysTray_ActiveOnStartActivated;
        sysTray.ExitActivated += SysTray_ExitActivated;

        if (sysTray.ActiveOnStartChecked)
        {
            SysTray_ActiveActivated(null, null);
        }
    }

    private void SysTray_ActiveActivated(object sender, EventArgs e)
    {
        worker.Enabled = false;

        if (sysTray.ActiveChecked)
        {
            sysTray.SetStatus("Idle", false, IdleIcon);
        }
        else
        {
            sysTray.SetStatus("Active", true, MainIcon);

            worker.Enabled = true;
        }
    }

    private void SysTray_ActiveOnStartActivated(object sender, EventArgs e)
    {
        sysTray.ActiveOnStartChecked = !sysTray.ActiveOnStartChecked;

        Settings.ActiveOnStart = sysTray.ActiveOnStartChecked;
    }

    private void SysTray_ExitActivated(object sender, EventArgs e)
    {
        worker.Dispose();
        sysTray.Dispose();

        ExitThread();
    }
}