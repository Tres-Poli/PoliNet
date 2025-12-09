namespace Runtime.Application.Server.StateMachine.States.Initialize
{
    using Networking.Shared;

    public sealed class ServerInitializeStateProvider : ServerStateProviderBase<ServerInitializeState>
    {
        private readonly INetworkServer _networkServer;
        public override ServerStateType ServerStateType => ServerStateType.Initialize;

        public ServerInitializeStateProvider(INetworkServer networkServer)
        {
            _networkServer = networkServer;
        }
        
        public override ServerInitializeState GetState()
        {
            return new ServerInitializeState(_networkServer);
        }
    }
}