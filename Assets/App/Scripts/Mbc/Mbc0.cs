
namespace StudioKurage.Emulator.Gameboy
{
    public class Mbc0 : Mbc
    {
        public Mbc0 (byte[][]romBanks)
        {
            this.romBanks = romBanks;
            this.romBankIndex = 1;
        }

        public override byte rb (ushort address)
        {
            return romBank [address];
        }

        public override void wb (ushort address, byte value)
        {
            romBank [address] = value;
        }
    }
}
