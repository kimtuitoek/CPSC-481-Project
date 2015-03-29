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
            // go through Other Groups list, add all that are checked
            int i = 0;
            while (OtherGroups.HasItems)
            {
                try {
                    Object obj = OtherGroups.Items[i++];
                    if (obj != null) {
                        // get selected object
                        StackPanel sp = (StackPanel)obj;

                         // check if checkbox is selected
                        CheckBox cb = (CheckBox)sp.Children[4];
                        if (!(bool)cb.IsChecked) {
                            continue;
                        }

                         // check if this group has already been added
                        bool added = false;
                        TextBlock tb = (TextBlock)sp.Children[2];
                        int j = 0;
                        while (YourGroups.HasItems) {
                            try {
                                Object obje = YourGroups.Items[j++];
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
    }
}
