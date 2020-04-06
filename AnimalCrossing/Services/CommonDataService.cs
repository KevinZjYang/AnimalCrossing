using System;
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
                var animals = con.Table<AnimalsFish>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Fish>(item.Data);

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

                        var normal = SelectHemisphereAndConstructFish(hemisphere, obj, owned, museumHave);
                        normalAnimals.Add(normal);
                    }
                    else
                    {
                        var normal = SelectHemisphereAndConstructFish(hemisphere, obj, false, false);
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
                if (item.AppearMonth[thisMonth - 1] == "1")
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
                var animals = con.Table<AnimalsInsect>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);

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

                        var normal = SelectHemisphereAndConstructInsect(hemisphere, obj, owned, museumHave);

                        normalAnimals.Add(normal);
                    }
                    else
                    {
                        var normal = SelectHemisphereAndConstructInsect(hemisphere, obj, false, false);

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
                if (item.AppearMonth[thisMonth - 1] == "1")
                {
                    normalAnimals.Add(item);
                }
            }

            return normalAnimals;
        }

        private static NormalAnimal SelectHemisphereAndConstructFish(Hemisphere hemisphere, Fish obj, bool owned, bool museumHave)
        {
            if (hemisphere == Hemisphere.North)
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                return normal;
            }
            else
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                return normal;
            }
        }

        private static NormalAnimal SelectHemisphereAndConstructInsect(Hemisphere hemisphere, Insect obj, bool owned, bool museumHave)
        {
            if (hemisphere == Hemisphere.North)
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                return normal;
            }
            else
            {
                var normal = new NormalAnimal { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                return normal;
            }
        }
    }
}
