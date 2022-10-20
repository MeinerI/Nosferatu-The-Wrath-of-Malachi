
namespace FXA
{
	public static partial class Program
	{
		private static string READ_STRING()
		{
			int string_size = fxaReader.ReadInt32();  //  читаем длину имени блока

			string block_str_name = "";

			if (string_size > 0) // если длина строки больше ноля
			{
				byte[] byteArray = new byte[string_size];
				fxaReader.Read(byteArray, 0, string_size);
				block_str_name = System.Text.Encoding.Default.GetString(byteArray);
			}

			return block_str_name;
		}
	}
}
