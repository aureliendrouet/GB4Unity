
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        #region 8 bits registers
        public byte a; // accumulator register
        public byte f; // status register

        public byte b; // used as a 8-bit counter
        public byte c; // used to interface with hardware ports

        public byte d; // usually used with e
        public byte e; // usually used with d

        public byte h; // usually not used in 8-bit form
        public byte l; // usually not used in 8-bit form
        #endregion

        #region 16 bits registers
        public ushort pc; // program counter
        public ushort sp; // stack pointer
        #endregion

        #region 8 bits registers used as 16 bits
        public ushort af // normally not used
        {
            get { return (ushort)(a << 8 | (byte)f); }
            private set { a = (byte)((value >> 8) & 0xFF); f =  (byte)(value & 0xFF); }
        }

        public ushort bc // used as a 16-bit counter
        {
            get { return (ushort)(b << 8 | c); }
            private set { b = (byte)((value >> 8) & 0xFF); c = (byte)(value & 0xFF); }
        }

        public ushort de // hold the address of a memory location that is a destination
        {
            get { return (ushort)(d << 8 | e); }
            private set { d = (byte)((value >> 8) & 0xFF); e = (byte)(value & 0xFF); }
        }

        public ushort hl // general 16-bit register
        {
            get { return (ushort)(h << 8 | l); }
            private set { h = (byte)((value >> 8) & 0xFF); l = (byte)(value & 0xFF); }
        }
        #endregion

        #region stack utilities
        public byte popb()
        {
            var value = mmu.rb(sp);
            sp ++;
            return value;
        }

        public ushort popw()
        {
            var value = mmu.rw(sp);
            sp += 2;
            return value;
        }

        public void pushb(byte value)
        {
            sp --;
            mmu.wb(sp, value);
        }

        public void pushw(ushort value)
        {
            sp -= 2;
            mmu.ww(sp, value);
        }

        public void pushw(ushort value, ushort offset)
        {
            sp -= 2;
            mmu.ww(sp, (ushort)(value + offset));
        }
        #endregion
    }
}
