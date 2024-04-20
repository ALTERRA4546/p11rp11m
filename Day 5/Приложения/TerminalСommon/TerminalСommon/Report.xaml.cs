using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;

namespace TerminalСommon
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public Report()
        {
            InitializeComponent();
            Initial();
        }

        //Предварительная выборка данных за день и заполнение ComboBox
        private void Initial()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    List<string> kol = new List<string>();
                    kol.Add("День");
                    kol.Add("Месяц");
                    kol.Add("Год");
                    type.ItemsSource = kol;
                    type.SelectedIndex = 0;

                    var zayavka = from z in bd.Заявка
                                  join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                  join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                  group pos by new { z.Дата_и_время_посящения.Value.Day } into g
                                  select new
                                  {
                                      День_посещения = g.Key.Day,
                                      Количество_посещающих = g.Count()
                                  };
                    dataOut.ItemsSource = zayavka.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Заполнение DataGrid по искомым занчениям
        private void rep()
        {
            try
            {
                using (var bd = new HranitelProEntities())
                {
                    //Проверка группировки
                    if (groupCh.IsChecked == false)
                    {
                        //Выборка данных без группировки по подразделениям
                        switch (type.SelectedIndex)
                        {
                            //Заполнение DataGrid количеством посетителей по дням
                            case 0:
                                var zayavka = from z in bd.Заявка
                                              join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                              join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                              group pos by new { z.Дата_и_время_посящения.Value.Day } into g
                                              select new
                                              {
                                                  День_посещения = g.Key.Day,
                                                  Количество_посещающих = g.Count()
                                              };
                                dataOut.ItemsSource = zayavka.ToList();
                                break;
                            //Заполнение DataGrid количеством посетителей по месяцам
                            case 1:
                                var zayavka1 = from z in bd.Заявка
                                               join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                               join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                               group pos by new { z.Дата_и_время_посящения.Value.Month } into g
                                               select new
                                               {
                                                   Месяц = g.Key.Month,
                                                   Количество_посещающих = g.Count()
                                               };
                                dataOut.ItemsSource = zayavka1.ToList();
                                break;
                            //Заполнение DataGrid количеством посетителей по годам
                            case 2:
                                var zayavka2 = from z in bd.Заявка
                                               join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                               join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                               group pos by new { z.Дата_и_время_посящения.Value.Year } into g
                                               select new
                                               {
                                                   Год = g.Key.Year,
                                                   Количество_посещающих = g.Count()
                                               };
                                dataOut.ItemsSource = zayavka2.ToList();
                                break;
                        }
                    }
                    //Выборка данных с группировкой по подразделениям
                    else
                    {
                        switch (type.SelectedIndex)
                        {
                            //Заполнение DataGrid количеством посетителей по дням
                            case 0:
                                var zayavka = from z in bd.Заявка
                                              join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                              join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                              join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                              group pos by new { z.Дата_и_время_посящения.Value.Day, podr.Назавние } into g
                                              select new
                                              {
                                                  День_посещения = g.Key.Day,
                                                  Подразделение = g.Key.Назавние,
                                                  Количество_посещающих = g.Count()
                                              };
                                dataOut.ItemsSource = zayavka.ToList();
                                break;
                            //Заполнение DataGrid количеством посетителей по месяцам
                            case 1:
                                var zayavka1 = from z in bd.Заявка
                                               join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                               join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                               join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                               group pos by new { z.Дата_и_время_посящения.Value.Month, podr.Назавние } into g
                                               select new
                                               {
                                                   Месяц = g.Key.Month,
                                                   Подразделение = g.Key.Назавние,
                                                   Количество_посещающих = g.Count()
                                               };
                                dataOut.ItemsSource = zayavka1.ToList();
                                break;
                            //Заполнение DataGrid количеством посетителей по годам
                            case 2:
                                var zayavka2 = from z in bd.Заявка
                                               join podr in bd.Подразделение on z.Код_подразделения equals podr.Код_подразделения
                                               join pos in bd.Посещающие on z.Код_заявки equals pos.Код_заявки
                                               join sotr in bd.Сотрудник on z.Код_сотрудника equals sotr.Код_сотрудника
                                               group pos by new { z.Дата_и_время_посящения.Value.Year, podr.Назавние } into g
                                               select new
                                               {
                                                   Год = g.Key.Year,
                                                   Подразделение = g.Key.Назавние,
                                                   Количество_посещающих = g.Count()
                                               };
                                dataOut.ItemsSource = zayavka2.ToList();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.DefaultExt = ".pdf";
            fd.Filter = "PDF files (*.pdf)|*.pdf";
            if (fd.ShowDialog() == true)
            {
                SaveToPdf(dataOut, fd.FileName);
            }
        }

        private void SaveToPdf(DataGrid dataGrid, string fileName)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            document.Open();

            PdfPTable pdfTable = new PdfPTable(dataGrid.Columns.Count);
            for (int j = 0; j < dataGrid.Columns.Count; j++)
            {
                pdfTable.AddCell(new Phrase(dataGrid.Columns[j].Header.ToString()));
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) is TextBlock)
                    {
                        TextBlock cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock;
                        pdfTable.AddCell(new Phrase(cellContent.Text));
                    }
                }
            }

            document.Add(pdfTable);
            document.Close();
        }*/

        private void type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rep();
        }

        private void groupCh_Click(object sender, RoutedEventArgs e)
        {
            rep();
        }
    }
}
