namespace Runtime.Shared
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public sealed class SerializableType
    {
        [ReadOnly] 
        [SerializeField]
        public string referenceValue;

        private Type _type;

        public SerializableType(Type type)
        {
            SetType(type);
        }

        public void SetType(Type type)
        {
            referenceValue = GetReferenceValue(type);
            _type = type;
        }

        public Type GetType()
        {
            return GetReferenceType(referenceValue);
        }
        
        public static string GetReferenceValue(Type type)
        {
            return type != null
                ? type.FullName + ", " + type.Assembly.GetName().Name
                : string.Empty;
        }
    
        public static Type GetReferenceType(string referenceValue)
        {
            return !string.IsNullOrEmpty(referenceValue)
                ? Type.GetType(referenceValue)
                : null;
        }
    }
}