namespace Runtime.LeoEcs
{
    using System.Collections.Generic;
    using Shared;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PoliNet/Cache/EcsFeatures", fileName = "EcsFeatures")]
    public class EcsFeaturesCached : ScriptableObject
    {
        [SerializeField]
        public List<SerializableType> aspects = new List<SerializableType>();
        
        [SerializeField]
        public List<SerializableType> features = new List<SerializableType>();
    }
}