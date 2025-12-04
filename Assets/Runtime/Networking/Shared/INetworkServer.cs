namespace Runtime.Networking.Shared
{
    using System;
    using Riptide;

    public interface INetworkServer : IDisposable
    {
        public void Start();
        public IDisposable Subscribe<T>(Action<T> callback) where T : struct;
        public void Send<T>(T message, ushort clientId, MessageSendMode sendMode) where T : struct;
        public void SendToAll<T>(T message, MessageSendMode sendMode) where T : struct;
    }
}