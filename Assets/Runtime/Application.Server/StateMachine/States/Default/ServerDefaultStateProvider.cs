namespace Runtime.Application.Server.StateMachine.States.Initialize
{
    using Networking.Shared;

    public sealed class ServerDefaultStateProvider : ServerStateProviderBase<ServerDefaultState>
    {
        private readonly INetworkServer _networkServer;
        public override ServerStateType ServerStateType => ServerStateType.Initialize;
        
        public override ServerDefaultState GetState()
        {
            return new ServerDefaultState();
        }
    }
}