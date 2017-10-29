using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Mbc1 : Mbc0
    {
        public Mbc1 (byte[][]romBanks) : base (romBanks)
        {
        }

        public override void wb (int address, byte value)
        {
            if (value == 0) {
                romBankIndex = 0;
            } else {
                romBankIndex = (romBankIndex & 0x60) | (value & 0x1F);

                if (romBankIndex == 0x00 || romBankIndex == 0x20 || romBankIndex == 0x40 || romBankIndex == 0x60) {
                    romBankIndex++;
                }
            }
        }
    }
}
