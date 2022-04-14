using Newtonsoft.Json;

namespace dtui
{
    public class Configuration
    {
        [JsonProperty(PropertyName = "language", Required = Required.Always)]
        public string Language { get; init; } = string.Empty;
    }
}
