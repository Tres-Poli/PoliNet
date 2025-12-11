namespace Runtime.UI.Base
{
    using Shared;

    public abstract class ViewBase<T> : LifetimeMonoBehaviour, IView<T> where T : IViewModel
    {
        public virtual void Initialize(T viewModel)
        {
        }
    }
}