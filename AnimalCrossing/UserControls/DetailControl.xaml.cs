using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AnimalCrossing.Models;
using AnimalCrossing.Services;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace AnimalCrossing.UserControls
{
    public sealed partial class DetailControl : UserControl
    {
        public enum Animal
        {
            Fish,
            Insect
        }

        private Popup Popup;

        private Animal _animal;

        public NormalAnimal NormalAnimals
        {
            get { return (NormalAnimal)GetValue(NormalAnimalsProperty); }
            set { SetValue(NormalAnimalsProperty, value); }
        }

        public static readonly DependencyProperty NormalAnimalsProperty =
            DependencyProperty.Register("NormalAnimals", typeof(NormalAnimal), typeof(DetailControl), new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (NormalAnimal)e.OldValue;
            var newValue = (NormalAnimal)e.NewValue;
            if (oldValue == newValue || newValue == null) return;

            //值改变了，刷新数据
            var target = d as DetailControl;
            target?.ChangeBindData(newValue);
        }

        private void ChangeBindData(NormalAnimal newValue)
        {
            Owned = newValue.Owned;
            Museum = newValue.MuseumHave;
        }

        public bool Owned
        {
            get { return (bool)GetValue(OwnedProperty); }
            set { SetValue(OwnedProperty, value); }
        }

        public static readonly DependencyProperty OwnedProperty =
            DependencyProperty.Register("Owned", typeof(bool), typeof(DetailControl), new PropertyMetadata(null));

        public bool Museum
        {
            get { return (bool)GetValue(MuseumProperty); }
            set { SetValue(MuseumProperty, value); }
        }

        public static readonly DependencyProperty MuseumProperty =
            DependencyProperty.Register("Museum", typeof(bool), typeof(DetailControl), new PropertyMetadata(null));

        public DetailControl()
        {
            this.InitializeComponent();
            Popup = new Popup
            {
                Child = this
            };
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            this.Loaded += DetailControl_Loaded;
            this.Unloaded += DetailControl_Unloaded;
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
        }

        public DetailControl(NormalAnimal animals, Animal animal) : this()
        {
            NormalAnimals = animals;
            _animal = animal;
        }

        private void DetailControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void DetailControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        public void Show()
        {
            this.Popup.IsOpen = true;
        }

        public void Hide()
        {
            this.Popup.IsOpen = false;
        }

        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            if (e.OriginalSource == grid)
            {
                Hide();
            }
        }

        private async void AddOwnedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_animal == Animal.Fish)
            {
                await SQLiteService.AddUserFish(new UserFish { Name = NormalAnimals.Name, Owned = true, MuseumHave = NormalAnimals.MuseumHave });
            }
            else
            {
                await SQLiteService.AddUserInsect(new UserInsect { Name = NormalAnimals.Name, Owned = true, MuseumHave = NormalAnimals.MuseumHave });
            }
            Owned = true;
            NormalAnimals.Owned = true;
        }

        private async void AddMuseumButton_Click(object sender, RoutedEventArgs e)
        {
            if (_animal == Animal.Fish)
            {
                await SQLiteService.AddUserFish(new UserFish { Name = NormalAnimals.Name, Owned = NormalAnimals.Owned, MuseumHave = true });
            }
            else
            {
                await SQLiteService.AddUserInsect(new UserInsect { Name = NormalAnimals.Name, Owned = NormalAnimals.Owned, MuseumHave = true });
            }
            Museum = true;
            NormalAnimals.MuseumHave = true;
        }

        private async void RemoveMuseumButton_Click(object sender, RoutedEventArgs e)
        {
            if (_animal == Animal.Fish)
            {
                await SQLiteService.AddUserFish(new UserFish { Name = NormalAnimals.Name, Owned = NormalAnimals.Owned, MuseumHave = false });
            }
            else
            {
                await SQLiteService.AddUserInsect(new UserInsect { Name = NormalAnimals.Name, Owned = NormalAnimals.Owned, MuseumHave = false });
            }
            Museum = false;
            NormalAnimals.MuseumHave = false;
        }

        private async void RemoveOwnedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_animal == Animal.Fish)
            {
                await SQLiteService.AddUserFish(new UserFish { Name = NormalAnimals.Name, Owned = false, MuseumHave = NormalAnimals.MuseumHave });
            }
            else
            {
                await SQLiteService.AddUserInsect(new UserInsect { Name = NormalAnimals.Name, Owned = false, MuseumHave = NormalAnimals.MuseumHave });
            }
            Owned = false;
            NormalAnimals.Owned = false;
        }
    }
}
