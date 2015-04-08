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

        // variables for used in add/remove friends confirmation
        //bool Yes = false;
        //bool No = false;
        Window promptWindow;
        //StackPanel sp1,sp;
        //TextBlock tb;
        //Object obj,obj1;
        //bool add = false;   // true = add friend, false = remove friend
        bool clicked = false;
        int operation;  // 0 = add friend, 1 = remove friend

        // event handler for Yes Button
        private void YesPressed(object sender, RoutedEventArgs e) {
            clicked = true;
            promptWindow.Hide();
            this.IsEnabled = true;
            if (operation == 0) {
                AddFriend(sender,e);
            }
            if (operation == 1) {
                RemoveFriend(sender,e);
            }
            clicked = false;
        }

        private void NoPressed(object sender, RoutedEventArgs e) {
            //clicked = true;
            promptWindow.Hide();
            this.IsEnabled = true;
            //clicked = false;
        }

        // mode: 0 = yes and no button
        // mode: 1 = ok button
        private void PopWindow(String message, int mode, int op) {
            operation = op;
            promptWindow = new Window();
            //promptWindow.Background = System.Drawing
            promptWindow.WindowStyle = WindowStyle.ToolWindow;
            promptWindow.Width = 350;
            promptWindow.Height = 100;
            promptWindow.ResizeMode = ResizeMode.NoResize;
            promptWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            TextBlock tbl = new TextBlock();
            tbl.HorizontalAlignment = HorizontalAlignment.Center;
            tbl.VerticalAlignment = VerticalAlignment.Center;
            tbl.FontFamily = new FontFamily("Segoe UI"); // source: https://msdn.microsoft.com/en-us/library/system.windows.controls.textblock.fontfamily%28v=vs.110%29.aspx
            tbl.FontSize = 20;
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.TextAlignment = TextAlignment.Center;
            //tbl.Text = "Add Freind " + tblo.Text + "?";
            tbl.Text = message;
            StackPanel spa = new StackPanel();
            spa.Orientation = Orientation.Vertical;
            spa.Children.Add(tbl);
            StackPanel span = new StackPanel();
            span.Orientation = Orientation.Horizontal;
            spa.Children.Add(span);
            span.HorizontalAlignment = HorizontalAlignment.Center;
            span.VerticalAlignment = VerticalAlignment.Center;
            Button bu = new Button();
            bu.Width = 60;
            bu.Height = 30;
            bu.Content = "Yes";
            bu.Name = "Yes";
            if (mode == 1) {
                bu.Visibility = Visibility.Hidden;
            }
            // link an eventhandler
            // source: https://msdn.microsoft.com/en-us/library/ms743596%28v=vs.110%29.aspx
            bu.Click += new RoutedEventHandler(YesPressed);
            span.Children.Add(bu);
            Button bu1 = new Button();
            bu1.Width = 60;
            bu1.Height = 30;
            bu1.Content = "Ok";
            bu1.Name = "Ok";
            if (mode == 0) {
                bu1.Visibility = Visibility.Hidden;
            }
            // link an eventhandler
            // source: https://msdn.microsoft.com/en-us/library/ms743596%28v=vs.110%29.aspx
            bu1.Click += new RoutedEventHandler(NoPressed);
            span.Children.Add(bu1);
            Button bu2 = new Button();
            bu2.Width = 60;
            bu2.Height = 30;
            bu2.Content = "No";
            bu2.Name = "No";
            if (mode == 1) {
                bu2.Visibility = Visibility.Hidden;
            }
            // link an eventhandler
            // source: https://msdn.microsoft.com/en-us/library/ms743596%28v=vs.110%29.aspx
            bu2.Click += new RoutedEventHandler(NoPressed);
            span.Children.Add(bu2);
            promptWindow.Content = spa;
            promptWindow.Show();
        }

        private void AddFriend(object sender, RoutedEventArgs e) {
            if (!clicked) {
                // prompt user for conformation before adding
                //add = true;
                //TextBlock tblo = (TextBlock)sp1.Children[2];
                String message = "Confirm Add Freinds?";
                PopWindow(message, 0, 0);
                this.IsEnabled = false;
                return;
            }

            // wait for user confirmation
            //while (!clicked) {}
            //clicked = false;

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
                                    // tell user this friend already added
                                    //MyFriendsActions.Text = "Friend " + tb.Text + " already added";
                                    String message = "Friend " + tb.Text + " already added";
                                    PopWindow(message,1,2); // op = 2, neither add friend, nor remove friends
                                    this.IsEnabled = false;
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

                            // display this operation on UI
                            //MyFriendsActions.Text = "Friend " + tb.Text + " added";
                            MyFriendsActions.Text = "Selected Friends Added";
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) {
                    break;
                }
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e) {
            if (!clicked) {
                String message = "Confirm Remove Freinds?";
                PopWindow(message, 0, 1);
                this.IsEnabled = false;
                return;
            }

            // wait for user confirmation
            //while (!clicked) {}
            //clicked = false;

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
                            //add = false;
                            // remove friend
                            FriendsBox.Items.Remove(obj);

                            // debug
                            //FriendsSearchBox.Text = sp.Name;

                            // display this operation on UI
                            TextBlock tb = (TextBlock)sp.Children[2];
                            //MyFriendsActions.Text = "Friend " + tb.Text + " removed";
                            MyFriendsActions.Text = "Selected Friends Removed";
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
            // do no search if nothing is typed in
            if (FriendsSearchBox.Text.Equals("")) {
                FriendsListBox.Items.Clear();   // clear list if nothing is searched
                MyFriendsSearchActions.Text = "";
                return;
            }

            // get database
            // source: https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
            String path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Users.txt";
            String[] content = System.IO.File.ReadAllLines(@path);

            // debug
            //for (int i = 0; i < content.Length; i++) {
                //System.Diagnostics.Debug.WriteLine(content[i]);
            //}

            // clear all existing items in the search panel before adding newly searched items
            FriendsListBox.Items.Clear();

            // retrive name, level, and image, and compare with the requested search
            String requested = FriendsSearchBox.Text;
            int added = 0;  // keep track of how many items added, later to be displayed
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
                

                // debug
                //System.Diagnostics.Debug.WriteLine("name: " + name.Length + "\nrequested: " + requested.Length);
                String toCompare;
                try {
                    toCompare = name.Substring(0, requested.Length);
                }
                catch(ArgumentOutOfRangeException) {
                    continue;
                }

                // debug
                //System.Diagnostics.Debug.WriteLine(toCompare + ' ' + requested);

                // source: https://msdn.microsoft.com/en-us/library/cc165449.aspx
                if (!requested.Equals(toCompare, StringComparison.OrdinalIgnoreCase)) {
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
                added++;
            }

            // display number of matches found on UI
            MyFriendsSearchActions.Text = "Found " + added + " Matches";
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
            sp.Width = 270;
            sp.Height = 75;
            sp.Orientation = Orientation.Horizontal;
            sp.HorizontalAlignment = HorizontalAlignment.Stretch;
            sp.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Visibility = Visibility.Visible;

            // set background color
            // source: http://stackoverflow.com/questions/18020779/how-to-use-code-behind-to-create-stackpanel-border-background
            BrushConverter scb = new BrushConverter();
            sp.Background = (Brush)scb.ConvertFrom("#FF353535");

            // add image
            // source: https://msdn.microsoft.com/en-us/library/system.windows.controls.image.source%28v=vs.110%29.aspx
            Image i = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            Uri u = new Uri(image, UriKind.Relative);
            bi.UriSource = u;
            bi.EndInit();
            i.Source = bi;
            i.Stretch = Stretch.Fill;
            i.StretchDirection = StretchDirection.Both;
            i.Width = 57;
            i.Height = 55;  // auto in the xaml, not sure how to set it here
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Children.Add(i);

            // add dummy button1
            Button b = new Button();
            b.Width = 32;
            b.Height = 74;
            b.Visibility = Visibility.Hidden;
            sp.Children.Add(b);

            // add textblock
            TextBlock tb = new TextBlock();
            tb.Foreground = (Brush)scb.ConvertFrom("#FFF9F5F5");
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 118;
            //tb.Height = 80; // auto in the xaml :(
            tb.FontFamily = new FontFamily("Segoe UI"); // source: https://msdn.microsoft.com/en-us/library/system.windows.controls.textblock.fontfamily%28v=vs.110%29.aspx
            tb.FontSize = 20;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text += name;
            tb.Text += " lvl ";
            tb.Text += level;
            sp.Children.Add(tb);

            // add dummy button2
            // same as dummy1
            Button bu = new Button();
            bu.Width = 32;
            bu.Height = 74;
            bu.Visibility = Visibility.Hidden;
            sp.Children.Add(bu);

            // add checkbox
            CheckBox cb = new CheckBox();
            cb.HorizontalAlignment = HorizontalAlignment.Center;
            cb.VerticalAlignment = VerticalAlignment.Center;
            sp.Children.Add(cb);

            // finally, add this stackpanel to our UI
            FriendsListBox.Items.Add(sp);
        }
    }
}
