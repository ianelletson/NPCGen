using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Data
{
    public class Gender : Value
    {
        [DefaultValue(default(List<string>))] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public List<string> Pronouns;

        [DefaultValue(false)] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Singular;
    }
}