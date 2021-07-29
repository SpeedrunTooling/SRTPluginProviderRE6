using System.Runtime.InteropServices;

namespace SRTPluginProviderRE6.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]

    public unsafe struct GameDifficultyAdjustment
    {
        [FieldOffset(0x0)] private int leon;
        [FieldOffset(0x4)] private int helena;
        [FieldOffset(0x8)] private int chris;
        [FieldOffset(0xC)] private int piers;
        [FieldOffset(0x10)] private int jake;
        [FieldOffset(0x14)] private int sherry;
        [FieldOffset(0x18)] private int ada;
        [FieldOffset(0x1C)] private int agent;

        public int Leon => leon;
        public int Helena => helena;
        public int Chris => chris;
        public int Piers => piers;
        public int Jake => jake;
        public int Sherry => sherry;
        public int Ada => ada;
        public int Agent => agent;
    }
}