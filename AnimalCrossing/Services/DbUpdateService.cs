using AnimalCrossing.Helpers;
using AnimalCrossing.Models;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;

namespace AnimalCrossing.Services
{
    public class DbUpdateService
    {
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        private const string DBVERNAME = "DB.ver";

        internal static async Task<bool> DownloadIfAppropriateAsync()
        {
            var info = await GetLatestDbInfoAsync();
            if (info != null)
            {
                if (SystemInformation.IsFirstRun || SystemInformation.IsAppUpdated)
                {
                    var result = await DownloadDbFileAsync(info.DbName);
                    if (result)
                    {
                        Helpers.SettingsHelper.SaveLocalSetting(SettingsKey.DbVersionKey, info.Time);

                        ExtendedSplash.Current.SetMessage("数据库下载完成");
                        //await Task.Delay(TimeSpan.FromSeconds(5));
                        //await CoreApplication.RequestRestartAsync(string.Empty);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var result = await UpdateIfNeed(info);
                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private static async Task<UpdateInfo> GetLatestDbInfoAsync()
        {
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable == false)
            {
                ExtendedSplash.Current.SetMessage("应用需要连接网络，请连接网络后重新打开应用");
            }
            Uri source = new Uri($"https://file.iuwp.top/{DBVERNAME}");
            var destinationFile = await localFolder.CreateFileAsync(
                 DBVERNAME, CreationCollisionOption.ReplaceExisting);
            var downloaded = new BackgroundDownloader();
            var download = downloaded.CreateDownload(source, destinationFile);
            var operation = await download.StartAsync();
            if (operation.Progress.Status == BackgroundTransferStatus.Completed)
            {
                var json = await FileIO.ReadTextAsync(destinationFile);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateInfo>(json);
            }
            else
            {
                ExtendedSplash.Current.SetMessage("获取数据库版本失败，请检查网络后重新打开应用");
                return null;
            }
        }

        private static async Task<bool> UpdateIfNeed(UpdateInfo info)
        {
            ExtendedSplash.Current.SetMessage("检查数据库更新...");
            var ver = DateTimeOffset.Parse(info.Time);

            var localVer = DateTimeOffset.Parse(SettingsHelper.GetLocalSetting(SettingsKey.DbVersionKey));

            if (ver > localVer)
            {
                ExtendedSplash.Current.SetMessage("更新数据库...");
                var result = await DownloadDbFileAsync(info.DbName);
                if (result)
                {
                    Helpers.SettingsHelper.SaveLocalSetting(SettingsKey.DbVersionKey, info.Time);
                    //var notify = new Helpers.NotifyPopup("数据库下载完成，即将自动重启应用", TimeSpan.FromSeconds(5));
                    //notify.Show();

                    //await Task.Delay(TimeSpan.FromSeconds(5));
                    //await CoreApplication.RequestRestartAsync(string.Empty);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private static async Task<bool> DownloadDbFileAsync(string fileName)
        {
            Uri source = new Uri($"https://file.iuwp.top/{fileName}");

            Helpers.NotifyPopup notify;
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable == false)
            {
                ExtendedSplash.Current.SetMessage("下载数据库需要连接网络，请连接网络后重新打开应用");
                return false;
            }
            ExtendedSplash.Current.SetMessage("正在下载数据库...");

            var destinationFile = await localFolder.CreateFileAsync(
               SQLiteService.DBNAME, CreationCollisionOption.ReplaceExisting);

            var downloaded = new BackgroundDownloader();
            var download = downloaded.CreateDownload(source, destinationFile);
            var operation = await download.StartAsync();

            if (operation.Progress.Status == BackgroundTransferStatus.Completed)
            {
                return true;
            }
            else
            {
                Helpers.SettingsHelper.SaveLocalSetting(SettingsKey.DbVersionKey, "2020/1/1");
                ExtendedSplash.Current.SetMessage("数据库下载失败，请检查网络状况后，重新开启应用");
                return false;
            }
        }
    }
}
