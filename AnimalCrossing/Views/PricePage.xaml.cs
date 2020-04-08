using System;

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
    }
}
