using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace WpfDip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, List<string>> filt = new Dictionary<string, List<string>>();
        Program prog = new Program();
        List<IssueWork> issueList = new List<IssueWork>();
        //string filtType = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btView_Click(object sender, RoutedEventArgs e)
        {
            issueList.AddRange(prog.CreateIssuesList(filt));
            dgAll.ItemsSource = issueList;
            MessageBox.Show("Выборка задач завершена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btExportCSV_Click(object sender, RoutedEventArgs e)
        {
            if (dgAll.Items.Count > 0)
            {
                ExportWindow ew = new ExportWindow(issueList, "csv");
                this.Hide();
                ew.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("Сначала выберите задачи", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btExportJSON_Click(object sender, RoutedEventArgs e)
        {
            if (dgAll.Items.Count > 0)
            {
                ExportWindow ew = new ExportWindow(issueList, "json");
                this.Hide();
                ew.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("Сначала выберите задачи", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
        }        

        private void btDel_Click(object sender, RoutedEventArgs e) 
        {
            CheckBox cb = new CheckBox();
            List<IssueWork> RemoveList = new List<IssueWork>();

            foreach(var s in dgAll.ItemsSource)
            {
                cb = dgAll.Columns[0].GetCellContent(s) as CheckBox;
                if ((bool)cb.IsChecked)
                {
                    RemoveList.Add(s as IssueWork);
                }
            }
            issueList.RemoveAll(c => RemoveList.Contains(c));
            dgAll.ItemsSource = null;
            dgAll.ItemsSource = issueList;
        }

        private void cbSummary_Checked(object sender, RoutedEventArgs e)
        {
            FilterWindow fw = new FilterWindow(filt, "summary");
            this.Hide();
            fw.ShowDialog();
            this.Show();
        }
    }
}
