using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: AssemblyTitle("WinAwake")]
[assembly: AssemblyDescription("WinAwake")]
[assembly: AssemblyCompany("WinAwake")]
[assembly: AssemblyCopyright("Ruben Silveira")]
[assembly: AssemblyProduct("WinAwake")]
[assembly: Guid("FE3D24DB-5C73-4AC0-AB1F-C4E76A45D5C3")]

[assembly: AssemblyVersion("3.23.7.21")]

class App : ApplicationContext
{
    private static readonly Icon MainIcon;
    private static readonly Icon IdleIcon;

    private readonly Worker worker;
    private readonly SysTray sysTray;

    private bool activeStatus;
    private bool isInSession;

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
        // TODO: Can this even be determined on start?
        isInSession = true;

        worker = new Worker();
        sysTray = new SysTray(Settings.ActiveOnStart);

        sysTray.ActiveActivated += SysTray_ActiveActivated;
        sysTray.ActiveOnStartActivated += SysTray_ActiveOnStartActivated;
        sysTray.UpdateActivated += SysTray_UpdateActivated;
        sysTray.AboutActivated += SysTray_AboutActivated;
        sysTray.ExitActivated += SysTray_ExitActivated;

        SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        ActiveStatus = sysTray.ActiveOnStartChecked;
    }

    private bool ActiveStatus
    {
        set
        {
            Debug.WriteLine("Setting status to " + value);

            bool isOnBatteryOnly = PowerLineStatus.Offline == SystemInformation.PowerStatus.PowerLineStatus;

            activeStatus = value;
            worker.Enabled = activeStatus && isInSession && !isOnBatteryOnly;

            if (!activeStatus)
            {
                sysTray.SetStatus("Idle", false, IdleIcon);
            }
            else if (isInSession && !isOnBatteryOnly)
            {
                sysTray.SetStatus("Active", true, MainIcon);
            }
            else
            {
                sysTray.SetStatus("Paused", false, IdleIcon);

                Debug.WriteLine("Setting status to Paused instead");
            }
        }
    }

    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        Debug.WriteLine(e.Reason + " happening");

        isInSession = (SessionSwitchReason.SessionUnlock == e.Reason
            || SessionSwitchReason.RemoteConnect == e.Reason
            || SessionSwitchReason.ConsoleConnect == e.Reason);

        ActiveStatus = activeStatus;
    }

    private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
        Debug.WriteLine(e.Mode + " happening");

        if (PowerModes.Suspend == e.Mode)
        {
            isInSession = false;
        }
        else if (PowerModes.Resume == e.Mode)
        {
            isInSession = true;
        }

        ActiveStatus = activeStatus;
    }

    private void SysTray_ActiveActivated(object sender, EventArgs e)
    {
        ActiveStatus = !activeStatus;
    }

    private void SysTray_ActiveOnStartActivated(object sender, EventArgs e)
    {
        sysTray.ActiveOnStartChecked = !sysTray.ActiveOnStartChecked;

        Settings.ActiveOnStart = sysTray.ActiveOnStartChecked;
    }

    private async void SysTray_UpdateActivated(object sender, EventArgs e)
    {
        if (sysTray.UpdateText == "!!! Download update !!!")
        {
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/RubenSilveira/WinAwake/releases/latest", UseShellExecute = true });
        }
        else
        {
            sysTray.UpdateEnabled = false;
            sysTray.UpdateText = "Checking for update";

            if (await Task<bool>.Run(() => { return UpdateChecker.Check(); }))
            {
                sysTray.UpdateText = "!!! Download update !!!";
                sysTray.UpdateEnabled = true;
            }
            else
            {
                sysTray.UpdateText = "No available update";
            }
        }
    }

    private void SysTray_AboutActivated(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo { FileName = "https://github.com/RubenSilveira/WinAwake", UseShellExecute = true });
    }

    private void SysTray_ExitActivated(object sender, EventArgs e)
    {
        SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;

        worker.Dispose();
        sysTray.Dispose();

        ExitThread();
    }
}