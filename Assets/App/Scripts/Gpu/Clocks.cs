
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Gpu
    {
        // cycles
        const int MachinCyclesPerHblank = 51;
        const int MachinCyclesPerVblank = 114;
        const int MachinCyclesPerOam    = 20;
        const int MachinCyclesPerVram   = 43;
        const int CyclesPerFrame        = 17556;

        long mc;
        long delay;
    }
}
