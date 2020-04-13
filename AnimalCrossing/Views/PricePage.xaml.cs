using System;
using System.Diagnostics;
using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AnimalCrossing.Views
{
    public sealed partial class PricePage : Page
    {
        private PriceViewModel ViewModel
        {
            get { return ViewModelLocator.Current.PriceViewModel; }
        }

        public PricePage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            try
            {
                var functionString = string.Format("document.getElementById('inlineInput').value;");
                var content = await webView.InvokeScriptAsync("eval", new string[] { functionString });
                if (!Helpers.StringHelper.IsNullOrEmptyOrWhiteSpace(content))
                {
                    Helpers.SettingsHelper.SaveRoamingSetting(SettingsKey.TurnipPriceKey, content);
                }
            }
            catch (Exception ex)
            {

                Debug.Write(ex.Message);
            }
          
        }
    }
}
