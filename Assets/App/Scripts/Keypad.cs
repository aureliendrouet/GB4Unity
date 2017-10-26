using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Keypad
    {
        internal Mmu mmu;

        internal static readonly byte[] Keys = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        internal byte keys;
        internal bool joypadEnabled;
        internal bool buttonEnabled;

        internal static class KeypadFlag
        {
            internal static byte Button = 0x10;
            internal static byte Joypad = 0x20;
        }

        internal byte data;

        public Keypad (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            keys = 0x00;
        }

        public byte memory {
            get {
                if (buttonEnabled) {
                    return (byte)(0xE0 | (~keys & 0x0F));
                }
                if (joypadEnabled) {
                    return (byte)(0xD0 | (~(keys >> 4) & 0x0F));
                }
                return 0xFE;
            }
            set {
                buttonEnabled = (value & KeypadFlag.Button) == KeypadFlag.Button;
                joypadEnabled = (value & KeypadFlag.Joypad) == KeypadFlag.Joypad;
            }
        }

        public void Press (int index)
        {
            keys = (byte)(keys | Keys [index]);
            mmu.RequestInterrupt (InterruptFlag.Joypad);
        }

        public void Release (int index)
        {
            keys = (byte)(keys & ~Keys [index]);
        }
    }
}