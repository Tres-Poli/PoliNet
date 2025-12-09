namespace Runtime.UI
{
    using System;
    using MessagePipe;
    using Messaging.Messages.Application;
    using Shared.Enums;
    using UniRx;

    public sealed class UIScreenManager : IDisposable
    {
        private CompositeDisposable _disposables;
        
        public UIScreenManager(ISubscriber<RequestUIScreenMessage> requestScreenSub)
        {
            _disposables = new CompositeDisposable();
            
            _disposables.Add(requestScreenSub.Subscribe(RequestScreen_Callback));
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void RequestScreen_Callback(RequestUIScreenMessage message)
        {
            switch (message.ScreenType)
            {
                case UIScreenType.Default:
                    break;
                
                case UIScreenType.Loading:
                    break;
            }
        }
    }
}