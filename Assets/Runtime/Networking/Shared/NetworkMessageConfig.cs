namespace Runtime.Networking.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Shared;
    using UnityEngine;
    
#if UNITY_EDITOR
    using MessagePack;
    using UnityEditor;
    using Sirenix.OdinInspector;
#endif

    [CreateAssetMenu(menuName = "Poli/Configs/NetworkMessageConfig", fileName = "NetworkMessageConfig")]
    public sealed class NetworkMessageConfig : ScriptableObject
    {
        private Dictionary<Type, RuntimeNetworkMessageConfigEntry> _assembledMapByType;
        private RuntimeNetworkMessageConfigEntry[] _assembledMapByKey;
        
        public List<NetworkMessageConfigEntry> Entries = new();
        public int MessageTypeBytes => 2;

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
            
            var messageTypes = TypeCache.GetTypesWithAttribute<MessagePackObjectAttribute>();
            var mediators = TypeCache.GetTypesDerivedFrom<IMessageMediator>();
            
            try
            {
                checked
                {
                    for (int i = 0; i < messageTypes.Count; i++)
                    {
                        var messageType = messageTypes[i];
                        var mediatorType = mediators.FirstOrDefault(x => x.GenericTypeArguments.Length > 0 && x.GenericTypeArguments[0] == messageType);
                        Entries.Add(new NetworkMessageConfigEntry
                        {
                            Key = (UInt16)i,
                            Type = new SerializableType(messageType),
                            MediatorType = new SerializableType(mediatorType)
                        });
                    }
                }
            }
            catch (OverflowException ex)
            {
                Debug.LogError(ex);
            }
            
            AssetDatabase.SaveAssets();
        }
        
#endif
    }

    [Serializable]
    public struct NetworkMessageConfigEntry
    {
        public UInt16 Key;
        public SerializableType Type;
        public SerializableType MediatorType;
    }

    public struct RuntimeNetworkMessageConfigEntry
    {
        public UInt16 Key;
        public Type Type;
        public Type MediatorType;
    }
}