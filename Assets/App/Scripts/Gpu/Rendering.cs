using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {

        #region Texture

        Texture2D texture;
        Color[] defaultTextureColors = Enumerable.Repeat (Color.clear, WindowWidth * WindowHeight).ToArray ();

        public void Render ()
        {
            texture.Apply ();
        }

        public void Clear ()
        {
            texture.SetPixels (defaultTextureColors);
        }

        #endregion

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
        Color[] userPalette = new Color[4];

        const int ScreenWidth = 256;
        const int ScreenHeight = 256;

        const byte WindowWidth = 160;
        const byte WindowHeight = 144;

        const int ColorCount = 4;
        const int TilesPerRow = 32;
        const int BitsPerByte = 8;
        const int BitsPerAddress = 16;

        const int SpriteDataSize = 0x04;

        const int MaxSpriteCount = 40;

        Color[] backgroundPalette = new Color[4];
        Color[] spritePalette = new Color[4];

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
                    RenderSprites ();
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
                texture.SetPixel (lx, ly, backgroundPalette [colorIndex]);
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
                texture.SetPixel (lx, ly, backgroundPalette [colorIndex]);
            }
        }

        void RenderSprites ()
        {
            int spriteWidth = 8;
            int spriteHeight = (largeSprite ? 16 : 8);

            for (int spriteCounter = MaxSpriteCount - 1; spriteCounter >= 0; --spriteCounter) {

                var sa = ReadSpriteAttributes (spriteCounter, spriteWidth, spriteHeight);

                // check if the sprite is not on the screen
                if (sa.x >= WindowWidth || sa.x <= -spriteWidth || sa.y >= WindowHeight || sa.y <= -spriteHeight) {
                    continue;
                }

                byte tileY = (byte)(ly - sa.y);

                // check if it is not on the current ly
                if (sa.y > (short)ly || tileY >= spriteWidth) {
                    continue;
                }

                // update palette
                byte palette = mmu.rb (sa.paletteSelection ? Address.SpriteTilemapA : Address.SpriteTilemapB);

                for (int i = 0; i < 4; ++i) {
                    spritePalette [i] = userPalette [(palette >> (i * 2)) & 0x3];
                }

                // find tile address
                ushort spriteTileAddress;

                if (largeSprite) {
                    if (sa.flipY) {
                        tileY = (byte)(15 - tileY);
                    }

                    // check which 8x8 tile has to be used
                    if (tileY < 8) {
                        spriteTileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * (sa.tileIndex & 0xFE)); // Upper tile, ignore LSB
                    } else {
                        spriteTileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * (sa.tileIndex | 0x01)); // Lower tile, set LSB
                        tileY -= 8;
                    }
                } else {
                    if (sa.flipY) {
                        tileY = (byte)(7 - tileY);
                    }

                    spriteTileAddress = (ushort)(Address.SpriteTileset + BitsPerAddress * sa.tileIndex);
                }

                for (int x = 0; x < spriteWidth; ++x) {
                    if (sa.x + x < 0 || sa.x + x >= WindowWidth) {
                        continue;
                    }

                    byte pixel = ReadTile (spriteTileAddress, (byte)(sa.flipX ? (7 - x) : x), tileY); // 0 = transparency

                    if (pixel > 0 && (sa.priority || (texture.GetPixel (sa.x + x, ly) == backgroundPalette [0]))) {
                        texture.SetPixel (sa.x + x, ly, spritePalette [pixel]);
                    }
                }
            }
        }

        #endregion

        #region Sprite Attributes

        // Sprite attributes are read from OAM
        // Used for drawing sprites or objects (OBJ)
        // There can be up to 40 OBJs
        // OAM RAM $FE00 - $FE9F
        // 10 OBJs can be displayed on the same Y line
        // Display data for OBH characters is stored in OAM $FE00 - $FE9F
        // y-axis coordinate (1 byte)
        // x-axis coordinate (1 byte)
        // character code - tile number (1 byte)
        // attribute data (1 byte)
        struct SpriteAttributes
        {
            public short y;
            public short x;
            public byte tileIndex;
            public bool priority;
            public bool flipX;
            public bool flipY;
            public bool paletteSelection;
        }

        const byte Offset_Sprite_Y             = 0x00;
        const byte Offset_Sprite_X             = 0x01;
        const byte Offset_Sprite_TileIndex     = 0x02;
        const byte Offset_Sprite_AttributeData = 0x03;

        enum SpriteAttributeFlag : byte
        {
            PaletteSelection = 0x10,
            FlipX = 0x20,
            FlipY = 0x40,
            Priority = 0x80,
        }

        SpriteAttributes ReadSpriteAttributes (int spriteCounter, int spriteWidth, int spriteHeight)
        {
            var sa = new SpriteAttributes ();

            ushort spriteDataIndex = (ushort)(Address.Oam + SpriteDataSize * spriteCounter);

            sa.y = (short)(mmu.rb (spriteDataIndex + Offset_Sprite_Y) - spriteHeight);
            sa.x = (short)(mmu.rb (spriteDataIndex + Offset_Sprite_X) - spriteWidth);

            sa.tileIndex = mmu.rb (spriteDataIndex + Offset_Sprite_TileIndex);

            // attribute data
            byte attributeData = mmu.rb (spriteDataIndex + Offset_Sprite_AttributeData);

            sa.paletteSelection = !HasFlag (attributeData, (byte)SpriteAttributeFlag.PaletteSelection);
            sa.flipX            =  HasFlag (attributeData, (byte)SpriteAttributeFlag.FlipX);
            sa.flipY            =  HasFlag (attributeData, (byte)SpriteAttributeFlag.FlipY);
            sa.priority         =  HasFlag (attributeData, (byte)SpriteAttributeFlag.Priority);

            return sa;
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
