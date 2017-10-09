using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class CpuView : MonoBehaviour
    {
        [SerializeField] InputField[] uint8Registers;
        [SerializeField] InputField[] uint16Registers;
        [SerializeField] InputField[] flags;
        [SerializeField] InputField[] clocks;
        [SerializeField] InputField[] signals;

        const string UInt8RegisterTemplate = "{0:X2}";
        const string UInt16RegisterTemplate = "{0:X4}";

        Cpu cpu;

        public void Setup (Cpu _)
        {
            cpu = _;
        }

        public void UpdateRegisterA (string value)
        {
            cpu.a = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterF (string value)
        {
            cpu.f = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterB (string value)
        {
            cpu.b = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterC (string value)
        {
            cpu.c = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterD (string value)
        {
            cpu.d = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterE (string value)
        {
            cpu.e = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterH (string value)
        {
            cpu.h = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterL (string value)
        {
            cpu.l = StringUtil.HexToByte (value);
            Refresh ();
        }

        public void UpdateRegisterPC (string value)
        {
            cpu.pc = Convert.ToUInt16 (value, 16);
            Refresh ();
        }

        public void UpdateRegisterSP (string value)
        {
            cpu.sp = Convert.ToUInt16 (value, 16);
            Refresh ();
        }

        public void UpdateClockMC (string value)
        {
            cpu.mc = Convert.ToInt32 (value);
            Refresh ();
        }

        public void UpdateClockIMC (string value)
        {
            cpu.imc = Convert.ToInt32 (value);
            Refresh ();
        }

        public void UpdateFlagZF (string value)
        {
            cpu.zf = value == "1" ? true : false;
            Refresh ();
        }

        public void UpdateFlagSF (string value)
        {
            cpu.sf = value == "1" ? true : false;
            Refresh ();
        }

        public void UpdateFlagHCF (string value)
        {
            cpu.hcf = value == "1" ? true : false;
            Refresh ();
        }

        public void UpdateFlagCF (string value)
        {
            cpu.cf = value == "1" ? true : false;
            Refresh ();
        }

        public void UpdateSignalIME (string value)
        {
            cpu.ime = Convert.ToInt32 (value);
            Refresh ();
        }

        public void UpdateSignalHLT (string value)
        {
            cpu.hlt = Convert.ToInt32 (value);
            Refresh ();
        }

        public void UpdateSignalSTP (string value)
        {
            cpu.stp = Convert.ToInt32 (value);
            Refresh ();
        }

        public void Refresh ()
        {
            // registers 8 bits
            uint8Registers [0].text = String.Format (UInt8RegisterTemplate, cpu.a);
            uint8Registers [1].text = String.Format (UInt8RegisterTemplate, cpu.f);
            uint8Registers [2].text = String.Format (UInt8RegisterTemplate, cpu.b);
            uint8Registers [3].text = String.Format (UInt8RegisterTemplate, cpu.c);
            uint8Registers [4].text = String.Format (UInt8RegisterTemplate, cpu.d);
            uint8Registers [5].text = String.Format (UInt8RegisterTemplate, cpu.e);
            uint8Registers [6].text = String.Format (UInt8RegisterTemplate, cpu.h);
            uint8Registers [7].text = String.Format (UInt8RegisterTemplate, cpu.l);

            // registers 16 bits
            uint16Registers [0].text = String.Format (UInt16RegisterTemplate, cpu.pc);
            uint16Registers [1].text = String.Format (UInt16RegisterTemplate, cpu.sp);

            // flags
            flags [0].text = cpu.zf ? "1" : "0";
            flags [1].text = cpu.sf ? "1" : "0";
            flags [2].text = cpu.hcf ? "1" : "0";
            flags [3].text = cpu.cf ? "1" : "0";

            // clocks
            clocks [0].text = cpu.mc.ToString ();

            // signals
            signals [0].text = cpu.ime.ToString ();
            signals [1].text = cpu.hlt.ToString ();
            signals [2].text = cpu.stp.ToString ();
        }
    }
}
