namespace Runtime.Networking
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class MessageProvider : IDisposable
    {
        private Dictionary<Type, IMessageMediator> _messageSubscribers;
        
        public MessageProvider(NetworkMessageConfig networkMessageConfig)
        {
            _messageSubscribers = new Dictionary<Type, IMessageMediator>();
            foreach (var configEntry in networkMessageConfig.Entries)
            {
                var mediator = Activator.CreateInstance(configEntry.MediatorType.GetType()) as IMessageMediator;
                _messageSubscribers.Add(mediator.MessageType, mediator);
            }
        }
        
        protected void Publish(Type messageType, byte[] payload)
        {
            if (!_messageSubscribers.TryGetValue(messageType, out var mediator))
            {
                return;
            }
            
            mediator.Publish(payload);
        }
        
        public IDisposable Subscribe<T>(Action<T> callback) where T : struct
        {
            if (!_messageSubscribers.TryGetValue(typeof(T), out var mediator))
            {
                return null;
            }

            return ((IMessageMediator<T>)mediator).Subscribe(callback);
        }

        public virtual void Dispose()
        {
            if (_messageSubscribers == null || _messageSubscribers.Count == 0)
            {
                _messageSubscribers = null;
                return;
            }

            foreach (var kvp in _messageSubscribers)
            {
                if (kvp.Value == null)
                {
                    continue;
                }
                
                kvp.Value.Dispose();
            }
        }
    }
}