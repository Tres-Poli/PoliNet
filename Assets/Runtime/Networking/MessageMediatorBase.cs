namespace Runtime.Networking
{
    using System;
    using Riptide;
    using Shared;
    using UniRx;

    public abstract class MessageMediatorBase<T> : IMessageMediator<T> where T : IMessageSerializable, new()
    {
        private ReactiveCommand<MessageInfo<T>> _messageRx = new();
        
        public Type MessageType => typeof(T);

        public IDisposable Subscribe(Action<MessageInfo<T>> callback)
        {
            return _messageRx.Subscribe(callback);
        }
        
        public void Publish(Message message, ushort senderId)
        {
            _messageRx.Execute(new MessageInfo<T>
            {
                Message = message.GetSerializable<T>(),
                SenderId = senderId
            });
        }

        public void Dispose()
        {
            _messageRx?.Dispose();
        }
    }
}