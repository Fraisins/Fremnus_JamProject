namespace Project.Core.Game
{
    public interface IGameService
    {
        bool HasNextLevel();
        void ToNextLevel();
        void RestartLevel();
    }
}