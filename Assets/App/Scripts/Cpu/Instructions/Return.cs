
namespace StudioKurage.Emulator.Gameboy
{
    
    public partial class Cpu
    {
        static Instruction RETNZ = (_) => { RET(_, !_.zf); };
        static Instruction RETZ  = (_) => { RET(_,  _.zf); };
        static Instruction RETNC = (_) => { RET(_, !_.cf); };
        static Instruction RETC  = (_) => { RET(_,  _.cf); };
        static Instruction RETNI = (_) => { _.pc = _.popw(); };
        static Instruction RETI  = (_) => { _.pc = _.popw(); _.ime = true; };
       
        static void RET (Cpu _, bool b) { if (b) { _.pc = _.popw(); _.timing = btiming; } }
    }
}
