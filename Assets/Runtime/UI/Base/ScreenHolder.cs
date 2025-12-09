namespace Runtime.UI.Base
{
    using System;

    public struct ScreenHolder<T> : IDisposable where T : IViewModel
    {
        private T _viewModel;
        private IModel<T> _model;
        private IView<T> _view;

        public ScreenHolder(T vm, IModel<T> m, IView<T> v)
        {
            _viewModel = vm;
            _model = m;
            _view = v;
        }

        public void Initialize()
        {
            _model.Initialize(_viewModel);
            _view.Initialize(_viewModel);
        }
        
        public void Dispose()
        {
            _viewModel.Dispose();
            _model.Dispose();
            _view.Dispose();
        }
    }
}