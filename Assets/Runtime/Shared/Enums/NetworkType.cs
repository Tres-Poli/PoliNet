namespace Runtime.Shared.Enums
{
    using System;

    [Flags]
    public enum NetworkType : byte
    {
        Client = 1,
        Server = 1 << 1
    }
}