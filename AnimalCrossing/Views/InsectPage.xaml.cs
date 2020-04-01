using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AnimalCrossing.Views
{
    public sealed partial class InsectPage : Page
    {
        private InsectViewModel ViewModel
        {
            get { return ViewModelLocator.Current.InsectViewModel; }
        }

        public InsectPage()
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
