using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class Mbc1 : Mbc
    {
        public Mbc1 (byte[][]romBanks) : base(romBanks)
        {
        }

        public void wb (ushort address, byte value)
        {
            if (value == 0) {
                currentRomBankIndex = 0;
            } else {
                currentRomBankIndex = (currentRomBankIndex & 0x60) | (value & 0x1F);

                if (currentRomBankIndex == 0x00 || currentRomBankIndex == 0x20 || currentRomBankIndex == 0x40 || currentRomBankIndex == 0x60) {
                    currentRomBankIndex++;
                }
            }
        }
    }
}
