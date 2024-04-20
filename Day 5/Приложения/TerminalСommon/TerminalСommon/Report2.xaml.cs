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
using static System.Net.Mime.MediaTypeNames;
using static TerminalСommon.ViewOrder;

namespace TerminalСommon
{
    /// <summary>
    /// Логика взаимодействия для Report2.xaml
    /// </summary>
    public partial class Report2 : Window
    {
        public Report2()
        {
            InitializeComponent();
            Initial();
        }

        //Предварительная выборка данных о посетителей на территории
        private void Initial()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    var zayavka = from z in bd.Заявка
                                  join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                  join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                  join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                  join org in bd.Организация on poset.Код_организации equals org.Код_организации
                                  where (z.Время_входа != null && z.Время_выхода == null)
                                  select new
                                  {
                                      poset.Код_посетителя,
                                      poset.Фамилия,
                                      poset.Имя,
                                      poset.Отчество,
                                      poset.Номер_телефона,
                                      poset.Email,
                                      poset.Дата_рождения,
                                      Организация = org.Название,
                                      Подразделение = podr.Назавние
                                  };
                    dataOut.ItemsSource = zayavka.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Заполнение DataGrid списком посетителей на территории
        private void rep()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    //Проверка группироваки
                    if (groupCh.IsChecked == false)
                    {
                        //Выбрка данных о посетителях на территории без группироваки
                        var zayavka = from z in bd.Заявка
                                      join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                      join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                      join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                      join org in bd.Организация on poset.Код_организации equals org.Код_организации
                                      where (z.Время_входа != null && z.Время_выхода == null)
                                      select new
                                      {
                                          poset.Код_посетителя,
                                          poset.Фамилия,
                                          poset.Имя,
                                          poset.Отчество,
                                          poset.Номер_телефона,
                                          poset.Email,
                                          poset.Дата_рождения,
                                          Организация = org.Название,
                                          Подразделение = podr.Назавние
                                      };
                        dataOut.ItemsSource = zayavka.ToList();
                    }
                    else
                    {
                        //Выбрка данных о посетителях на территории с группировакой
                        var zayavka = from z in bd.Заявка
                                      join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                      join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                      join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                      join org in bd.Организация on poset.Код_организации equals org.Код_организации
                                      where (z.Время_входа != null && z.Время_выхода == null)
                                      group poset by new
                                      {
                                          poset.Код_посетителя,
                                          poset.Фамилия,
                                          poset.Имя,
                                          poset.Отчество,
                                          poset.Номер_телефона,
                                          poset.Email,
                                          poset.Дата_рождения,
                                          Организация = org.Название,
                                          Подразделение = podr.Назавние,
                                      } into g
                                      select new
                                      {
                                          g.Key.Код_посетителя,
                                          g.Key.Фамилия,
                                          g.Key.Имя,
                                          g.Key.Отчество,
                                          g.Key.Номер_телефона,
                                          g.Key.Email,
                                          g.Key.Дата_рождения,
                                          Организация = g.Key.Организация,
                                          Подразделение = g.Key.Подразделение
                                      };
                        dataOut.ItemsSource = zayavka.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupCh_Click(object sender, RoutedEventArgs e)
        {
            rep();
        }
    }
}
