namespace Runtime.Networking.Loaders
{
    using Poli.Boot;
    using Shared;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/NetworkClientLoader", fileName = "NetworkClientLoader")]
    public sealed class NetworkClientLoader : ScriptableLoader
    {
        [SerializeField]
        private NetworkConfig config;
        
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            if (!builder.Exists(config.GetType()))
            {
                builder.RegisterInstance(Instantiate(config));
            }
            
            builder.Register<INetworkClient, RiptideClient>(Lifetime.Singleton);
        }
    }
}