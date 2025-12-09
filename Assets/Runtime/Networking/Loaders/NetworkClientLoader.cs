namespace Runtime.Networking.Loaders
{
    using Poli.Boot;
    using Riptide.Utils;
    using Shared;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/NetworkClientLoader", fileName = "NetworkClientLoader")]
    public sealed class NetworkClientLoader : ScriptableLoader
    {
        [SerializeField]
        private NetworkConfig _networkConfig;
        
        [SerializeField]
        private NetworkMessageConfig _networkMessageConfig;
        
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
            
            var networkConfig = Instantiate(_networkConfig);
            var networkMessageConfig = Instantiate(_networkMessageConfig);
            
            var messageProvider = new MessageProvider(networkMessageConfig);
            var messageTypeProvider = new MessageTypeProvider(networkMessageConfig);
            
            var client = new RiptideClient(messageProvider, messageTypeProvider, networkConfig);
            builder.RegisterInstance<INetworkClient>(client);
        }
    }
}