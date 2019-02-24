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

namespace QuickTranslate
{
    /// <summary>
    /// Interaction logic for ExportCsvDialog.xaml
    /// </summary>
    public partial class ExportCsvDialog : Window
    {
        public ExportCsvDialog()
        {
            InitializeComponent();
            btnOk.Focus();
        }

        public new bool DialogResult { get; set; } = false;

        public bool UseColumns { get; private set; } = false;

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            UseColumns = checkBox.IsChecked.Value;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
