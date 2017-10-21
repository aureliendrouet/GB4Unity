﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Mobo
    {
        public Cpu cpu;
        public Mmu mmu;
        public Gpu gpu;
        public Apu apu;
        public Timer timer;

        public Mobo ()
        {
            mmu = new Mmu ();
            cpu = new Cpu (mmu);
            gpu = new Gpu (mmu);
            apu = new Apu (mmu);
            timer = new Timer (mmu);
        }

        public void Reset ()
        {
            cpu.Reset ();
            mmu.Reset ();
            apu.Reset ();
            gpu.Reset ();
            timer.Reset ();
        }

        public void LoadRom (byte[] rom)
        {
            mmu.LoadRom (rom);
        }

        public long Tick ()
        {
            if (!cpu.hlt) {
                cpu.ExecNextOpcode ();
            } else {
                cpu.lmc = 1;
            }

            cpu.CheckInterrupts ();

            cpu.mc += cpu.lmc;
            long cc = cpu.lcc;

            if (!cpu.stp) {
                apu.Tick (cc);
                gpu.Tick (cc, cpu.ime);
                timer.Tick (cc);
            }

            return cc;
        }
    }
}
