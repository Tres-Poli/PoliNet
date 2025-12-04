namespace Runtime.Networking
{
    using Riptide;
    using Shared;
    using Shared.Messages;
    using UnityEngine;

    public sealed class ClientNetworkTime
    {
        private readonly INetworkClient _client;
        
        private float _feedbackRequestTime;
        private float _roundTripTime;

        private float _syncTime;
        private float _localTimeTimestamp;
        
        public float GameTime => _syncTime + (Time.unscaledTime - _localTimeTimestamp);
        
        public ClientNetworkTime(INetworkClient client)
        {
            _client = client;
        }
        
        public void SyncTime()
        {
            _client.Subscribe<SyncTimeResponseMessage>(SyncTimeResponse_Callback);
        }

        private void RequestTimeFeedback()
        {
            _feedbackRequestTime = Time.unscaledTime;
            _client.Send(new SyncTimeRequestMessage(), MessageSendMode.Reliable);
        }

        private void SyncTimeResponse_Callback(SyncTimeResponseMessage message)
        {
            var rtt = (Time.unscaledTime - _feedbackRequestTime) - message.ServerSideDelay;
            _localTimeTimestamp = Time.unscaledTime;
            _syncTime = message.ServerTime + rtt / 2;
        }
    }
}