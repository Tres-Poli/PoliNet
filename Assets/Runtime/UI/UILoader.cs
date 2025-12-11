namespace Runtime.UI
{
    using DefaultScreen;
    using LoadingScreen;
    using MainMenuScreen;
    using Poli.Boot;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    [CreateAssetMenu(menuName = "Poli/Loaders/UILoader", fileName = "UILoader")]
    public sealed class UILoader : ScriptableLoader
    {
        [SerializeField] 
        private UIConfig _uiConfig;
        
        public override void Load(IContainerBuilder builder, LifetimeScope scope)
        {
            var uiConfigInstance = Instantiate(_uiConfig);

            builder.RegisterInstance(uiConfigInstance);

            builder.Register<DefaultScreenProvider>(Lifetime.Singleton);
            builder.Register<LoadingScreenProvider>(Lifetime.Singleton);
            builder.Register<MainMenuScreenProvider>(Lifetime.Singleton);

            // Startable
            builder.RegisterEntryPoint<UIScreenManager>();
            
            // PostStartable
        }
    }
}