using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Mobo
    {
        public Cpu cpu;
        public Mmu mmu;
        public Gpu gpu;
        public Audio audio;
        public Timer timer;

        public Mobo ()
        {
            mmu = new Mmu ();
            cpu = new Cpu (mmu);
            gpu = new Gpu (mmu);
            audio = new Audio (mmu);
            timer = new Timer (mmu);
        }

        public void Reset ()
        {
            cpu.Reset ();
            mmu.Reset ();
            audio.Reset ();
            gpu.Reset ();
            timer.Reset ();
        }

        public void LoadRom (byte[] rom)
        {
            mmu.LoadRom (rom);
        }

        public long Tick ()
        {
            cpu.ExecNextOpcode ();
            audio.Tick (cpu.imc);
            gpu.Tick (cpu.imc);
            timer.Tick (cpu.imc);
            return cpu.imc;
        }
    }
}
