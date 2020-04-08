using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NPOI.SS.UserModel;
using System.Threading.Tasks;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AnimalCrossingDbTool
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<string> Cells = new ObservableCollection<string>();

        private IList<ICell> Ret;
        public MainPage()
        {
            this.InitializeComponent();
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
            Ret = await ExcelService.ReadAllCellsAsync(file.Path, SheetName.Text, 2, column);

            Cells.Clear();
            foreach (var item in Ret)
            {
                Cells.Add(ExcelService.GetCellValue(item));
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
                var name = ExcelService.GetCellValue(Ret[i]);
                var number = ExcelService.GetCellValue(Ret[i + 1]);
                var eng = ExcelService.GetCellValue(Ret[i + 2]);
                var jap = ExcelService.GetCellValue(Ret[i + 3]);
                var price = ExcelService.GetCellValue(Ret[i + 4]);
                var position = ExcelService.GetCellValue(Ret[i + 5]);
                var weather = ExcelService.GetCellValue(Ret[i + 6]);
                var time = ExcelService.GetCellValue(Ret[i + 7]);
                //北半球
                var Nappear = new List<string>();
                for (int j = 1; j < 13; j++)
                {
                    var value = ExcelService.GetCellValue(Ret[i + 7 + j]);
                    Nappear.Add(value);
                }
                var Nmonth = new Month { AppearMonth = Nappear };
                var north = new North { Month = Nmonth };

                //南半球
                var Sappear = new List<string>();
                for (int j = 1; j < 13; j++)
                {
                    var value = ExcelService.GetCellValue(Ret[i + 19 + j]);
                    Sappear.Add(value);
                }
                var Smonth = new Month { AppearMonth = Sappear };
                var south = new South { Month = Smonth };

                //汇总
                var insect = new Insect { Name = name, Number = Convert.ToInt32(number), English = eng, Japanese = jap, Price = Convert.ToInt32(price), Position = position, Weather = weather, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(insect);
                SQLiteService.AddInsectData(name, json);
                i = i + 31;
            }
            Progress = "写入数据完成";
        }

        private async void FishButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SheetName.Text != "fish")
            {
                Progress = "Sheet Name Wrong!";
                return;
            }
            await ReadFile();
            Progress = "开始写入数据库...";
            for (int i = 0; i < Ret.Count; i++)
            {
                if (!string.IsNullOrEmpty(ExcelService.GetCellValue(Ret[i])))
                {
                    var name = ExcelService.GetCellValue(Ret[i]);
                    var number = ExcelService.GetCellValue(Ret[i + 1]);
                    var eng = ExcelService.GetCellValue(Ret[i + 2]);
                    var jap = ExcelService.GetCellValue(Ret[i + 3]);
                    var price = ExcelService.GetCellValue(Ret[i + 4]);
                    var position = ExcelService.GetCellValue(Ret[i + 5]);
                    var shape = ExcelService.GetCellValue(Ret[i + 6]);
                    var time = ExcelService.GetCellValue(Ret[i + 7]);
                    //北半球
                    var Nappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = ExcelService.GetCellValue(Ret[i + 7 + j]);
                        Nappear.Add(value);
                    }
                    var Nmonth = new Month { AppearMonth = Nappear };
                    var north = new North { Month = Nmonth };

                    //南半球
                    var Sappear = new List<string>();
                    for (int j = 1; j < 13; j++)
                    {
                        var value = ExcelService.GetCellValue(Ret[i + 19 + j]);
                        Sappear.Add(value);
                    }
                    var Smonth = new Month { AppearMonth = Sappear };
                    var south = new South { Month = Smonth };

                    //汇总
                    var fish = new Fish { Name = name, Number = Convert.ToInt32(number), English = eng, Japanese = jap, Price = Convert.ToInt32(price), Position = position, Shape = shape, Time = time, Hemisphere = new Hemisphere { North = north, South = south } };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(fish);
                    SQLiteService.AddFishData(name, json);

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
            DependencyProperty.Register("Progress", typeof(string), typeof(MainPage), new PropertyMetadata(null));
    }
}
