
namespace StudioKurage.Emulator
{
    public static class Flag
    {
        public static void Set (ref byte r, byte flag)
        {
            r |= flag;
        }

        public static void Reset (ref byte r, byte flag)
        {
            r = (byte)(r & ~flag);
        }

        public static bool Has (byte value, byte flag)
        {
            return (value & flag) == flag;
        }
    }
}