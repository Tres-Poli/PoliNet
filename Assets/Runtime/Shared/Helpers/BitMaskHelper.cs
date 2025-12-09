namespace Runtime.Shared.Helpers
{
    using System;
    using System.Runtime.CompilerServices;

    public static class BitMaskHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBit(int value, int mask)
        {
            return (value & mask) > 0;
        }

        public static bool HasBit<T>(T value, T mask) where T : Enum
        {
            return false;
        }
    }
}