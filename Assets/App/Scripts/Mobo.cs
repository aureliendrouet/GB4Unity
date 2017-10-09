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

        public Mobo ()
        {
            mmu = new Mmu ();
            cpu = new Cpu (mmu);
            gpu = new Gpu (mmu);
        }

        public void Reset ()
        {
            cpu.Reset ();
            gpu.Reset ();
            mmu.Reset ();
        }

        public void LoadRom (byte[] rom)
        {
            mmu.LoadRom (rom);
        }

        public void Tick (float deltaTime)
        {
            
        }
    }
}
