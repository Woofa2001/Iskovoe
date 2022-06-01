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
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using Microsoft.Win32;


namespace Iskovoe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int buf_id;
        public string ImagePath;
        public string DataSource = @"HOME-PC\SQLEXPRESS"; // Соединение
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
            GetImageBase64FromDb();
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
            if (Stack1Label.Visibility == Visibility.Visible) Stack1Label.Visibility = Visibility.Collapsed; else Stack1Label.Visibility = Visibility.Visible;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakeIskovoeWindow window = new MakeIskovoeWindow();
            window.Show();
            Close();
        }

        private void PutImageBase64InDb(string iFile)
        {
            // конвертация изображения в base64
            string base64String = null;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(iFile))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            // получение расширения файла изображения не забыв удалить точку перед расширением
            string iImageExtension = (System.IO.Path.GetExtension(iFile)).Replace(".", "").ToLower();
            // запись изображения в БД
            using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=" + DataSource + "; Initial Catalog=Iskovoe; Integrated Security=True")) // строка подключения к БД
            {
                string commandText = "UPDATE EXECUTOR SET image = @image, image_format = @image_format WHERE id_executor = " + buf_id; // запрос на вставку
                SqlCommand command = new SqlCommand(commandText, sqlConnection);
                command.Parameters.AddWithValue("@image", base64String); // записываем само изображение
                command.Parameters.AddWithValue("@image_format", iImageExtension); // записываем расширение изображения
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        private void GetImageBase64FromDb()
        {
            // получаем данные их БД
            List<string> iScreen = new List<string>(); // сделав запрос к БД мы получим множество строк в ответе, поэтому мы их сможем загнать в массив/List
            List<string> iScreen_format = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=" + DataSource + "; Initial Catalog=Iskovoe; Integrated Security=True"))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = @"SELECT [image], [image_format] FROM [EXECUTOR] WHERE [id_executor] =" + buf_id; // наша запись в БД под id=1, поэтому в запросе "WHERE [id] = 1"
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                string iTrimText = null;
                while (sqlReader.Read()) // считываем и вносим в лист результаты
                {
                    iTrimText = sqlReader["image"].ToString().TrimStart().TrimEnd(); // читаем строки с изображениями
                    iScreen.Add(iTrimText);
                    iTrimText = sqlReader["image_format"].ToString().TrimStart().TrimEnd(); // читаем строки с форматом изображения
                    iScreen_format.Add(iTrimText);
                }
                sqlConnection.Close();
            }
            // конвертируем данные в изображение
            string base64StringImage = iScreen[0]; // возвращает массив байт из БД. Так как у нас SQL вернёт одну запись и в ней хранится нужное нам изображение, то из листа берём единственное значение с индексом '0'
            byte[] imageData = Convert.FromBase64String(base64StringImage);
            MemoryStream ms = new MemoryStream(imageData);
            System.Drawing.Image newImage = System.Drawing.Image.FromStream(ms);
            // сохраняем изоражение на диск
            string iImageExtension = iScreen_format[0]; // получаем расширение текущего изображения хранящееся в БД
            string iImageName = @"C:\result_new_base64" + "." + iImageExtension; // задаём путь сохранения и имя нового изображения
            if (iImageExtension == "png") { newImage.Save(iImageName, System.Drawing.Imaging.ImageFormat.Png); }
            else if (iImageExtension == "jpg" || iImageExtension == "jpeg") { newImage.Save(iImageName, System.Drawing.Imaging.ImageFormat.Jpeg); }
            else if (iImageExtension == "gif") { newImage.Save(iImageName, System.Drawing.Imaging.ImageFormat.Gif); }
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(iImageName,UriKind.Relative));
            ImageElipse.Fill = imageBrush;
            //ImageElipse.ImageSource = iImageName;
            //ImageElipse.ImageSource = New BitmapImage(New Uri(iImageName, UriKind.Relative))
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            //var fileContent = string.Empty;
            //var filePath = string.Empty;
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif;)|*.png;*.jpeg;*.jpg;*.gif";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    filePath = File.ReadAllText(openFileDialog.FileName);

            //    var fileStream = openFileDialog.OpenFile();

            //    using (StreamReader reader = new StreamReader(fileStream))
            //    {
            //        fileContent = reader.ReadToEnd();
            //    }
            //}
            //if (filePath != "") {
                PutImageBase64InDb(@"C:\2.jpg"); // запись изображения в БД
                GetImageBase64FromDb();
            MessageBox.Show("Картинка добавлена");
            //}
        }
    }
}
