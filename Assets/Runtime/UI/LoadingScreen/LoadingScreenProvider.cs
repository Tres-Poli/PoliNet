namespace Runtime.UI.LoadingScreen
{
    using Base;
    using Shared.Enums;
    using UnityEngine;

    public sealed class LoadingScreenProvider : IScreenProvider<LoadingScreenViewModel>
    {
        private readonly UIConfig _uiConfig;

        public LoadingScreenProvider(UIConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }
        
        
        public ScreenHolder<LoadingScreenViewModel> GetScreen()
        {
            var instance = Object.Instantiate(_uiConfig.GetViewPrefab(UIScreenType.Loading));
            var v = instance.GetComponent<LoadingScreenView>();
            
            var m = new LoadingScreenModel();
            var vm = new LoadingScreenViewModel();

            return new ScreenHolder<LoadingScreenViewModel>(vm, m, v);
        }
    }
}