
namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x00()
        {
            var file_name = READ_STRING();
            var path_name = READ_STRING();

            int val1 = fxaReader.ReadInt32();
            int val2 = fxaReader.ReadInt32();
            int val3 = fxaReader.ReadInt32();
            int val4 = fxaReader.ReadInt32();

            int val5 = fxaReader.ReadInt32();  //  /0/1/2/3/4/
            int val6 = fxaReader.ReadInt32();  //  /0/1/2/3/4/
            int val7 = fxaReader.ReadInt32();  //  ii_ii_ii_ii
            int val8 = fxaReader.ReadInt32();  //  ii_ii_ii_ii
        }
    }
}
