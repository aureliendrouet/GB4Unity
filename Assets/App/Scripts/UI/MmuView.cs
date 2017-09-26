using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class MmuView : MonoBehaviour
    {
        [SerializeField] Transform inputContainer;
        [SerializeField] Transform linkContainer;
        [SerializeField] InputField page;

        Mmu mmu;
        InputField[] inputs;
        Button[] buttons;

        public void SetMmu (Mmu mmu)
        {
            this.mmu = mmu;
        }

        void Awake ()
        {
            inputs = new InputField[inputContainer.childCount];

            foreach (Transform child in inputContainer) {
                inputs [child.GetSiblingIndex ()] = child.GetComponent<InputField> ();
            }

            buttons = new Button[linkContainer.childCount];

            foreach (Transform child in linkContainer) {
                buttons[child.GetSiblingIndex ()] = child.GetComponent<Button> ();
            }
        }

        public void Refresh ()
        {
            byte upper = StringUtil.HexToByte (page.text);
            page.text = string.Format ("{0:X2}", upper);
            
            foreach (var input in inputs) {
                byte lower = (byte)input.transform.GetSiblingIndex ();
                ushort address = (ushort)(upper << 8 | lower);
                input.text = string.Format ("{0:X2}", mmu.rb (address));
            }
        }
    }
}