namespace Runtime.Ecs.Networking
{
    using EcsSource;
    using LeoEcs;
    using Systems;

    public class NetworkingFeature : IEcsFeature
    {
        public void InitializeFeature(IEcsSystems systems)
        {
            systems.Add(new NetworkDefaultSystem());
        }
    }
}