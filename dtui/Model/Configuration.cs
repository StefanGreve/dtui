using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Terminal.Gui;

namespace dtui
{
    [Serializable]
    public class TextColor
    {
        [JsonProperty(PropertyName = "forecolor")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Color ForeColor { get; set; }

        [JsonProperty(PropertyName = "backcolor")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Color BackColor { get; set; }
    }

    [Serializable]
    public class ColorScheme
    {
        [JsonProperty(PropertyName = "disabled")]
        public TextColor Disabled { get; init; } = new TextColor { ForeColor = Color.DarkGray, BackColor = Color.Black };

        [JsonProperty(PropertyName = "normal")]
        public TextColor Normal { get; init; } = new TextColor { ForeColor = Color.Gray, BackColor = Color.Black };

        [JsonProperty(PropertyName = "hot-normal")]
        public TextColor HotNormal { get; init; } = new TextColor { ForeColor = Color.Magenta, BackColor = Color.Black };

        [JsonProperty(PropertyName = "focus")]
        public TextColor Focus { get; init; } = new TextColor { ForeColor = Color.BrightMagenta, BackColor = Color.Gray };

        [JsonProperty(PropertyName = "hot-focus")]
        public TextColor HotFocus { get; init; } = new TextColor { ForeColor = Color.White, BackColor = Color.Black };
    }

    [Serializable]
    public class Configuration
    {
        [JsonProperty(PropertyName = "language", Required = Required.Always)]
        public string Language { get; set; } = dtui.Language.English;

        [JsonProperty(PropertyName = "colorscheme")]
        public dtui.ColorScheme ColorScheme { get; set; } = new dtui.ColorScheme();
    }
}
