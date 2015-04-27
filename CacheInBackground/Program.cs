
namespace CacheInBackground
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            Queue_Background qb = new Queue_Background();
            Task t = qb.run();
            for (int i = 0; i < 10; i++)
            {
                qb.Enqueue(i);
            }
            while (true)
            {
                switch (Console.In.ReadLine())
                {
                    case "q":
                        qb.Cancel();
                        t.Wait();
                        return;
                }
            }
        }
    }
}
