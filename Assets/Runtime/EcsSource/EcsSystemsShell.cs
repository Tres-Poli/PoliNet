namespace Runtime.LeoEcs
{
    using Leopotam.EcsLite;
    using VContainer.Unity;

    public sealed class EcsSystemsShell : IEcsSystems
    {
        private readonly EcsSystems _systems;
        private readonly LifetimeScope _scope;

        public EcsSystemsShell(EcsSystems systems, LifetimeScope scope)
        {
            _systems = systems;
            _scope = scope;
        }

        public EcsWorld GetWorld()
        {
            return _systems.GetWorld();
        }

        public IEcsSystems Add(IEcsSystem system)
        {
            _systems.Add(system);
            _scope.Container.Inject(system);
            
            return this;
        }

        public void Init()
        {
            _systems.Init();
        }

        public void Run()
        {
            _systems.Run();
        }
    }
}