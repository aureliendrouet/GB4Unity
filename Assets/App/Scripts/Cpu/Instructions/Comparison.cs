
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction CPr_b = (_) => { CP(_, _.b); };
        static Instruction CPr_c = (_) => { CP(_, _.c); };
        static Instruction CPr_d = (_) => { CP(_, _.d); };
        static Instruction CPr_e = (_) => { CP(_, _.e); };
        static Instruction CPr_h = (_) => { CP(_, _.h); };
        static Instruction CPr_l = (_) => { CP(_, _.l); };
        static Instruction CPr_a = (_) => { CP(_, _.a); };
        static Instruction CPHL  = (_) => { CP(_, _.mmu.rb(_.hl)); };
        static Instruction CPn   = (_) => { CP(_, _.mmu.rb(_.pc++)); };

        // Compare with a
        static void CP(Cpu _, byte n)  { _.zf = _.a == n; _.sf = true; _.hcf = (_.a & 0x0F) < ((_.a - n) & 0x0F); _.cf = _.a < n; }
    }
}
