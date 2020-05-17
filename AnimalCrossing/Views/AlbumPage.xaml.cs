using System;

using AnimalCrossing.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(MainGridView);
        }
    }
}
