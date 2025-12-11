namespace Runtime.UI.Base
{
    using UnityEngine;

    public interface IScreenProvider
    {
        public ScreenHolder GetScreen(Transform parent);
    }
}