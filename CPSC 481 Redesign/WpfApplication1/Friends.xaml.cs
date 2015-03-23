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
using System.Windows.Markup;
using System.IO;
using System.Xml;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void AddFriend(object sender, RoutedEventArgs e) {
            // go through search friends, add all thats checked
            int i = 0;
            while (FriendsListBox.HasItems) {
                try {
                    Object obj = FriendsListBox.Items[i++];
                    if (obj != null) {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                        // check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked) {
                            continue;
                        }

                        // check if this friend is already added
                        bool added = false;
                        TextBlock tb = (TextBlock)sp.Children[2];
                        int j = 0;
                        while (FriendsBox.HasItems) {
                            try {
                                Object obje = FriendsBox.Items[j++];
                                StackPanel spa = (StackPanel)obje;
                                TextBlock tbl = (TextBlock)spa.Children[2];

                                if (tb.Text.Equals(tbl.Text)) {
                                    added = true;
                                    break;
                                }
                            }
                            catch (ArgumentOutOfRangeException) {
                                break;
                            }
                        }

                        // otherwise, add friend
                        if (!added) {
                            // debug
                            //FriendsSearchBox.Text = FriendsListBox.SelectedItem.ToString();

                            // create a deep copy (clone)
                            // source: https://msdn.microsoft.com/en-us/library/system.windows.markup.xamlwriter%28v=vs.110%29.aspx
                            String savedStackPanel = XamlWriter.Save(sp);
                            StringReader sr = new StringReader(savedStackPanel);
                            XmlReader xr = XmlReader.Create(sr);
                            StackPanel sp1 = (StackPanel)XamlReader.Load(xr);

                            // cloned stackpanel should not have checkbox selected
                            cb = (CheckBox)sp1.FindName("FriendsCheckBox");
                            cb.IsChecked = false;

                            // add item to the friend list
                            FriendsBox.Items.Add(sp1);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) {
                    break;
                }
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e) {
            // go through friends list, remove all thats checked
            int i = 0;

            // debug
            //FriendsSearchBox.Text = i.ToString();

            while (FriendsBox.HasItems) {
                try {
                    // debug
                    //FriendsSearchBox.Text = i.ToString();

                    Object obj = FriendsBox.Items[i++];

                    if (obj != null) {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                        // check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked) {
                            continue;
                        }
                        else {
                            // remove friend
                            FriendsBox.Items.Remove(obj);

                            // debug
                            //FriendsSearchBox.Text = sp.Name;
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) {
                    break;
                }
            }
        }

        private void Quit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void OpenGroup(object sender, RoutedEventArgs e) {
            Home.groups.Show();
            this.Hide();
        }

        private void OpenPlay(object sender, RoutedEventArgs e)
        {
            Home.play.Show();
            this.Hide();
        }
    }
}
