using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Keypad
    {
        Mmu mmu;

        ushort address = (ushort)(Address.Keypad - Address.Mmio_L);

        public class Key
        {
            public byte value;
            public byte mask;

            public Key (byte row, byte column)
            {
                this.value = (byte)(1 << row + 1 << column);
                this.mask = (byte)(~this.value);
            }
        }

        public static readonly Key Right = new Key (4, 0);
        public static readonly Key Left = new Key (4, 1);
        public static readonly Key Up = new Key (4, 2);
        public static readonly Key Down = new Key (4, 3);
        public static readonly Key A = new Key (5, 0);
        public static readonly Key B = new Key (5, 1);
        public static readonly Key Select = new Key (5, 2);
        public static readonly Key Start = new Key (5, 3);

        public static readonly Key[] Keys = new Key[] {
            Right, Left, Up, Down, A, B, Select, Start
        };

        public Keypad (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Press (int index)
        {
            Press (Keys [index]);
        }

        public void Press (Key key)
        {
            mmu.mmio [address] &= key.mask;
            mmu.RequestInterrupt (InterruptFlag.Joypad);
        }

        public void Release (int index)
        {
            Release (Keys [index]);
        }

        public void Release (Key key)
        {
            mmu.mmio [address] |= key.value;
        }
    }
}