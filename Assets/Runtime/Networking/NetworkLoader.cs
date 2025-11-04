namespace Runtime.Networking
{
    using Boot;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "PoliNet/Loaders/NetworkLoader", fileName = "NetworkLoader")]
    public class NetworkLoader : ScriptableLoader
    {
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            builder.Register<INetwork, RiptideNetwork>(Lifetime.Singleton);
        }
    }
}