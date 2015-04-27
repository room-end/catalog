namespace CacheInBackground
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    sealed class Queue_Background
    {
     private BlockingCollection<int> queue;
        private CancellationTokenSource cancellation;

        internal Queue_Background()
        {
            queue = new BlockingCollection<int>();
            cancellation = new CancellationTokenSource();
        }

        internal void Enqueue(int i)
        {
            Console.Out.WriteLine("try to add {0}", i);
            queue.Add(i);
            Console.Out.WriteLine("{0} added", i);
        }

        internal void Cancel()
        {
            Console.Out.WriteLine("try to cancel");
            cancellation.Cancel();
            Console.Out.WriteLine("canceled");
        }

        internal async Task run()
        {
            await Task.Run(() =>
                {
                    try
                    {
                        while (true)
                        {
                            int y = queue.Take(cancellation.Token); //this blocks if there are no items in the queue.
                            //                       Task _do = Task.Run(() =>
                            {
                                //do whatever you have to do
                                Console.Out.WriteLine("entering {0}", y);
                                Thread.Sleep(1000);
                            }
                            //                       );
                            //if (_do.IsCompleted)
                            //    break;
                        }
                    }
                    catch (Exception x)
                    {
                        Console.Error.WriteLine(x);
                    }
                });
        }
    }
}
