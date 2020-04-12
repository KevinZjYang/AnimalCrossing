using System;
using System.Threading.Tasks;
using System.Windows.Input;

using AnimalCrossing.Helpers;
using AnimalCrossing.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace AnimalCrossing.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : ViewModelBase
    {
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

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        private bool _isNorthCheck;

        public bool IsNorthCheck
        {
            get { return _isNorthCheck; }
            set { Set(ref _isNorthCheck, value); }
        }

        private bool _isSouthCheck;

        public bool IsSouthCheck
        {
            get { return _isSouthCheck; }
            set { Set(ref _isSouthCheck, value); }
        }

        private ICommand _southChecked;

        public ICommand SouthCheckedCommand
        {
            get
            {
                if (_southChecked == null)
                {
                    _southChecked = new RelayCommand<RoutedEventArgs>(SouthChecked);
                }
                return _southChecked;
            }
        }

        private void SouthChecked(RoutedEventArgs obj)
        {
            SettingsHelper.SaveLocalSetting(SettingsKey.DefaultHemisphereKey, "South");
        }

        private ICommand _northChecked;

        public ICommand NorthCheckedCommand
        {
            get
            {
                if (_northChecked == null)
                {
                    _northChecked = new RelayCommand<RoutedEventArgs>(NorthChecked);
                }
                return _northChecked;
            }
        }

        private void NorthChecked(RoutedEventArgs obj)
        {
            SettingsHelper.SaveLocalSetting(SettingsKey.DefaultHemisphereKey, "North");
        }

        public SettingsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            SetDefaultHemisphere();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private void SetDefaultHemisphere()
        {
            var hemisphere = Helpers.SettingsHelper.GetLocalSetting(SettingsKey.DefaultHemisphereKey);
            if (hemisphere != null)
            {
                if (hemisphere == "North")
                {
                    IsNorthCheck = true;
                    IsSouthCheck = false;
                }
                else
                {
                    IsNorthCheck = false;
                    IsSouthCheck = true;
                }
            }
            else
            {
                IsNorthCheck = true;
                IsSouthCheck = false;
            }
        }
    }
}
