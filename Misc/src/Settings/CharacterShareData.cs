using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
    class CharacterShareData
    {
        public int PlayerID = -1;
        public bool Sprint = false;

        public Character getCharacter()
        {
            if (PlayerID < 0)
            {
                return null;
            }
            return PlayerID == 0 ? CharacterManager.Instance.GetFirstLocalCharacter() : CharacterManager.Instance.GetSecondLocalCharacter();
        }
    }
}
