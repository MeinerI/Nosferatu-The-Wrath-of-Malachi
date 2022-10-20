namespace FXA
{
    public static partial class Program
    {
        static void READ_BLOCK_0x01()
        {
            var file_name = READ_STRING();
            var path_name = READ_STRING();

            var val01 = fxaReader.ReadInt32();
            var val02 = fxaReader.ReadInt32(); 

            int count = fxaReader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var val03 = fxaReader.ReadInt32();  //  0/1/2/3/4/
                var val04 = fxaReader.ReadInt32();

                var val05 = fxaReader.ReadSingle();
                var val06 = fxaReader.ReadSingle();
                var val07 = fxaReader.ReadSingle();

                var val08 = fxaReader.ReadSingle();
                var val09 = fxaReader.ReadSingle();
                var val10 = fxaReader.ReadSingle();

                var val11 = fxaReader.ReadSingle();
                var val12 = fxaReader.ReadSingle();
                var val13 = fxaReader.ReadSingle();

                var val14 = fxaReader.ReadSingle();
                var val15 = fxaReader.ReadSingle();

                var val16 = fxaReader.ReadInt32();
                var val17 = fxaReader.ReadInt32();
                var val18 = fxaReader.ReadInt32();
            }

            var val19 = fxaReader.ReadInt32();  //  0/1
            var val20 = fxaReader.ReadInt32();  //  int 
        }
    }
}
