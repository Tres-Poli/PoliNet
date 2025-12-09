namespace Runtime.Networking
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Riptide;
    using Shared;
    using Shared.Messages;
    using UnityEngine;

    public sealed class RiptideClient : INetworkClient
    {
        private readonly MessageProvider _messageProvider;
        private readonly MessageTypeProvider _messageTypeProvider;
        private readonly NetworkConfig _networkConfig; 
        private readonly Client _client;

        private CancellationTokenSource _cts;

        private IDisposable _syncTimeSubscription;
        private float _syncTimestamp;
        private float _serverTime = -1f;
        
        public bool IsConnected => _client.IsConnected;

        public float ServerTime => _serverTime > 0
            ? _serverTime + (Time.unscaledTime - _syncTimestamp)
            : -1f;

        public RiptideClient(MessageProvider messageProvider, MessageTypeProvider messageTypeProvider, NetworkConfig networkConfig)
        {
            _messageProvider = messageProvider;
            _messageTypeProvider = messageTypeProvider;
            _networkConfig = networkConfig;
            
            _client = new Client();
        }
        
        public void Dispose()
        {
            _client.MessageReceived -= MessageReceived_Callback;
            _client.Connected -= LocalClientConnected_Callback;
            
            _client.Disconnect();
            
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            
            _messageProvider.Dispose();
            _syncTimeSubscription?.Dispose();
        }

        public void Start()
        {
            _client.MessageReceived += MessageReceived_Callback;
            _client.Connected += LocalClientConnected_Callback;
            _client.Connect($"{_networkConfig.address}:{_networkConfig.port}", 
                useMessageHandlers: false);
            
            _cts = new CancellationTokenSource();
            DoUpdateAsync(_cts.Token).Forget();
        }

#region MESSAGE_MANAGING

        public void Send<T>(T message, MessageSendMode sendMode) where T : struct, IMessageSerializable
        {
            var messageId = _messageTypeProvider.GetMessageId<T>();
            var messageToSend = Message.Create(sendMode, messageId);
            messageToSend.AddSerializable(message);
            _client.Send(messageToSend);
        }

        public IDisposable Subscribe<T>(Action<MessageInfo<T>> callback) where T : struct, IMessageSerializable
        {
            return _messageProvider.Subscribe(callback);
        }
        
        private void MessageReceived_Callback(object sender, MessageReceivedEventArgs args)
        {
            var message = args.Message;
            Debug.Log($"Client received message: {args.MessageId}");
            var messageType = _messageTypeProvider.GetMessageType(args.MessageId);
            _messageProvider.Publish(messageType, message, args.FromConnection.Id);
            message.Release();
        }
        
#endregion

        private async UniTaskVoid DoUpdateAsync(CancellationToken ct)
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            while (!ct.IsCancellationRequested)
            {
                _client.Update();
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private void LocalClientConnected_Callback(object sender, EventArgs args)
        {
            SyncTime().Forget();
        }

        private async UniTaskVoid SyncTime()
        {
            await UniTask.WaitUntil(() => _client.SmoothRTT > -1);
            _syncTimeSubscription = Subscribe<GetTimeResponseMessage>(GetServerTimeResponse_Callback);
            Send(new GetTimeRequestMessage(), MessageSendMode.Unreliable);
        }

        private void GetServerTimeResponse_Callback(MessageInfo<GetTimeResponseMessage> info)
        {
            _syncTimestamp = Time.unscaledTime - _client.SmoothRTT / 2000f;
            _serverTime = info.Message.ServerTime;
        }
    }
}