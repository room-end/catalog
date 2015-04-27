namespace catalog.viewer
{
    using catalog.viewer.Helpers;
    using catalog.viewer.Model.Photography;
    using catalog.viewer.Properties;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal delegate void processing_message(Frame frame, string message);

    internal sealed class Queue_Resize
    {
        internal static Queue_Resize instance
        {
            get
            {
                return _self.Value;
            }
        }

        internal event processing_message resize_message;

        private Queue_Resize()
        {
        }

        internal void resize(Frame frame, int width, int height)
        {
            if (_runner == null)
                _runner = run();
            if (frame == null)
                throw new ArgumentNullException("frame");
            if (!File.Exists(Path.Combine(frame.path, frame.filename)))
                return;
            Directory.CreateDirectory(frame.path_preview);
            queue.Add(frame);
        }

        internal void resize(Frame frame)
        {
            if (_runner == null)
                _runner = run();
            if (frame == null)
                throw new ArgumentNullException("frame");
            queue.Add(frame);
        }

        internal void cancel()
        {
            try
            {
                cancel_queue.Cancel();
                cancel_task.Cancel();
            }
            catch (Exception)
            {
                Trace.TraceError("may not be!");
            }
        }

        internal Task run()
        {
            _runner = new Task(o =>
            {
                try
                {
                    while (true)
                    {
                        Frame frame = queue.Take(cancel_queue.Token); //this blocks if there are no items in the queue.
                        Task _inner_task = new Task(_ =>
                        {
                            FileInfo src_fi = new FileInfo(Path.Combine(frame.path, frame.filename));
                            if (!src_fi.Exists)
                                return;
                            Directory.CreateDirectory(frame.path_preview);
                            FileInfo dst_fi = new FileInfo(Path.Combine(frame.path_preview, frame.preview_filename));
                            if (dst_fi.Exists && (src_fi.LastWriteTime < dst_fi.LastWriteTime))
                                return;

                            int width = Settings.Default.width;
                            int height = Settings.Default.height;
                            if (resize_message != null)
                                resize_message(frame, string.Format("Resizing '{0}'...", Path.Combine(frame.path, frame.filename)));
                            //do whatever you have to do
                            try
                            {
                                TIFF.Resize(frame.path, frame.path_preview, frame.filename, width, height);
                                if (resize_message != null)
                                    resize_message(frame, string.Format("'{0}' resized", Path.Combine(frame.path, frame.filename)));
                            }
                            catch (Exception x)
                            {
                                if (resize_message != null)
                                    resize_message(frame, string.Format("Resizng '{0}' failed: {1}", Path.Combine(frame.path, frame.filename), x.Message));
                            }
                        }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.PreferFairness);
                        _inner_task.Start(scheduler);
                    }
                }
                catch (OperationCanceledException)
                {
                    Trace.TraceWarning("run canceled");
                }
                catch (Exception x)
                {
                    if (resize_message != null)
                        resize_message(null, string.Format("Resizng Queue failed: {0}", x.Message));
                }
            }, cancel_task, TaskCreationOptions.LongRunning);
            _runner.Start();
            return _runner;
        }
        private BlockingCollection<Frame> queue = new BlockingCollection<Frame>();
        private CancellationTokenSource cancel_queue = new CancellationTokenSource();
        private CancellationTokenSource cancel_task = new CancellationTokenSource();
        private LimitedConcurrencyLevelTaskScheduler scheduler = new LimitedConcurrencyLevelTaskScheduler(Environment.ProcessorCount);
        private Task _runner = null;
        private static readonly Lazy<Queue_Resize> _self = new Lazy<Queue_Resize>(() => new Queue_Resize());
    }
}
