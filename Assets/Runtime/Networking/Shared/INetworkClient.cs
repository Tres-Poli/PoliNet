namespace Runtime.Networking.Shared
{
    using System;
    using Riptide;

    public interface INetworkClient : IDisposable
    {
        public bool IsConnected { get; }
        public float ServerTime { get; }
        
        public void Start();
        public IDisposable Subscribe<T>(Action<MessageInfo<T>> callback) where T : struct, IMessageSerializable;
        public void Send<T>(T message, MessageSendMode sendMode) where T : struct, IMessageSerializable;
    }
}