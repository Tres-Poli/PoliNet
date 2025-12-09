namespace Runtime.Networking.Shared
{
    using System;
    using Riptide;

    public interface IMessageMediator : IDisposable
    {
        public Type MessageType { get; }
        public void Publish(Message message, ushort senderId);
    }

    public interface IMessageMediator<T> : IMessageMediator where T : IMessageSerializable
    {
        public IDisposable Subscribe(Action<MessageInfo<T>> callback);
    }
}