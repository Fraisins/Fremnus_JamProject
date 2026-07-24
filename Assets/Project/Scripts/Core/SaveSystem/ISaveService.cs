using Project.Core.Events;

namespace Project.Core.SaveSystem
{
    public interface ISaveService
    {
        void SaveFinishedLevel(string levelName, GameLevelFinishedEvent evt);
        bool HasSavedLevel(string levelName);
        bool TryGetLevelData(string levelName, out SaveData.LevelData levelData);
    }
}