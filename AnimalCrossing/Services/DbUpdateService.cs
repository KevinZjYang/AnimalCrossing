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

        internal static async Task DownloadIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
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
                                var notify = new Helpers.NotifyPopup("数据库下载完成，即将自动重启应用", TimeSpan.FromSeconds(5));
                                notify.Show();

                                await Task.Delay(TimeSpan.FromSeconds(5));
                                await CoreApplication.RequestRestartAsync(string.Empty);
                            }
                        }
                        else
                        {
                            await UpdateIfNeed(info);
                        }
                    }
                });
        }

        private static async Task<UpdateInfo> GetLatestDbInfoAsync()
        {
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
                return null;
            }
        }

        private static async Task UpdateIfNeed(UpdateInfo info)
        {
            var ver = DateTimeOffset.Parse(info.Time);

            var localVer = DateTimeOffset.Parse(SettingsHelper.GetLocalSetting(SettingsKey.DbVersionKey));

            if (ver > localVer)
            {
                var result = await DownloadDbFileAsync(info.DbName);
                if (result)
                {
                    Helpers.SettingsHelper.SaveLocalSetting(SettingsKey.DbVersionKey, info.Time);
                    var notify = new Helpers.NotifyPopup("数据库下载完成，即将自动重启应用", TimeSpan.FromSeconds(5));
                    notify.Show();

                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await CoreApplication.RequestRestartAsync(string.Empty);
                }
            }
        }

        private static async Task<bool> DownloadDbFileAsync(string fileName)
        {
            Uri source = new Uri($"https://file.iuwp.top/{fileName}");

            Helpers.NotifyPopup notify;
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable == false)
            {
                notify = new Helpers.NotifyPopup("下载数据库需要连接网络，请连接网络后重新打开应用", TimeSpan.FromSeconds(60));
                notify.Show();
                return false;
            }

            notify = new Helpers.NotifyPopup("正在下载数据库...", TimeSpan.FromSeconds(5));
            notify.Show();

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
                notify = new Helpers.NotifyPopup("数据库下载失败，请检查网络状况后，重新开启应用", TimeSpan.FromSeconds(60));
                notify.Show();
                return false;
            }
        }
    }
}
