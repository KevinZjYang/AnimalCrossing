﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalCrossing.Models;

namespace AnimalCrossing.Services
{
    public static class CommonDataService
    {
        public enum Hemisphere
        {
            South,
            North
        }

        internal static List<NormalAlbum> GetAllAlbums()
        {
            List<NormalAlbum> normalAlbums = new List<NormalAlbum>();
            var con = SQLiteService.GetDbConnection();
            var albums = con.Table<Album>().ToList();

            var usercon = SQLiteService.GetUserDbConnection();
            foreach (var item in albums)
            {
                List<UserAlbum> userAlbums;
                userAlbums = usercon.Table<UserAlbum>().Where(p => p.Name == item.Name).ToList();
                if (userAlbums.Count > 0)
                {
                    var normal = new NormalAlbum { Name = item.Name, BuyPrice = item.BuyPrice, Cover = item.Cover, ForeignName = item.ForeignName, Number = item.Number, SalePrice = item.SalePrice, Source = item.Source, Owned = userAlbums[0].Owned };
                    normalAlbums.Add(normal);
                }
                else
                {
                    var normal = new NormalAlbum { Name = item.Name, BuyPrice = item.BuyPrice, Cover = item.Cover, ForeignName = item.ForeignName, Number = item.Number, SalePrice = item.SalePrice, Source = item.Source, Owned = false };
                    normalAlbums.Add(normal);
                }
            }
            con.Dispose();
            usercon.Dispose();
            return normalAlbums;
        }

        /// <summary>
        /// 获取所有的鱼
        /// </summary>
        /// <param name="bookCount">
        /// 图鉴开启数量
        /// </param>
        /// <param name="museumCount">
        /// 博物馆收藏数量
        /// </param>
        /// <param name="hemisphere">
        /// 选择南北半球
        /// </param>
        /// <returns>
        /// </returns>
        internal static List<NormalAnimal> GetAllFishes(out int bookCount, out int museumCount, Hemisphere hemisphere)
        {
            bookCount = 0; museumCount = 0;
            List<NormalAnimal> normalAnimals = new List<NormalAnimal>();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsFish>().OrderBy(p => p.Number).ToList();

                foreach (var item in animals)
                {
                    List<UserFish> userFish;
                    using (var usercon = SQLiteService.GetUserDbConnection())
                    {
                        userFish = usercon.Table<UserFish>().Where(p => p.Name == item.Name).ToList();
                    }
                    if (userFish.Count > 0)
                    {
                        var owned = userFish[0].Owned;
                        var museumHave = userFish[0].MuseumHave;
                        if (owned) bookCount += 1;
                        if (museumHave) museumCount += 1;

                        var normal = SelectHemisphereAndConstructFish(hemisphere, item, owned, museumHave);
                        normalAnimals.Add(normal);
                    }
                    else
                    {
                        var normal = SelectHemisphereAndConstructFish(hemisphere, item, false, false);
                        normalAnimals.Add(normal);
                    }
                }
            }

            return normalAnimals;
        }

        /// <summary>
        /// 获取本月的鱼
        /// </summary>
        /// <param name="bookCount">
        /// </param>
        /// <param name="museumCount">
        /// </param>
        /// <param name="hemisphere">
        /// </param>
        /// <returns>
        /// </returns>
        internal static List<NormalAnimal> GetThisMonthFishes(out int bookCount, out int museumCount, Hemisphere hemisphere)
        {
            bookCount = 0; museumCount = 0;
            List<NormalAnimal> normalAnimals = new List<NormalAnimal>();
            var animals = GetAllFishes(out int book, out int museum, hemisphere);
            foreach (var item in animals)
            {
                var thisMonth = DateTime.Now.Month;
                if (item.Owned) bookCount += 1;
                if (item.MuseumHave) museumCount += 1;
                if (item.AppearMonth[thisMonth - 1])
                {
                    normalAnimals.Add(item);
                }
            }

            return normalAnimals;
        }

        /// <summary>
        /// 获取所有的昆虫
        /// </summary>
        /// <param name="bookCount">
        /// 图鉴开启数量
        /// </param>
        /// <param name="museumCount">
        /// 博物馆收藏数量
        /// </param>
        /// <param name="hemisphere">
        /// 选择南北半球
        /// </param>
        /// <returns>
        /// </returns>
        internal static List<NormalAnimal> GetAllInsects(out int bookCount, out int museumCount, Hemisphere hemisphere)
        {
            bookCount = 0; museumCount = 0;
            List<NormalAnimal> normalAnimals = new List<NormalAnimal>();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsInsect>().OrderBy(p => p.Number).ToList();
                foreach (var item in animals)
                {
                    //var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);

                    List<UserInsect> userInsect;
                    using (var usercon = SQLiteService.GetUserDbConnection())
                    {
                        userInsect = usercon.Table<UserInsect>().Where(p => p.Name == item.Name).ToList();
                    }
                    if (userInsect.Count > 0)
                    {
                        var owned = userInsect[0].Owned;
                        var museumHave = userInsect[0].MuseumHave;

                        if (owned) bookCount += 1;
                        if (museumHave) museumCount += 1;

                        var normal = SelectHemisphereAndConstructInsect(hemisphere, item, owned, museumHave);

                        normalAnimals.Add(normal);
                    }
                    else
                    {
                        var normal = SelectHemisphereAndConstructInsect(hemisphere, item, false, false);

                        normalAnimals.Add(normal);
                    }
                }

                return normalAnimals;
            }
        }

        /// <summary>
        /// 获取本月的昆虫
        /// </summary>
        /// <param name="bookCount">
        /// </param>
        /// <param name="museumCount">
        /// </param>
        /// <param name="hemisphere">
        /// </param>
        /// <returns>
        /// </returns>
        internal static List<NormalAnimal> GetThisMonthInsects(out int bookCount, out int museumCount, Hemisphere hemisphere)
        {
            bookCount = 0; museumCount = 0;
            List<NormalAnimal> normalAnimals = new List<NormalAnimal>();
            var animals = GetAllInsects(out int book, out int museum, hemisphere);
            foreach (var item in animals)
            {
                var thisMonth = DateTime.Now.Month;
                if (item.Owned) bookCount += 1;
                if (item.MuseumHave) museumCount += 1;
                if (item.AppearMonth[thisMonth - 1])
                {
                    normalAnimals.Add(item);
                }
            }

            return normalAnimals;
        }

        private static NormalAnimal SelectHemisphereAndConstructFish(Hemisphere hemisphere, AnimalsFish obj, bool owned, bool museumHave)
        {
            string time;
            if (obj.Time.Contains("全天"))
            {
                time = "全天";
            }
            else
            {
                time = obj.Time;
            }
            if (hemisphere == Hemisphere.North)
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = obj.Image, Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = time, AppearMonth = MonthStringToList(obj.North), Owned = owned, MuseumHave = museumHave };
                return normal;
            }
            else
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = obj.Image, Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = time, AppearMonth = MonthStringToList(obj.South), Owned = owned, MuseumHave = museumHave };
                return normal;
            }
        }

        private static NormalAnimal SelectHemisphereAndConstructInsect(Hemisphere hemisphere, AnimalsInsect obj, bool owned, bool museumHave)
        {
            string time;
            if (obj.Time.Contains("全天"))
            {
                time = "全天";
            }
            else
            {
                time = obj.Time;
            }
            if (hemisphere == Hemisphere.North)
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = obj.Image, Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = time, AppearMonth = MonthStringToList(obj.North), Owned = owned, MuseumHave = museumHave };
                return normal;
            }
            else
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = obj.Image, Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = time, AppearMonth = MonthStringToList(obj.South), Owned = owned, MuseumHave = museumHave };
                return normal;
            }
        }

        private static List<bool> MonthStringToList(string month)
        {
            List<bool> appear = new List<bool>();
            if (Helpers.StringHelper.IsNullOrEmptyOrWhiteSpace(month))
            {
                for (int i = 0; i < 12; i++)
                {
                    appear.Add(false);
                }
            }
            else if (month.Contains("、"))
            {
                var collection = month.Split("、");
                if (month.Contains("（全年）"))
                {
                    for (int i = 0; i < 12; i++)
                    {
                        appear.Add(true);
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (collection.Contains((i + 1).ToString()))
                        {
                            appear.Add(true);
                        }
                        else
                        {
                            appear.Add(false);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i.ToString() == month)
                    {
                        appear.Add(true);
                    }
                    else
                    {
                        appear.Add(false);
                    }
                }
            }
            return appear;
        }
    }
}
