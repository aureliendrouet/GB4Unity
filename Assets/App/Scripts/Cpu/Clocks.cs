using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        public long mc;  // machin cycle
        public long cc;  // clock cycle
        public long imc; // machin cycle of last instruction
        public long icc; // clock cycle of last instruction

        public long m {
            set {
                imc = value;
                icc = value * 4; // always equals m * 4
            }
        }
    }
}
