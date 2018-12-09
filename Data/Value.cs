using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Data
{
    public abstract class Value : IComparable<Value>
    {
        public string Name;

        [DefaultValue(50)] [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Weight;

        public int CompareTo(Value other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }
}