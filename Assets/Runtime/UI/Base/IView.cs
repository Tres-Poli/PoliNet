namespace Runtime.UI.Base
{
    using System;

    public interface IView : IDisposable
    {
    }
    
    public interface IView<T> : IView where T : IViewModel
    {
        public void Initialize(T viewModel);
    }
}