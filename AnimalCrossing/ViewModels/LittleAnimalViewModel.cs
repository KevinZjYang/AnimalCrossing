using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using AnimalCrossing.UserControls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace AnimalCrossing.ViewModels
{
    public class LittleAnimalViewModel : ViewModelBase
    {
        private List<LittleAnimal> _littleAnimal;

        public List<LittleAnimal> LittleAnimals
        {
            get
            {
                if (_littleAnimal == null)
                {
                    _littleAnimal = new List<LittleAnimal>();
                }
                return _littleAnimal;
            }
            set { Set(ref _littleAnimal, value); }
        }

        private LittleAnimal _storedItem;
        private ICommand _itemClick;

        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClick == null)
                {
                    _itemClick = new RelayCommand<ItemClickEventArgs>((p) =>
                    {
                        var para = p.ClickedItem as LittleAnimal;
                        ConnectedAnimation animation = null;
                        var container = _adaptiveGridView.ContainerFromItem(p.ClickedItem) as GridViewItem;
                        if (container != null)
                        {
                            _storedItem = container.Content as LittleAnimal;
                            LittleAnimalDeatil = _storedItem;
                            animation = _adaptiveGridView.PrepareConnectedAnimation("forwardAnimation", _storedItem, "connectedElement");
                            animation.Configuration = new GravityConnectedAnimationConfiguration();
                        }
                        DetailVisibility = Visibility.Visible;
                        animation.TryStart(_detailImageEx);
                    });
                }
                return _itemClick;
            }
        }

        private Visibility _detailVisibility = Visibility.Collapsed;

        public Visibility DetailVisibility
        {
            get { return _detailVisibility; }
            set { Set(ref _detailVisibility, value); }
        }

        public ICommand CloseDetail
        {
            get
            {
                return new RelayCommand<Windows.UI.Xaml.Input.TappedRoutedEventArgs>(async (args) =>
                {
                    Grid grid = args.OriginalSource as Grid;
                    if (args.OriginalSource == grid)
                    {
                        //_isDetailShow = false;

                        if (_storedItem == null)
                        {
                            DetailVisibility = Visibility.Collapsed;
                            return;
                        }
                        ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", _detailImageEx);

                        // Collapse the smoke when the animation completes.
                        animation.Completed += Animation_Completed;

                        // If the connected item appears outside the viewport, scroll it into view.
                        //ImageGridView.ScrollIntoView(_storedItem, ScrollIntoViewAlignment.Default);
                        //ImageGridView.UpdateLayout();

                        // Use the Direct configuration to go back (if the API is available).
                        //if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        //{
                        //    animation.Configuration = new DirectConnectedAnimationConfiguration();
                        //}
                        animation.Configuration = new BasicConnectedAnimationConfiguration();
                        // Play the second connected animation.
                        await _adaptiveGridView.TryStartConnectedAnimationAsync(animation, _storedItem, "connectedElement");
                        _storedItem = null;
                        //DetailImageUrl = null;
                    }
                });
            }
        }

        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            DetailVisibility = Visibility.Collapsed;
        }

        public ICommand ItemSelectionChanged
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>((args) =>
                {
                    var items = args.AddedItems;
                    LittleAnimalDeatil = items[0] as LittleAnimal;
                    _storedItem = LittleAnimalDeatil;
                });
            }
        }

        private LittleAnimal _littleAnimalDetail;

        public LittleAnimal LittleAnimalDeatil
        {
            get { return _littleAnimalDetail; }
            set { Set(ref _littleAnimalDetail, value); }
        }

        public LittleAnimalViewModel()
        {
        }

        private AdaptiveGridView _adaptiveGridView;
        private HN.Controls.ImageEx _detailImageEx;

        public void Initialize(AdaptiveGridView gridView, HN.Controls.ImageEx detailImageEx)
        {
            _adaptiveGridView = gridView;
            _detailImageEx = detailImageEx;
        }

        public void LoadData()
        {
            using (var con = SQLiteService.GetDbConnection())
            {
                var littleAnimals = con.Table<LittleAnimal>().ToList();
                LittleAnimals = littleAnimals;
                //LittleAnimalDeatil = LittleAnimals[0];
            }
        }
    }
}
