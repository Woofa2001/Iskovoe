﻿using System;
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
    /// Логика взаимодействия для AddDeptorPage.xaml
    /// </summary>
    public partial class AddDeptorPage : Page
    {
        private MakeIskovoeWindow _window;
        public AddDeptorPage(MakeIskovoeWindow window)
        {
            InitializeComponent();
            DataContext = this;
            _window = window;
        }

        private void AddDeptorsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
