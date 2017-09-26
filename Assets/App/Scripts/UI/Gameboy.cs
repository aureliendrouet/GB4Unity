using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Gameboy : MonoBehaviour
    {
        [SerializeField] string romName;

        [Header ("Views")]
        [SerializeField] MoboView moboView;
        [SerializeField] CpuView cpuView;
        [SerializeField] MmuView mmuView;
        [SerializeField] DisassemblyView disassemblyView;

        Cpu cpu;
        Mmu mmu;

        void Start ()
        {
            string filename = string.Format ("{0}/App/Resources/Roms/{1}.gb", Application.dataPath, romName);

            if (!File.Exists (filename)) {
                Debug.LogErrorFormat ("{0} not found", filename);
                return;
            }

            byte[] rom = File.ReadAllBytes (filename);

            mmu = new Mmu ();
            mmu.LoadRom (rom);

            cpu = new Cpu ();
            cpu.Boot (mmu);

            moboView.SetCpu (cpu);
            cpuView.SetCpu (cpu);
            mmuView.SetMmu (mmu);
            disassemblyView.SetCpu (cpu);
        }
    }
}