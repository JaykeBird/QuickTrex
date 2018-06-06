using QuickTranslate.Base;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickTranslate
{
    /// <summary>
    /// Interaction logic for TranslateItem.xaml
    /// </summary>
    public partial class TranslateItem : UserControl
    {
        public TranslateItem()
        {
            InitializeComponent();
        }

        public delegate void DropEventHandler(object sender, DropEventArgs e);

        public event TextChangedEventHandler TextChanged;
        public event EventHandler DragStart;
        public event EventHandler DragEnd;
        public event DropEventHandler ItemDrop;

        /// <summary>
        /// The visual column which the ID text field is displayed within.
        /// </summary>
        public ColumnDefinition ColumnId
        {
            get
            {
                return colId;
            }
        } 

        /// <summary>
        /// The visual column which the base translation is displayed within.
        /// </summary>
        public ColumnDefinition ColumnBaseTranslation
        {
            get
            {
                return colBase;
            }
        }

        /// <summary>
        /// The visual column which the translation is displayed within.
        /// </summary>
        public ColumnDefinition ColumnTranslation
        {
            get
            {
                return colTitem;
            }
        }

        /// <summary>
        /// The identifying name of the translatable string.
        /// </summary>
        public string Id
        {
            get
            {
                return txtId.Text;
            }
            set
            {
                txtId.Text = value;
            }
        }

        /// <summary>
        /// The value used in the base translation file (if one is opened).
        /// </summary>
        public string BaseString
        {
            get
            {
                return txtBase.Text;
            }
            set
            {
                txtBase.Text = value;
            }
        }

        /// <summary>
        /// The value of the translated string.
        /// </summary>
        public string TranslatedString
        {
            get
            {
                return txtTitem.Text;
            }
            set
            {
                txtTitem.Text = value;
            }
        }

        /// <summary>
        /// A representation of the translated string that this UI element is displaying.
        /// </summary>
        public Translation Translation
        {
            get
            {
                return new Translation(txtId.Text, txtTitem.Text);
            }
            set
            {
                txtId.Text = value.Id;
                txtTitem.Text = value.TranslatedItem;
            }
        }

        /// <summary>
        /// Determines whether or not this element is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return chkSelect.IsChecked.Value;
            }
            set
            {
                chkSelect.IsChecked = value;
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void txtTitem_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        #region Drag-Drop Support
        private void dragField_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragStart?.Invoke(this, EventArgs.Empty);
                DragDropEffects ef = DragDrop.DoDragDrop(this, Id + "\n" + BaseString + "\n" + TranslatedString, DragDropEffects.Move);
                DragEnd?.Invoke(this, EventArgs.Empty);

                if (ef == DragDropEffects.None)
                {
                    // no drop happened
                }
                else
                {
                    // drop happened
                    
                }
            }
        }

        /// <summary>
        /// Display the drop grids (used while another element is being dropped onto this one).
        /// </summary>
        public void ShowDropGrids()
        {
            dropGridAbove.Visibility = Visibility.Visible;
            dropGridBelow.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide the drop grids (used while another element is being dropped onto this one).
        /// </summary>
        public void HideDropGrids()
        {
            dropGridAbove.Visibility = Visibility.Collapsed;
            dropGridBelow.Visibility = Visibility.Collapsed;
            dropAboveShow.Visibility = Visibility.Collapsed;
            dropBelowShow.Visibility = Visibility.Collapsed;
        }

        private void dropGridAbove_DragEnter(object sender, DragEventArgs e)
        {
            dropAboveShow.Visibility = Visibility.Visible;
        }

        private void dropGridAbove_DragLeave(object sender, DragEventArgs e)
        {
            dropAboveShow.Visibility = Visibility.Collapsed;
        }

        private void dropGridAbove_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void dropGridAbove_Drop(object sender, DragEventArgs e)
        {
            ItemDrop?.Invoke(this, new DropEventArgs(this, true, (string) e.Data.GetData(DataFormats.StringFormat)));
        }

        private void dropGridBelow_DragEnter(object sender, DragEventArgs e)
        {
            dropBelowShow.Visibility = Visibility.Visible;
        }

        private void dropGridBelow_DragLeave(object sender, DragEventArgs e)
        {
            dropBelowShow.Visibility = Visibility.Collapsed;
        }

        private void dropGridBelow_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void dropGridBelow_Drop(object sender, DragEventArgs e)
        {
            ItemDrop?.Invoke(this, new DropEventArgs(this, false, (string)e.Data.GetData(DataFormats.StringFormat)));
        }
        #endregion

    }
}
