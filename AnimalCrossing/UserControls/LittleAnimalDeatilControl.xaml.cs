using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AnimalCrossing.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace AnimalCrossing.UserControls
{
    public sealed partial class LittleAnimalDeatilControl : UserControl
    {
        Popup Popup;
        public LittleAnimal LittleAnimal;
        public LittleAnimalDeatilControl()
        {
            this.InitializeComponent();
            Popup = new Popup
            {
                Child = this
            };
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            this.Loaded += LittleAnimalDeatilControl_Loaded;
            this.Unloaded += LittleAnimalDeatilControl_Unloaded;
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
        }
        public LittleAnimalDeatilControl(LittleAnimal animal) : this()
        {
            LittleAnimal = animal;
        }
        private void LittleAnimalDeatilControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        private void LittleAnimalDeatilControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            if (e.OriginalSource == grid)
            {
                Hide();
            }
        }
        public void Show()
        {
            this.Popup.IsOpen = true;
        }

        public void Hide()
        {
            this.Popup.IsOpen = false;
        }
    }
}
