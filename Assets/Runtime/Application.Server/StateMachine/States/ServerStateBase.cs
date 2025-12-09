namespace Runtime.Application.Server.StateMachine.States
{
    using UniRx;

    public abstract class ServerStateBase : IState
    {
        private readonly ReactiveCommand<int> _onNext = new();
        
        public int StateType => (int)ServerStateType;
        public ReactiveCommand<int> OnNext => _onNext;
        
        public abstract ServerStateType ServerStateType { get; }
        
        public virtual void Dispose()
        {
            _onNext?.Dispose();
        }
        
        public virtual void Initialize()
        {
        }

        protected void Next(ServerStateType value)
        {
            _onNext.Execute((int)value);
        }
    }
}