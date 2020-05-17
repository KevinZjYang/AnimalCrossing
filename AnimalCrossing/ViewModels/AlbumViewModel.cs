using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace AnimalCrossing.ViewModels
{
    public class AlbumViewModel : ViewModelBase
    {
        private List<NormalAlbum> _album;

        public List<NormalAlbum> Albums
        {
            get
            {
                if (_album == null)
                {
                    _album = new List<NormalAlbum>();
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

        private ListViewSelectionMode _listSelectionMode = ListViewSelectionMode.None;

        public ListViewSelectionMode ListSelectionMode
        {
            get { return _listSelectionMode; }
            set { Set(ref _listSelectionMode, value); }
        }

        private bool _isEditMode = false;

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { Set(ref _isEditMode, value); }
        }

        public ICommand OnEditMode
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ListSelectionMode = ListSelectionMode == ListViewSelectionMode.None ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
                    IsEditMode = ListSelectionMode == ListViewSelectionMode.None ? false : true;
                });
            }
        }

        public ICommand OnAddItemCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //IsLoading = true;
                    var count = _adaptiveGridView.SelectedItems.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        var item = _adaptiveGridView.SelectedItems[i] as NormalAlbum;
                        SQLiteService.AddUserAlbum(new UserAlbum { Name = item.Name, Owned = true });
                    }
                    Albums = CommonDataService.GetAllAlbums();
                    IsEditMode = false;
                    ListSelectionMode = ListViewSelectionMode.None;
                    //IsLoading = false;
                });
            }
        }

        public ICommand OnUpdateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //IsLoading = true;
                    var count = _adaptiveGridView.SelectedItems.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        var item = _adaptiveGridView.SelectedItems[i] as NormalAlbum;
                        SQLiteService.AddUserAlbum(new UserAlbum { Name = item.Name, Owned = false });
                    }
                    Albums = CommonDataService.GetAllAlbums();
                    IsEditMode = false;
                    ListSelectionMode = ListViewSelectionMode.None;
                    //IsLoading = false;
                });
            }
        }

        private bool _isOwnedOn;

        public bool IsOwnedOn
        {
            get { return _isOwnedOn; }
            set { Set(ref _isOwnedOn, value); }
        }

        public ICommand HideOwend
        {
            get
            {
                return new RelayCommand<RoutedEventArgs>((args) =>
                {
                    var toggleSwitch = args.OriginalSource as ToggleSwitch;
                    IsOwnedOn = toggleSwitch.IsOn;
                    if (IsOwnedOn)
                    {
                        Albums = Albums.FindAll(delegate (NormalAlbum album)
                        {
                            if (album.Owned)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        });
                    }
                    else
                    {
                        Albums = Services.CommonDataService.GetAllAlbums();
                    }
                });
            }
        }

        //private bool _isLoading;

        //public bool IsLoading
        //{
        //    get { return _isLoading; }
        //    set { Set(ref _isLoading, value); }
        //}

        public AlbumViewModel()
        {
        }

        private AdaptiveGridView _adaptiveGridView;

        public void Initialize(AdaptiveGridView gridView)
        {
            _adaptiveGridView = gridView;
            LoadData();
        }

        public void LoadData()
        {
            //IsLoading = true;
            Albums = Services.CommonDataService.GetAllAlbums();
            //IsLoading = false;
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
