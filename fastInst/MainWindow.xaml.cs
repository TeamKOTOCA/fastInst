using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace fastInst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //各種設定をここに入れて
            //GitHubのユーザー名
            public string GithubUser = "teamKOTOCA";
            //GitHubのリポジトリの名前。
            public string GithubRepoName = "PortaPad";
            //引っ張ってくるexeファイルの名前。「.exe」まで含める。一言一句正確にかくこと。
            public string ProgramFileName = "PortaPad.exe";
            //サービスとして設定するかどうか
            public bool SetForService = true;
                public string ServiceDescription = "Portapadの起動を制御";


        int pagecount = 0;
        public MainWindow()
        {
            InitializeComponent();
            Mainframe.Navigate(new start());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pagechange();
        }

        public void pagechange() {
            if (pagecount == 0)
            {
                Mainframe.Navigate(new RulePage());
                Gobtn.IsEnabled = false;
            }
            else if (pagecount == 1)
            {
                Mainframe.Navigate(new progres());
                Gobtn.IsEnabled = false;
                Gobtn.Content = "インストール中";
                Cancelbtn.IsEnabled = false;
            }
            else if (pagecount == 2)
            {
                Mainframe.Navigate(new finish());
                Gobtn.IsEnabled = true;
                Gobtn.Content = "終了";
            }
            else if (pagecount >= 3)
            {
                Application.Current.Shutdown();
            }
                pagecount++;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}