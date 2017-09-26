
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        #region 8 bits loads
        // r = r           (load register from register)
        static Instruction LDrr_bb = (_) => { _.m = 1; };
        static Instruction LDrr_bc = (_) => { _.b = _.c; _.m = 1; };
        static Instruction LDrr_bd = (_) => { _.b = _.d; _.m = 1; };
        static Instruction LDrr_be = (_) => { _.b = _.e; _.m = 1; };
        static Instruction LDrr_bh = (_) => { _.b = _.h; _.m = 1; };
        static Instruction LDrr_bl = (_) => { _.b = _.l; _.m = 1; };
        static Instruction LDrr_ba = (_) => { _.b = _.a; _.m = 1; };
        static Instruction LDrr_cb = (_) => { _.c = _.b; _.m = 1; };
        static Instruction LDrr_cc = (_) => {            _.m = 1; };
        static Instruction LDrr_cd = (_) => { _.c = _.d; _.m = 1; };
        static Instruction LDrr_ce = (_) => { _.c = _.e; _.m = 1; };
        static Instruction LDrr_ch = (_) => { _.c = _.h; _.m = 1; };
        static Instruction LDrr_cl = (_) => { _.c = _.l; _.m = 1; };
        static Instruction LDrr_ca = (_) => { _.c = _.a; _.m = 1; };
        static Instruction LDrr_db = (_) => { _.d = _.b; _.m = 1; };
        static Instruction LDrr_dc = (_) => { _.d = _.c; _.m = 1; };
        static Instruction LDrr_dd = (_) => {            _.m = 1; };
        static Instruction LDrr_de = (_) => { _.d = _.e; _.m = 1; };
        static Instruction LDrr_dh = (_) => { _.d = _.h; _.m = 1; };
        static Instruction LDrr_dl = (_) => { _.d = _.l; _.m = 1; };
        static Instruction LDrr_da = (_) => { _.d = _.a; _.m = 1; };
        static Instruction LDrr_eb = (_) => { _.e = _.b; _.m = 1; };
        static Instruction LDrr_ec = (_) => { _.e = _.c; _.m = 1; };
        static Instruction LDrr_ed = (_) => { _.e = _.d; _.m = 1; };
        static Instruction LDrr_ee = (_) => {            _.m = 1; };
        static Instruction LDrr_eh = (_) => { _.e = _.h; _.m = 1; };
        static Instruction LDrr_el = (_) => { _.e = _.l; _.m = 1; };
        static Instruction LDrr_ea = (_) => { _.e = _.a; _.m = 1; };
        static Instruction LDrr_hb = (_) => { _.h = _.b; _.m = 1; };
        static Instruction LDrr_hc = (_) => { _.h = _.c; _.m = 1; };
        static Instruction LDrr_hd = (_) => { _.h = _.d; _.m = 1; };
        static Instruction LDrr_he = (_) => { _.h = _.e; _.m = 1; };
        static Instruction LDrr_hh = (_) => {            _.m = 1; };
        static Instruction LDrr_hl = (_) => { _.h = _.l; _.m = 1; };
        static Instruction LDrr_ha = (_) => { _.h = _.a; _.m = 1; };
        static Instruction LDrr_lb = (_) => { _.l = _.b; _.m = 1; };
        static Instruction LDrr_lc = (_) => { _.l = _.c; _.m = 1; };
        static Instruction LDrr_ld = (_) => { _.l = _.d; _.m = 1; };
        static Instruction LDrr_le = (_) => { _.l = _.e; _.m = 1; };
        static Instruction LDrr_lh = (_) => { _.l = _.h; _.m = 1; };
        static Instruction LDrr_ll = (_) => {            _.m = 1; };
        static Instruction LDrr_la = (_) => { _.l = _.a; _.m = 1; };
        static Instruction LDrr_ab = (_) => { _.a = _.b; _.m = 1; };
        static Instruction LDrr_ac = (_) => { _.a = _.c; _.m = 1; };
        static Instruction LDrr_ad = (_) => { _.a = _.d; _.m = 1; };
        static Instruction LDrr_ae = (_) => { _.a = _.e; _.m = 1; };
        static Instruction LDrr_ah = (_) => { _.a = _.h; _.m = 1; };
        static Instruction LDrr_al = (_) => { _.a = _.l; _.m = 1; };
        static Instruction LDrr_aa = (_) => {            _.m = 1; };

        // r = @r          (load register from memory)
        static Instruction LDrHLm_b = (_) => { _.b = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_c = (_) => { _.c = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_d = (_) => { _.d = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_e = (_) => { _.e = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_h = (_) => { _.h = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_l = (_) => { _.l = _.mmu.rb(_.hl); _.m = 2; };
        static Instruction LDrHLm_a = (_) => { _.a = _.mmu.rb(_.hl); _.m = 2; };

        static Instruction LDABCm = (_) => { _.a = _.mmu.rb(_.bc); _.m = 2; };
        static Instruction LDADEm = (_) => { _.a = _.mmu.rb(_.de); _.m = 2; };

        // @r = r          (write memory from register)
        static Instruction LDHLmr_b = (_) => { _.mmu.wb(_.hl, _.b); _.m = 2; };
        static Instruction LDHLmr_c = (_) => { _.mmu.wb(_.hl, _.c); _.m = 2; };
        static Instruction LDHLmr_d = (_) => { _.mmu.wb(_.hl, _.d); _.m = 2; };
        static Instruction LDHLmr_e = (_) => { _.mmu.wb(_.hl, _.e); _.m = 2; };
        static Instruction LDHLmr_h = (_) => { _.mmu.wb(_.hl, _.h); _.m = 2; };
        static Instruction LDHLmr_l = (_) => { _.mmu.wb(_.hl, _.l); _.m = 2; };
        static Instruction LDHLmr_a = (_) => { _.mmu.wb(_.hl, _.a); _.m = 2; };

        static Instruction LDBCmA = (_) => { _.mmu.wb(_.bc, _.a); _.m = 2; };
        static Instruction LDDEmA = (_) => { _.mmu.wb(_.de, _.a); _.m = 2; };

        // r = @pc         (load register from memory)
        static Instruction LDrn_b = (_) => { _.b = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_c = (_) => { _.c = _.mmu.rb(_.pc); UnityEngine.Debug.LogFormat("{0:x4}", _.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_d = (_) => { _.d = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_e = (_) => { _.e = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_h = (_) => { _.h = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_l = (_) => { _.l = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };
        static Instruction LDrn_a = (_) => { _.a = _.mmu.rb(_.pc); _.pc ++; _.m = 2; };

        // @r = @pc        (write memory from memory)
        static Instruction LDHLmn = (_) => { _.mmu.wb(_.hl, _.mmu.rb(_.pc)); _.pc ++; _.m = 3; };

        // @@pc = r        (write memory from register)
        static Instruction LDmmA  = (_) => { _.mmu.wb(_.mmu.rw(_.pc), _.a); _.pc += 2; _.m = 4; };
      //static Instruction LDmmHL = (_) => { _.mmu.ww(_.mmu.rw(_.pc),_.hl); _.pc += 2; _.m = 5; };

        // r = @@pc        (load register from memory)
        static Instruction LDAmm = (_) => { _.a = _.mmu.rb(_.mmu.rw(_.pc)); _.pc += 2;  _.m = 4; };

        // @hl = a; hl + n
        static Instruction LDHLIA = (_) => { _.mmu.wb(_.hl, _.a); _.hl ++; _.m = 2; };
        static Instruction LDHLDA = (_) => { _.mmu.wb(_.hl, _.a); _.hl --; _.m = 2; };

        // r = @hl; hl + n
        static Instruction LDAHLI = (_) => { _.a = _.mmu.rb(_.hl); _.hl ++; _.m = 2; };
        static Instruction LDAHLD = (_) => { _.a = _.mmu.rb(_.hl); _.hl --; _.m = 2; };

        // r = @pc + 0xff00
        static Instruction LDAIOn = (_) => { _.a = _.mmu.rb(_.mmu.rb(_.pc)); _.pc ++;  _.m = 3; };

        // @pc + 0xff00 = r
        static Instruction LDIOnA = (_) => { _.mmu.wb(_.mmu.rb(_.pc), _.a); _.pc ++;  _.m = 3; };

        // r = @r + 0xff00
        static Instruction LDAIOC = (_) => { _.a = _.mmu.rb(_.c); _.m = 2; };

        // @r + 0xff00 = r
        static Instruction LDIOCA = (_) => { _.mmu.wb(_.c, _.a); _.m = 2; };

        static Instruction LDmmSP = (_) => { ushort w = _.mmu.rw(_.pc); _.pc += 2; _.mmu.ww(w, _.sp); _.m = 5; };
        #endregion

        #region 16 bits loads
        // r = @pc         (load register from memory)
        static Instruction LDBCnn = (_) => { _.bc = _.mmu.rw(_.pc); _.pc += 2;  _.m = 3; };
        static Instruction LDDEnn = (_) => { _.de = _.mmu.rw(_.pc); _.pc += 2;  _.m = 3; };
        static Instruction LDHLnn = (_) => { _.hl = _.mmu.rw(_.pc); _.pc += 2;  _.m = 3; };

        // st = @pc        (load stack pointer from memory)
        static Instruction LDSPnn = (_) => { _.sp = _.mmu.rw(_.pc); _.pc += 2;  _.m = 3; };

        // hl = sp + n
        static Instruction LDHLSPn = (_) => {
            byte n = _.mmu.rb(_.pc); _.pc ++; ushort res = (ushort)(_.sp + n); _.hl = res;
            _.zf = false; _.sf = false; _.hcf = ((_.sp ^ n ^ res) & 0x10) == 0x10; _.cf = ((_.sp ^ n ^ res) & 0x100) == 0x100;
            _.m = 3;
        };

        // sp = hl
        static Instruction LDSPHL = (_) => { _.sp = _.hl; _.m = 2; };
        #endregion
    }
}
