using SQLite;
using System.IO;
using System.Linq;
using AnimalCrossing.Models;
using Windows.Storage;

namespace AnimalCrossing.Services
{
    public static class SQLiteService
    {
        #region 图鉴数据库

        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBNAME);
        public const string DBNAME = "Animals1.3.1.db";

        public static SQLiteConnection GetDbConnection()
        {
            var option = new SQLiteConnectionString(DbPath);
            var liteConnection = new SQLiteConnection(option);

            liteConnection.CreateTable<AnimalsInsect>();
            liteConnection.CreateTable<AnimalsFish>();
            liteConnection.CreateTable<Plant>();
            liteConnection.CreateTable<LittleAnimal>();
            liteConnection.CreateTable<Album>();
            return liteConnection;
        }

        //public static void AddInsectData(string name, string json)
        //{
        //    using (var con = GetDbConnection())
        //    {
        //        var animal = new AnimalsInsect { Name = name, Data = json };
        //        con.InsertOrReplace(animal);
        //    }
        //}

        //public static void AddFishData(string name, string json)
        //{
        //    using (var con = GetDbConnection())
        //    {
        //        var animal = new AnimalsFish { Name = name, Data = json };
        //        con.InsertOrReplace(animal);
        //    }
        //}

        #endregion 图鉴数据库

        #region 用户信息数据库

        public readonly static string UserDbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "User.db");

        public static SQLiteConnection GetUserDbConnection()
        {
            //var option = new SQLiteConnectionString(DbPath, true, key: "pCBR8Jg7n6lHPcwh");
            var option = new SQLiteConnectionString(UserDbPath);
            var liteConnection = new SQLiteConnection(option);

            liteConnection.CreateTable<UserInsect>();
            liteConnection.CreateTable<UserFish>();
            return liteConnection;
        }

        public static void AddUserInsect(UserInsect userInsect)
        {
            using (var con = GetUserDbConnection())
            {
                con.InsertOrReplace(userInsect);
            }
        }

        public static void AddUserFish(UserFish userFish)
        {
            using (var con = GetUserDbConnection())
            {
                con.InsertOrReplace(userFish);
            }
        }

        #endregion 用户信息数据库
    }
}
