using System;
using System.IO;

// Memory Management Unit
//
// Memory Map
//
// Interrupt Enable Register
// --------------------------- FFFF
// Internal RAM
// --------------------------- FF80
// Empty but unusable for I/O
// --------------------------- FF4C
// I/O ports
// --------------------------- FF00
// Empty but unusable for I/O
// --------------------------- FEA0
// Object Attributes Memory
// --------------------------- FE00
// Echo of 8kB Internal RAM
// --------------------------- E000
// 8kB Internal RAM
// --------------------------- C000
// 8kB switchable RAM bank
// --------------------------- A000
// 8kB Video RAM
// --------------------------- 8000 --
// 16kB switchable ROM bank         |
// --------------------------- 4000 |= 32kB Cartrigbe
// 16kB ROM bank #0                 |
// --------------------------- 0000 --
// * NOTE: b = bit, B = byte
namespace StudioKurage.Emulator.Gameboy
{
    public class Mmu
    {
        // FFFF - interrupt enable register
        public byte ie;

        // FF80-FFFE - zero page
        protected byte[] zram = new byte[127];

        // FF0F - interrupt request register
        public byte ir;

        // FEA0-FF7F - Memory-mapped I/O
        protected byte[] mmio = new byte[224];

        // FEA0-FEFF & FF4C-FF7F - unusable
        // skip

        // FE00-FE9F - Object Attribute Memory (for sprites)
        protected byte[] oam = new byte[160];

        // E000-FDFF - working ram (shadown)
        // skip

        // C000-DFFF - working ram
        protected byte[] wram = new byte[8192];

        // A000-BFFF, cartridge external ram
        // managed by mbc

        // 8000-9FFF - video ram
        public byte[] vram = new byte[8192];

        // 4000-7FFF - cartridge rom, other banks
        // managed by mbc

        // 0000-3FFF - cartridge rom, bank 0
        // managed by mbc

        // bios
        byte[] bios = new byte[] { 
            0x31, 0xFE, 0xFF, 0xAF, 0x21, 0xFF, 0x9F, 0x32, 0xCB, 0x7C, 0x20, 0xFB, 0x21, 0x26, 0xFF, 0x0E,
            0x11, 0x3E, 0x80, 0x32, 0xE2, 0x0C, 0x3E, 0xF3, 0xE2, 0x32, 0x3E, 0x77, 0x77, 0x3E, 0xFC, 0xE0,
            0x47, 0x11, 0x04, 0x01, 0x21, 0x10, 0x80, 0x1A, 0xCD, 0x95, 0x00, 0xCD, 0x96, 0x00, 0x13, 0x7B,
            0xFE, 0x34, 0x20, 0xF3, 0x11, 0xD8, 0x00, 0x06, 0x08, 0x1A, 0x13, 0x22, 0x23, 0x05, 0x20, 0xF9,
            0x3E, 0x19, 0xEA, 0x10, 0x99, 0x21, 0x2F, 0x99, 0x0E, 0x0C, 0x3D, 0x28, 0x08, 0x32, 0x0D, 0x20,
            0xF9, 0x2E, 0x0F, 0x18, 0xF3, 0x67, 0x3E, 0x64, 0x57, 0xE0, 0x42, 0x3E, 0x91, 0xE0, 0x40, 0x04,
            0x1E, 0x02, 0x0E, 0x0C, 0xF0, 0x44, 0xFE, 0x90, 0x20, 0xFA, 0x0D, 0x20, 0xF7, 0x1D, 0x20, 0xF2,
            0x0E, 0x13, 0x24, 0x7C, 0x1E, 0x83, 0xFE, 0x62, 0x28, 0x06, 0x1E, 0xC1, 0xFE, 0x64, 0x20, 0x06,
            0x7B, 0xE2, 0x0C, 0x3E, 0x87, 0xF2, 0xF0, 0x42, 0x90, 0xE0, 0x42, 0x15, 0x20, 0xD2, 0x05, 0x20,
            0x4F, 0x16, 0x20, 0x18, 0xCB, 0x4F, 0x06, 0x04, 0xC5, 0xCB, 0x11, 0x17, 0xC1, 0xCB, 0x11, 0x17,
            0x05, 0x20, 0xF5, 0x22, 0x23, 0x22, 0x23, 0xC9, 0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B,
            0x03, 0x73, 0x00, 0x83, 0x00, 0x0C, 0x00, 0x0D, 0x00, 0x08, 0x11, 0x1F, 0x88, 0x89, 0x00, 0x0E,
            0xDC, 0xCC, 0x6E, 0xE6, 0xDD, 0xDD, 0xD9, 0x99, 0xBB, 0xBB, 0x67, 0x63, 0x6E, 0x0E, 0xEC, 0xCC,
            0xDD, 0xDC, 0x99, 0x9F, 0xBB, 0xB9, 0x33, 0x3E, 0x3c, 0x42, 0xB9, 0xA5, 0xB9, 0xA5, 0x42, 0x4C,
            0x21, 0x04, 0x01, 0x11, 0xA8, 0x00, 0x1A, 0x13, 0xBE, 0x20, 0xFE, 0x23, 0x7D, 0xFE, 0x34, 0x20,
            0xF5, 0x06, 0x19, 0x78, 0x86, 0x23, 0x05, 0x20, 0xFB, 0x86, 0x20, 0xFE, 0x3E, 0x01, 0xE0, 0x50
        };

        public bool biosActive;

        // Memory Bank Controller
        protected Mbc mbc;

        // Rom Bank Length
        const int RomBankLength = 0x4000;

        public bool lcd {
            get {
                return (rb (0xFF40) & 0x80) == 0x80;
            }
        }

        public Mmu ()
        {
            Reset ();
        }

        public void Reset ()
        {
            ie = 0;
            ir = 0;
            Array.Clear (zram, 0, zram.Length);
            Array.Clear (mmio, 0, mmio.Length);
            Array.Clear (oam, 0, oam.Length);
            Array.Clear (wram, 0, wram.Length);
            Array.Clear (vram, 0, vram.Length);

            wbr (0xFF05, (byte)0x00);
            wbr (0xFF06, (byte)0x00);
            wbr (0xFF07, (byte)0x00);
            wbr (0xFF10, (byte)0x80);
            wbr (0xFF11, (byte)0xBF);
            wbr (0xFF12, (byte)0xF3);
            wbr (0xFF14, (byte)0xBF);
            wbr (0xFF16, (byte)0x3F);
            wbr (0xFF17, (byte)0x00);
            wbr (0xFF19, (byte)0xBF);
            wbr (0xFF1A, (byte)0x7F);
            wbr (0xFF1B, (byte)0xFF);
            wbr (0xFF1C, (byte)0x9F);
            wbr (0xFF1E, (byte)0xBF);
            wbr (0xFF20, (byte)0xFF);
            wbr (0xFF21, (byte)0x00);
            wbr (0xFF22, (byte)0x00);
            wbr (0xFF23, (byte)0xBF);
            wbr (0xFF24, (byte)0x77);
            wbr (0xFF25, (byte)0xF3);
            wbr (0xFF26, (byte)0xF1);
            wbr (0xFF40, (byte)0x91);
            wbr (0xFF42, (byte)0x00);
            wbr (0xFF43, (byte)0x00);
            wbr (0xFF45, (byte)0x00);
            wbr (0xFF47, (byte)0xFC);
            wbr (0xFF48, (byte)0xFF);
            wbr (0xFF49, (byte)0xFF);
            wbr (0xFF4A, (byte)0x00);
            wbr (0xFF4B, (byte)0x00);
            wbr (0xFFFF, (byte)0x00);

            mbc = null;
        }

        public void LoadRom (byte[] rom)
        {
            byte cartridgeType = rom [Address.CartridgeType];
            byte romSize = rom [Address.RomSize];
            byte ramSize = rom [Address.RamSize];

            int romBankCount, ramBankCount, ramLength;

            GetRomBankInfo (romSize, out romBankCount);
            GetRamBankInfo (ramSize, out ramBankCount, out ramLength);

            byte[][] romBanks = new byte[romBankCount][];
            byte[][] ramBanks = new byte[ramBankCount][];

            romBanks [0] = rom;

            for (int i = 1; i < romBankCount; ++i) {
                romBanks [i] = new byte[RomBankLength];
                Array.Copy (rom, i * RomBankLength, romBanks [i], 0, RomBankLength);
            }

            for (int i = 0; i < ramBankCount; ++i) {
                ramBanks [i] = new byte[ramLength];
            }

            mbc = GetMbc (cartridgeType, romBanks, ramBanks);
        }

        protected void GetRomBankInfo (byte romSize, out int romBankCount)
        {
            switch (romSize) {
            case 0x52:
                romBankCount = 72;
                break;
            case 0x53:
                romBankCount = 80;
                break;
            case 0x54:
                romBankCount = 96;
                break;
            default:
                romBankCount = (int)Math.Pow (2, romSize + 1);
                break;
            }
        }

        protected void GetRamBankInfo (byte ramSize, out int ramBankCount, out int ramLength)
        {
            switch (ramSize) {
            case 0x01:
                ramLength = 2048;
                ramBankCount = 1;
                break;
            case 0x02:
                ramLength = 8192;
                ramBankCount = 1;
                break;
            case 0x03:
                ramLength = 8192;
                ramBankCount = 4;
                break;
            default:
                ramLength = 0;
                ramBankCount = 0;
                break;
            }
        }

        protected Mbc GetMbc (byte cartridgeType, byte[][] romBanks, byte[][] ramBanks)
        {
            Mbc mbc = null;

            switch (cartridgeType) {
            case 0x00:
                mbc = new Mbc0 (romBanks);
                break;
            case 0x01:
                mbc = new Mbc1 (romBanks);
//            case 0x02:
//                mbc = new Mbc1 (romBanks, ramBanks);
//            case 0x03:
//                mbc = new Mbc1 (romBanks, ramBanks, true);
                break;
            default:
                throw new Exception ("Cartridge not implemented");
            }

            return mbc;
        }

        private void wbr (ushort address, byte value)
        {
            if ((address >= Address.RamBank_L) && (address <= Address.RamBank_M)) {
                mbc.rwb ((ushort)(address - Address.RamBank_L), value);
            } else {
                byte[] mem;
                ushort resolvedAddress;
                ResolveMemoryAddress (address, out mem, out resolvedAddress);
                mem [resolvedAddress] = value;
            }
        }

        // read byte
        public byte rb (ushort address)
        {
            if (biosActive && address < Address.Bios_M) {
                return bios [address];
            }
            if (address >= Address.Apu_L && address <= Address.Apu_M) {
//                return audio.rr(address);
                return 0;
            }
            if ((address >= Address.RamBank_L) && (address <= Address.RamBank_M)) {
                return mbc.rrb ((ushort)(address - Address.RamBank_L));
            }
            if (address == Address.InterruptRequest) {
                return ir;
            }
            if (address == Address.InterruptEnable) {
                return ie;
            }
            byte[] memory;
            ushort resolvedAddress;
            ResolveMemoryAddress (address, out memory, out resolvedAddress);

            return memory [resolvedAddress];
        }

        // write byte
        public void wb (ushort address, byte value)
        {
            if (address >= Address.Apu_L && address <= Address.Apu_M) {
                //apu.wb(address, value);
                return;
            }
            if (address <= Address.RomBank_M) {
                mbc.wb (address, value);
                return;
            }
            if ((address >= Address.RamBank_L) && (address <= Address.RamBank_M)) {
                mbc.rwb ((ushort)(address - Address.RamBank_L), value);
                return;
            }
            if (address == Address.Dma) {
                TransfertDma (value);
                return;
            }
            if (address == Address.TimerDivider) {
                TrapDividerRegister ();
                return;
            }
            if (address == Address.InterruptRequest) {
                ir = value;
                return;
            }
            if (address == Address.InterruptEnable) {
                ie = value;
                return;
            }

            byte[] memory;
            ushort resolvedAddress;
            ResolveMemoryAddress (address, out memory, out resolvedAddress);

            memory [resolvedAddress] = value;
        }

        // read word - 16 bits in little endian
        public ushort rw (ushort address)
        {
            int v = rb (address) | (rb (address + 1) << 8);
            return (ushort)v;
        }

        // write word - 16 bits in little endian
        public void ww (int address, ushort val)
        {
            wb (address, val & 0x00FF);
            wb (address + 1, (val & 0xFF00) >> 8);
        }

        // cast method helpers
        public byte rb (int address)
        {
            return rb ((ushort)address);
        }

        public void wb (int address, int value)
        {
            wb ((ushort)address, (byte)value);
        }

        public void ww (int address, int value)
        {
            ww ((ushort)address, (ushort)value);
        }

        void ResolveMemoryAddress (ushort address, out byte[] memory, out ushort resolvedAddress)
        {
            if (address <= Address.Rom_M) {
                memory = mbc.rom;
                resolvedAddress = address;
                return;
            }
            if ((address >= Address.RomBank_L) && (address <= Address.RomBank_M)) {
                memory = mbc.romBank;
                resolvedAddress = (ushort)(address - Address.RomBank_L);
                return;
            }
            if ((address >= Address.Vram_L) && (address <= Address.Vram_M)) {
                memory = vram;
                resolvedAddress = (ushort)(address - Address.Vram_L);
                return;
            }
            if ((address >= Address.Wram_L) && (address <= Address.Wram_M)) {
                memory = wram;
                resolvedAddress = (ushort)(address - Address.Wram_L);
                return;
            }
            if ((address >= Address.Wram_Echo_L) && (address <= Address.Wram_Echo_M)) {
                memory = wram;
                resolvedAddress = (ushort)(address - Address.Wram_Echo_L);
                return;
            }
            if ((address >= Address.Oam_L) && (address <= Address.Oam_M)) {
                memory = oam;
                resolvedAddress = (ushort)(address - Address.Oam_L);
                return;
            }
            if ((address >= Address.Mmio_L) && (address <= Address.Mmio_M)) {
                memory = mmio;
                resolvedAddress = (ushort)(address - Address.Mmio_L);
                return;
            }
            if ((address >= Address.Zram_L) && (address <= Address.Zram_M)) {
                memory = zram;
                resolvedAddress = (ushort)(address - Address.Zram_L);
                return;
            }

            // should never happen
            memory = new byte[1];
            resolvedAddress = 0;
        }

        public byte[] r (ushort begin, ushort end)
        {
            int length = (end - begin) + 1;
            byte[] data = new byte[length ];
            for (ushort i = 0; i < length; i++) {
                data [i] = rb (begin + i);
            }
            return data;
        }

        #region Direct Memory Access

        public void Dma (ushort dest, ushort src, ushort n)
        {
            for (; n > 0; n--) {
                wb (dest++, rb (src++));
            }
        }

        void TransfertDma (byte value)
        {
            // address is value * 100
            ushort address = (ushort)(value << 8);

            for (int i = 0; i < oam.Length; ++i) {
                oam [i] = rb (address + i);
            }
        }

        #endregion

        void TrapDividerRegister ()
        {
            mmio [Address.TimerDivider - Address.Mmio_L] = 0;
        }

        #region Interrupt

        public void RequestInterrupt (byte flag)
        {
            wb (Address.InterruptRequest, rb (Address.InterruptRequest) | flag);
        }

        #endregion
    }
}
