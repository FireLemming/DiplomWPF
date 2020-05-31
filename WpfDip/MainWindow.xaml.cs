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
        Program prog = new Program();
        List<IssueWork> issueList = new List<IssueWork>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btView_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(100);
            Dictionary<string, List<string>> filt = new Dictionary<string, List<string>>();
            issueList.AddRange(prog.CreateIssuesList(filt));
            dgAll.ItemsSource = issueList;
            MessageBox.Show("Выборка задач завершена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prog.CSVWork(issueList, tbPathCSV.Text + ".csv");
            }
            catch
            {
                MessageBox.Show("Ошибка пути. Попробуйте снова", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //добавить всплывающее окно или ещё что для пути файла
        }

        private void btJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prog.JsonWork(issueList, tbPathJSON.Text + ".json");
            }
            catch
            {
                MessageBox.Show("Ошибка пути. Попробуйте снова", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
       
            //выбрать, экспорт будет либо из таблицы, лиюо из списка задач, выдаваемых методом.
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
    }
}
