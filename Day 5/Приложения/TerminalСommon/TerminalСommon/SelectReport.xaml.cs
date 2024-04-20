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
    /// Логика взаимодействия для SelectReport.xaml
    /// </summary>
    public partial class SelectReport : Window
    {
        public SelectReport()
        {
            InitializeComponent();
        }

        //Открытие справки о количестве поситителей
        private void EnterA_Click(object sender, RoutedEventArgs e)
        {
            Report r = new Report();
            r.Show();
            this.Hide();
        }

        //Открытие справки о посетителях на территории
        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            Report2 r = new Report2();
            r.Show();
            this.Hide();
        }
    }
}
