namespace Runtime.Application.Client.StateMachine
{
    using System;
    using States;
    using VContainer.Unity;
    using UniRx;
    using VContainer;

    public sealed class ClientStateMachine : IPostStartable
    {
        private ClientStateProvider _stateProvider;

        private ClientStateBase _currentState;
        private IDisposable _stateCallbackSub;
        
        [Inject]
        public ClientStateMachine(ClientStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public void PostStart()
        {
            SwitchState(ClientStateType.Initialize);
        }

        private void SwitchState(ClientStateType stateType)
        {
            _stateCallbackSub?.Dispose();
            _currentState?.Dispose();
            
            _currentState = _stateProvider.GetState(stateType);
            _currentState.Initialize();
            _stateCallbackSub = _currentState.OnNext.Subscribe(SwitchState);
        }
    }
}