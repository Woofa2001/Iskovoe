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
    }
}
