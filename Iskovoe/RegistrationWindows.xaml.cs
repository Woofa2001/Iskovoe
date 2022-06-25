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
using System.Text.RegularExpressions;

namespace Iskovoe
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindows.xaml
    /// </summary>
    public partial class RegistrationWindows : Window
    {
        private Data.IskovoeEntities Database;
        public RegistrationWindows(Data.IskovoeEntities Database)
        {
                InitializeComponent();
            this.Database = Database;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceCore.DB.Executor.Any(P => P.login == LoginTextBox.Text))
            {
                MessageBox.Show("Логин занят", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
            }
            else
            {
                var input = PasswordTextBox.Text == "" ? PasswordPasswordBox.Password : PasswordTextBox.Text;
                var Number = new Regex(@"[0-9]+");
                var UpperChar = new Regex(@"[A-Z|А-Я]+");
                var SpecialSymbol = new Regex(@"(?=.*[\W])");
                var Min8Chars = new Regex(@".{8,}");
                if (Number.IsMatch(input) && UpperChar.IsMatch(input) && SpecialSymbol.IsMatch(input) && Min8Chars.IsMatch(input))
                {
                    if ((PasswordPasswordBox.Password == PasswordPasswordBoxTwo.Password) && (PasswordTextBox.Text == PasswordTextBoxTwo.Text) || ((PasswordPasswordBox.Password == PasswordTextBoxTwo.Text) && (PasswordPasswordBoxTwo.Password == PasswordTextBox.Text))) // проверка на совпадение паролей
                    {
                        // Создание и инициализация нового пользователя системы
                        Data.Executor User = new Data.Executor();
                        User.login = LoginTextBox.Text;
                        User.password = PasswordPasswordBox.Password != "" ? PasswordPasswordBox.Password : PasswordTextBox.Text;
                        //User.name_user = "tr";
                        // Добавление его в базу данных
                        Database.Executor.Add(User);
                        // Сохранение изменений
                        Database.SaveChanges();
                        Close();
                    }
                    else MessageBox.Show("Пароли не совпадают");
                }
                else MessageBox.Show("Введен не соотвествующий пароль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
            }
        }

        private void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            String Password = PasswordPasswordBox.Password;
            Visibility Visibility = PasswordPasswordBox.Visibility;
            double Width = PasswordPasswordBox.ActualWidth;
            // Переброска информации из TextBox'а в PasswordBox
            PasswordPasswordBox.Password = PasswordTextBox.Text;
            PasswordPasswordBox.Visibility = PasswordTextBox.Visibility;
            PasswordPasswordBox.Width = PasswordTextBox.Width;
            // Возврат информации из временных буферов в TextBox
            PasswordTextBox.Text = Password;
            PasswordTextBox.Visibility = Visibility;
            PasswordTextBox.Width = Width;
        }

        private void PasswordButtontwo_Click(object sender, RoutedEventArgs e)
        {
            String Password = PasswordPasswordBoxTwo.Password;
            Visibility Visibility = PasswordPasswordBoxTwo.Visibility;
            double Width = PasswordPasswordBoxTwo.ActualWidth;
            // Переброска информации из TextBox'а в PasswordBox
            PasswordPasswordBoxTwo.Password = PasswordTextBoxTwo.Text;
            PasswordPasswordBoxTwo.Visibility = PasswordTextBoxTwo.Visibility;
            PasswordPasswordBoxTwo.Width = PasswordTextBoxTwo.Width;
            // Возврат информации из временных буферов в TextBox
            PasswordTextBoxTwo.Text = Password;
            PasswordTextBoxTwo.Visibility = Visibility;
            PasswordTextBoxTwo.Width = Width;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
