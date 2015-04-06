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
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class Groups : Window
    {
        public Groups()
        {
            InitializeComponent();
        }

        private void GoToFriends(object sender, RoutedEventArgs e)
        {
            Home.friends.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LeaveGroup(object sender, RoutedEventArgs e)
        {
            //Confirm choice
            MessageBoxResult result = MessageBox.Show("Do you want to leave these groups?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            // go through the YourGroups list, remove all that are checked
            int i = 0;

            while (YourGroups.HasItems)
            {
                try
                {
                    Object obj = YourGroups.Items[i++];

                    if (obj != null)
                    {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                        // check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked)
                        {
                            continue;
                        }
                        else
                        {
                            // remove group
                            YourGroups.Items.Remove(obj);

                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }
        }

        private void JoinGroup(object sender, RoutedEventArgs e)
        {
            //Confirm choice
            MessageBoxResult result = MessageBox.Show("Do you want to add these groups?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            // go through Other Groups list, add all that are checked
            int i = 0;
            while (OtherGroups.HasItems)
            {
                try
                {
                    Object obj = OtherGroups.Items[i++];
                    if (obj != null)
                    {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                        // check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked)
                        {
                            continue;
                        }

                        // check if this group has already been added
                        bool added = false;
                        TextBlock tb = (TextBlock)sp.Children[2];
                        int j = 0;
                        while (YourGroups.HasItems)
                        {
                            try
                            {
                                Object obje = YourGroups.Items[j++];
                                StackPanel spa = (StackPanel)obje;
                                TextBlock tbl = (TextBlock)spa.Children[2];

                                if (tb.Text.Equals(tbl.Text))
                                {
                                    added = true;
                                    break;
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                break;
                            }
                        }

                        // otherwise, add friend
                        if (!added)
                        {
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
                            YourGroups.Items.Add(sp1);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OpenPlay(object sender, RoutedEventArgs e)
        {
            Home.play.Show();
            this.Hide();
        }

        private void SearchGroup(object sender, RoutedEventArgs e)
        {
            // get database
            // source: https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
            String path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Groups.txt";
            String[] content = System.IO.File.ReadAllLines(@path);

            // debug
            //for (int i = 0; i < content.Length; i++) {
            //System.Diagnostics.Debug.WriteLine(content[i]);
            //}

            // retrive name, level, and image, and compare with the requested search
            String requested = JoinGroupBox.Text;
            for (int i = 0; i < content.Length; i++)
            {
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
                if (!requested.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int prev = delimiter + 1;

                // debug
                //System.Diagnostics.Debug.WriteLine(prev);
                //System.Diagnostics.Debug.WriteLine(delimiter);
                //System.Diagnostics.Debug.WriteLine(line.Length);

                delimiter = FindNextDelimiter(line, prev);
                String image = line.Substring(prev, delimiter - prev);

                // debug
                //System.Diagnostics.Debug.WriteLine(delimiter);
                //System.Diagnostics.Debug.WriteLine(line.Length);

                // debug
                //System.Diagnostics.Debug.WriteLine(name+" "+level+" "+image+"\n");
                //System.Diagnostics.Debug.WriteLine(name+"\t"+level+"\n");

                // load player information
                LoadGroup(name, image);
            }
        }

        // used by SearchGroup to locate the delimiter ';'
        // return length of string if no such delimiter found
        private int FindNextDelimiter(String s, int prevDelimiter)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            int delimiter = prevDelimiter;
            for (; delimiter < s.Length; delimiter++)
            {
                if (s[delimiter] == ';')
                {
                    return delimiter;
                }
            }

            return s.Length;
        }

        // constructs a stack panel with image and group name displayed
        private void LoadGroup(String name, String image)
        {
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
            b.Width = 29;
            b.Height = 90;
            b.Visibility = Visibility.Hidden;
            sp.Children.Add(b);

            // add textblock
            TextBlock tb = new TextBlock();
            tb.Text += name;
            sp.Children.Add(tb);

            // add dummy button2
            // same as dummy1
            Button bu = new Button();
            bu.Width = 29;
            bu.Height = 90;
            bu.Visibility = Visibility.Hidden;
            sp.Children.Add(bu);

            // add checkbox
            CheckBox cb = new CheckBox();
            sp.Children.Add(cb);

            // finally, add this stackpanel to our UI
            OtherGroups.Items.Add(sp);
        }
    }
}