namespace Runtime.Application
{
    using System;
    using UniRx;

    public interface IState : IDisposable
    {
        public int StateType { get; }

        public void Initialize();
    }
}