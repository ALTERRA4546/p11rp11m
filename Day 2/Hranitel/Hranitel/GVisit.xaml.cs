using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Hranitel.LVisit;

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для GVisit.xaml
    /// </summary>
    public partial class GVisit : Window
    {
        public GVisit()
        {
            InitializeComponent();
            Initial();
        }

        public string imagePath;
        public string filePath;
        public int kodePaset = -1;
        public List<Посетитель> poset;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Initial()
        {
            poset = new List<Посетитель>();
            using (var bd = new HranitelProEntities())
            {
                var podraz = bd.Подразделение.Select(f => f.Назавние);
                podrazdel.ItemsSource = podraz.ToList();
            }
        }

        public int Group = -1;

        private void clearForm_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            poset.Clear();
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
            Group = -1;
            kodePaset = -1;
        }

        private void sendData_Click(object sender, RoutedEventArgs e)
        {
            if (famaly.Text == "" || name.Text == "" || otchestvo.Text == "" || phone.Text == "" || email.Text == "" || organisation.Text == "" || birthday.SelectedDate == null || seriesPasp.Text == "" || nomerPasp.Text == "" || fromDate.SelectedDate == null || toDate.SelectedDate == null || target.Text == "" || podrazdel.SelectedIndex < 0 || fio.Text == "")
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }

            DateTime fd = fromDate.SelectedDate.Value;
            DateTime td = toDate.SelectedDate.Value;
            string trarg = target.Text;
            string famimot = fio.Text;
            string fam = famaly.Text;
            string nam = name.Text;
            string otc = otchestvo.Text;
            string phon = phone.Text;
            if (!UInt64.TryParse(phon, out _))
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

            using (var bd = new HranitelProEntities())
            {
                var propusk = new Пропуск();
                var zayavka = new Заявка();
                var poseshaush = new Посещающие();

                propusk.Срок_начала_действия = fd;
                propusk.Срок_окончания_действия = td;
                propusk.Цель_посещения = trarg;
                bd.Пропуск.Add(propusk);
                bd.SaveChanges();

                var kodeprop = bd.Пропуск.OrderByDescending(f => f.Код_пропуска).FirstOrDefault();
                var kodepodraz = bd.Подразделение.FirstOrDefault(s => s.Назавние == podrazdel.SelectedItem.ToString());
                var kodesotrud = bd.Сотрудник.FirstOrDefault(s => s.Код_сотрудника == SelectSotrud.KodSotrud);

                zayavka.Код_типа_заявки = 2;
                zayavka.Код_пропуска = kodeprop.Код_пропуска;
                zayavka.Код_подразделения = kodepodraz.Код_подразделения;
                zayavka.Код_сотрудника = kodesotrud.Код_сотрудника;
                zayavka.Дата_создания = DateTime.Now.Date;
                zayavka.Дата_и_время_посящения = DateTime.Now.AddDays(2);
                zayavka.Код_статуса = 1;
                bd.Заявка.Add(zayavka);
                bd.SaveChanges();
                var kodezayavka = bd.Заявка.OrderByDescending(z => z.Код_заявки).FirstOrDefault();

                foreach (var element in poset)
                {
                    bd.Посетитель.Add(element);
                    bd.SaveChanges();
                    var lastKode = bd.Посетитель.OrderByDescending(s => s.Код_посетителя).FirstOrDefault();
                    poseshaush.Код_заявки = kodezayavka.Код_заявки;
                    poseshaush.Код_посетителя = lastKode.Код_посетителя;
                    poseshaush.Код_группы = Group;
                    bd.Посещающие.Add(poseshaush);
                    bd.SaveChanges();
                }
            }
            Clear();
            dataOut.ItemsSource = null;
            MessageBox.Show("Данные сохранены");
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            if (famaly.Text == "" || name.Text == "" || otchestvo.Text == "" || phone.Text == "" || email.Text == "" || organisation.Text == "" || birthday.SelectedDate == null || seriesPasp.Text == "" || nomerPasp.Text == "" || fromDate.SelectedDate == null || toDate.SelectedDate == null || target.Text == "" || podrazdel.SelectedIndex < 0 || fio.Text == "")
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }

            DateTime fd = fromDate.SelectedDate.Value;
            DateTime td = toDate.SelectedDate.Value;
            string trarg = target.Text;
            string famimot = fio.Text;
            string fam = famaly.Text;
            string nam = name.Text;
            string otc = otchestvo.Text;
            if (!UInt64.TryParse(phone.Text, out _))
            {
                MessageBox.Show("Телефон введен в неправильном формате");
                return;
            }
            string phon = phone.Text;

            phon = "+" + phon;
            phon = phon.Insert(2, "(");
            phon = phon.Insert(6, ")");
            phon = phon.Insert(10, "-");
            phon = phon.Insert(13, "-");

            string mail = email.Text;
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

                var group = new Группа();
                var organizat = new Организация();
                var posetitel = new Посетитель();

                if (Group < 0)
                {
                    var kodesotrud = bd.Сотрудник.FirstOrDefault(s => s.Код_сотрудника == SelectSotrud.KodSotrud);
                    var kodegroup = bd.Группа.OrderByDescending(g => g.Код_группы).FirstOrDefault();
                    group.Дата_создания = DateTime.Now;
                    group.Код_сотрудника = kodesotrud.Код_сотрудника;
                    group.Название = "ГР" + (kodegroup.Код_группы + 1);
                    bd.Группа.Add(group);
                    bd.SaveChanges();
                    var kodegroupupdate = bd.Группа.OrderByDescending(g => g.Код_группы).FirstOrDefault();
                    Group = kodegroupupdate.Код_группы;
                }

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

                if (kodePaset < 0)
                {
                    var kodeposetit = bd.Посетитель.OrderByDescending(f => f.Код_посетителя).FirstOrDefault();
                    kodePaset = kodeposetit.Код_посетителя + 1;
                }
                else
                    kodePaset++;

                posetitel.Код_посетителя = kodePaset;
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

                poset.Add(posetitel);

                bd.SaveChanges();
                Update();
            }
        }

        private void selectSotrud_Click(object sender, RoutedEventArgs e)
        {
            SelectedSotrudnic ss = new SelectedSotrudnic();
            using (var bd = new HranitelProEntities())
            {
                var podraz = bd.Подразделение.FirstOrDefault(s => s.Назавние == podrazdel.SelectedItem.ToString());
                SelectSotrud.Podrazdel = podraz.Код_подразделения;
            }
            ss.ShowDialog();
            fio.Text = SelectSotrud.Famaly + " " + SelectSotrud.Name + " " + SelectSotrud.Otchestvo;
        }

        private void Update()
        {
            using (var bd = new HranitelProEntities())
            {
                var group = poset.Select(s=> new { s.Код_посетителя, ФИО = s.Фамилия+" "+s.Имя+" "+s.Отчество, s.Номер_телефона});
                dataOut.ItemsSource = group.ToList();
            }
        }

        byte[] ImageConvert(string filePath)
        {
            byte[] imageData = File.ReadAllBytes(filePath);
            return imageData;
        }

        private void uploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "image(*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (fd.ShowDialog() == true)
            {
                imagePath = fd.FileName;
            }
        }

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
