
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
    public class Command : BroadcastPluginBase , ICommandHandler
    {
        private const string STANZA = "Command";
        private ILogger<Command>? _logger;
        private readonly IConfiguration? _configuration;
        private static InfoPage? _infoPage;
        private readonly static JobProcessor processor = new JobProcessor();
        private static readonly Dictionary<string, string> JobList = new();
        public Command() : base() { }

        public Command(IConfiguration configuration, ILogger<Command> logger , IPluginRegistry registry) :
            base(configuration, CreateControl(configuration , logger , registry,  processor), Resources.red, STANZA)
        {
            _logger = logger;
            _configuration = configuration.GetSection(STANZA) ;
             
            _logger?.LogInformation("Command Plugin Starting");

            processor.SetPage(_infoPage);

            var sampleRate = _configuration.GetValue("SampleRate", 10000.0); 

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
        private static IInfoPage? CreateControl(IConfiguration configuration, ILogger<Command> logger , IPluginRegistry pluginRegistry, JobProcessor processor)
        {
            _infoPage = new InfoPage(configuration.GetSection(STANZA), logger , processor , pluginRegistry );
            return _infoPage;
        }
        public void CommandHandler(CommandItem cmd)
        {
            if( _infoPage != null ) _infoPage?.AddJob(cmd);

            if( cmd.CommandType != CommandTypes.OperatingSystem )
            {
                _logger?.LogInformation("Only Operating System commands are supported");
                return;
            }

            if (cmd.Status == CommandStatus.Queued)
            {
                cmd.Status = CommandStatus.InProgress;
                processor.EnqueueJob(cmd);
            }
        }
    }
}