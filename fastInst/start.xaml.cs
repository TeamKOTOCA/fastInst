using System;
using System.Windows;
using System.Windows.Controls;

namespace fastInst
{
    /// <summary>
    /// finish.xaml の相互作用ロジック
    /// </summary>
    public partial class start : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public start()
        {
            InitializeComponent();
            starttext.Text = "インストールを開始します。" +
                "インストールを開始するには、[次へ]を押してください";
        }
    }
}
