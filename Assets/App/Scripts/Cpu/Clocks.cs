using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        // machin cycles
        public long mc;

        // last instruction machin cycles
        public long lmc;

        // last instruction clock cycles
        public long lcc {
            get {
                return lmc * 4;
            }
        }
    }
}
