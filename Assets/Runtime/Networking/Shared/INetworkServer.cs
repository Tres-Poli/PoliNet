namespace Runtime.Networking.Shared
{
    using System;
    using Riptide;

    public interface INetworkServer : IDisposable
    {
        public float ServerTime { get; }
        
        public void Start();
        public IDisposable Subscribe<T>(Action<MessageInfo<T>> callback) where T : struct, IMessageSerializable;
        public void Send<T>(T message, ushort clientId, MessageSendMode sendMode) where T : struct, IMessageSerializable;
        public void SendToAll<T>(T message, MessageSendMode sendMode) where T : struct, IMessageSerializable;
    }
}