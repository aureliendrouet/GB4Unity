using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StudioKurage.Emulator.Gameboy
{
    public class MoboView : MonoBehaviour
    {
        [SerializeField] InputField opcodeInputField;
        [SerializeField] InputField waitForSecondsInputField;
        [SerializeField] Button runButton;
        [SerializeField] Button pauseButton;

        [SerializeField] UnityEvent beforeOpcodeExecuted;
        [SerializeField] UnityEvent afterOpcodeExecuted;

        protected Mobo mobo;

        public void Setup (Mobo _)
        {
            mobo = _;
        }

        public void ExecuteOpCode ()
        {
            byte opcode = StringUtil.HexToByte (opcodeInputField.text);

            mobo.cpu.ExecOpcode (opcode);

            opcodeInputField.text = "";
        }

        public void ExecuteNextOpcode ()
        {
            beforeOpcodeExecuted.Invoke ();
            mobo.cpu.ExecNextOpcode ();
            afterOpcodeExecuted.Invoke ();
        }

        //        public void Run ()
        //        {
        //            StartCoroutine (RunAsync ());
        //            runButton.gameObject.SetActive (false);
        //            pauseButton.gameObject.SetActive (true);
        //        }

        public void Pause ()
        {
            StopAllCoroutines ();
            runButton.gameObject.SetActive (true);
            pauseButton.gameObject.SetActive (false);
        }

        //        IEnumerator RunAsync ()
        //        {
        //            float seconds = Mathf.Max (0.05f, StringUtil.ToFloat (waitForSecondsInputField.text));
        //            waitForSecondsInputField.text = seconds.ToString ();
        //
        //            var wait = new WaitForSeconds (seconds);
        //
        //            while (true) {
        //                ExecuteNextOpcode ();
        //                yield return wait;
        //            }
        //        }
        [SerializeField] Material material;
        [SerializeField] RenderTexture renderTexture;
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
            int cyclesFor60fps = 69905;
            float deltaTimeFor60fps = 1 / 60f;
            float cross = cyclesFor60fps * deltaTimeFor60fps;
            long cycles;
            long maxCycles;

            while (true) {
                yield return null;
                cycles = 0;
                maxCycles = Mathf.RoundToInt (cross / Time.deltaTime);

                while (cycles < maxCycles) {
                    cycles += mobo.Tick ();
                }

                if (mobo.gpu.frameRendered) {
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
    }
}
