namespace Runtime.UI.Base
{
    using System;

    public struct ScreenHolder : IDisposable
    {
        private IViewModel _viewModel;
        private IModel _model;
        private IView _view;

        public ScreenHolder(IViewModel vm, IModel m, IView v)
        {
            _viewModel = vm;
            _model = m;
            _view = v;
        }
        
        public void Dispose()
        {
            _viewModel.Dispose();
            _model.Dispose();
            _view.Dispose();
        }
    }
}