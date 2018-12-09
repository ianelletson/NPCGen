using System;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NPCGen
{
    public partial class Values
    {
        public class ValueConverter : JsonConverter
        {
            // Source: https://stackoverflow.com/questions/22537233/json-net-how-to-deserialize-interface-property-based-on-parent-holder-object/22539730#22539730

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(Values.ValueContainer));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                            JsonSerializer serializer)
            {
                var jo = JObject.Load(reader);
                var container = new Values.ValueContainer
                {
                        Type = (string) jo["type"],
                        Values = new List<Value>()
                };

                bool SameType(string t) =>
                        string.Equals(container.Type, t, StringComparison.InvariantCultureIgnoreCase);

                foreach (var obj in jo["values"])
                {
                    void Add<T>() where T : Value => container.Values.Add(obj.ToObject<T>(serializer));
                    if (SameType("FirstName"))
                    {
                        Add<FirstName>();
                    }
                    else if (SameType("LastName"))
                    {
                        Add<LastName>();
                    }
                    else if (SameType("Gender"))
                    {
                        Add<Gender>();
                    }
                    else if (SameType("Item"))
                    {
                        Add<Item>();
                    }
                    else if (SameType("Race"))
                    {
                        Add<Race>();
                    }
                    else if (SameType("Age"))
                    {
                        Add<Age>();
                    }
                    else if (SameType("PhysicalTrait"))
                    {
                        Add<PhysicalTrait>();
                    }
                    else if (SameType("PersonalityTrait"))
                    {
                        Add<PersonalityTrait>();
                    }
                    else if (SameType("Want"))
                    {
                        Add<Want>();
                    }
                    else if (SameType("Secret"))
                    {
                        Add<Secret>();
                    }
                    else if (SameType("Bond"))
                    {
                        Add<Bond>();
                    }
                    else if (SameType("Hook"))
                    {
                        Add<Hook>();
                    }
                    else if (SameType("Alignment"))
                    {
                        Add<Alignment>();
                    }
                    else if (SameType("Occupation"))
                    {
                        Add<Occupation>();
                    }
                }

                return container;
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}