using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Problem01
{
    class Program
    {
        const int size = 1000000000;
        static byte[] Data_Global = new byte[size];
        static long Sum_Global = 0;
        static int G_index = 0;
        [Obsolete]
        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                Data_Global = (byte[])bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void sum()
        {
            int total = 0;
            byte data = Data_Global[G_index];
            if (data % 2 == 0)
            {
                total -= data;
            }
            else if (data % 3 == 0)
            {
                total += (data * 2);
            }
            else if (data% 5 == 0)
            {
                total += (data / 2);
            }
            else if (data % 7 == 0)
            {
                total += (data / 3);
            }
            //Data_Global[G_index] = 0;
            Sum_Global += total;
            G_index++;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();
            for (i = 0; i < size; i++)
                sum();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}