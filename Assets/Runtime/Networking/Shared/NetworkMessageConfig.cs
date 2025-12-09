namespace Runtime.Networking.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Shared;
    using UnityEngine;
    
#if UNITY_EDITOR
    using Riptide;
    using UnityEditor;
    using Sirenix.OdinInspector;
#endif

    [CreateAssetMenu(menuName = "Poli/Configs/NetworkMessageConfig", fileName = "NetworkMessageConfig")]
    public sealed class NetworkMessageConfig : ScriptableObject
    {
        private Dictionary<Type, RuntimeNetworkMessageConfigEntry> _assembledMapByType;
        private RuntimeNetworkMessageConfigEntry[] _assembledMapByKey;
        
        public List<NetworkMessageConfigEntry> Entries = new();

        public Dictionary<Type, RuntimeNetworkMessageConfigEntry> MapByType()
        {
            if (_assembledMapByType != null)
            {
                return _assembledMapByType;
            }

            _assembledMapByType = Entries.ToDictionary(x => x.Type.GetType(), x => new RuntimeNetworkMessageConfigEntry
            {
                Key = x.Key,
                Type = x.Type.GetType(),
                MediatorType = x.MediatorType.GetType()
            });

            return _assembledMapByType;
        }

        public RuntimeNetworkMessageConfigEntry[] MapByKey()
        {
            if (_assembledMapByKey != null)
            {
                return _assembledMapByKey;
            }

            _assembledMapByKey = Entries.Select(x => new RuntimeNetworkMessageConfigEntry
            {
                Key = x.Key,
                Type = x.Type.GetType(),
                MediatorType = x.Type.GetType()
            }).ToArray();

            return _assembledMapByKey;
        }
      
#if UNITY_EDITOR
        
        [Button("Collect messages")]
        public void CollectMessages()
        {
            Entries.Clear();
            
            var messageTypes = TypeCache.GetTypesDerivedFrom<IMessageSerializable>();
            var mediators = TypeCache.GetTypesDerivedFrom<IMessageMediator>();

            for (int i = 0; i < messageTypes.Count; i++)
            {
                var messageType = messageTypes[i];
                var mediatorType = mediators.FirstOrDefault(x =>
                {
                    if (x.BaseType == null)
                    {
                        return false;
                    }

                    return x.BaseType.GetGenericArguments().Contains(messageType);
                });
                
                Entries.Add(new NetworkMessageConfigEntry
                {
                    Key = (UInt16)i,
                    Type = new SerializableType(messageType),
                    MediatorType = new SerializableType(mediatorType)
                });
            }
            
            AssetDatabase.SaveAssets();
        }
        
#endif
    }

    [Serializable]
    public struct NetworkMessageConfigEntry
    {
        public ushort Key;
        public SerializableType Type;
        public SerializableType MediatorType;
    }

    public struct RuntimeNetworkMessageConfigEntry
    {
        public ushort Key;
        public Type Type;
        public Type MediatorType;
    }
}