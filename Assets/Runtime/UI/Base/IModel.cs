namespace Runtime.UI.Base
{
    using System;

    public interface IModel : IDisposable
    {
    }
    
    public interface IModel<T> : IModel where T : IViewModel
    {
        public void Initialize(T viewModel);
    }
}