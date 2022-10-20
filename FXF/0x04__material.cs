
namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x04()
        {
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz

            var val01 = fxaReader.ReadSingle();
            var val02 = fxaReader.ReadSingle();
            var val03 = fxaReader.ReadSingle();
            var val04 = fxaReader.ReadSingle();

            var val05 = fxaReader.ReadSingle();
            var val06 = fxaReader.ReadSingle();
            var val07 = fxaReader.ReadSingle();

            fxaReader.ReadInt32();  //  hz

            var val08 = fxaReader.ReadSingle();
            var val09 = fxaReader.ReadSingle();
            var val010 = fxaReader.ReadSingle();

            fxaReader.ReadInt32();  //  hz

            var val11 = fxaReader.ReadSingle();
            var val12 = fxaReader.ReadSingle();
            var val13 = fxaReader.ReadSingle();

            fxaReader.ReadInt32();  //  hz

            var val14 = fxaReader.ReadSingle();

            int val15 = fxaReader.ReadInt32();  //  0/1
            int val16 = fxaReader.ReadInt32();  //  0/1

            fxaReader.ReadInt32();  //  hz

            int count = fxaReader.ReadInt32();
            int val17;
            for (int i = 0; i < count; i++)
                val17 = fxaReader.ReadInt32();

            int val18 = fxaReader.ReadInt32();    //  0/1/2/3/4/5/6 
            int val19 = fxaReader.ReadInt32();    //  0/7 
            int val20 = fxaReader.ReadInt32();    //  0/1
            int val21 = fxaReader.ReadInt32();    //  0/1 
            int val22 = fxaReader.ReadInt32();    //  0/1 
            int val23 = fxaReader.ReadInt32();    //  0/1 
            int val24 = fxaReader.ReadInt32();    //  0/2/4 

            fxaReader.ReadInt32();  //  ii_ii_ii_ii 

            fxaReader.ReadInt32();  //  hz
            fxaReader.ReadInt32();  //  hz
        }
    }
}
