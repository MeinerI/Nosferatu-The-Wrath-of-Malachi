using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assimp;

static class FXM__2__ASSIMP
{
    static string extTextureFileName; // расширение текстуры
    static string materialName;

    public static void WORK()
    {
        List<short> face_list = new();
        List<float> vert_list = new();
        List<float> norm_list = new();
        List<float> uvst_list = new();

        string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.fxm", SearchOption.AllDirectories); // ищем fxm

        foreach (var fxmName in allFilesName) // для каждого файла 
        {
            using BinaryReader fxm = new(File.Open(fxmName, FileMode.Open)); // открыли на чтение

            for (int temp_s = 0; temp_s < 11; temp_s++) 
                fxm.ReadSingle(); // skip 11 floats

            int submesh__count = fxm.ReadInt32();

            for (int subm = 0; subm < submesh__count; subm++) // для каждой сабмеши 
            {
                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                int name__length = fxm.ReadInt32(); // читаем количество букв в имени файла текстуры 

                byte[] textureFileNameHex = new byte[name__length];
                fxm.Read(textureFileNameHex, 0, name__length);
                materialName = System.Text.Encoding.Default.GetString(textureFileNameHex);

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                //	получаем имена всех текстур файлов и расширение нужного

                List<string> texture_files = Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.jpg", SearchOption.AllDirectories).ToList();
                texture_files.AddRange(Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.tga", SearchOption.AllDirectories).ToList());
                texture_files.AddRange(Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.JPG", SearchOption.AllDirectories).ToList());
                texture_files.AddRange(Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.TGA", SearchOption.AllDirectories).ToList());

                for (int i = 0; i < texture_files.Count; i++)
                {
                    if (materialName == Path.GetFileNameWithoutExtension(texture_files[i]))
                    {
                        extTextureFileName = Path.GetExtension(texture_files[i]);
                        break;
                    }
                }

                texture_files.Clear();

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                for (int temp_s = 0; temp_s < 6; temp_s++) 
                    fxm.ReadSingle();  //  skip 6 floats

                int faces__count = fxm.ReadInt32();  //	количество граней

                int vertex_count = fxm.ReadInt32();  //	количество вершин

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                for (int i = 0; i < faces__count; i++) // читаем грани f v1 v2 v3
                {
                    face_list.Add(fxm.ReadInt16());
                    face_list.Add(fxm.ReadInt16());
                    face_list.Add(fxm.ReadInt16());
                }

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                for (int i = 0; i < vertex_count; i++) // считываем информацию по каждой вершине v vn vt
                {
                    vert_list.Add(fxm.ReadSingle());
                    vert_list.Add(fxm.ReadSingle());
                    vert_list.Add(fxm.ReadSingle());
                    
                    norm_list.Add(fxm.ReadSingle());
                    norm_list.Add(fxm.ReadSingle());
                    norm_list.Add(fxm.ReadSingle());

                    uvst_list.Add(fxm.ReadSingle());
                    uvst_list.Add(fxm.ReadSingle());
                }

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ
                
                // записываем всю информацию в *.x файл

                //...
                
                vert_list.Clear();
                uvst_list.Clear();
                norm_list.Clear();
                face_list.Clear();

            } 

        }       //	foreach (var fxmName in allFilesName)

    }		//	static void Main()

}		//	class Program
