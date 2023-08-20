using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Problem01
{
    class Program
    {
        const int size = 1000000000,threadCount =8,length=size/threadCount;
        static object obj = new object();
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
            int startIndex,endIndex,total=0;
            lock (obj)
            {
                startIndex = G_index;
                G_index += length;
                endIndex = G_index;
            }
            for (;startIndex<endIndex ;startIndex++ )
            {
                byte data = Data_Global[startIndex];
                if (data % 2 == 0)
                {
                    total -= data;
                }
                else if (data % 3 == 0)
                {
                    total += data * 2;
                }
                else if (data % 5 == 0)
                {
                    total += data / 2;
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
            //Data_Global[G_index] = 0;
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
            Thread[] threads = new Thread[threadCount];
            sw.Start();
            for (i = 0; i < threadCount; i++) {
                threads[i] = new Thread(new ThreadStart(sum));
                threads[i].Start();
            }
            for (i = 0; i < threadCount; i++)
                threads[i].Join();
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}