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
}