using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class GpuView : View
    {
        [SerializeField] GameObject[] lcdc;
        [SerializeField] GameObject[] stat;
        [SerializeField] Text[] lylyc;
        [SerializeField] Text[] scroll;
        [SerializeField] Text[] window;

        void OnEnable ()
        {
            Refresh ();
        }

        void Refresh ()
        {
            var gpu = mobo.gpu;
            lcdc [0].SetActive (gpu.backgroundEnabled);
            lcdc [1].SetActive (gpu.objectEnabled);
            lcdc [2].SetActive (gpu.largeTile);
            lcdc [3].SetActive (gpu.backgroundTilemapSelection);
            lcdc [4].SetActive (gpu.backgroundTilesetSelection);
            lcdc [5].SetActive (gpu.windowEnabled);
            lcdc [6].SetActive (gpu.windowTilemapSelection);
            lcdc [7].SetActive (gpu.lcdEnabled);
            stat [0].SetActive (gpu.lcdMode == Gpu.LcdMode.Hblank);
            stat [1].SetActive (gpu.lcdMode == Gpu.LcdMode.Vblank);
            stat [2].SetActive (gpu.lcdMode == Gpu.LcdMode.Oam);
            stat [3].SetActive (gpu.lcdMode == Gpu.LcdMode.Vram);
            stat [4].SetActive (gpu.hblankEnabled);
            stat [5].SetActive (gpu.vblankEnabled);
            stat [6].SetActive (gpu.oamEnabled);
            stat [7].SetActive (gpu.lyLycEnabled);
            stat [8].SetActive (gpu.matched);
            lylyc [0].text = gpu.ly.ToString ();
            lylyc [1].text = gpu.lyc.ToString ();
            scroll [0].text = gpu.scx.ToString ();
            scroll [1].text = gpu.scy.ToString ();
            window [0].text = gpu.wx.ToString ();
            window [1].text = gpu.wy.ToString ();
        }
    }
}
