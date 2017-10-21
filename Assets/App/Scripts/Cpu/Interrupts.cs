using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        // interrupt master enable
        public bool ime;

        // disable interrupt pending count
        int diPendingCount;

        // enable interrupt pending count
        int eiPendingCount;

        // halt
        public bool hlt;

        // stop
        public bool stp;

        public void CheckInterrupts ()
        {
            // mask off disabled interrupts
            byte flags = (byte)(mmu.ir & mmu.ie);

            // check power mode
            if (!stp) {
                if (flags > 0) {
                    hlt = false;
                }
            } else {
                if ((flags & InterruptFlag.Joypad) == InterruptFlag.Joypad) {
                    stp = false;
                    hlt = false;
                }
            }

            // when EI or DI is used to change the IME
            // the change takes effect after the next instruction is executed
            if (diPendingCount >= 0) {
                diPendingCount++;
                if (diPendingCount == 2) {
                    ime = false;
                    diPendingCount = -1;
                }
            }

            if (eiPendingCount >= 0) {
                eiPendingCount++;
                if (eiPendingCount == 2) {
                    ime = true;
                    eiPendingCount = -1;
                }
            }

            if (ime) {
                if ((flags & InterruptFlag.Joypad) == InterruptFlag.Joypad) {
                    Interrupt (Address.InterruptHandler_Joypad, InterruptFlag.Joypad);
                }

                if ((flags & InterruptFlag.SerialTransferComplete) == InterruptFlag.SerialTransferComplete) {
                    Interrupt (Address.InterruptHandler_SerialTransferComplete, InterruptFlag.SerialTransferComplete);
                }

                if ((flags & InterruptFlag.TimeOverflow) == InterruptFlag.TimeOverflow) {
                    Interrupt (Address.InterruptHandler_TimeOverflow, InterruptFlag.TimeOverflow);
                }

                if ((flags & InterruptFlag.LcdStat) == InterruptFlag.LcdStat) {
                    Interrupt (Address.InterruptHandler_LcdcStat, InterruptFlag.LcdStat);
                }

                if ((flags & InterruptFlag.Vblank) == InterruptFlag.Vblank) {
                    Interrupt (Address.InterruptHandler_Vblank, InterruptFlag.Vblank);
                }
            }
        }

        void Interrupt (ushort address, byte flag)
        {
            // keep pc to return later
            pushw (pc);

            // jump to interupt service routine
            pc = address;

            // reset flag
            mmu.ir = (byte)(mmu.ir & ~flag);

            // disable
            ime = false;

            // update clock
            lmc += 5;
        }
    }
}
