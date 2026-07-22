namespace Project.Core
{
    public interface IProjectService
    {
        GameState State { get; }
        public void SetState(GameState state);
    }
}