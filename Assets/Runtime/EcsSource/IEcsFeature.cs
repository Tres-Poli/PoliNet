namespace Runtime.EcsSource
{
    using LeoEcs;

    public interface IEcsFeature
    {
        public void InitializeFeature(IEcsSystems systems);
    }
}