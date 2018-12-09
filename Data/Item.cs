using System.ComponentModel;
using Newtonsoft.Json;

namespace Data
{
    public class Item : Value
    {
        [DefaultValue(1)] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double Worth;
    }
}