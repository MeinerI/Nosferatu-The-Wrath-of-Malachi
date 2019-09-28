// читает всё правильно, но я не знаю как создать в одном *.obj файле несколько групп объектов с разными вершинами и гранями
// ещё есть недоработка в создании и указании файла с библиотекой материалов *.mtl

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class fxm2obj
{
		static void Main()	
		{
				int v1, v2, v3;				//	грани
				float x, y, z;				//	координаты точки
				float vn1, vn2, vn3;	//	нормали
				float u, v;						//	развёртка

				int vertex_count;			//	количество вершин
				int faces__count;			//	количество граней

				string textureFileName;

				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.fxm",  SearchOption.AllDirectories) ; 
				
				foreach (var fxmName in allFilesName)
				{
						//	открыли *.fxm файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(fxmName, FileMode.Open)))
						{
								//	открыли *.obj на запись 

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(fxmName) + "/" + Path.GetFileNameWithoutExtension(fxmName) + ".obj"))
								{

								//	пропускаем первые непонятные 44 байт с начала файла

										for (int temp_s = 0; temp_s < 11; temp_s++ ) br.ReadSingle();		//	11 по 4

								//	количество саб-мешей

										float submesh__hex = br.ReadSingle();
										byte[] byteArray = BitConverter.GetBytes(submesh__hex);
										int submesh__count = BitConverter.ToInt32(byteArray, 0);

								//	для каждой сабмеши 

										for (int subm = 0; subm < submesh__count ; subm++)
										{

{										//	читаем количество букв в имени файла текстуры 

												float name__lengtf = br.ReadSingle();
												byteArray = BitConverter.GetBytes(name__lengtf);
												int name__length = BitConverter.ToInt32 (byteArray, 0);

										//	читаем имя файла текстуры 

												byte[] textureFileNameHex = new byte[name__length];
												br.Read(textureFileNameHex, 0, name__length);
												textureFileName = System.Text.Encoding.Default.GetString(textureFileNameHex);
}

{										//	после имени пропускаем 24 байт "пустой" информации

												for (int temp_s = 0; temp_s < 6; temp_s++ ) br.ReadSingle();	//	6 по 4

										//	количество граней

												float faces__hex = br.ReadSingle();
												byteArray = BitConverter.GetBytes(faces__hex);
												faces__count = BitConverter.ToInt32(byteArray, 0);

										//	количество вершин

												float vertex_hex = br.ReadSingle();
												byteArray = BitConverter.GetBytes(vertex_hex);
												vertex_count = BitConverter.ToInt32(byteArray, 0);
}

{
										//	берём имя *.fxm файла из полного пути к нему
										//	string fileName = Path.GetFileNameWithoutExtension(fxmName);

										//	записываем в *.obj файл 

												//	добавляем библиотеку материалов

												//	перед этим её надо создать

												/*
														newmtl material_0
														Ka 0.200000 0.200000 0.200000
														Kd 1.000000 1.000000 1.000000
														Ks 1.000000 1.000000 1.000000
														map_Kd red_tex.png
												*/

												sw.WriteLine("mtllib " + Path.GetDirectoryName(fxmName) + "/" + Path.GetFileNameWithoutExtension(fxmName) + ".mtl");
												sw.WriteLine("usemtl material_" + submesh__count);
												sw.WriteLine();
}

{										//	читаем грани в виде строк f v1 v2 v3

												sw.WriteLine("o object_name" + subm);

												for ( int i = 0 ; i < faces__count ; i++ )
												{
														v1 = br.ReadInt16()+1;
														v2 = br.ReadInt16()+1;
														v3 = br.ReadInt16()+1;

												//	sw.WriteLine("f " + v1 + " " + v2 + " " + v3);

														sw.WriteLine("f " + v1 + "/" + v1 + "/" + v1 + " " +
																								v2 + "/" + v2 + "/" + v2 + " " +
																								v3 + "/" + v3 + "/" + v3 );
												}
														sw.WriteLine();
}

{										//	считываем информацию по каждой вершине v vn vt

												for ( int i = 0 ; i < vertex_count ; i++ )
												{
														x = br.ReadSingle();
														y = br.ReadSingle();
														z = br.ReadSingle();

														sw.WriteLine("v " + x + " " + y + " " + z);

														vn1 = br.ReadSingle();
														vn2 = br.ReadSingle();
														vn3 = br.ReadSingle();

														u = br.ReadSingle();
														v = br.ReadSingle();

														sw.WriteLine("vt " + u + " " + v);

														sw.WriteLine("vn " + vn1 + " " + vn2 + " " + vn3 + "\n");
												}
}
										}		//	проход по сабмешам

								}		//	using StreamWriter	//	закрываем файл на запись 

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var fxmName in allFilesName)

		}		//	static void Main()

}		//	class Program
