﻿using System;
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
    /// Логика взаимодействия для CheckOrder.xaml
    /// </summary>
    public partial class CheckOrder : Window
    {
        public CheckOrder()
        {
            InitializeComponent();
            Initial();
        }

        //Заполнение DataGrid выбранными данными о заявках и посетителях, а также установка значний в TextBox
        private void Initial()
        {
            try
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
                                      Код_сотрудника = sotr.Код_авторизации,
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

                    var stat = bd.Статус.Select(s => s.Название);
                    var zayavData = bd.Заявка.FirstOrDefault(s => s.Код_заявки == Zayavka.kodZayavky);
                    var statusKod = bd.Статус.FirstOrDefault(s => s.Код_cтатуса == zayavData.Код_статуса);
                    if (zayavData.Время_выхода != null)
                    {
                        timepickfrom.Text = zayavData.Время_входа.ToString();
                        timepickto.Text = zayavData.Время_выхода.ToString();
                    }
                    else
                    {
                        timepickfrom.Text = "0:00:00";
                        timepickto.Text = "0:00:00";
                    }

                    dataOrder.ItemsSource = zayavka.ToList();
                    dataPoset.ItemsSource = posetit.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Сохранение изменений в завке
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    var zayavka = bd.Заявка.FirstOrDefault(z => z.Код_заявки == Zayavka.kodZayavky);
                    string[] checkData = timepickfrom.Text.Split(':');
                    if ((Convert.ToInt32(checkData[0]) < 0 || Convert.ToInt32(checkData[0]) > 24) || (Convert.ToInt32(checkData[1]) < 0 || Convert.ToInt32(checkData[1]) > 60) || (Convert.ToInt32(checkData[2]) < 0 || Convert.ToInt32(checkData[2]) > 60))
                    {
                        MessageBox.Show("Время указано неверно");
                        return;
                    }
                    string[] checkData1 = timepickto.Text.Split(':');
                    if ((Convert.ToInt32(checkData1[0]) < 0 || Convert.ToInt32(checkData1[0]) > 24) || (Convert.ToInt32(checkData1[1]) < 0 || Convert.ToInt32(checkData1[1]) > 60) || (Convert.ToInt32(checkData1[2]) < 0 || Convert.ToInt32(checkData1[2]) > 60))
                    {
                        MessageBox.Show("Время указано неверно");
                        return;
                    }

                    zayavka.Время_входа = Convert.ToDateTime(timepickfrom.Text).TimeOfDay;
                    zayavka.Время_выхода = Convert.ToDateTime(timepickto.Text).TimeOfDay;
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
