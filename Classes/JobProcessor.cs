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
            string output = string.Empty;
            string error = string.Empty;

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"{switches} {command} \"{arguments}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = new Process { StartInfo = psi };
                process.Start();

                output = await process.StandardOutput.ReadToEndAsync();
                error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error running command: {ex.Message}");
                error = ex.Message;
            }

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

                Debug.WriteLine( $"[ProcessQueueAsync] : Running job: {job.Value} , Command: {job.FullComand}" );  

                try
                {
                    if( string.IsNullOrEmpty( job.FullComand ) )
                    {
                        job.Status = CommandStatus.Failed;
                        job.Errors = "No command specified.";
                        _Master?.CommandWriter(job);
                        _infoPage?.RefreshJobs();
                        semaphore.Release();
                        continue;
                    }

                    job.Status = CommandStatus.InProgress;
                    _Master?.CommandWriter(job);

                    var output = await RunCommandAsync( job.FullComand  , "");

                    job.Result =  output["STDIO"];
                    job.Errors =  output["STDERR"];

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
            }
        }

        public void Stop() => _cts.Cancel();

        internal void SetMaster(ICache master)
        {
            Debug.WriteLine($"[SetMaster] Setting master cache : {master.ToString()}");

            _Master = master;
        }

        internal void SetPage(InfoPage? infoPage)
        {
            _infoPage = infoPage;
        }
    }

}
