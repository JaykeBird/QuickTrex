using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace QuickTranslate
{
    /// <summary>
    /// Interaction logic for AddItemDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        public AddItemDialog()
        {
            InitializeComponent();
        }

        public AddItemDialog(string itemType)
        {
            InitializeComponent();

            Title = "Add " + itemType;
            txtDesc.Text = "Name of the " + itemType + ":";
        }
        
        public AddItemDialog(string itemType, string name)
        {
            InitializeComponent();

            Title = "Add " + itemType;
            txtDesc.Text = "Name of the " + itemType + ":";

            txtName.Text = name;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }

        public string ItemName { get; set; }
        public new bool DialogResult { get; set; } = false;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            ItemName = txtName.Text;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOK_Click(this, new RoutedEventArgs());
            }
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            DialogHelper.HideExtraCaptionButtons(this);
        }
    }
}
