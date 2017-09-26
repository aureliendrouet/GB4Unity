using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        public int mc;  // machin cycle
        public int cc;  // clock cycle
        public int imc; // machin cycle of last instruction
        public int icc; // clock cycle of last instruction

        public int m {
            set {
                imc = value;
                icc = value * 4; // always equals m * 4
            }
        }
    }
}
