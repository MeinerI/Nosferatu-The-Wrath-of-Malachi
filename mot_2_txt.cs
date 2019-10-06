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

										br.ReadSingle();	//	первые 4 обычно равны 01 00 00 00
										
										float hz1 = br.ReadSingle();		//	какое то значение

										sw.WriteLine("hz1 = " + hz1);

										br.ReadSingle();	//	обычно равны 00 00 00 00

										float hz2 = br.ReadSingle();		//	какое то значение

										sw.WriteLine("hz2 = " + hz2);

								//	пропускаем 12 = 3 по 4

										for (int temp_s = 0; temp_s < 3; temp_s++ ) br.ReadSingle();

								//	количество "узлов"

										float roots__hex = br.ReadSingle();
										byte[] byteArray = BitConverter.GetBytes(roots__hex);
										int roots__count = BitConverter.ToInt32(byteArray, 0);

										sw.WriteLine("количество узлов = " + roots__count);

	/////////////////////////////////////////////////////////////////////////////////////////

								//	для каждого "узла"

										for (int root = 0; root < roots__count ; root++)
										{

												sw.WriteLine("\n========================\n");

												float hz3 = br.ReadSingle();		//	какое то значение
												sw.WriteLine("hz3 = " + hz3);

{										//	читаем количество букв в имени "узла"

												float name__lengtf = br.ReadSingle();
												byteArray = BitConverter.GetBytes(name__lengtf);
												int name__length = BitConverter.ToInt32 (byteArray, 0);
												sw.WriteLine("длина имени узла = " + name__length);

										//	читаем имя "узла"

												byte[] bipedNameHex = new byte[name__length];
												br.Read(bipedNameHex, 0, name__length);
												string bipedName = System.Text.Encoding.Default.GetString(bipedNameHex);

												sw.WriteLine("имя узла = " + bipedName + "\n");
}

	/////////////////////////////////////////////////////////////////////////////////////////

{										//	каждую строку пишем в файл

												for ( int i = 0 ; i < 30 ; i++ )
												{
														float temp = br.ReadSingle();
														
												//	разделитель 02 00 00 00 . 00 00 00 00
												//	if (temp == (2.802597e-45f)) sw.WriteLine();

														sw.WriteLine(temp);
												}
}

	/////////////////////////////////////////////////////////////////////////////////////////

										}		//	проход по сабмешам

								}		//	using StreamWriter	//	закрываем *.txt файл на запись  //	sw.Close();  //		sw.Dispose();

						}		//	using BinaryReader	//	закрываем файл на чтение

				}		//	foreach (var motName in allFilesName)

		}		//	static void Main()

}		//	class Program
