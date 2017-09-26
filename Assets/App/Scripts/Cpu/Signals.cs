using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        public int ime; // interrupt master enable
        public int hlt; // halt
        public int stp; // stop
    }
}
