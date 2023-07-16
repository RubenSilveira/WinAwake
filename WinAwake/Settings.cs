using Microsoft.Win32;

static class Settings
{
    internal static bool ActiveOnStart
    {
        get
        {
            object o = Registry.GetValue("HKEY_CURRENT_USER\\Software\\WinAwake", "ActiveOnStart", 0);
            
            if (null != o)
            {
                return (0 != (int)o);
            }

            return false;
        }
        set
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\WinAwake", "ActiveOnStart", value ? 1 : 0, RegistryValueKind.DWord);
        }
    }
}
