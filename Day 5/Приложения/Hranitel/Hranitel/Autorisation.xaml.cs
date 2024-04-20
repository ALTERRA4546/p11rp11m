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

namespace Hranitel
{
    /// <summary>
    /// Логика взаимодействия для Autorisation.xaml
    /// </summary>
    public partial class Autorisation : Window
    {
        public Autorisation()
        {
            InitializeComponent();
        }

        public static class PosetData
        {
            public static int posetKode {  get; set; }
        }

        //Авторизация пользователя
        private void EnterB_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" || password.Text == "")
            {
                MessageBox.Show("Поля не заполнены");
                return;
            }

            string log = login.Text;
            string pas = password.Text;

            using (var bd = new HranitelProEntities())
            {
                switch (ways.SelectedIndex)
                {
                    case 0:
                        var user1 = bd.Авторизация.FirstOrDefault(f=> f.Логин == log && f.Пароль == pas);
                        if (user1 != null)
                        {
                            PosetData.posetKode = user1.Код_посетителя;
                            ViewVisit vv = new ViewVisit();
                            vv.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверно введен логин или пароль");
                        }
                        break;

                    case 1:
                        var user2 = bd.Database.SqlQuery<HranitelProEntities>($@"SELECT * FROM Авторизация WHERE Логин = '{log}' AND Пароль = '{pas}'").ToList();
                        if (user2.Count != 0)
                        {
                            var user21 = bd.Авторизация.FirstOrDefault(f => f.Логин == log && f.Пароль == pas);
                            PosetData.posetKode = user21.Код_посетителя;
                            ViewVisit vv = new ViewVisit();
                            vv.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверно введен логин или пароль");
                        }
                        break;

                    case 2:
                        var user3 = bd.Database.SqlQuery<HranitelProEntities>($@"EXEC Autorisation '{log}', '{pas}'").ToList();
                        if (user3.Count != 0)
                        {
                            var user31 = bd.Авторизация.FirstOrDefault(f => f.Логин == log && f.Пароль == pas);
                            PosetData.posetKode = user31.Код_посетителя;
                            ViewVisit vv = new ViewVisit();
                            vv.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неверно введен логин или пароль");
                        }
                        break;
                }
            }
        }

        //Переход на форму регистрации
        private void RegistartionB_Click(object sender, RoutedEventArgs e)
        {
            Registration r = new Registration();
            r.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
