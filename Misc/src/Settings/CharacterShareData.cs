namespace Misc
{
    class CharacterShareData
    {
        public int playerID = -1;
        public bool sprint = false;
        public CharacterShareData(int playerID)
        {
            this.playerID = playerID;
        }

        public Character getCharacter()
        {
            if (playerID < 0)
            {
                return null;
            }
            if (playerID == 0)
            {
                return CharacterManager.Instance.GetFirstLocalCharacter();
            }
            else if (playerID == 1)
            {
                return CharacterManager.Instance.GetSecondLocalCharacter();
            }
            else
            {
                return null;
            }
        }
    }
}
