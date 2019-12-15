//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;
	using System.Collections;using System.Collections.Generic;
	using System.Text;using System.Text.RegularExpressions;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class dx_2_fxm
{

		static string[] readText; // хранит все строки из *.x файла // видно из всего кода
		static int subMeshCounter;
		static int vertex_count;	//	количество вершин
		static int faces__count;	//	количество граней

		static string TextureFilename;	//	почему то оно не видно во всём файле :с
		static string[] xyz;
		static string[] asd;

		static List<string> strListFaces = new List<string>();
		static List<string> strListVerts = new List<string>();
		static List<string> strListNorms = new List<string>();
		static List<string> strListTexts = new List<string>();

//	жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

		static void Main()	
		{

		//	точки вместо запятых	// хотя для obj это не важно вроде бы	// как и табы вместо пробелов
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

		//	ищем все (зачем? нужен только один) *.x файлы в папках и подпапках
				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.x",  SearchOption.AllDirectories); 

				foreach (var xName in allFilesName)	//	для каждого x файла 
				{
//////////
						subMeshCounter = 0;

						readText = File.ReadAllLines(xName, Encoding.UTF8/*ASCII*/);			// читаем все строки из *.x файла
						if (!readText[0].Contains("txt")) break;

				//	теперь нужно удалить все пустые строки (и желательно пробелы?),
				//	чтобы легче было парсить строки из текстового x-файла

						List<string> new_text = new List<string>();

						foreach (var str in readText)
						{
								string x = str.Replace("\t", "");
								//	x = str.Replace(" ", "");
								if (x != "") new_text.Add(x);
						}

//////////

						string fxmName = Path.GetFileNameWithoutExtension(xName) + ".fxm"; // получаем имя родного ".fxm" файла
						string fxmPathName = Path.GetFileNameWithoutExtension(xName) + "__edit" + ".fxm";

						Console.WriteLine();
						Console.WriteLine();
						Console.WriteLine(fxmName);
						Console.WriteLine(fxmPathName);
						Console.WriteLine();


using (BinaryReader br = new BinaryReader(File.Open(fxmName, FileMode.Open)))	//	открыли *.fxm файл на чтение
{
		using (BinaryWriter sw = new BinaryWriter(File.Open(fxmPathName, FileMode.Create)))	//	открыли *.fxm файл на чтение
		{



		//	считываем что то из начала файла fxm
		//	возможно это "указатель" на подпапку из fxf файла 
				for (int temp_s = 0; temp_s < 11; temp_s++ ) 
				sw.Write(br.ReadSingle());		//	11 по 4


		//	узнаём количество сабмешей

				for (int i = 0; i < new_text.Count ; i++) // для каждой строки
				{
						if (new_text[i].Split(' ').Contains("Mesh"))
								subMeshCounter++; // счётчик сабмешей увеличился
				}
		//	записываем его в файл
				sw.Write(subMeshCounter);

				

				//	теперь нужно распарсить строки из х-файла
				//	и записать их в новый fxm_* файл 

						for (int i = 0; i < new_text.Count ; i++) // для каждой строки
						{

						//	это нужно сделать для каждой сабмеши
						//	поэтому создадим для них массивы элементов 





								if (new_text[i].Split(' ').Contains("Mesh"))	//	то следущая строка = количеству вершин 
							//if (new_text[i].StartsWith("Mesh "))
								{

										vertex_count = Convert.ToInt32(new_text[i+1].Replace(";", "").Trim(' '));	//	количество
										//Console.WriteLine(vertex_count);

								//	массив вершин

										for (int vc = 0; vc < vertex_count; vc++)		//	для каждой строки
										{
												string str = new_text[i+2+vc].Substring(0, new_text[i+2+vc].Length - 1);
												str = str.Replace(",", " ");
												str = str.Replace(";", " ");
												xyz = str.Split(new char[] {' ', '\n'}, 3, StringSplitOptions.RemoveEmptyEntries);

												//Console.WriteLine(xyz[0] + " " + xyz[1] + " " + xyz[2]);
												strListVerts.Add(xyz[0] + " " + xyz[1] + " " + xyz[2]);
										}

								//	массив граней

										faces__count = Convert.ToInt32(new_text[i+2+vertex_count].Replace(";", "").Trim());	//	количество
										//Console.WriteLine(faces__count);
										
										for (int fc = 0; fc < faces__count; fc++)		//	для каждой строки
										{
												string str = new_text[i+1+2+vertex_count+fc].Substring(2, new_text[i+1+2+vertex_count+fc].Length - 4);
												str = str.Replace(",", " ");
												str = str.Replace(";", "");
												asd = str.Split(new char[] {' ', '\n'}, 3, StringSplitOptions.RemoveEmptyEntries);
												//Console.WriteLine(asd[0] + " " + asd[1] + " " + asd[2]);

												strListFaces.Add(asd[0]);
												strListFaces.Add(asd[1]);
												strListFaces.Add(asd[2]);
										}
								}





						//	if (new_text[i].StartsWith("Texture_Filename"))
								if (new_text[i].Contains("\"")) // если содержит двойные кавычки, то это точно строка
								{
								    TextureFilename = new_text[i].Replace("\"", "").Replace(" ", "");
								    int index = new_text[i].IndexOf("{") + 1; // индекс кавычки // если стоят в одной строке
								    int size = TextureFilename.Length - index - 5; // размер без [.ext;]
										TextureFilename = TextureFilename.Substring(index, size);
								}





//////////////	Normals		//	код обрабатывает такие же строки как и в блоке Mesh 

								if (new_text[i].Split(' ').Contains("MeshNormals"))
								{
										int normal_count = Convert.ToInt32(new_text[i+1].Replace(";", "").Trim());	//	количество

										for (int vc = 0; vc < normal_count; vc++)		//	для каждой строки
										{
												string str = new_text[i+2+vc].Substring(0, new_text[i+2+vc].Length - 1);
												str = str.Replace(";", " ");	xyz = str.Split(new char[] {' ', '\n'}, 3, StringSplitOptions.RemoveEmptyEntries);
										//	Console.WriteLine(xyz[0] + " " + xyz[1] + " " + xyz[2]);
												strListNorms.Add(xyz[0] + " " + xyz[1] + " " + xyz[2]);
										}
								}





								if (new_text[i].Split(' ').Contains("MeshTextureCoords"))
								{
										vertex_count = Convert.ToInt32(new_text[i+1].Replace(";", "").Trim());	//	количество

										for (int vc = 0; vc < vertex_count; vc++)		//	для каждой строки
										{
												string str = new_text[i+2+vc].Substring(0, new_text[i+2+vc].Length - 1);
												str = str.Replace(";", " ");	asd = str.Split(new char[] {' ', '\n'}, 3, StringSplitOptions.RemoveEmptyEntries);
												//Console.WriteLine(asd[0] + " " + asd[1]);
												strListTexts.Add(asd[0] + " " + asd[1]);
										}
								}
								




								if (new_text[i].Split(' ').Contains("//"))
								{

								//	записываем размер +/= имя материала
										sw.Write(TextureFilename.Length);
										sw.Write(Encoding.ASCII.GetBytes(TextureFilename)); // byte[]

								//	какая то инфа после имени материала

										byte[] buffer = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
																		 0x12, 0x01, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00};
										sw.Write(buffer);

										sw.Write(faces__count);
										sw.Write(vertex_count);
									
										foreach (var q in strListFaces)	sw.Write(Convert.ToInt16(q));

										for (int iii = 0; iii < strListVerts.Count; iii++)
										{
												string[] splitXYZ = strListVerts[iii].Split();
												sw.Write(Convert.ToSingle(splitXYZ[0]));
												sw.Write(Convert.ToSingle(splitXYZ[1]));
												sw.Write(Convert.ToSingle(splitXYZ[2]));

												string[] splitNRML = strListNorms[iii].Split();
												sw.Write(Convert.ToSingle(splitNRML[0]));
												sw.Write(Convert.ToSingle(splitNRML[1]));
												sw.Write(Convert.ToSingle(splitNRML[2]));

												string[] splitUVST = strListTexts[iii].Split();
												sw.Write(Convert.ToSingle(splitUVST[0]));
												sw.Write(Convert.ToSingle(splitUVST[1])); 
										}

										strListVerts.Clear();
										strListNorms.Clear();
										strListTexts.Clear();
										strListFaces.Clear();
								}


						}
		}
}


				}		//	foreach (var xName in allFilesName)

		}		//	static void Main()

}		//	class Program
