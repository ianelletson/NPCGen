using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

namespace NPCGen
{
    public partial class Values
    {
        [JsonConverter(typeof(NPCGen.Values.ValueConverter))]
        public class ValueContainer
        {
            public string Type;
            public List<Value> Values;
        }
    }
}