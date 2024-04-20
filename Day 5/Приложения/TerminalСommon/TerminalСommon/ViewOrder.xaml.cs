using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace TerminalСommon
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
            GenReport2();
            //System.Timers.Timer timer = new System.Timers.Timer(10800000);
            //timer.Elapsed += TimerElapsed;
            //timer.Start();
        }

        /*private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GenReport();
        }*/

        //Публичный класс для передачи кода заявки между окнами
        public static class Zayavka
        { 
            public static int kodZayavky;
        }

        //Заполнение DataGrid заявками и заполнение ComboBox-во данными которые будут фильтроваться
        private void Initial()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    var ty = bd.Тип_заявки.Select(s => s.Название).ToList();
                    var pd = bd.Подразделение.Select(s => s.Назавние).ToList();
                    var st = bd.Статус.Select(s => s.Название).ToList();
                    ty.Add("Все");
                    pd.Add("Все");
                    st.Add("Все");
                    type.ItemsSource = ty;
                    podrazd.ItemsSource = pd;
                    status.ItemsSource = st;
                    type.SelectedItem = "Все";
                    podrazd.SelectedItem = "Все";
                    status.SelectedItem = "Все";

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Предварительное распределение сортируемиых параметров
        private void Filter(string tp, string pod, string stat)
        {
            string t = null;
            string p = null;
            string s = null;
            if (tp != "Все")
                t = tp;
            if (pod != "Все")
                p = pod;
            if (stat != "Все")
                s = stat;
            Filtration(t, p, s);
        }

        //Фильтрация записей
        private void Filtration(string tp, string pod, string stat)
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
                                  where ((tp == null || t.Название == tp) && (pod == null || pdr.Назавние == pod) && (stat == null || s.Название == stat))
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
                    dataOut.ItemsSource = zayavka.ToList();
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

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (type.SelectedIndex < 0 || podrazd.SelectedIndex < 0 || status.SelectedIndex < 0)
            {
                return;
            }
            Filter(type.SelectedItem.ToString(), podrazd.SelectedItem.ToString(), status.SelectedItem.ToString());
        }

        private void report_Click(object sender, RoutedEventArgs e)
        {
            SelectReport sr = new SelectReport();
            sr.Show();
        }

        /*private void GenReport()
        {
            using (var bd = new HranitelProEntities())
            {
                var zayavka = from z in bd.Заявка
                              join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                              join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                              join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                              group pos by podr into g
                              select new
                              {
                                  Подразделение = g.Key.Назавние,
                                  Количество_посещающих = g.Count()
                              };


                if (!Directory.Exists("Отчеты ТБ"))
                {
                    Directory.CreateDirectory("Отчеты ТБ");
                }

                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($@"Отчеты ТБ\{DateTime.Now.TimeOfDay}.pdf".Replace(':','_'), FileMode.Create));

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12);

                doc.Open();
                PdfPTable table = new PdfPTable(2);

                table.AddCell(new Paragraph("Подразделение", font));
                table.AddCell(new Paragraph("Количество посещающих",font));

                foreach (var item in zayavka)
                {
                    table.AddCell(new Paragraph(item.Подразделение.ToString(),font));
                    table.AddCell(new Paragraph(item.Количество_посещающих.ToString(),font));
                }
                
                doc.Add(table);
                doc.Close();
            }
        }*/

        //Создание отчета о количестве поситителей каждые 3 часа в папке Отчеты ТБ
        private void GenReport2()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    var zayavka = from z in bd.Заявка
                                  join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                  join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                  let visitTime = z.Дата_и_время_посящения
                                  let intervalStart = new
                                  {
                                      Hour = visitTime.Value.Hour / 3 * 3,
                                      Minute = 0,
                                      Second = 0
                                  }
                                  group pos by new { podr.Назавние, Interval = intervalStart } into g
                                  select new
                                  {
                                      Подразделение = g.Key.Назавние,
                                      Интервал_времени = g.Key.Interval.Hour,
                                      Количество_посещающих = g.Count()
                                  };


                    if (!Directory.Exists("Отчеты ТБ"))
                    {
                        Directory.CreateDirectory("Отчеты ТБ");
                    }

                    Document doc = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($@"Отчеты ТБ\{DateTime.Now}.pdf".Replace(':', '_'), FileMode.Create));

                    BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(baseFont, 12);

                    doc.Open();
                    PdfPTable table = new PdfPTable(3);

                    table.AddCell(new Paragraph("Время", font));
                    table.AddCell(new Paragraph("Подразделение", font));
                    table.AddCell(new Paragraph("Количество посещающих", font));

                    foreach (var item in zayavka)
                    {
                        table.AddCell(new Paragraph(item.Интервал_времени.ToString(), font));
                        table.AddCell(new Paragraph(item.Подразделение.ToString(), font));
                        table.AddCell(new Paragraph(item.Количество_посещающих.ToString(), font));
                    }

                    doc.Add(table);
                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
