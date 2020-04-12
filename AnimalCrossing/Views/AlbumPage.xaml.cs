using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AnimalCrossing.Views
{
    public sealed partial class AlbumPage : Page
    {
        private AlbumViewModel ViewModel
        {
            get { return ViewModelLocator.Current.AlbumViewModel; }
        }

        public AlbumPage()
        {
            InitializeComponent();
        }
    }
}
