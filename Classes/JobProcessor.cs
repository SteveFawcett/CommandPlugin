using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CommandPlugin.Classes
{
    public class JobProcessor
    {
        private readonly Channel<Func<Task>> _channel;
        private readonly CancellationTokenSource _cts = new();

        public JobProcessor()
        {
            _channel = Channel.CreateUnbounded<Func<Task>>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

            Task.Run(ProcessQueueAsync);
        }

        public void EnqueueJob(Func<Task> job)
        {
            if (!_channel.Writer.TryWrite(job))
                throw new InvalidOperationException("Failed to enqueue job.");
        }

        private async Task ProcessQueueAsync()
        {
            await foreach (var job in _channel.Reader.ReadAllAsync(_cts.Token))
            {
                try
                {
                    await job();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Job failed: {ex.Message}");
                }
            }
        }

        public void Stop() => _cts.Cancel();
    }

}
