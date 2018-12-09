using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Newtonsoft.Json;

namespace NPCGen
{
    public class Npc
    {
        #region Properties

        // TODO statblock
        public string Name;
        public string Age;
        public string Race;
        public string Alignment;
        public string Occupation;
        public Dictionary<string, int> StatBlock;
        public Dictionary<string, double> Items;

        public List<string> Hooks
        {
            get => _hooks;
            set => _hooks = FormatListForPronouns(value);
        }

        public List<string> Traits
        {
            get => _traits;
            set => _traits = FormatListForPronouns(value);
        }

        public List<string> Wants
        {
            get => _wants;
            set => _wants = FormatListForPronouns(value);
        }

        public List<string> Secrets
        {
            get => _secrets;
            set => _secrets = FormatListForPronouns(value);
        }

        public List<string> Bonds
        {
            get => _bonds;
            set => _bonds = FormatListForPronouns(value);
        }

        [JsonIgnore]
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (value.Pronouns != null && value.Pronouns.Count == 3)
                {
                    _pronoun = value.Pronouns[0];
                    _objective = value.Pronouns[1];
                    _possessive = value.Pronouns[2];
                }
                else
                {
                    if (value.Pronouns != null && value.Pronouns.Count == 1)
                    {
                        _pronoun = value.Pronouns[0];
                    }

                    _objective = "them";
                    _possessive = "theirs";
                }

                _gender = value;
                _genderName = value.Name;
            }
        }

        private Gender _gender;

        [JsonProperty("Gender")] private string _genderName;
        [JsonProperty("Pronoun")] private string _pronoun;
        [JsonProperty("Objective")] private string _objective;
        [JsonProperty("Possessive")] private string _possessive;

        private List<string> _traits;
        private List<string> _hooks;
        private List<string> _wants;
        private List<string> _secrets;
        private List<string> _bonds;

        private const int Tab = 2;

        private static readonly List<string> Vowels = new List<string> {"a", "e", "i", "o", "u", "h"};

        #endregion

        public Npc()
        {
            Name = string.Empty;
            Age = string.Empty;
            Race = string.Empty;
            Alignment = string.Empty;
            Traits = new List<string>();
            Wants = new List<string>();
            Secrets = new List<string>();
            Hooks = new List<string>();
            Bonds = new List<string>();
            StatBlock = new Dictionary<string, int>();
            Items = new Dictionary<string, double>();
        }

        // TODO whole thing
        public string Print(int verbosity = 0)
        {
            var sb = new StringBuilder();
            var indent = 0;
            sb.AppendLine($"{Name} is {GetArticle(Alignment)} {Alignment} {Age} {Gender.Name} {Race}");
            sb.AppendLine($"{Occupation}");

            var tc = verbosity == 0 ? 1 : -1;
            sb.Append(FormatTraits(tc, ++indent));
            --indent;

            if (verbosity == 0)
                return sb.AppendLine().ToString();

            // TODO indentation and pretty print should be much better

            sb.Append(FormatWants(-1, ++indent));
            --indent;

            sb.AppendLine("Secrets:");
            Secrets.ForEach(s => sb.AppendLine($"{Indent(++indent)} {s}"));
            --indent;

            sb.AppendLine("Hooks:");
            Hooks.ForEach(h => sb.AppendLine($"{Indent(++indent)} {h}"));
            --indent;

            sb.AppendLine("Bonds:");
            Bonds.ForEach(b => sb.AppendLine($"{Indent(++indent)} {b}"));
            --indent;

            sb.Append(FormatItems(-1, ++indent));
            --indent;

            if (verbosity == 1)
                return sb.ToString();

            sb.AppendLine("Stats:");
            sb.Append(FormatStatBlock(++indent));

            return sb.ToString();
        }

        public string GetBasicData()
        {
            var sb = new StringBuilder();
            string Fmt(string d) => IsValid(d) ? $"{d} " : string.Empty;
            sb.Append(Fmt(Alignment));
            sb.Append(Fmt(Gender.Name).ToLower());
            sb.Append($"<b>{Fmt(Race).ToLower()}</b>");
            return sb.ToString();
        }

        public Dictionary<string, string> GetAllDataFormatted()
        {
            // for each value, give header : body, formatted
            string Fmt(IEnumerable<string> ies) =>
                    string.Join("\n", ies.Select(s => s.First().ToString().ToUpper() + s.Substring(1)));

            var output = new Dictionary<string, string>();
            output.Add("Traits:", Fmt(Traits));
            output.Add("Wants:", Fmt(Wants));
            output.Add("Hooks:", Fmt(Hooks));
            output.Add("Secrets:", Fmt(Secrets));
            output.Add("Bonds:", Fmt(Bonds));
            output.Add("Stat Block:", FormatStatBlock(0));

            return output;
        }

        private string CreateTable(int verbosity = 0)
        {
            var vs = "|";
            var ts = "=";
            var hs = "-";
            var cs = "+";
            var tab = ' ';
            var longestLine = 0;

            string Indent(int c = 1) => new string(tab, c);
            void Longer(string l) => longestLine = longestLine < l.Length ? l.Length : longestLine;

            // TODO verbosity 0 just header, no table
            var header = $"{vs} {Name} {vs} {_gender.Name} {vs} {Race} {vs}";
            longestLine = header.Length;

            foreach (var t in Traits)
            {
                var line = $"{vs} {Indent()} {t} {vs}";
                Longer(line);
            }

            var sb = new StringBuilder();

            return sb.ToString();
        }

        /*
         * =======+========+=======
         * | Name | Gender | Race |
         * =======+========+=======
         * | Traits:              |
         * |    x                 |
         * |    y                 |
         * |----------------------|
         * etc
         */

        private string FormatHeader(int indent = 0)
        {
            var sb = new StringBuilder();


            return sb.ToString();
        }

        // TODO modifiers
        private string FormatStatBlock(int indent)
        {
            if (StatBlock == null || StatBlock.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            var vb = new StringBuilder();

            sb.Append($"{Indent(indent)}");
            foreach (var stat in StatBlock)
            {
                //sb.Append($"{Indent(indent)}| {stat.Key} ");
                //vb.Append($"{Indent(indent)}| {stat.Value} ");
                sb.Append($"| {stat.Key} : {stat.Value} ");
            }

            sb.Append("|");

            sb.AppendLine();
            sb.Append(vb);

            return sb.ToString();
        }

        private string FormatItems(int count, int indent)
        {
            if (Items == null || Items.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            var formatted = 0;
            if (IsValid(Secrets[0]))
            {
                sb.AppendLine("Inventory:");

                foreach (var i in Items)
                {
                    if (count != -1 && formatted >= count)
                        break;

                    if (!IsValid(i.Key))
                        continue;

                    if (formatted == 0)
                        sb.AppendLine($"{Indent(indent)}Name | Cost");
                    sb.Append(Indent(indent));
                    sb.AppendLine($"{i.Key} | {i.Value}");

                    ++formatted;
                }
            }

            return sb.ToString();
        }

        private string FormatWants(int count, int indent)
        {
            if (Wants == null || Wants.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            var formatted = 0;
            if (IsValid(Wants[0]))
            {
                var w = Gender.Singular ? "wants" : "want";
                sb.AppendLine($"{_pronoun} {w}:");

                foreach (var want in Wants)
                {
                    if (count != -1 && formatted >= count)
                        break;

                    if (!IsValid(want))
                        continue;

                    sb.Append(Indent(indent));
                    sb.AppendLine(want);

                    ++formatted;
                }
            }

            return sb.ToString();
        }

        private string FormatTraits(int count, int indent)
        {
            if (Traits == null || Traits.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();

            var formatted = 0;
            if (IsValid(Traits[0]))
            {
                var be = Gender.Singular ? "is" : "are";
                sb.AppendLine($"{_pronoun} {be}:");

                foreach (var t in Traits)
                {
                    if (count != -1 && formatted >= count)
                        break;

                    if (!IsValid(t))
                        continue;

                    sb.Append(Indent(indent));
                    sb.AppendLine($"{t}");

                    ++formatted;
                }
            }

            return sb.ToString();
        }

        private string FormatForPronoun(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;

            var fmt = s;
            if (IsValid(_pronoun))
                fmt = fmt.Replace("{pronoun}", _pronoun);
            if (IsValid(_objective))
                fmt = fmt.Replace("{objective}", _objective);
            if (IsValid(_possessive))
                fmt = fmt.Replace("{possessive}", _possessive);

            return fmt;
        }

        private List<string> FormatListForPronouns(List<string> value)
        {
            if (Gender == null || !IsValid(Gender.Name))
            {
                return value;
            }

            var fmtl = new List<string>();
            value.ForEach(t => fmtl.Add(FormatForPronoun(t)));
            return fmtl;
        }

        private static bool IsValid(string s) => !string.IsNullOrWhiteSpace(s);

        private static string GetArticle(string s)
        {
            return Vowels.Contains(s.Substring(0, 1)) ? "an" : "a";
        }

        private static string Indent(int indent)
        {
            if (indent == 0)
                return string.Empty;

            var sb = new StringBuilder();
            for (var i = 0; i < indent; ++i)
                for (var j = 0; j < Tab; ++j)
                    sb.Append(" ");

            return sb.ToString();
        }
    }
}