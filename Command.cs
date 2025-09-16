using BroadcastPluginSDK.abstracts;
using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using Command.Classes;
using Command.Forms;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Command
{
    public class Command : BroadcastPluginBase 
    {
        private const string STANZA = "Value";
        private ILogger<Command>? _logger;
        private readonly IConfiguration? _configuration;
        private readonly IPluginRegistry? _registry;
        private static InfoPage? _infoPage;
        private readonly static JobProcessor processor = new JobProcessor();
        private readonly  CommandList? _commandList;
        public ICache? Master;
        public Command() : base() { }

        public Command(IConfiguration configuration, ILogger<Command> logger , IPluginRegistry registry) :
            base(configuration, CreateControl(configuration , logger , processor), Resources.red, STANZA)
        {
            _logger = logger;
            _registry = registry;
            _configuration = configuration.GetSection(STANZA) ;
            double sampleRate = _configuration?.GetValue<int?>("SampleRate") ?? 10000;
            _commandList = new( logger , _configuration);

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

            _logger?.LogInformation($"Value Plugin initialized. Sample Rate: {sampleRate} ");
            processor.SetPage(_infoPage);
        }

        public static IInfoPage? CreateControl(IConfiguration configuration, ILogger<Command> logger , JobProcessor processor)
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
                return;
            }

            processor.SetMaster( Master);

            foreach ( var job in Master.CommandReader(BroadcastPluginSDK.Classes.CommandStatus.New) )
            {
                ImageChangedInvoke(Resources.green ); 

                UpdateJobStatus(job , CommandStatus.Queued );

                processor.EnqueueJob(job);

            }

            ImageChangedInvoke(Resources.red);
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