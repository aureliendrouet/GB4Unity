
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        public static class RegisterFlag
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
                return (f & RegisterFlag.Zero) == RegisterFlag.Zero;
            }
            set {
                if (value) {
                    f = (byte)(f | RegisterFlag.Zero);
                } else {
                    f = (byte)(f & ~RegisterFlag.Zero);
                }
            }
        }

        public bool sf {
            get {
                return (f & RegisterFlag.Sub) == RegisterFlag.Sub;
            }
            set {
                if (value) {
                    f = (byte)(f | RegisterFlag.Sub);
                } else {
                    f = (byte)(f & ~RegisterFlag.Sub);
                }
            }
        }

        public bool hcf {
            get {
                return (f & RegisterFlag.HalfCarry) == RegisterFlag.HalfCarry;
            }
            set {
                if (value) {
                    f = (byte)(f | RegisterFlag.HalfCarry);
                } else {
                    f = (byte)(f & ~RegisterFlag.HalfCarry);
                }
            }
        }

        public bool cf {
            get {
                return (f & RegisterFlag.Carry) == RegisterFlag.Carry;
            }
            set {
                if (value) {
                    f = (byte)(f | RegisterFlag.Carry);
                } else {
                    f = (byte)(f & ~RegisterFlag.Carry);
                }
            }
        }
    }
}
