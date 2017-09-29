
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction ADDr_b = (_) => { ADD(_, _.b); };
        static Instruction ADDr_c = (_) => { ADD(_, _.c); };
        static Instruction ADDr_d = (_) => { ADD(_, _.d); };
        static Instruction ADDr_e = (_) => { ADD(_, _.e); };
        static Instruction ADDr_h = (_) => { ADD(_, _.h); };
        static Instruction ADDr_l = (_) => { ADD(_, _.l); };
        static Instruction ADDr_a = (_) => { ADD(_, _.a); };
        static Instruction ADDHL  = (_) => { ADD(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction ADDn   = (_) => { ADD(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction ADDHLBC = (_) => { ADDW(_, _.bc); };
        static Instruction ADDHLDE = (_) => { ADDW(_, _.de); };
        static Instruction ADDHLHL = (_) => { ADDW(_, _.hl); };
        static Instruction ADDHLSP = (_) => { ADDW(_, _.sp); };

        static Instruction ADDSPn = (_) => { byte n = _.mmu.rb(_.pc); ushort sp = _.sp; int i = sp + n; _.sp = (ushort)i; _.zf = false; _.sf = false; _.hcf = ((sp ^ n ^ (i & 0xFFFF)) & 0x0010) == 0x0010; _.cf = ((sp ^ n ^ (i & 0xFFFF)) & 0x0100) == 0x0100; _.pc++; _.m = 4; };

        static Instruction ADCr_b = (_) => { ADC(_, _.b); };
        static Instruction ADCr_c = (_) => { ADC(_, _.c); };
        static Instruction ADCr_d = (_) => { ADC(_, _.d); };
        static Instruction ADCr_e = (_) => { ADC(_, _.e); };
        static Instruction ADCr_h = (_) => { ADC(_, _.h); };
        static Instruction ADCr_l = (_) => { ADC(_, _.l); };
        static Instruction ADCr_a = (_) => { ADC(_, _.a); };

        static Instruction ADCHL = (_) => { ADC(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction ADCn  = (_) => { ADC(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction SUBr_b = (_) => { SUB(_, _.b); };
        static Instruction SUBr_c = (_) => { SUB(_, _.c); };
        static Instruction SUBr_d = (_) => { SUB(_, _.d); };
        static Instruction SUBr_e = (_) => { SUB(_, _.e); };
        static Instruction SUBr_h = (_) => { SUB(_, _.h); };
        static Instruction SUBr_l = (_) => { SUB(_, _.l); };
        static Instruction SUBr_a = (_) => { SUB(_, _.a); };
        static Instruction SUBHL  = (_) => { SUB(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction SUBn   = (_) => { SUB(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction SBCr_b = (_) => { SBC(_, _.b); };
        static Instruction SBCr_c = (_) => { SBC(_, _.c); };
        static Instruction SBCr_d = (_) => { SBC(_, _.d); };
        static Instruction SBCr_e = (_) => { SBC(_, _.e); };
        static Instruction SBCr_h = (_) => { SBC(_, _.h); };
        static Instruction SBCr_l = (_) => { SBC(_, _.l); };
        static Instruction SBCr_a = (_) => { SBC(_, _.a); };
        static Instruction SBCHL  = (_) => { SBC(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction SBCn   = (_) => { SBC(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction ANDr_b = (_) => { AND(_, _.b); };
        static Instruction ANDr_c = (_) => { AND(_, _.c); };
        static Instruction ANDr_d = (_) => { AND(_, _.d); };
        static Instruction ANDr_e = (_) => { AND(_, _.e); };
        static Instruction ANDr_h = (_) => { AND(_, _.h); };
        static Instruction ANDr_l = (_) => { AND(_, _.l); };
        static Instruction ANDr_a = (_) => { AND(_, _.a); };
        static Instruction ANDHL  = (_) => { AND(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction ANDn   = (_) => { AND(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction ORr_b = (_) => { OR(_, _.b); };
        static Instruction ORr_c = (_) => { OR(_, _.c); };
        static Instruction ORr_d = (_) => { OR(_, _.d); };
        static Instruction ORr_e = (_) => { OR(_, _.e); };
        static Instruction ORr_h = (_) => { OR(_, _.h); };
        static Instruction ORr_l = (_) => { OR(_, _.l); };
        static Instruction ORr_a = (_) => { OR(_, _.a); };
        static Instruction ORHL  = (_) => { OR(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction ORn   = (_) => { OR(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction XORr_b = (_) => { XOR(_, _.b); };
        static Instruction XORr_c = (_) => { XOR(_, _.c); };
        static Instruction XORr_d = (_) => { XOR(_, _.d); };
        static Instruction XORr_e = (_) => { XOR(_, _.e); };
        static Instruction XORr_h = (_) => { XOR(_, _.h); };
        static Instruction XORr_l = (_) => { XOR(_, _.l); };
        static Instruction XORr_a = (_) => { XOR(_, _.a); };
        static Instruction XORHL  = (_) => { XOR(_, _.mmu.rb(_.hl));   _.m = 2; };
        static Instruction XORn   = (_) => { XOR(_, _.mmu.rb(_.pc++)); _.m = 2; };

        static Instruction INCr_b = (_) => { _.b = INC(_, _.b); };
        static Instruction INCr_c = (_) => { _.c = INC(_, _.c); };
        static Instruction INCr_d = (_) => { _.d = INC(_, _.d); };
        static Instruction INCr_e = (_) => { _.e = INC(_, _.e); };
        static Instruction INCr_h = (_) => { _.h = INC(_, _.h); };
        static Instruction INCr_l = (_) => { _.l = INC(_, _.l); };
        static Instruction INCr_a = (_) => { _.a = INC(_, _.a); };

        static Instruction INCHLm = (_) => { _.mmu.wb(_.hl, INC(_, _.mmu.rb(_.hl))); _.m = 3; };

        static Instruction INCBC = (_) => { _.bc ++; };
        static Instruction INCDE = (_) => { _.de ++; };
        static Instruction INCHL = (_) => { _.hl ++; };
        static Instruction INCSP = (_) => { _.sp ++; };

        static Instruction DECr_b = (_) => { _.b = DEC(_, _.b); };
        static Instruction DECr_c = (_) => { _.c = DEC(_, _.c); };
        static Instruction DECr_d = (_) => { _.d = DEC(_, _.d); };
        static Instruction DECr_e = (_) => { _.e = DEC(_, _.e); };
        static Instruction DECr_h = (_) => { _.h = DEC(_, _.h); };
        static Instruction DECr_l = (_) => { _.l = DEC(_, _.l); };
        static Instruction DECr_a = (_) => { _.a = DEC(_, _.a); };

        static Instruction DECHLm = (_) => { _.mmu.wb(_.hl, DEC(_, _.mmu.rb(_.hl))); _.m = 3; };

        static Instruction DECBC = (_) => { _.bc --; };
        static Instruction DECDE = (_) => { _.de --; };
        static Instruction DECHL = (_) => { _.hl --; };
        static Instruction DECSP = (_) => { _.sp --; };

        static void ADD(Cpu _, byte n) { byte a = _.a; _.a = (byte)(a + n); _.zf = _.a == 0; _.sf = false; _.hcf = (((a & 0x0F) + (n & 0x0F)) & 0x10) != 0; _.cf = (a + n) > 0xFF; _.m = 1; }

        static void ADDW(Cpu _, ushort w) { ushort hl = _.hl; _.hl = (ushort)(hl + w); _.sf = false; _.hcf = (((hl & 0x0FFF) + (w & 0x0FFF)) & 0x1000) != 0; _.cf = (hl + w) > 0xFFFF; _.m = 3; }

        static void ADC(Cpu _, byte n) { int c = _.cf ? 1 : 0; int i = _.a + n + c; _.zf = ((byte)i) == 0; _.sf = false; _.hcf = ((_.a & 0x0F) + (n & 0x0F) + c) > 0x0F; _.cf = i > 0xFF; _.a = (byte)i; _.m = 2; }

        static void SUB(Cpu _, byte n) { byte a = _.a; _.a = (byte)(a - n); _.zf = _.a == 0; _.sf = true; _.hcf = ((_.a ^ n ^ a) & 0x10) != 0; _.cf = (a - n) < 0; _.m = 1; }

        static void SBC(Cpu _, byte n) { int c = _.cf ? 1 : 0; int i = _.a - n - c; _.zf = ((byte)i) == 0; _.sf = true; _.hcf = (((_.a & 0x0F) - (n & 0x0F) - c) < 0x00); _.cf = i < 0x00; _.a = (byte)i;_.m = 1; }

        static void AND(Cpu _, byte n) { _.a &= n; _.zf = _.a == 0; _.sf = false; _.hcf = true; _.cf = false; _.m = 1; }

        static void OR(Cpu _, byte n)  { _.a |= n; _.zf = _.a == 0; _.sf = false; _.hcf = true; _.cf = false;_.m = 1; }

        static void XOR(Cpu _, byte n) { _.a ^= n; _.zf = _.a == 0; _.sf = false; _.hcf = true; _.cf = false;_.m = 1; }

        static byte INC(Cpu _, byte r) { r ++; _.zf = r == 0; _.sf = false; _.hcf = (r & 0x0F) == 0x0F; _.m = 1; return r; }

        static byte DEC(Cpu _, byte r) { r --; _.zf = r == 0; _.sf = true;  _.hcf = (r & 0x0F) == 0x0F; _.m = 1; return r; }
    }
}
