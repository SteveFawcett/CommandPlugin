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
        private readonly JobCardRenderer _renderer = new();
        private readonly ILogger<Command> _logger;
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

        public InfoPage(IConfiguration configuration, ILogger<Command> logger, JobProcessor processor)
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

            JobListBox.Refresh();
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

        internal void RefreshJobs()
        {
            ApplyJobFilter();
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            int index = JobListBox.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                if (JobListBox.Items[index] is CommandItem item)
                {
                    foreach (var badge in item.clickableBadges)
                    {
                        if (badge.bounds.Contains(e.Location))
                        {
                            HandleBadgeClick(badge.label, item);
                            break;
                        }
                    }
                }
            }
        }

        private void HandleBadgeClick(string label , CommandItem item )
        {
            Results f = new Results();
            _logger.LogInformation("Badge {Label} clicked for Job {JobId}", label, item.Id);

            if ( label == "Errors" && !string.IsNullOrEmpty(item.Errors))
            {
                f.SetText = item.Errors;
                f.Text = $"Errors for Job {item.Id}";
                f.ShowDialog();
            }
            else if(label == "Results" && !string.IsNullOrEmpty(item.Result))
            {
                f.SetText = item.Result;
                f.Text = $"Result for Job {item.Id}";
                f.ShowDialog();
            }
        }
    }
}
