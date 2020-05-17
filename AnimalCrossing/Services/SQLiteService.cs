using SQLite;
using System.IO;
using System.Linq;
using AnimalCrossing.Models;
using Windows.Storage;
using System.Threading.Tasks;

namespace AnimalCrossing.Services
{
    public static class SQLiteService
    {
        #region 图鉴数据库

        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBNAME);
        public const string DBNAME = "AnimalsCrossing.db";

        public static async Task<SQLiteAsyncConnection> GetDbConnection()
        {
            var option = new SQLiteConnectionString(DbPath);
            var liteConnection = new SQLiteAsyncConnection(option);

            await liteConnection.CreateTableAsync<AnimalsInsect>();
            await liteConnection.CreateTableAsync<AnimalsFish>();
            await liteConnection.CreateTableAsync<Plant>();
            await liteConnection.CreateTableAsync<LittleAnimal>();
            await liteConnection.CreateTableAsync<Album>();
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

        public static async Task<SQLiteAsyncConnection> GetUserDbConnection()
        {
            //var option = new SQLiteConnectionString(DbPath, true, key: "pCBR8Jg7n6lHPcwh");
            var option = new SQLiteConnectionString(UserDbPath);
            var liteConnection = new SQLiteAsyncConnection(option);

            await liteConnection.CreateTableAsync<UserInsect>();
            await liteConnection.CreateTableAsync<UserFish>();
            await liteConnection.CreateTableAsync<UserAlbum>();
            return liteConnection;
        }

        public static async Task AddUserInsect(UserInsect userInsect)
        {
            var con = await GetUserDbConnection();
            await con.InsertOrReplaceAsync(userInsect);
            await con.CloseAsync();
        }

        public static async Task AddUserFish(UserFish userFish)
        {
            var con = await GetUserDbConnection();
            await con.InsertOrReplaceAsync(userFish);
            await con.CloseAsync();
        }

        public static async Task AddUserAlbum(UserAlbum userAlbum)
        {
            var con = await GetUserDbConnection();
            await con.InsertOrReplaceAsync(userAlbum);
            await con.CloseAsync();
        }

        #endregion 用户信息数据库
    }
}
