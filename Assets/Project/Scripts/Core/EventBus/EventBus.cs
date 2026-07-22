using System;
using System.Collections.Generic;

namespace Project.Core.Events
{
    public class EventBus : IEventBus, IDisposable
    {
        private readonly Dictionary<Type, Delegate> _subscribers = new();

        public void Subscribe<T>(Action<T> func) where T : IEvent
        {
            Type type = typeof(T);
            if (_subscribers.TryGetValue(type, out Delegate found))
                _subscribers[type] = Delegate.Combine(found, func); // add another
            else
                _subscribers[type] = func;
        }

        public void Unsubscribe<T>(Action<T> func) where T : IEvent
        {
            Type type = typeof(T);
            if (!_subscribers.TryGetValue(type, out Delegate found)) 
                return;
            
            Delegate result = Delegate.Remove(found, func);
            if (result == null)
                _subscribers.Remove(type);
            else
                _subscribers[type] = result;
        }

        public void Publish<T>(T eventData) where T : IEvent
        {
            if (_subscribers.TryGetValue(typeof(T), out Delegate del))
                (del as Action<T>)?.Invoke(eventData);
        }

        public void Dispose()
        {
            _subscribers.Clear();
        }
    }
}