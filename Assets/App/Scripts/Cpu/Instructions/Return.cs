
namespace StudioKurage.Emulator.Gameboy
{
    
    public partial class Cpu
    {
        static Instruction RETNZ = (_) => { RET(_, !_.zf); };
        static Instruction RETZ  = (_) => { RET(_,  _.zf); };
        static Instruction RETNC = (_) => { RET(_, !_.cf); };
        static Instruction RETC  = (_) => { RET(_,  _.cf); };
        static Instruction RETNI = (_) => { RET(_,  true); };
        static Instruction RETI  = (_) => { RET(_,  true); _.ime= 1; };
       
        static void RET (Cpu _, bool b) { if (b) { _.pc = _.popw(); _.m = 3; } else { _.m = 1; } }
    }
}
