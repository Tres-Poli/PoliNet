namespace Runtime.Application
{
    using System.Collections.Generic;
    using Poli.Boot;
    using Shared.Enums;
    using Shared.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Poli/Configs/PoliNetBoot", fileName = "PoliNetBootConfig")]
    public class PoliNetBootConfig : BootConfig
    {
        [SerializeField]
        private NetworkType _networkType;
        
        [SerializeField]
        private CompositeLoader _common;
        
        [SerializeField]
        private CompositeLoader _client;
        
        [SerializeField]
        private CompositeLoader _server;
        
        public override ILoader[] GetLoaders()
        {
            var loaders = new List<ILoader>();
            loaders.Add(_common);
            
            if (BitMaskHelper.HasBit(_networkType, NetworkType.Client))
            {
                loaders.Add(_client);
            }

            if (BitMaskHelper.HasBit(_networkType, NetworkType.Server))
            {
                loaders.Add(_server);
            }

            return loaders.ToArray();
        }
    }
}