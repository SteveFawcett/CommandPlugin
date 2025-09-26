
using BroadcastPluginSDK.abstracts;
using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using Command.Classes;
using Command.Forms;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace Command
{
    public class Command : BroadcastPluginBase 
    {
        private const string STANZA = "Command";
        private ILogger<Command>? _logger;
        private readonly IConfiguration? _configuration;
        private readonly IPluginRegistry? _registry;
        private static InfoPage? _infoPage;
        private readonly static JobProcessor processor = new JobProcessor();
        private static readonly Dictionary<string, string> JobList = new();
        public Command() : base() { }
        private ICache? Master;

        public Command(IConfiguration configuration, ILogger<Command> logger , IPluginRegistry registry) :
            base(configuration, CreateControl(configuration , logger , registry,  processor), Resources.red, STANZA)
        {
            _logger = logger;
            _registry = registry;
            _configuration = configuration.GetSection(STANZA) ;
                       
            processor.SetPage(_infoPage);

            var sampleRate = _configuration.GetValue("SampleRate", 10000.0); 

            StartPeriodicTimerAsync( sampleRate);

            foreach( var job in _configuration.GetSection("Jobs").GetChildren() )
            {
                var key = job.GetValue<string>("Name", string.Empty);
                var value = job.GetValue<string>("Command", string.Empty);
                if( !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) )
                {
                    JobList.Add(key, value);
                }
            }
        }

        private void Command_ImageChangedInvoke(object? sender, Image e)
        {
            ImageChangedInvoke( e);
        }

        public static IInfoPage? CreateControl(IConfiguration configuration, ILogger<Command> logger , IPluginRegistry pluginRegistry, JobProcessor processor)
        {
            _infoPage = new InfoPage(configuration.GetSection(STANZA), logger , processor , pluginRegistry );
            return _infoPage;
        }
        private async Task StartPeriodicTimerAsync(double sampleRate)
        {
            _logger?.LogInformation("Starting Periodic Timer");

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(sampleRate));
            _ = Task.Run(async () =>
            {
                while (await timer.WaitForNextTickAsync())
                {
                    if (_registry != null)
                    {
                        try
                        {
                            Master = _registry.MasterCache();
                            GetAllJobsInCache();
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Error accessing Master Cache from Registry");
                        }
                    }
                }
            });
        }

        private void GetAllJobsInCache()
        {
            _logger?.LogDebug("Requesting list of Jobs");

            if (Master == null)
            {
                return;
            }

            processor.SetMaster(Master);

            foreach (CommandStatus status in Enum.GetValues(typeof(CommandStatus)))
            {
                if (status == CommandStatus.Completed || status == CommandStatus.Failed)
                {
                    continue; // Skip Completed and Failed jobs
                }

                // Get all jobs with status New and enqueue them for processing
                foreach (var job in Master.CommandReader(status))
                {
                    _logger?.LogInformation($"Enqueuing Job {job.Key} for processing, status {job.Status}");
                    ImageChangedInvoke(Resources.green);

                    if (string.IsNullOrEmpty(job.FullComand))
                    {
                        job.FullComand = JobList.TryGetValue(job.Value, out var result) ? result : null;
                        _logger?.LogInformation($"Found command {job.Value} => {job.FullComand}");
                    }

                    _infoPage?.AddJob(job);

                    if (job.Status == CommandStatus.New)
                    {
                        job.Status = CommandStatus.Queued;
                        processor.EnqueueJob(job);
                    }
                }
                ImageChangedInvoke(Resources.red);
            }
        }
    }
}