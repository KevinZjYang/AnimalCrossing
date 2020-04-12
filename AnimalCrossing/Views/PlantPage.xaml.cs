using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AnimalCrossing.Views
{
    public sealed partial class PlantPage : Page
    {
        private PlantViewModel ViewModel
        {
            get { return ViewModelLocator.Current.PlantViewModel; }
        }

        public PlantPage()
        {
            InitializeComponent();
        }
    }
}
