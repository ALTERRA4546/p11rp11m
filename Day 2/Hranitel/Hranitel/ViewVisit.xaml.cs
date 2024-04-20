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
using static System.Windows.Forms.AxHost;

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для ViewVisit.xaml
    /// </summary>
    public partial class ViewVisit : Window
    {
        public ViewVisit()
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
                                  z.Дата_и_время_посящения,
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
                dataOut.ItemsSource = zayavka.ToList();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
