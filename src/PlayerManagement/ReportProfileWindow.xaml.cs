using NLog;
using System.Windows;
using System.Windows.Controls;
using Tailgrab.Common;
using VRChat.API.Model;
using static Tailgrab.Clients.VRChat.VRChatClient;

namespace Tailgrab.PlayerManagement
{
    public partial class ReportProfileWindow : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ServiceRegistry _serviceRegistry;

        public List<ReportReasonItem> ReportReasons { get; } = new List<ReportReasonItem>
        {
            new ReportReasonItem("Sexual Content", "sexual"),
            new ReportReasonItem("Hateful Content", "hateful"),
            new ReportReasonItem("Gore and Violence", "gore"),
            new ReportReasonItem("Child Exploitation", "child"),
            new ReportReasonItem("Other", "other")
        };

        public ReportProfileWindow(ServiceRegistry serviceRegistry)
        {
            _serviceRegistry = serviceRegistry;
            InitializeComponent();
            DataContext = this;
        }

        public ReportProfileWindow(ServiceRegistry serviceRegistry, string userId)
        {
            _serviceRegistry = serviceRegistry;
            InitializeComponent();
            DataContext = this;
            UserIdTextBox.Text = userId.Trim();

            if (string.IsNullOrEmpty(userId))
            {
                ReportDescriptionTextBox.Text = string.Empty;
                return;
            }

            try
            {
                // Get the player from PlayerManager
                Player? player = _serviceRegistry.GetPlayerManager().GetPlayerByUserId(userId);

                if (player != null && !string.IsNullOrEmpty(player.AIEval))
                {
                    ReportDescriptionTextBox.Text = player.AIEval;
                    logger.Debug($"Loaded AI evaluation for user: {userId}");
                }
                else
                {
                    ReportDescriptionTextBox.Text = "No AI evaluation available for this user.";
                    logger.Debug($"No AI evaluation found for user: {userId}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error loading AI evaluation for user: {userId}");
                ReportDescriptionTextBox.Text = $"Error loading AI evaluation: {ex.Message}";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = UserIdTextBox.Text.Trim();
            string category = CategoryTextBox.Text;
            string reportReason = ReportReasonComboBox.SelectedValue?.ToString() ?? string.Empty;
            string reportDescription = ReportDescriptionTextBox.Text;

            if (string.IsNullOrEmpty(userId))
            {
                System.Windows.MessageBox.Show("Please enter a User ID.", "Validation Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Call the method that will handle the future web service call
            bool success = await SubmitReport(userId, category, reportReason, reportDescription);

            // Show success message
            if (success)
            {
                System.Windows.MessageBox.Show("Your report has been submitted", "Success",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Failed to submit report. Please try again later.", "Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private async Task<bool> SubmitReport(string userId, string category, string reportReason, string reportDescription)
        {
            ModerationReportPayload rpt = new ModerationReportPayload();
            rpt.Type = "user";
            rpt.Category = "profile";
            rpt.Reason = reportReason;
            rpt.ContentId = userId;
            rpt.Description = reportDescription;

            ModerationReportDetails rptDtls = new ModerationReportDetails();
            rptDtls.InstanceType = "Group Public";
            rptDtls.InstanceAgeGated = false;
            rptDtls.UserInSameInstance = true;
            rpt.Details = new List<ModerationReportDetails>() { rptDtls };

            bool success = await _serviceRegistry.GetVRChatAPIClient().SubmitModerationReportAsync(rpt);
            if (success)
            {
                logger.Info($"Report submitted - UserId: {userId}, " +
                           $"Category: {category}, ReportReason: {reportReason}, Description: {reportDescription}");
            }
            else
            {
                logger.Warn($"Failed to submit report - UserId: {userId}, " +
                           $"Category: {category}, ReportReason: {reportReason}, Description: {reportDescription}");
            }
            return success;
        }
    }
}
