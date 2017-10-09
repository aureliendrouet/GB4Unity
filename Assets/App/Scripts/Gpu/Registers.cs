
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
        byte lcdc;

        enum LcdcFlag : byte
        {
            BackgroundEnabled = 0x01,
            ForegroundEnabled     = 0x02,
            TileSizeSelection = 0x04,
            BackgroundTileIndexBankSelection = 0x08,
            BackgroundTileDataBankSelection = 0x10,
            WindowEnabled     = 0x20,
            WindowTileIndexBankSelection = 0x40,
            LcdEnabled        = 0x80,
        }

        bool lcdEnabled {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.LcdEnabled);
            }
        }
        bool backgroundEnabled {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.BackgroundEnabled);
            }
        }
        bool windowEnabled {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.WindowEnabled);
            }
        }
        bool foregroundEnabled {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.ForegroundEnabled);
            }
        }

        bool backgroundTilesetSelection {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.BackgroundTileDataBankSelection);
            }
        }

        bool backgroundTilemapSelection {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.BackgroundTileIndexBankSelection);
            }
        }

        bool windowTilemapSelection {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.WindowTileIndexBankSelection);
            }
        }

        bool largeSprite {
            get {
                return HasFlag (lcdc, (byte)LcdcFlag.TileSizeSelection);
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
        byte stat;

        enum LcdMode : byte
        {
            Hblank = 0, // horizontal blank period
            Vblank = 1, // vertical blank period
            Oam    = 2, // searching sprite attribute
            Vram   = 3, // transferring data to lcd
        }

        enum StatFlag : byte
        {
            Match  = 0x04,
            Hblank = 0x08,
            Vblank = 0x10,
            Oam    = 0x20,
            LyLyc  = 0x40,
        }

        bool hblankEnabled {
            get {
                return HasFlag (stat, (byte)StatFlag.Hblank);
            }
        }

        bool vblankEnabled {
            get {
                return HasFlag (stat, (byte)StatFlag.Vblank);
            }
        }

        bool oamEnabled {
            get {
                return HasFlag (stat, (byte)StatFlag.Oam);
            }
        }

        // window y
        short wy;

        // window x
        short wx;

        // line
        byte ly;

        // line control
        byte lyc;

        // interupt request register
        byte irr;

        enum IrrFlag : byte
        {
            Vblank  = 0x01,
            LcdStat = 0x02,
        }

        #region Register Utility

        LcdMode lcdMode;

        void SetLcdMode (LcdMode mode)
        {
            this.lcdMode = mode;
            SetStatLcdMode (mode);
        }

        void SetStatLcdMode (LcdMode mode)
        {
            stat = (byte)((stat & 0xFC) | (byte)mode);
        }

        void CompareLyLyc ()
        {
            if (ly == lyc) {
                SetFlag (ref stat, (byte)StatFlag.Match);
                if (HasFlag (stat, (byte)StatFlag.LyLyc)) { 
                    SetFlag (ref irr, (byte)IrrFlag.LcdStat);
                }
            } else {
                ResetFlag (ref stat, (byte)StatFlag.Match);
            }
        }

        void SetFlag (ref byte r, byte flag)
        {
            r |= flag;
        }

        void ResetFlag (ref byte r, byte flag)
        {
            r = (byte)(r & ~flag);
        }

        bool HasFlag (byte value, byte flag)
        {
            return (value & flag) == flag;
        }

        #endregion
    }
}
