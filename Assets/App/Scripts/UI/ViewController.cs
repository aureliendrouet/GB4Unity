using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class ViewController : MonoBehaviour
    {
        [SerializeField] string romName;

        [SerializeField] Button[] buttons;
        [SerializeField] View[] views;

        Mobo mobo;

        void Start ()
        {
            // limit to 60 fps
            Application.targetFrameRate = 60;

            // create
            mobo = new Mobo ();

            // setup view
            foreach (var view in views) {
                view.Setup (mobo);
            }

            // load default rom
            string filename = string.Format ("{0}/App/Resources/Roms/{1}.gb", Application.dataPath, romName);

            if (!File.Exists (filename)) {
                Debug.LogErrorFormat ("{0} not found", filename);
                return;
            }

            byte[] rom = File.ReadAllBytes (filename);

            mobo.LoadRom (rom);
        }

        public void SwitchView (Button button)
        {
            int index = button.transform.GetSiblingIndex ();
            var view = views [index];
            view.gameObject.SetActive (!view.gameObject.activeSelf);
        }
    }
}