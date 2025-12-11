namespace Runtime.UI
{
    using Base;
    using LoadingScreen;
    using MessagePipe;
    using Messaging.Messages.Application;
    using Shared;
    using Shared.Enums;
    using UniRx;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;
    using Object = UnityEngine.Object;

    public sealed class UIScreenManager : IStartable
    {
        private CompositeDisposable _disposables;

        private readonly LinearMap<UIScreenType, IScreenProvider> _providersMap;
        private readonly UIConfig _uiConfig;

        private GameObject _canvas;
        private ScreenHolder? _currScreenHolder;
        
        [Inject]
        public UIScreenManager(ISubscriber<RequestUIScreenMessage> requestScreenSub, LoadingScreenProvider loadingProvider, UIConfig uiConfig)
        {
            _uiConfig = uiConfig;
            
            _providersMap = LinearMap<UIScreenType, IScreenProvider>.Create();
            _providersMap[(int)UIScreenType.Loading] = loadingProvider;
            
            _disposables = new CompositeDisposable();
            _disposables.Add(requestScreenSub.Subscribe(RequestScreenMessage_Callback));
        }
        
        public void Start()
        {
            _canvas = Object.Instantiate(_uiConfig.CanvasPrefab);
        }
        
        public void Dispose()
        {
            if (_currScreenHolder.HasValue)
            {
                _currScreenHolder.Value.Dispose();
            }
            
            _disposables.Dispose();
            _disposables = null;
        }

        private void RequestScreenMessage_Callback(RequestUIScreenMessage message)
        {
            if (_currScreenHolder.HasValue)
            {
                _currScreenHolder.Value.Dispose();
            }
            
            _currScreenHolder = _providersMap[(int)message.ScreenType].GetScreen(_canvas.transform);
        }
    }
}