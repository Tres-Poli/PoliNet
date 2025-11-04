namespace Runtime.LeoEcs
{
    using Leopotam.EcsLite;

    public interface IEcsSystems
    {
        public EcsWorld GetWorld();
        public IEcsSystems Add(IEcsSystem system);
        public void Init();
        public void Run();
    }
}