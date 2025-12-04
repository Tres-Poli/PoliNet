namespace Runtime.Networking.Shared
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "PoliNet/Configs/NetworkConfig", fileName = "NetworkConfig")]
    public class NetworkConfig : ScriptableObject
    {
        public string address = "127.0.0.1";
        public ushort port = 7777;
        public ushort maxClients = 2;
    }
}