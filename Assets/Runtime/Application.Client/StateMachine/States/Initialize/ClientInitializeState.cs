namespace Runtime.Application.Client.StateMachine.States.Initialize
{
    using Cysharp.Threading.Tasks;
    using Networking.Shared;

    public sealed class ClientInitializeState : ClientStateBase
    {
        private readonly INetworkClient _networkClient;
        public override ClientStateType ClientStateType => ClientStateType.Initialize;

        public ClientInitializeState(INetworkClient networkClient)
        {
            _networkClient = networkClient;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _networkClient.Start();
            WaitForNextStateConditionsAsync().Forget();
        }

        private async UniTaskVoid WaitForNextStateConditionsAsync()
        {
            var conditions = new UniTask[]
            {
                UniTask.WaitUntil(() => _networkClient.IsConnected)
            };

            await UniTask.WhenAll(conditions);

            Next(ClientStateType.Menu);
        }
    }
}