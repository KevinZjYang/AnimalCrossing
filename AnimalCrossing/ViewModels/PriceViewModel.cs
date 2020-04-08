using System;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AnimalCrossing.ViewModels
{
    public class PriceViewModel : ViewModelBase
    {
        // TODO WTS: Set the URI of the page to show by default
        private const string DefaultUrl = "http://api.iuwp.top";

        private Uri _source;

        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (value)
                {
                    IsShowingFailedMessage = false;
                }

                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Set(ref _isLoadingVisibility, value); }
        }

        private bool _isShowingFailedMessage;

        public bool IsShowingFailedMessage
        {
            get
            {
                return _isShowingFailedMessage;
            }

            set
            {
                if (value)
                {
                    IsLoading = false;
                }

                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Set(ref _failedMesageVisibility, value); }
        }

        private ICommand _navCompleted;

        public ICommand NavCompletedCommand
        {
            get
            {
                if (_navCompleted == null)
                {
                    _navCompleted = new RelayCommand<WebViewNavigationCompletedEventArgs>(NavCompleted);
                }

                return _navCompleted;
            }
        }

        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            RaisePropertyChanged(nameof(BrowserBackCommand));
            RaisePropertyChanged(nameof(BrowserForwardCommand));

            //await InputSaveData();
        }

        private ICommand _navFailed;

        public ICommand NavFailedCommand
        {
            get
            {
                if (_navFailed == null)
                {
                    _navFailed = new RelayCommand<WebViewNavigationFailedEventArgs>(NavFailed);
                }

                return _navFailed;
            }
        }

        private void NavFailed(WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        private ICommand _retryCommand;

        public ICommand RetryCommand
        {
            get
            {
                if (_retryCommand == null)
                {
                    _retryCommand = new RelayCommand(Retry);
                }

                return _retryCommand;
            }
        }

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Refresh();
        }

        private ICommand _browserBackCommand;

        public ICommand BrowserBackCommand
        {
            get
            {
                if (_browserBackCommand == null)
                {
                    _browserBackCommand = new RelayCommand(() => _webView?.GoBack(), () => _webView?.CanGoBack ?? false);
                }

                return _browserBackCommand;
            }
        }

        private ICommand _browserForwardCommand;

        public ICommand BrowserForwardCommand
        {
            get
            {
                if (_browserForwardCommand == null)
                {
                    _browserForwardCommand = new RelayCommand(() => _webView?.GoForward(), () => _webView?.CanGoForward ?? false);
                }

                return _browserForwardCommand;
            }
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() => _webView?.Refresh());
                }

                return _refreshCommand;
            }
        }

        private ICommand _openInBrowserCommand;

        public ICommand OpenInBrowserCommand
        {
            get
            {
                if (_openInBrowserCommand == null)
                {
                    _openInBrowserCommand = new RelayCommand(async () => await Windows.System.Launcher.LaunchUriAsync(Source));
                }

                return _openInBrowserCommand;
            }
        }

        private WebView _webView;

        public PriceViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
            _webView.Settings.IsJavaScriptEnabled = true;
        }

        //private async void InputSaveData()
        //{
        //await _webView.InvokeScriptAsync("eval", new[]
        //{
        //    $"document.getElementById('inlineInput').value = '99 110/122 133/111 62/54 98/69 64';"
        //});

        //var functionString = string.Format(@"document.getElementsByClassName('logging')[0].click();");
        //await GeekWebView.InvokeScriptAsync("eval", new string[] { functionString });
        //}

        //private async void InvokeScripAsync(string id, string content)
        //{
        //    await _webView.InvokeScriptAsync("eval", new[]
        //   {
        //        $"document.getElementById('{id}').value = '{content}';"
        //    });
        //}
    }
}
