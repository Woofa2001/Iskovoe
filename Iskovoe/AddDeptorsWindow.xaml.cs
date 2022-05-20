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
using System.Windows.Shapes;

namespace Iskovoe
{
    /// <summary>
    /// Логика взаимодействия для AddDeptorsWindow.xaml
    /// </summary>
    public partial class AddDeptorsWindow : Window
    {
        public AddDeptorsWindow()
        {
            InitializeComponent();
        }

        private void AddDeptorsButton_Click(object sender, RoutedEventArgs e)
        {
            var A = new Data.Debtors();
            A.name_dolg = FIOTextBox.Text;
            A.inn = INNTextBox.Text;
            A.kpp = KPPTextBox.Text;
            A.phone = PhoneTextBox.Text;
            A.adress = AdressTextBox.Text;
            SourceCore.DB.Debtors.Add(A);
            SourceCore.DB.SaveChanges();
            Close();
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
