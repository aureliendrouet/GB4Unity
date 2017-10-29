
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {
        // LCDC Control Register
        // Configures how object data is read and where it is read from
        //
        // 0x01 = 0000 000X
        // CGB Mode: BG display always on
        // DMG Mode: 0 BG display off, 1 BG display on
        //
        // 0x02 = 0000 00X0
        // OBJ On Flag
        // 0: off
        // 1: on
        //
        // 0x04 = 0000 0X00
        // OBJ Block Composition Selection Flag
        // 0: 8 x 8  dots
        // 1: 8 x 16 dots
        //
        // 0x08 = 0000 X000
        // BG Code Area Selection Flag
        // 0: 0x9800-0x9BFF
        // 1: 0x9C00-0x9FFF
        //
        // 0x10 = 000X 0000
        // BG Character Data Selection Flag
        // 0: 0x8800-0x97FF
        // 1: 0x8000-0x8FFF
        //
        // 0x20 = 00X0 0000
        // Windowing On Flag
        // 0: off
        // 1: on
        //
        // 0x40 = 0X00 0000
        // Window Code Area Selection Flag
        // 0: 0x9800-0x9BFF
        // 1: 0x9C00-0x9FFF
        //
        // 0x80 = X000 0000
        // LCD Controler Operation Stop Flag
        // 0: LCDC Off
        // 1: LCDC On
        internal byte lcdc;

        static class LcdcFlag
        {
            public static byte BackgroundEnabled          = 0x01;
            public static byte ObjectEnabled              = 0x02;
            public static byte TileSizeSelection          = 0x04;
            public static byte BackgroundTilemapSelection = 0x08;
            public static byte BackgroundTilesetSelection = 0x10;
            public static byte WindowEnabled              = 0x20;
            public static byte WindowTilemapSelection     = 0x40;
            public static byte LcdEnabled                 = 0x80;
        }

        public bool lcdEnabled {
            get {
                return (lcdc & LcdcFlag.LcdEnabled) == LcdcFlag.LcdEnabled;
            }
        }
        public bool backgroundEnabled {
            get {
                return (lcdc & LcdcFlag.BackgroundEnabled) == LcdcFlag.BackgroundEnabled;
            }
        }
        public bool windowEnabled {
            get {
                return (lcdc & LcdcFlag.WindowEnabled) == LcdcFlag.WindowEnabled;
            }
        }
        public bool objectEnabled {
            get {
                return (lcdc & LcdcFlag.ObjectEnabled) == LcdcFlag.ObjectEnabled;
            }
        }

        public bool backgroundTilesetSelection {
            get {
                return (lcdc & LcdcFlag.BackgroundTilesetSelection) == LcdcFlag.BackgroundTilesetSelection;
            }
        }

        public bool backgroundTilemapSelection {
            get {
                return (lcdc & LcdcFlag.BackgroundTilemapSelection) == LcdcFlag.BackgroundTilemapSelection;
            }
        }

        public bool windowTilemapSelection {
            get {
                return (lcdc & LcdcFlag.WindowTilemapSelection) == LcdcFlag.WindowTilemapSelection;
            }
        }

        public bool largeTile {
            get {
                return (lcdc & LcdcFlag.TileSizeSelection) == LcdcFlag.TileSizeSelection;
            }
        }

        // LCD controller status and interrupt configuration
        //
        // 0000 00XX
        // Mode Flag
        // 00: Enable CPU access to all display RAM
        // 01: in vertical blanking period
        // 10: Searching OAM RAM
        // 11: Transferrning data to the LCD driver
        //
        // 0000 0X00
        // Match Flag
        // 0: LYC = LCDC LY
        // 1: LYC = LCDC LY
        //
        // Interrupt selection according to LCD status
        // 0000 X000 H-Blank Interrupt
        // 000X 0000 V-Blank Interrupt
        // 00X0 0000 OAM Interrupt
        // 0X00 0000 LYC = LY Interupt
        internal byte stat;

        public enum LcdMode : byte
        {
            Hblank = 0, // horizontal blank period
            Vblank = 1, // vertical blank period
            Oam    = 2, // searching sprite attribute
            Vram   = 3, // transferring data to lcd
        }

        public static class StatFlag
        {
            public static byte Match  = 0x04;
            public static byte Hblank = 0x08;
            public static byte Vblank = 0x10;
            public static byte Oam    = 0x20;
            public static byte LyLyc  = 0x40;
        }

        public bool matched {
            get {
                return (stat & StatFlag.Match) == StatFlag.Match;
            }
            set {
                if (value) {
                    stat = (byte)(stat | StatFlag.Match);
                } else {
                    stat = (byte)(stat & ~StatFlag.Match);
                }
            }
        }

        public bool hblankEnabled {
            get {
                return (stat & StatFlag.Hblank) == StatFlag.Hblank;
            }
        }

        public bool vblankEnabled {
            get {
                return (stat & StatFlag.Vblank) == StatFlag.Vblank;
            }
        }

        public bool oamEnabled {
            get {
                return (stat & StatFlag.Oam) == StatFlag.Oam;
            }
        }

        public bool lyLycEnabled {
            get {
                return (stat & StatFlag.LyLyc) == StatFlag.LyLyc;
            }
        }

        // scroll y
        // the y position of the background where to start drawing the viewing area from 
        internal byte scy;

        // scroll x
        // the x position of the background to start drawing the viewing area from
        internal byte scx;

        // line
        internal byte ly;

        // line control
        internal byte lyc;

        // window y
        // the y Position of the viewing area to start drawing the window from
        internal byte wy;

        // window x
        // the x Position of the viewing area to start drawing the window from
        internal byte wx;

        // background palette
        internal byte bgp;

        // object palette 0
        internal byte obp0;

        // object palette 1
        internal byte obp1;

        #region Register Utility

        public LcdMode lcdMode;

        void SetLcdMode (LcdMode mode)
        {
            this.lcdMode = mode;
            SetStatLcdMode (mode);
        }

        void SetStatLcdMode (LcdMode mode)
        {
            stat = (byte)((stat & 0xFC) | (byte)mode);
        }

        void CompareLyLyc (bool ime)
        {
            if (ly == lyc) {
                matched = true;
            } else {
                matched = false;
            }
            if (ime && lyLycEnabled) { 
                mmu.RequestInterrupt(InterruptFlag.LcdStat);
            }
        }

        #endregion
    }
}
