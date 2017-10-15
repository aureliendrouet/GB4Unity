
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        #region 8 bits loads
        // r = r           (put register to register)
        static Instruction LDrr_bb = (_) => { };
        static Instruction LDrr_bc = (_) => { _.b = _.c; };
        static Instruction LDrr_bd = (_) => { _.b = _.d; };
        static Instruction LDrr_be = (_) => { _.b = _.e; };
        static Instruction LDrr_bh = (_) => { _.b = _.h; };
        static Instruction LDrr_bl = (_) => { _.b = _.l; };
        static Instruction LDrr_ba = (_) => { _.b = _.a; };
        static Instruction LDrr_cb = (_) => { _.c = _.b; };
        static Instruction LDrr_cc = (_) => {            };
        static Instruction LDrr_cd = (_) => { _.c = _.d; };
        static Instruction LDrr_ce = (_) => { _.c = _.e; };
        static Instruction LDrr_ch = (_) => { _.c = _.h; };
        static Instruction LDrr_cl = (_) => { _.c = _.l; };
        static Instruction LDrr_ca = (_) => { _.c = _.a; };
        static Instruction LDrr_db = (_) => { _.d = _.b; };
        static Instruction LDrr_dc = (_) => { _.d = _.c; };
        static Instruction LDrr_dd = (_) => {            };
        static Instruction LDrr_de = (_) => { _.d = _.e; };
        static Instruction LDrr_dh = (_) => { _.d = _.h; };
        static Instruction LDrr_dl = (_) => { _.d = _.l; };
        static Instruction LDrr_da = (_) => { _.d = _.a; };
        static Instruction LDrr_eb = (_) => { _.e = _.b; };
        static Instruction LDrr_ec = (_) => { _.e = _.c; };
        static Instruction LDrr_ed = (_) => { _.e = _.d; };
        static Instruction LDrr_ee = (_) => {            };
        static Instruction LDrr_eh = (_) => { _.e = _.h; };
        static Instruction LDrr_el = (_) => { _.e = _.l; };
        static Instruction LDrr_ea = (_) => { _.e = _.a; };
        static Instruction LDrr_hb = (_) => { _.h = _.b; };
        static Instruction LDrr_hc = (_) => { _.h = _.c; };
        static Instruction LDrr_hd = (_) => { _.h = _.d; };
        static Instruction LDrr_he = (_) => { _.h = _.e; };
        static Instruction LDrr_hh = (_) => {            };
        static Instruction LDrr_hl = (_) => { _.h = _.l; };
        static Instruction LDrr_ha = (_) => { _.h = _.a; };
        static Instruction LDrr_lb = (_) => { _.l = _.b; };
        static Instruction LDrr_lc = (_) => { _.l = _.c; };
        static Instruction LDrr_ld = (_) => { _.l = _.d; };
        static Instruction LDrr_le = (_) => { _.l = _.e; };
        static Instruction LDrr_lh = (_) => { _.l = _.h; };
        static Instruction LDrr_ll = (_) => {            };
        static Instruction LDrr_la = (_) => { _.l = _.a; };
        static Instruction LDrr_ab = (_) => { _.a = _.b; };
        static Instruction LDrr_ac = (_) => { _.a = _.c; };
        static Instruction LDrr_ad = (_) => { _.a = _.d; };
        static Instruction LDrr_ae = (_) => { _.a = _.e; };
        static Instruction LDrr_ah = (_) => { _.a = _.h; };
        static Instruction LDrr_al = (_) => { _.a = _.l; };
        static Instruction LDrr_aa = (_) => {            };

        // r = @hl          (put memory to register)
        static Instruction LDrHLm_b = (_) => { _.b = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_c = (_) => { _.c = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_d = (_) => { _.d = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_e = (_) => { _.e = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_h = (_) => { _.h = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_l = (_) => { _.l = _.mmu.rb(_.hl); };
        static Instruction LDrHLm_a = (_) => { _.a = _.mmu.rb(_.hl); };

        static Instruction LDABCm = (_) => { _.a = _.mmu.rb(_.bc); };
        static Instruction LDADEm = (_) => { _.a = _.mmu.rb(_.de); };

        // @hl = r          (put register to memory)
        static Instruction LDHLmr_b = (_) => { _.mmu.wb(_.hl, _.b); };
        static Instruction LDHLmr_c = (_) => { _.mmu.wb(_.hl, _.c); };
        static Instruction LDHLmr_d = (_) => { _.mmu.wb(_.hl, _.d); };
        static Instruction LDHLmr_e = (_) => { _.mmu.wb(_.hl, _.e); };
        static Instruction LDHLmr_h = (_) => { _.mmu.wb(_.hl, _.h); };
        static Instruction LDHLmr_l = (_) => { _.mmu.wb(_.hl, _.l); };
        static Instruction LDHLmr_a = (_) => { _.mmu.wb(_.hl, _.a); };

        static Instruction LDBCmA = (_) => { _.mmu.wb(_.bc, _.a); };
        static Instruction LDDEmA = (_) => { _.mmu.wb(_.de, _.a); };

        // r = @pc         (put memory to register)
        static Instruction LDrn_b = (_) => { _.b = _.mmu.rb(_.pc++); };
        static Instruction LDrn_c = (_) => { _.c = _.mmu.rb(_.pc++); };
        static Instruction LDrn_d = (_) => { _.d = _.mmu.rb(_.pc++); };
        static Instruction LDrn_e = (_) => { _.e = _.mmu.rb(_.pc++); };
        static Instruction LDrn_h = (_) => { _.h = _.mmu.rb(_.pc++); };
        static Instruction LDrn_l = (_) => { _.l = _.mmu.rb(_.pc++); };
        static Instruction LDrn_a = (_) => { _.a = _.mmu.rb(_.pc++); };

        // @hl = @pc        (put memory to memory)
        static Instruction LDHLmn = (_) => { _.mmu.wb(_.hl, _.mmu.rb(_.pc++)); };

        // @@pc = a        (put memory to memory)
        static Instruction LDmmA  = (_) => { _.mmu.wb(_.mmu.rw(_.pc), _.a); _.pc += 2; };

        // a = @@pc        (put memory to register)
        static Instruction LDAmm = (_) => { _.a = _.mmu.rb(_.mmu.rw(_.pc)); _.pc += 2;  };

        // @hl = a; hl++ || hl--
        static Instruction LDHLIA = (_) => { _.mmu.wb(_.hl, _.a); _.hl ++; };
        static Instruction LDHLDA = (_) => { _.mmu.wb(_.hl, _.a); _.hl --; };

        // a = @hl; hl++ || hl--
        static Instruction LDAHLI = (_) => { _.a = _.mmu.rb(_.hl); _.hl ++; };
        static Instruction LDAHLD = (_) => { _.a = _.mmu.rb(_.hl); _.hl --; };

        // a = @@pc        (put memory to register)
        static Instruction LDAIOn = (_) => { _.a = _.mmu.rb(_.mmu.rb(_.pc++)); };

        // @@pc = a        (put register to memory)
        static Instruction LDIOnA = (_) => { _.mmu.wb(_.mmu.rb(_.pc++), _.a); };

        // a = @c          (put memory to register)
        static Instruction LDAIOC = (_) => { _.a = _.mmu.rb(_.c); };

        // @c = a          (put register to memory)
        static Instruction LDIOCA = (_) => { _.mmu.wb(_.c, _.a); };
        #endregion

        #region 16 bits loads
        // rr = @pc        (put memory to register)
        static Instruction LDBCnn = (_) => { _.bc = _.mmu.rw(_.pc); _.pc += 2;  };
        static Instruction LDDEnn = (_) => { _.de = _.mmu.rw(_.pc); _.pc += 2;  };
        static Instruction LDHLnn = (_) => { _.hl = _.mmu.rw(_.pc); _.pc += 2;  };

        // sp = @pc        (put memory to stack pointer)
        static Instruction LDSPnn = (_) => { _.sp = _.mmu.rw(_.pc); _.pc += 2;  };

        // @pc = sp        (put stack pointer to memory)
        static Instruction LDmmSP = (_) => { _.mmu.ww(_.mmu.rw(_.pc), _.sp); _.pc += 2; };

        // hl = sp + n
        static Instruction LDHLSPn = (_) => {
            byte n = _.mmu.rb(_.pc++); ushort res = (ushort)(_.sp + n); _.hl = res;
            _.zf = false; _.sf = false; _.hcf = ((_.sp ^ n ^ res) & 0x10) == 0x10; _.cf = ((_.sp ^ n ^ res) & 0x100) == 0x100;
        };

        // sp = hl         (put hl to sp
        static Instruction LDSPHL = (_) => { _.sp = _.hl; };
        #endregion
    }
}
