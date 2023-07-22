using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

static class UpdateChecker
{
    internal static bool Check()
    {
        for (int i = 0; i < 5; i++)
        {
            try
            {
                Thread.Sleep((i * 3 + 1) * 1000);

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
                            return (new Version(Regex.Match(reader.ReadToEnd(), "WinAwake v(\\d+\\.\\d+\\.\\d+\\.\\d+)").Groups[1].Value)) > Assembly.GetExecutingAssembly().GetName().Version;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        return false;
    }
}
