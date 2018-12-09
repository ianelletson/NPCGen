using System.Collections.Generic;
using Data;
using Weighted_Randomizer;

namespace NPCGen
{
    public class Randomizer
    {
        private IWeightedRandomizer<FirstName> _firstNameRandomizer;
        private IWeightedRandomizer<LastName> _lastNameRandomizer;
        private IWeightedRandomizer<Age> _ageRandomizer;
        private IWeightedRandomizer<Race> _raceRandomizer;
        private IWeightedRandomizer<Gender> _genderRandomizer;
        private IWeightedRandomizer<PhysicalTrait> _physTraitRandomizer;
        private IWeightedRandomizer<PersonalityTrait> _persTraitRandomizer;
        private IWeightedRandomizer<Want> _wantsRandomizer;
        private IWeightedRandomizer<Secret> _secretsRandomizer;
        private IWeightedRandomizer<Hook> _hooksRandomizer;
        private IWeightedRandomizer<Bond> _bondsRandomizer;
        private IWeightedRandomizer<Item> _itemRandomizer;
        private IWeightedRandomizer<Alignment> _alignmentRandomizer;
        private IWeightedRandomizer<Occupation> _occupationRandomizer;
        private const int GiveUp = 10;
        private Values _values;

        public Randomizer(Values values)
        {
            _values = values;
            Initialize();
        }

        private static bool InOrEmpty(ICollection<string> col, string val) =>
                col == null || (col.Count == 0 || col.Contains(val));

        private static T GetSatisfiedRestriction<T>(IWeightedRandomizer<T> randomizer, T next, string gender,
                                                    string race, int count = 0) where T : RestrictedValue
        {
            if (string.IsNullOrWhiteSpace(gender) && string.IsNullOrWhiteSpace(race))
                return next;

            if (count < GiveUp)
            {
                var inOrEmptyGender = InOrEmpty(next.RestrictedToGender, gender);
                var inOrEmptyRace = InOrEmpty(next.RestrictedToRace, race);
                if (!inOrEmptyGender || !inOrEmptyRace)
                {
                    next = randomizer.NextWithReplacement();
                    return GetSatisfiedRestriction(randomizer, next, gender, race, ++count);
                }
            }

            return next;
        }

        public FirstName GetNextFirstName(string gender = null, string race = null)
        {
            if (_firstNameRandomizer.TotalWeight <= 0) return new FirstName();

            var nextName = GetSatisfiedRestriction(_firstNameRandomizer,
                    _firstNameRandomizer.NextWithReplacement(), gender, race);

            return nextName;
        }

        public LastName GetNextLastName(string gender = null, string race = null)
        {
            if (_lastNameRandomizer.TotalWeight <= 0) return new LastName();
            var nextName = GetSatisfiedRestriction(_lastNameRandomizer,
                    _lastNameRandomizer.NextWithReplacement(), gender, race);

            return nextName;
        }

        public Age GetNextAge()
        {
            if (_ageRandomizer.TotalWeight <= 0) return new Age();
            return _ageRandomizer.NextWithReplacement();
        }

        public Race GetNextRace()
        {
            if (_raceRandomizer.TotalWeight <= 0) return new Race();
            return _raceRandomizer.NextWithReplacement();
        }

        public Gender GetNextGender()
        {
            if (_genderRandomizer.TotalWeight <= 0) return new Gender();
            return _genderRandomizer.NextWithReplacement();
        }

        public PhysicalTrait GetNextPhysicalTrait()
        {
            if (_physTraitRandomizer.TotalWeight <= 0) return new PhysicalTrait();
            return _physTraitRandomizer.NextWithReplacement();
        }

        public PersonalityTrait GetNextPersonalityTrait()
        {
            if (_persTraitRandomizer.TotalWeight <= 0) return new PersonalityTrait();
            return _persTraitRandomizer.NextWithReplacement();
        }

        public Want GetNextWant()
        {
            if (_wantsRandomizer.TotalWeight <= 0) return new Want();
            return _wantsRandomizer.NextWithReplacement();
        }

        public Secret GetNextSecret()
        {
            if (_secretsRandomizer.TotalWeight <= 0) return new Secret();
            return _secretsRandomizer.NextWithReplacement();
        }

        public Hook GetNextHook()
        {
            if (_hooksRandomizer.TotalWeight <= 0) return new Hook();
            return _hooksRandomizer.NextWithReplacement();
        }

        public Bond GetNextBond()
        {
            if (_bondsRandomizer.TotalWeight <= 0) return new Bond();
            return _bondsRandomizer.NextWithReplacement();
        }

        public Item GetNextItem()
        {
            if (_itemRandomizer.TotalWeight <= 0) return new Item();
            return _itemRandomizer.NextWithReplacement();
        }

        public Alignment GetNextAlignment()
        {
            if (_alignmentRandomizer.TotalWeight <= 0) return new Alignment();
            return _alignmentRandomizer.NextWithReplacement();
        }

        public Occupation GetNextOccupation()
        {
            if (_occupationRandomizer.TotalWeight <= 0) return new Occupation();
            return _occupationRandomizer.NextWithReplacement();
        }

        private void Initialize()
        {
            _firstNameRandomizer = new StaticWeightedRandomizer<FirstName>();
            _lastNameRandomizer = new StaticWeightedRandomizer<LastName>();
            _ageRandomizer = new StaticWeightedRandomizer<Age>();
            _raceRandomizer = new StaticWeightedRandomizer<Race>();
            _genderRandomizer = new StaticWeightedRandomizer<Gender>();
            _physTraitRandomizer = new StaticWeightedRandomizer<PhysicalTrait>();
            _persTraitRandomizer = new StaticWeightedRandomizer<PersonalityTrait>();
            _wantsRandomizer = new StaticWeightedRandomizer<Want>();
            _secretsRandomizer = new StaticWeightedRandomizer<Secret>();
            _hooksRandomizer = new StaticWeightedRandomizer<Hook>();
            _bondsRandomizer = new StaticWeightedRandomizer<Bond>();
            _itemRandomizer = new StaticWeightedRandomizer<Item>();
            _alignmentRandomizer = new StaticWeightedRandomizer<Alignment>();
            _occupationRandomizer = new StaticWeightedRandomizer<Occupation>();

            foreach (var fn in _values.FirstNames)
            {
                _firstNameRandomizer.Add(fn, fn.Weight);
            }

            foreach (var ln in _values.LastNames)
            {
                _lastNameRandomizer.Add(ln, ln.Weight);
            }

            foreach (var a in _values.Ages)
            {
                _ageRandomizer.Add(a, a.Weight);
            }

            foreach (var r in _values.Races)
            {
                _raceRandomizer.Add(r, r.Weight);
            }

            foreach (var g in _values.Genders)
            {
                _genderRandomizer.Add(g, g.Weight);
            }

            foreach (var phys in _values.PhysicalTraits)
            {
                _physTraitRandomizer.Add(phys, phys.Weight);
            }

            foreach (var pers in _values.PersonalityTraits)
            {
                _persTraitRandomizer.Add(pers, pers.Weight);
            }

            foreach (var w in _values.Wants)
            {
                _wantsRandomizer.Add(w, w.Weight);
            }

            foreach (var s in _values.Secrets)
            {
                _secretsRandomizer.Add(s, s.Weight);
            }

            foreach (var h in _values.Hooks)
            {
                _hooksRandomizer.Add(h, h.Weight);
            }

            foreach (var b in _values.Bonds)
            {
                _bondsRandomizer.Add(b, b.Weight);
            }

            foreach (var i in _values.Items)
            {
                _itemRandomizer.Add(i, i.Weight);
            }

            foreach (var a in _values.Alignments)
            {
                _alignmentRandomizer.Add(a, a.Weight);
            }

            foreach (var o in _values.Occupations)
            {
                _occupationRandomizer.Add(o, o.Weight);
            }
        }
    }
}