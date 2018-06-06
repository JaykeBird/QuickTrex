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
    /// Interaction logic for Properties.xaml
    /// </summary>
    public partial class PropertiesDialog : Window
    {
        public PropertiesDialog()
        {
            InitializeComponent();
        }

        public new bool DialogResult { get; set; } = false;

        public PropertiesDialog(Base.Properties props, Base.Properties baseProps)
        {
            InitializeComponent();

            Properties = props;
            BaseProperties = baseProps;

            txtAuthor.Text = props.Author;
            txtDescription.Text = props.Description;
            txtLanguage.Text = props.Language;
            txtProduct.Text = props.Product;
            txtTranslator.Text = props.Translator;
            txtTranslatorContact.Text = props.TranslatorContact;
        }

        public Base.Properties Properties { get; set; }
        public Base.Properties BaseProperties { get; set; }

        public void LoadProperties()
        {
            txtAuthor.Text = Properties.Author;
            txtDescription.Text = Properties.Description;
            txtLanguage.Text = Properties.Language;
            txtProduct.Text = Properties.Product;
            txtTranslator.Text = Properties.Translator;
            txtTranslatorContact.Text = Properties.TranslatorContact;
        }

        private void btnLoadBase_Click(object sender, RoutedEventArgs e)
        {
            if (BaseProperties != null)
            {
                txtAuthor.Text = BaseProperties.Author;
                txtDescription.Text = BaseProperties.Description;
                txtLanguage.Text = BaseProperties.Language;
                txtProduct.Text = BaseProperties.Product;
                txtTranslator.Text = BaseProperties.Translator;
                txtTranslatorContact.Text = BaseProperties.TranslatorContact;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Properties.Author = txtAuthor.Text;
            Properties.Description = txtDescription.Text;
            Properties.Language = txtLanguage.Text;
            Properties.Product = txtProduct.Text;
            Properties.Translator = txtTranslator.Text;
            Properties.TranslatorContact = txtTranslatorContact.Text;

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
