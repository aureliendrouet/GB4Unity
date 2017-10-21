
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {
        // cycles
        const int HblankCycles = 207;
        const int VblankCycles = 4560;
        const int OamCycles    = 83;
        const int VramCycles   = 175;

        const int LineCycles = 456;

        long cc;
        long delay;
    }
}
