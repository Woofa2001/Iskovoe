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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

namespace Iskovoe.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProvonorPage.xaml
    /// </summary>
    public partial class AddProvonorPage : System.Windows.Controls.Page
    {
        public int buf_id;
        public AddProvonorPage(int buf_id_iscovoe)
        {
            InitializeComponent();
            TipFormComboBox.ItemsSource = SourceCore.DB.Tip_forms.ToList();
            SostavComboBox.ItemsSource = SourceCore.DB.Sostav.ToList();
            DataGridPravonor.ItemsSource = SourceCore.DB.Pravonor.Where(id_pravonor => id_pravonor.id_pravonor == buf_id_iscovoe).ToList();
            buf_id = buf_id_iscovoe;
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddPravonorButton_Click(object sender, RoutedEventArgs e)
        {
            //var 
            var A = new Data.Pravonor();
            A.id_iskovoe = buf_id;
            A.Tip_forms = (Data.Tip_forms)TipFormComboBox.SelectedItem;
            A.Sostav = (Data.Sostav)SostavComboBox.SelectedItem;
            //A.Period.month = (Data.Period)MonthComboBox.SelectedItem;
            //A.Period.year = int.Parse(YearTextBox.Text);
            A.summa = decimal.Parse(SumTextBox.Text);
            //A.Period.last_date = (Data.Debtors)DataGridDeptors.SelectedItem;
            //A.Period.last_date = LastDatePicker.SelectedDate;
            SourceCore.DB.Pravonor.Add(A);
            // Сохранение изменений
            SourceCore.DB.SaveChanges();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDocument();
        }

        //Create document method  
        private void CreateDocument()
        {
            try
            {
                //Create an instance for word app  
                Word.Application winword = new Word.Application();

                //Set animation status for word application  
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.  
                winword.Visible = false;

                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;

                object template = Type.Missing;
                string path = Directory.GetCurrentDirectory();

                //Меняем шаблон
                template = @"" + path + "\\Document\\Iscovoe.docx";

                //Создаем документ 
                Word.Document document = winword.Documents.Add(ref template, ref missing, ref missing, ref missing);


                ////Create a new document  
                //Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add header into the document  
                foreach (Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 10;
                    headerRange.Text = "Header text goes here";
                }

                //Add the footers into the document  
                foreach (Word.Section wordSection in document.Sections)
                {
                    //Get the footer range and add the footer details.  
                    Word.Range footerRange = wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Footer text goes here";
                }

                //adding text to document  
                document.Content.SetRange(0, 0);
                document.Content.Text = "This is test document " + Environment.NewLine;

                //Save the document  
                document.Save();
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Document created successfully !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //        public void ff()
        //        {
        //            // = D0 = A1 = D0 = BE = D0 = B7 = D0 = B4 = D0 = B0 = D0 = BD = D0 = B8 = D0 = B5 = D0 = B4 =
        //            //= D0 = BE = D0 = BA = D1 = 83 = D0 = BC = D0 = B5 = D0 = BD = D1 = 82 = D0 = B0
        //            // = D0 = 92 = D0 = BC = D0 = B5 = D1 = 81 = D1 = 82 = D0 = BE:
        //            Word.Application wordApp = 3D new Word.Application();
        //            // =D0=9D=D1=83=D0=B6=D0=BD=D0=BE:
        //            Type wordType = 3D Type.GetTypeFromProgID("Word.Application")
        //            dynamic wordApp = 3D Activator.CreateInstance(wordType);
        //            wordApp.Visible = 3D true;
        //            Word.Document wordDoc = 3D null;
        //            dynamic wordDoc = 3D null;
        //            Object template = 3D(Environment.CurrentDirectory + "\\Docs\=\Temp.docx");


        //                wordDoc = 3D wordApp.Documents.Add(ref template);
        //                //=D0=97=D0=B0=D0=BC=D0=B5=D0=BD=D0=B0 =D0=BC=D0=B5=D1=
        //= 82 = D0 = BE = D0 = BA = D0 = B4 = D0 = B0 = D0 = BD = D0 = BD = D1 = 8B = D0 = BC = D0 = B8
        //                var items = 3D new Dictionary<string, string>
        //                {
        //                    { "%docNum%", order.idOrder.ToString()
        //    },
        //                    { "%docDate%", order.docDate.Value.ToString("dd.MM.y=
        //yyy") },
        //                    { "%stName%", order.ScholarshipTypes.stName
        //}
        //                };
        //            foreach (var item in items)
        //            {
        //                Word.Find find = 3D wordApp.Selection.Find;
        //                find.Text = 3D item.Key;
        //                find.Replacement.Text = 3D item.Value;
        //                Object wrap = 3D Word.WdFindWrap.wdFindContinue;
        //                Object replace = 3D Word.WdReplace.wdReplaceAll;
        //                find.Execute(FindText: Type.Missing,
        //                    MatchCase: false,
        //                    MatchWholeWord: false,
        //                    MatchWildcards: false,
        //                    MatchSoundsLike: Type.Missing,
        //                    MatchAllWordForms: false,
        //                    Forward: true,
        //                    Wrap: wrap,
        //                    Format: false,
        //                    ReplaceWith: Type.Missing, Replace: replace);
        //            }
        //            //                //=D0=A1=D0=BE=D0=B7=D0=B4=D0=B0=D0=BD=D0=B8=D0=B5 =D0=
        //            //= B8 = D0 = B7 = D0 = B0 = D0 = BF = D0 = BE = D0 = BB = D0 = BD = D0 = B5 = D0 = BD = D0 = B8 = D0 = B5 = D1 = 82 =
        //            //= D0 = B0 = D0 = B1 = D0 = BB = D0 = B8 = D1 = 86 = D1 = 8B
        //            wordApp.Selection.Find.Execute("%Table%");
        //            Word.Range wordRange = 3D wordApp.Selection.Range;
        //            int row = 3D OrderStringDataGrid.Items.Count + 2;
        //            int col = 3D OrderStringDataGrid.Columns.Count;
        //            Word.Table wordTable = 3D wordDoc.Tables.Add(wordRange, r =
        //            ow, col);
        //            wordTable.set = 5FStyle("=D0=A1=D0=B5=D1=82=D0=BA=D0=B0 =
        //            = D1 = 82 = D0 = B0 = D0 = B1 = D0 = BB = D0 = B8 = D1 = 86 = D1 = 8B");


        //              wordTable.ApplyStyleHeadingRows = 3D true;
        //            wordTable.ApplyStyleLastRow = 3D false;
        //            wordTable.ApplyStyleFirstColumn = 3D true;
        //            wordTable.ApplyStyleLastColumn = 3D false;
        //            wordTable.ApplyStyleRowBands = 3D true;
        //            wordTable.ApplyStyleColumnBands = 3D false;
        //            wordTable.Cell(1, 1).Range.Text = 3D "=D0=A4=D0=B0=D0=BC=
        //            = D0 = B8 = D0 = BB = D0 = B8 = D1 = 8F, = D0 = 98 = D0 = BC = D1 = 8F, = D0 = 9E = D1 = 82 = D1 = 87 = D0 = B5 =

        //                       = D1 = 81 = D1 = 82 = D0 = B2 = D0 = BE";
        //                wordTable.Cell(1, 2).Range.Text = 3D "=D0=93=D1=80=D1=83=
        // = D0 = BF = D0 = BF = D0 = B0";
        //                wordTable.Cell(1, 3).Range.Text = 3D "=D0=A1=D1=83=D0=BC=
        // = D0 = BC = D0 = B0(= D1 = 80 = D1 = 83 = D0 = B1.)";
        //                wordTable.Cell(1, 4).Range.Text = 3D "=D0=94=D0=B0=D1=82=
        // = D0 = B0 = D0 = BD = D0 = B0 = D1 = 87 = D0 = B0 = D0 = BB = D0 = B0 = D0 = B4 = D0 = B5 = D0 = B9 = D1 = 81 = D1 =
        // = 82 = D0 = B2 = D0 = B8 = D1 = 8F";
        //                wordTable.Cell(1, 5).Range.Text = 3D "=D0=94=D0=B0=D1=82=
        // = D0 = B0 = D0 = BA = D0 = BE = D0 = BD = D1 = 86 = D0 = B0 = D0 = B4 = D0 = B5 = D0 = B9 = D1 = 81 = D1 = 82 = D0 =
        // = B2 = D0 = B8 = D1 = 8F";
        //                wordTable.Columns[1].Width = 3D 190;
        //            wordTable.Columns[2].Width = 3D 80;
        //            wordTable.Columns[3].Width = 3D 70;
        //            wordTable.Columns[4].Width = 3D 70;
        //            wordTable.Columns[5].Width = 3D 70;
        //            List<DataBase.OrderStrings> orderStrings = 3D SourceCore.=
        //            getBase().OrderStrings.Where(o = 3D > o.idOrder = 3D = 3D order.idOrder).ToLi =
        //            st();
        //            decimal sum = 3D 0;
        //            for (var i = 3D 0; i < row - 2; i++)
        //                {
        //                wordTable.Cell(i + 2, 1).Range.Text = 3D orderStrings =
        //            [i].Students.fio;
        //                wordTable.Cell(i + 2, 2).Range.Text = 3D orderStrings =
        //            [i].Students.Groups.gName;
        //                wordTable.Cell(i + 2, 3).Range.Text = 3D orderStrings =
        //            [i].cost.ToString();
        //                wordTable.Cell(i + 2, 4).Range.Text = 3D orderStrings =
        //            [i].startDate.Value.ToString("dd.MM.yyyy");
        //                wordTable.Cell(i + 2, 5).Range.Text = 3D orderStrings =
        //            [i].finishDate.Value.ToString("dd.MM.yyyy");
        //                sum += 3D(decimal)orderStrings[i].cost;
        //            }
        //            wordTable.Cell(row, 2).Range.Text = 3D "=D0=9E=D0=B1=D1=
        //            = 89 = D0 = B0 = D1 = 8F = D1 = 81 = D1 = 83 = D0 = BC = D0 = BC = D0 = B0";
        //                wordTable.Cell(row, 2).Range.Font.Bold = 3D 3;
        //            wordTable.Cell(row, 3).Range.Text = 3D sum.ToString();
        //        }

        //Формирование Word-документа из таблицы OrderStrings
        //public void WordExportButtonClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //Создание документа
        //        Type wordType = Type.GetTypeFromProgID("Word.Application");
        //        dynamic wordApp = Activator.CreateInstance(wordType);
        //        wordApp.Visible = true;
        //        dynamic wordDoc = null;
        //        Object template = (Environment.CurrentDirectory + "\\Docs\\Temp.docx");
        //        wordDoc = wordApp.Documents.Add(ref template);
        //        //Замена меток данными
        //        //var items = new Dictionary<string, string>
        //        //{
        //        //    { "%docNum%", order.idOrder.ToString() },
        //        //    { "%docDate%", order.docDate.Value.ToString("dd.MM.yyyy") },
        //        //    { "%stName%", order.ScholarshipTypes.stName }
        //        //};
        //        foreach (var item in items)
        //        {
        //            Word.Find find = wordApp.Selection.Find;
        //            find.Text = item.Key;
        //            find.Replacement.Text = item.Value;
        //            Object wrap = Word.WdFindWrap.wdFindContinue;
        //            Object replace = Word.WdReplace.wdReplaceAll;
        //            find.Execute(FindText: Type.Missing,
        //                MatchCase: false,
        //                MatchWholeWord: false,
        //                MatchWildcards: false,
        //                MatchSoundsLike: Type.Missing,
        //                MatchAllWordForms: false,
        //                Forward: true,
        //                Wrap: wrap,
        //                Format: false,
        //                ReplaceWith: Type.Missing, Replace: replace);
        //        }
        //        //Создание и заполнение таблицы
        //        wordApp.Selection.Find.Execute("%Table%");
        //        Word.Range wordRange = wordApp.Selection.Range;
        //        //int row = OrderStringDataGrid.Items.Count + 2;
        //        //int col = OrderStringDataGrid.Columns.Count;
        //        Word.Table wordTable = wordDoc.Tables.Add(wordRange, row, col);
        //        wordTable.set_Style("Сетка таблицы");
        //        wordTable.ApplyStyleHeadingRows = true;
        //        wordTable.ApplyStyleLastRow = false;
        //        wordTable.ApplyStyleFirstColumn = true;
        //        wordTable.ApplyStyleLastColumn = false;
        //        wordTable.ApplyStyleRowBands = true;
        //        wordTable.ApplyStyleColumnBands = false;
        //        wordTable.Cell(1, 1).Range.Text = "Фамилия, Имя, Отчество";
        //        wordTable.Cell(1, 2).Range.Text = "Группа";
        //        wordTable.Cell(1, 3).Range.Text = "Сумма (руб.)";
        //        wordTable.Cell(1, 4).Range.Text = "Дата начала действия";
        //        wordTable.Cell(1, 5).Range.Text = "Дата конца действия";
        //        wordTable.Columns[1].Width = 190;
        //        wordTable.Columns[2].Width = 80;
        //        wordTable.Columns[3].Width = 70;
        //        wordTable.Columns[4].Width = 70;
        //        wordTable.Columns[5].Width = 70;
        //        List<DataBase.OrderStrings> orderStrings = SourceCore.getBase().OrderStrings.Where(o => o.idOrder == order.idOrder).ToList();
        //        decimal sum = 0;
        //        for (var i = 0; i < row - 2; i++)
        //        {
        //            wordTable.Cell(i + 2, 1).Range.Text = orderStrings[i].Students.fio;
        //            wordTable.Cell(i + 2, 2).Range.Text = orderStrings[i].Students.Groups.gName;
        //            wordTable.Cell(i + 2, 3).Range.Text = orderStrings[i].cost.ToString();
        //            wordTable.Cell(i + 2, 4).Range.Text = orderStrings[i].startDate.Value.ToString("dd.MM.yyyy");
        //            wordTable.Cell(i + 2, 5).Range.Text = orderStrings[i].finishDate.Value.ToString("dd.MM.yyyy");
        //            sum += (decimal)orderStrings[i].cost;
        //        }
        //        wordTable.Cell(row, 2).Range.Text = "Общая сумма";
        //        wordTable.Cell(row, 2).Range.Font.Bold = 3;
        //        wordTable.Cell(row, 3).Range.Text = sum.ToString();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Не удалось создать документ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}
    }

}
