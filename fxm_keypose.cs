//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;using System.Numerics;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class fxm_2_dx
{
		static void Main()	
		{
				int v1, v2, v3;			//	грани
				float x, y, z;			//	координаты точки
				float w1, w2, w3, w4;		//	веса 
				float c1, c2, c3, c4;		//	индексы/цвет argb/rgba

				int bc1, bc2, bc3, bc4;
				int bc5, bc6, bc7, bc8;
				int bc9, bc10, bc11, bc12;
				int bc13, bc14, bc15, bc16;

				float vn1, vn2, vn3;	//	нормали
				float u, v;		//	развёртка

				int vertex_count;	//	количество вершин
				int faces__count;	//	количество граней

				List<string> face_list = new List<string>();
				List<string> vert_list = new List<string>();
				List<string> norm_list = new List<string>();
				List<string> uvst_list = new List<string>();
				
			//	List<VertexTypePNT> vertex_PNT_List = new List<VertexTypePNT>();
			//	List<VertexTypePWCNT> vertex_PWCNT_List = new List<VertexTypePWCNT>();

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
							//	TODO	//	получается слишком много файлов для саб-мешей
							//	может быть лучше сделать для каждого из них свой каталог и переместить туда нужные текстуры?

						//	открыли *.x на запись 
								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(fxmName) + "/" + Path.GetFileNameWithoutExtension(fxmName) + ".x"))
								{						//	Directory.CreateDirectory(Path.GetDirectoryName(fxmName) + "\\" + Path.GetFileNameWithoutExtension(fxmName));

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(
@"xof 0303txt 0032
");

/////////////////////////////////////////////////////////////////////////////////////////

// Mesh mesh = new Mesh();

/////////////////////////////////////////////////////////////////////////////////////////

								//	версия модели 
										br.ReadInt32();
										br.ReadInt32();

										int frame_count = br.ReadInt32();	//	количество "костей"

										Console.WriteLine("\n" + frame_count + "\n");

										Node[] Frames = new Node[frame_count];	//	лист фреймов

										Matrix4x4 matrix1111 = new Matrix4x4
										(
												1.0f, 0.0f, 0.0f, 0.0f, 
												0.0f, 1.0f, 0.0f, 0.0f, 
												0.0f, 0.0f, 1.0f, 0.0f, 
												0.0f, 0.0f, 0.0f, 1.0f 
										);

								//	инициализируем их поля null-значениями 

										for (int subm = 0; subm < frame_count ; subm++)
										{
												Frames[subm] = new Node(null, matrix1111, null);
										}

/////////////////////////////////////////////////////////////////////////////////////////

										for (int subm = 0; subm < frame_count ; subm++)
										{

										//	разделитель между всеми костями
										//	00 00 00 00 			00 00 00 00		
												br.ReadInt32();		br.ReadInt32();		

										//	читаем имя фрейма 

												int frame_name_length = br.ReadInt32();		//	читаем число символов в строке имени jointa
												byte[] frameNameHex = new byte[frame_name_length];	//	создаём массив байт под них
												br.Read(frameNameHex, 0, frame_name_length);	//	читаем байты в массив
												string frame_name = Encoding.Default.GetString(frameNameHex);

										// читаем номер родительской кости 

												int parent_joint_number = br.ReadInt32();

												if (parent_joint_number == -1) 
														parent_joint_number = 0; // Bip01 "ссылается" сам на себя

										//	читаем матрицу трансформации

												float[] a = new float[16];  for (int i = 0; i < 16 ; i++)  a[i] = br.ReadSingle();
												Matrix4x4 matrix = new Matrix4x4(a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], 
												a[8], a[9], a[10], a[11], a[12], a[13], a[14], a[15] );

										//	создаём Фрейм на основе считанных данных 

												Frames[subm] = new Node(frame_name, matrix, Frames[parent_joint_number]);

										//	запослняем список дочерних костей

												for (int i = 0; i < frame_count; i++) // сравниваем каждый фрейм с каждым
													for (int h = 0; h < frame_count; h++) // для проверки наличия родителя
														if (Frames[i].Parent == Frames[h] && (!Frames[h].Child.Contains(Frames[i]))) 
														//	если поле.Parent.ФреймаX ссылается на ссылку_хранящуюся_в_массиве
														//	т.е. если поле имеет такого родителя и такого дитя ещё не содержится в списке дитей
																Frames[h].Child.Add(Frames[i]); // то добавляем [его] в список детей его родителя
										}

// а как их вывести в х-файл?... сложно

/////////////////////////////////////////////////////////////////////////////////////////

										for (int sub = 0; sub < 11 ; sub++)		//	пропускаем 44 нулевых байт
										{
												br.ReadInt32(); // 00 00 00 00
										}

/////////////////////////////////////////////////////////////////////////////////////////

										int submesh__count = br.ReadInt32();	//	количество саб-мешей	//	одна

/////////////////////////////////////////////////////////////////////////////////////////

										int mesh_name_length = br.ReadInt32();		//	читаем число символов в строке имени jointa
										byte[] mesh_Name_Hex = new byte[mesh_name_length];	//	создаём массив байт под них
										br.Read(mesh_Name_Hex, 0, mesh_name_length);	//	читаем байты в массив
										string mesh_name = Encoding.Default.GetString(mesh_Name_Hex);

/////////////////////////////////////////////////////////////////////////////////////////

										for (int sub = 0; sub < 6 ; sub++)
										{
												br.ReadInt32(); 
										//	00 00 00 00---00 00 00 00---00 00 00 00 
										//	12 01 00 00---20 00 00 00---01 00 00 00 
										//	D3DFVF_NORMAL		D3DFVF_XYZ		D3DFVF_TEX1
										//	#define D3DFVF_DIFFUSE	0x040
										//	#define D3DFVF_SPECULAR	0x080
										}

/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждой сабмеши 

										for (int subm = 0; subm < 1 ; subm++)
										{
												sw.WriteLine("Mesh Subset_1 {");

	/////////////////////////////////////////////////////////////////////////////////////////

												faces__count = br.ReadInt32();	//	количество граней
												vertex_count = br.ReadInt32();	//	количество вершин

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

										//	считываем информацию по каждой вершине v vn vt

												for ( int i = 0 ; i < vertex_count ; i++ )
												{
														x = br.ReadSingle();
														y = br.ReadSingle();
														z = br.ReadSingle();

														vn1 = br.ReadSingle();
														vn2 = br.ReadSingle();
														vn3 = br.ReadSingle();

														u = br.ReadSingle();
														v = br.ReadSingle();

													//	vertex_PNT_List.Add(new VertexTypePNT(new Vector3 (x, y, z), new Vector3 (vn1, vn2, vn3), new Vector2 (u, v)));
														vert_list.Add(x + ";" + y + ";" + z + ";");
														uvst_list.Add(u + ";" + v + ";");
														norm_list.Add(vn1 + ";" + vn2 + ";" + vn3 + ";");
												}

/////////////////////////////////////////////////////////////////////////////////////////

//	записываем всю информацию в *.x файл

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

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
		""" + "P01_FamilyDog.jpg" + @"""" + @";	//	текстуру для этих моделей ищи в FXLibrary.fxf
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
/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine("MeshTextureCoords {");

sw.WriteLine(uvst_list.Count + ";");

for (int q = 0; q < uvst_list.Count; q++) 
{
		sw.Write(uvst_list[q]); 
		if (q == uvst_list.Count - 1 ) { sw.Write(";"); break;}
		else sw.Write(",\n");
}		sw.WriteLine(); 

sw.WriteLine("} // закрыли MeshTextureCoords");

//???????????????????????????????????????????????????????????????????????????????????????
//???????????????????????????????????????????????????????????????????????????????????????
//???????????????????????????????????????????????????????????????????????????????????????

/*
sw.WriteLine();
sw.WriteLine("MeshVertexColors {");
sw.WriteLine();
sw.WriteLine(vertex_count + ";");
sw.WriteLine();
*/

sw.WriteLine();

//	считываем информацию по каждой вершине v w c vn vt

w1 = w2 = w3 = w4 = 0.0f; // присвоим нули
c1 = c2 = c3 = c4 = 0.0f; // а то ругается :)

for ( int i = 0 ; i < vertex_count ; i++ )
{
		x = br.ReadSingle();
		y = br.ReadSingle();
		z = br.ReadSingle();

		byte b1 = br.ReadByte();		
		byte b2 = br.ReadByte();		
		byte b3 = br.ReadByte();		
		byte b4 = br.ReadByte();		

		if ( b1 == 0 )
		{
				w1 = br.ReadSingle();		

				bc1	= br.ReadByte();		
				bc2	= br.ReadByte();		
				bc3	= br.ReadByte();		
				bc4	= br.ReadByte();		
					
				bc5	= br.ReadByte();		
				bc6	= br.ReadByte();		
				bc7	= br.ReadByte();		
				bc8	= br.ReadByte();		
					
				bc9	= br.ReadByte();	
				bc10	= br.ReadByte();	
				bc11	= br.ReadByte();	
				bc12	= br.ReadByte();	
					
				bc13	= br.ReadByte();	
				bc14	= br.ReadByte();	
				bc15	= br.ReadByte();	
				bc16	= br.ReadByte();	
				
				c1 = bc1;
				c2 = bc5;
				c3 = bc9;
				c4 = bc13;
		}

		if ( b1 == 2 )
		{
				w1 = br.ReadSingle();		
				w2 = br.ReadSingle();		
				
				bc1	= br.ReadByte();		
				bc2	= br.ReadByte();		
				bc3	= br.ReadByte();		
				bc4	= br.ReadByte();		
					
				bc5	= br.ReadByte();		
				bc6	= br.ReadByte();		
				bc7	= br.ReadByte();		
				bc8	= br.ReadByte();		
					
				bc9	= br.ReadByte();	
				bc10	= br.ReadByte();	
				bc11	= br.ReadByte();	
				bc12	= br.ReadByte();	
				
				c1 = bc1;
				c2 = bc5;
				c3 = bc9;
				c4 = bc10;
		}

		if ( b1 == 3 )
		{
				w1 = br.ReadSingle();		
				w2 = br.ReadSingle();		
				w3 = br.ReadSingle();		

				bc1 = br.ReadByte();		
				bc2 = br.ReadByte();		
				bc3 = br.ReadByte();		
				bc4 = br.ReadByte();		
					
				bc5 = br.ReadByte();		
				bc6 = br.ReadByte();		
				bc7 = br.ReadByte();		
				bc8 = br.ReadByte();		
				
				c1 = bc1;
				c2 = bc5;
				c3 = bc6;
				c4 = bc7;
		}

		if ( b1 == 4 )
		{
				w1 = br.ReadSingle();		
				w2 = br.ReadSingle();		
				w3 = br.ReadSingle();		
				w4 = br.ReadSingle();		

				bc1 = br.ReadByte();		
				bc2 = br.ReadByte();		
				bc3 = br.ReadByte();		
				bc4 = br.ReadByte();		
				
				c1 = bc1;
				c2 = bc2;
				c3 = bc3;
				c4 = bc4;
		}

		vn1 = br.ReadSingle();
		vn2 = br.ReadSingle();
		vn3 = br.ReadSingle();

		u = br.ReadSingle();
		v = br.ReadSingle();

/*
		vertex_PWCNT_List.Add
		(
			new VertexTypePWCNT
			(
				new Vector3 (x, y, z), 
				new Vector4 (w1, w2, w3, w4), 
				new Vector4 (c1, c2, c3, c4), 
				new Vector3 (vn1, vn2, vn3), 
				new Vector2 (u, v)
			)
		);

if (i < vertex_count-1)
sw.WriteLine( i + ";" + c1 + ";" + c2 + ";" + c3 + ";" + c4 + ";;," );
*/

}

/*
sw.WriteLine( (vertex_count-1) + ";" + c1 + ";" + c2 + ";" + c3 + ";" + c4 + ";;;" );
sw.WriteLine("} // закрыли MeshVertexColors");
*/

//???????????????????????????????????????????????????????????????????????????????????????
//???????????????????????????????????????????????????????????????????????????????????????
//???????????????????????????????????????????????????????????????????????????????????????


sw.WriteLine(@"
} // закрыли Mesh
");

/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(@"
} // закрыли Frame Body
");
/////////////////////////////////////////////////////////////////////////////////////////

sw.WriteLine(@"
} // закрыли Frame SceneRoot 
");

/////////////////////////////////////////////////////////////////////////////////////////

vert_list.Clear();
uvst_list.Clear();
norm_list.Clear();
face_list.Clear();

//vertex_PNT_List.Clear();
//vertex_PWCNT_List.Clear();

/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам

								}		//	using StreamWriter	//	закрываем *.x файл на запись  //	sw.Close();  //		sw.Dispose();

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var fxmName in allFilesName)

		}		//	static void Main()

}		//	class Program

//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO


public class Node // XFile frame
{
    public string Name;
    public Matrix4x4 TrafoMatrix;
    public Node Parent;
    public List<Node> Child = new List<Node>();

//	public List<Mesh> Meshes = new List<Mesh>();

    public Node(string name, Matrix4x4 mtr, Node parent = null)
    {
        this.Name = name;
        this.TrafoMatrix = mtr;
        this.Parent = parent;
    }
}

/*

//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO

public class Mesh 
{
		public string fileTexture; 
		public Vertex[] MeshVertex; 
		public Vertex[] MeshNormals; 
		public int[] MeshFace; 
		public int[] MeshFaceNormals; 
		public float[,] TextCoords; 
}

//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO

	class VertexTypePNT
	{
		public Vector3 Position;	//	x y z
		public Vector3 Normal;		//	n1 n2 n3
		public Vector2 ST;			//	u v

		public VertexTypePNT(Vector3 position, Vector3 normal, Vector2 uv)
		{
			Position = position;
			Normal = normal;
			ST = uv;
		}		
	}

//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO

	class VertexTypePWCNT
	{
		public Vector3 position;		//	x y z
		public Vector4 weight;
		public Vector4 color;			//	RGBA
		public Vector3 normal;			//	n1 n2 n3
		public Vector2 textCoord;	//	u v

		public VertexTypePWCNT(Vector3 position, Vector4 weight, Vector4 color, Vector3 normal, Vector2 uv )
		{
			this.position = position;
			this.weight = weight;
			this.color = color;
			this.normal = normal;
			this.textCoord = uv;
		}		
	}

//OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
*/
