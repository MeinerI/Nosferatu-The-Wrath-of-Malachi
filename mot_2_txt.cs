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

						//	открыли *.txt на запись 

								using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(motName) + "/" + Path.GetFileNameWithoutExtension(motName) + ".txt"))
								{
										PrintHexString(br, sw, 7);
										int roots__count = br.ReadInt32();
										sw.WriteLine("количество узлов = " + roots__count);
										PrintHexString(br, sw, 1);

	/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждого "узла"

										for (int root = 0; root < roots__count ; root++)
										{
												sw.WriteLine("\n========================\n" + "имя узла = " 
												+ ReadString(br, br.ReadInt32()) + "\n");

										//	каждую строку пишем в файл

	/////////////////////////////////////////////////////////////////////////////////////////

												int ts = br.ReadInt32();		//	02 00 00 00 или 06 00 00 00
												int zero = br.ReadInt32();	//	00 00 00 00

												int count = 0;

												if (ts == 2 || ts == 6)
												{
														if (ts == 2)	count = 4;
														if (ts == 6)	count = 14;

														sw.WriteLine("\n" + ts.ToString("00") + "-00-00-00\t\t\t00-00-00-00\t\t\t\n");

														ReadAndWriteFloat(br, sw, count);
														PrintHexString(br, sw, 1);	//	разделитель		
														ReadAndWriteFloat(br, sw, count);
												}
												else 
												{
														for (int i = 0; i < 153; i++)
														{
																if (i == ts && ts != 2 && ts != 6)
																{
																		count = (ts * 4) + (ts - 1);
																		break;
																}
														}

														ReadAndWriteFloat(br, sw, count);
												}

	/////////////////////////////////////////////////////////////////////////////////////////

												ts = br.ReadInt32();		//	02 или 03 или 06
												zero = br.ReadInt32();	//	00 00 00 00

												if (ts == 2 || ts == 3 || ts == 6)
												{
														if (ts == 2)	count = 3;
														if (ts == 3)	count = 5;
														if (ts == 6)	count = 11;

														sw.WriteLine("\n" + ts.ToString("00") + "-00-00-00\t\t\t00-00-00-00\t\t\t\n");

														ReadAndWriteFloat(br, sw, count);
														PrintHexString(br, sw, 1);	//	разделитель		
														ReadAndWriteFloat(br, sw, count);
												}

												else 
												{
														for (int i = 4; i < 153; i++)
														{
																if (i == ts && ts != 2 && ts != 3 && ts != 6)
																{
																		count = (ts * 4) - 1;
																		break;
																}
														}

														ReadAndWriteFloat(br, sw, count);
												}

	/////////////////////////////////////////////////////////////////////////////////////////

										//	end:

												ts = br.ReadInt32();		//	02 00 00 00
												zero = br.ReadInt32();	//	00 00 00 00

												if (ts == 2)
												{
														if (ts == 2)	count = 3;

														sw.WriteLine("\n" + ts.ToString("00") + "-00-00-00\t\t\t00-00-00-00\t\t\t\n");

														ReadAndWriteFloat(br, sw, count);
														PrintHexString(br, sw, 1);	//	разделитель		
														ReadAndWriteFloat(br, sw, count);
												}
												else
												{
														for (int i = 3; i < 153; i++)
														{
																if (i == ts && ts != 2)
																{
																		count = (ts * 4) - 1;
																		break;
																}
														}

														ReadAndWriteFloat(br, sw, count);
												}

	/////////////////////////////////////////////////////////////////////////////////////////

										//	у последнего блока нет разделителя
										//	или он есть у всех, но идёт в начале (после имени? блока) {что!? не помню}
										//	поэтому не читаем последние 4 байта файла - их не существует

												var stream = br.BaseStream;
												if (stream.Position == stream.Length - 4) break;

	/////////////////////////////////////////////////////////////////////////////////////////

												PrintHexString(br, sw, 2);	//	разделитель		//	00 00 00 00 AB AA 2A 3E

	/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам
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

//	ReadAndWriteFloat(br, sw, 4);

		static void ReadAndWriteFloat(BinaryReader br, StreamWriter sw, int count)
		{
				for (int i = 0; i < count; i++)
				{
						sw.WriteLine(br.ReadSingle());
				}
		}

//

}		//	class Program
