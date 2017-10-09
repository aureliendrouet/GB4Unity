using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StudioKurage.Emulator.Gameboy
{
    public class MoboView : MonoBehaviour
    {
        [SerializeField] InputField opcodeInputField;
        [SerializeField] InputField waitForSecondsInputField;
        [SerializeField] Button runButton;
        [SerializeField] Button pauseButton;

        [SerializeField] UnityEvent beforeOpcodeExecuted;
        [SerializeField] UnityEvent afterOpcodeExecuted;

        protected Mobo mobo;

        public void Setup (Mobo _)
        {
            mobo = _;
        }

        public void ExecuteOpCode ()
        {
            byte opcode = StringUtil.HexToByte (opcodeInputField.text);

            mobo.cpu.ExecOpcode (opcode);

            opcodeInputField.text = "";
        }

        public void ExecuteNextOpcode ()
        {
            beforeOpcodeExecuted.Invoke ();
            mobo.cpu.ExecNextOpcode ();
            afterOpcodeExecuted.Invoke ();
        }

        public void Run ()
        {
            StartCoroutine (RunAsync ());
            runButton.gameObject.SetActive (false);
            pauseButton.gameObject.SetActive (true);
        }

        public void Pause ()
        {
            StopAllCoroutines ();
            runButton.gameObject.SetActive (true);
            pauseButton.gameObject.SetActive (false);
        }

        IEnumerator RunAsync ()
        {
            float seconds = Mathf.Max (0.05f, StringUtil.ToFloat (waitForSecondsInputField.text));
            waitForSecondsInputField.text = seconds.ToString ();

            var wait = new WaitForSeconds (seconds);

            while (true) {
                ExecuteNextOpcode ();
                yield return wait;
            }
        }
    }
}
