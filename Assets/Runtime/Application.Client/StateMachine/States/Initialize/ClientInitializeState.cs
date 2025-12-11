namespace Runtime.Application.Client.StateMachine.States.Initialize
{
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using Messaging.Messages.Application;
    using Networking.Shared;
    using Shared.Enums;

    public sealed class ClientInitializeState : ClientStateBase
    {
        private readonly INetworkClient _networkClient;
        private readonly IPublisher<RequestUIScreenMessage> _requestScreenPub;
        public override ClientStateType ClientStateType => ClientStateType.Initialize;

        public ClientInitializeState(INetworkClient networkClient, IPublisher<RequestUIScreenMessage> requestScreenPub)
        {
            _networkClient = networkClient;
            _requestScreenPub = requestScreenPub;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _networkClient.Start();
            _requestScreenPub.Publish(new RequestUIScreenMessage
            {
                ScreenType = UIScreenType.Loading
            });
            
            WaitForNextStateConditionsAsync().Forget();
        }

        private async UniTaskVoid WaitForNextStateConditionsAsync()
        {
            var conditions = new[]
            {
                UniTask.WaitUntil(() => _networkClient.IsConnected),
                UniTask.WaitForSeconds(5f)
            };

            await UniTask.WhenAll(conditions);

            Next(ClientStateType.Menu);
        }
    }
}