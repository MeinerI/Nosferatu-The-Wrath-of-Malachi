//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class fxf2obj
{
		static void Main()	
		{

		//	ищем все fmx файлы в папках и подпапках

				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.fxf",  SearchOption.AllDirectories) ; 

		//	для каждого fmx файла 

				foreach (var fxfName in allFilesName)
				{
						Console.WriteLine(fxfName);

				//	открыли *.fxf файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(fxfName, FileMode.Open)))
						{

						//	открыли *.txt на запись 

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(fxfName) 
												+ "/" + Path.GetFileNameWithoutExtension(fxfName) + ".txt"))
								{

										float hz1 = br.ReadSingle();	//	04 00 00 00		//	
										float hz2 = br.ReadSingle();	//	00 00 C8 42		//	single = 100

										for (int zero = 0; zero < 4 ; zero++) br.ReadSingle();	
										//	00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 

//////////////////	какой то первый блок состоит из 8 "пронумерованных" блоков по 17,26,12,1,15,1,29,1 "папок"

										float big_block_h = br.ReadSingle();	//	08 00 00 00		//	8  блоков
										byte[] byteArray = BitConverter.GetBytes(big_block_h);
										int big_block_count = BitConverter.ToInt32 (byteArray, 0);

										for (int big_block = 0; big_block < big_block_count ; big_block++)
										{
												float count_h = br.ReadSingle();		//	читаем количество под-блоков 
												byteArray = BitConverter.GetBytes(count_h);
												int count = BitConverter.ToInt32 (byteArray, 0);

												for (int root = 0; root < count ; root++)	//	читаем "под-блоки"
												{
														float lengtf = br.ReadSingle();	//	читаем длину имени блока
														byteArray = BitConverter.GetBytes(lengtf);
														int length = BitConverter.ToInt32 (byteArray, 0);

														byte[] name_h = new byte[length];		//	читаем название блока
														br.Read(name_h, 0, length);
														string name = System.Text.Encoding.Default.GetString(name_h);
														sw.WriteLine("name = " + name);
												}

												sw.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
										}

//////////////////	hz

										for (int zero = 0; zero < 2 ; zero++) 
										sw.Write(BitConverter.ToString(BitConverter.GetBytes(br.ReadSingle())) + "\t\t\t"); sw.WriteLine();
										sw.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");

////////////////////

										Console.WriteLine(ReadReference(br, sw, 6));	//	01 Ref Sound
										Console.WriteLine(ReadReference(br, sw, 9));	//	02 Ref Texture
										Console.WriteLine(ReadReference(br, sw, 22));	//	03 Ref Mesh

										Console.WriteLine(ReadMaterial(br, sw));	//	04 Ref Material

										Console.WriteLine(ReadMaterial(br, sw));	//	10
										
										Console.WriteLine(ReadReference(br, sw, 9));	//	11
										Console.WriteLine(ReadReference(br, sw, 9));	//	12

										Console.WriteLine(ReadMaterial(br, sw));	//	13	
										Console.WriteLine(ReadMaterial(br, sw));	//	14

										for (int repeater = 0; repeater < 41; repeater++)
										{
												Console.WriteLine(ReadReference(br, sw, 9));	//	с  15
												Console.WriteLine(ReadMaterial(br, sw));			//	по 96
										}

										for (int repeater = 0; repeater < 7; repeater++)
										{
												Console.WriteLine(ReadReference(br, sw, 4));	//	с 97 по 103
										}

										Console.WriteLine(ReadReference(br, sw, 9));
										Console.WriteLine(ReadReference(br, sw, 4));

										Console.WriteLine(ReadMaterial(br, sw));
										
										Console.WriteLine(ReadReference(br, sw, 4));
										Console.WriteLine(ReadReference(br, sw, 9));
										Console.WriteLine(ReadReference(br, sw, 4));

										Console.WriteLine(ReadMaterial(br, sw));

										Console.WriteLine(ReadReference(br, sw, 9));
										Console.WriteLine(ReadMaterial(br, sw));
										Console.WriteLine(ReadReference(br, sw, 9));
										Console.WriteLine(ReadReference(br, sw, 9));

										var stream = br.BaseStream;
										Console.WriteLine("\n"+stream.Position);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

								}		//	using StreamWriter	//	закрываем *.txt файл на запись  //	sw.Close();  //		sw.Dispose();

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var fxfName in allFilesName)

		}		//	static void Main()

//////

//	HexFloat2Int32(br.ReadSingle())

		static int HexFloat2Int32(float float_value)
		{
				byte[] b_ar = BitConverter.GetBytes(float_value);
				return BitConverter.ToInt32 (b_ar, 0);
		}

//////

//	sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())));

		static string ReadString(BinaryReader br, int nbyte)
		{
				byte[] name_h = new byte[nbyte];
				br.Read(name_h, 0, nbyte);
				return System.Text.Encoding.Default.GetString(name_h);
		}

//////

//	count = 37	//	ReadBig

		static int ReadMaterial(BinaryReader br, StreamWriter sw)
		{
				int n_block = HexFloat2Int32(br.ReadSingle());
				sw.WriteLine("№ блока = " + n_block + "\n");				br.ReadSingle();	//	00 00 00 00 
				sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())) + "\n");

				for (int zero = 0; zero < 37; zero++)
				sw.Write(BitConverter.ToString(BitConverter.GetBytes(br.ReadSingle())) + "\t\t\t"); 
				sw.WriteLine("\n\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
				return n_block;
		}

//////

//	.x			count = 4

//	.jpg		count = 9
//	.wav		count = 9
//	.tga		count = 9

		static int ReadReference(BinaryReader br, StreamWriter sw, int count)
		{
				int n_block = HexFloat2Int32(br.ReadSingle());
				sw.WriteLine("№ блока = " + n_block + "\n");				br.ReadSingle();	//	00 00 00 00 
				sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())));
				sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())));
				sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())));					sw.WriteLine();

				for (int zero = 0; zero < count ; zero++) 
				sw.Write(BitConverter.ToString(BitConverter.GetBytes(br.ReadSingle())) + "\t\t\t"); 
				sw.WriteLine("\n\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
				return n_block;
		}

//////

}		//	class Program
