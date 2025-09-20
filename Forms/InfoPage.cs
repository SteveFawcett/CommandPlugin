using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using Command.Classes;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace Command.Forms 
{
    public partial class InfoPage : UserControl, IInfoPage
    {
        private readonly ILogger<Command> _logger;
        private readonly IConfiguration _configuration;
        private readonly JobProcessor _processor;

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

        public InfoPage(IConfiguration configuration, ILogger<Command> logger, JobProcessor processor)
        {
            _logger = logger;
            _configuration = configuration;
            _processor = processor;

            InitializeComponent();

            // Some test data
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 1", Value = "dir", Status = CommandStatus.Queued });
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 2", Value = "dir", Status = CommandStatus.New });
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 3", Value = "dir", Status = CommandStatus.New });
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 4", Value = "dir", Status = CommandStatus.Completed });
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 5", Value = "dir", Status = CommandStatus.Failed });
            JobListBox.AddUpdateItem(new CommandItem { Key = "Sample Job 6", Value = "dir", Status = CommandStatus.Completed });

            Icon = Resources.red;
            jobTypes.SelectedIndex = 1;
        }

        public Control GetControl()
        {
            return this;
        }

        private bool ShouldHide(CommandItem item)
        {
            bool shouldHide = jobTypes.Text switch
            {
                "Active" => item.Status == CommandStatus.Completed || item.Status == CommandStatus.Failed,
                "Failed" => item.Status != CommandStatus.Failed,
                "Completed" => item.Status != CommandStatus.Completed,
                _ => false
            };

            return shouldHide;
        }

        private void ApplyJobFilter()
        {
            if (JobListBox.InvokeRequired)
            {
                JobListBox.Invoke(new MethodInvoker(ApplyJobFilter));
                return;
            }

            JobListBox.FilterItems( item => !ShouldHide(item) );

        }
   
        private void jobTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyJobFilter();
        }

        internal void RefreshJobs()
        {
            ApplyJobFilter();
        }
    }
}
