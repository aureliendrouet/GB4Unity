using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {
        // vertical blank period
        const int VblankLineBegin = 144;
        const int VblankLineEnd = 153;

        Mmu mmu;

        bool enabled;
        public bool frameRendered { get; protected set; }
        bool lineRendered;

        public Gpu (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            SetLcdMode (LcdMode.Vblank);
            ly = VblankLineBegin;
            cc = 0;
            delay = 0;
            enabled = true;
            frameRendered = false;
            lineRendered = false;
            lcdc = 0x91;
            scy = 0x00;
            scx = 0x00;
            lyc = 0x00;
            wx = 0x00;
            wy = 0x00;
        }

        public void Tick (long lcc, bool ime)
        {
            frameRendered = false;

            if (!lcdEnabled) {
                return;
            }

//            if (lcdEnabled) {
//                if (!enabled && delay == 0) {
//                    EnableLcd ();
//                }
//            } else {
//                if (enabled) {
//                    DisableLcd ();
//                }
//            }

//            if (enabled) {
                cc += lcc;

                switch (lcdMode) {

                case LcdMode.Oam:
                    TickOam ();
                    break;

                case LcdMode.Vram:
                    TickVram (ime);
                    break;

                case LcdMode.Hblank:
                    TickHblank (ime);
                    break;

                case LcdMode.Vblank:
                    TickVblank (ime);
                    break;
                }
//                CompareLyLyc ();
//            } else {
//                TickDisabled (lmc);
//            }
        }

        void TickOam ()
        {
            if (cc >= OamCycles) {
                cc -= OamCycles;
                SetLcdMode (LcdMode.Vram);
//                lineRendered = false;
            }
        }

        void TickVram (bool ime)
        {
//            if (!lineRendered && (cc >= (ly > 0 ? 12L : 40L))) {
//                lineRendered = true;
//                RenderScanline ();
//            }

            if (cc >= VramCycles) {
                cc -= VramCycles;

                RenderScanline ();

                SetLcdMode (LcdMode.Hblank);

//                DoHdma ();

                if (ime && hblankEnabled) {
                    mmu.RequestInterrupt(InterruptFlag.LcdStat);
                }
            }
        }

        void TickHblank (bool ime)
        {
            if (cc >= HblankCycles) {
                cc -= HblankCycles;

                ly++;

                CompareLyLyc (ime);

                if (ly == VblankLineBegin) {
                    frameRendered = true;

                    SetLcdMode (LcdMode.Vblank);

                    mmu.RequestInterrupt(InterruptFlag.Vblank);

                    if (ime && vblankEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                } else {
                    SetLcdMode (LcdMode.Oam);

                    if (ime && oamEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                }
            }
        }

        void TickVblank (bool ime)
        {
            if (cc >= LineCycles) {
                cc -= LineCycles;

                ly++;

                if (ly > VblankLineEnd) {
                    ly = 0;

                    SetLcdMode (LcdMode.Oam);

                    if (ime && oamEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                }
            }
        }

//        struct Hdma
//        {
//            public bool transferActive;
//            public ushort source;
//            public ushort destination;
//            public ushort length;
//        };
//
//        Hdma hdma;
//
//        void DoHdma ()
//        {
//            if (hdma.transferActive)
//            {
//                // hdma only works between this range
//                if (ly >= 0 && ly <= 143) {
//                    // transfer $10 bytes
//                    mmu.TransferDma(hdma.destination, hdma.source, 0x10);
//
//                    // advance source $10 bytes
//                    hdma.source += 0x10;
//
//                    // advance destination $10 bytes
//                    hdma.destination += 0x10;
//
//                    // count down the length
//                    hdma.length -= 0x10;
//
//                    if (hdma.length == 0) {
//                        hdma.transferActive = false;
//                    }
//                }
//            }
//        }

        // lcd is about to be enabled
//        void TickDisabled (long imc)
//        {
//            if (delay > 0) {
//                delay -= imc;
//                if (delay <= 0) {
//                    Reset ();
//                    enabled = true;
//                    cc = (-delay);
//                    delay = 0;
//                    ly = 0;
//                    SetLcdMode (LcdMode.Hblank);
//                    CompareLyLyc ();
//                }
//            } else {
//                cc += imc;
//                if (cc >= CyclesPerLine) {
//                    cc -= CyclesPerLine;
//                    frameRendered = true;
//                }
//            }
//        }

//        void EnableLcd ()
//        {
//            delay = 61;
//        }
//
//        void DisableLcd ()
//        {
//            ly = 0;
//            SetLcdMode (LcdMode.Hblank);
//            enabled = false;
//            cc = 0;
//            delay = 0;
//            enabled = false;
//            frameRendered = false;
//            lineRendered = false;
//            Clear ();
//        }
//
//        void Clear()
//        {
//            System.Array.Clear (frame, 0, frame.Length);
//        }
    }
}