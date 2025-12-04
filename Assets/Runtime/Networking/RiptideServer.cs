namespace Runtime.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using MessagePack;
    using Riptide;
    using Shared;

    public sealed class RiptideServer : MessageProvider, INetworkServer
    {
        private readonly NetworkConfig _config;
        private readonly NetworkMessageConfig _networkMessageConfig;
        private readonly Server _server;

        private CancellationTokenSource _cts;
        private Byte[] _inMessageCache;
        
        private Dictionary<Type, RuntimeNetworkMessageConfigEntry> _messageMapByType;

        public RiptideServer(NetworkConfig config, NetworkMessageConfig networkMessageConfig) : base(networkMessageConfig)
        {
            _config = config;
            _networkMessageConfig = networkMessageConfig;

            _server = new Server();
            _inMessageCache = new Byte[1024];
            _messageMapByType = _networkMessageConfig.MapByType();
        }
        
        public void Start()
        {
            _server.MessageReceived += MessageReceived_Callback;
            _server.Start(_config.port, _config.maxClients,
                useMessageHandlers: false);
            

            _cts = new CancellationTokenSource();
            DoUpdateAsync(_cts.Token).Forget();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _server.Stop();
            _server.MessageReceived -= MessageReceived_Callback;
            
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
        
        public void Send<T>(T message, ushort clientId, MessageSendMode sendMode) where T : struct
        {
            var messageToSend = Message.Create(sendMode);
            if (!_messageMapByType.TryGetValue(typeof(T), out var messageInfo))
            {
                return;
            }

            messageToSend.AddUShort(messageInfo.Key);
            var payload = MessagePackSerializer.Serialize(message);
            messageToSend.AddBytes(payload);
            
            _server.Send(messageToSend, clientId);
        }
        
        public void SendToAll<T>(T message, MessageSendMode sendMode) where T : struct
        {
            var messageToSend = Message.Create(sendMode);
            if (!_messageMapByType.TryGetValue(typeof(T), out var messageInfo))
            {
                return;
            }

            messageToSend.AddUShort(messageInfo.Key);
            var payload = MessagePackSerializer.Serialize(message);
            messageToSend.AddBytes(payload);
            
            _server.SendToAll(messageToSend);
        }
        
        private async UniTaskVoid DoUpdateAsync(CancellationToken ct)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            while (!ct.IsCancellationRequested)
            {
                _server.Update();
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }
        
        private void MessageReceived_Callback(object sender, MessageReceivedEventArgs args)
        {
            var message = args.Message;
            var payloadSize = message.BytesInUse - _networkMessageConfig.MessageTypeBytes;
            
            message.GetBytes(_networkMessageConfig.MessageTypeBytes, _inMessageCache);

            var messageId = BitConverter.ToUInt16(_inMessageCache);
            var messageInfo = _networkMessageConfig.Entries.FirstOrDefault(x => x.Key == messageId);
            
            message.GetBytes(payloadSize, _inMessageCache, _networkMessageConfig.MessageTypeBytes);
            var messageType = messageInfo.Type.GetType();
            
            Publish(messageType, _inMessageCache);
        }
    }
}