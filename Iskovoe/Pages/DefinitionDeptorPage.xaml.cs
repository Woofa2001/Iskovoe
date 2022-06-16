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
        public int Id_executor;
        private MakeIskovoeWindow MakeIskovoeWindow;
        public DefinitionDeptorPage(MakeIskovoeWindow makeIskovoeWindow, int id_executor)
        {
            InitializeComponent();
            DataContext = this;
            DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.OrderBy(P => P.name_dolg).Skip((BlockNum - 1) * BlockRecordsCount).Take(BlockRecordsCount).ToList(); ;
            MakeIskovoeWindow = makeIskovoeWindow;
            Id_executor = id_executor;
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
            AddDeptorsWindow window = new AddDeptorsWindow();
            window.ShowDialog();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridDeptors.SelectedItem != null)
            {
                var A = new Data.Iskovoe();
                A.id_executor = Id_executor+1;
                A.Debtors = (Data.Debtors)DataGridDeptors.SelectedItem;
                SourceCore.DB.Iskovoe.Add(A);
                // Сохранение изменений
                SourceCore.DB.SaveChanges();
                int buf_id_iscovoe = A.id_iskovoe;
                MakeIskovoeWindow.MakeIscovoeFrame.Navigate(new Pages.AddProvonorPage(buf_id_iscovoe));
            } 
            else 
            {
                MessageBox.Show("Выберите должника");
            }
        }

        // Текущий номер блока информации в таблице
        private int _BlockNum = 1;
        public int BlockNum
        {
            get
            {
                return _BlockNum;
            }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }
                else
                {
                    if (value > BlockCount)
                    {
                        value = BlockCount;
                    }
                }
                if (_BlockNum != value)
                {
                    _BlockNum = value;
                    BlockNumLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                }
                UpdateGrid(null);
            }
        }

        // Количество записей в блоке информации в таблице
        private int _BlockRecordsCount = 5;
        public int BlockRecordsCount
        {
            get
            {
                return _BlockRecordsCount;
            }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }
                if (_BlockRecordsCount != value)
                {
                    _BlockRecordsCount = value;
                    BlockCountLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    BlockNum = _BlockNum;
                    UpdateGrid(null);
                }
            }
        }

        public int BlockCount
        {
            get { return (SourceCore.DB.Debtors.OrderBy(P => P.name_dolg).Count() - 1) / BlockRecordsCount + 1; }
        }

        private void FirstBlockButton_Click(object sender, RoutedEventArgs e)
        {
            BlockNum = 1;
        }

        private void PreviosBlockButton_Click(object sender, RoutedEventArgs e)
        {
            BlockNum--;
        }

        private void NextBlockButton_Click(object sender, RoutedEventArgs e)
        {
            BlockNum++;
        }

        private void LastBlockButton_Click(object sender, RoutedEventArgs e)
        {
            BlockNum = BlockCount;
        }

        //Метод обновления грида
        public void UpdateGrid(Data.Debtors Debtors)
        {
            if ((Debtors == null) && (DataGridDeptors.ItemsSource != null))
            {
                Debtors = (Data.Debtors)DataGridDeptors.SelectedItem;
            }
            DataGridDeptors.ItemsSource = SourceCore.DB.Debtors.OrderBy(P => P.name_dolg).Skip((BlockNum - 1) * BlockRecordsCount).Take(BlockRecordsCount).ToList();
            DataGridDeptors.SelectedItem = Debtors;
        }
    }
}
