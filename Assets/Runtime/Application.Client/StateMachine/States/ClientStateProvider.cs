namespace Runtime.Application.Client.StateMachine.States
{
    using Initialize;
    using Shared;
    using VContainer;

    public sealed class ClientStateProvider
    {
        private LinearMap<ClientStateType, IStateProvider<ClientStateBase>> _stateProviders;
        
        [Inject]
        public ClientStateProvider(ClientDefaultStateProvider defaultProvider, ClientInitializeStateProvider initializeStateProvider)
        {
            _stateProviders = LinearMap<ClientStateType, IStateProvider<ClientStateBase>>.Create();
            
            _stateProviders[(int)ClientStateType.Default] = defaultProvider;
            _stateProviders[(int)ClientStateType.Initialize] = initializeStateProvider;
        }
        
        public ClientStateBase GetState(ClientStateType stateType)
        {
            return _stateProviders[(int)stateType].GetState();
        }
    }
}