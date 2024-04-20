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

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для SelectedTypeZayavka.xaml
    /// </summary>
    public partial class SelectedTypeZayavka : Window
    {
        public SelectedTypeZayavka()
        {
            InitializeComponent();
        }

        private void LitchnVisit(object sender, RoutedEventArgs e)
        {
            LVisit lv = new LVisit();
            lv.Show();
            this.Hide();
        }

        private void GrupaVisit(object sender, RoutedEventArgs e)
        {
            GVisit gv = new GVisit();
            gv.Show();
            this.Hide();
        }

        private void AvtorizationVisit(object sender, RoutedEventArgs e)
        {
            Autorisation aut = new Autorisation();
            aut.Show();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
