//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
	using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class mot___2___Idol_FX
{
		static void Main()	
		{

		//	ищем все fmx файлы в папках и подпапках

				string[] allFilesName 
				= Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mot",  SearchOption.AllDirectories) ; 

		//	для каждого fmx файла 

				foreach (var motName in allFilesName)
				{
						Console.WriteLine(motName);

				//	открыли *.mot файл на чтение

						using (BinaryReader br = new BinaryReader(File.Open(motName, FileMode.Open)))
						{

						//	открыли *.txt на запись 

								using (StreamWriter sw = 
								new StreamWriter(Path.GetDirectoryName(motName) 
								+ "/" + Path.GetFileNameWithoutExtension(motName) + ".txt"))
								{
										ReadAndWriteInt32(br, sw, 1); 	sw.WriteLine();		//	1. считываем 4 байта		//	пусть это будет Int32 
										ReadAndWriteFloat(br, sw, 3); 	sw.WriteLine();		//	2. считываем 12 байт		//	пусть это будет 3 float 
										ReadAndWriteFloat(br, sw, 3); 	sw.WriteLine();		//	3. считываем 12 байт		//	пусть это будет 3 float 

										int roots__count = br.ReadInt32();	//	4. считываем 4 байта
										sw.WriteLine("количество узлов = " + roots__count); //	(число нод?) 
										if (roots__count <= 0 ) break;  //  если они <= 0 - закрываемся

	/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждого "узла"

										for (int root = 0; root < roots__count ; root++)  //  в цикле (по числу нод):
										{
												sw.WriteLine("\n====================================================\n");

												ReadAndWriteBytes(br, sw, 4);		//	6. считываем 4 байта

										//  7. считываем 4 байта длины строки						//	8. считываем строку

												sw.WriteLine("имя узла = " + ReadString(br, br.ReadInt32()));

	/////////////////////////////////////////////////////////////////////////////////////////

												sw.WriteLine("------------------------");

												int counter1 = br.ReadInt32();		//		9. считываем счётчик цикла (4 байта). 

												if (counter1 > 0) 	//	Если счётчик цикла > 0 то, запускаем цикл.
												{
														for (int i = 0; i < counter1; i++)
														{
																ReadAndWriteFloat(br, sw, 1);		//		считываем 4 байта
																ReadAndWriteFloat(br, sw, 4);		//		считываем 16 байт
														}
												}

												sw.WriteLine("------------------------");

	/////////////////////////////////////////////////////////////////////////////////////////

												int counter2 = br.ReadInt32();		//		11. считываем счётчик цикла (4 байта). 

												if (counter2 > 0) 	//	Если счётчик цикла > 0 то, запускаем цикл.
												{
														for (int i = 0; i < counter2; i++)
														{
																ReadAndWriteFloat(br, sw, 1);		//		считываем 4 байта
																ReadAndWriteFloat(br, sw, 3);		//		считываем 12 байт
														}
												}

												sw.WriteLine("------------------------");

	/////////////////////////////////////////////////////////////////////////////////////////

												int counter3 = br.ReadInt32();		//		13. считываем счётчик цикла (4 байта). 

												if (counter3 > 0) 	//	Если счётчик цикла > 0 то, запускаем цикл.
												{
														for (int i = 0; i < counter3; i++)
														{
																ReadAndWriteFloat(br, sw, 1);		//		считываем 4 байта
																ReadAndWriteFloat(br, sw, 3);		//		считываем 12 байт
														}
												}

	/////////////////////////////////////////////////////////////////////////////////////////

												int counter4 = br.ReadInt32();		//		15. считываем счётчик цикла (4 байта). 

												if (counter4 > 0) 	//	Если счётчик цикла > 0 то, запускаем цикл.
												{
														for (int i = 0; i < counter4; i++)
														{
																ReadAndWriteFloat(br, sw, 1);		//		считываем 4 байта
																ReadAndWriteFloat(br, sw, 16);	//		считываем 64 байт
														}
												}

	/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по nodes
								}		//	using StreamWriter	//	закрываем *.txt файл на запись  //	sw.Close();  //		sw.Dispose();
						}		//	using BinaryReader	//	закрываем файл на чтение
				}		//	foreach (var motName in allFilesName)
		}		//	static void Main()

//////////////////////////////////////////////////////////////////////////////////////////

//	sw.WriteLine(ReadString(br, br.ReadInt32()));

		static string ReadString(BinaryReader br, int nbyte)
		{
				byte[] name_h = new byte[nbyte];
				br.Read(name_h, 0, nbyte);
				return System.Text.Encoding.Default.GetString(name_h);
		}

//////////////////////////////////////////////////////////////////////////////////////////

//	ReadAndWriteFloat(br, sw, count);

		static void ReadAndWriteFloat(BinaryReader br, StreamWriter sw, int count)
		{
				for (int i = 0; i < count; i++)
				{
						sw.Write(br.ReadSingle() + "     ");
				} 
					sw.WriteLine();
		}

//////////////////////////////////////////////////////////////////////////////////////////

//	ReadAndWriteInt32(br, sw, count);

		static void ReadAndWriteInt32(BinaryReader br, StreamWriter sw, int count)
		{
				for (int i = 0; i < count; i++)
				{
						sw.Write(br.ReadInt32() + "     ");
				}
					sw.WriteLine();
		}

//////////////////////////////////////////////////////////////////////////////////////////

//	ReadAndWriteBytes(br, sw, count);

		static void ReadAndWriteBytes(BinaryReader br, StreamWriter sw, int count)
		{
				for (int i = 0; i < count; i++)
				{
						sw.Write(br.ReadByte() + "     ");
				}
					sw.WriteLine(); sw.WriteLine();
		}

//

}		//	class Program
