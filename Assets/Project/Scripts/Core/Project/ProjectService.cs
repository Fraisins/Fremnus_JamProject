using Project.Core.Events;

namespace Project.Core
{
    public class ProjectService : IProjectService
    {
        private readonly IEventBus _eventBus;
        private GameState _state;
        
        public GameState State => _state;

        public ProjectService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void SetState(GameState state)
        {
            if (state == _state)
                return;
            
            _state = state;
            _eventBus.Publish(new GameStateChangedEvent(_state));
        }

    }
}