using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Hranitel.LVisit;

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для SelectedSotrudnic.xaml
    /// </summary>
    public partial class SelectedSotrudnic : Window
    {
        public SelectedSotrudnic()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            using (var bd = new HranitelProEntities())
            {
                var sotrudnik = from s in bd.Сотрудник
                                where (s.Код_подразделения == SelectSotrud.Podrazdel)
                                select s.Код_сотрудника+" "+s.Фамилия+" "+s.Имя+" "+s.Отчество;
                selectSotrud.ItemsSource = sotrudnik.ToList();
                selectSotrud.SelectedIndex = 0;
            }
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] mas = selectSotrud.SelectedItem.ToString().Split(' ');
                SelectSotrud.KodSotrud = Convert.ToInt32(mas[0]);
                SelectSotrud.Famaly = mas[1];
                SelectSotrud.Name = mas[2];
                SelectSotrud.Otchestvo = mas[3];
            }
            catch
            {
                MessageBox.Show("Сотрудник не был выбран");
            }
            this.Hide();
        }
    }
}
