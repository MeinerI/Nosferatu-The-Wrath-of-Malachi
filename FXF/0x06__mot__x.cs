namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x06()
        {
            string file_name = READ_STRING();
            string path_name = READ_STRING();

            int val1 = fxaReader.ReadInt32();
            int val2 = fxaReader.ReadInt32();
            int val3 = fxaReader.ReadInt32();
        }
    }
}
