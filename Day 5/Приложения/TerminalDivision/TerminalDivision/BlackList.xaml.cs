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
using static TerminalDivision.ViewOrder;

namespace TerminalDivision
{
    /// <summary>
    /// Логика взаимодействия для BlackList.xaml
    /// </summary>
    public partial class BlackList : Window
    {
        public BlackList()
        {
            InitializeComponent();
        }

        //Добавление пользователя в черный список
        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prich.Text == "")
                {
                    MessageBox.Show("Укажите причину помещения посетителя в черный список");
                    return;
                }
                using (var bd = new HranitelProEntities())
                {
                    var bl = new Черный_список();

                    bl.Код_посетителя = Zayavka.kodPosetit;
                    bl.Причина_добавления = prich.Text;
                    bd.Черный_список.Add(bl);
                    bd.SaveChanges();
                    MessageBox.Show("Пользователь добавлен в черный список");
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
