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
    /// Interaction logic for ExportCsDialog.xaml
    /// </summary>
    public partial class ExportCsDialog : Window
    {
        public new bool DialogResult { get; set; } = false;
        
        public ExportCsDialog()
        {
            InitializeComponent();
            btnOk.Focus();
        }

        public string NamespaceName { get; private set; } = "";
        public string ClassName { get; private set; } = "";
        public bool UseReadOnlyDictionary { get; private set; } = false;

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            NamespaceName = txtNamespace.Text;
            ClassName = txtClass.Text;
            UseReadOnlyDictionary = checkBox.IsChecked.Value;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            DialogHelper.HideExtraCaptionButtons(this);
        }
    }
}
