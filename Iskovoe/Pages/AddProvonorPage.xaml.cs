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
        private Word.Paragraphs wordparagraphs;
        private Word.Paragraph wordparagraph;
        private Word.Application wordapp;
        private Word.Documents worddocuments;
        private Word.Document worddocument;
        string NameMounth;
        decimal buf_sum = 0;

        public AddProvonorPage(int buf_id_iscovoe)
        {
            InitializeComponent();
            TipFormComboBox.ItemsSource = SourceCore.DB.Tip_forms.ToList();
            SostavComboBox.ItemsSource = SourceCore.DB.Sostav.ToList();
            DataGridPravonor.ItemsSource = SourceCore.DB.Pravonor.Where(P => P.id_iskovoe == buf_id_iscovoe).ToList();
            buf_id = buf_id_iscovoe;
            SumLabel.Content = "0";
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddPravonorButton_Click(object sender, RoutedEventArgs e)
        {
            if (YearTextBox.Text.Length == 4)
            {
                if (SumTextBox.Text != "")
                {
                    var B = new Data.Period();
                    B.month = MonthComboBox.SelectedItem.ToString();
                    B.year = int.Parse(YearTextBox.Text);
                    //B.last_date = LastDatePicker.SelectedDate;
                    SourceCore.DB.Period.Add(B);
                    var A = new Data.Pravonor();
                    A.id_iskovoe = buf_id;
                    A.id_period = B.id_period;
                    A.Tip_forms = (Data.Tip_forms)TipFormComboBox.SelectedItem;
                    A.Sostav = (Data.Sostav)SostavComboBox.SelectedItem;
                    A.summa = decimal.Parse(SumTextBox.Text) * 500;
                    buf_sum = buf_sum + decimal.Parse(SumTextBox.Text) * 500;
                    SumLabel.Content = buf_sum+"р";
                    SourceCore.DB.Pravonor.Add(A);
                    // Сохранение изменений
                    SourceCore.DB.SaveChanges();
                    DataGridPravonor.ItemsSource = SourceCore.DB.Pravonor.Where(P => P.id_iskovoe == buf_id).ToList();
                }
                else
                {
                    MessageBox.Show("Количество людей", "Внимание");
                }
            }
            else
            {
                MessageBox.Show("Введите коректный год", "Внимание");
            }
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

                //Save the document  
                wordDoc.Save();
                wordDoc.Close(ref missing, ref missing, ref missing);
                wordDoc = null;
                wordApp.Quit(ref missing, ref missing, ref missing);
                wordApp = null;

                MainWindow window = new MainWindow(buf_id);
                window.Show();
                MakeIskovoeWindow makeIskovoe = new MakeIskovoeWindow(buf_id);
                makeIskovoe.Close();

                wordapp = new Word.Application();
                wordapp.Visible = true;
                Object template1 = Type.Missing;
                Object newTemplate = false;
                Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
                Object visible = true;
                wordapp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);
                template1 = Environment.CurrentDirectory + "\\Document\\s1.txt ";
                worddocument =wordapp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);

                worddocuments = wordapp.Documents;
                Object name = "Заявление";
                //Для Visual Studio 2003
                //worddocument=(Word.Document)worddocuments.Item(ref name);
                worddocument = (Word.Document)worddocuments.get_Item(ref name);
                worddocument.Activate();
                //Подготавливаем параметры для сохранения документа
                Object fileName = Environment.CurrentDirectory + "\\s2.doc";
                Object fileFormat = Word.WdSaveFormat.wdFormatDocument;
                Object lockComments = false;
                Object password = "";
                Object addToRecentFiles = false;
                Object writePassword = "";
                Object readOnlyRecommended = false;
                Object embedTrueTypeFonts = false;
                Object saveNativePictureFormat = false;
                Object saveFormsData = false;
                Object saveAsAOCELetter = Type.Missing;
                Object encoding = Type.Missing;
                Object insertLineBreaks = Type.Missing;
                Object allowSubstitutions = Type.Missing;
                Object lineEnding = Type.Missing;
                Object addBiDiMarks = Type.Missing;
                worddocument.SaveAs(ref fileName,

               ref fileFormat, ref lockComments,
               ref password, ref addToRecentFiles, ref writePassword,
               ref readOnlyRecommended, ref embedTrueTypeFonts,
               ref saveNativePictureFormat, ref saveFormsData,
               ref saveAsAOCELetter, ref encoding, ref insertLineBreaks,
               ref allowSubstitutions, ref lineEnding, ref addBiDiMarks);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Документ создан");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TipFormComboBox.SelectedIndex = 0;
            SostavComboBox.SelectedIndex = 0;
            List<String> ColumnsIscovoe = new List<string>();
            MonthComboBox.SelectedIndex = 0;
            for (int J = 0; J < 12; J++)
            {
                switch (J)
                {
                    case 0:
                        NameMounth = "Январь";
                        break;
                    case 1:
                        NameMounth = "Февраль";
                        break;
                    case 2:
                        NameMounth = "Март";
                        break;
                    case 3:
                        NameMounth = "Апрель";
                        break;
                    case 4:
                        NameMounth = "Май";
                        break;
                    case 5:
                        NameMounth = "Июнь";
                        break;
                    case 6:
                        NameMounth = "Июль";
                        break;
                    case 7:
                        NameMounth = "Август";
                        break;
                    case 8:
                        NameMounth = "Сентябрь";
                        break;
                    case 9:
                        NameMounth = "Октябрь";
                        break;
                    case 10:
                        NameMounth = "Ноябрь";
                        break;
                    case 11:
                        NameMounth = "Декабрь";
                        break;
                }
                ColumnsIscovoe.Add(""+NameMounth+"");
            }
            MonthComboBox.ItemsSource = ColumnsIscovoe;
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
