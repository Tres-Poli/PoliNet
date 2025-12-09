namespace Runtime.Application.Client.StateMachine.States.Initialize
{
    using Networking.Shared;
    
    public sealed class ClientInitializeStateProvider : ClientStateProviderBase<ClientInitializeState>
    {
        private readonly INetworkClient _networkClient;
        public override ClientStateType ClientStateType => ClientStateType.Initialize;
        
        public ClientInitializeStateProvider(INetworkClient networkClient)
        {
            _networkClient = networkClient;
        }
        
        public override ClientInitializeState GetState()
        {
            return new ClientInitializeState(_networkClient);
        }
    }
}