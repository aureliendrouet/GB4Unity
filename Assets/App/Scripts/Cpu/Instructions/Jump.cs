
namespace StudioKurage.Emulator.Gameboy
{
    public partial class Cpu
    {
        // jump
        static Instruction JPnn   = (_) => { _.pc = _.mmu.rw(_.pc); };
        static Instruction JPHL   = (_) => { _.pc = _.hl; };

        // conditional jump
        static Instruction JPNZnn = (_) => { JP(_,  !_.zf); };
        static Instruction JPZnn  = (_) => { JP(_,   _.zf); };
        static Instruction JPNCnn = (_) => { JP(_, !_.cf); };
        static Instruction JPCnn  = (_) => { JP(_,  _.cf); };

        // conditional relative jump with signed byte to move backward
        static Instruction JRn   = (_) => { sbyte i = (sbyte)_.mmu.rb(_.pc++); _.pc += (ushort)i; };
        static Instruction JRNZn = (_) => { JR(_,  !_.zf); };
        static Instruction JRZn  = (_) => { JR(_,   _.zf); };
        static Instruction JRNCn = (_) => { JR(_, !_.cf); };
        static Instruction JRCn  = (_) => { JR(_,  _.cf); };

        static void JP (Cpu _, bool b) { if (b) { JPnn(_); _.timing = btiming; } else { _.pc += 2; } }

        static void JR (Cpu _, bool b) { if (b) { JRn(_); _.timing = btiming; } else { _.pc++; } }
    }
}
