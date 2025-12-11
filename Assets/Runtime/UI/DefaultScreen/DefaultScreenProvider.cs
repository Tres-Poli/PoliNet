namespace Runtime.UI.DefaultScreen
{
    using Base;
    using Shared.Enums;
    using UnityEngine;

    public class DefaultScreenProvider : IScreenProvider
    {
        private readonly UIConfig _uiConfig;

        public DefaultScreenProvider(UIConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }

        public ScreenHolder GetScreen(Transform parent)
        {
            var m = new DefaultScreenModel();
            var v = Object.Instantiate(_uiConfig.GetViewPrefab(UIScreenType.Default), parent, false).GetComponent<DefaultScreenView>();
            var vm = new DefaultScreenViewModel();

            m.Initialize(vm);
            v.Initialize(vm);

            return new ScreenHolder(vm, m, v);
        }
    }
}