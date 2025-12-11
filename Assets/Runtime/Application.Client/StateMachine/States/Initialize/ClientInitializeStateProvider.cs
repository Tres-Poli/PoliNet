namespace Runtime.Application.Client.StateMachine.States.Initialize
{
    using MessagePipe;
    using Messaging.Messages.Application;
    using Networking.Shared;
    using VContainer;

    public sealed class ClientInitializeStateProvider : ClientStateProviderBase<ClientInitializeState>
    {
        private readonly INetworkClient _networkClient;
        private readonly IPublisher<RequestUIScreenMessage> _requestScreenPub;
        public override ClientStateType ClientStateType => ClientStateType.Initialize;
        
        [Inject]
        public ClientInitializeStateProvider(INetworkClient networkClient, IPublisher<RequestUIScreenMessage> requestScreenPub)
        {
            _networkClient = networkClient;
            _requestScreenPub = requestScreenPub;
        }
        
        public override ClientInitializeState GetState()
        {
            return new ClientInitializeState(_networkClient, _requestScreenPub);
        }
    }
}