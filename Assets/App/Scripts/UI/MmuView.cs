using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class MmuView : View
    {
        [SerializeField] Transform inputContainer;
        [SerializeField] Transform linkContainer;
        [SerializeField] InputField page;

        InputField[] inputs;
        Button[] buttons;
        string[] links = new string[] { "00", "40", "80", "A0", "C0", "FE", "FF", "FF", "FF" };

        Mmu mmu;

        public override void Setup (Mobo _)
        {
            base.Setup (_);
            mmu = _.mmu;
        }

        void Awake ()
        {
            inputs = new InputField[inputContainer.childCount];

            foreach (Transform child in inputContainer) {
                var input = child.GetComponent<InputField> ();
                input.onEndEdit.AddListener (CreateInputListener (input));
                inputs [child.GetSiblingIndex ()] = input;
            }

            buttons = new Button[linkContainer.childCount];

            foreach (Transform child in linkContainer) {
                buttons [child.GetSiblingIndex ()] = child.GetComponent<Button> ();
            }
        }

        public void Refresh ()
        {
            if (!gameObject.activeInHierarchy) {
                return;
            }

            byte upper = StringUtil.HexToByte (page.text);
            page.text = string.Format ("{0:X2}", upper);
            
            foreach (var input in inputs) {
                byte lower = (byte)input.transform.GetSiblingIndex ();
                ushort address = (ushort)(upper << 8 | lower);
                input.text = string.Format ("{0:X2}", mmu.rb (address));
            }
        }

        public UnityAction<string> CreateInputListener (InputField input)
        {
            return (string value) => {
                SetMemory (input);
                Refresh ();
            };
        }

        public void SetMemory (InputField input)
        {
            byte upper = StringUtil.HexToByte (page.text);
            byte lower = (byte)input.transform.GetSiblingIndex ();
            ushort address = (ushort)(upper << 8 | lower);
            byte value = StringUtil.HexToByte (input.text);
            mmu.wb (address, value);
        }

        public void SetPage (Button button)
        {
            int index = System.Array.IndexOf (buttons, button);

            if (index < 0 || index > buttons.Length - 1) {
                return;
            }

            page.text = links [index];

            Refresh ();
        }
    }
}