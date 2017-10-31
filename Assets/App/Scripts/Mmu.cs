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
        public byte[] zram = new byte[127];

        // FF0F - interrupt request register
        public byte ir;

        // FEA0-FF7F - Memory-mapped I/O
        public byte[] mmio = new byte[224];

        // FEA0-FEFF & FF4C-FF7F - unusable
        // skip

        // FE00-FE9F - Object Attribute Memory (for sprites)
        public byte[] oam = new byte[160];

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
        static byte[] Bios = new byte[] { 
            0x31, 0xFE, 0xFF, 0xAF, 0x21, 0xFF, 0x9F, 0x32, 0xCB, 0x7C, 0x20, 0xFB, 0x21, 0x26, 0xFF, 0x0E,
            0x11, 0x3E, 0x80, 0x32, 0xE2, 0x0C, 0x3E, 0xF3, 0xE2, 0x32, 0x3E, 0x77, 0x77, 0x3E, 0xFC, 0xE0,
            0x47, 0x11, 0x04, 0x01, 0x21, 0x10, 0x80, 0x1A, 0xCD, 0x95, 0x00, 0xCD, 0x96, 0x00, 0x13, 0x7B,
            0xFE, 0x34, 0x20, 0xF3, 0x11, 0xD8, 0x00, 0x06, 0x08, 0x1A, 0x13, 0x22, 0x23, 0x05, 0x20, 0xF9,
            0x3E, 0x19, 0xEA, 0x10, 0x99, 0x21, 0x2F, 0x99, 0x0E, 0x0C, 0x3D, 0x28, 0x08, 0x32, 0x0D, 0x20,
            0xF9, 0x2E, 0x0F, 0x18, 0xF3, 0x67, 0x3E, 0x64, 0x57, 0xE0, 0x42, 0x3E, 0x91, 0xE0, 0x40, 0x04,
            0x1E, 0x02, 0x0E, 0x0C, 0xF0, 0x44, 0xFE, 0x90, 0x20, 0xFA, 0x0D, 0x20, 0xF7, 0x1D, 0x20, 0xF2,
            0x0E, 0x13, 0x24, 0x7C, 0x1E, 0x83, 0xFE, 0x62, 0x28, 0x06, 0x1E, 0xC1, 0xFE, 0x64, 0x20, 0x06,
            0x7B, 0xE2, 0x0C, 0x3E, 0x87, 0xE2, 0xF0, 0x42, 0x90, 0xE0, 0x42, 0x15, 0x20, 0xD2, 0x05, 0x20,
            0x4F, 0x16, 0x20, 0x18, 0xCB, 0x4F, 0x06, 0x04, 0xC5, 0xCB, 0x11, 0x17, 0xC1, 0xCB, 0x11, 0x17,
            0x05, 0x20, 0xF5, 0x22, 0x23, 0x22, 0x23, 0xC9, 0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B,
            0x03, 0x73, 0x00, 0x83, 0x00, 0x0C, 0x00, 0x0D, 0x00, 0x08, 0x11, 0x1F, 0x88, 0x89, 0x00, 0x0E,
            0xDC, 0xCC, 0x6E, 0xE6, 0xDD, 0xDD, 0xD9, 0x99, 0xBB, 0xBB, 0x67, 0x63, 0x6E, 0x0E, 0xEC, 0xCC,
            0xDD, 0xDC, 0x99, 0x9F, 0xBB, 0xB9, 0x33, 0x3E, 0x3C, 0x42, 0xB9, 0xA5, 0xB9, 0xA5, 0x42, 0x3C,
            0x21, 0x04, 0x01, 0x11, 0xA8, 0x00, 0x1A, 0x13, 0xBE, 0x20, 0xFE, 0x23, 0x7D, 0xFE, 0x34, 0x20,
            0xF5, 0x06, 0x19, 0x78, 0x86, 0x23, 0x05, 0x20, 0xFB, 0x86, 0x20, 0xFE, 0x3E, 0x01, 0xE0, 0x50
        };

        public bool biosActive;

        // cartridge data
        public byte[] cartridge;

        // memory bank controller
        protected Mbc mbc;

        // rom bank length
        const int RomBankLength = 0x4000;

        public bool lcd {
            get {
                return (rb (0xFF40) & 0x80) == 0x80;
            }
        }

        Gpu gpu;
        Keypad keypad;
        Timer timer;

        public void SetComponents (Gpu gpu, Timer timer, Keypad keypad)
        {
            this.gpu    = gpu;
            this.timer  = timer;
            this.keypad = keypad;
        }

        public void Reset (bool bios)
        {
            biosActive = bios;

            ie = 0;
            ir = 0;

            Array.Clear (zram, 0, zram.Length);
            Array.Clear (mmio, 0, mmio.Length);
            Array.Clear (oam, 0, oam.Length);
            Array.Clear (wram, 0, wram.Length);
            Array.Clear (vram, 0, vram.Length);

            mbc = null;

            if (biosActive) {
                return;
            }

            wbr (0xFFFF, 0x00);
            wbr (Address.NR10, 0x80);
            wbr (Address.NR11, 0xBF);
            wbr (Address.NR12, 0xF3);
            wbr (Address.NR14, 0xBF);
            wbr (Address.NR21, 0x3F);
            wbr (Address.NR22, 0x00);
            wbr (Address.NR24, 0xBF);
            wbr (Address.NR30, 0x7F);
            wbr (Address.NR31, 0xFF);
            wbr (Address.NR32, 0x9F);
            wbr (Address.NR33, 0xBF);
            wbr (Address.NR41, 0xFF);
            wbr (Address.NR42, 0x00);
            wbr (Address.NR43, 0x00);
            wbr (Address.NR44, 0xBF);
            wbr (Address.NR50, 0x77);
            wbr (Address.NR51, 0xF3);
            wbr (Address.NR52, 0xF1);
            wbr (Address.Bgp, 0xFC);
            wbr (Address.Obp0, 0xFF);
            wbr (Address.Obp1, 0xFF);
        }

        public void LoadRom (byte[] rom)
        {
            cartridge = rom;

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
                return Bios [address];
            }

//            switch(address & 0xF000) {
//            case 0x0000:
//            case 0x1000:
//            case 0x2000:
//            case 0x3000:
//                // rom bank 0
//                return mbc.rom[address];
//
//            case 0x4000:
//            case 0x5000:
//            case 0x6000:
//            case 0x7000:
//                // rom banks 1 and higher
//                return mbc.romBank[address - 0x4000];
//
//            case 0x8000:
//            case 0x9000:
//                // working ram
//                return vram [address - 0x8000];
//
//            case 0xA000:
//            case 0xB000:
//                // ram bank
//                return mbc.rrb (address);
//
//            case 0xC000:
//            case 0xD000:
//                // working ram
//                return wram[address - 0xC000];
//            case 0xE000:
//                // remaining echo ram
//                return wram[address - 0xE000];
//
//            case 0xF000:
//                switch (address & 0x0F00) {
//                case 0x000:
//                case 0x100:
//                case 0x200:
//                case 0x300:
//                case 0x400:
//                case 0x500:
//                case 0x600:
//                case 0x700:
//                case 0x800:
//                case 0x900:
//                case 0xA00:
//                case 0xB00:
//                case 0xC00:
//                case 0xD00:
//                    // remaining echo ram
//                    return wram [address - 0xF000];
//
//                case 0xE00:
//                    if (address >= Address.Oam_L && address <= Address.Oam_M) {
//                        // oam
//                        return oam [address - Address.Oam_L];
//                    } else {
//                        // outside range of oam (empty)
//                        return 0x00;
//                    }
//
//                case 0xF00:
//                    // Zero-page RAM, Memory-mapped I/O, including sound, graphics, etc
//                    switch (address) {
//                    // gpu
//                    case Address.Lcdc:
//                        return gpu.lcdc;
//                    case Address.Stat:
//                        return gpu.stat;
//                    case Address.Scy:
//                        return gpu.scy;
//                    case Address.Scx:
//                        return gpu.scx;
//                    case Address.Ly:
//                        return gpu.ly;
//                    case Address.Lyc:
//                        return gpu.lyc;
//                    case Address.Wy:
//                        return gpu.wy;
//                    case Address.Wx:
//                        return gpu.wx;
//                    case Address.Bgp:
//                        return gpu.bgp;
//                    case Address.Obp0:
//                        return gpu.obp0;
//                    case Address.Obp1:
//                        return gpu.obp1;
//
//                    // keypad
//                    case Address.Keypad:
////                        UnityEngine.Debug.LogFormat ("R {0}", Convert.ToString (keypad.memory, 2).PadLeft (8, '0'));
//                        return keypad.memory;
//
//                    // interrupt
//                    case Address.InterruptRequest:
//                        return ir;
//                    case Address.InterruptEnable:
//                        return ie;
//
//                    // timer
//                    case Address.TimerCounter:
//                        return timer.counter;
//                    case Address.TimerDivider:
//                        return timer.divider;
//                    case Address.TimerController:
//                        return timer.controller;
//                    case Address.TimerModulator:
//                        return timer.modulator;
//
//                    default:
//                        if (address >= 0xFF80 && address <= 0xFFFE) {
//                            return zram [address - 0xFF80];
//                        }
//                        break;
//                    }
//                    break;
//                }
//                break;
//            }
//            UnityEngine.Debug.Log ("SHOULD NOT BE HERE " + Convert.ToString(address, 16));

            switch (address) {
                // gpu
            case Address.Lcdc:
                return gpu.lcdc;
            case Address.Stat:
                return gpu.stat;
            case Address.Scy:
                return gpu.scy;
            case Address.Scx:
                return gpu.scx;
            case Address.Ly:
                return gpu.ly;
            case Address.Lyc:
                return gpu.lyc;
            case Address.Wy:
                return gpu.wy;
            case Address.Wx:
                return gpu.wx;
            case Address.Bgp:
                return gpu.bgp;
            case Address.Obp0:
                return gpu.obp0;
            case Address.Obp1:
                return gpu.obp1;

                // keypad
            case Address.Keypad:
                 UnityEngine.Debug.LogFormat ("R {0}", Convert.ToString (keypad.memory, 2).PadLeft (8, '0'));
                return keypad.memory;

                // interrupt
            case Address.InterruptRequest:
                return ir;
            case Address.InterruptEnable:
                return ie;

                // timer
            case Address.TimerCounter:
                return timer.counter;
            case Address.TimerDivider:
                return timer.divider;
            case Address.TimerController:
                return timer.controller;
            case Address.TimerModulator:
                return timer.modulator;
            }

            if (address >= Address.RamBank_L && address <= Address.RamBank_M) {
                return mbc.rrb (address - Address.RamBank_L);
            }

            byte[] memory;
            ushort resolvedAddress;
            ResolveMemoryAddress (address, out memory, out resolvedAddress);

            return memory [resolvedAddress];
        }

        // write byte
        public void wb (ushort address, byte value)
        {
            switch (address) {
            case Address.Lcdc:
                gpu.lcdc = value;
                return;
            case Address.Stat:
                gpu.stat = value;
                return;
            case Address.Scy:
                gpu.scy = value;
                return;
            case Address.Scx:
                gpu.scx = value;
                return;
            case Address.Ly:
                gpu.ly = value;
                return;
            case Address.Lyc:
                gpu.lyc = value;
                return;
            case Address.Wy:
                gpu.wy = value;
                return;
            case Address.Wx:
                gpu.wx = value;
                return;
            case Address.Bgp:
                gpu.bgp = value;
                return;
            case Address.Obp0:
                gpu.obp0 = value;
                return;
            case Address.Obp1:
                gpu.obp1 = value;
                return;

                // keypad
            case Address.Keypad:
                keypad.memory = value;
                UnityEngine.Debug.LogFormat ("W {0} {1}", Convert.ToString (value, 2).PadLeft (8, '0'), Convert.ToString (keypad.memory, 2).PadLeft (8, '0'));
                
                return;

                // timer
            case Address.TimerCounter:
                timer.counter = value;
                return;
            case Address.TimerDivider:
                TrapDividerRegister ();
                return;
            case Address.TimerController:
                timer.controller = value;
                return;
            case Address.TimerModulator:
                timer.modulator = value;
                return;

                // interrupt
            case Address.InterruptRequest:
                ir = value;
                return;
            case Address.InterruptEnable:
                ie = value;
                return;

                // dma
            case Address.Dma:
                TransfertDma (value);
                return;
            }

            if (address >= Address.Apu_L && address <= Address.Apu_M) {
                //apu.wb(address, value);
                return;
            }
            if ((address >= Address.RomBank_L) && (address <= Address.RomBank_M)) {
                mbc.wb ((ushort)(address - Address.RomBank_L), value);
                return;
            }
            if ((address >= Address.RamBank_L) && (address <= Address.RamBank_M)) {
                mbc.rwb ((ushort)(address - Address.RamBank_L), value);
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

        public void TransfertDma (byte value)
        {
            // address is value * 100
            ushort address = (ushort)(value << 8);

            for (int i = 0; i < oam.Length; ++i) {
                oam [i] = rb (address + i);
            }
        }

//        public void TransferDma (ushort dst, ushort src, ushort n)
//        {
//            for (; n > 0; n--) {
//                wb (dst++, rb (src++));
//            }
//        }

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
