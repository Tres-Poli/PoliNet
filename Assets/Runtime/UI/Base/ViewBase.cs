namespace Runtime.UI.Base
{
    using UnityEngine;

    public abstract class ViewBase<T> : MonoBehaviour, IView<T> where T : IViewModel
    {
        public virtual void Initialize(T viewModel)
        {
        }

        public virtual void Dispose()
        {
        }
    }
}