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
            if (LoginTextBox.Text.Length > 0) // проверяем логин
            {
                if ((PasswordPasswordBox.Password.Length > 0) || (PasswordTextBox.Text.Length > 0)) // проверяем пароль
                {
                    if ((PasswordPasswordBox.Password.Length >= 6) || (PasswordTextBox.Text.Length >= 6))
                    {
                        if ((PasswordPasswordBoxTwo.Password.Length > 0) || (PasswordTextBoxTwo.Text.Length > 0)) // проверяем второй пароль
                        {
                            if ((PasswordPasswordBox.Password == PasswordPasswordBoxTwo.Password) && (PasswordTextBox.Text == PasswordTextBoxTwo.Text) || ((PasswordPasswordBox.Password == PasswordTextBoxTwo.Text) && (PasswordPasswordBoxTwo.Password == PasswordTextBox.Text))) // проверка на совпадение паролей
                            {
                                bool en = true; // английская раскладка
                                bool symbol = false; // символ
                                bool number = false; // цифра

                                for (int i = 0; i < PasswordPasswordBox.Password.Length; i++) // перебираем символы
                                {
                                    if (PasswordPasswordBox.Password[i] >= 'А' && PasswordPasswordBox.Password[i] <= 'Я') en = false; // если русская раскладка
                                    if (PasswordPasswordBox.Password[i] >= '0' && PasswordPasswordBox.Password[i] <= '9') number = true; // если цифры
                                    if (PasswordPasswordBox.Password[i] == '_' || PasswordPasswordBox.Password[i] == '-' ||
                                        PasswordPasswordBox.Password[i] == '!') symbol = true; // если символ
                                }

                                if (!en)
                                    MessageBox.Show("Доступна только английская раскладка"); // выводим сообщение
                                else if (!symbol)
                                    MessageBox.Show("Добавьте один из следующих символов: _ - !"); // выводим сообщение
                                else if (!number)
                                    MessageBox.Show("Добавьте хотя бы одну цифру"); // выводим сообщение
                                if (en && symbol && number) // проверяем соответствие
                                {
                                    // Создание и инициализация нового пользователя системы
                                    Data.Executor Executor = Database.Executor.SingleOrDefault(u => u.login == LoginTextBox.Text);
                                    Data.Executor Executors = new Data.Executor();
                                    if (Executor == null)
                                    {
                                        Executor.login = LoginTextBox.Text;
                                        Executor.password = PasswordPasswordBox.Password != "" ? PasswordPasswordBox.Password : PasswordTextBox.Text;
                                        //Executor.ID_ROLE = 0;
                                        // Добавление его в базу данных
                                        Database.Executor.Add(Executor);
                                        // Сохранение изменений
                                        Database.SaveChanges();
                                        MessageBox.Show("Пользователь зарегистрирован");
                                        AutoritizationWindow window = new AutoritizationWindow();
                                        Close();
                                        window.ShowDialog();
                                    }
                                    else MessageBox.Show("Пользователь с таким логином уже существует");
                                }
                            }
                            else MessageBox.Show("Пароли не совпадают");
                        }
                        else MessageBox.Show("Повторите пароль");
                    }
                    else MessageBox.Show("пароль слишком короткий, минимум 6 символов");
                }
                else MessageBox.Show("Укажите пароль");
            }
            else MessageBox.Show("Укажите логин");
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
