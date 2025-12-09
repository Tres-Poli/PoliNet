namespace Runtime.Application.Server.StateMachine.States.Initialize
{
    using Networking.Shared;

    public sealed class ServerInitializeState : ServerStateBase
    {
        private readonly INetworkServer _networkServer;
        public override ServerStateType ServerStateType => ServerStateType.Initialize;

        public ServerInitializeState(INetworkServer networkServer)
        {
            _networkServer = networkServer;
        }
    }
}