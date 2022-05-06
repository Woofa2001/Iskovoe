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


namespace Iskovoe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int buf_id;

        public MainWindow(int id)
        {
            InitializeComponent();
            buf_id = id;
            DataContext = this;
            DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(id_dolg => id_dolg.id_dolg.Value == buf_id).ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> ColumnsIscovoe = new List<string>();
            for (int J = 0; J < 2; J++)
            {
                ColumnsIscovoe.Add(DataGridIscovoe.Columns[J].Header.ToString());
            }
            FilterCombobox.ItemsSource = ColumnsIscovoe;
            FilterCombobox.SelectedIndex = 0;
            foreach (DataGridColumn ColumnProposals in DataGridIscovoe.Columns)
            {
                ColumnProposals.CanUserSort = false;
            }
            if (SourceCore.DB.Executor.ToList()[buf_id-1].name_executor != null)
            {
                NameLabel.Content = SourceCore.DB.Executor.ToList()[buf_id-1].name_executor.ToString();
            }
            if (SourceCore.DB.Executor.ToList()[buf_id-1].id_post != null)
            {
                INNLabel.Content = SourceCore.DB.Executor.ToList()[buf_id-1].Post.name_post.ToString();
            }
            if (SourceCore.DB.Executor.ToList()[buf_id-1].passport != null)
            {
                KppLabel.Content = SourceCore.DB.Executor.ToList()[buf_id-1].passport.ToString();
            }
            if (SourceCore.DB.Executor.ToList()[buf_id-1].image != null)
            {
                PhoneLabel.Content = SourceCore.DB.Executor.ToList()[buf_id-1].image.ToString();
            }
        }

        private void FilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterCombobox.SelectedIndex == 1)
            {
                TextBoxEnd.Visibility = Visibility.Visible;
                TextBoxStart.Width = 135;
            }
            else
            {
                TextBoxEnd.Visibility = Visibility.Hidden;
                TextBoxStart.Width = 270;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            switch (FilterCombobox.SelectedIndex)
            {
                case 0:
                    if (TextBoxStart.Text != "")
                    {
                        DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(filtercase => filtercase.Debtors.name_dolg.Contains(textbox.Text)&& filtercase.id_dolg.Value == buf_id).ToList();
                    }
                    else
                        DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(id_dolg => id_dolg.id_dolg.Value == buf_id).ToList();
                    break;
                case 1:
                    if ((TextBoxStart.Text != "") || (TextBoxEnd.Text != ""))
                    {
                        DateTime.TryParse(TextBoxStart.Text, out DateTime val);
                        DateTime.TryParse(TextBoxEnd.Text, out DateTime val1);
                        if ((TextBoxStart.Text != "") && (TextBoxEnd.Text == ""))
                        {
                            DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(filtercase => filtercase.data_iscovoe.Value >= val).ToList();
                        }
                        else if ((TextBoxStart.Text != "") && (TextBoxEnd.Text == ""))
                        {
                            DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(filtercase => filtercase.data_iscovoe.Value <= val1).ToList();
                        }
                        else
                        {
                            DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(filtercase => (filtercase.data_iscovoe.Value >= val) && (filtercase.data_iscovoe.Value <= val1)).ToList();
                        }
                    }
                    else
                        DataGridIscovoe.ItemsSource = SourceCore.DB.Iskovoe.Where(id_dolg => id_dolg.id_dolg.Value == buf_id).ToList();
                    break;
            }
        }

        private void TextBoxEnd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AutoritizationWindow window = new AutoritizationWindow();
            window.Show();
            Close();
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            if (StackLabel.Visibility == Visibility.Visible) StackLabel.Visibility = Visibility.Collapsed; else StackLabel.Visibility = Visibility.Visible;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void RowDefinition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
