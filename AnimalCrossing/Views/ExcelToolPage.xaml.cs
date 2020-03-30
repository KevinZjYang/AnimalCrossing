using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnimalCrossing.ViewModels;
using NPOI.SS.UserModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using AnimalCrossing.Models;

namespace AnimalCrossing.Views
{
    public sealed partial class ExcelToolPage : Page
    {
        private ExcelToolViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ExcelToolViewModel; }
        }
        public ObservableCollection<string> Cells = new ObservableCollection<string>();

        private IList<ICell> Ret;
        public ExcelToolPage()
        {
            InitializeComponent();
        }

        private async void InputButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //var fileOpenPicker = new FileOpenPicker();
            //fileOpenPicker.FileTypeFilter.Add(".xls");
            //fileOpenPicker.FileTypeFilter.Add(".xlsx");
            //var file = await fileOpenPicker.PickSingleFileAsync();
            //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/W020190617630075964590.xls"));
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/data.xlsx"));

            var column = new List<int>();
            for (int i = 1; i < 33; i++)
            {
                column.Add(i);
            }
            Ret = await Services.ExcelService.ReadAllCellsAsync(file.Path, SheetName.Text, 2, column);

            Cells.Clear();
            foreach (var item in Ret)
            {
                Cells.Add(Services.ExcelService.GetCellValue(item));
            }
        }

        private void InsectButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            for (int i = 0; i < Ret.Count; i++)
            {
                if (!string.IsNullOrEmpty(Services.ExcelService.GetCellValue(Ret[i])))
                {
                    var name = Services.ExcelService.GetCellValue(Ret[i]);
                    var number = Services.ExcelService.GetCellValue(Ret[i + 1]);
                    var eng = Services.ExcelService.GetCellValue(Ret[i + 2]);
                    var jap = Services.ExcelService.GetCellValue(Ret[i + 3]);
                    var price = Services.ExcelService.GetCellValue(Ret[i + 4]);
                    var position = Services.ExcelService.GetCellValue(Ret[i + 5]);
                    var weather = Services.ExcelService.GetCellValue(Ret[i + 6]);
                    var time = Services.ExcelService.GetCellValue(Ret[i + 7]);
                    //北半球
                    var Nappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = Services.ExcelService.GetCellValue(Ret[i + 7+j]);
                        Nappear.Add(value);
                    }
                    var Nmonth = new Month { AppearMonth = Nappear };
                    var north = new North { Month = Nmonth };

                    //南半球
                    var Sappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = Services.ExcelService.GetCellValue(Ret[i + 19 + j]);
                        Sappear.Add(value);
                    }
                    var Smonth = new Month { AppearMonth = Sappear };
                    var south = new South { Month = Smonth };

                    //汇总
                    var insect = new Insect { Name = name, Number = number, English = eng, Japanese = jap, Price = price, Position = position, Weather = weather, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(insect);
                    Services.SQLiteService.AddInsectData(name,json);
                
                    i = i + 31;
                }
                else
                {
                    i = i + 31;
                }
            }
        }

        private void FishButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            for (int i = 0; i < Ret.Count; i++)
            {
                if (!string.IsNullOrEmpty(Services.ExcelService.GetCellValue(Ret[i])))
                {
                    var name = Services.ExcelService.GetCellValue(Ret[i]);
                    var number = Services.ExcelService.GetCellValue(Ret[i + 1]);
                    var eng = Services.ExcelService.GetCellValue(Ret[i + 2]);
                    var jap = Services.ExcelService.GetCellValue(Ret[i + 3]);
                    var price = Services.ExcelService.GetCellValue(Ret[i + 4]);
                    var position = Services.ExcelService.GetCellValue(Ret[i + 5]);
                    var shape = Services.ExcelService.GetCellValue(Ret[i + 6]);
                    var time = Services.ExcelService.GetCellValue(Ret[i + 7]);
                    //北半球
                    var Nappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = Services.ExcelService.GetCellValue(Ret[i + 7 + j]);
                        Nappear.Add(value);
                    }
                    var Nmonth = new Month { AppearMonth = Nappear };
                    var north = new North { Month = Nmonth };

                    //南半球
                    var Sappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = Services.ExcelService.GetCellValue(Ret[i + 19 + j]);
                        Sappear.Add(value);
                    }
                    var Smonth = new Month { AppearMonth = Sappear };
                    var south = new South { Month = Smonth };

                    //汇总
                    var fish = new Fish { Name = name, Number = number, English = eng, Japanese = jap, Price = price, Position = position,Shape=shape, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(fish);
                    Services.SQLiteService.AddFishData(name, json);

                    i = i + 31;
                }
                else
                {
                    i = i + 31;
                }
            }
        }
    }
}
