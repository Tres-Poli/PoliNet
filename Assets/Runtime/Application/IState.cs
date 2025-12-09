namespace Runtime.Application
{
    using System;
    using UniRx;

    public interface IState : IDisposable
    {
        public int StateType { get; }
        public ReactiveCommand<int> OnNext { get; }

        public void Initialize();
    }
}