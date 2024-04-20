using System;
using System.Collections.Generic;
using System.Data;
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
using static TerminalСommon.ViewOrder;

namespace TerminalСommon
{
    /// <summary>
    /// Логика взаимодействия для CheckOrder.xaml
    /// </summary>
    public partial class CheckOrder : Window
    {
        public CheckOrder()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
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
                              where (z.Код_заявки == Zayavka.kodZayavky)
                              select new
                              {
                                  z.Код_заявки,
                                  Тип_заявки = t.Название,
                                  Статус = s.Название,
                                  Подразделение = pdr.Назавние,
                                  sotr.Код_сотрудника,
                                  prp.Код_пропуска,
                                  prp.Срок_начала_действия,
                                  prp.Срок_окончания_действия,
                                  prp.Цель_посещения,
                                  z.Дата_создания,
                                  z.Дата_и_время_посящения
                              };

                var posetit = from z in bd.Заявка
                              join t in bd.Тип_заявки on z.Код_типа_заявки equals t.Код_типа_заявки
                              join s in bd.Статус on z.Код_статуса equals s.Код_cтатуса
                              join prp in bd.Пропуск on z.Код_пропуска equals prp.Код_пропуска
                              join pdr in bd.Подразделение on z.Код_подразделения equals pdr.Код_подразделения
                              join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                              join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                              join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                              join org in bd.Организация on poset.Код_организации equals org.Код_организации
                              where (z.Код_заявки == Zayavka.kodZayavky)
                              select new
                              {
                                  poset.Код_посетителя,
                                  poset.Фамилия,
                                  poset.Имя,
                                  poset.Отчество,
                                  poset.Номер_телефона,
                                  poset.Email,
                                  poset.Серия_паспорта,
                                  poset.Номер_паспорта,
                                  Организация = org.Название,
                                  poset.Дата_рождения,
                                  poset.Примечания
                              };

                var stat = bd.Статус.Select(s=>s.Название);
                var zayavData = bd.Заявка.FirstOrDefault(s=> s.Код_заявки == Zayavka.kodZayavky);
                var statusKod = bd.Статус.FirstOrDefault(s => s.Код_cтатуса == zayavData.Код_статуса);
                if (zayavData.Дата_и_время_посящения != null)
                {
                    string[] selectdate = zayavData.Дата_и_время_посящения.ToString().Split(' ');
                    string[] temp = selectdate[0].Split('.');
                    datepick.SelectedDate = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[0]));
                    timepick.Text = selectdate[1];
                }
                else
                {
                    datepick.SelectedDate = new DateTime(2024,01,01);
                    timepick.Text = "0:00:00";
                }
                status.ItemsSource = stat.ToList();
                status.SelectedItem = statusKod.Название;
                dataOrder.ItemsSource = zayavka.ToList();
                dataPoset.ItemsSource = posetit.ToList();

                var blackList = from z in bd.Заявка
                                join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                join poset in bd.Посетитель on pos.Код_посетителя equals poset.Код_посетителя
                                join bl in bd.Черный_список on pos.Код_посетителя equals bl.Код_посетителя
                                where (z.Код_заявки == Zayavka.kodZayavky)
                                select new
                                {
                                    bl.Код_посетителя
                                };

                if (blackList.ToList().Count > 0)
                {
                    MessageBox.Show("Посетитель в черном списке");
                    status.SelectedItem = "не одобрена";
                    status.IsReadOnly = true;
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    var zayavka = bd.Заявка.FirstOrDefault(z => z.Код_заявки == Zayavka.kodZayavky);
                    string[] temp = datepick.SelectedDate.ToString().Split(' ');
                    zayavka.Дата_и_время_посящения = Convert.ToDateTime(temp[0] + " " + timepick.Text);
                    var stat = bd.Статус.FirstOrDefault(s => s.Название == status.SelectedItem);
                    zayavka.Код_статуса = stat.Код_cтатуса;
                    bd.SaveChanges();
                    MessageBox.Show("Данные сохранены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex);
            }
        }
    }
}
