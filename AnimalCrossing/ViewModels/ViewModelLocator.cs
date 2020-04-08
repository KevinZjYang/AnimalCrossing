using System;

using AnimalCrossing.Services;
using AnimalCrossing.Views;

using GalaSoft.MvvmLight.Ioc;

namespace AnimalCrossing.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<MainViewModel, MainPage>();
            Register<HomeViewModel, HomePage>();
            
            Register<FishViewModel, FishPage>();
            Register<InsectViewModel, InsectPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<PriceViewModel, PricePage>();
        }

        public PriceViewModel PriceViewModel => SimpleIoc.Default.GetInstance<PriceViewModel>();

        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public InsectViewModel InsectViewModel => SimpleIoc.Default.GetInstance<InsectViewModel>();

        public FishViewModel FishViewModel => SimpleIoc.Default.GetInstance<FishViewModel>();
     

        public HomeViewModel HomeViewModel => SimpleIoc.Default.GetInstance<HomeViewModel>();

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
