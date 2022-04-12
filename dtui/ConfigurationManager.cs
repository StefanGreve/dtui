using System.Globalization;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dtui
{
    internal class ConfigurationManager
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

        public ConfigurationManager()
        {
            Directory.CreateDirectory(Folder);

            if (!File.Exists(Path))
            {
                var config = new Configuration
                {
                    Language = Language.English
                };

                string data = JsonConvert.SerializeObject(config, JsonSettings);

                File.WriteAllText(Path, data);
            }
        }

#pragma warning disable CS8603 // Possible null reference return.
        public static Configuration Configuration => JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Path), JsonSettings);
#pragma warning restore CS8603 // Possible null reference return.

        public static void ChangeCulture()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Configuration.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Configuration.Language);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.OutputEncoding = Configuration.Language switch
            {
                Language.Japanese => Encoding.GetEncoding(932),
                _ => Encoding.Default
            };
        }
    }
}
