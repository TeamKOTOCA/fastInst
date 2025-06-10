using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace fastInst
{
    /// <summary>
    /// progres.xaml の相互作用ロジック
    /// </summary>
    public partial class progres : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public progres()
        {
            InitializeComponent();
            InstallProgres();
        }
        private async void InstallProgres()
        {
            string apiUrl = "https://api.github.com/repos/" + mainWindow.GithubUser + "/" + mainWindow.GithubRepoName + "/releases/latest";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "fastinst");

            string json = await client.GetStringAsync(apiUrl);
            using var doc = JsonDocument.Parse(json);
            progress.Value = 10;

            // assets -> name を探す
            var assets = doc.RootElement.GetProperty("assets");
            foreach (var asset in assets.EnumerateArray())
            {

                statusmsg.Content += asset.GetProperty("name").GetString();
                if (asset.GetProperty("name").GetString() == mainWindow.ProgramFileName)
                {
                    statusmsg.Content += "\n プログラムファイルをインストールしてます";
                    string downloadUrl = asset.GetProperty("browser_download_url").GetString();
                    byte[] fileBytes = await client.GetByteArrayAsync(downloadUrl);
                    string folder = @"C:\Program Files\" + mainWindow.GithubRepoName;
                    string fullPath = System.IO.Path.Combine(folder, mainWindow.ProgramFileName);
                    Directory.CreateDirectory(folder);
                    await File.WriteAllBytesAsync(fullPath, fileBytes);
                    progress.Value = 60;
                    statusmsg.Content = "起動ファイルなどを構成中";
                    if (mainWindow.SetForService)
                    {
                        setservice();
                    }

                    break;
                }
            }
            ((MainWindow)Application.Current.MainWindow).pagechange();
        }

        private void setservice()
        {
            try
            {
                string exePath = @"C:\Program Files\" + mainWindow.GithubRepoName + @"\" + mainWindow.ProgramFileName;
                string serviceName = mainWindow.GithubRepoName; //内部名

                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = mainWindow.ServiceDescription;

                    // トリガー：ログオン時に起動
                    td.Triggers.Add(new LogonTrigger());

                    // 操作：アプリを実行
                    td.Actions.Add(new ExecAction(exePath, null, null));

                    // 設定：アプリが落ちたら再起動（再試行3回、10秒おき）
                    td.Settings.RestartCount = 20;
                    td.Settings.RestartInterval = TimeSpan.FromSeconds(30);
                    td.Settings.StopIfGoingOnBatteries = false;
                    td.Settings.DisallowStartIfOnBatteries = false;

                    // 登録（必要に応じてドメイン・ユーザー指定）
                    ts.RootFolder.RegisterTaskDefinition(@"PortapadSystemSet", td,
                        TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
                }
                return;

            }
            catch (Exception ex) {
                MessageBox.Show(
                    "サービス設定中にエラーが発生しました。",
                    "エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Application.Current.Shutdown();
            }
        }

    }
}
