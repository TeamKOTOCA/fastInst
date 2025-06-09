using System;
using System.Windows;
using System.Windows.Controls;

namespace fastInst
{
    /// <summary>
    /// finish.xaml の相互作用ロジック
    /// </summary>
    public partial class finish : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public finish()
        {
            InitializeComponent();
            finishtext.Text = "インストールが完了しました。" +
                "\n アンインストールする場合は、" + @"C:\Program Files\" + mainWindow.GithubRepoName + "をフォルダーごと削除してください。 \n" +
                "このインストーラーはアンインストール非対応です。";
        }
    }
}
