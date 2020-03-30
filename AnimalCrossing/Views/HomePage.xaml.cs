using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AnimalCrossing.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel
        {
            get { return ViewModelLocator.Current.HomeViewModel; }
        }

        public HomePage()
        {
            InitializeComponent();
        }
    }
}
