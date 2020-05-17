using System;
using AnimalCrossing.Services;
using AnimalCrossing.Models;

using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Threading.Tasks;

namespace AnimalCrossing.ViewModels
{
    public class PlantViewModel : ViewModelBase
    {
        private List<Plant> _plants;

        public List<Plant> Plants
        {
            get
            {
                if (_plants == null)
                {
                    _plants = new List<Plant>();
                }
                return _plants;
            }
            set { Set(ref _plants, value); }
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

        public PlantViewModel()
        {
            //LoadData();
        }

        public async Task LoadData()
        {
            var con = await SQLiteService.GetDbConnection();
            var plants = await con.Table<Plant>().ToListAsync();
            Plants = plants;
            await con.CloseAsync();
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
