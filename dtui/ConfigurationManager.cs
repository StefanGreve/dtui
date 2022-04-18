using System.Globalization;
using System.Text;

using Newtonsoft.Json;

namespace dtui
{
    internal static class ConfigurationManager
    {
        public static string Folder
        {
            get
            {
                string defaultPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "dtui");

                return Environment.OSVersion.Platform switch
                {
                    PlatformID.Win32NT => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dtui"),
                    PlatformID.Unix => defaultPath, // this field also applies to MacOSX operating systems
                    _ => defaultPath
                };
            }
        }

        public static string Path => System.IO.Path.Combine(Folder, "settings.json");

        private static readonly JsonSerializerSettings JsonSettings = new() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };

        public static void Init()
        {
            Directory.CreateDirectory(Folder);

            if (!File.Exists(Path))
            {
                string data = JsonConvert.SerializeObject(new Configuration(), JsonSettings);
                File.WriteAllText(Path, data);
            }
        }

        public static Configuration Configuration => JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Path), JsonSettings)!;

        public static void ChangeCulture(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.OutputEncoding = language switch
            {
                Language.Japanese => Encoding.GetEncoding(932),
                Language.Chinese => Encoding.GetEncoding(936),
                Language.Spanish => Encoding.GetEncoding(1252),
                _ => Encoding.Default
            };
        }
    }
}
