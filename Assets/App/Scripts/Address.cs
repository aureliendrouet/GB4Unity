
namespace StudioKurage.Emulator.Gameboy
{
    public static class Address
    {
        // Cartridge
        public const ushort CartridgeType = 0x0147;
        public const ushort RomSize = 0x0148;
        public const ushort RamSize = 0x0149;

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
        public const ushort BackgroundPalette = 0xFF47;

        public const ushort WindowTilemapA = 0x9C00;
        public const ushort WindowTilemapB = 0x9800;

        public const ushort BackgroundTilesetA = 0x8000;
        public const ushort BackgroundTilesetB = 0x9000;

        public const ushort BackgroundTilemapA = 0x9C00;
        public const ushort BackgroundTilemapB = 0x9800;

        public const ushort SpriteTilemapA = 0xFF49;
        public const ushort SpriteTilemapB = 0xFF48;

        public const ushort SpriteTileset = 0x8000;

        public const ushort Oam = 0xFE00;
    }
}
