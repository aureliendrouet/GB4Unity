using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StudioKurage.Emulator.Gameboy
{
    public class MoboView : View
    {
        [Header ("Lcd")]
        [SerializeField] LcdView lcd;

        [Header ("Opcode")]
        [SerializeField] InputField opcodeInputField;

        [Header ("Tick")]
        [SerializeField] InputField tickInputField;

        [Header ("Frame")]
        [SerializeField] InputField frameInputField;

        [Header ("Player")]
        [SerializeField] Slider speedSlider;
        [SerializeField] Button runButton;
        [SerializeField] Button pauseButton;

        [Header ("Events")]
        [SerializeField] UnityEvent beforeOpcodeExecuted;
        [SerializeField] UnityEvent afterOpcodeExecuted;

        #region Cpu Only

        public void Opcode ()
        {
            byte opcode = StringUtil.HexToByte (opcodeInputField.text);

            mobo.cpu.ExecOpcode (opcode);

            opcodeInputField.text = "";
        }

        public void NextOpcode ()
        {
            beforeOpcodeExecuted.Invoke ();
            mobo.cpu.ExecNextOpcode ();
            afterOpcodeExecuted.Invoke ();
        }

        #endregion

        #region Tick

        public void NextTick ()
        {
            int count = ParseInt (tickInputField, 1, 9999);

            for (int i = 0; i < count; i++) {
                mobo.Tick ();
            }

            lcd.UpdateFrame ();

            afterOpcodeExecuted.Invoke ();
        }

        #endregion

        #region Frame

        public void NextFrame ()
        {
            int count = ParseInt (frameInputField, 1, 9999);

            for (int i = 0; i < count; i++) {
                do {
                    mobo.Tick ();
                } while (!mobo.gpu.frameRendered);
            }

            lcd.UpdateFrame ();

            afterOpcodeExecuted.Invoke ();
        }

        #endregion

        #region Player

        public void Run ()
        {
            StartCoroutine (RunAsync ());
            runButton.gameObject.SetActive (false);
            pauseButton.gameObject.SetActive (true);
        }

        IEnumerator RunAsync ()
        {
            float cyclesPerSecond = 4194304; // 4.194304 MHz
            long cycles;
            long maxCycles;

            while (true) {
                cycles = 0;
                maxCycles = (int)(cyclesPerSecond * Time.deltaTime * speedSlider.value);

                while (cycles < maxCycles) {
                    cycles += mobo.Tick ();

                    if (mobo.gpu.frameRendered) {
                        lcd.UpdateFrame ();
                    }
                }

                yield return null;
            }
        }

        public void Pause ()
        {
            StopAllCoroutines ();
            runButton.gameObject.SetActive (true);
            pauseButton.gameObject.SetActive (false);
        }

        #endregion

        int ParseInt (InputField inputField, int min, int max)
        {
            int value;
            bool parsed = int.TryParse (inputField.text, out value);

            if (parsed) {
                value = Math.Min (max, Math.Max (min, value));
            } else {
                value = min;
            }

            inputField.text = value.ToString ();

            return value;
        }
    }
}
