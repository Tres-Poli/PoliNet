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

    public sealed class RiptideClient : MessageProvider, INetworkClient
    {
        private readonly NetworkConfig _config;
        private readonly NetworkMessageConfig _networkMessageConfig;
        private readonly Client _client;

        private CancellationTokenSource _cts;
        private Byte[] _inMessageCache;

        private Dictionary<Type, RuntimeNetworkMessageConfigEntry> _messageMapByType;

        public RiptideClient(NetworkConfig config, NetworkMessageConfig  networkMessageConfig) : base(networkMessageConfig)
        {
            _config = config;
            _networkMessageConfig = networkMessageConfig;
            
            _client = new Client();
            _inMessageCache = new Byte[1024];
            _messageMapByType = _networkMessageConfig.MapByType();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            _client.MessageReceived -= MessageReceived_Callback; 
            _client.Disconnect();
            
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        public void Start()
        {
            _client.Connect($"{_config.address}:{_config.port}", 
                useMessageHandlers: false);
            
            _client.MessageReceived += MessageReceived_Callback;
            
            _cts = new CancellationTokenSource();
            DoUpdateAsync(_cts.Token).Forget();
        }

        public void Send<T>(T message, MessageSendMode sendMode) where T : struct
        {
            var messageToSend = Message.Create(sendMode);
            if (!_messageMapByType.TryGetValue(typeof(T), out var messageInfo))
            {
                return;
            }

            messageToSend.AddUShort(messageInfo.Key);
            var payload = MessagePackSerializer.Serialize(message);
            messageToSend.AddBytes(payload);
            
            _client.Send(messageToSend);
        }

        private async UniTaskVoid DoUpdateAsync(CancellationToken ct)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            while (!ct.IsCancellationRequested)
            {
                _client.Update();
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