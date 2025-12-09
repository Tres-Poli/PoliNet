namespace Runtime.Networking.Shared.Messages
{
    using Riptide;
    
    public struct GetTimeResponseMessage : IMessageSerializable
    {
        public float ServerTime;

        public void Serialize(Message message)
        {
            message.AddFloat(ServerTime);
        }

        public void Deserialize(Message message)
        {
            ServerTime = message.GetFloat();
        }
    }
}