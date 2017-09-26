
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction SWAPr_b  = (_) => { SWAP(_, ref _.b); };
        static Instruction SWAPr_c  = (_) => { SWAP(_, ref _.c); };
        static Instruction SWAPr_d  = (_) => { SWAP(_, ref _.d); };
        static Instruction SWAPr_e  = (_) => { SWAP(_, ref _.e); };
        static Instruction SWAPr_h  = (_) => { SWAP(_, ref _.h); };
        static Instruction SWAPr_l  = (_) => { SWAP(_, ref _.l); };
        static Instruction SWAPr_a  = (_) => { SWAP(_, ref _.a); };

        static Instruction SWAPrHLm = (_) => { byte n = _.mmu.rb(_.hl); int r = (((n & 0x0F) << 4) | ((n & 0xF0) >> 4)); _.mmu.wb(_.hl, (byte)r); _.zf = r == 0; _.sf = false; _.hcf = false; _.cf = false; _.m = 4; };

        static void SWAP   (Cpu _, ref byte r) { byte tmp = r; r = _.mmu.rb(_.hl); _.mmu.wb(_.hl, tmp); _.zf = r == 0; _.sf = false; _.hcf = false; _.cf = false; _.m = 4; }
    }
}
