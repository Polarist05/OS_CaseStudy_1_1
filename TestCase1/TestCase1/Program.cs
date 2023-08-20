using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Problem01
{

    class Program
    {
        const int size = 1000000000;
        const int threadCount = 8;
        const int length = size / threadCount;
        static byte[] Data_Global = new byte[size];
        static long Sum_Global = 0;
        static int G_index = 0;
        private static object obj = new object();
        [Obsolete]
        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            var arr = new byte[10];
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

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;

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
            Thread[] threads = new Thread[threadCount];
            ThreadStart start = new ThreadStart(sum);
            int i = threadCount - 1;
            sw.Start();
            for (; i >= 0; i--)
            {
                threads[i] = new Thread(start);
                threads[i].Start();
            }
            for (i = threadCount - 1; i >= 0; i--)
                threads[i].Join();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }

        private static void sum()
        {
            int total = 0, a;
            lock (obj)
            {
                a = G_index;
                G_index += length;
            }
            byte data;
            for (int j = length + a - 1; j >= a; j--)
            {
                data = Data_Global[j];
                if ((data & 1) == 0)
                {
                    total -= data;
                }
                else if (data % 3 == 0)
                {
                    total += data << 1;
                }
                else if (data % 5 == 0)
                {
                    total += data >> 1;
                }
                else if (data % 7 == 0)
                {
                    total += data / 3;
                }
            }
            lock (obj)
            {
                Sum_Global += total;
            }
        }
    }
}
