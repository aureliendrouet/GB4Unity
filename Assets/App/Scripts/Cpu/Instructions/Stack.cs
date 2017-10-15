
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction PUSHBC = (_) => { _.pushw(_.bc); };
        static Instruction PUSHDE = (_) => { _.pushw(_.de); };
        static Instruction PUSHHL = (_) => { _.pushw(_.hl); };
        static Instruction PUSHAF = (_) => { _.pushw(_.af); };

        static Instruction POPBC = (_) => { _.bc = _.popw(); };
        static Instruction POPDE = (_) => { _.de = _.popw(); };
        static Instruction POPHL = (_) => { _.hl = _.popw(); };
        static Instruction POPAF = (_) => { _.af = _.popw(); };
    }
}
