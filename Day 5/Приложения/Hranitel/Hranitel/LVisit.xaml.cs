using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    /// Логика взаимодействия для LVisit.xaml
    /// </summary>
    public partial class LVisit : Window
    {
        public LVisit()
        {
            InitializeComponent();
            Initial();
        }

        //Статичный класс для передачи данных между формами
        public static class SelectSotrud
        {
            public static int Podrazdel { get; set; }
            public static int KodSotrud {get;set;}
            public static string Famaly {get;set;}
            public static string Name {get;set;}
            public static string Otchestvo {get;set;}
        }

        //Публичные переменные
        public string imagePath;
        public string filePath;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Добавление данных в ComboBox из таблицы подразделения
        private void Initial()
        {
            using (var bd = new HranitelProEntities())
            {
                var podraz = bd.Подразделение.Select(f=>f.Назавние);
                podrazdel.ItemsSource = podraz.ToList();
                podrazdel.SelectedIndex = 0;
            }
        }

        //Очистка формы
        private void clearForm_Click(object sender, RoutedEventArgs e)
        {
            target.Clear();
            fio.Clear();
            famaly.Clear();
            name.Clear();
            otchestvo.Clear();
            phone.Clear();
            email.Clear();
            organisation.Clear();
            primechanie.Clear();
            seriesPasp.Clear();
            nomerPasp.Clear();
        }

        //Отправка данных в базу данных
        private void sendData_Click(object sender, RoutedEventArgs e)
        {
            if (famaly.Text == "" || name.Text == "" || otchestvo.Text == "" || phone.Text == "" || email.Text == "" || organisation.Text == "" || birthday.SelectedDate == null || seriesPasp.Text == "" || nomerPasp.Text == "" || fromDate.SelectedDate == null || toDate.SelectedDate == null || target.Text == "" || podrazdel.SelectedIndex < 0 || fio.Text == "")
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }

            try
            {
                DateTime fd = fromDate.SelectedDate.Value;
                DateTime td = toDate.SelectedDate.Value;
                string trarg = target.Text;
                string famimot = fio.Text;
                string fam = famaly.Text;
                string nam = name.Text;
                string otc = otchestvo.Text;
                string phon = phone.Text;
                if (!UInt64.TryParse(phone.Text, out _) || phone.Text.Length != 11)
                {
                    MessageBox.Show("Телефон введен в неправильном формате");
                    return;
                }

                phon = "+" + phon;
                phon = phon.Insert(2, "(");
                phon = phon.Insert(6, ")");
                phon = phon.Insert(10, "-");
                phon = phon.Insert(13, "-");

                string mail = email.Text;
                if (mail.IndexOf("@mail.ru") >= 0 || mail.IndexOf("@gmail.com") >= 0) ;
                else
                {
                    MessageBox.Show("Почта введена неверно укажите @mail.ru или @gmail.com");
                    return;
                }
                string organ = organisation.Text;
                string primec = primechanie.Text;
                DateTime birt = birthday.SelectedDate.Value;
                if (!int.TryParse(seriesPasp.Text, out _))
                {
                    MessageBox.Show("Серия паспорта введен в неправильном формате");
                    return;
                }
                int ser = Convert.ToInt32(seriesPasp.Text);
                if (!int.TryParse(nomerPasp.Text, out _))
                {
                    MessageBox.Show("Номер паспорта введен в неправильном формате");
                    return;
                }
                int nom = Convert.ToInt32(nomerPasp.Text);
                int kodeOrgan = 0;

                using (var bd = new HranitelProEntities())
                {
                    var propusk = new Пропуск();
                    var organizat = new Организация();
                    var posetitel = new Посетитель();
                    var poseshayshiy = new Посещающие();
                    var zayavka = new Заявка();

                    //Создание пропуска
                    propusk.Срок_начала_действия = fd;
                    propusk.Срок_окончания_действия = td;
                    propusk.Цель_посещения = trarg;
                    bd.Пропуск.Add(propusk);
                    bd.SaveChanges();

                    var kodeprop = bd.Пропуск.OrderByDescending(f => f.Код_пропуска).FirstOrDefault();

                    var or = bd.Организация.FirstOrDefault(f => f.Название == organ);
                    if (or == null)
                    {
                        organizat.Название = organ;
                        bd.Организация.Add(organizat);
                        bd.SaveChanges();
                        var kodeorgan = bd.Организация.OrderByDescending(f => f.Код_организации).FirstOrDefault();
                        kodeOrgan = kodeorgan.Код_организации;
                    }
                    else
                    {
                        kodeOrgan = or.Код_организации;
                    }

                    //Создание посетителя
                    posetitel.Фамилия = fam;
                    posetitel.Имя = nam;
                    posetitel.Отчество = otc;
                    posetitel.Номер_телефона = phon;
                    posetitel.Email = mail;
                    posetitel.Дата_рождения = birt;
                    posetitel.Код_организации = kodeOrgan;
                    posetitel.Серия_паспорта = ser;
                    posetitel.Номер_паспорта = nom;
                    if (File.Exists(imagePath))
                    {
                        posetitel.Фотография = ImageConvert(imagePath);
                    }
                    if (File.Exists(filePath))
                    {
                        posetitel.Скан_паспорта = ImageConvert(filePath);
                    }
                    bd.Посетитель.Add(posetitel);
                    bd.SaveChanges();

                    var kodeposetit = bd.Посетитель.OrderByDescending(f => f.Код_посетителя).FirstOrDefault();
                    var kodepodraz = bd.Подразделение.FirstOrDefault(s => s.Назавние == podrazdel.SelectedItem.ToString());
                    var kodesotrud = bd.Сотрудник.FirstOrDefault(s => s.Код_сотрудника == SelectSotrud.KodSotrud);

                    //Создание заявки
                    zayavka.Код_типа_заявки = 1;
                    zayavka.Код_пропуска = kodeprop.Код_пропуска;
                    zayavka.Код_подразделения = kodepodraz.Код_подразделения;
                    zayavka.Код_сотрудника = kodesotrud.Код_сотрудника;
                    zayavka.Дата_создания = DateTime.Now.Date;
                    zayavka.Дата_и_время_посящения = DateTime.Now.AddDays(2);
                    zayavka.Код_статуса = 1;
                    bd.Заявка.Add(zayavka);
                    bd.SaveChanges();

                    var kodezayavka = bd.Заявка.OrderByDescending(z => z.Код_заявки).FirstOrDefault();

                    //Создание посещения
                    poseshayshiy.Код_заявки = kodezayavka.Код_заявки;
                    poseshayshiy.Код_посетителя = kodeposetit.Код_посетителя;
                    bd.Посещающие.Add(poseshayshiy);
                    bd.SaveChanges();

                    MessageBox.Show("Данные сохранены");
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Ошибка: " + ex);
            }
        }

        //Выбор сотрудника
        private void selectSotrud_Click(object sender, RoutedEventArgs e)
        {
            SelectedSotrudnic ss = new SelectedSotrudnic();
            using (var bd = new HranitelProEntities())
            {
                var podraz = bd.Подразделение.FirstOrDefault(s=>s.Назавние == podrazdel.SelectedItem.ToString());
                SelectSotrud.Podrazdel = podraz.Код_подразделения;
            }
            ss.ShowDialog();
            fio.Text = SelectSotrud.Famaly+" "+SelectSotrud.Name+" "+SelectSotrud.Otchestvo;
        }

        //Конвертация файлов для их загрузки в бд
        byte[] ImageConvert(string filePath)
        {
            byte[] imageData = File.ReadAllBytes(filePath);  
            return imageData;
        }

        //Диалог выбора изображения
        private void uploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "image(*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (fd.ShowDialog() == true)
            {
                imagePath = fd.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(fd.FileName));
                imag.Source = imageSource;
            }
        }

        //Диалог выбора прикрепляемого файла
        private void selectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "pdf(*.pdf)|*.pdf";
            if (fd.ShowDialog() == true)
            {
                filePath = fd.FileName;
            }
        }
    }
}
