
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

        static Instruction DAA = (_) => {
            int a = _.a;
            bool n = _.sf;
            bool h = _.hcf;
            bool c = _.cf;

            if (!n) {
                if (c || (a > 0x99)) {
                    a = (a + 0x60) & 0xFF;
                    _.cf = true;
                }
                if (h || (a & 0x0F) > 0x09) {
                    a = (a + 0x06) & 0xFF;
                    _.hcf = false;
                }
            } else if (c && h) {
                a = (a + 0x9A) & 0xFF;
                _.hcf = false;
            } else if (c) {
                a = (a + 0xA0) & 0xFF;
            } else if (h) {
                a = (a - 0xFA) & 0xFF;
                _.hcf = false;
            }

            _.zf = a == 0;
            _.a = (byte)a;
        };
    }
}
