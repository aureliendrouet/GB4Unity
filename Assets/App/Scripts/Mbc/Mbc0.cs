
namespace StudioKurage.Emulator.Gameboy
{
    public class Mbc0 : Mbc
    {
        public Mbc0 (byte[][]romBanks)
        {
            this.romBanks = romBanks;
            this.romBankIndex = 1;
        }

        public override void wb (int address, byte value)
        {
            romBank [address] = value;
        }
    }
}
