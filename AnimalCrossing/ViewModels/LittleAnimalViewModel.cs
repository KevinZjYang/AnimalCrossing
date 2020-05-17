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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ICommand BrithdayItemClick
        {
            get
            {
                return new RelayCommand<ItemClickEventArgs>((args) =>
                {
                    var item = args.ClickedItem as LittleAnimal;
                    var targetItem = LittleAnimals.Find(p => p.Name == item.Name);
                    _adaptiveGridView.ScrollIntoView(targetItem);
                    _adaptiveGridView.SelectedItem = targetItem;
                });
            }
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

        public async Task LoadData()
        {
            var con = await SQLiteService.GetDbConnection();
            var littleAnimals = await con.Table<LittleAnimal>().ToListAsync();
            LittleAnimals = littleAnimals;
            //LittleAnimalDeatil = LittleAnimals[0];

            TodayIsWhoBrithday(LittleAnimals);
        }

        private List<LittleAnimal> _todayBirthday;

        public List<LittleAnimal> TodayBrithday
        {
            get { return _todayBirthday; }
            set { Set(ref _todayBirthday, value); }
        }

        private void TodayIsWhoBrithday(List<LittleAnimal> animals)
        {
            var date = $"{ DateTimeOffset.Now.Month}月{ DateTimeOffset.Now.Day}日";
            TodayBrithday = animals.FindAll(p => p.Brithday == date);
        }
    }
}
