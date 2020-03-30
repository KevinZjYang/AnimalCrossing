using SQLite;
using System.IO;
using System.Linq;
using AnimalCrossing.Models;
using Windows.Storage;

namespace AnimalCrossing.Services
{
    public class SQLiteService
    {
        /// <summary>
        /// 数据库文件所在路径，这里使用 RoamingFolder，数据库文件名叫 Fav.db。
        /// </summary>
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Animals.db");

        public static SQLiteConnection GetDbConnection()
        {
            // 连接数据库，如果数据库文件不存在则创建一个空数据库。
            var option = new SQLiteConnectionString(DbPath, true, key: "pCBR8Jg7n6lHPcwh");
            //var option = new SQLiteConnectionString(DbPath);
            var liteConnection = new SQLiteConnection(option);

            // 创建 Person 模型对应的表，如果已存在，则忽略该操作。
            liteConnection.CreateTable<AnimalsInsect>();
            liteConnection.CreateTable<AnimalsFish>();
            return liteConnection;
        }

        public static void AddInsectData(string name,string json)
        {
            using (var con = GetDbConnection())
            {
                var animal = new AnimalsInsect { Name = name, Data = json };
                con.InsertOrReplace(animal);
            }
        }
        public static void AddFishData(string name, string json)
        {
            using (var con = GetDbConnection())
            {
                var animal = new AnimalsFish { Name = name, Data = json };
                con.Insert(animal);
            }
        }
        //public static void DeleteSingle(string id)
        //{
        //    using (var con = GetDbConnection())
        //    {
        //        con.Delete<FavoriteImage>(id);
        //    }
        //}

        //public static void DeleteAll()
        //{
        //    using (var con = GetDbConnection())
        //    {
        //        con.DeleteAll<FavoriteImage>();
        //    }
        //}
    }
}
