namespace Runtime.Boot
{
    using System;
    using System.Collections.Generic;
    using VContainer;
    using VContainer.Unity;

    [Serializable]
    public class Loader
    {
        public List<ScriptableLoader> loaders = new List<ScriptableLoader>();

        public void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            foreach (var loader in loaders)
            {
                loader.Load(builder, scope);
            }
        }
    }
}