using BroadcastPluginSDK.abstracts;
using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using CommandPlugin.Classes;
using CommandPlugin.Forms;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Command
{
    public class Command : BroadcastPluginBase 
    {
        private const string STANZA = "Command";
        private ILogger<IPlugin>? _logger;
        private readonly IConfiguration? _configuration;
        private readonly IPluginRegistry? _registry;
        private static InfoPage? _infoPage;
        private readonly static JobProcessor processor = new JobProcessor();

        public ICache? Master;
        public Command() : base() { }

        public Command(IConfiguration configuration, ILogger<IPlugin> logger , IPluginRegistry registry) :
            base(configuration, CreateControl(configuration , logger , processor), Resources.red, STANZA)
        {
            _logger = logger;
            _registry = registry;
            _configuration = configuration.GetSection(STANZA) ;
            double sampleRate = _configuration?.GetValue<int?>("SampleRate") ?? 1000;

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(sampleRate));
                _ = Task.Run(async () =>
                {
                    while (await timer.WaitForNextTickAsync())
                    {
                        if (_registry != null)
                        {
                            Master = _registry?.MasterCache();
                            GetOpenJobs();
                        }
                    }
                });

            _logger?.LogInformation($"Command Plugin initialized. Sample Rate: {sampleRate} ");
        }

        public static IInfoPage? CreateControl(IConfiguration configuration, ILogger<IPlugin> logger , JobProcessor processor)
        {
            // Return your UserControl here, make sure you use UserControl and the IInfoPage interface
            _infoPage = new InfoPage(configuration.GetSection(STANZA), logger , processor);
            return _infoPage;
        }

        private void GetOpenJobs()
        {
            _logger?.LogDebug("Requesting list of Jobs");

            if ( Master == null )
            {
                _logger?.LogWarning("No master cache defined, cannot get open jobs.");
                return;
            }

            foreach ( var job in Master.CommandReader(BroadcastPluginSDK.Classes.CommandStatus.New) )
            {
                ImageChangedInvoke(Resources.green ); 

                UpdateJobStatus(job , CommandStatus.Queued );

                processor.EnqueueJob(() =>
                {
                    UpdateJobStatus(job, CommandStatus.InProgress);
                    bool success = ExecuteJob(job);
                    UpdateJobStatus(job, success ? CommandStatus.Completed : CommandStatus.Failed);
                    return Task.CompletedTask;
                });
            }

            ImageChangedInvoke(Resources.red);
        }

        private bool ExecuteJob(CommandItem job )
        {
            // Execute the command here
            // This is a placeholder for actual command execution logic
            _logger?.LogInformation("Executing job {job}", job.Id);

            return true;
        }

        private void UpdateJobStatus(CommandItem job , CommandStatus newStatus )
        {
            job.Status = newStatus;

            if (_infoPage != null)
            {
                _infoPage.JobCard(job);
            }

            Master?.CommandWriter(job);
        }
    }
}