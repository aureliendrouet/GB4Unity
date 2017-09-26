
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction RLr_b = (_) => { RL(_, ref _.b); };
        static Instruction RLr_c = (_) => { RL(_, ref _.c); };
        static Instruction RLr_d = (_) => { RL(_, ref _.d); };
        static Instruction RLr_e = (_) => { RL(_, ref _.e); };
        static Instruction RLr_h = (_) => { RL(_, ref _.h); };
        static Instruction RLr_l = (_) => { RL(_, ref _.l); };
        static Instruction RLr_a = (_) => { RL(_, ref _.a); };

        static Instruction RRr_b = (_) => { RR(_, ref _.b); };
        static Instruction RRr_c = (_) => { RR(_, ref _.c); };
        static Instruction RRr_d = (_) => { RR(_, ref _.d); };
        static Instruction RRr_e = (_) => { RR(_, ref _.e); };
        static Instruction RRr_h = (_) => { RR(_, ref _.h); };
        static Instruction RRr_l = (_) => { RR(_, ref _.l); };
        static Instruction RRr_a = (_) => { RR(_, ref _.a); };

        static Instruction RLCr_b = (_) => { RLC(_, ref _.b); };
        static Instruction RLCr_c = (_) => { RLC(_, ref _.c); };
        static Instruction RLCr_d = (_) => { RLC(_, ref _.d); };
        static Instruction RLCr_e = (_) => { RLC(_, ref _.e); };
        static Instruction RLCr_h = (_) => { RLC(_, ref _.h); };
        static Instruction RLCr_l = (_) => { RLC(_, ref _.l); };
        static Instruction RLCr_a = (_) => { RLC(_, ref _.a); };

        static Instruction RRCr_b = (_) => { RRC(_, ref _.b); };
        static Instruction RRCr_c = (_) => { RRC(_, ref _.c); };
        static Instruction RRCr_d = (_) => { RRC(_, ref _.d); };
        static Instruction RRCr_e = (_) => { RRC(_, ref _.e); };
        static Instruction RRCr_h = (_) => { RRC(_, ref _.h); };
        static Instruction RRCr_l = (_) => { RRC(_, ref _.l); };
        static Instruction RRCr_a = (_) => { RRC(_, ref _.a); };

        static Instruction SLAr_b = (_) => { SLA(_, ref _.b); };
        static Instruction SLAr_c = (_) => { SLA(_, ref _.c); };
        static Instruction SLAr_d = (_) => { SLA(_, ref _.d); };
        static Instruction SLAr_e = (_) => { SLA(_, ref _.e); };
        static Instruction SLAr_h = (_) => { SLA(_, ref _.h); };
        static Instruction SLAr_l = (_) => { SLA(_, ref _.l); };
        static Instruction SLAr_a = (_) => { SLA(_, ref _.a); };

        static Instruction SRAr_b = (_) => { SRA(_, ref _.b); };
        static Instruction SRAr_c = (_) => { SRA(_, ref _.c); };
        static Instruction SRAr_d = (_) => { SRA(_, ref _.d); };
        static Instruction SRAr_e = (_) => { SRA(_, ref _.e); };
        static Instruction SRAr_h = (_) => { SRA(_, ref _.h); };
        static Instruction SRAr_l = (_) => { SRA(_, ref _.l); };
        static Instruction SRAr_a = (_) => { SRA(_, ref _.a); };

        static Instruction SRLr_b = (_) => { SRL(_, ref _.b); };
        static Instruction SRLr_c = (_) => { SRL(_, ref _.c); };
        static Instruction SRLr_d = (_) => { SRL(_, ref _.d); };
        static Instruction SRLr_e = (_) => { SRL(_, ref _.e); };
        static Instruction SRLr_h = (_) => { SRL(_, ref _.h); };
        static Instruction SRLr_l = (_) => { SRL(_, ref _.l); };
        static Instruction SRLr_a = (_) => { SRL(_, ref _.a); };

        static Instruction RLA  = (_) => { var co = _.a & 0x80; _.a = (byte) ((_.a << 1) |  (_.cf ? 0x01 : 0x00));       SF(_, co); _.m = 1; };
        static Instruction RRA  = (_) => { var co = _.a & 0x01; _.a = (byte) ((_.a >> 1) | ((_.cf ? 0x01 : 0x00) << 7)); SF(_, co); _.m = 1; };

        static Instruction RLCA = (_) => { var co = _.a & 0x80; _.a = (byte) ((_.a << 1) | (co >> 7)); SF(_, co); _.m = 1; };
        static Instruction RRCA = (_) => { var co = _.a & 0x01; _.a = (byte) ((_.a >> 1) | (co << 7)); SF(_, co); _.m = 1; };

        static Instruction SRAHL = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x01; byte r = (byte)((hl >> 1) | (hl & 0x80));               _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction SLAHL = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x80; byte r = (byte) (hl << 1);                              _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction SRLHL = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x01; byte r = (byte) (hl >> 1);                              _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction RLCHL = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x80; byte r = (byte)((hl << 1) | (co >> 0x80));              _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction RRCHL = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x01; byte r = (byte)((hl >> 1) | (co << 0x80));              _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction RLHL  = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x80; byte r = (byte)((hl << 1) | (_.cf ? 0x01 : 0x00));      _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };
        static Instruction RRHL  = (_) => { byte hl = _.mmu.rb(_.hl); var co = hl & 0x01; byte r = (byte)((hl >> 1) | (_.cf ? 0x80 : 0x00))     ; _.mmu.wb(_.hl, r); SF(_, co, r); _.m = 4; };

        static void RLC (Cpu _, ref byte r) { var co = r & 0x80; r = (byte) ((r << 1) | (co >> 7)); SF(_, co, r); _.m = 2; }
        static void RRC (Cpu _, ref byte r) { var co = r & 0x01; r = (byte) ((r >> 1) | (co << 7)); SF(_, co, r); _.m = 2; }

        static void RL  (Cpu _, ref byte r) { var co = r & 0x80; r = (byte) ((r << 1) |  (_.cf ? 0x01 : 0x00));       SF(_, co, r); _.m = 2; }
        static void RR  (Cpu _, ref byte r) { var co = r & 0x01; r = (byte) ((r >> 1) | ((_.cf ? 0x01 : 0x00) << 7)); SF(_, co, r); _.m = 2; }

        static void SLA (Cpu _, ref byte r) { var co = r & 0x80; r = (byte) (r << 1);                SF(_, co, r); _.m = 2; }
        static void SRA (Cpu _, ref byte r) { var co = r & 0x01; r = (byte) ((r & 0x80) | (r >> 1)); SF(_, co, r); _.m = 2; }
        static void SRL (Cpu _, ref byte r) { var co = r & 0x01; r = (byte) (r >> 1);                SF(_, co, r); _.m = 2; }

        static void SF (Cpu _, int co) { _.f = (byte)(_.f & 0x0f); _.cf = co != 0; }
        static void SF (Cpu _, int co, int r) { SF (_, co); _.zf = r == 0; }
    }
}
