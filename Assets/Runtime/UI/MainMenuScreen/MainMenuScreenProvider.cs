namespace Runtime.UI.MainMenuScreen
{
    using Base;
    using Shared.Enums;
    using UnityEngine;
    using VContainer;

    public sealed class MainMenuScreenProvider : IScreenProvider
    {
        private readonly UIConfig _uiConfig;

        [Inject]
        public MainMenuScreenProvider(UIConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }
        
        public ScreenHolder GetScreen(Transform parent)
        {
            var instance = Object.Instantiate(_uiConfig.GetViewPrefab(UIScreenType.Menu), parent, false);
            var v = instance.GetComponent<MainMenuScreenView>();
            
            var m = new MainMenuScreenModel();
            var vm = new MainMenuScreenViewModel();
            
            m.Initialize(vm);
            v.Initialize(vm);

            return new ScreenHolder(vm, m, v);
        }
    }
}