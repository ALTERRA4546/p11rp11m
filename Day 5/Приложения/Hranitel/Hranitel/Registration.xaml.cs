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

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        //Регистрация посещающего
        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (famaly.Text == "" || name.Text == "" || otchestvo.Text == "" || phone.Text == "" || email.Text == "" || organisation.Text == "" || birthday.SelectedDate == null || seriesPasp.Text == "" || nomerPasp.Text == "" || password.Text == "")
                {
                    MessageBox.Show("Не все поля были заполнены");
                    return;
                }

                string fam = famaly.Text;
                string nam = name.Text;
                string otc = otchestvo.Text;
                string pho = phone.Text;
                if (!UInt64.TryParse(phone.Text, out _) || phone.Text.Length != 11)
                {
                    MessageBox.Show("Телефон введен в неправильном формате");
                    return;
                }

                pho = "+" + pho;
                pho = pho.Insert(2, "(");
                pho = pho.Insert(6, ")");
                pho = pho.Insert(10, "-");
                pho = pho.Insert(13, "-");

                string mail = email.Text;
                if (mail.IndexOf("@mail.ru") >= 0 || mail.IndexOf("@gmail.com") >= 0) ;
                else
                {
                    MessageBox.Show("Почта введена неверно укажите @mail.ru или @gmail.com");
                    return;
                }
                string log = AutoGenLogin1(mail);
                string organ = organisation.Text;
                DateTime birth = birthday.SelectedDate.Value;
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
                string pas = password.Text;

                int kode = 0;

                using (var bd = new HranitelProEntities())
                {
                    switch (ways.SelectedIndex)
                    {
                        case 0:
                            var org = new Организация();
                            var pasetit = new Посетитель();
                            var avtoris = new Авторизация();

                            var existorg = bd.Организация.FirstOrDefault(f => f.Название == organ);
                            if (existorg == null)
                            {
                                org.Название = organ;
                                bd.Организация.Add(org);
                                bd.SaveChanges();
                                var kodeorg = bd.Организация.OrderByDescending(f => f.Код_организации).FirstOrDefault();
                                kode = kodeorg.Код_организации;
                            }
                            else
                            {
                                kode = existorg.Код_организации;
                            }

                            pasetit.Фамилия = fam;
                            pasetit.Имя = nam;
                            pasetit.Отчество = otc;
                            pasetit.Номер_телефона = pho;
                            pasetit.Email = mail;
                            
                            pasetit.Дата_рождения = Convert.ToDateTime(birth).Date;
                            pasetit.Код_организации = kode;
                            pasetit.Серия_паспорта = ser;
                            pasetit.Номер_паспорта = nom;
                            bd.Посетитель.Add(pasetit);
                            bd.SaveChanges();

                            var kodepasetit = bd.Посетитель.OrderByDescending(f => f.Код_посетителя).FirstOrDefault();
                            kode = kodepasetit.Код_посетителя;

                            avtoris.Код_посетителя = kode;
                            //avtoris.Логин = log;
                            avtoris.Пароль = pas;
                            bd.Авторизация.Add(avtoris);
                            bd.SaveChanges();
                            break;
                        case 1:
                            var user1 = bd.Database.SqlQuery<HranitelProEntities>($@"DECLARE @Kode INT;
                        IF EXISTS (SELECT * FROM Организация WHERE Название = '{organ}')
		                SELECT @Kode = [Код организации] FROM Организация WHERE Название = '{organ}';
	                    ELSE
	                    BEGIN
		                INSERT INTO Организация VALUES ('{organ}')
		                SELECT TOP 1 @Kode = [Код организации] FROM Организация ORDER BY [Код организации] DESC;
	                    END
                        INSERT INTO Посетитель (Фамилия, Имя, Отчество, [Номер телефона], Email, [Дата рождения], [Код организации], [Серия паспорта], [Номер паспорта]) VALUES ('{fam}', '{nam}', '{otc}', '{pho}', '{mail}', '{birth}', @Kode, '{ser}', '{nom}')
	                    SELECT TOP 1 @Kode = [Код посетителя] FROM Посетитель ORDER BY [Код организации] DESC;
	                    INSERT INTO Авторизация ([Код посетителя], Пароль) VALUES (@Kode, '{pas}')").ToList();
                            break;

                        case 2:
                            var user2 = bd.Database.SqlQuery<HranitelProEntities>($@"EXEC Registration '{fam}', '{nam}', '{otc}', '{pho}', '{mail}', '{organ}', '{birth}', '{ser}', '{nom}', '{log}', '{pas}'").ToList();
                            break;
                    }
                    var newLogin = bd.Авторизация.OrderByDescending(s=>s.Код_авторизации).FirstOrDefault();
                    MessageBox.Show("Успешная регистрация\nВаш логин: " + newLogin.Логин);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Метод генерации уникального логина
        string AutoGenLogin1(string input)
        {
            string word = input.Replace("@mail.ru","").Replace("@gmail.com","");
            Random rnd = new Random();
            string sWord = "";
            char[] sCh = word.ToCharArray();
            Array.Sort(sCh, (a, b) => rnd.Next(-1, 5));
            sWord = new string(sCh) + rnd.Next(100,9999);
            return sWord;
        }

        //Метод генерации уникального логина
        string AutoGenLogin2(string input) 
        {
            string word = input.Replace("@mail.ru", "").Replace("@gmail.com", "");
            Random rnd = new Random();
            string sWord = word + rnd.Next(100, 9999);
            return sWord;
        }
    }
}
