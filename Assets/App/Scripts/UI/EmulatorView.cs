using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StudioKurage.Emulator.Gameboy
{
    public class EmulatorView : View
    {
        [Header ("UI")]
        [SerializeField, Range (0.01f, 1f)] float speed;
        [SerializeField] Button runButton;
        [SerializeField] Button pauseButton;

        [Header ("Graphics")]
        [SerializeField] RectTransform panel;
        [SerializeField] RectTransform screen;
        [SerializeField] RenderTexture renderTexture;
        [SerializeField] Material material;
        [SerializeField] Color[] colors;
        Texture2D texture;

        void Awake ()
        {
            texture = new Texture2D (renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            material.mainTexture = texture;
        }

        public void Run ()
        {
            StartCoroutine (RunAsync ());
            runButton.gameObject.SetActive (false);
            pauseButton.gameObject.SetActive (true);
        }

        IEnumerator RunAsync ()
        {
            float cyclesPerSecond = 4194304; // 4.194304 MHz
            long cycles;
            long maxCycles;

            while (true) {
                cycles = 0;
                maxCycles = (int)(cyclesPerSecond * Time.deltaTime * speed);

                while (cycles < maxCycles) {
                    cycles += mobo.Tick ();

                    if (mobo.gpu.frameRendered) {
                        UpdateFrame ();
                    }
                }

                yield return null;
            }
        }

        public void Pause ()
        {
            StopAllCoroutines ();
            runButton.gameObject.SetActive (true);
            pauseButton.gameObject.SetActive (false);
        }

        public void NextFrame ()
        {
            while (!mobo.gpu.frameRendered) {
                mobo.Tick ();
            }
            UpdateFrame ();
        }

        void UpdateFrame ()
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

        Vector2[] screenSizes = new Vector2[] {
            new Vector2(160, 144),
            new Vector2(320, 288),
            new Vector2(480, 432),
            new Vector2(640, 576),
        };
        Vector2 panelOffsets = new Vector2 (32f, 120f);

        public void SetZoom (int i)
        {
            panel.sizeDelta = screenSizes [i] + panelOffsets;
            screen.sizeDelta = screenSizes [i];
        }
    }
}
