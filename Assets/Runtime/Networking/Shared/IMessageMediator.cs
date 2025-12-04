namespace Runtime.Networking.Shared
{
    using System;

    public interface IMessageMediator : IDisposable
    {
        public Type MessageType { get; }
        public void Publish(byte[] payload);
    }

    public interface IMessageMediator<T> : IMessageMediator where T : struct
    {
        public IDisposable Subscribe(Action<T> callback);
    }
}