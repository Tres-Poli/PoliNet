namespace Runtime.Shared.Helpers
{
    using System.Runtime.CompilerServices;

    public static class BitMaskHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBit(int value, int mask)
        {
            return (value & mask) > 0;
        }
    }
}