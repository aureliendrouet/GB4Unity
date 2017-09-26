using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        // Single Byte Instruction Set
        // 0xXX
        static Instruction[] map = new Instruction[] {
            /*      0         1         2         3         4         5         6         7         8         9         A         B         C         D         E         F         */
            /* 0 */ NOP,      LDBCnn,   LDBCmA,   INCBC,    INCr_b,   DECr_b,   LDrn_b,   RLCA,     LDmmSP,   ADDHLBC,  LDABCm,   DECBC,    INCr_c,   DECr_c,   LDrn_c,   RRCA,
            /* 1 */ STOP,     LDDEnn,   LDDEmA,   INCDE,    INCr_d,   DECr_d,   LDrn_d,   RLA,      JRn,      ADDHLDE,  LDADEm,   DECDE,    INCr_e,   DECr_e,   LDrn_e,   RRA,
            /* 2 */ JRNZn,    LDHLnn,   LDHLIA,   INCHL,    INCr_h,   DECr_h,   LDrn_h,   DAA,      JRZn,     ADDHLHL,  LDAHLI,   DECHL,    INCr_l,   DECr_l,   LDrn_l,   CPL,
            /* 3 */ JRNCn,    LDSPnn,   LDHLDA,   INCSP,    INCHLm,   DECHLm,   LDHLmn,   SCF,      JRCn,     ADDHLSP,  LDAHLD,   DECSP,    INCr_a,   DECr_a,   LDrn_a,   CCF,
            /* 4 */ LDrr_bb,  LDrr_bc,  LDrr_bd,  LDrr_be,  LDrr_bh,  LDrr_bl,  LDrHLm_b, LDrr_ba,  LDrr_cb,  LDrr_cc,  LDrr_cd,  LDrr_ce,  LDrr_ch,  LDrr_cl,  LDrHLm_c, LDrr_ca,
            /* 5 */ LDrr_db,  LDrr_dc,  LDrr_dd,  LDrr_de,  LDrr_dh,  LDrr_dl,  LDrHLm_d, LDrr_da,  LDrr_eb,  LDrr_ec,  LDrr_ed,  LDrr_ee,  LDrr_eh,  LDrr_el,  LDrHLm_e, LDrr_ea,
            /* 6 */ LDrr_hb,  LDrr_hc,  LDrr_hd,  LDrr_he,  LDrr_hh,  LDrr_hl,  LDrHLm_h, LDrr_ha,  LDrr_lb,  LDrr_lc,  LDrr_ld,  LDrr_le,  LDrr_lh,  LDrr_ll,  LDrHLm_l, LDrr_la,
            /* 7 */ LDHLmr_b, LDHLmr_c, LDHLmr_d, LDHLmr_e, LDHLmr_h, LDHLmr_l, HALT,     LDHLmr_a, LDrr_ab,  LDrr_ac,  LDrr_ad,  LDrr_ae,  LDrr_ah,  LDrr_al,  LDrHLm_a, LDrr_aa,
            /* 8 */ ADDr_b,   ADDr_c,   ADDr_d,   ADDr_e,   ADDr_h,   ADDr_l,   ADDHL,    ADDr_a,   ADCr_b,   ADCr_c,   ADCr_d,   ADCr_e,   ADCr_h,   ADCr_l,   ADCHL,    ADCr_a,
            /* 9 */ SUBr_b,   SUBr_c,   SUBr_d,   SUBr_e,   SUBr_h,   SUBr_l,   SUBHL,    SUBr_a,   SBCr_b,   SBCr_c,   SBCr_d,   SBCr_e,   SBCr_h,   SBCr_l,   SBCHL,    SBCr_a,
            /* A */ ANDr_b,   ANDr_c,   ANDr_d,   ANDr_e,   ANDr_h,   ANDr_l,   ANDHL,    ANDr_a,   XORr_b,   XORr_c,   XORr_d,   XORr_e,   XORr_h,   XORr_l,   XORHL,    XORr_a,
            /* B */ ORr_b,    ORr_c,    ORr_d,    ORr_e,    ORr_h,    ORr_l,    ORHL,     ORr_a,    CPr_b,    CPr_c,    CPr_d,    CPr_e,    CPr_h,    CPr_l,    CPHL,     CPr_a,
            /* C */ RETNZ,    POPBC,    JPNZnn,   JPnn,     CALLNZnn, PUSHBC,   ADDn,     RST00,    RETZ,     RETNI,    JPZnn,    MAPcb,    CALLZnn,  CALLnn,   ADCn,     RST08,
            /* D */ RETNC,    POPDE,    JPNCnn,   UNKNOWN,  CALLNCnn, PUSHDE,   SUBn,     RST10,    RETC,     RETI,     JPCnn,    UNKNOWN,  CALLCnn,  UNKNOWN,  SBCn,     RST18,
            /* E */ LDIOnA,   POPHL,    LDIOCA,   UNKNOWN,  UNKNOWN,  PUSHHL,   ANDn,     RST20,    ADDSPn,   JPHL,     LDmmA,    UNKNOWN,  UNKNOWN,  UNKNOWN,  ORn,      RST28,
            /* F */ LDAIOn,   POPAF,    LDAIOC,   DI,       UNKNOWN,  PUSHAF,   XORn,     RST30,    LDHLSPn,  LDSPHL,   LDAmm,    EI,       UNKNOWN,  UNKNOWN,  CPn,      RST38,
        };

        // Prefixed Instruction Set
        // 0xCBXX
        static Instruction[] cbmap = new Instruction[] {
            /*      0         1         2         3         4         5         6         7         8         9         A         B         C         D         E         F         */
            /* 0 */ RLCr_b,   RLCr_c,   RLCr_d,   RLCr_e,   RLCr_h,   RLCr_l,   RLCHL,    RLCr_a,   RRCr_b,   RRCr_c,   RRCr_d,   RRCr_e,   RRCr_h,   RRCr_l,   RRCHL,    RRCr_a,
            /* 1 */ RLr_b,    RLr_c,    RLr_d,    RLr_e,    RLr_h,    RLr_l,    RLHL,     RLr_a,    RRr_b,    RRr_c,    RRr_d,    RRr_e,    RRr_h,    RRr_l,    RRHL,     RRr_a,
            /* 2 */ SLAr_b,   SLAr_c,   SLAr_d,   SLAr_e,   SLAr_h,   SLAr_l,   SLAHL,    SLAr_a,   SRAr_b,   SRAr_c,   SRAr_d,   SRAr_e,   SRAr_h,   SRAr_l,   SRAHL,    SRAr_a,
            /* 3 */ SWAPr_b,  SWAPr_c,  SWAPr_d,  SWAPr_e,  SWAPr_h,  SWAPr_l,  SWAPrHLm, SWAPr_a,  SRLr_b,   SRLr_c,   SRLr_d,   SRLr_e,   SRLr_h,   SRLr_l,   SRLHL,    SRLr_a,
            /* 4 */ BIT0b,    BIT0c,    BIT0d,    BIT0e,    BIT0h,    BIT0l,    BIT0m,    BIT0a,    BIT1b,    BIT1c,    BIT1d,    BIT1e,    BIT1h,    BIT1l,    BIT1m,    BIT1a,
            /* 5 */ BIT2b,    BIT2c,    BIT2d,    BIT2e,    BIT2h,    BIT2l,    BIT2m,    BIT2a,    BIT3b,    BIT3c,    BIT3d,    BIT3e,    BIT3h,    BIT3l,    BIT3m,    BIT3a,
            /* 6 */ BIT4b,    BIT4c,    BIT4d,    BIT4e,    BIT4h,    BIT4l,    BIT4m,    BIT4a,    BIT5b,    BIT5c,    BIT5d,    BIT5e,    BIT5h,    BIT5l,    BIT5m,    BIT5a,
            /* 7 */ BIT6b,    BIT6c,    BIT6d,    BIT6e,    BIT6h,    BIT6l,    BIT6m,    BIT6a,    BIT7b,    BIT7c,    BIT7d,    BIT7e,    BIT7h,    BIT7l,    BIT7m,    BIT7a,
            /* 8 */ RES0b,    RES0c,    RES0d,    RES0e,    RES0h,    RES0l,   RES0hl,    RES0a,    RES1b,    RES1c,    RES1d,    RES1e,    RES1h,    RES1l,   RES1hl,    RES1a,
            /* 9 */ RES2b,    RES2c,    RES2d,    RES2e,    RES2h,    RES2l,   RES2hl,    RES2a,    RES3b,    RES3c,    RES3d,    RES3e,    RES3h,    RES3l,   RES3hl,    RES3a,
            /* A */ RES4b,    RES4c,    RES4d,    RES4e,    RES4h,    RES4l,   RES4hl,    RES4a,    RES5b,    RES5c,    RES5d,    RES5e,    RES5h,    RES5l,   RES5hl,    RES5a,
            /* B */ RES6b,    RES6c,    RES6d,    RES6e,    RES6h,    RES6l,   RES6hl,    RES6a,    RES7b,    RES7c,    RES7d,    RES7e,    RES7h,    RES7l,   RES7hl,    RES7a,
            /* C */ SET0b,    SET0c,    SET0d,    SET0e,    SET0h,    SET0l,   SET0hl,    SET0a,    SET1b,    SET1c,    SET1d,    SET1e,    SET1h,    SET1l,   SET1hl,    SET1a,
            /* D */ SET2b,    SET2c,    SET2d,    SET2e,    SET2h,    SET2l,   SET2hl,    SET2a,    SET3b,    SET3c,    SET3d,    SET3e,    SET3h,    SET3l,   SET3hl,    SET3a,
            /* E */ SET4b,    SET4c,    SET4d,    SET4e,    SET4h,    SET4l,   SET4hl,    SET4a,    SET5b,    SET5c,    SET5d,    SET5e,    SET5h,    SET5l,   SET5hl,    SET5a,
            /* F */ SET6b,    SET6c,    SET6d,    SET6e,    SET6h,    SET6l,   SET6hl,    SET6a,    SET7b,    SET7c,    SET7d,    SET7e,    SET7h,    SET7l,   SET7hl,    SET7a,
        };
    }
}
