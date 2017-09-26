
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        public static class Flag
        {
            // Set when the result of a math operation is zero or two values match when using the CP instruction.
            public static byte Zero = 0x80;

            // Set if a subtraction was performed in the last math instruction.
            public static byte Sub = 0x40;

            // Set if a carry occurred from the lower nibble in the last math operation.
            public static byte HalfCarry = 0x20;

            // Set if a carry occurred from the last math operation or if register A is the smaller value when executing the CP instruction.
            public static byte Carry = 0x10;
        }

        public bool zf {
            get {
                return (f & Flag.Zero) == Flag.Zero;
            }
            set {
                if (value) {
                    f = (byte)(f | Flag.Zero);
                } else {
                    f = (byte)(f & ~Flag.Zero);
                }
            }
        }

        public bool sf {
            get {
                return (f & Flag.Sub) == Flag.Sub;
            }
            set {
                if (value) {
                    f = (byte)(f | Flag.Sub);
                } else {
                    f = (byte)(f & ~Flag.Sub);
                }
            }
        }

        public bool hcf {
            get {
                return (f & Flag.HalfCarry) == Flag.HalfCarry;
            }
            set {
                if (value) {
                    f = (byte)(f | Flag.HalfCarry);
                } else {
                    f = (byte)(f & ~Flag.HalfCarry);
                }
            }
        }

        public bool cf {
            get {
                return (f & Flag.Carry) == Flag.Carry;
            }
            set {
                if (value) {
                    f = (byte)(f | Flag.Carry);
                } else {
                    f = (byte)(f & ~Flag.Carry);
                }
            }
        }
    }
}
