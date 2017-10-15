using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public class View : MonoBehaviour
    {
        protected Mobo mobo;

        public virtual void Setup (Mobo _)
        {
            mobo = _;
        }
    }
}
