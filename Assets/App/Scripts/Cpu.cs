using System;
using System.Collections;

namespace StudioKurage.Emulator.Gameboy
{
    // central processing unit (sharp LR35902)
    public partial class Cpu
    {
        // memory management unit
        public Mmu mmu;

        public Cpu (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            // registers
            af = 0x01B0;
            bc = 0x0013;
            de = 0x00D8;
            hl = 0x014D;
            pc = (ushort)(mmu.biosActive ? 0x0000 : 0x0100);
            sp = 0xFFFE;
            // clocks
            mc = 0;
            lmc = 0;
            // interrupts
            ime = false;
            diPendingCount = -1;
            eiPendingCount = -1;
            stp = false;
            hlt = false;
        }

        public ushort ReadOpcode ()
        {
            return mmu.rw (pc);
        }

        public void ExecNextOpcode ()
        {
            byte opcode = mmu.rb (pc++);
            ExecOpcode (opcode);
        }

        Instruction[] instructions;

        public void ExecOpcode (byte opcode)
        {
            if (opcode == 0xCB) {
                timing = cbtiming;
                instructions = cbmap;
                opcode = mmu.rb (pc++);
            } else {
                timing = atiming;
                instructions = map;
            }

            instructions[opcode] (this);
            lmc = timing[opcode];
            mc += lmc;

            if (mmu.biosActive && pc == 0x0100) {
                mmu.biosActive = false;
            }
        }

        public ushort opcode {
            get {
                return mmu.rw (pc);
            }
        }

        delegate void Instruction (Cpu Cpu);
    }
}
