using BroadcastPluginSDK;
using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using Command.Classes;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Command.Forms 
{
    public partial class InfoPage : UserControl, IInfoPage
    {
        private readonly ILogger<Command> _logger;
        private readonly IConfiguration _configuration;
        private readonly JobProcessor _processor;
        private readonly IPluginRegistry? _registry;

        public Image Icon
        {
            set
            {
                if (value != null)
                {
                    LogoPictureBox.Image = value;
                }
            }
            get
            {
                return LogoPictureBox.Image;
            }
        }

        public InfoPage(IConfiguration configuration, ILogger<Command> logger, JobProcessor processor, IPluginRegistry registry)
        {
            _logger = logger;
            _configuration = configuration;
            _processor = processor;
            _registry = registry;

            InitializeComponent();

            JobListBox.OnItemSelected += (s, e) =>
            {
                var res = new Results();
                res.SetText = e.Items[0].Result + "Errors\n\n" + e.Items[0].Errors ;
                res.ShowDialog();
            };

            Icon = Resources.red;
            jobTypes.SelectedIndex = 1;

            _registry = registry;
        }
        public Control GetControl()
        {
            return this;
        }

        private bool ShouldShow(CommandItem item)
        {
            bool shouldshow = jobTypes.Text switch
            {
                "Active" => item.Status == CommandStatus.InProgress || item.Status == CommandStatus.New || item.Status == CommandStatus.Queued,
                "Failed" => item.Status == CommandStatus.Failed,
                "Completed" => item.Status == CommandStatus.Completed,
                _ => true
            };

            return shouldshow;
        }

        private void ApplyJobFilter()
        {
            if (JobListBox.InvokeRequired)
            {
                JobListBox.Invoke(new MethodInvoker(ApplyJobFilter));
                return;
            }

            JobListBox.FilterItems( item => ShouldShow(item) );
        }
   
        private void jobTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyJobFilter();
        }

        internal void RefreshJobs()
        {
            ApplyJobFilter();
        }

        internal void AddJob(CommandItem job)
        {
            JobListBox.AddUpdateItem(job);
        }

    }
}
