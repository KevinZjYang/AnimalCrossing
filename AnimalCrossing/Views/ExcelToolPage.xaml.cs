using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnimalCrossing.ViewModels;
using NPOI.SS.UserModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using AnimalCrossing.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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

        private async Task ReadFile()
        {
            Progress = "开始读取excel文件...";
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
            await Task.CompletedTask.ConfigureAwait(true);
        }

        private async void InsectButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SheetName.Text != "insect")
            {
                Progress = "Sheet Name Wrong!";
                return;
            }
            await ReadFile();
            Progress = "开始写入数据库...";
            for (int i = 0; i < Ret.Count; i++)
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
                var insect = new Insect { Name = name, Number = Convert.ToInt32(number), English = eng, Japanese = jap, Price = Convert.ToInt32(price), Position = position, Weather = weather, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(insect);
                Services.SQLiteService.AddInsectData(name, json);
                i = i + 31;
            }
            Progress = "写入数据完成";
        }

        private async void FishButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(SheetName.Text != "fish")
            {
                Progress = "Sheet Name Wrong!";
                return;
            }
            await ReadFile();
            Progress = "开始写入数据库...";
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
                    var fish = new Fish { Name = name, Number = Convert.ToInt32(number), English = eng, Japanese = jap, Price = Convert.ToInt32(price), Position = position,Shape=shape, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(fish);
                    Services.SQLiteService.AddFishData(name, json);
     
                    i = i + 31;
                }
                else
                {
                    i = i + 31;
                }
            }
            Progress = "写入数据完成";
        }



        public string Progress
        {
            get { return (string)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Progress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(string), typeof(ExcelToolPage), new PropertyMetadata(null));

            
    }
}
