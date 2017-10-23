
namespace StudioKurage.Emulator.Gameboy
{
    public static class Address
    {
        // Bios
        public const ushort Bios_L = 0x0000;
        public const ushort Bios_M = 0x0100;

        // Cartridge
        public const ushort NintendoLogo_L = 0x0104;
        public const ushort NintendoLogo_M = 0x0133;

        public const ushort GameTitle_L = 0x0134;
        public const ushort GameTitle_M = 0x013E;

        public const ushort ManufacturerCode_L = 0x013F;
        public const ushort ManufacturerCode_M = 0x0142;

        public const ushort GameboyColorCompatibility = 0x0143;

        public const ushort NewLicense_L = 0x0144;
        public const ushort NewLicense_M = 0x0145;

        public const ushort SuperGameboyCompatibility = 0x0146;

        public const ushort CartridgeType = 0x0147;
        public const ushort RomSize       = 0x0148;
        public const ushort RamSize       = 0x0149;

        public const ushort DestinationCode = 0x014A;

        public const ushort OldLicense = 0x014B;

        public const ushort MaskRomVersion = 0x014C;

        public const ushort ComplementChecksum = 0x014D;

        public const ushort CheckSum_L = 0x014E;
        public const ushort CheckSum_M = 0x014F;

        // Mmu
        public const ushort Rom_L        = 0x0000;
        public const ushort Rom_M        = 0x3FFF;

        public const ushort RomBank_L    = 0x4000;
        public const ushort RomBank_M    = 0x7FFF;

        public const ushort Vram_L       = 0x8000;
        public const ushort Vram_M       = 0x9FFF;

        public const ushort RamBank_L    = 0xA000;
        public const ushort RamBank_M    = 0xBFFF;

        public const ushort Wram_L       = 0xC000;
        public const ushort Wram_M       = 0xDFFF;

        public const ushort Wram_Echo_L  = 0xE000;
        public const ushort Wram_Echo_M  = 0xFDFF;

        public const ushort Oam_L        = 0xFE00;
        public const ushort Oam_M        = 0xFE9F;

        public const ushort Mmio_L       = 0xFEA0;
        public const ushort Apu_L        = 0xFF10;
        public const ushort Apu_M        = 0xFF3F;
        public const ushort Mmio_M       = 0xFF7F;

        public const ushort Zram_L       = 0xFF80;
        public const ushort Zram_M       = 0xFFFE;

        // Rendering
        public const ushort CharacterRam_L = 0x8000;
        public const ushort CharacterRam_M = 0x97FF;

        public const ushort BackgroundMapDataA_L = 0x9800;
        public const ushort BackgroundMapDataA_M = 0x9BFF;

        public const ushort BackgroundMapDataB_L = 0x9C00;
        public const ushort BackgroundMapDataB_M = 0x9FFF;

        public const ushort TilesetA       = 0x8000;
        public const ushort TilesetB       = 0x9000;

        public const ushort BackgroundTilemapB = 0x9800;
        public const ushort BackgroundTilemapA = 0x9C00;

        public const ushort WindowTilemapB = 0x9800;
        public const ushort WindowTilemapA = 0x9C00;

        // Timer
        public const ushort TimerLo          = 0xFF03;
        public const ushort TimerDivider     = 0xFF04;
        public const ushort TimerCounter     = 0xFF05;
        public const ushort TimerModulator   = 0xFF06;
        public const ushort TimerController  = 0xFF07;

        // Apu
        public const ushort NR10 = 0xFF10; // Channel 1 Sweep
        public const ushort NR11 = 0xFF11; // Channel 1 Sound length/Wave Pattern
        public const ushort NR12 = 0xFF12; // Channel 1 Volume Envelop
        public const ushort NR13 = 0xFF13; // Channel 1 Frequency LOW
        public const ushort NR14 = 0xFF14; // Channel 1 Frequency HIGH

        public const ushort NR20 = 0xFF15; // UNUSED
        public const ushort NR21 = 0xFF16; // Channel 2 Sound length/Wave Pattern
        public const ushort NR22 = 0xFF17; // Channel 2 Volume Envelop
        public const ushort NR23 = 0xFF18; // Channel 2 Frequency LOW
        public const ushort NR24 = 0xFF19; // Channel 2 Frequency HIGH

        public const ushort NR30 = 0xFF1A; // Channel 3 ON/OFF
        public const ushort NR31 = 0xFF1B; // Channel 3 Sound length
        public const ushort NR32 = 0xFF1C; // Channel 3 Select Output Level
        public const ushort NR33 = 0xFF1D; // Channel 3 Frequency LOW
        public const ushort NR34 = 0xFF1E; // Channel 3 Frequency HIGH

        public const ushort NR41 = 0xFF20; // Channel 4 Sound length
        public const ushort NR42 = 0xFF21; // Channel 4 Volume Envelope
        public const ushort NR43 = 0xFF22; // Channel 4 Polynomial Counter
        public const ushort NR44 = 0xFF23; // Channel 4 Counter/consecutive selection

        public const ushort NR50 = 0xFF24; // Sound Control Register ON/OFF / Volume Control
        public const ushort NR51 = 0xFF25; // Output terminal selection
        public const ushort NR52 = 0xFF26; // Sound On/Off

        public const ushort WavePatternRam_L = 0xFF30;
        public const ushort WavePatternRam_M = 0xFF3F;

        // Gpu
        public const ushort Lcdc  = 0xFF40;
        public const ushort Stat  = 0xFF41;
        public const ushort Scy   = 0xFF42;
        public const ushort Scx   = 0xFF43;
        public const ushort Ly    = 0xFF44;
        public const ushort Lyc   = 0xFF45;
        public const ushort Dma   = 0xFF46;
        public const ushort Bgp   = 0xFF47;
        public const ushort Obp0  = 0xFF48;
        public const ushort Obp1  = 0xFF49;
        public const ushort Wx    = 0xFF4A;
        public const ushort Wy    = 0xFF4B;
        public const ushort Key1  = 0xFF4D;
        public const ushort Vbk   = 0xFF4F; // Vram Bank Selection
        public const ushort Hdma1 = 0xFF51; // New DMA Source Hight
        public const ushort Hdma2 = 0xFF52; // New DMA Source Low
        public const ushort Hdma3 = 0xFF53; // New DMA Destination Hight
        public const ushort Hdma4 = 0xFF54; // New DMA Destination Low
        public const ushort Hdma5 = 0xFF55; // New DMA Length/Mode/Start
        public const ushort Bgpi  = 0xFF68; // Background Palette Index
        public const ushort Bgpd  = 0xFF69; // Background Palette Data
        public const ushort Obpi  = 0xFF6A; // Object Palette index
        public const ushort Obpd  = 0xFF6B; // Object Palette Data
        public const ushort Svbk  = 0xFF70;

        // Interrupt
        public const ushort InterruptHandler_Vblank                 = 0x0040;
        public const ushort InterruptHandler_LcdcStat               = 0x0048;
        public const ushort InterruptHandler_TimeOverflow           = 0x0050;
        public const ushort InterruptHandler_SerialTransferComplete = 0x0058;
        public const ushort InterruptHandler_Joypad                 = 0x0060;
        public const ushort InterruptRequest                        = 0xFF0F;
        public const ushort InterruptEnable                         = 0xFFFF;

        // Keypad
        public const ushort Keypad = 0xFF00;

        // Serial
        public const ushort SerialShift = 0xFF01;
        public const ushort SerialControl = 0xFF02;
    }
}
