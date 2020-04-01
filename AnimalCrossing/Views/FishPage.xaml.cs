using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoadData();
        }

        
    }
}
