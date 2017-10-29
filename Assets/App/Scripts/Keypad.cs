using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Keypad
    {
        internal Mmu mmu;

        internal byte memory;

        internal bool[] buttonKeys;
        internal bool[] joypadKeys;

        internal static class KeypadFlag
        {
            internal static byte Joypad = 0x10;
            internal static byte Button = 0x20;
        }

        public Keypad (Mmu mmu)
        {
            this.mmu = mmu;
        }

        public void Reset ()
        {
            buttonKeys = new bool[4];
            joypadKeys = new bool[4];
        }

        public void Tick()
        {
            byte flag;
            if ((memory & KeypadFlag.Button) == 0) {
                for (int i = 0; i < 4; i++) {
                    flag = (byte)(1 << i);

                    if (buttonKeys [i]) {
                        // pressed
                        if ((memory & flag) == flag) {
                            mmu.RequestInterrupt (InterruptFlag.Joypad);
                        }
                        memory &= (byte)~flag;
                    } else {
                        // released
                        memory |= flag;
                    }
                }
            } else if ((memory & KeypadFlag.Joypad) == 0) {
                for (int i = 0; i < 4; i++) {
                    flag = (byte)(1 << i);

                    if (joypadKeys [i]) {
                        // pressed
                        if ((memory & flag) == flag) {
                            mmu.RequestInterrupt (InterruptFlag.Joypad);
                        }
                        memory &= (byte)~flag;
                    } else {
                        // released
                        memory |= flag;
                    }
                }
            }
        }
            
            public void PressJoypad (int index)
            {
                joypadKeys [index] = true;
            }

            public void ReleaseJoypad (int index)
            {
                joypadKeys [index] = false;
            }

            public void PressButton (int index)
            {
                buttonKeys [index] = true;
            }

            public void ReleaseButton (int index)
            {
                buttonKeys [index] = false;
            }

//        internal bool joypadEnabled;
//        internal bool buttonEnabled;

//        public byte memory {
//            get {
////                byte res = (byte)(data ^ 0xFF);
//
////                if ((res & KeypadFlag.Button) == 0) {
//                if (buttonEnabled) {
////                    return (byte)(res & (~keys & 0x0F));
//                    return (byte)(0xE0 | (~keys & 0x0F));
//                }
////                if ((res & KeypadFlag.Joypad) == 0) {
//                if (joypadEnabled) {
//                    return (byte)(0xD0 | ((~keys >> 4) & 0x0F));
////                    return (byte)(res & ((~keys >> 4) & 0x0F));
//                }
//                return 0xFF;
//            }
//            set {
//                Debug.Log ("WRITE " + System.Convert.ToString (value, 2).PadLeft(8, '0'));
//                buttonEnabled = (value & KeypadFlag.Button) == 0;
//                joypadEnabled = (value & KeypadFlag.Joypad) == 0;
//            }
                    //        }

//
//        public void PressJoypad (int index)
//        {
//            joypadKeys [index] = true;
////            byte flag = (byte)(1 << index);
////
////            bool alreadyPressed = (keys & flag) == flag;
////
////            keys = (byte)(keys | flag);
////
////            if (alreadyPressed) {
////                return;
////            }
////
////            bool buttonPressed = index < 4;
////
////            if ((buttonPressed && buttonEnabled) || (!buttonPressed && joypadEnabled)) {
////                mmu.RequestInterrupt (InterruptFlag.Joypad);
////                Debug.Log ("INTERRUPT OF BUTTON ? " + buttonEnabled + " JOYPAD ? " + joypadEnabled);
////            }
//        }
//
//        public void ReleaseJoypad (int index)
//        {
//            joypadKeys [index] = false;
////            byte flag = (byte)(1 << index);
////            keys = (byte)(keys & ~flag);
//        }
    }
}