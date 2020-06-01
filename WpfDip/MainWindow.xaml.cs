using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
        public static Dictionary<string, List<string>> filt = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> filtBack = new Dictionary<string, List<string>>();
        Program prog = new Program();
        List<IssueWork> issueList = new List<IssueWork>();
        //string filtType = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод для открытия окна выставления фильтров(нужен для сохранения страрых значений при снятии галочки)
        /// </summary>
        public bool OpenFilterWindow(bool check, string type)
        {
            if (check)//если чекбокс проставлен
            {
                if (filtBack.ContainsKey(type))//если ключ существует в резервном списке
                {
                    filt.Add(type, filtBack[type]);//довавление в список фильтров, заданных ранее
                    //filtBack[type].Clear();//очистка резервного списка
                    filtBack.Remove(type);//удавление ключа/очистка резервного списка
                }
                FilterWindow fw = new FilterWindow(filt, type);
                this.Hide();
                fw.ShowDialog();
                this.Show();
            }
            else
            {
                if (filt.ContainsKey(type))//если существует ключ в основном словаре
                {
                    if (filtBack.ContainsKey(type))//если существует ключ в резервном словаре
                        filtBack[type].AddRange(filt[type]);//Добавление значений словаря во второй словарь для хранения фильтров
                    else
                        filtBack.Add(type, filt[type]);
                    filt.Remove(type);//удавление ключа/очистка основного списка
                }
            }
            if (filtBack.Count == 0 && filt.Count == 0)
                return false;//если фильтиры пустые - чекбокс отожмётся
            else return true;
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

        private void cbSummary_Click(object sender, RoutedEventArgs e)
        {
            string type = "summary";
            bool check = (bool)cbSummary.IsChecked;//эти строки для каждово cb свои, а дальше вызов метода
            if (!OpenFilterWindow(check, type))
                cbSummary.IsChecked = false;
            
        }
        
    }
}
