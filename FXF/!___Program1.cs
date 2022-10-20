namespace FXA
{
    public static partial class Program
    {
        static BinaryReader? fxaReader = null!;

        //////////////////////////////////////////////////////////////////////////////////

        static void Main()
        {
            var fileName = "FXLibrary.fxf";

            using (FileStream fs = File.OpenRead(fileName)) // открываем на чтение
            {
                using (fxaReader = new BinaryReader(fs, System.Text.Encoding.Default, false)) // бинарник
                {
                    //////////////////////////////////////////////////////////////////////////////////

                    var hz1 = fxaReader.ReadInt32();     // 04 00 00 00
                    var hz2 = fxaReader.ReadSingle();

                    for (int i = 0; i < 4; i++)
                        fxaReader.ReadInt32();  //  zero

                    ///какой то первый блок состоит 
                    ///из 8 "пронумерованных" блоков 
                    ///по 17,26,12,1,15,1,29,1 "папок"

                    int big_block_count = fxaReader.ReadInt32();

                    for (int big_block = 0; big_block < big_block_count; big_block++)
                    {
                        int sub_block_count = fxaReader.ReadInt32();          //	читаем количество под-блоков 

                        for (int root = 0; root < sub_block_count; root++)    //	читаем "под-блоки"
                        {
                            var block_name = READ_STRING();
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////////////

                    int block_count = fxaReader.ReadInt32();

                    while (fs.Length != fs.Position)
                    {
                        int block_type = fxaReader.ReadInt32();

                        /////////////////////////////////////////

                        int block_number = fxaReader.ReadInt32();
                        int block_folder = fxaReader.ReadInt32();
                        var block_name = READ_STRING();

                        /////////////////////////////////////////

                        if (block_type == 0x00) READ_BLOCK_0x00();
                        if (block_type == 0x01) READ_BLOCK_0x01();
                        if (block_type == 0x02) READ_BLOCK_0x02();
                        if (block_type == 0x04) READ_BLOCK_0x04();
                        if (block_type == 0x06) READ_BLOCK_0x06();
                        if (block_type == 0x07) READ_BLOCK_0x07();
                    }

                    //////////////////////////////////////////////////////////////////////////////////
                }
            }
        }
    }
}
