using System.ComponentModel;
using Newtonsoft.Json;

namespace Data
{
    public class Alignment : Value
    {
        [DefaultValue("Neutral")] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Ethic;

        [DefaultValue("Neutral")] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Moral;
    }
}