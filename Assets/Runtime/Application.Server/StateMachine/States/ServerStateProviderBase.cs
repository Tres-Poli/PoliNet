namespace Runtime.Application.Server.StateMachine.States
{
    public abstract class ServerStateProviderBase<T> : IStateProvider<T> where T : ServerStateBase
    {
        public int StateType => (int)ServerStateType;
        public abstract ServerStateType ServerStateType { get; }
        
        public abstract T GetState();
    }
}