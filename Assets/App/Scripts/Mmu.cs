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
// Sprite Attrib Memory (OAM)
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
        // FFFF - interupt enable register
        protected byte[] ie = new byte[1];

        // FF80-FFFE - zero page
        protected byte[] zram = new byte[127];

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
        // skip

        // 8000-9FFF - video ram
        protected byte[] vram = new byte[8192];

        // 4000-7FFF - cartridge rom, other banks
        protected byte[] romBank;

        // 0000-3FFF - cartridge rom, bank 0
        protected byte[] rom;

        // bios (not used)
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

        public bool biosActive = false;

        // Memory Bank Controller
        protected Mbc mbc;

        // Rom Bank Length
        const int RomBankLength = 0x4000;

        // Special Addresses
        const int Address_CartridgeType = 0x0147;
        const int Address_RomSize = 0x0148;
        const int Address_RamSize = 0x0149;

        public Mmu ()
        {
            Reset ();
        }

        public void Reset ()
        {
            Array.Clear (ie, 0, ie.Length);
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
            this.rom = rom;

            byte cartridgeType = rom [Address_CartridgeType];
            byte romSize = rom [Address_RomSize];
            byte ramSize = rom [Address_RamSize];

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
            romBank = mbc.currentRomBank;
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
                break;
            default:
                throw new Exception ("Cartridge not implemented");
            }

            return mbc;
        }

        private void wbr (ushort address, byte value)
        {
            if ((address >= 0xA000) && (address <= 0xBFFF)) {
                mbc.wr ((ushort)(address - 0xA000), value);
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
            if (biosActive && address < 0x100) {
                return bios [address];
            }
            if (address >= 0xFF10 && address <= 0xFF3F) {
//                return audio.rr(address);
            }
            if ((address >= 0xA000) && (address <= 0xBFFF)) {
                return mbc.rr ((ushort)(address - 0xA000));
            }

            byte[] memory;
            ushort resolvedAddress;
            ResolveMemoryAddress (address, out memory, out resolvedAddress);

            return memory [resolvedAddress];
        }

        // write byte
        public void wb (ushort address, byte value)
        {
            if (address >= 0xFF10 && address <= 0xFF3F) {
                //audio.wb(address, value);
                return;
            }
            if (address <= 0x7FFF) { // Write to ROM
                mbc.wb (address, value);
                romBank = mbc.currentRomBank;
                return;
            }
            if ((address >= 0xA000) && (address <= 0xBFFF)) {
                mbc.wr ((ushort)(address - 0xA000), value);
                return;
            }

            byte[] memory;
            ushort resolvedAddress;
            ResolveMemoryAddress (address, out memory, out resolvedAddress);

            memory [resolvedAddress] = value;

            // FF46 - DMA - DMA Transfer and Start Address (W)
            if (address == 0xFF46) {
                byte startAddress = (byte)(rb (0xFF46) * 0x100);
                for (int i = 0; i < 160; ++i) {
                    wb (0xFE00 + i, rb (startAddress + i));
                }
            }
            // FF04 - Divider Timer - Resets to 0 whenever written to
            if (address == 0xFF04) {
                memory [resolvedAddress] = 0;
            }
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
            wb (address, val & 0x00ff);
            wb (address + 1, (val & 0xff00) >> 8);
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
            if (address <= 0x3FFF) {
                memory = rom;
                resolvedAddress = address;
                return;
            }
            if ((address >= 0x4000) && (address <= 0x7FFF)) {
                memory = romBank;
                resolvedAddress = (ushort)(address - 0x4000);
                return;
            }
            if ((address >= 0x8000) && (address <= 0x9FFF)) {
                memory = vram;
                resolvedAddress = (ushort)(address - 0x8000);
                return;
            }
            if ((address >= 0xC000) && (address <= 0xDFFF)) {
                memory = wram;
                resolvedAddress = (ushort)(address - 0xC000);
                return;
            }
            if ((address >= 0xE000) && (address <= 0xFDFF)) {
                memory = wram;
                resolvedAddress = (ushort)(address - 0xE000);
                return;
            }
            if ((address >= 0xFE00) && (address <= 0xFE9F)) {
                memory = oam;
                resolvedAddress = (ushort)(address - 0xFE00);
                return;
            }
            if ((address >= 0xFEA0) && (address <= 0xFF7F)) {
                memory = mmio;
                resolvedAddress = (ushort)(address - 0xFEA0);
                return;
            }
            if ((address >= 0xFF80) && (address <= 0xFFFE)) {
                memory = zram;
                resolvedAddress = (ushort)(address - 0xFF80);
                return;
            }
            if (address == 0xFFFF) {
                memory = ie;
                resolvedAddress = (ushort)0;
                return;
            }

            // should never happen
            memory = new byte[1];
            resolvedAddress = (ushort)0;
        }
    }
}
