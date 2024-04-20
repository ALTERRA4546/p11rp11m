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

namespace TerminalСommon
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

        //Авторизация сотрудника
        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int kode = Convert.ToInt32(login.Text);
                using (var bd = new HranitelProEntities())
                {
                    var otdel = bd.Отдел.FirstOrDefault(s => s.Название == "Общий отдел");
                    var sotrudnik = bd.Сотрудник.Where(s => s.Код_отдела == otdel.Код_отдела).FirstOrDefault(s => s.Код_авторизации == kode);
                    if (sotrudnik != null)
                    {
                        ViewOrder vo = new ViewOrder();
                        vo.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверный код авторизации");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
