//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class anb_2_x
{
		static int vertex_count;			//	количество вершин
		static int faces__count;			//	количество граней

		static List<string> face_list   = new List<string>();
		static List<string> face_list2  = new List<string>();
		
		static int submesh__count;
		static int first_line_in_file;

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

		static void Main()	
		{
				int uv_count;

				int v1, v2, v3;				//	грани
				float x, y, z;				//	координаты точки
				float vn1, vn2, vn3;	//	нормали
				float vt1, vt2, vt3;	//	UVs Indices
				float u, v;						//	UVs развёртка

				List<string> vert_list   = new List<string>();
				List<string> norm_list   = new List<string>();

				List<string> vert_list2   = new List<string>();
				List<string> norm_list2   = new List<string>();

				List<string> uvst_list   = new List<string>();
				List<string> face_vt_list = new List<string>();

		//	точки вместо запятых	// хотя для obj это не важно вроде бы	// как и табы вместо пробелов
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

		//	ищем все fmx файлы в папках и подпапках

				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.anb",  SearchOption.AllDirectories) ; 

		//	для каждого fmx файла 

				foreach (var anbName in allFilesName)
				{
						//	открыли *.fxm файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(anbName, FileMode.Open)))
						{

								//	открыли *.x на запись 

								//	TODO	//	получается слишком много файлов для саб-мешей
								//	может быть лучше сделать для каждого из них свой каталог и переместить туда нужные текстуры?

								//	Directory.CreateDirectory(Path.GetDirectoryName(anbName) + "\\" + Path.GetFileNameWithoutExtension(anbName));

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(anbName) + "/" + Path.GetFileNameWithoutExtension(anbName) + ".x"))
								{
										first_line_in_file = br.ReadInt32();	// количество фреймов-моделей

										submesh__count = br.ReadInt32();	//	количество саб-мешей

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(
@"xof 0303txt 0032

template MorphAnimationKey
{
  <2746B58A-B375-4cc3-8D23-7D094D3C7C67>
  DWORD  Time;        // Key's time
  STRING MeshName;    // Mesh to use (name reference)
}

template MorphAnimationSet
{
  <0892DE81-915A-4f34-B503-F7C397CB9E06>
  DWORD NumKeys;  // # keys in animation
  array MorphAnimationKey Keys[NumKeys];
}

Material HZ_Material {
 1.000000;1.000000;1.000000;1.000000;;
 3.200000;
 0.000000;0.000000;0.000000;;
 0.000000;0.000000;0.000000;;
 TextureFilename  {
  ""hz.jpg"";
 }
}

");

/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждой сабмеши 

										sw.WriteLine("// " + first_line_in_file + "\n");

										for (int subm = 0; subm < submesh__count ; subm++)
										{
												sw.WriteLine("Mesh mShape_" + subm + " {");
										//	sw.WriteLine("Mesh mShape {");

	/////////////////////////////////////////////////////////////////////////////////////////

												vertex_count = br.ReadInt32();	//	количество вершин
												uv_count = br.ReadInt32();			//	количество УВ?
												faces__count = br.ReadInt32();	//	количество граней

	/////////////////////////////////////////////////////////////////////////////////////////

										//	считываем информацию по каждой вершине v vn

												for ( int i = 0 ; i < vertex_count ; i++ )
												{
														x = br.ReadSingle();
														y = br.ReadSingle();
														z = br.ReadSingle();

														vert_list.Add(x + ";" + y + ";" + z + ";");

														vn1 = br.ReadSingle();
														vn2 = br.ReadSingle();
														vn3 = br.ReadSingle();
														norm_list.Add(vn1 + ";" + vn2 + ";" + vn3 + ";");
												}

	/////////////////////////////////////////////////////////////////////////////////////////

										//	читаем грани в виде строк f v1 v2 v3

												for ( int i = 0 ; i < faces__count ; i++ )
												{
														v1 = br.ReadInt16();
														v2 = br.ReadInt16();
														v3 = br.ReadInt16();

														face_list.Add("3;" + v1 + "," + v2 + "," + v3 + ";" );
												}

/////////////////////////////////////////////////////////////////////////////////////////

										//	считываем информацию по каждой вершине vt

												for ( int i = 0 ; i < uv_count ; i++ )
												{
														u = br.ReadSingle();
														v = br.ReadSingle();

														uvst_list.Add(u + ";" + v + ";");
												}

/////////////////////////////////////////////////////////////////////////////////////////

										//	читаем грани в виде строк f vt1 vt2 vt3

												for ( int i = 0 ; i < faces__count ; i++ )
												{
														vt1 = br.ReadInt16();
														vt2 = br.ReadInt16();
														vt3 = br.ReadInt16();

														face_vt_list.Add("3;" + vt1 + "," + vt2 + "," + vt3 + ";" );
												}

//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

//	записываем всю информацию в *.x файл

sw.WriteLine(vert_list.Count + ";");

for (int q = 0; q < vert_list.Count; q++) 
{
		sw.Write(vert_list[q]); 
		if (q == vert_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(face_list.Count + ";"); 

for (int q = 0; q < face_list.Count; q++) 
{
		sw.Write(face_list[q]); 
		if (q == face_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("MeshNormals {");

sw.WriteLine(norm_list.Count + ";");

for (int q = 0; q < norm_list.Count; q++) 
{
		sw.Write(norm_list[q]); 
		if (q == norm_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(face_list.Count + ";"); 

for (int q = 0; q < face_list.Count; q++) 
{
		sw.Write(face_list[q]); 
		if (q == face_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

sw.WriteLine("}");			//	закрыли MeshNormals

/////////////////////////////////////////////////////////////////////////////////////////

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

sw.WriteLine("{ HZ_Material }");

sw.WriteLine("\n}");	//	закрыли MeshMaterialList

/////////////////////////////////////////////////////////////////////////////////////////

sw.Write("\n");

sw.WriteLine("// количество UV в некоторых файлах не совпадает с Вершинами");

sw.WriteLine("// " + "MeshTextureCoords {");

sw.WriteLine("// " + uvst_list.Count + ";");

for (int q = 0; q < uvst_list.Count; q++) 
{
		sw.Write("// " + uvst_list[q]); 
		if (q == uvst_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

/////////////////////////////////////////////////////////////////////////////////////////

sw.Write("\n");

sw.WriteLine("// " + "индексы текстурных координат ??? такого шаблона вроде нет \n"); 

sw.WriteLine("// " + face_vt_list.Count + ";"); 

for (int q = 0; q < face_vt_list.Count; q++) 
{
		sw.Write("// " + face_vt_list[q]); 
		if (q == face_vt_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

sw.WriteLine("// " + "}");			//	закрыли MeshTextureCoords

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("}\n");			//	закрыли Mesh

//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

vert_list.Clear();
uvst_list.Clear();
norm_list.Clear();

face_list2 = face_list.GetRange(0, face_list.Count);

face_list.Clear();

face_vt_list.Clear();

/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам ******************************************

//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

								//	на одну меньше 

										for (int subm = (first_line_in_file - (first_line_in_file - submesh__count)); subm < (submesh__count * first_line_in_file) ; subm++)
										{
												sw.WriteLine("Mesh mShape_" + subm + " {");

	/////////////////////////////////////////////////////////////////////////////////////////

										//	считываем информацию по каждой вершине v vn

												for ( int i = 0 ; i < vertex_count ; i++ )
												{
														x = br.ReadSingle();
														y = br.ReadSingle();
														z = br.ReadSingle();

														vert_list2.Add(x + ";" + y + ";" + z + ";");

														vn1 = br.ReadSingle();
														vn2 = br.ReadSingle();
														vn3 = br.ReadSingle();

														norm_list2.Add(vn1 + ";" + vn2 + ";" + vn3 + ";");
												}

	/////////////////////////////////////////////////////////////////////////////////////////

												//	записываем всю информацию в *.x файл

												sw.WriteLine(vert_list2.Count + ";");

												for (int q = 0; q < vert_list2.Count; q++) 
												{
														sw.Write(vert_list2[q]); 
														if (q == vert_list2.Count - 1 ) { sw.Write(";"); break;}
														else sw.Write(",\n");
												}		sw.WriteLine(); 

												/////////////////////////////////////////////////////////////////////////////////////////

												sw.WriteLine(face_list2.Count + ";"); 

												for (int q = 0; q < face_list2.Count; q++) 
												{
														sw.Write(face_list2[q]); 
														if (q == face_list2.Count - 1 ) { sw.Write(";"); break;}
														else sw.Write(",\n");
												}		sw.WriteLine(); 

												/////////////////////////////////////////////////////////////////////////////////////////

												sw.WriteLine("MeshNormals {");

												sw.WriteLine(norm_list2.Count + ";");

												for (int q = 0; q < norm_list2.Count; q++) 
												{
														sw.Write(norm_list2[q]); 
														if (q == norm_list2.Count - 1 ) { sw.Write(";"); break;}
														else sw.Write(",\n");
												}		sw.WriteLine(); 

												/////////////////////////////////////////////////////////////////////////////////////////

												sw.WriteLine(face_list2.Count + ";"); 

												for (int q = 0; q < face_list2.Count; q++) 
												{
														sw.Write(face_list2[q]); 
														if (q == face_list2.Count - 1 ) { sw.Write(";"); break;}
														else sw.Write(",\n");
												}		sw.WriteLine(); 

												sw.WriteLine("}");			//	закрыли MeshNormals

												sw.WriteLine("}\n");		//	закрыли Mesh

												vert_list2.Clear();
												norm_list2.Clear();
										}

												face_list2.Clear();

//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

sw.WriteLine("MorphAnimationSet Set {");

sw.WriteLine(first_line_in_file * submesh__count + ";");

int ioo = first_line_in_file * submesh__count;

for (int io = 0, step = 0; io < ioo; io++, step+=500)
{
		sw.Write(step + "; " + "\"mShape_" + (io) + "\";");
		if (io == (ioo - 1) ) { sw.Write(";"); break;}
		else sw.WriteLine(",");
}

sw.WriteLine("\n}");			//	закрыли MorphAnimationSet

/////////////////////////////////////////////////////////////////////////////////////////


								}		//	using StreamWriter	//	закрываем *.x файл на запись  //	sw.Close();  //		sw.Dispose();

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var anbName in allFilesName)

		}		//	static void Main()

}		//	class Program
