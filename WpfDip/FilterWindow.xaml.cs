using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;

namespace WpfDip
{
    /// <summary>
    /// Логика взаимодействия для FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        string type;
        string cbValue = "";
        Dictionary<string, List<string>> filt;
        List<string> parList = new List<string>();
        List<string> initialList = new List<string>();
        List<string> finalList = new List<string>();
        int count = 0;
        public FilterWindow(Dictionary<string, List<string>> Filt,string Type)
        {
            InitializeComponent();
            type = Type;
            filt = Filt;

            if (filt.ContainsKey(type))// фильтр уже существует, заполнение значением
                filt[type].ForEach(c => tbFilter.Text += c + "\n");
            if (type == "paramchangecount")//отображение элементов для счётчика переходов параметров
            {
                tbFilter.Visibility = Visibility.Collapsed;
                lbTextFilt.Visibility = Visibility.Collapsed;

                tbFinal.Visibility = Visibility.Visible;
                lbInitialText.Visibility = Visibility.Visible;
                lbFinalText.Visibility = Visibility.Visible;
                cbParChange.Visibility = Visibility.Visible;
                tbInitial.Visibility = Visibility.Visible;
                tbFinal.Visibility = Visibility.Visible;
            }
            if (type == "paramchangefilter")
            {
                tbFilter.Visibility = Visibility.Collapsed;
                lbTextFilt.Visibility = Visibility.Collapsed;

                tbFinal.Visibility = Visibility.Visible;
                lbInitialText.Visibility = Visibility.Visible;
                lbFinalText.Visibility = Visibility.Visible;
                cbParChange.Visibility = Visibility.Visible;
                tbInitial.Visibility = Visibility.Visible;
                tbFinal.Visibility = Visibility.Visible;

                btUp.Visibility = Visibility.Visible;
                btDown.Visibility = Visibility.Visible;
                tbNum.Visibility = Visibility.Visible;

            }

                switch (type)
            {
                case "summary":
                    lbHeader.Content = "Аннотация";
                    break;
                case "key":
                    lbHeader.Content = "Ключ";
                    break;
                case "priority":
                    lbHeader.Content = "Приоритет";
                    break;
                case "status":
                    lbHeader.Content = "Статус";
                    break;
                case "type":
                    lbHeader.Content = "Тип";
                    break;
                case "created":
                    lbHeader.Content = "Время создания";
                    break;
                case "environment":
                    lbHeader.Content = "Окружение";
                    break;
                case "project":
                    lbHeader.Content = "Проект";
                    break;
                case "assigneeuser":
                    lbHeader.Content = "Исполнитель";
                    break;
                case "reporteruser":
                    lbHeader.Content = "Автор";
                    break;
                case "paramchangecount":
                    lbHeader.Content = "Счётсчик изменений параметров";
                    break;
                case "paramchangefilter":
                    lbHeader.Content = "Фильтр изменений параметров";
                    break;
            }
            
        }
        /// <summary>
        /// Заполнение словаря при выборе подсчёта изменений параметра
        /// </summary>
        private void FillParamChangeCount()
        {
            if(cbParChange.SelectedIndex == -1)
            {
                MessageBox.Show("Выбирите, значения какого параметра задаются", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            tbInitial.Text = tbInitial.Text.Replace(" ", "");
            tbFinal.Text = tbFinal.Text.Replace(" ", "");
            initialList.AddRange(tbInitial.Text.Replace("\r", "").Split('\n'));//заполнение двух списков значениями из текстбоксов
            finalList.AddRange(tbFinal.Text.Replace("\r", "").Split('\n'));

            if (cbParChange.Text == "Статус")
                cbValue = "status";
            else cbValue = "priority";

            if (initialList.Count > finalList.Count)
                for (int i = 0; i < initialList.Count; i++)
                {
                    if (initialList[i] == null || finalList[i] == null)
                    {
                        if (initialList[i] == null)
                            parList.Add("" + "-" + finalList[i] + ";" + cbValue);
                        if (finalList[i] == null)
                            parList.Add(initialList[i] + "-" + "" + ";" + cbValue);
                    }
                    else
                        parList.Add(initialList[i] + "-" + finalList[i] + ";" + cbValue);
                }
            else
                for (int i = 0; i < finalList.Count; i++)
                {
                    if (initialList[i] == null || finalList[i] == null)
                    {
                        if (initialList[i] == null)
                            parList.Add("" + "-" + finalList[i] + ";" + cbValue);
                        if (finalList[i] == null)
                            parList.Add(initialList[i] + "-" + "" + ";" + cbValue);
                    }
                    else
                        parList.Add(initialList[i] + "-" + finalList[i] + ";" + cbValue);
                }
            if (!filt.ContainsKey(type))
                filt.Add(type, parList);
            else
                filt[type].AddRange(parList);
            filt[type] = filt[type].Distinct().ToList();
            MainWindow.countLimit = 1;
            this.Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (type == "paramchangecount" || type == "paramchangefilter")
                FillParamChangeCount();
            else
            {
                tbFilter.Text = tbFilter.Text.Replace(" ", "");
                parList.AddRange(tbFilter.Text.Replace("\r", "").Split('\n'));//заполнение списка параметрами из текстбокса
                parList.RemoveAll(c => c == "" || c == " ");
                if (parList.Count != 0)
                {
                    if (!filt.ContainsKey(type))//проверка на не существование ключа
                        filt.Add(type, parList);
                    else//ключ существует
                        filt[type].AddRange(parList);
                    filt[type] = filt[type].Distinct().ToList();//удаление повторябщихся значений
                }
                this.Close();
            }
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            tbFilter.Clear();
            filt.Remove(type);
        }

        private void cbParChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            tbParam.Visibility = Visibility.Visible;
            cbParChange.Visibility = Visibility.Hidden;
            if (cbParChange.SelectedIndex == 0)
                cbValue = "Статуса";
            else cbValue = "Приоритета";
            tbParam.Text = "Подсчёт изменений" + "\n" + cbValue;
        }

        private void btUp_Click(object sender, RoutedEventArgs e)
        {
            count += 1;
            tbNum.Text = count.ToString();
        }

        private void btDown_Click(object sender, RoutedEventArgs e)
        {
            if (count > 0)
                count -= 1;
            tbNum.Text = count.ToString();
        }
    }
}
