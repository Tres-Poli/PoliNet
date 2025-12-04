namespace Runtime.Networking
{
    using System;
    using MessagePack;
    using Shared;
    using UniRx;

    public abstract class MessageMediatorBase<T> : IMessageMediator<T> where T : struct
    {
        private ReactiveCommand<T> _messageRx = new();
        
        public Type MessageType => typeof(T);

        public IDisposable Subscribe(Action<T> callback)
        {
            return _messageRx.Subscribe(callback);
        }
        
        public void Publish(byte[] payload)
        {
            var message = MessagePackSerializer.Deserialize<T>(payload);
            _messageRx.Execute(message);
        }

        public void Dispose()
        {
            _messageRx?.Dispose();
        }
    }
}