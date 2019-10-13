//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class mot2obj
{
		static void Main()	
		{

		//	ищем все fmx файлы в папках и подпапках

				string[] allFilesName = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mot",  SearchOption.AllDirectories) ; 

		//	для каждого fmx файла 

				foreach (var motName in allFilesName)
				{
						Console.WriteLine(motName);

				//	открыли *.mot файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(motName, FileMode.Open)))
						{

						//	открыли *.x на запись 

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(motName) + "/" + Path.GetFileNameWithoutExtension(motName) + ".x"))
								{
										PrintHexString(br, sw, 7);
										int roots__count = BitConverter.ToInt32(BitConverter.GetBytes(br.ReadSingle()), 0);
										sw.WriteLine("количество узлов = " + roots__count);
										PrintHexString(br, sw, 1);

	/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждого "узла"

										for (int root = 0; root < roots__count ; root++)
										{
												sw.WriteLine("\n========================\n" + "имя узла = " 
												+ ReadString(br, HexFloat2Int32(br.ReadSingle())) + "\n");

										//	каждую строку пишем в файл

												PrintHexString(br, sw, 2);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												PrintHexString(br, sw, 1);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												PrintHexString(br, sw, 2);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												PrintHexString(br, sw, 1);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												PrintHexString(br, sw, 2);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												PrintHexString(br, sw, 1);	//	разделитель

												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());
												sw.WriteLine(br.ReadSingle());

												var stream = br.BaseStream;
												if (stream.Position == stream.Length - 4) break;

												PrintHexString(br, sw, 2);	//	разделитель

	/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам
								}		//	using StreamWriter	//	закрываем *.x файл на запись  //	sw.Close();  //		sw.Dispose();
						}		//	using BinaryReader	//	закрываем файл на чтение
				}		//	foreach (var motName in allFilesName)
		}		//	static void Main()

//////////////////////////////////////////////////////////////////////////////////////////

//	HexFloat2Int32(br.ReadSingle())

		static int HexFloat2Int32(float float_value)
		{
				return BitConverter.ToInt32(BitConverter.GetBytes(float_value), 0);
		}

//////////////////////////////////////////////////////////////////////////////////////////

//	sw.WriteLine(ReadString(br, HexFloat2Int32(br.ReadSingle())));

		static string ReadString(BinaryReader br, int nbyte)
		{
				byte[] name_h = new byte[nbyte];
				br.Read(name_h, 0, nbyte);
				return System.Text.Encoding.Default.GetString(name_h);
		}

//////////////////////////////////////////////////////////////////////////////////////////

		static void PrintHexString(BinaryReader br, StreamWriter sw, int count)
		{
				sw.WriteLine();

				for (int zero = 0; zero < count; zero++)
				{
						sw.Write(BitConverter.ToString(BitConverter.GetBytes(br.ReadSingle())) + "\t\t\t"); 
				}

				sw.WriteLine();
				sw.WriteLine();
		}

//////////////////////////////////////////////////////////////////////////////////////////

//

}		//	class Program
