namespace Runtime.Networking.Shared
{
    using Riptide;

    public struct MessageInfo<T> where T : IMessageSerializable
    {
        public T Message;
        public ushort SenderId;
    }
}