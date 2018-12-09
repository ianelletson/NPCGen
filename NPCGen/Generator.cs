using System;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace NPCGen
{
    internal class Generator
    {
        public readonly List<Npc> GeneratedNpcs;

        private readonly Randomizer _randomizer;
        private Npc _ip;
        private readonly Random _rng;

        public Generator(Randomizer randomizer)
        {
            _randomizer = randomizer;
            GeneratedNpcs = new List<Npc>();
            _rng = new Random();
        }

        public Npc CreateNpc()
        {
            // TODO allow spec

            _ip = new Npc
            {
                    Gender = GetGender(),
                    Race = GetRace(),
                    Alignment = GetAlignment()
            };

            _ip.Name = GetName();
            _ip.Age = GetAge();
            _ip.Traits = GetTraits(2);
            _ip.Wants = GetWants(2);
            _ip.Secrets = GetSecrets();
            _ip.Hooks = GetHooks();
            _ip.Bonds = GetBonds();
            _ip.StatBlock = GetStatBlock();
            _ip.Items = GetItems();
            _ip.Occupation = GetOccupation();

            GeneratedNpcs.Add(_ip);

            return _ip;
        }

        private string GetName()
        {
            return
                    $"{_randomizer.GetNextFirstName(_ip.Gender.Name, _ip.Race).Name} {_randomizer.GetNextLastName(_ip.Gender.Name, _ip.Race).Name}";
        }

        private string GetAge()
        {
            return _randomizer.GetNextAge().Name;
        }

        private string GetRace()
        {
            return _randomizer.GetNextRace().Name;
        }

        private Gender GetGender()
        {
            return _randomizer.GetNextGender();
        }

        private string GetAlignment()
        {
            return _randomizer.GetNextAlignment().Name;
        }

        private string GetOccupation()
        {
            return _randomizer.GetNextOccupation().Name;
        }

        private List<string> GetTraits(int count = 1)
        {
            var traits = new List<string>
            {
                    _randomizer.GetNextPhysicalTrait().Name
            };

            PersonalityTrait Gnpt() => _randomizer.GetNextPersonalityTrait();
            traits.AddRange(GenerateNoDuplicates(Gnpt, count));

            return traits;
        }


        private List<string> GetWants(int count = 1)
        {
            Want Gnw() => _randomizer.GetNextWant();
            return GenerateNoDuplicates(Gnw, count);
        }

        private List<string> GetSecrets(int count = 1)
        {
            Secret Gns() => _randomizer.GetNextSecret();
            return GenerateNoDuplicates(Gns, count);
        }

        private List<string> GetHooks(int count = 1)
        {
            Hook Gnh() => _randomizer.GetNextHook();
            return GenerateNoDuplicates(Gnh, count);
        }

        private List<string> GetBonds(int count = 1)
        {
            Bond Gnb() => _randomizer.GetNextBond();
            return GenerateNoDuplicates(Gnb, count);
        }

        private Dictionary<string, double> GetItems(int count = 1)
        {
            var d = new Dictionary<string, double>();
            for (var c = 0; c < count; ++c)
            {
                var i = _randomizer.GetNextItem();
                if (d.ContainsKey(i.Name))
                {
                    d[i.Name] += i.Worth;
                }
                else
                {
                    d.Add(i.Name, i.Worth);
                }
            }

            return d;
        }

        // TODO various power levels, arrays, racial traits, etc
        private Dictionary<string, int> GetStatBlock()
        {
            // TODO make stats readable from file
            var block = new Dictionary<string, int>();

            block.Add("strength", Roll(4, 1));
            block.Add("dexterity", Roll(4, 1));
            block.Add("constitution", Roll(4, 1));
            block.Add("intelligence", Roll(4, 1));
            block.Add("wisdom", Roll(4, 1));
            block.Add("charisma", Roll(4, 1));

            return block;
        }

        private static List<string> GenerateNoDuplicates<T>(Func<T> generator, int count, int giveUp = 4)
                where T : Value
        {
            var vals = new List<string>();
            for (var i = 0; i < count; ++i)
            {
                string t;
                var attempts = 0;
                do
                {
                    t = generator().Name;
                    if (attempts > giveUp)
                        break;
                    ++attempts;
                } while (vals.Contains(t));

                vals.Add(t);
            }

            return vals;
        }

        private int Roll(int count, int drop = 0, int sides = 6)
        {
            var rolls = new List<int>();
            for (var i = 0; i < count; ++i)
            {
                rolls.Add(_rng.Next(1, sides + 1));
            }

            if (drop <= 0)
                return rolls.Sum();

            rolls.Sort();
            for (var i = 0; i < drop; ++i)
            {
                rolls.RemoveAt(0);
            }

            return rolls.Sum();
        }
    }
}