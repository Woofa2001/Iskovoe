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
            DataGridPravonor.ItemsSource = SourceCore.DB.Pravonor.Where(P => P.id_iskovoe == buf_id_iscovoe).ToList();
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
            DataGridPravonor.ItemsSource = SourceCore.DB.Pravonor.Where(P => P.id_iskovoe == buf_id).ToList();
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
                Type wordType = Type.GetTypeFromProgID("Word.Application");
                dynamic wordApp = Activator.CreateInstance(wordType);

                wordApp.Visible = true;

                //Set animation status for word application  
                wordApp.ShowAnimation = false;

                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;

                //Открытие документа
                dynamic wordDoc = null;
                object template = (Environment.CurrentDirectory + "\\Document\\Iscovoe.docx");
                wordDoc = wordApp.Documents.Add(ref template);

                //Add header into the document  
                foreach (Word.Section section in wordDoc.Sections)
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
                foreach (Word.Section wordSection in wordDoc.Sections)
                {
                    //Get the footer range and add the footer details.  
                    Word.Range footerRange = wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Footer text goes here";
                }

                //adding text to document  
                wordDoc.Content.SetRange(0, 0);
                wordDoc.Content.Text = "This is test document " + Environment.NewLine;

                //Save the document  
                wordDoc.Save();
                wordDoc.Close(ref missing, ref missing, ref missing);
                wordDoc = null;
                wordApp.Quit(ref missing, ref missing, ref missing);
                wordApp = null;
                MessageBox.Show("Document created successfully !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
