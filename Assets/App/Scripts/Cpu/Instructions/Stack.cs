
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction PUSHBC = (_) => { _.pushw(_.bc);   _.m = 3; };
        static Instruction PUSHDE = (_) => { _.pushw(_.de);   _.m = 3; };
        static Instruction PUSHHL = (_) => { _.pushw(_.hl);   _.m = 3; };
        static Instruction PUSHAF = (_) => { _.pushw(_.af);   _.m = 3; };

        static Instruction POPBC = (_) => { _.bc = _.popw(); _.m = 3; };
        static Instruction POPDE = (_) => { _.de = _.popw(); _.m = 3; };
        static Instruction POPHL = (_) => { _.hl = _.popw(); _.m = 3; };
        static Instruction POPAF = (_) => { _.af = _.popw(); _.m = 3; };
    }
}
