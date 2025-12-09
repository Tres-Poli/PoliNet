namespace Runtime.Application.Client.StateMachine.States
{
    using StateMachine;

    public abstract class ClientStateProviderBase<T> : IStateProvider<T> where T : ClientStateBase
    {
        public int StateType => (int)ClientStateType;
        public abstract ClientStateType ClientStateType { get; }
        
        public abstract T GetState();
    }
}