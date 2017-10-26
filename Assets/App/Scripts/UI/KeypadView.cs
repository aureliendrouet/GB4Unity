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
            mobo.keypad.Press (index);
            Refresh ();
        }

        public void Release (int index)
        {
            buttons [index].color = releasedColor;
            mobo.keypad.Release (index);
            Refresh ();
        }

        void Refresh ()
        {
            keysText.text = Convert.ToString (mobo.keypad.keys, 2).PadLeft(8, '0');
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
