namespace Runtime
{
    using Networking;
    using Networking.Shared;
    using Riptide.Utils;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Tests : MonoBehaviour
    {
        [SerializeField] 
        private NetworkConfig _networkConfig;

        [SerializeField] 
        private NetworkMessageConfig _networkMessageConfig;

        [SerializeField] 
        [ReadOnly] 
        private bool _isConnected;

        [SerializeField] 
        [ReadOnly] 
        private float _clientTime;

        [SerializeField] 
        [ReadOnly] 
        private float _serverTime;

        private INetworkClient _networkClient;
        private INetworkServer _networkServer;
        
        private void Start()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
            
            _networkClient = new RiptideClient(new MessageProvider(_networkMessageConfig), new MessageTypeProvider(_networkMessageConfig), _networkConfig);
            _networkServer = new RiptideServer(new MessageProvider(_networkMessageConfig), new MessageTypeProvider(_networkMessageConfig), _networkConfig);
            
            _networkServer.Start();
        }

        private void Update()
        {
            _isConnected = _networkClient.IsConnected;
            _clientTime = _networkClient.ServerTime;
            _serverTime = _networkServer.ServerTime;
        }

        [Button("Connect")]
        private void ConnectClient()
        {
            _networkClient.Start();
        }
    }
}