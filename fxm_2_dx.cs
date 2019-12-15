//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class fxm_2_dx
{
		static string extTextureFileName;		//	расширение текстуры

		static void Main()	
		{
				int v1, v2, v3;				//	грани
				float x, y, z;				//	координаты точки
				float vn1, vn2, vn3;	//	нормали
				float u, v;						//	развёртка

				int vertex_count;			//	количество вершин
				int faces__count;			//	количество граней

				string materialName;	//	имя материала

				List<string> face_list = new List<string>();
				List<string> vert_list = new List<string>();
				List<string> norm_list = new List<string>();
				List<string> uvst_list = new List<string>();

		//	точки вместо запятых	// хотя для obj это не важно вроде бы	// как и табы вместо пробелов
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

		//	ищем все fmx файлы в папках и подпапках

				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.fxm",  SearchOption.AllDirectories) ; 

		//	для каждого fmx файла 

				foreach (var fxmName in allFilesName)
				{
						//	открыли *.fxm файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(fxmName, FileMode.Open)))
						{

								//	открыли *.x на запись 

								//	TODO	//	получается слишком много файлов для саб-мешей
								//	может быть лучше сделать для каждого из них свой каталог и переместить туда нужные текстуры?

								//	Directory.CreateDirectory(Path.GetDirectoryName(fxmName) + "\\" + Path.GetFileNameWithoutExtension(fxmName));

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(fxmName) + "/" + Path.GetFileNameWithoutExtension(fxmName) + ".x"))
								{

								//	пропускаем первые непонятные 44 байт с начала файла

										for (int temp_s = 0; temp_s < 11; temp_s++ ) br.ReadSingle();		//	11 по 4

										int submesh__count = br.ReadInt32();	//	количество саб-мешей

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(
@"xof 0303txt 0032
");

/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждой сабмеши 

										for (int subm = 0; subm < submesh__count ; subm++)
										{

										//	sw.WriteLine("Mesh mShape_" + subm + " {");
												sw.WriteLine("Mesh mShape {");

/////////////////////////////////////////////////////////////////////////////////////////

{										//	читаем количество букв в имени файла текстуры 

												int name__length = br.ReadInt32();

										//	читаем имя файла текстуры 
										//	на самом деле это имя метериала, который хранится в файле FXLibrary.fxf

												byte[] textureFileNameHex = new byte[name__length];
												br.Read(textureFileNameHex, 0, name__length);
												materialName = System.Text.Encoding.Default.GetString(textureFileNameHex);
}

	/////////////////////////////////////////////////////////////////////////////////////////

{										//	получаем имена всех текстур файлов и расширение нужного

												List<string>	texture_files = Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.jpg", SearchOption.AllDirectories).ToList();
												texture_files.AddRange(Directory.GetFiles(Path.GetDirectoryName(fxmName), "*.tga", SearchOption.AllDirectories).ToList());

												for (int i = 0; i < texture_files.Count; i++)
												{
														if (materialName == Path.GetFileNameWithoutExtension(texture_files[i])) 
														{
																extTextureFileName = Path.GetExtension(texture_files[i]);
																break;
														}
												}
													texture_files.Clear();
}

	/////////////////////////////////////////////////////////////////////////////////////////

{										//	после имени пропускаем 24 байт "пустой" информации
			
												for (int temp_s = 0; temp_s < 6; temp_s++ ) br.ReadSingle();	//	6 по 4

												faces__count = br.ReadInt32();	//	количество граней

												vertex_count = br.ReadInt32();	//	количество вершин
}

	/////////////////////////////////////////////////////////////////////////////////////////

{										//	читаем грани в виде строк f v1 v2 v3

												for ( int i = 0 ; i < faces__count ; i++ )
												{
														v1 = br.ReadInt16();
														v2 = br.ReadInt16();
														v3 = br.ReadInt16();

														face_list.Add("3;" + v1 + "," + v2 + "," + v3 + ";" );
												}
}

	/////////////////////////////////////////////////////////////////////////////////////////

{										//	считываем информацию по каждой вершине v vn vt

												for ( int i = 0 ; i < vertex_count ; i++ )
												{
														x = br.ReadSingle();
														y = br.ReadSingle();
														z = br.ReadSingle();

														vert_list.Add(x + ";" + y + ";" + z + ";");

														vn1 = br.ReadSingle();
														vn2 = br.ReadSingle();
														vn3 = br.ReadSingle();

														u = br.ReadSingle();
														v = br.ReadSingle();

														uvst_list.Add(u + ";" + v + ";");

														norm_list.Add(vn1 + ";" + vn2 + ";" + vn3 + ";");
												}
}

/////////////////////////////////////////////////////////////////////////////////////////

{

//	записываем всю информацию в *.x файл

sw.WriteLine(vert_list.Count + ";");

for (int q = 0; q < vert_list.Count; q++) 
{
		sw.Write(vert_list[q]); 
		if (q == vert_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 



sw.WriteLine(face_list.Count + ";"); 

for (int q = 0; q < face_list.Count; q++) 
{
		sw.Write(face_list[q]); 
		if (q == face_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("MeshMaterialList {");
sw.WriteLine("1;");

sw.WriteLine(face_list.Count + ";");

for (int q = 0; q < face_list.Count; q++) 
{
		sw.Write("0"); 
		if (q == face_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",");
}

sw.WriteLine(@"
Material {
		1.0; 1.0; 1.0; 1.000000;;
		1.000000;
		0.000000; 0.000000; 0.000000;;
		0.000000; 0.000000; 0.000000;;
		TextureFilename { 
		""" + materialName + extTextureFileName + @"""" + @";
		}
	}
}
");

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("MeshNormals {");

sw.WriteLine(norm_list.Count + ";");

for (int q = 0; q < norm_list.Count; q++) 
{
		sw.Write(norm_list[q]); 
		if (q == norm_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 


sw.WriteLine(face_list.Count + ";"); 

for (int q = 0; q < face_list.Count; q++) 
{
		sw.Write(face_list[q]); 
		if (q == face_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

sw.WriteLine("}");			//	закрыли MeshNormals

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("MeshTextureCoords {");

sw.WriteLine(uvst_list.Count + ";");

for (int q = 0; q < uvst_list.Count; q++) 
{
		sw.Write(uvst_list[q]); 
		if (q == uvst_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

sw.WriteLine("}");			//	закрыли MeshTextureCoords

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(@"
}
// zxc разделение мешей и отчистка списков
");

}

/////////////////////////////////////////////////////////////////////////////////////////

vert_list.Clear();
uvst_list.Clear();
norm_list.Clear();
face_list.Clear();

/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам

								}		//	using StreamWriter	//	закрываем *.x файл на запись  //	sw.Close();  //		sw.Dispose();

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var fxmName in allFilesName)

		}		//	static void Main()

}		//	class Program
