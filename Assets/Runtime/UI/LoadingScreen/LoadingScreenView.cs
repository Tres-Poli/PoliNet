namespace Runtime.UI.LoadingScreen
{
    using System.Collections.Generic;
    using Base;
    using Elements;
    using UnityEngine;

    public sealed class LoadingScreenView : ViewBase<LoadingScreenViewModel>
    {
        [SerializeField]
        private List<ThrobberUIElement> _throbbers = new();

        public override void Initialize(LoadingScreenViewModel vm)
        {
            base.Initialize(vm);
            foreach (var throbber in _throbbers)
            {
                throbber.DoSpinAsync(LifetimeToken).Forget();
            }
        }
    }
}