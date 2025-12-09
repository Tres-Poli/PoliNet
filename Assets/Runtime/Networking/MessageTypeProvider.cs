namespace Runtime.Networking
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public sealed class MessageTypeProvider
    {
        private readonly RuntimeNetworkMessageConfigEntry[] _messageMapByKey;
        private readonly Dictionary<Type, RuntimeNetworkMessageConfigEntry> _messageMapByType;

        public MessageTypeProvider(NetworkMessageConfig networkMessageConfig)
        {
            _messageMapByKey = networkMessageConfig.MapByKey();
            _messageMapByType = networkMessageConfig.MapByType();
        }

        public ushort GetMessageId<T>()
        {
            return _messageMapByType[typeof(T)].Key;
        }
        
        public Type GetMessageType(ushort key)
        {
            var messageInfo = _messageMapByKey[key % _messageMapByKey.Length];
            return messageInfo.Type;
        }
    }
}