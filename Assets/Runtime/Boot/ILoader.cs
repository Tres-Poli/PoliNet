namespace Runtime.Boot
{
    using Cysharp.Threading.Tasks;
    using VContainer;
    using VContainer.Unity;

    public interface ILoader
    {
        public void Load(IContainerBuilder builder, LifetimeScope scope);
    }
}