namespace Project.Core.Game
{
    public class GameLevelChoiceHolder
    {
        public string currentLevelName = "Level1";

        private void SetLevelName(string levelName)
        {
            currentLevelName = levelName;
        }
    }
}