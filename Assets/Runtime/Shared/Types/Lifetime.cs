namespace Runtime.Shared
{
    using System;
    using System.Threading;
    using UniRx;
    using UnityEngine;

    public interface ILifetime : IDisposable
    {
        public CancellationToken LifetimeToken { get; }
        public void RegisterDisposeCallback(Action callback);
    }
    
    public class Lifetime : ILifetime
    {
        private ReactiveCommand _onDisposeRx = new();
        private CompositeDisposable _disposables = new();
        private CancellationTokenSource _cts = new();

        public CancellationToken LifetimeToken => _cts.Token;
        
        public void RegisterDisposeCallback(Action callback)
        {
            _disposables.Add(_onDisposeRx.Subscribe(_ => callback.Invoke()));
        }
        
        public virtual void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            
            _onDisposeRx.Execute();
            _onDisposeRx.Dispose();
            _onDisposeRx = null;

            _disposables.Dispose();
            _disposables = null;
        }
    }

    public class LifetimeMonoBehaviour : MonoBehaviour, ILifetime
    {
        private Lifetime _lifetime = new();
        
        public CancellationToken LifetimeToken => _lifetime.LifetimeToken;
        
        public void RegisterDisposeCallback(Action callback)
        {
            _lifetime.RegisterDisposeCallback(callback);
        }
        
        public virtual void Dispose()
        {
            _lifetime.Dispose();
            _lifetime = null;
        }
    }
}