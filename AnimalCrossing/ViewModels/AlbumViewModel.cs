using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using AnimalCrossing.Models;
using AnimalCrossing.Services;

namespace AnimalCrossing.ViewModels
{
    public class AlbumViewModel : ViewModelBase
    {
        private List<Album> _album;

        public List<Album> Albums
        {
            get
            {
                if (_album == null)
                {
                    _album = new List<Album>();
                }
                return _album;
            }
            set { Set(ref _album, value); }
        }

        private ICommand _itemClick;

        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClick == null)
                {
                    _itemClick = new RelayCommand<ItemClickEventArgs>(GridViewItemClick);
                }
                return _itemClick;
            }
        }

        public AlbumViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            using (var con = SQLiteService.GetDbConnection())
            {
                var albums = con.Table<Album>().ToList();
                Albums = albums;
            }
        }

        private void GridViewItemClick(ItemClickEventArgs obj)
        {
            //var animal = obj.ClickedItem as NormalAnimal;
            //if (animal == null) return;
            //UserControls.DetailControl detail = new UserControls.DetailControl(animal, _whichAnimal);
            //detail.Show();
        }
    }
}
