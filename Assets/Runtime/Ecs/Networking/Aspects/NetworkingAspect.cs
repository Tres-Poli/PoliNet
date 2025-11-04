namespace Runtime.Ecs.Networking.Aspects
{
    using Components;
    using EcsSource;
    using Leopotam.EcsLite;

    public sealed class NetworkingAspect : IEcsAspect
    {
        public EcsPool<MessageComponent> MessagePool;
    }
}