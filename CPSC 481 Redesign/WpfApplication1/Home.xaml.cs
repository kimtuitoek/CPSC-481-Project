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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public static Window friends, groups, home, play;
        
        public Home()
        {
            InitializeComponent();
            initializeWindows();

            // debug
            //System.Diagnostics.Debug.WriteLine(UsernameBox.Text);
        }

        private void initializeWindows() {
            friends = new Friends();
            friends.Hide();
            groups = new Groups();
            groups.Hide();
            home = this;
            play = new Play();
            play.Hide();
        }

        private void OpenFriendsPage(object sender, RoutedEventArgs e) {
            // display username on Friends window
            Grid g = (Grid)friends.Content;
            TextBlock tb = (TextBlock)g.Children[2];
            tb.Text = UsernameBox.Text;
            //System.Diagnostics.Debug.WriteLine(UsernameBox.Text);

            friends.Show();
            this.Hide();
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void OpenGroupsPage(object sender, RoutedEventArgs e) {
            // display username on Groups window


            groups.Show();
            this.Hide();
        }

        private void OpenPlayPage(object sender, RoutedEventArgs e) {
            play.Show();
            this.Hide();
        }
    }
}
