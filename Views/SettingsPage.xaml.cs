using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using HAClient.Helpers;
using HAClient.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HAClient.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        //// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private string _hacAccount;
        public string HACAccount
        {
            get { return _hacAccount; }
            set { Set(ref _hacAccount, (string)value); }
        }

        private string _hacCommandKey;
        public string HACCommandKey
        {
            get { return _hacCommandKey; }
            set { Set(ref _hacCommandKey, (string)value); }
        }

        private string _smtpServer;
        public string SMTPServer
        {
            get { return _smtpServer; }
            set { Set(ref _smtpServer, (string)value); }
        }

        private int _smtpPort;
        public int SMTPPort
        {
            get { return _smtpPort; }
            set { Set(ref _smtpPort, (int)value); }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { Set(ref _displayName, (string)value); }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, (string)value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, (string)value); }
        }

        public SettingsPage()
        {
            Loaded += SettingsPage_Loaded;
            InitializeComponent();
        }

        private async void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            HACAccount = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(HACAccount));
            HACCommandKey = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(HACCommandKey));
            SMTPServer = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(SMTPServer));
            SMTPPort = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<int>(nameof(SMTPPort));
            DisplayName = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(DisplayName));
            Username = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(Username));
            Password = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(Password));
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
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

        private async void HACAccountChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(HACAccount), Settings_HACAccount.Text);
        }

        private async void HACCommandKeyChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(HACCommandKey), Settings_HACCommandKey.Text);
        }

        private async void SMTPServerChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(SMTPServer), Settings_SMTPServer.Text);
        }

        private async void SMTPPortChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(SMTPPort), Settings_SMTPPort.Text);
        }

        private async void DisplayNameChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(DisplayName), Settings_DisplayName.Text);
        }

        private async void UsernameChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(Username), Settings_Username.Text);
        }

        private async void PasswordChanged(object sender, RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(Password), Settings_Password.Password);
        }
    }
}
