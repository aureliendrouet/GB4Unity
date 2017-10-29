using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class KeypadView : View
    {
        [SerializeField] Color pressedColor;
        [SerializeField] Color releasedColor;
        [SerializeField] Text keysText;
        [SerializeField] Text memoryText;
        [SerializeField] Image[] buttons;

        KeyCode[] keys = new KeyCode[] {
            KeyCode.L, KeyCode.Semicolon, KeyCode.J, KeyCode.K, KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.S
        };

        public void Press (int index)
        {
            buttons [index].color = pressedColor;
            if (index < 4) {
                mobo.keypad.PressButton (index);
            } else {
                mobo.keypad.PressJoypad (index - 4);
            }
            Refresh ();
        }

        public void Release (int index)
        {
            buttons [index].color = releasedColor;
            if (index < 4) {
                mobo.keypad.ReleaseButton (index);
            } else {
                mobo.keypad.ReleaseJoypad (index - 4);
            }
            Refresh ();
        }

        void Refresh ()
        {
            keysText.text = "";
            for (int i = mobo.keypad.joypadKeys.Length - 1; i >= 0; i--) {
                keysText.text += mobo.keypad.joypadKeys [i] ? "1" : "0";
            }
            for (int i = mobo.keypad.buttonKeys.Length - 1; i >= 0; i--) {
                keysText.text += mobo.keypad.buttonKeys [i] ? "1" : "0";
            }
            memoryText.text = Convert.ToString (mobo.keypad.memory, 2).PadLeft(8, '0');
        }

        void Update ()
        {
            for (int i = 0; i < keys.Length; i++) {
                if (Input.GetKeyUp (keys [i])) {
                    Release (i);
                }
                if (Input.GetKeyDown (keys [i])) {
                    Press (i);
                }
            }
        }
    }
}
