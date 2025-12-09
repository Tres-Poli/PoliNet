namespace Runtime.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Enums;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Poli/Configs/UIConfig", fileName = "UIConfig")]
    public sealed class UIConfig : ScriptableObject
    {
        [SerializeField] 
        private List<UIConfigEntry> _entries;

        public GameObject GetViewPrefab(UIScreenType type)
        {
            var result = _entries.First(x => x.ScreenType == type);
            return result.ViewPrefab;
        }
    }

    [Serializable]
    public struct UIConfigEntry
    {
        public UIScreenType ScreenType;
        public GameObject ViewPrefab;
    }
}