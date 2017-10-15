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
        [SerializeField] Button runButton;
        [SerializeField] Button pauseButton;

        [Header ("Graphics")]
        [SerializeField] RenderTexture renderTexture;
        [SerializeField] Material material;
        [SerializeField] Color[] colors;
        Texture2D texture;

        public void Run ()
        {
            texture = new Texture2D (renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            material.mainTexture = texture;

            StartCoroutine (RunAsync ());
            runButton.gameObject.SetActive (false);
            pauseButton.gameObject.SetActive (true);
        }

        IEnumerator RunAsync ()
        {
            float cyclesPerSecond = 4194304; // 4.194304 MHz
            long cycles;
            long maxCycles;
            float speed = 0.1f; // 10 times slower

            while (true) {
                yield return null;
                cycles = 0;
                maxCycles = Mathf.RoundToInt (cyclesPerSecond * Time.deltaTime * speed);

                while (cycles < maxCycles) {
                    cycles += mobo.Tick ();
                }

                if (mobo.gpu.frameRendered) {
                    UpdateFrame ();
                }
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
                int x = i % Gpu.WindowWidth;
                int y = i / Gpu.WindowWidth;
                texture.SetPixel (x, y, colors [frame [i]]);
            }

            texture.Apply ();

            RenderTexture.active = null;
        }
    }
}
