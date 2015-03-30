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

        private void CreateAnAccount(object sender, RoutedEventArgs e) {
            // verify password re-enter match
            if (!CreateAccountEnterPasswordTextBox.Text.Equals(CreateAccountReEnterPasswordTextBox.Text)) {
                return;
            }

            String username = CreateAccountUserNameTextBox.Text;
            String password = CreateAccountEnterPasswordTextBox.Text;

            try {
                // save username and password in our database (text file)
                // source: https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
                // source: https://msdn.microsoft.com/en-us/library/system.io.directory.getcurrentdirectory%28v=vs.110%29.aspx
                String path = System.IO.Directory.GetCurrentDirectory();
                path += "\\UserAccounts.txt";
                System.IO.File.AppendAllText(@path, username + " " + password + "\n");
            }
            catch(SystemException) {
                // something wrong with textfile
                Environment.Exit(0);
            }

            // log user in if success
            Window w = new Home();
            w.Show();
            this.Hide();
        }
    }
}
