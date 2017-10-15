using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterruptFlag
{
    public static byte Vblank  = 0x01;
    public static byte LcdStat = 0x02;
    public static byte TimeOverflow   = 0x04;
    public static byte SerialTransferComplete  = 0x08;
    public static byte Joypad  = 0x10;
}
