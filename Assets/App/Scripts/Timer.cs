using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Timer
    {
        Mmu mmu;

        byte divider;

        byte counter;

        // timer modulator
        byte modulator;

        // timer controller
        // --00   4096 Hz   4096 / 256 =   16 overflows per seconds
        // --01 262144 Hz 262144 / 256 = 1024 overflows per seconds262144 Hz
        // --10  65536 Hz  65536 / 256 =  256 overflows per seconds65536 Hz
        // --11  16384 Hz  16384 / 256 =   64 overflows per seconds16384 Hz 
        // -X-- enabled or disabled
        byte controller;

        // timer counter
        // gameboy frequency / timer frequency 
        long tc;

        long bc, dc;

        int[] timerFrequencies = new int[] {
            64, 
            1,  
            4,  
            16  
        };

        public static byte TimerControllerEnabledFlag = 0x04;

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

        public void Tick (long cycles)
        {
            LoadMemory ();

            tc += cycles;

            // timer ticks occur at 1/16 the CPU cycles
            while (tc >= 16) {
                tc -= 16;
                bc++;
                dc++;

                // do divider clock
                if (dc == 16) {
                    divider++;
                    dc = 0;
                }

                // only if timer is enabled
                if ((controller & TimerControllerEnabledFlag) == TimerControllerEnabledFlag) {
                    // get frequency 
                    int freq = timerFrequencies [controller & 0x03];

                    // increment counter
                    while (bc >= freq) {
                        bc -= freq;

                        // overflow!
                        if (counter == 0xFF) {
                            counter = modulator;
                            mmu.RequestInterrupt (InterruptFlag.TimeOverflow);
                        } else {
                            counter++;
                        }
                    }
                }
            }

            WriteMemory ();
        }

        void LoadMemory ()
        {
            divider = mmu.rb (Address.TimerDivider);
            counter = mmu.rb (Address.TimerCounter);
            modulator = mmu.rb (Address.TimerModulator);
            controller = mmu.rb (Address.TimerController);
        }

        void WriteMemory ()
        {
            mmu.wb (Address.TimerDivider, divider);
            mmu.wb (Address.TimerCounter, counter);
            mmu.wb (Address.TimerModulator, modulator);
            mmu.wb (Address.TimerController, controller);
        }
    }
}
