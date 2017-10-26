using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Timer
    {
        Mmu mmu;

        // divider timer is permanently set to increment at 16384 Hz
        // 1/16th of the timer base speed
        // go back to zero after the overflow
        internal byte divider;

        // counter" timer is programmable
        // it can be set to one of four speeds (the base divided by 1, 4, 16 or 64
        // go back to modulator value after the overflow
        // send an interrupt when it overflows
        internal byte counter;

        // timer modulator
        internal byte modulator;

        // timer controller
        // --XX speed
        // -X-- enabled or disabled
        //
        // Speed
        // 00:   4096 Hz
        // 01: 262144 Hz (base speed)
        // 10:  65536 Hz
        // 11:  16384 Hz
        //
        // 4194304 Hz /   4096 Hz / 16 (base) = 64
        // 4194304 Hz / 262144 Hz / 16 (base) = 1
        // 4194304 Hz /  65536 Hz / 16 (base) = 4
        // 4194304 Hz /  16384 Hz / 16 (base) = 16
        internal byte controller;

        public static byte TimerControllerEnabledFlag = 0x04;

        // t-clock (clock cycles)
        long tc;

        // base clock
        long bc;

        // div clock
        long dc;

        // ticks per second at base speed
        const int TicksPerSecondsAtBaseSpeed = 16;

        int[] timerFrequencies = new int[] {
            64, 
            1,  
            4,  
            16  
        };

        public Timer (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            divider = 0;
            counter = 0;
            modulator = 0;
            controller = 0;
            tc = 0;
            bc = 0;
            dc = 0;
        }

        public void Tick (long cc)
        {
            // m-clock increments at 1/4 the m-clock rate
            tc += cc;

            // timer ticks occur at 1/16 the CPU cycles
            while (tc >= TicksPerSecondsAtBaseSpeed) {
                tc -= TicksPerSecondsAtBaseSpeed;
                bc++;
                dc++;

                // divider clock
                if (dc == TicksPerSecondsAtBaseSpeed) {
                    divider++;
                    dc = 0;
                }

                // check if timer is enabled
                if ((controller & TimerControllerEnabledFlag) == TimerControllerEnabledFlag) {
                    // get frequency 
                    int threshold = timerFrequencies [controller & 0x03];

                    while (bc >= threshold) {
                        bc -= threshold;

                        if (counter == 0xFF) {
                            // overflow!
                            counter = modulator;
                            mmu.RequestInterrupt (InterruptFlag.TimeOverflow);
                        } else {
                            // increment counter
                            counter++;
                        }
                    }
                }
            }
        }
    }
}
