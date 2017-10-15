
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction CALLnn   = (_) => { _.pushw(_.pc, 2); _.pc = _.mmu.rw(_.pc); };

        static Instruction CALLNZnn = (_) => { CALL(_, !_.zf); };
        static Instruction CALLZnn  = (_) => { CALL(_,  _.zf); };
        static Instruction CALLNCnn = (_) => { CALL(_, !_.cf); };
        static Instruction CALLCnn  = (_) => { CALL(_,  _.cf); };

        // push address of next instruction onto stack and then jump to address of current instruction
        static void CALL(Cpu _, bool b) { if (b) { CALLnn(_); _.timing = btiming; } else { _.pc += 2; } }
    }
}
