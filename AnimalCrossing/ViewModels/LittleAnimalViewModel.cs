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

        public LittleAnimalViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            using (var con = SQLiteService.GetDbConnection())
            {
                var littleAnimals = con.Table<LittleAnimal>().ToList();
                LittleAnimals = littleAnimals;
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
