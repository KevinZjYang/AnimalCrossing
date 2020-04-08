using SQLite;
using System.IO;
using System.Linq;
using Windows.Storage;
using AnimalCrossingDbTool.Models;

namespace AnimalCrossingDbTool
{
    public static class SQLiteService
    {
        #region 图鉴数据库

        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBNAME);
        public const string DBNAME = "Animals.db";

        public static SQLiteConnection GetDbConnection()
        {
            //var option = new SQLiteConnectionString(DbPath, true, key: "pCBR8Jg7n6lHPcwh");
            var option = new SQLiteConnectionString(DbPath);
            var liteConnection = new SQLiteConnection(option);

            liteConnection.CreateTable<AnimalsInsect>();
            liteConnection.CreateTable<AnimalsFish>();
            return liteConnection;
        }

        public static void AddInsectData(string name, string json)
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
                con.InsertOrReplace(animal);
            }
        }

        #endregion 图鉴数据库

     
    }
}
