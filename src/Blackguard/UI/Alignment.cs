using System;

namespace Blackguard.UI;

[Flags]
public enum Alignment {
    Left   = 0b_0000_0001, // 1
    Right  = 0b_0000_0010, // 2
    Center = 0b_0000_0100, // 4
    Fill   = 0b_0000_1000, // 8
    Top    = 0b_0001_0000, // 32
    Bottom = 0b_0010_0000  // 64
}

public static class AlignmentMethods {
    private const byte HAlignmentsMask = (byte)(Alignment.Left | Alignment.Right | Alignment.Center);
    private const byte VAlignmentsMask = (byte)(Alignment.Top | Alignment.Bottom);

    // Function adds values to an alignment, forcing only one of HAlignments and one of VAlignments to be selected
    public static void UpdateAlignment(this ref Alignment alignment, Alignment changed) {
        byte byteChanged = (byte)changed;

        // If this is nonzero, it means one of the alignment fields is being changed
        if ((byteChanged & HAlignmentsMask) != 0) {
            alignment &= ~(Alignment)HAlignmentsMask;
        }

        if ((byteChanged & VAlignmentsMask) != 0) {
            alignment &= ~(Alignment)VAlignmentsMask;
        }

        alignment |= (Alignment)byteChanged;
    }
}
