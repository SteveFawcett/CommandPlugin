using BroadcastPluginSDK.Classes;
using BroadcastPluginSDK.Interfaces;
using CommandPlugin.Classes;
using CommandPlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace CommandPlugin.Forms 
{
    public partial class InfoPage : UserControl, IInfoPage
    {
        private readonly JobCardRenderer _renderer = new();
        private readonly ILogger<IPlugin> _logger;
        private readonly IConfiguration _configuration;
        private readonly JobProcessor _processor;
        private List<CommandItem> allJobs = new();

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

        public InfoPage(IConfiguration configuration, ILogger<IPlugin> logger, JobProcessor processor)
        {
            _logger = logger;
            _configuration = configuration;
            _processor = processor;

            InitializeComponent();

            Icon = Resources.red;
            jobTypes.SelectedIndex = 1;
        }

        public Control GetControl()
        {
            return this;
        }

        internal void JobCard(CommandItem job)
        {
            foreach (var ljob in allJobs)
            {
                if (ljob.Id == job.Id)
                {
                    ljob.Status = job.Status;
                    ApplyJobFilter();
                    return;
                }
            }

            allJobs.Add(job);
            ApplyJobFilter();
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

            JobListBox.BeginUpdate();
            JobListBox.Items.Clear();

            foreach (var item in allJobs) // allJobs is your full source list
            {
                if (!ShouldHide(item))
                    JobListBox.Items.Add(item);
            }

            JobListBox.EndUpdate();
        }

        private void JobListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= JobListBox.Items.Count) return;

            var item = (CommandItem)JobListBox.Items[e.Index];
            if (item != null)
            {
                _renderer.Draw(e.Graphics, e.Bounds, item, (e.State & DrawItemState.Selected) != 0);
            }

            e.DrawFocusRectangle();
        }

        private void jobTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyJobFilter();
        }

        private void display_results(object sender, EventArgs e)
        {
            if (JobListBox.SelectedItem is CommandItem item)
            {
                StderrTextBox.Text = item.Errors;
                StdioTextBox.Text = item.Result;
            }
            else
            {
                StdioTextBox.Text = string.Empty;
                StderrTextBox.Text = string.Empty;
            }
        }
    }
}
