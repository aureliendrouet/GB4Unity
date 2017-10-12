using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {
        #region Scanline
        // Region      Usage
        // 8000-87FF   Tile set #A: tiles   0 to  127
        // 8800-8FFF   Tile set #A: tiles 128 to  255
        //             Tile set #B: tiles  -1 to -128
        // 9000-97FF   Tile set #B: tiles   0 to  127
        // 9800-9BFF   Tile map #A
        // 9C00-9FFF   Tile map #B
        //
        // 4 colors = 2 bits
        //
        // tile type A = 8 x 8 pixels
        // 08 pixels x 08 pixels x 02 bits (color) = 128 bits or 16 bytes
        //
        // tile type B = 8 x 16 pixels
        // 08 pixels x 16 pixels x 02 bits (color) = 256 bits or 32 bytes
        //
        // background 256 x 256 pixels
        // 32 tiles x 32 tiles
        // scolling coordinates (wx, wy)
        //
        // palette 4 colors x 2 bits = 8 bits or 1 bytes
        //  [7-6]     [5-4]     [3-2]     [1-0]
        // color 3 - color 2 - color 1 - color 0
        //
        public byte[] frame = new byte[WindowHeight * WindowWidth];
        byte[] userPalette = new byte[4] { 0, 1,  2, 3 };

        const int ScreenWidth = 256;
        const int ScreenHeight = 256;

        public const byte WindowWidth = 160;
        public const byte WindowHeight = 144;

        const int ColorCount = 4;
        const int TilesPerRow = 32;
        const int BitsPerByte = 8;
        const int BitsPerAddress = 16;


        byte[] backgroundPalette = new byte[4];

        void RenderScanline ()
        {
            if (lcdEnabled) {
                if (backgroundEnabled) {
                    RenderBackground ();
                }

                if (windowEnabled) {
                    RenderWindow ();
                }

                if (foregroundEnabled) {
                    RenderObjects ();
                }
            }
        }

        void UpdateBackgroundPalette ()
        {
            byte palette = mmu.rb (Address.BackgroundPalette);

            for (int i = 0; i < ColorCount; ++i) {
                backgroundPalette [i] = userPalette [(palette >> (i * 2)) & 0x3];
            }
        }

        void RenderBackground ()
        {
            UpdateBackgroundPalette ();

            ushort backgroundTilemapAddress;

            if (backgroundTilemapSelection) {
                backgroundTilemapAddress = Address.BackgroundTilemapA;
            } else {
                backgroundTilemapAddress = Address.BackgroundTilemapB;
            }

            ushort backgroundTilesetAddress;

            if (backgroundTilesetSelection) {
                backgroundTilesetAddress = Address.BackgroundTilesetA;
            } else {
                backgroundTilesetAddress = Address.BackgroundTilesetB;
            }

            byte tileX, tileY;
            ushort tileRow, tileColumn;
            ushort tileIndexAddress, tileAddress;
            int tileIndex;
            byte colorIndex;

            tileRow = (ushort)(((wy + ly) % ScreenHeight) / BitsPerByte);
            tileY = (byte)(((wy + ly) % ScreenHeight) % BitsPerByte);

            int offset = ly * WindowWidth;

            for (int lx = 0; lx < WindowWidth; ++lx) {
                tileColumn = (ushort)(((wx + lx) % ScreenWidth) / BitsPerByte);
                tileX = (byte)(((wx + lx) % ScreenWidth) % BitsPerByte);

                // find tile index
                tileIndexAddress = (ushort)(backgroundTilemapAddress + tileRow * TilesPerRow + tileColumn);

                if (backgroundTilesetAddress == Address.BackgroundTilesetA) {
                    tileIndex = mmu.rb (tileIndexAddress);
                } else {
                    tileIndex = (sbyte)(mmu.rb (tileIndexAddress));
                }

                // find tile address
                tileAddress = (ushort)(backgroundTilesetAddress + tileIndex * BitsPerAddress);

                // read tile color
                colorIndex = ReadTile (tileAddress, tileX, tileY);

                // update texture
                frame [offset + lx] = backgroundPalette [colorIndex];
            }
        }

        void RenderWindow ()
        {
            // check if the window is displayed
            if (wy < 0 || wy >= WindowHeight || wx < -7 || wx >= WindowWidth) {
                return;
            }

            // check if the window is on the current ly
            if (wy > (short)(ly)) {
                return;
            }

            UpdateBackgroundPalette ();

            ushort windowTilemapAddress;

            if (windowTilemapSelection) {
                windowTilemapAddress = Address.WindowTilemapA;
            } else {
                windowTilemapAddress = Address.WindowTilemapB;
            }

            ushort windowTilesetAddress;

            if (backgroundTilesetSelection) {
                windowTilesetAddress = Address.BackgroundTilesetA;
            } else {
                windowTilesetAddress = Address.BackgroundTilesetB;
            }

            byte tileX, tileY;
            ushort tileRow, tileColumn;
            ushort tileIndexAddress, tileAddress;
            int tileIndex;
            byte colorIndex;

            tileRow = (ushort)((ly - wy) / BitsPerByte);
            tileY = (byte)((ly - wy) % BitsPerByte);

            int offset = ly * WindowWidth;

            for (int lx = (wx < 0) ? 0 : wx; lx < WindowWidth; ++lx) {
                tileColumn = (ushort)((lx - wx) / BitsPerByte);
                tileX = (byte)((lx - wx) % BitsPerByte);

                // find tile index
                tileIndexAddress = (ushort)(windowTilemapAddress + tileRow * TilesPerRow + tileColumn);

                if (windowTilesetAddress == Address.BackgroundTilesetA) {
                    tileIndex = mmu.rb (tileIndexAddress);
                } else {
                    tileIndex = (sbyte)(mmu.rb (tileIndexAddress));
                }

                // find tile address
                tileAddress = (ushort)(windowTilesetAddress + tileIndex * BitsPerAddress);

                // read tile color
                colorIndex = ReadTile (tileAddress, tileX, tileY);

                // update texture
                frame [offset + lx] = backgroundPalette [colorIndex];
            }
        }

        #endregion

        #region Foreground
        // Object attributes are read from OAM
        // Used for drawing sprites or objects (OBJ)
        // There can be up to 40 OBJs
        // OAM RAM $FE00 - $FE9F
        // 10 OBJs can be displayed on the same Y line
        // Display data for OBH characters is stored in OAM $FE00 - $FE9F
        // - y-axis coordinate (1 byte)
        // - x-axis coordinate (1 byte)
        // - character code (1 byte)
        // - attribute data (1 byte)
        byte[] objectPalette = new byte[4];

        const int ObjectDataSize = 0x04;
        const int MaxObjectCount = 40;

        struct ObjectAttributes
        {
            public short y;
            public short x;
            public byte characterCode;
            public bool paletteSelection; 
            public bool flipX;
            public bool flipY;
            public bool priority;
        }

        enum ObjectAttributeFlag : byte
        {
            PaletteSelection = 0x10, // palette selection
            FlipX            = 0x20, // horizontal flip flag
            FlipY            = 0x40, // vertical flip flag
            Priority         = 0x80, // display priority (0: priority to OBJ, 1: priority to BG)
        }

        ObjectAttributes ReadObjectAttributes (int counter, int width, int height)
        {
            var oa = new ObjectAttributes ();

            ushort index = (ushort)(Address.Oam_L + ObjectDataSize * counter);

            oa.y = (short)(mmu.rb (index + 0x00) - height);
            oa.x = (short)(mmu.rb (index + 0x01) - width);

            oa.characterCode = mmu.rb (index + 0x02);

            byte attributeData = mmu.rb (index + 0x03);

            oa.paletteSelection = HasFlag (attributeData, (byte)ObjectAttributeFlag.PaletteSelection);
            oa.flipX            = HasFlag (attributeData, (byte)ObjectAttributeFlag.FlipX);
            oa.flipY            = HasFlag (attributeData, (byte)ObjectAttributeFlag.FlipY);
            oa.priority         = HasFlag (attributeData, (byte)ObjectAttributeFlag.Priority);

            return oa;
        }

        void RenderObjects ()
        {
            int width = 8;
            int height = (largeTile ? 16 : 8);

            for (int counter = MaxObjectCount - 1; counter >= 0; --counter) {
                // read oam
                var oa = ReadObjectAttributes (counter, width, height);

                // out of screen
                if (oa.x >= WindowWidth || oa.x <= -width || oa.y >= WindowHeight || oa.y <= -height) {
                    continue;
                }

                byte tileY = (byte)(ly - oa.y);

                // not on current line
                if (oa.y > (short)ly || tileY >= width) {
                    continue;
                }

                // update palette
                byte palette = mmu.rb (oa.paletteSelection ? Address.ObjectTilemapB : Address.ObjectTilemapA);

                for (int i = 0; i < 4; ++i) {
                    objectPalette [i] = userPalette [(palette >> (i * 2)) & 0x3];
                }

                // find tile address
                ushort tileAddress;

                if (largeTile) {
                    if (oa.flipY) {
                        tileY = (byte)(15 - tileY);
                    }

                    // check which 8x8 tile has to be used
                    if (tileY < 8) {
                        tileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * (oa.characterCode & 0xFE)); // upper tile, ignore lsb
                    } else {
                        tileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * (oa.characterCode | 0x01)); // lower tile, set lsb
                        tileY -= 8;
                    }
                } else {
                    if (oa.flipY) {
                        tileY = (byte)(7 - tileY);
                    }

                    tileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * oa.characterCode);
                }

                int offset = ly * WindowWidth;

                // render the line...
                for (int x = 0; x < width; ++x) {
                    // skip if out of window
                    if (oa.x + x < 0 || oa.x + x >= WindowWidth) {
                        continue;
                    }

                    // flip?
                    byte tileX = (byte)(oa.flipX ? (7 - x) : x);

                    // read vram
                    byte pixel = ReadTile (tileAddress, tileX, tileY);

                    // skip if pixel is transparent
                    if (pixel == 0) {
                        continue;
                    }

                    // write if priority or background is transparen
                    bool backgroundTransparent = frame[offset + oa.x + x] == backgroundPalette [0];

                    if (oa.priority || backgroundTransparent) {
                        frame[offset + oa.x + x] = objectPalette [pixel];
                    }
                }
            }
        }

        #endregion

        byte ReadTile (ushort address, byte x, byte y)
        {
            byte lsBits = mmu.rb (address + (y * 2));
            byte msBits = mmu.rb (address + (y * 2) + 1);

            byte lsb = (byte)((lsBits >> (7 - x)) & 0x01);
            byte msb = (byte)((msBits >> (7 - x)) & 0x01);
            byte col = (byte)((msb << 1) | lsb);

            return col;
        }
    }
}
