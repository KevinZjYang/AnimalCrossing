using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace AnimalCrossing.Services
{
    public class FirstRunDbDownloadService
    {
        internal static async Task InitializeAsync()
        {
            Uri source = new Uri($"http://api.iuwp.top:9000/{Services.SQLiteService.DBNAME}");

            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.TryGetItemAsync(SQLiteService.DBNAME);
            var property = await file.GetBasicPropertiesAsync();
            if (file == null || property.Size < 51200)
            {
                Helpers.NotifyPopup notify;
                if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable == false)
                {
                    notify = new Helpers.NotifyPopup("第一次需要连接网络，下载数据库，请连接网络后重新打开应用", TimeSpan.FromSeconds(60));
                    notify.Show();
                    return;
                }
                notify = new Helpers.NotifyPopup("首次启动，正在下载数据库...", TimeSpan.FromSeconds(5));
                notify.Show();

                var destinationFile = await localFolder.CreateFileAsync(
                   SQLiteService.DBNAME, CreationCollisionOption.ReplaceExisting);

                var downloaded = new BackgroundDownloader();
                var download = downloaded.CreateDownload(source, destinationFile);
                var operation = await download.StartAsync();

                if (operation.Progress.Status != BackgroundTransferStatus.Completed)
                {
                    notify = new Helpers.NotifyPopup("数据库下载失败，请检查网络状况后，重新开启应用", TimeSpan.FromSeconds(60));
                    notify.Show();
                }
                else
                {
                    notify = new Helpers.NotifyPopup("数据库下载完成，即将自动重启应用", TimeSpan.FromSeconds(5));
                    notify.Show();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await CoreApplication.RequestRestartAsync(string.Empty);
                }
            }
            else
            {
                return;
            }
        }
    }
}
