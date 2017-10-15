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
        [SerializeField] InputField opcodeInputField;

        [SerializeField] UnityEvent beforeOpcodeExecuted;
        [SerializeField] UnityEvent afterOpcodeExecuted;

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
    }
}
