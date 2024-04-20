using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        public ViewOrder()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial();
        }

        public static class Zayavka
        {
            public static int kodZayavky;
            public static int kodPosetit;
        }

        //Заполнение DataGrid заявками и заполнение ComboBox-во данными которые будут фильтроваться
        private void Initial()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    string stat = "одобрена";
                    var ty = bd.Тип_заявки.Select(s => s.Название).ToList();
                    var pd = bd.Подразделение.Select(s => s.Назавние).ToList();
                    ty.Add("Все");
                    pd.Add("Все");

                    var zayavka = from z in bd.Заявка
                                  join t in bd.Тип_заявки on z.Код_типа_заявки equals t.Код_типа_заявки
                                  join s in bd.Статус on z.Код_статуса equals s.Код_cтатуса
                                  join prp in bd.Пропуск on z.Код_пропуска equals prp.Код_пропуска
                                  join pdr in bd.Подразделение on z.Код_подразделения equals pdr.Код_подразделения
                                  join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                  join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                  join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                  join org in bd.Организация on poset.Код_организации equals org.Код_организации
                                  where (s.Название == stat)
                                  select new
                                  {
                                      poset.Код_посетителя,
                                      z.Код_заявки,
                                      Тип_заявки = t.Название,
                                      poset.Фамилия,
                                      poset.Имя,
                                      poset.Отчество,
                                      poset.Номер_телефона,
                                      poset.Email,
                                      poset.Серия_паспорта,
                                      poset.Номер_паспорта,
                                      Организация = org.Название,
                                      Статус = s.Название,
                                      Подразделение = pdr.Назавние,
                                      Код_сотрудника = sotr.Код_авторизации,
                                      prp.Код_пропуска,
                                      prp.Срок_начала_действия,
                                      prp.Срок_окончания_действия,
                                      prp.Цель_посещения,
                                      z.Дата_создания,
                                      z.Дата_и_время_посящения
                                  };
                    dataOut.ItemsSource = zayavka.ToList();
                    dataOut.Columns[0].MaxWidth = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Фильтрация записей
        private void FiltrationOnDate(DatePicker dt)
        {
            try
            {
                string stat = "одобрена";
                using (var bd = new HranitelProEntities())
                {
                    var zayavka = from z in bd.Заявка
                                  join t in bd.Тип_заявки on z.Код_типа_заявки equals t.Код_типа_заявки
                                  join s in bd.Статус on z.Код_статуса equals s.Код_cтатуса
                                  join prp in bd.Пропуск on z.Код_пропуска equals prp.Код_пропуска
                                  join pdr in bd.Подразделение on z.Код_подразделения equals pdr.Код_подразделения
                                  join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                  join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                  join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                  join org in bd.Организация on poset.Код_организации equals org.Код_организации
                                  where ((dt.SelectedDate == null || z.Дата_создания == dt.SelectedDate.Value))
                                  select new
                                  {
                                      poset.Код_посетителя,
                                      z.Код_заявки,
                                      Тип_заявки = t.Название,
                                      poset.Фамилия,
                                      poset.Имя,
                                      poset.Отчество,
                                      poset.Номер_телефона,
                                      poset.Email,
                                      poset.Серия_паспорта,
                                      poset.Номер_паспорта,
                                      Организация = org.Название,
                                      Статус = s.Название,
                                      Подразделение = pdr.Назавние,
                                      Код_сотрудника = sotr.Код_авторизации,
                                      prp.Код_пропуска,
                                      prp.Срок_начала_действия,
                                      prp.Срок_окончания_действия,
                                      prp.Цель_посещения,
                                      z.Дата_создания,
                                      z.Дата_и_время_посящения
                                  };
                    dataOut.ItemsSource = zayavka.ToList();
                    dataOut.Columns[0].MaxWidth = 0;
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

        //Открытие заявки в новом окне
        private void select_Click(object sender, RoutedEventArgs e)
        {
            if (dataOut.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dataOut.ItemContainerGenerator.ContainerFromIndex(dataOut.SelectedIndex);
            DataGridCell cell = dataOut.Columns[0].GetCellContent(row).Parent as DataGridCell;
            Zayavka.kodZayavky = Convert.ToInt32(((TextBlock)cell.Content).Text);
            CheckOrder co = new CheckOrder();
            co.ShowDialog();
        }

        private void type_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dateCrCheck.IsChecked == false)
            {
                dateCr.SelectedDate = null;
            }
            FiltrationOnDate(dateCr);
        }

        private void dateCr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dateCr.SelectedDate != null)
                FiltrationOnDate(dateCr);
        }

        private void dateCr_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FiltrationOnDate(dateCr);
        }

        //Открытие окна черного списка
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (dataOut.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dataOut.ItemContainerGenerator.ContainerFromIndex(dataOut.SelectedIndex);
            DataGridCell cell = dataOut.Columns[0].GetCellContent(row).Parent as DataGridCell;
            Zayavka.kodPosetit = Convert.ToInt32(((TextBlock)cell.Content).Text);
            BlackList bl = new BlackList();
            bl.ShowDialog();
        }
    }
}
