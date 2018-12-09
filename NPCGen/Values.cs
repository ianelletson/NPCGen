using System;
using System.Collections.Generic;
using System.IO;
using Data;
using Newtonsoft.Json;

namespace NPCGen
{
    public partial class Values
    {
        public List<FirstName> FirstNames = new List<FirstName>();
        public List<LastName> LastNames = new List<LastName>();
        public List<Gender> Genders = new List<Gender>();
        public List<Race> Races = new List<Race>();
        public List<Age> Ages = new List<Age>();
        public List<PhysicalTrait> PhysicalTraits = new List<PhysicalTrait>();
        public List<PersonalityTrait> PersonalityTraits = new List<PersonalityTrait>();
        public List<Item> Items = new List<Item>();
        public List<Want> Wants = new List<Want>();
        public List<Secret> Secrets = new List<Secret>();
        public List<Bond> Bonds = new List<Bond>();
        public List<Hook> Hooks = new List<Hook>();
        public List<Alignment> Alignments = new List<Alignment>();
        public List<Occupation> Occupations = new List<Occupation>();

        public Values(string filePath)
        {
            ParseJson(filePath);
        }

        private void ParseJson(string filePath)
        {
            var file = File.ReadAllText(filePath);
            var valueContainers = JsonConvert.DeserializeObject<List<Values.ValueContainer>>(file);

            foreach (var vc in valueContainers)
            {
                bool SameType(string t) =>
                        string.Equals(vc.Type, t, StringComparison.InvariantCultureIgnoreCase);

                if (SameType("FirstName"))
                {
                    foreach (FirstName v in vc.Values)
                    {
                        FirstNames.Add(v);
                    }
                }
                else if (SameType("LastName"))
                {
                    foreach (LastName v in vc.Values)
                    {
                        LastNames.Add(v);
                    }
                }
                else if (SameType("Gender"))
                {
                    foreach (Gender v in vc.Values)
                    {
                        Genders.Add(v);
                    }
                }
                else if (SameType("Race"))
                {
                    foreach (Race v in vc.Values)
                    {
                        Races.Add(v);
                    }
                }
                else if (SameType("Age"))
                {
                    foreach (Age v in vc.Values)
                    {
                        Ages.Add(v);
                    }
                }
                else if (SameType("PhysicalTrait"))
                {
                    foreach (PhysicalTrait v in vc.Values)
                    {
                        PhysicalTraits.Add(v);
                    }
                }
                else if (SameType("PersonalityTrait"))
                {
                    foreach (PersonalityTrait v in vc.Values)
                    {
                        PersonalityTraits.Add(v);
                    }
                }
                else if (SameType("Item"))
                {
                    foreach (Item v in vc.Values)
                    {
                        Items.Add(v);
                    }
                }
                else if (SameType("Want"))
                {
                    foreach (Want v in vc.Values)
                    {
                        Wants.Add(v);
                    }
                }
                else if (SameType("Secret"))
                {
                    foreach (Secret v in vc.Values)
                    {
                        Secrets.Add(v);
                    }
                }
                else if (SameType("Bond"))
                {
                    foreach (Bond v in vc.Values)
                    {
                        Bonds.Add(v);
                    }
                }
                else if (SameType("Hook"))
                {
                    foreach (Hook v in vc.Values)
                    {
                        Hooks.Add(v);
                    }
                }
                else if (SameType("Alignment"))
                {
                    foreach (Alignment v in vc.Values)
                    {
                        Alignments.Add(v);
                    }
                }
                else if (SameType("Occupation"))
                {
                    foreach (Occupation v in vc.Values)
                    {
                        Occupations.Add(v);
                    }
                }
            }
        }
    }
}