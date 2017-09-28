
// Memory Bank Controller
namespace StudioKurage.Emulator.Gameboy
{
    public abstract class Mbc
    {
        protected byte[][] romBanks;

        protected int romBankIndex;

        public byte[] rom {
            get {
                return romBanks [0];
            }
        }

        public byte[] romBank {
            get {
                return romBanks [romBankIndex];
            }
        }

        public Mbc ()
        {
        }

        public virtual byte rb (ushort address)
        {
            return 0;
        }

        public virtual void wb (ushort address, byte val)
        {
        }

        public virtual byte rrb (ushort address)
        {
            return 0;
        }

        public virtual void rwb (ushort address, byte value)
        {
        }
    }
}

