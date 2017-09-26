
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction CALLnn   = (_) => { CALL(_, true); };
        static Instruction CALLNZnn = (_) => { CALL(_, !_.zf); };
        static Instruction CALLZnn  = (_) => { CALL(_,  _.zf); };
        static Instruction CALLNCnn = (_) => { CALL(_, !_.cf); };
        static Instruction CALLCnn  = (_) => { CALL(_,  _.cf); };

        static void CALL(Cpu _, bool b) { if (b) { _.pushw(_.pc, 2); _.pc = _.mmu.rw(_.pc); _.m = 5; } else { _.pc += 2; _.m = 3; } }
    }
}
