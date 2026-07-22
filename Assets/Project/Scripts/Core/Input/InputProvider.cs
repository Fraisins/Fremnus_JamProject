using System;

namespace Project.Core.Input
{
    public class InputProvider : IInputProvider, IDisposable
    {
        public InputActions InputActions { get; }
        
        private InputProvider()
        {
            InputActions = new InputActions();
        }
        
        public void Dispose()
        {
            InputActions.Disable();
            InputActions.Dispose();
        }
    }
}