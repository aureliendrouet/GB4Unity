
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction RST00 = (_) => { RST(_, 0x00); };
        static Instruction RST08 = (_) => { RST(_, 0x08); };
        static Instruction RST10 = (_) => { RST(_, 0x10); };
        static Instruction RST18 = (_) => { RST(_, 0x18); };
        static Instruction RST20 = (_) => { RST(_, 0x20); };
        static Instruction RST28 = (_) => { RST(_, 0x28); };
        static Instruction RST30 = (_) => { RST(_, 0x30); };
        static Instruction RST38 = (_) => { RST(_, 0x38); };

        static void RST (Cpu _, byte v) { _.pushw(_.pc); _.pc = v; }
    }
}
