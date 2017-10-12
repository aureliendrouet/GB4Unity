using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class ViewController : MonoBehaviour
    {
        [SerializeField] string romName;

        [Header ("Views")]
        [SerializeField] MoboView moboView;
        [SerializeField] CpuView cpuView;
        [SerializeField] MmuView mmuView;
        [SerializeField] DisassemblyView disassemblyView;

        Mobo mobo;
        Disassembler disassembler;

        void Start ()
        {
            // limit to 60 fps
            Application.targetFrameRate = 60;

            // create
            mobo = new Mobo ();
            disassembler = new Disassembler (mobo.cpu);

            // setup view
            moboView.Setup (mobo);
            cpuView.Setup (mobo.cpu);
            mmuView.Setup (mobo.mmu);
            disassemblyView.Setup (disassembler);

            // load default rom
            string filename = string.Format ("{0}/App/Resources/Roms/{1}.gb", Application.dataPath, romName);

            if (!File.Exists (filename)) {
                Debug.LogErrorFormat ("{0} not found", filename);
                return;
            }

            byte[] rom = File.ReadAllBytes (filename);

            mobo.LoadRom (rom);
        }
    }
}