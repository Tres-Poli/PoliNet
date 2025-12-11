namespace Runtime.UI.LoadingScreen
{
    using Base;
    using Shared.Enums;
    using UnityEngine;
    using VContainer;

    public sealed class LoadingScreenProvider : IScreenProvider
    {
        private readonly UIConfig _uiConfig;

        [Inject]
        public LoadingScreenProvider(UIConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }
        
        public ScreenHolder GetScreen(Transform parent)
        {
            var instance = Object.Instantiate(_uiConfig.GetViewPrefab(UIScreenType.Loading), parent, false);
            var v = instance.GetComponent<LoadingScreenView>();
            
            var m = new LoadingScreenModel();
            var vm = new LoadingScreenViewModel();
            
            m.Initialize(vm);
            v.Initialize(vm);

            return new ScreenHolder(vm, m, v);
        }
    }
}