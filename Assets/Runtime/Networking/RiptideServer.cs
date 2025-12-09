namespace Runtime.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Riptide;
    using Shared;
    using Shared.Messages;
    using UnityEngine;

    public sealed class RiptideServer : INetworkServer
    {
        private readonly NetworkConfig _config;
        private readonly MessageTypeProvider _messageTypeProvider;
        private readonly MessageProvider _messageProvider;
        private readonly Server _server;

        private CancellationTokenSource _cts;
        private IDisposable _timeSyncSub;
        
        private Dictionary<Type, RuntimeNetworkMessageConfigEntry> _messageMapByType;

        public float ServerTime => Time.unscaledTime;

        public RiptideServer(MessageProvider messageProvider, MessageTypeProvider messageTypeProvider, NetworkConfig config)
        {
            _config = config;
            _messageProvider = messageProvider;
            _messageTypeProvider = messageTypeProvider;
            
            _server = new Server();
        }

        public void Start()
        {
            _server.MessageReceived += MessageReceived_Callback;
            _server.Start(_config.port, _config.maxClients,
                useMessageHandlers: false);

            _timeSyncSub = Subscribe<GetTimeRequestMessage>(GetTimeRequestMessage_Callback);
            
            _cts = new CancellationTokenSource();
            DoUpdateAsync(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _server.Stop();
            _server.MessageReceived -= MessageReceived_Callback;
            
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;

            _messageProvider.Dispose();
            _timeSyncSub.Dispose();
        }
        
#region MESSAGE_MANAGING
        
        public void Send<T>(T message, ushort clientId, MessageSendMode sendMode) where T : struct, IMessageSerializable
        {
            var messageId = _messageTypeProvider.GetMessageId<T>();
            var messageToSend = Message.Create(sendMode, messageId);
            messageToSend.AddSerializable(message);
            _server.Send(messageToSend, clientId);
        }
        
        public void SendToAll<T>(T message, MessageSendMode sendMode) where T : struct, IMessageSerializable
        {
            var messageId = _messageTypeProvider.GetMessageId<T>();
            var messageToSend = Message.Create(sendMode, messageId);
            messageToSend.AddSerializable(message);
            _server.SendToAll(messageToSend);
        }

        public IDisposable Subscribe<T>(Action<MessageInfo<T>> callback) where T : struct, IMessageSerializable
        {
            return _messageProvider.Subscribe(callback);
        }
        
        private void MessageReceived_Callback(object sender, MessageReceivedEventArgs args)
        {
            var message = args.Message;
            Debug.Log($"Server received message: {args.MessageId}");
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
                _server.Update();
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private void GetTimeRequestMessage_Callback(MessageInfo<GetTimeRequestMessage> messageInfo)
        {
            var senderId = messageInfo.SenderId;
            var responseMessage = new GetTimeResponseMessage
            {
                ServerTime = Time.unscaledTime
            };
            
            Send(responseMessage, senderId, MessageSendMode.Unreliable);
        }
    }
}