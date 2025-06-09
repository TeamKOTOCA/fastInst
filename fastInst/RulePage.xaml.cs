using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace fastInst
{
    /// <summary>
    /// RulePage.xaml の相互作用ロジック
    /// </summary>
    public partial class RulePage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public RulePage()
        {
            InitializeComponent();
            LoadRuleFromGithub();
        }

        private async Task LoadRuleFromGithub()
        {
            string apiUrl = "https://api.github.com/repos/" + mainWindow.GithubUser + "/" + mainWindow.GithubRepoName + "/releases/latest";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "fastinst");

            string json = await client.GetStringAsync(apiUrl);
            using var doc = JsonDocument.Parse(json);

            // assets -> name: rule.md を探す
            var assets = doc.RootElement.GetProperty("assets");
            foreach (var asset in assets.EnumerateArray())
            {
                if (asset.GetProperty("name").GetString() == "rule.md")
                {
                    string downloadUrl = asset.GetProperty("browser_download_url").GetString();
                    string markdown = await client.GetStringAsync(downloadUrl);

                    break;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.Gobtn.IsEnabled = true;
            }
        }
    }
}
