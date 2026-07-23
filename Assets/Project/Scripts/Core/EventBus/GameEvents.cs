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
        public readonly int successRobots;
        public GameLevelFinishedEvent(int successRobots)
        {
            this.successRobots = successRobots;
        }
    }
}