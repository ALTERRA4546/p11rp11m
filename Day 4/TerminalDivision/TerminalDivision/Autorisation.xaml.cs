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

namespace TerminalDivision
{
    /// <summary>
    /// Логика взаимодействия для Autorisation.xaml
    /// </summary>
    public partial class Autorisation : Window
    {
        public Autorisation()
        {
            InitializeComponent();
        }

        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            int kode = Convert.ToInt32(login.Text);
            using (var bd = new HranitelProEntities())
            {
                var otdel = bd.Отдел.FirstOrDefault(s => s.Название == "Подразделение");
                var sotrudnik = bd.Сотрудник.Where(s => s.Код_отдела == otdel.Код_отдела).FirstOrDefault(s => s.Код_авторизации == kode);
                if (sotrudnik != null)
                {
                    ViewOrder vo = new ViewOrder();
                    vo.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Логин или пароль неправильный");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
