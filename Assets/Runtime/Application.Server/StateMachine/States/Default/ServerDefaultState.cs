namespace Runtime.Application.Server.StateMachine.States.Initialize
{
    using Networking.Shared;

    public sealed class ServerDefaultState : ServerStateBase
    {
        private readonly INetworkServer _networkServer;
        public override ServerStateType ServerStateType => ServerStateType.Default;
    }
}