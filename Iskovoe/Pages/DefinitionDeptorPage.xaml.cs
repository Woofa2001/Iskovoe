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

namespace Iskovoe.Pages
{
    /// <summary>
    /// Логика взаимодействия для DefinitionDeptorPage.xaml
    /// </summary>
    public partial class DefinitionDeptorPage : Page
    {
        public MakeIskovoeWindow _window;
        public DefinitionDeptorPage(MakeIskovoeWindow window)
        {
            InitializeComponent();
            DataContext = this;
            _window = window;
            DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.ToList();
        }

        private void FilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxStart.Text = "";
        }   

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            switch (FilterCombobox.SelectedIndex)
            {
                case 0:
                    DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.Where(filtercase => filtercase.name_dolg.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.Where(filtercase => filtercase.inn.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.Where(filtercase => filtercase.kpp.Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.Where(filtercase => filtercase.phone.Contains(textbox.Text)).ToList();
                    break;
                case 4:
                    DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.Where(filtercase => filtercase.adress.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        private void TextBoxEnd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> ColumnsIscovoe = new List<string>();
            for (int J = 0; J < 5; J++)
            {
                ColumnsIscovoe.Add(DataGridDeptors.Columns[J].Header.ToString());
            }
            FilterCombobox.ItemsSource = ColumnsIscovoe;
            FilterCombobox.SelectedIndex = 0;
            foreach (DataGridColumn ColumnProposals in DataGridDeptors.Columns)
            {
                ColumnProposals.CanUserSort = false;
            }
        }

        private void AddDeptorsButton_Click(object sender, RoutedEventArgs e)
        {
            _window.MakeIscovoeFrame.Navigate(new Pages.AddDeptorPage(_window));
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
