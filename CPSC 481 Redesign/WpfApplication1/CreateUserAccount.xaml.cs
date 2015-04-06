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
    /// Interaction logic for CreateUserAccount.xaml
    /// </summary>
    public partial class CreateUserAccount : Window
    {
        public CreateUserAccount()
        {
            InitializeComponent();
        }

        private void GoToMainWindow(object sender, RoutedEventArgs e) {
            MainWindow.mainwindow.Show();
            this.Hide();
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        // this function serves 2 purposes
        // create account if user havnt done so
        // sign in if user created an account
        private void CreateAnAccount(object sender, RoutedEventArgs e) {
            if (CreateAccountButton1.Content.Equals("Create Account")) {
                // verify password re-enter match
                if (!CreateAccountEnterPasswordTextBox.Text.Equals(CreateAccountReEnterPasswordTextBox.Text)) {
                    CreateAccountStatusBox.Text = "Password Re-entered does not match";
                    return;
                }

                String username = CreateAccountUserNameTextBox.Text;
                String password = CreateAccountEnterPasswordTextBox.Text;

                // empty username/password rejected
                if (username.Equals("")) {
                    CreateAccountStatusBox.Text = "Empty username not allowed";
                    return;
                }
                if (password.Equals("")) {
                    CreateAccountStatusBox.Text = "Empty password not allowed";
                    return;
                }

                try {
                    // save username and password in our database (text file)
                    // source: https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
                    // source: https://msdn.microsoft.com/en-us/library/system.io.directory.getcurrentdirectory%28v=vs.110%29.aspx
                    String path = System.IO.Directory.GetCurrentDirectory();
                    path += "\\UserAccounts.txt";
                    // note: this does not prevent duplicate entry :(
                    System.IO.File.AppendAllText(@path, username + " " + password + "\n");
                }
                catch (SystemException) {
                    // something wrong with textfile
                    Environment.Exit(0);
                }

                CreateAccountStatusBox.Text = "Account " + username + " created";
                CreateAccountButton1.Content = "Sign In";
                return;
            }

            if (CreateAccountButton1.Content.Equals("Sign In")) {
                // log user in if success
                Window w = new Home();
                w.Show();
                this.Hide();
                CreateAccountStatusBox.Text = "";
                CreateAccountButton1.Content = "Create Account";
            }
        }

        private void EnterPressed(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                CreateAnAccount(sender,e);
            }
        }
    }
}
