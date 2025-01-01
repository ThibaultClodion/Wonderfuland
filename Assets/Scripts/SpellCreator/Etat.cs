using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellCreator
{
    class Etat
    {
        public enum SpecialEffectsEnum
        {
            INVISIBILITY,
            INVULNERABLE,
            LINK
        };

        public int maxCount;
        public List<Effect> effects; // Condition sur les effets suffisant?
    }
}
