
// Memory Bank Controller
namespace StudioKurage.Emulator.Gameboy
{
    public abstract class Mbc
    {
        protected byte[][] romBanks;

        protected int currentRomBankIndex;

        public virtual byte[] currentRomBank {
            get {
                byte[] romBank = null;

                if (currentRomBankIndex < romBanks.Length) {
                    romBank = romBanks [currentRomBankIndex];
                }

                return romBank;
            }
        }

        public Mbc ()
        {
        }

        public Mbc (byte[][]romBanks)
        {
            this.romBanks = romBanks;
            this.currentRomBankIndex = 1;
        }

        public virtual byte rb (int address)
        {
            return 0;
        }

        public virtual void wb (int address, byte val)
        {
        }

        public virtual void wr (int address, byte value)
        {
        }

        public virtual byte rr (int address)
        {
            return 0;
        }
    }
}
