
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        #region 8 bits registers
        public byte a; // accumulator register
        public byte f; // flag register

        public byte b;
        public byte c;

        public byte d;
        public byte e;

        public byte h;
        public byte l;
        #endregion

        #region 16 bits registers
        public ushort pc; // program counter
        public ushort sp; // stack pointer
        #endregion

        #region 8 bits registers used as 16 bits
        public ushort af
        {
            get { return (ushort)(a << 8 | (byte)f); }
            private set { a = (byte)((value >> 8) & 0xFF); f =  (byte)(value & 0xFF); }
        }

        public ushort bc
        {
            get { return (ushort)(b << 8 | c); }
            private set { b = (byte)((value >> 8) & 0xFF); c = (byte)(value & 0xFF); }
        }

        public ushort de
        {
            get { return (ushort)(d << 8 | e); }
            private set { d = (byte)((value >> 8) & 0xFF); e = (byte)(value & 0xFF); }
        }

        public ushort hl
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
