namespace Runtime.Ecs.Networking.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif

    [Serializable]
    public sealed class NetworkDefaultSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<MessageComponent> _pool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _pool = _world.GetPoolByType(typeof(MessageComponent)) as EcsPool<MessageComponent>;
        }

        public void Run(EcsSystems systems)
        {
        }
    }
}