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
using System.Drawing;

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
                            cb = (CheckBox)sp1.Children[4];
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

        private void OpenPlay(object sender, RoutedEventArgs e) {
            Home.play.Show();
            this.Hide();
        }

        private void SearchFriend(object sender, RoutedEventArgs e) {
            // get database
            // source: https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
            String path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Users.txt";
            String[] content = System.IO.File.ReadAllLines(@path);

            // debug
            //for (int i = 0; i < content.Length; i++) {
                //System.Diagnostics.Debug.WriteLine(content[i]);
            //}

            // retrive name, level, and image, and compare with the requested search
            String requested = FriendsSearchBox.Text;
            for (int i = 0; i < content.Length; i++) {
                String line = content[i];

                // debug
                //System.Diagnostics.Debug.WriteLine(line);

                int delimiter = 0; 
                delimiter = FindNextDelimiter(line, delimiter);
                String name = line.Substring(0, delimiter);

                // debug
                //System.Diagnostics.Debug.WriteLine(delimiter);

                // check if this is the requested search
                // source: https://msdn.microsoft.com/en-us/library/cc165449.aspx
                if (!requested.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }
                
                int prev = delimiter+1;
                delimiter = FindNextDelimiter(line, prev);
                String level = line.Substring(prev,delimiter-prev);

                // debug
                //System.Diagnostics.Debug.WriteLine(prev);
                //System.Diagnostics.Debug.WriteLine(delimiter);
                //System.Diagnostics.Debug.WriteLine(line.Length);

                prev = delimiter+1;
                delimiter = FindNextDelimiter(line, prev);
                String image = line.Substring(prev,delimiter-prev);

                // debug
                //System.Diagnostics.Debug.WriteLine(delimiter);
                //System.Diagnostics.Debug.WriteLine(line.Length);

                // debug
                //System.Diagnostics.Debug.WriteLine(name+" "+level+" "+image+"\n");
                //System.Diagnostics.Debug.WriteLine(name+"\t"+level+"\n");

                // load player information
                LoadPlayer(name,level,image);
            }
        }

        // used by SearchFriend to locate the delimiter ';'
        // return length of string if no such delimiter found
        private int FindNextDelimiter(String s, int prevDelimiter) {
            if (s.Length == 0) {
                return 0;
            }

            int delimiter = prevDelimiter;
            for (; delimiter < s.Length; delimiter++) {
                if (s[delimiter] == ';') {
                    return delimiter;
                }
            }

            return s.Length;
        }

        // constructs a stack panel with image and player info displayed
        private void LoadPlayer(String name, String level, String image) {
            StackPanel sp = new StackPanel();
            sp.Width = 233;
            sp.Height = 75;
            sp.Orientation = Orientation.Horizontal;
            sp.HorizontalAlignment = HorizontalAlignment.Stretch;
            sp.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Visibility = Visibility.Visible;

            // set background color
            // source: http://stackoverflow.com/questions/18020779/how-to-use-code-behind-to-create-stackpanel-border-background
            BrushConverter scb = new BrushConverter();
            sp.Background = (Brush)scb.ConvertFrom("#FFF08011");

            // add image
            // source: https://msdn.microsoft.com/en-uS/office/office365/windows.ui.xaml.controls.image.source.aspx
            Image i = new Image();
            BitmapImage bi = new BitmapImage();
            //Uri u = new Uri(image);
            //bi.UriSource = u;
            i.Source = bi;
            i.Stretch = Stretch.Uniform;
            i.StretchDirection = StretchDirection.Both;
            i.Width = 56;
            i.Height = 56;  // auto in the xaml, not sure how to set it here
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Children.Add(i);

            // add dummy button1
            Button b = new Button();
            b.Width = 14;
            b.Height = 74;
            b.Visibility = Visibility.Hidden;
            sp.Children.Add(b);

            // add textblock
            TextBlock tb = new TextBlock();
            tb.Text += name;
            tb.Text += ' ';
            tb.Text += level;
            sp.Children.Add(tb);

            // add dummy button2
            // same as dummy1
            Button bu = new Button();
            bu.Width = 14;
            bu.Height = 74;
            bu.Visibility = Visibility.Hidden;
            sp.Children.Add(bu);

            // add checkbox
            CheckBox cb = new CheckBox();
            sp.Children.Add(cb);

            // finally, add this stackpanel to our UI
            FriendsListBox.Items.Add(sp);
        }
    }
}
