namespace Runtime.Networking.Shared.Messages
{
    using MessagePack;

    [MessagePackObject]
    public struct SyncTimeResponseMessage
    {
        [Key(0)] public float ServerSideDelay;
        [Key(2)] public float ServerTime;
    }
}