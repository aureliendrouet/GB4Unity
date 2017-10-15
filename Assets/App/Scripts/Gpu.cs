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
            mc = 0;
            delay = 0;
            enabled = true;
            frameRendered = false;
            lineRendered = false;
        }

        public void Tick (long imc)
        {
            LoadMemory ();

            frameRendered = false;

            if (lcdEnabled) {
                if (!enabled && delay == 0) {
                    EnableLcd ();
                }
            } else {
                if (enabled) {
                    DisableLcd ();
                }
            }

            if (enabled) {
                mc += imc;

                switch (lcdMode) {

                case LcdMode.Oam:
                    TickOam ();
                    break;

                case LcdMode.Vram:
                    TickVram ();
                    break;

                case LcdMode.Hblank:
                    TickHblank ();
                    break;

                case LcdMode.Vblank:
                    TickVblank ();
                    break;
                }
                CompareLyLyc ();
            } else {
                TickDisabled (imc);
            }

            WriteMemory ();
        }

        void LoadMemory ()
        {
            lcdc = mmu.rb (Address.Lcdc);
            stat = mmu.rb (Address.Stat);
            scy = (short)mmu.rb (Address.Scy);
            scx = (short)mmu.rb (Address.Scx);
            ly = mmu.rb (Address.Ly);
            lyc = mmu.rb (Address.Lyc);
            wy = (short)mmu.rb (Address.Wy);
            wx = (short)mmu.rb (Address.Wx);
        }

        void WriteMemory ()
        {
            mmu.wb (Address.Lcdc, lcdc);
            mmu.wb (Address.Stat, stat);
            mmu.wb (Address.Ly, ly);
            mmu.wb (Address.Lyc, lyc);
        }

        void TickOam ()
        {
            if (mc >= MachinCyclesPerOam) {
                mc -= MachinCyclesPerOam;
                SetLcdMode (LcdMode.Vram);
                lineRendered = false;
            }
        }

        void TickVram ()
        {
            if (!lineRendered && (mc >= (ly > 0 ? 12L : 40L))) {
                lineRendered = true;
                RenderScanline ();
            }

            if (mc >= MachinCyclesPerVram) {
                mc -= MachinCyclesPerVram;

                SetLcdMode (LcdMode.Hblank);
                if (hblankEnabled) {
                    mmu.RequestInterrupt(InterruptFlag.LcdStat);
                }
            }
        }

        void TickHblank ()
        {
            if (mc >= MachinCyclesPerHblank) {
                mc -= MachinCyclesPerHblank;
                ly++;

                if (ly == VblankLineBegin) {
                    frameRendered = true;

                    SetLcdMode (LcdMode.Vblank);
                    if (vblankEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                    mmu.RequestInterrupt(InterruptFlag.Vblank);
                } else {
                    SetLcdMode (LcdMode.Oam);
                    if (oamEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                }
            }
        }

        void TickVblank ()
        {
            if (mc >= MachinCyclesPerVblank) {
                mc -= MachinCyclesPerVblank;
                ly++;

                if (ly > VblankLineEnd) {
                    ly = 0;

                    SetLcdMode (LcdMode.Oam);
                    if (oamEnabled) {
                        mmu.RequestInterrupt(InterruptFlag.LcdStat);
                    }
                }
            }
        }

        // lcd is about to be enabled
        void TickDisabled (long imc)
        {
            if (delay > 0) {
                delay -= imc;
                if (delay <= 0) {
                    Reset ();
                    enabled = true;
                    mc = (-delay);
                    delay = 0;
                    ly = 0;
                    SetLcdMode (LcdMode.Hblank);
                    CompareLyLyc ();
                }
            } else {
                mc += imc;
                if (mc >= CyclesPerFrame) {
                    mc -= CyclesPerFrame;
                    frameRendered = true;
                }
            }
        }

        void EnableLcd ()
        {
            delay = 61;
        }

        void DisableLcd ()
        {
            ly = 0;
            SetLcdMode (LcdMode.Hblank);
            enabled = false;
            mc = 0;
            delay = 0;
            enabled = false;
            frameRendered = false;
            lineRendered = false;
            Clear ();
        }

        void Clear()
        {
            System.Array.Clear (frame, 0, frame.Length);
        }
    }
}