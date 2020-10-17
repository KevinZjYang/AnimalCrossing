using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.UI.Interop;

namespace AnimalCrossing.Views
{
    public sealed partial class FishPage : Page
    {
        private FishViewModel ViewModel
        {
            get { return ViewModelLocator.Current.FishViewModel; }
        }

        public FishPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataAsync();
        }
    }
}
