using AnimalCrossing.Models;
using AnimalCrossing.Services;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace AnimalCrossing.UserControls
{
    public sealed partial class AlbumControl : UserControl
    {
        public Models.NormalAlbum NormalAlbum
        {
            get { return (Models.NormalAlbum)GetValue(NormalAlbumProperty); }
            set { SetValue(NormalAlbumProperty, value); }
        }

        public static readonly DependencyProperty NormalAlbumProperty =
            DependencyProperty.Register("NormalAlbum", typeof(Models.NormalAlbum), typeof(AlbumControl), new PropertyMetadata(null));

        public bool Owend
        {
            get { return (bool)GetValue(OwendProperty); }
            set { SetValue(OwendProperty, value); }
        }

        public static readonly DependencyProperty OwendProperty =
            DependencyProperty.Register("Owend", typeof(bool), typeof(AlbumControl), new PropertyMetadata(null));

        public AlbumControl()
        {
            this.InitializeComponent();
            rootGrid.PointerEntered += RootGrid_PointerEntered;
            rootGrid.PointerExited += RootGrid_PointerExited;
            Unloaded += AlbumControl_Unloaded;
        }

        private void AlbumControl_Unloaded(object sender, RoutedEventArgs e)
        {
            rootGrid.PointerEntered -= RootGrid_PointerEntered;
            rootGrid.PointerExited -= RootGrid_PointerExited;
        }

        private void RootGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
        }

        private void RootGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Owend)
            {
                AddButton.Visibility = Visibility.Collapsed;
                RemoveButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddButton.Visibility = Visibility.Visible;
                RemoveButton.Visibility = Visibility.Collapsed;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteService.AddUserAlbum(new UserAlbum { Name = NormalAlbum.Name, Owned = false });
            Owend = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteService.AddUserAlbum(new UserAlbum { Name = NormalAlbum.Name, Owned = true });
            Owend = true;
        }
    }
}
