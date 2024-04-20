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

namespace TerminalSecurity
{
    /// <summary>
    /// Логика взаимодействия для ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        public ViewOrder()
        {
            InitializeComponent();
            Initial();
        }

        public static class Zayavka
        {
            public static int kodZayavky;
        }

        private void Initial()
        {
            using (var bd = new HranitelProEntities())
            {
                string stat = "одобрена";
                var ty = bd.Тип_заявки.Select(s => s.Название).ToList();
                var pd = bd.Подразделение.Select(s => s.Назавние).ToList();
                ty.Add("Все");
                pd.Add("Все");
                type.ItemsSource = ty;
                podrazd.ItemsSource = pd;
                type.SelectedItem = "Все";
                podrazd.SelectedItem = "Все";

                var zayavka = from z in bd.Заявка
                              join t in bd.Тип_заявки on z.Код_типа_заявки equals t.Код_типа_заявки
                              join s in bd.Статус on z.Код_статуса equals s.Код_cтатуса
                              join prp in bd.Пропуск on z.Код_пропуска equals prp.Код_пропуска
                              join pdr in bd.Подразделение on z.Код_подразделения equals pdr.Код_подразделения
                              join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                              join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                              join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                              join org in bd.Организация on poset.Код_организации equals org.Код_организации
                              where(s.Название == stat)
                              select new
                              {
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
            }
        }

        private void Filter(string tp, string pod, string fi)
        {
            string t = null;
            string p = null;
            string f = null;
            DatePicker d = dateCr;
            if (tp != "Все")
                t = tp;
            if (pod != "Все")
                p = pod;
            if (fi != "")
                f = fi;
            FiltrationOnDate(t, p, d, f);
        }

        private void FiltrationOnDate(string tp, string pod, DatePicker dt, string fi)
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
                              where ((tp == null || t.Название == tp) && (pod == null || pdr.Назавние == pod) && (dt.SelectedDate == null || z.Дата_создания == dt.SelectedDate.Value) && (s.Название == stat) && (fi == null || poset.Фамилия.Contains(fi) || poset.Имя.Contains(fi) || poset.Отчество.Contains(fi) || poset.Номер_паспорта.ToString().Contains(fi)))
                              select new
                              {
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
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

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

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (type.SelectedIndex < 0 || podrazd.SelectedIndex < 0)
                return;
            Filter(type.SelectedItem.ToString(), podrazd.SelectedItem.ToString(), find.Text);
        }

        private void type_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dateCrCheck.IsChecked == false)
            {
                dateCr.SelectedDate = null;
            }
            Filter(type.SelectedItem.ToString(), podrazd.SelectedItem.ToString(), find.Text);
        }

        private void find_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Filter(type.SelectedItem.ToString(), podrazd.SelectedItem.ToString(), find.Text);
            }
        }
    }
}
