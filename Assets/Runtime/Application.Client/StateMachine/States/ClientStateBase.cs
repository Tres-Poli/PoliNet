namespace Runtime.Application.Client.StateMachine.States
{
    using StateMachine;
    using UniRx;

    public abstract class ClientStateBase : IState
    {
        private readonly ReactiveCommand<int> _onNext = new();
        
        public int StateType => (int)ClientStateType;
        public ReactiveCommand<int> OnNext => _onNext;
        
        public abstract ClientStateType ClientStateType { get; }
        
        public virtual void Dispose()
        {
            _onNext?.Dispose();
        }
        
        public virtual void Initialize()
        {
        }

        protected void Next(ClientStateType value)
        {
            _onNext.Execute((int)value);
        }
    }
}