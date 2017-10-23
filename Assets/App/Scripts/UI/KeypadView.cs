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

        [SerializeField] Image[] buttons;

        KeyCode[] keys = new KeyCode[] {
            KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.L, KeyCode.Semicolon, KeyCode.J, KeyCode.K
        };

        public void Press (int index)
        {
            buttons [index].color = pressedColor;
            mobo.keypad.Press (index);
        }

        public void Release (int index)
        {
            buttons [index].color = releasedColor;
            mobo.keypad.Release (index);
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
