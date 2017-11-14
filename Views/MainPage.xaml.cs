using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using LightBuzz.SMTP;

using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml.Controls;

namespace HAClient.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        public MainPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void btnCommand_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Sending message");
            var hacAccount = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "HACAccount");
            var hacCommandKey = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "HACCommandKey");
            var smtpServer = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "SMTPServer");
            var smtpPort = await Helpers.SettingsStorageExtensions.ReadAsync<int>(Windows.Storage.ApplicationData.Current.LocalSettings, "SMTPPort");
            var displayName = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "DisplayName");
            var username = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "Username");
            var password = await Helpers.SettingsStorageExtensions.ReadAsync<string>(Windows.Storage.ApplicationData.Current.LocalSettings, "Password");

            EasClientDeviceInformation CurrentDeviceInfor = new EasClientDeviceInformation();
            string DeviceName = CurrentDeviceInfor.FriendlyName;

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort, true, username, password))
            {
                Windows.ApplicationModel.Email.EmailMessage emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
                emailMessage.Sender = new Windows.ApplicationModel.Email.EmailRecipient(username, displayName);
                emailMessage.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(hacAccount, hacCommandKey));
                emailMessage.Subject = txtCommand.Text;
                emailMessage.Body = "Sent from: " + DeviceName;

                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
