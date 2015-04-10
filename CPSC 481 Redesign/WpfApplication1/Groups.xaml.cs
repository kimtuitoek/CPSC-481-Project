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

        Window promptWindow;
        bool clicked = false;
        int operation;  // 0 = add friend, 1 = remove friend


        // event handler for Yes Button
        private void YesPressed(object sender, RoutedEventArgs e)
        {
            clicked = true;
            promptWindow.Hide();
            this.IsEnabled = true;
            if (operation == 0)
            {
                JoinGroup(sender, e);
            }
            if (operation == 1)
            {
                LeaveGroup(sender, e);
            }
            clicked = false;
        }

        private void NoPressed(object sender, RoutedEventArgs e)
        {
            //clicked = true;
            promptWindow.Hide();
            this.IsEnabled = true;
            //clicked = false;
        }

        // mode: 0 = yes and no button
        // mode: 1 = ok button
        private void PopWindow(String message, int mode, int op)
        {
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
            if (mode == 1)
            {
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
            if (mode == 0)
            {
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
            if (mode == 1)
            {
                bu2.Visibility = Visibility.Hidden;
            }
            // link an eventhandler
            // source: https://msdn.microsoft.com/en-us/library/ms743596%28v=vs.110%29.aspx
            bu2.Click += new RoutedEventHandler(NoPressed);
            span.Children.Add(bu2);
            promptWindow.Content = spa;
            promptWindow.Show();
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
            /*//Confirm choice
            MessageBoxResult result = MessageBox.Show("Do you want to leave these groups?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }*/
            String message = "Do you want to leave these groups?";
            int popUp_type = 0;
            int opCode = 1;

            if (!clicked)
            {
                // prompt user for conformation before adding
                //add = true;
                //TextBlock tblo = (TextBlock)sp1.Children[2];
                //String message = "Do you want to add these groups?";
                PopWindow(message, popUp_type, opCode);
                this.IsEnabled = false;
                return;
            }

            // go through the YourGroups list, remove all that are checked
            int i = 0;
            bool anyChecked = false;
            while (YourGroups.HasItems)
            {
                try
                {
                    Object obj = YourGroups.Items[i++];

                    if (obj != null)
                    {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                        //check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked)
                        {
                            continue;
                        }
                        else
                        {
                           
                            //-------remove group from YourGroups-------
                            YourGroups.Items.Remove(obj);
                            anyChecked = true;
                            i--;

                            // check if this group has already exists in OtherGroups
                            TextBlock tb = (TextBlock)sp.Children[2];
                            int j = 0;
                            while (YourGroups.HasItems)
                            {
                                try
                                {
                                    Object obje = OtherGroups.Items[j++];
                                    StackPanel spa = (StackPanel)obje;
                                    TextBlock tbl = (TextBlock)spa.Children[2];

                                    if (tb.Text.Equals(tbl.Text))
                                    {
                                        OtherGroups.Items.Remove(obje);
                                        break;
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    break;
                                }
                            }

                            //--------Add to other groups list-------
                            // create a deep copy (clone)
                            String savedStackPanel = XamlWriter.Save(sp);
                            StringReader sr = new StringReader(savedStackPanel);
                            XmlReader xr = XmlReader.Create(sr);
                            StackPanel sp1 = (StackPanel)XamlReader.Load(xr);

                            // cloned stackpanel should not have checkbox selected
                            cb = (CheckBox)sp1.Children[4];
                            cb.IsChecked = false;
                            cb.Visibility = Visibility.Visible; //Ensure the checkbox is visible in your groups

                            // add item to the friend list
                            OtherGroups.Items.Add(sp1);

                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }

            if (!anyChecked)
            {
                message = "No groups selected";
                popUp_type = 1;
                opCode = 2;
                PopWindow(message, popUp_type, opCode);
                this.IsEnabled = false;
                return;
            }

            //Display confirmation message
            YourGroupsMessage.Text = "All selected groups removed";
        }

        private void JoinGroup(object sender, RoutedEventArgs e)
        {
           /* //Confirm choice
            MessageBoxResult result = MessageBox.Show("Do you want to add these groups?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }*/
            String message = "Do you want to add these groups?";
            int popUp_type = 0;
            int opCode = 0;
           
            if (!clicked)
            {
                // prompt user for conformation before adding
                //add = true;
                //TextBlock tblo = (TextBlock)sp1.Children[2];
                //String message = "Do you want to add these groups?";
                PopWindow(message, popUp_type, opCode);
                this.IsEnabled = false;
                return;
            }

            // go through Other Groups list, add all that are checked
            int i = 0;
            bool anyChecked = false;
            bool anyDisabled = false;
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

                        anyChecked = true;

                        /*
                        // check if this group is already added
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
                                    // tell user this friend already added
                                    //MyFriendsActions.Text = "Friend " + tb.Text + " already added";
                                    String message = "Group(s) " + tb.Text + " already added";
                                    PopWindow(message, 1, 2); // op = 2, neither add friend, nor remove friends
                                    this.IsEnabled = false;
                                    cb.IsEnabled = false;
                                    break;
                                }
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                break;
                            }
                        }
                        */
                        // otherwise, add group
                        if (cb.IsEnabled)
                        {
                            // create a deep copy (clone)
                            // source: https://msdn.microsoft.com/en-us/library/system.windows.markup.xamlwriter%28v=vs.110%29.aspx
                            String savedStackPanel = XamlWriter.Save(sp);
                            StringReader sr = new StringReader(savedStackPanel);
                            XmlReader xr = XmlReader.Create(sr);
                            StackPanel sp1 = (StackPanel)XamlReader.Load(xr);

                            //Make checkbox hidden in other groups
                            cb.IsEnabled = false;
                            anyDisabled = true;
                            cb.IsChecked = true;

                            // cloned stackpanel should not have checkbox selected
                            cb = (CheckBox)sp1.Children[4];
                            cb.IsChecked = false;
                            cb.Visibility = Visibility.Visible; //Ensure the checkbox is visible in your groups

                            // add item to the friend list
                            YourGroups.Items.Add(sp1);

                            //Display confirmation message
                            YourGroupsMessage.Text = "All selected groups added";
                       }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }

            if (!anyChecked || !anyDisabled)
            {

                message = "No groups selected";
                popUp_type = 1;
                opCode = 2;
                PopWindow(message, popUp_type, opCode);
                this.IsEnabled = false;
                return;

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

            // retrive Group name, and image, and compare with the requested search
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

        private void SearchOtherGroup(object sender, TextChangedEventArgs e)
        {
            // do no search if nothing is typed in
            if (JoinGroupBox.Text.Equals(""))
            {
                OtherGroups.Items.Clear();   // clear list if nothing is searched
                OtherGroupsMessage.Text = "";
                return;
            }

            // get database
            // source: https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
            String path = System.IO.Directory.GetCurrentDirectory();
            path += "\\Groups.txt";
            String[] content = System.IO.File.ReadAllLines(@path);

            // debug
            //for (int i = 0; i < content.Length; i++) {
            //System.Diagnostics.Debug.WriteLine(content[i]);
            //}

            // clear all existing items in the search panel before adding newly searched items
            OtherGroups.Items.Clear();

            // retrive Group name and image, and compare with the requested search
            String requested = JoinGroupBox.Text;
            int added = 0;  // keep track of how many items added, later to be displayed
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


                // debug
                //System.Diagnostics.Debug.WriteLine("name: " + name.Length + "\nrequested: " + requested.Length);
                String toCompare;
                try
                {
                    toCompare = name.Substring(0, requested.Length);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }

                // debug
                //System.Diagnostics.Debug.WriteLine(toCompare + ' ' + requested);

                // source: https://msdn.microsoft.com/en-us/library/cc165449.aspx
                if (!requested.Equals(toCompare, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // debug
                //System.Diagnostics.Debug.WriteLine(prev);
                //System.Diagnostics.Debug.WriteLine(delimiter);
                //System.Diagnostics.Debug.WriteLine(line.Length);

                int prev = delimiter + 1;
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
                added++;
            }

            // display number of matches found on UI
            OtherGroupsMessage.Text = "Found " + added + " Matches";
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
            // source: https://msdn.microsoft.com/en-uS/office/office365/windows.ui.xaml.controls.image.source.aspx
            Image i = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            Uri u = new Uri(image, UriKind.Relative);
            bi.UriSource = u;
            bi.EndInit();
            i.Source = bi;
            i.Stretch = Stretch.Fill;
            i.StretchDirection = StretchDirection.Both;
            i.Width = 62;
            i.Height = 55;  // auto in the xaml, not sure how to set it here
            i.HorizontalAlignment = HorizontalAlignment.Left;
            i.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Children.Add(i);

            // add dummy button1
            Button b = new Button();
            b.Width = 32;
            b.Height = 90;
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
            sp.Children.Add(tb);

            // add dummy button2
            // same as dummy1
            Button bu = new Button();
            bu.Width = 32;
            bu.Height = 90;
            bu.Visibility = Visibility.Hidden;
            sp.Children.Add(bu);

            // add checkbox
            CheckBox cb = new CheckBox();
            cb.HorizontalAlignment = HorizontalAlignment.Center;
            cb.VerticalAlignment = VerticalAlignment.Center;
            sp.Children.Add(cb);

            //Check whether group has already been added
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
                        cb.IsEnabled = false;
                        cb.IsChecked = true;
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }

            // finally, add this stackpanel to our UI
            OtherGroups.Items.Add(sp);
        }

        private void SearchYourGroups(object sender, TextChangedEventArgs e)
        {
            
            // retrive Group name and compare with the requested search
           String requested = YouGroupBox.Text;
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

                       // Get text from text  box in stack panel
                       TextBlock tb = (TextBlock)sp.Children[2];

                       string name = tb.Text;
                      
                       //Make comparison
                       String toCompare;
                       try
                       {
                           toCompare = name.Substring(0, requested.Length);
                       }

                       catch (ArgumentOutOfRangeException)
                       {
                           continue;
                       }
                       
                       //Display all stack panels if entered string is blank
                       if (requested == "'")
                       {
                           sp.Visibility = Visibility.Visible;
                       }
                        
                        //Otherwise hide stack panel
                       else if (!requested.Equals(toCompare, StringComparison.OrdinalIgnoreCase))
                       {
                           sp.Visibility = Visibility.Collapsed;
                           continue;
                       }
                       else
                       {
                           sp.Visibility = Visibility.Visible;
                       }
                       
                   }
               }
               catch (ArgumentOutOfRangeException)
               {
                   break;
               }
           }
        }


     

    }
}