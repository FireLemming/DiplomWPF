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

namespace WpfDip
{
    /// <summary>
    /// Логика взаимодействия для FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        string type;
        Dictionary<string, List<string>> filt;
        List<string> parList = new List<string>();
        public FilterWindow(Dictionary<string, List<string>> Filt,string Type)
        {
            InitializeComponent();
            type = Type;
            filt = Filt;
            if (filt.ContainsKey("summary"))
            {
                filt["summary"].ForEach(c => tbFilter.Text += c +"\n");
                lbHeader.Content = "Аннотация";
            }
        }


        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbFilter.Text = tbFilter.Text.Replace(" ", "");
            parList.AddRange(tbFilter.Text.Replace("\r", "").Split('\n'));//заполнение списка параметрами из текстбокса
            parList.RemoveAll(c => c == "" || c==" ");
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

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            tbFilter.Clear();
            filt.Remove(type);
        }
    }
}
