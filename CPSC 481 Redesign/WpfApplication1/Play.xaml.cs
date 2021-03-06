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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Play.xaml
    /// </summary>
    public partial class Play : Window
    {
        public Play()
        {
            InitializeComponent();
        }

        private void GoBackToHome(object sender, RoutedEventArgs e) {
            Home.home.Show();
            this.Hide();
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }
    }
}
