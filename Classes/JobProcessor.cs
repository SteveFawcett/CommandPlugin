using Accessibility;
using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using Command.Forms;
using System.Diagnostics;
using System.Threading.Channels;

namespace Command.Classes
{
    public class JobProcessor
    {
        private readonly Channel<CommandItem> _channel;
        private readonly CancellationTokenSource _cts = new();
        private ICache? _Master;
        private InfoPage? _infoPage;

        public JobProcessor()
        {
            _channel = Channel.CreateUnbounded<CommandItem>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

            Task.Run(ProcessQueueAsync);
        }

        public void EnqueueJob(CommandItem job)
        {
            if (!_channel.Writer.TryWrite(job))
                throw new InvalidOperationException("Failed to enqueue job.");
        }

        public async Task<Dictionary<string,string>> RunCommandAsync(string command, string arguments)
        { 
            string switches = "/c";

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"{switches} {command} \"{arguments}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            return new Dictionary<string, string>
            {
                { "STDIO" , output },
                { "STDERR", error }
            };
        }

        private async Task ProcessQueueAsync()
        {
            var semaphore = new SemaphoreSlim(1); // Limit concurrent jobs

            await foreach (var job in _channel.Reader.ReadAllAsync(_cts.Token))
            {
                await semaphore.WaitAsync();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        job.Status = CommandStatus.InProgress;
                        _Master?.CommandWriter(job);

                        var item = CommandList.GetCommandDetails(job.Command);
                        var output = await RunCommandAsync(item.Value, "");

                        job.Result = output["STDIO"];
                        job.Errors = output["STDERR"];

                        job.Status = string.IsNullOrEmpty(job.Errors)
                            ? CommandStatus.Completed
                            : CommandStatus.Failed;

                        _Master?.CommandWriter(job);
                    }
                    catch (Exception ex)
                    {
                        job.Status = CommandStatus.Failed;
                        _Master?.CommandWriter(job);
                        Debug.WriteLine($"Job failed: {ex.Message}");
                    }
                    finally
                    {
                        _infoPage?.RefreshJobs();
                        semaphore.Release();
                    }
                });
            }
        }


        public void Stop() => _cts.Cancel();

        internal void SetMaster(ICache master)
        {
            _Master = master;
        }

        internal void SetPage(InfoPage? infoPage)
        {
            _infoPage = infoPage;
        }
    }

}
