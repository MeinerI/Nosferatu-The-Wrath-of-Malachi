namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x07()
        {
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz

            string font_name = READ_STRING();

            int count = fxaReader.ReadInt32();

            for (int i = 0; i < count; i++) fxaReader.ReadByte();

            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz

            count = fxaReader.ReadInt32();
            var val00 = fxaReader.ReadSingle();
            for (int i = 0; i < count; i++) fxaReader.ReadByte();

            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz

            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz

            for (int i = 0; i < count; i++)
            {
                fxaReader.ReadInt32();  //  hz
                fxaReader.ReadInt32();  //  hz
                fxaReader.ReadInt32();  //  hz
                fxaReader.ReadInt32();  //  hz
            }
        }
    }
}
