using System.Collections.Generic;

namespace Data
{
    public abstract class RestrictedValue : Value
    {
        public List<string> RestrictedToGender;
        public List<string> RestrictedToRace;
        public List<string> RestrictedToAlignment;
    }
}