using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace dtui
{
    public class Configuration
    {
        [JsonProperty(PropertyName = "language", Required = Required.Always)]
        public string Language { get; init; } = string.Empty;
    }
}
