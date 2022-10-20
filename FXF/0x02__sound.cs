namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x02()
        {
            var file_name  = READ_STRING();
            var path_name  = READ_STRING();

            int val1 = fxaReader.ReadInt32();
            int val2 = fxaReader.ReadInt32();
            int val3 = fxaReader.ReadInt32();  //  000/111/222
            int val4 = fxaReader.ReadInt32();
            int val5 = fxaReader.ReadInt32();
        }
    }
}
