using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioKurage.Emulator.Gameboy
{
    public class LcdView : View
    {
        [SerializeField] RectTransform panel;
        [SerializeField] RectTransform screen;
        [SerializeField] RenderTexture renderTexture;
        [SerializeField] Material material;
        [SerializeField] Color[] colors;

        Texture2D texture;

        Vector2[] screenSizes = new Vector2[] {
            new Vector2 (160, 144),
            new Vector2 (320, 288),
            new Vector2 (480, 432),
            new Vector2 (640, 576),
        };

        Vector2 panelOffsets = new Vector2 (24f, 60f);

        void Awake ()
        {
            texture = new Texture2D (renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            material.mainTexture = texture;
        }

        public void UpdateFrame ()
        {
            RenderTexture.active = renderTexture;

            byte[] frame = mobo.gpu.frame;

            for (int i = 0; i < frame.Length; i++) {
                int x = i % Gpu.FrameWidth;
                int y = Gpu.FrameHeight - (i / Gpu.FrameWidth) - 1;
                texture.SetPixel (x, y, colors [frame [i]]);
            }

            texture.Apply ();

            RenderTexture.active = null;
        }

        public void SetZoom (int i)
        {
            panel.sizeDelta = screenSizes [i] + panelOffsets;
            screen.sizeDelta = screenSizes [i];
        }
    }
}
