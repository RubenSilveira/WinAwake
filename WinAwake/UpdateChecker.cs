using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;

class UpdateChecker : Timer
{
    internal event EventHandler StatusChanged;

    internal UpdateChecker()
    {
        Interval = 10 * 1000;
        AutoReset = false;
        Elapsed += UpdateChecker_Elapsed;

        Enabled = true;

        UpdateAvailable = false;
    }
    internal bool UpdateAvailable { get; private set; }

    private void UpdateChecker_Elapsed(object sender, ElapsedEventArgs e)
    {
        try
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp("https://github.com/RubenSilveira/WinAwake/releases/latest");

            IWebProxy webProxy = WebRequest.DefaultWebProxy;
            webProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Proxy = webProxy;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding(response.CharacterSet)))
                    {
                        if ((new Version(Regex.Match(reader.ReadToEnd(), "WinAwake v(\\d+\\.\\d+\\.\\d+\\.\\d+)").Groups[1].Value)) > Assembly.GetExecutingAssembly().GetName().Version)
                        {
                            UpdateAvailable = true;
                            StatusChanged?.Invoke(this, new EventArgs());
                        }
                    }
                }
            }
        }
        catch
        {
            Interval =
#if DEBUG
                5
#else
                5 * 60
#endif
                * 1000;
            Enabled = true;
        }
    }
}
