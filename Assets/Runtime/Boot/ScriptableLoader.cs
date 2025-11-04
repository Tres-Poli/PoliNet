namespace Runtime.Boot
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public abstract class ScriptableLoader : ScriptableObject, ILoader
    {
        public abstract void Load(IContainerBuilder builder, LifetimeScope scope);
    }
}