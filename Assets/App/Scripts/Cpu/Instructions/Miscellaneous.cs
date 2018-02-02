
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction UNKNOWN = (_) => { STOP(_); };

        static Instruction NOP  = (_) => { };

        static Instruction STOP = (_) => { _.stp = true; };
        static Instruction HALT = (_) => { _.hlt = true; };

        static Instruction DI   = (_) => { _.diPendingCount = 0; };
        static Instruction EI   = (_) => { _.eiPendingCount = 0; };

        static Instruction CPL = (_) => { _.a ^= 0xFF; _.sf = true; _.hcf = true; };

        static Instruction CCF = (_) => { _.cf = !_.cf; _.sf = false; _.hcf = false; };
        static Instruction SCF = (_) => { _.cf = true;  _.sf = false; _.hcf = false; };
    }
}
