
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction UNKNOWN = (_) => { STOP(_); };

        static Instruction MAPcb = (_) => { byte v = _.mmu.rb(_.pc++); cbmap[v](_); };

        static Instruction NOP  = (_) => { _.m = 1; };

        static Instruction STOP = (_) => { _.stp = 1; _.m = 1; };
        static Instruction HALT = (_) => { _.hlt = 1; _.m = 1; };

        static Instruction DI   = (_) => { _.ime = 0; _.m = 1; };
        static Instruction EI   = (_) => { _.ime = 1; _.m = 1; };

        static Instruction CPL = (_) => { _.a ^= 0xFF; _.sf = true; _.hcf = true; _.m = 1; };

        static Instruction CCF = (_) => { _.cf = !_.cf; _.sf = false; _.hcf = false; _.m = 1; };
        static Instruction SCF = (_) => { _.cf = true;  _.sf = false; _.hcf = false; _.m = 1; };

        static Instruction DAA = (_) => {
            int a = _.a;

            if (!_.sf) {
                if (_.hcf || (a & 0x0f) > 0x09) {
                    a += 0x06;
                }
                if (_.cf || (a & 0xf0) > 0x90) {
                    a += 0x06;
                }
            } else {
                if (_.hcf) {
                    a = (a - 0x06) & 0xFF;
                }
                if (_.cf) {
                    a -= 0x60;
                }
            }

            _.hcf = false;

            if ((a & 0x100) == 0x100) {
                _.cf = true;
            }

            a &= 0xFF;

            _.zf = a == 0;

            _.a = (byte)a;
        };
    }
}
