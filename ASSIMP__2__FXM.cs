using System;
using System.Collections.Generic;
using System.IO;
using Assimp;

static class ASSIMP__2__FXM
{
    public static void WORK()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); // точки вместо запятых

        string[] allXFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.x", SearchOption.AllDirectories);

        //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

        foreach (var xFileName in allXFilesName) //	для каждого x файла 
        {
            string oldFXMname = Path.GetFileNameWithoutExtension(xFileName) + ".fxm"; // получаем имя родного ".fxm" файла
            string newFXMname = Path.GetFileNameWithoutExtension(xFileName) + "__assimp" + ".fxm";

            //  TODO 
            //  JeremyAnsel.Media.DirectXFile.XFile d3DXof;
            //  d3DXof = XFile.FromFile(xFileName); // я не знаю как юзать Assimp Material'ы, поэтому юзаю чужой парсер
            //  но он не умееют читать бинарные файлы и ещё много чего
            //  "придётся" сначала конвертить xbinary в xTXT через ASSIMP, а потом читать текстуры через JeremyAnsel.Media

            //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

            AssimpContext assimpContext = new();

            Scene scene = assimpContext.ImportFile(xFileName,
                PostProcessSteps.JoinIdenticalVertices
                //|PostProcessSteps.GlobalScale
                |PostProcessPreset.ConvertToLeftHanded // оставить
                //|PostProcessSteps.TransformUVCoords
                );

            /*
            PostProcessSteps.FindDegenerates |
            PostProcessSteps.FindInstances |
            PostProcessSteps.FindInvalidData |
            PostProcessSteps.FixInFacingNormals |
            PostProcessSteps.GenerateNormals |
            PostProcessSteps.ForceGenerateNormals |
            PostProcessSteps.GenerateSmoothNormals |
            PostProcessSteps.OptimizeGraph |
            PostProcessSteps.OptimizeMeshes |
            PostProcessSteps.PreTransformVertices |
            PostProcessSteps.RemoveComponent |
            PostProcessSteps.RemoveRedundantMaterials |
            PostProcessSteps.TransformUVCoords |
            PostProcessSteps.ValidateDataStructure
            */

            // scene.SceneFlags = SceneFlags.NonVerboseFormat;

            if (scene.Meshes[0].HasBones) break; // TODO // если xFile имеет кости, то ИССЛЕДУЙ как запихать их в FXM лул

            //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

            using BinaryWriter fxm = new(File.Open(newFXMname, FileMode.Create)); //	открыли new.fxm файл на запись
            {
                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                if (File.Exists(oldFXMname)) // считываем что то из начала файла fxm // 11 по 4
                {
                    BinaryReader br = new(File.Open(oldFXMname, FileMode.Open));
                    for (int temp_s = 0; temp_s < 11; temp_s++)
                        fxm.Write(br.ReadSingle()); // возможно это коллайдер
                    br.Close();
                }
                else // для новых моделей, у которых нет аналогов в игре
                {
                    for (int temp_s = 0; temp_s < 11; temp_s++)
                        fxm.Write(0.0f); // начало забиваем мусором
                }

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                //  пишем количество мешей в newFXMname
                //  Assimp всё сливает видимо в одну 
                fxm.Write(scene.Meshes.Count);

                //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                //  для каждой меши пишем 

                int d3DXofMeshIndex = 0;

                foreach (var mesh in scene.Meshes)
                {
                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    int TextureFilenameSize = 3;
                    var TextureFilename = "123"; // d3DXof.Meshes[d3DXofMeshIndex].Materials[0].Filename;

                    // TODO // А ЕСЛИ В ПАПКЕ НЕТ ТАКОГО ФАЙЛА

                    if (String.IsNullOrEmpty(TextureFilename) || !File.Exists(TextureFilename))
                    {
                        TextureFilenameSize = 7; // заглушка
                        TextureFilename = "WebAdd1";
                    }
                    else // для новых Х-файлов с текстурами с расширениями в материалах
                    if (TextureFilename.Contains("jpg") ||
                        TextureFilename.Contains("JPG") ||
                        TextureFilename.Contains("tga") ||
                        TextureFilename.Contains("TGA") ||
                        TextureFilename.Contains("bmp") ||
                        TextureFilename.Contains("BMP"))
                        TextureFilename = TextureFilename[0..^4]; // Substring(0, Length - 4);

                    //	записываем размер +/= имя материала

                    TextureFilenameSize = TextureFilename.Length;
                    fxm.Write(TextureFilenameSize);
                    fxm.Write(System.Text.Encoding.ASCII.GetBytes(TextureFilename));

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    fxm.Write(new byte[] // какая то инфа после имени материала 6 по 4
                    {
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x12, 0x01, 0x00, 0x00, 0x20, 0x00,
                            0x00, 0x00, 0x01, 0x00, 0x00, 0x00
                    });

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    fxm.Write(mesh.FaceCount); // пишем число граней
                    fxm.Write(mesh.Vertices.Count); // пишем число вершин

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    // short[] faceIndices = mesh.GetShortIndices();

                    foreach (var faces in mesh.Faces)
                        foreach (var face in faces.Indices) // пишем все грани
                            fxm.Write(Convert.ToInt16(face));

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    //  КОСТЫЛЬ

                    List<Vector3D>[] uvsChannel = mesh.TextureCoordinateChannels;

                    List<float> uvs = new(); // сюда засыпаем текстурные координаты

                    //  внешний цикл не нужен, канал всё равно один
                    for (int j = 0; j < uvsChannel[0].Count; j++)
                    {
                        uvs.Add(uvsChannel[0][j].X);
                        uvs.Add(uvsChannel[0][j].Y);
                    }

                    Tuple<float, float>[] tuples = new Tuple<float, float>[mesh.Vertices.Count];

                    for (int i = 0; i < mesh.Vertices.Count; i++)
                    {
                        float u = uvs[i * 2]; 
                        float v = uvs[i * 2 + 1]; 
                        tuples[i] = Tuple.Create(uvs[i * 2], v);
                    }

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    for (int i = 0; i < mesh.Vertices.Count; i++)
                    {
                        fxm.Write(mesh.Vertices[i].X * 100); 
                        fxm.Write(mesh.Vertices[i].Y * 100);
                        fxm.Write(mesh.Vertices[i].Z * 100);

                        fxm.Write(mesh.Normals[i].X);
                        fxm.Write(mesh.Normals[i].Y);
                        fxm.Write(mesh.Normals[i].Z);

                        fxm.Write(tuples[i].Item1);
                        fxm.Write(tuples[i].Item2);
                    }

                    //ЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖЖ

                    d3DXofMeshIndex++; // текстура для следущей меши

                }   //  для каждой меши пишем

            }   //  BinaryWriter

        }   //  для каждого файла

    }   //  public static void WORK()

}   //  static class DX__2__FXM__ASSIMP__NEW
