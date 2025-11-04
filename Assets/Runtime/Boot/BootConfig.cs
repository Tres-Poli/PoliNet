namespace Runtime.Boot
{
    using System.Collections.Generic;
    using Shared.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PoliNet/Configs/Boot", fileName = "BootConfig")]
    public class BootConfig : ScriptableObject
    {
        public NetworkType type;

        [SerializeField]
        public Loader clientLoader;
        
        [SerializeField]
        public Loader serverLoader;

        public Loader[] GetActualLoaders()
        {
            var result = new List<Loader>(2);
            if (BitMaskHelper.HasBit((int)type, (int)NetworkType.Client))
            {
                result.Add(clientLoader);
            }

            if (BitMaskHelper.HasBit((int)type, (int)NetworkType.Server))
            {
                result.Add(serverLoader);
            }

            return result.ToArray();
        }
    }
}