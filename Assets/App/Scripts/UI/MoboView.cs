using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StudioKurage.Emulator.Gameboy
{
    public class MoboView : MonoBehaviour
    {
        [SerializeField] InputField opcodeInputField;
        [SerializeField] UnityEvent beforeOpcodeExecuted;
        [SerializeField] UnityEvent afterOpcodeExecuted;

        protected Cpu cpu;

        public void SetCpu (Cpu _)
        {
            cpu = _;
        }

        public void ExecuteOpCode ()
        {
            byte opcode = StringUtil.HexToByte (opcodeInputField.text);

            cpu.ExecOpcode (opcode);

            opcodeInputField.text = "";
        }

        public void ExecuteNextOpcode ()
        {
            beforeOpcodeExecuted.Invoke ();
            cpu.ExecNextOpcode ();
            afterOpcodeExecuted.Invoke ();
        }
    }
}
