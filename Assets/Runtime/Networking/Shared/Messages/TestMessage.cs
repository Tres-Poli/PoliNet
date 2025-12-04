namespace Runtime.Networking.Shared.Messages
{
    using MessagePack;

    [MessagePackObject]
    public struct TestMessage
    {
        [Key(0)]
        public int TestValue1;
        
        [Key(1)]
        public int TestValue2;
    }
}