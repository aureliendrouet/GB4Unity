
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        static Instruction BIT0b = (_) => { BIT(_, _.b & 0x01); };
        static Instruction BIT0c = (_) => { BIT(_, _.c & 0x01); };
        static Instruction BIT0d = (_) => { BIT(_, _.d & 0x01); };
        static Instruction BIT0e = (_) => { BIT(_, _.e & 0x01); };
        static Instruction BIT0h = (_) => { BIT(_, _.h & 0x01); };
        static Instruction BIT0l = (_) => { BIT(_, _.l & 0x01); };
        static Instruction BIT0a = (_) => { BIT(_, _.a & 0x01); };
        static Instruction BIT0m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x01, 3); };

        static Instruction BIT1b = (_) => { BIT(_, _.b & 0x02); };
        static Instruction BIT1c = (_) => { BIT(_, _.c & 0x02); };
        static Instruction BIT1d = (_) => { BIT(_, _.d & 0x02); };
        static Instruction BIT1e = (_) => { BIT(_, _.e & 0x02); };
        static Instruction BIT1h = (_) => { BIT(_, _.h & 0x02); };
        static Instruction BIT1l = (_) => { BIT(_, _.l & 0x02); };
        static Instruction BIT1a = (_) => { BIT(_, _.a & 0x02); };
        static Instruction BIT1m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x02, 3); };

        static Instruction BIT2b = (_) => { BIT(_, _.b & 0x04); };
        static Instruction BIT2c = (_) => { BIT(_, _.c & 0x04); };
        static Instruction BIT2d = (_) => { BIT(_, _.d & 0x04); };
        static Instruction BIT2e = (_) => { BIT(_, _.e & 0x04); };
        static Instruction BIT2h = (_) => { BIT(_, _.h & 0x04); };
        static Instruction BIT2l = (_) => { BIT(_, _.l & 0x04); };
        static Instruction BIT2a = (_) => { BIT(_, _.a & 0x04); };
        static Instruction BIT2m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x04, 3); };

        static Instruction BIT3b = (_) => { BIT(_, _.b & 0x08); };
        static Instruction BIT3c = (_) => { BIT(_, _.c & 0x08); };
        static Instruction BIT3d = (_) => { BIT(_, _.d & 0x08); };
        static Instruction BIT3e = (_) => { BIT(_, _.e & 0x08); };
        static Instruction BIT3h = (_) => { BIT(_, _.h & 0x08); };
        static Instruction BIT3l = (_) => { BIT(_, _.l & 0x08); };
        static Instruction BIT3a = (_) => { BIT(_, _.a & 0x08); };
        static Instruction BIT3m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x08, 3); };

        static Instruction BIT4b = (_) => { BIT(_, _.b & 0x10); };
        static Instruction BIT4c = (_) => { BIT(_, _.c & 0x10); };
        static Instruction BIT4d = (_) => { BIT(_, _.d & 0x10); };
        static Instruction BIT4e = (_) => { BIT(_, _.e & 0x10); };
        static Instruction BIT4h = (_) => { BIT(_, _.h & 0x10); };
        static Instruction BIT4l = (_) => { BIT(_, _.l & 0x10); };
        static Instruction BIT4a = (_) => { BIT(_, _.a & 0x10); };
        static Instruction BIT4m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x10, 3); };

        static Instruction BIT5b = (_) => { BIT(_, _.b & 0x20); };
        static Instruction BIT5c = (_) => { BIT(_, _.c & 0x20); };
        static Instruction BIT5d = (_) => { BIT(_, _.d & 0x20); };
        static Instruction BIT5e = (_) => { BIT(_, _.e & 0x20); };
        static Instruction BIT5h = (_) => { BIT(_, _.h & 0x20); };
        static Instruction BIT5l = (_) => { BIT(_, _.l & 0x20); };
        static Instruction BIT5a = (_) => { BIT(_, _.a & 0x20); };
        static Instruction BIT5m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x20, 3); };

        static Instruction BIT6b = (_) => { BIT(_, _.b & 0x40); };
        static Instruction BIT6c = (_) => { BIT(_, _.c & 0x40); };
        static Instruction BIT6d = (_) => { BIT(_, _.d & 0x40); };
        static Instruction BIT6e = (_) => { BIT(_, _.e & 0x40); };
        static Instruction BIT6h = (_) => { BIT(_, _.h & 0x40); };
        static Instruction BIT6l = (_) => { BIT(_, _.l & 0x40); };
        static Instruction BIT6a = (_) => { BIT(_, _.a & 0x40); };
        static Instruction BIT6m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x40, 3); };

        static Instruction BIT7b = (_) => { BIT(_, _.b & 0x80); };
        static Instruction BIT7c = (_) => { BIT(_, _.c & 0x80); };
        static Instruction BIT7d = (_) => { BIT(_, _.d & 0x80); };
        static Instruction BIT7e = (_) => { BIT(_, _.e & 0x80); };
        static Instruction BIT7h = (_) => { BIT(_, _.h & 0x80); };
        static Instruction BIT7l = (_) => { BIT(_, _.l & 0x80); };
        static Instruction BIT7a = (_) => { BIT(_, _.a & 0x80); };
        static Instruction BIT7m = (_) => { BIT(_, _.mmu.rb(_.hl) & 0x80, 3); };

        static Instruction RES0b  = (_) => { RES(_, 0xFE, ref _.b); };
        static Instruction RES0c  = (_) => { RES(_, 0xFE, ref _.c); };
        static Instruction RES0d  = (_) => { RES(_, 0xFE, ref _.d); };
        static Instruction RES0e  = (_) => { RES(_, 0xFE, ref _.e); };
        static Instruction RES0h  = (_) => { RES(_, 0xFE, ref _.h); };
        static Instruction RES0l  = (_) => { RES(_, 0xFE, ref _.l); };
        static Instruction RES0hl = (_) => { RESHL(_, 0xFE); };
        static Instruction RES0a  = (_) => { RES(_, 0xFE, ref _.a); };

        static Instruction RES1b  = (_) => { RES(_, 0xFD, ref _.b); };
        static Instruction RES1c  = (_) => { RES(_, 0xFD, ref _.c); };
        static Instruction RES1d  = (_) => { RES(_, 0xFD, ref _.d); };
        static Instruction RES1e  = (_) => { RES(_, 0xFD, ref _.e); };
        static Instruction RES1h  = (_) => { RES(_, 0xFD, ref _.h); };
        static Instruction RES1l  = (_) => { RES(_, 0xFD, ref _.l); };
        static Instruction RES1hl = (_) => { RESHL(_, 0xFD); };
        static Instruction RES1a  = (_) => { RES(_, 0xFD, ref _.a); };

        static Instruction RES2b  = (_) => { RES(_, 0xFB, ref _.b); };
        static Instruction RES2c  = (_) => { RES(_, 0xFB, ref _.c); };
        static Instruction RES2d  = (_) => { RES(_, 0xFB, ref _.d); };
        static Instruction RES2e  = (_) => { RES(_, 0xFB, ref _.e); };
        static Instruction RES2h  = (_) => { RES(_, 0xFB, ref _.h); };
        static Instruction RES2l  = (_) => { RES(_, 0xFB, ref _.l); };
        static Instruction RES2hl = (_) => { RESHL(_, 0xFB); };
        static Instruction RES2a  = (_) => { RES(_, 0xFB, ref _.a); };

        static Instruction RES3b  = (_) => { RES(_, 0xF7, ref _.b); };
        static Instruction RES3c  = (_) => { RES(_, 0xF7, ref _.c); };
        static Instruction RES3d  = (_) => { RES(_, 0xF7, ref _.d); };
        static Instruction RES3e  = (_) => { RES(_, 0xF7, ref _.e); };
        static Instruction RES3h  = (_) => { RES(_, 0xF7, ref _.h); };
        static Instruction RES3l  = (_) => { RES(_, 0xF7, ref _.l); };
        static Instruction RES3hl = (_) => { RESHL(_, 0xF7); };
        static Instruction RES3a  = (_) => { RES(_, 0xF7, ref _.a); };

        static Instruction RES4b  = (_) => { RES(_, 0xEF, ref _.b); };
        static Instruction RES4c  = (_) => { RES(_, 0xEF, ref _.c); };
        static Instruction RES4d  = (_) => { RES(_, 0xEF, ref _.d); };
        static Instruction RES4e  = (_) => { RES(_, 0xEF, ref _.e); };
        static Instruction RES4h  = (_) => { RES(_, 0xEF, ref _.h); };
        static Instruction RES4l  = (_) => { RES(_, 0xEF, ref _.l); };
        static Instruction RES4hl = (_) => { RESHL(_, 0xEF); };
        static Instruction RES4a  = (_) => { RES(_, 0xEF, ref _.a); };

        static Instruction RES5b  = (_) => { RES(_, 0xDF, ref _.b); };
        static Instruction RES5c  = (_) => { RES(_, 0xDF, ref _.c); };
        static Instruction RES5d  = (_) => { RES(_, 0xDF, ref _.d); };
        static Instruction RES5e  = (_) => { RES(_, 0xDF, ref _.e); };
        static Instruction RES5h  = (_) => { RES(_, 0xDF, ref _.h); };
        static Instruction RES5l  = (_) => { RES(_, 0xDF, ref _.l); };
        static Instruction RES5hl = (_) => { RESHL(_, 0xDF); };
        static Instruction RES5a  = (_) => { RES(_, 0xDF, ref _.a); };

        static Instruction RES6b  = (_) => { RES(_, 0xBF, ref _.b); };
        static Instruction RES6c  = (_) => { RES(_, 0xBF, ref _.c); };
        static Instruction RES6d  = (_) => { RES(_, 0xBF, ref _.d); };
        static Instruction RES6e  = (_) => { RES(_, 0xBF, ref _.e); };
        static Instruction RES6h  = (_) => { RES(_, 0xBF, ref _.h); };
        static Instruction RES6l  = (_) => { RES(_, 0xBF, ref _.l); };
        static Instruction RES6hl = (_) => { RESHL(_, 0xBF); };
        static Instruction RES6a  = (_) => { RES(_, 0xBF, ref _.a); };

        static Instruction RES7b  = (_) => { RES(_, 0x7F, ref _.b); };
        static Instruction RES7c  = (_) => { RES(_, 0x7F, ref _.c); };
        static Instruction RES7d  = (_) => { RES(_, 0x7F, ref _.d); };
        static Instruction RES7e  = (_) => { RES(_, 0x7F, ref _.e); };
        static Instruction RES7h  = (_) => { RES(_, 0x7F, ref _.h); };
        static Instruction RES7l  = (_) => { RES(_, 0x7F, ref _.l); };
        static Instruction RES7hl = (_) => { RESHL(_, 0x7F); };
        static Instruction RES7a  = (_) => { RES(_, 0x7F, ref _.a); };

        static Instruction SET0b  = (_) => { SET(_, 0x01, ref _.b); };
        static Instruction SET0c  = (_) => { SET(_, 0x01, ref _.c); };
        static Instruction SET0d  = (_) => { SET(_, 0x01, ref _.d); };
        static Instruction SET0e  = (_) => { SET(_, 0x01, ref _.e); };
        static Instruction SET0h  = (_) => { SET(_, 0x01, ref _.h); };
        static Instruction SET0l  = (_) => { SET(_, 0x01, ref _.l); };
        static Instruction SET0hl = (_) => { SETHL(_, 0x01); };
        static Instruction SET0a  = (_) => { SET(_, 0x01, ref _.a); };

        static Instruction SET1b  = (_) => { SET(_, 0x02, ref _.b); };
        static Instruction SET1c  = (_) => { SET(_, 0x02, ref _.c); };
        static Instruction SET1d  = (_) => { SET(_, 0x02, ref _.d); };
        static Instruction SET1e  = (_) => { SET(_, 0x02, ref _.e); };
        static Instruction SET1h  = (_) => { SET(_, 0x02, ref _.h); };
        static Instruction SET1l  = (_) => { SET(_, 0x02, ref _.l); };
        static Instruction SET1hl = (_) => { SETHL(_, 0x02); };
        static Instruction SET1a  = (_) => { SET(_, 0x02, ref _.a); };

        static Instruction SET2b  = (_) => { SET(_, 0x04, ref _.b); };
        static Instruction SET2c  = (_) => { SET(_, 0x04, ref _.c); };
        static Instruction SET2d  = (_) => { SET(_, 0x04, ref _.d); };
        static Instruction SET2e  = (_) => { SET(_, 0x04, ref _.e); };
        static Instruction SET2h  = (_) => { SET(_, 0x04, ref _.h); };
        static Instruction SET2l  = (_) => { SET(_, 0x04, ref _.l); };
        static Instruction SET2hl = (_) => { SETHL(_, 0x04); };
        static Instruction SET2a  = (_) => { SET(_, 0x04, ref _.a); };

        static Instruction SET3b  = (_) => { SET(_, 0x08, ref _.b); };
        static Instruction SET3c  = (_) => { SET(_, 0x08, ref _.c); };
        static Instruction SET3d  = (_) => { SET(_, 0x08, ref _.d); };
        static Instruction SET3e  = (_) => { SET(_, 0x08, ref _.e); };
        static Instruction SET3h  = (_) => { SET(_, 0x08, ref _.h); };
        static Instruction SET3l  = (_) => { SET(_, 0x08, ref _.l); };
        static Instruction SET3hl = (_) => { SETHL(_, 0x08); };
        static Instruction SET3a  = (_) => { SET(_, 0x08, ref _.a); };

        static Instruction SET4b  = (_) => { SET(_, 0x10, ref _.b); };
        static Instruction SET4c  = (_) => { SET(_, 0x10, ref _.c); };
        static Instruction SET4d  = (_) => { SET(_, 0x10, ref _.d); };
        static Instruction SET4e  = (_) => { SET(_, 0x10, ref _.e); };
        static Instruction SET4h  = (_) => { SET(_, 0x10, ref _.h); };
        static Instruction SET4l  = (_) => { SET(_, 0x10, ref _.l); };
        static Instruction SET4hl = (_) => { SETHL(_, 0x10); };
        static Instruction SET4a  = (_) => { SET(_, 0x10, ref _.a); };

        static Instruction SET5b  = (_) => { SET(_, 0x20, ref _.b); };
        static Instruction SET5c  = (_) => { SET(_, 0x20, ref _.c); };
        static Instruction SET5d  = (_) => { SET(_, 0x20, ref _.d); };
        static Instruction SET5e  = (_) => { SET(_, 0x20, ref _.e); };
        static Instruction SET5h  = (_) => { SET(_, 0x20, ref _.h); };
        static Instruction SET5l  = (_) => { SET(_, 0x20, ref _.l); };
        static Instruction SET5hl = (_) => { SETHL(_, 0x20); };
        static Instruction SET5a  = (_) => { SET(_, 0x20, ref _.a); };

        static Instruction SET6b  = (_) => { SET(_, 0x40, ref _.b); };
        static Instruction SET6c  = (_) => { SET(_, 0x40, ref _.c); };
        static Instruction SET6d  = (_) => { SET(_, 0x40, ref _.d); };
        static Instruction SET6e  = (_) => { SET(_, 0x40, ref _.e); };
        static Instruction SET6h  = (_) => { SET(_, 0x40, ref _.h); };
        static Instruction SET6l  = (_) => { SET(_, 0x40, ref _.l); };
        static Instruction SET6hl = (_) => { SETHL(_, 0x40); };
        static Instruction SET6a  = (_) => { SET(_, 0x40, ref _.a); };

        static Instruction SET7b  = (_) => { SET(_, 0x80, ref _.b); };
        static Instruction SET7c  = (_) => { SET(_, 0x80, ref _.c); };
        static Instruction SET7d  = (_) => { SET(_, 0x80, ref _.d); };
        static Instruction SET7e  = (_) => { SET(_, 0x80, ref _.e); };
        static Instruction SET7h  = (_) => { SET(_, 0x80, ref _.h); };
        static Instruction SET7l  = (_) => { SET(_, 0x80, ref _.l); };
        static Instruction SET7hl = (_) => { SETHL(_, 0x80); };
        static Instruction SET7a  = (_) => { SET(_, 0x80, ref _.a); };

        static void BIT  (Cpu _, int v, int t = 2) { _.zf = v == 0; _.sf = false; _.hcf = true; _.m = t; }

        static void RES  (Cpu _, int v, ref byte r) { r = (byte) (r & v); _.m = 2; }

        static void SET  (Cpu _, int v, ref byte r) { r = (byte) (r | v); _.m = 2; }

        static void RESHL(Cpu _, int v) { _.mmu.wb(_.hl, (byte)(_.mmu.rb(_.hl) & v)); _.m = 4; }

        static void SETHL(Cpu _, int v) { _.mmu.wb(_.hl, (byte)(_.mmu.rb(_.hl) | v)); _.m = 4; }
    }
}
