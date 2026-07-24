using Project.Core.Numbers;

namespace Project.Core.Events
{
    public readonly struct GamePausedEvent : IEvent { }
    public readonly struct GameResumedEvent : IEvent { }
    
    public readonly struct GameStateChangedEvent : IEvent
    {
        public readonly GameState state;
        public GameStateChangedEvent(GameState state) 
        {
            this.state = state;
        }
    }
    
    public readonly struct GameInitEvent : IEvent { }
    public readonly struct GameOverEvent : IEvent { }

    public readonly struct GameLevelFinishedEvent : IEvent
    {
        public readonly string levelName;
        public readonly int successRobots;
        public readonly int totalRobots;
        
        public GameLevelFinishedEvent(string levelName, int successRobots, int totalRobots)
        {
            this.levelName = levelName;
            this.successRobots = successRobots;
            this.totalRobots = totalRobots;
        }
    }
}