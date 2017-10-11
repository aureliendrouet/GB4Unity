
namespace StudioKurage.Emulator.Gameboy
{
    public static class Address
    {
        // Cartridge
        public const ushort CartridgeType = 0x0147;
        public const ushort RomSize = 0x0148;
        public const ushort RamSize = 0x0149;

        // Mmu
        public const ushort Bios         = 0x100;
        public const ushort Rom_L        = 0x0000;
        public const ushort Rom_H        = 0x3FFF;

        public const ushort RomBank_L    = 0x4000;
        public const ushort RomBank_H    = 0x7FFF;

        public const ushort Vram_L       = 0x8000;
        public const ushort Vram_H       = 0x9FFF;

        public const ushort RamBank_L    = 0xA000;
        public const ushort RamBank_H    = 0xBFFF;

        public const ushort Wram_L       = 0xC000;
        public const ushort Wram_H       = 0xDFFF;

        public const ushort Wram_Echo_L  = 0xE000;
        public const ushort Wram_Echo_H  = 0xFDFF;

        public const ushort Oam_L        = 0xFE00;
        public const ushort Oam_H        = 0xFE9F;

        public const ushort Mmio_L       = 0xFEA0;
        public const ushort DividerTimer = 0xFF04;
        public const ushort Audio_L      = 0xFF10;
        public const ushort Audio_H      = 0xFF3F;
        public const ushort Dma          = 0xFF46;
        public const ushort Mmio_H       = 0xFF7F;

        public const ushort Zram_L       = 0xFF80;
        public const ushort Zram_H       = 0xFFFE;

        public const ushort Ie           = 0xFFFF;

        // Gpu
        public const ushort Lcdc = 0xFF40;
        public const ushort Stat = 0xFF41;
        public const ushort Scy  = 0xFF42;
        public const ushort Scx  = 0xFF43;
        public const ushort Ly   = 0xFF44;
        public const ushort Lyc  = 0xFF45;

        // Interrupt
        public const ushort Irr  = 0xFF0F;

        // Rendering
        public const ushort BackgroundTilesetA = 0x8000;
        public const ushort BackgroundTilesetB = 0x9000;

        public const ushort SpriteTileset = 0x8000;

        public const ushort BackgroundTilemapA = 0x9C00;
        public const ushort BackgroundTilemapB = 0x9800;

        public const ushort WindowTilemapA = 0x9C00;
        public const ushort WindowTilemapB = 0x9800;

        public const ushort ObjectTilemapA = 0xFF49;
        public const ushort ObjectTilemapB = 0xFF48;

        public const ushort BackgroundPalette = 0xFF47;

    }
}
