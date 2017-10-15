using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class CartridgeView : View
    {
        [SerializeField] Text[] labels;

        Mmu mmu;

        public override void Setup (Mobo _)
        {
            base.Setup (_);
            mmu = mobo.mmu;
        }

        public void OnEnable ()
        {
            Refresh ();
        }

        void Refresh ()
        {
            labels [0].text = GetString (Address.GameTitle_L, Address.GameTitle_M);
            labels [1].text = GetString (Address.ManufacturerCode_L, Address.ManufacturerCode_M);
            labels [2].text = GetByte (Address.GameboyColorCompatibility);
            labels [3].text = GetString (Address.NewLicense_L, Address.NewLicense_M);
            labels [4].text = GetByte (Address.SuperGameboyCompatibility);
            labels [5].text = GetByte (Address.CartridgeType);
            labels [6].text = GetByte (Address.RomSize);
            labels [7].text = GetByte (Address.RamSize);
            labels [8].text = GetByte (Address.DestinationCode);
            labels [9].text = GetByte (Address.OldLicense);
            labels [10].text = GetByte (Address.MaskRomVersion);
            labels [11].text = GetByte (Address.ComplementChecksum);
            labels [12].text = GetBytes (Address.CheckSum_L, Address.CheckSum_M);
        }

        string GetByte (ushort address)
        {
            byte value = mmu.rb (address);
            return String.Format ("{0:X2}", value);
        }

        string GetBytes (ushort begin, ushort end)
        {
            byte[] values = mmu.r (begin, end);
            string str = "";
            foreach (byte value in values) {
                str += String.Format ("{0:X2} ", value);
            }
            return str;
        }

        string GetString (ushort begin, ushort end)
        {
            byte[] values = mmu.r (begin, end);
            return System.Text.Encoding.ASCII.GetString (values);
        }
    }
}
