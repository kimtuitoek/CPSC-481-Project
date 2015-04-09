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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Window createuseraccount, mainwindow;
        //public static TextBlock mainwindowtb = SignInErrorBox;

        public MainWindow()
        {
            InitializeComponent();

            // initialize windows
            createuseraccount = new CreateUserAccount();
            mainwindow = this;
        }

        private void OpenCreateAccount(object sender, RoutedEventArgs e) {
            SignInErrorBox.Text = "";
            createuseraccount.Show();
            this.Hide();
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void SignIn(object sender, RoutedEventArgs e) {
            // get database
            // source: https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
            String path = System.IO.Directory.GetCurrentDirectory();
            path += "\\UserAccounts.txt";
            String[] content = System.IO.File.ReadAllLines(@path);
            
            // debug
            //for (int i = 0; i < content.Length; i++) {
                //System.Diagnostics.Debug.WriteLine(content[i]);
            //}

            // parse username/password and verify
            for (int i = 0; i < content.Length; i++) {
                String line = content[i];

                // find delimiter
                int delimiter = 0;
                for (; delimiter < line.Length; delimiter++) {
                    if (line[delimiter] == ' ') {
                        break;
                    }
                }

                // debug
                //System.Diagnostics.Debug.WriteLine((delimiter+1) + " " + (line.Length-1));
                //System.Diagnostics.Debug.WriteLine(line);

                // record username/password
                String username = line.Substring(0,delimiter);
                String password = line.Substring(delimiter+1,line.Length-1-delimiter);
                //password = password.Substring(1, password.Length - 1);

                // debug
                //System.Diagnostics.Debug.WriteLine("username: " + username + "\npassword: " + password + "\n");
                //System.Diagnostics.Debug.WriteLine("userInputUsername: " + MainWindowUserNameTextBox.Text +
                    //"\nuserInputPassword: " + MainWindowPasswordTextBox.Text + "\n");
                //System.Diagnostics.Debug.WriteLine("username match: " + username.Equals(MainWindowUserNameTextBox.Text) +
                    //"\npassword match: " + password.Equals(MainWindowPasswordTextBox.Text) + "\n");

                // log user in if matching crediental found
                if (username.Equals(MainWindowUserNameTextBox.Text) && password.Equals(MainWindowPasswordBox.Password)) {
                    Window w = new Home();
                    w.Show();
                    this.Hide();

                    // hide sign in error message after a successful login
                    SignInErrorBox.Text = "";

                    // display username on Home window
                    Grid g = (Grid)w.Content;
                    TextBlock tb = (TextBlock)g.Children[2];
                    tb.Text = "Welcome " + username;
                    //System.Diagnostics.Debug.WriteLine(g.Children[2]);

                    break;
                }
            }
            
            // else, login crediental not found
            SignInErrorBox.Text = "Wrong Username or Password";
        }

        // sign in if enter is pressed
        private void CheckEnterAndSignIn(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SignIn(sender,e);
            }
        }
    }
}
