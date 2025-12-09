namespace Runtime.UI.Base
{
    using System;

    public interface IView<T> : IDisposable where T : IViewModel
    {
        public void Initialize(T viewModel);
    }
}