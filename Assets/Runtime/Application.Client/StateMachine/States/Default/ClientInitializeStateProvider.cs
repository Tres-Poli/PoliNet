namespace Runtime.Application.Client.StateMachine.States.Initialize
{
    public sealed class ClientDefaultStateProvider : ClientStateProviderBase<ClientDefaultState>
    {
        public override ClientStateType ClientStateType => ClientStateType.Default;
        
        public override ClientDefaultState GetState()
        {
            return new ClientDefaultState();
        }
    }
}