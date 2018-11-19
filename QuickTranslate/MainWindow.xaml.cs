using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using QuickTranslate.Base;
using QuickTranslate.Io;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Automation;

namespace QuickTranslate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitialKeyboardShortcutSetup();
        }

        bool setup = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rowFilter.Height = new GridLength(0);
            AddCategory("(None)");
            SelectCategory("(None)");
            MarkDirty(false);
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Categories

        List<Category> categories = new List<Category>();
        List<Category> baseCategories = new List<Category>();

        /// <summary>
        /// Represents name of the current category
        /// </summary>
        string catName = "(None)";

        /// <summary>
        /// Represents the currently selected category
        /// </summary>
        Category category = null;

        /// <summary>
        /// Represents the currently selected category from the base translation file
        /// </summary>
        Category baseCategory = null;

        #region Categories Pane

        private void mnuCategories_Click(object sender, RoutedEventArgs e)
        {
            if (mnuCategories.IsChecked)
            {
                mnuCategories.IsChecked = false;
            }
            else
            {
                mnuCategories.IsChecked = true;
            }
        }

        private void mnuCategories_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                colCategories.Width = new GridLength(150);
            }
            catch (NullReferenceException)
            {

            }
        }

        private void mnuCategories_Unchecked(object sender, RoutedEventArgs e)
        {
            colCategories.Width = new GridLength(0);
        }

        private void btnCloseCategories_Click(object sender, RoutedEventArgs e)
        {
            colCategories.Width = new GridLength(0);
            mnuCategories.IsChecked = false;
        }

        #endregion

        #region Category Events

        private void category_StylusDown(object sender, StylusDownEventArgs e)
        {
            SelectCategory((sender as Label).Content as string);
        }

        private void category_TouchDown(object sender, TouchEventArgs e)
        {
            SelectCategory((sender as Label).Content as string);
        }

        private void category_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectCategory((sender as Label).Content as string);
        }

        private void category_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                SelectCategory((sender as Label).Content as string);
            }
        }

        #endregion

        #region Base Methods

        void AddCategory(string name)
        {
            if (RegisterCategory(name))
            {
                if (name == "(None)")
                {
                    categories.Add(new Category(""));
                }
                else
                {
                    categories.Add(new Category(name));
                }
                MarkDirty(true);
            }

        }

        void AddCategory(Category cat)
        {
            string name = cat.Name;

            if (name == "")
            {
                name = "(None)";
            }

            if (RegisterCategory(name))
            {
                categories.Add(cat);
            }
        }

        bool RegisterCategory(string name)
        {
            foreach (Label item in stkCategories.Children)
            {
                if (item.Content as string == name)
                {
                    MessageBox.Show("There is already a category with the name \"" + name + "\".", "Cannot Add Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            Label tb = new Label();
            tb.Height = 24;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.Padding = new Thickness(10, 0, 0, 0);
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.HorizontalContentAlignment = HorizontalAlignment.Left;
            tb.Content = name;
            tb.Focusable = true;
            tb.IsTabStop = true;
            tb.KeyDown += category_KeyDown;
            tb.MouseDown += category_MouseDown;
            tb.TouchDown += category_TouchDown;
            tb.StylusDown += category_StylusDown;

            AutomationProperties.SetItemType(tb, "category item");
            AutomationProperties.SetItemStatus(tb, "not selected");

            stkCategories.Children.Add(tb);

            return true;
        }

        void DeleteCategory(string name)
        {
            Label sel = null;

            foreach (Label item in stkCategories.Children)
            {
                if (item.Content as string == name)
                {
                    sel = item;
                }
            }

            foreach (Category item in categories)
            {

            }

            if (sel != null)
            {
                SelectCategory("(None)");
                stkCategories.Children.Remove(sel);
            }
        }

        void ClearAllCategories()
        {
            stkCategories.Children.Clear();
            categories.Clear();
        }

        void SelectCategory(string name)
        {
            if (name == null)
            {
                return;
            }

            Label sel = null;

            foreach (Label item in stkCategories.Children)
            {
                item.Background = new SolidColorBrush(Colors.White);
                item.FontWeight = FontWeights.Normal;

                if (item.Content as string == name)
                {
                    sel = item;
                }
                else
                {
                    AutomationProperties.SetItemStatus(item, "not selected");
                }
            }

            if (sel != null)
            {
                sel.Background = new SolidColorBrush(Colors.LightGray);
                sel.FontWeight = FontWeights.SemiBold;
                catName = sel.Content as string;

                if (sel.Content as string == "(None)")
                {
                    mnuCategoryDelete.IsEnabled = false;
                }
                else
                {
                    mnuCategoryDelete.IsEnabled = true;
                }

                AutomationProperties.SetItemStatus(sel, "selected");

                SetupCategoryFromBackend(name);
            }
            else
            {
                catName = "(None)";
                SelectCategory("(None)");
            }
        }

        #endregion

        #region Other UI Items

        private void mnuCategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog aid = new AddItemDialog("category");

            aid.ShowDialog();

            if (aid.DialogResult)
            {
                if (string.IsNullOrWhiteSpace(aid.ItemName))
                {
                    MessageBox.Show("Category names cannot be blank.", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                AddCategory(aid.ItemName);
            }
        }

        private void mnuCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            if (catName != "(None)")
            {
                if (!confirmDelete || MessageBox.Show("Are you sure you want to delete the category \"" + catName + "\"?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DeleteCategory(catName);
                }
            }
        }

        #endregion

        void SetupCategoryFromBackend(string name)
        {

            if (name == "(None)")
            {
                name = "";
            }

            bool nameExists = false;

            foreach (Category item in categories)
            {
                if (item.Name == name)
                {
                    category = item;
                    nameExists = true;
                }
            }

            if (!nameExists)
            {
                categories.Add(new Category(name));
            }

            bool baseNameExists = false;

            foreach (Category item in baseCategories)
            {
                if (item.Name == name)
                {
                    baseCategory = item;
                    baseNameExists = true;
                }
            }

            if (!baseNameExists)
            {
                baseCategory = null;
            }

            stkTranslations.Children.Clear();

            List<Translation> baseTranslations = new List<Translation>();

            if (baseCategory != null)
            {
                baseTranslations = new List<Translation>(baseCategory.Translations);
            }

            foreach (Translation item in category.Translations)
            {
                string baseStr = "";

                if (baseCategory != null)
                {
                    Translation baseTranslation = baseCategory.GetTranslation(item.Id);
                    baseStr = baseTranslation.TranslatedItem;
                    baseTranslations.Remove(baseTranslation);
                }

                RegisterEntry(item.Id, baseStr, item.TranslatedItem);
            }

            if (baseCategory != null)
            {
                foreach (Translation item in baseTranslations)
                {
                    RegisterEntry(item.Id, item.TranslatedItem, "");
                }
            }
        }

        #endregion

        #region Entries

        bool confirmDelete = true;

        #region Base Methods

        void AddEntry(string name)
        {
            AddEntry(name, "", "");
        }

        void AddEntry(string name, string baseStr, string translated)
        {
            if (RegisterEntry(name, baseStr, translated))
            {
                category.AddTranslation(name, translated);
                MarkDirty(true);
            }
        }

        bool RegisterEntry(string name, string baseStr, string translated, int index = -1)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item.Id == name)
                {
                    MessageBox.Show("An entry already exists with the ID name \"" + name + "\".", "Cannot Add Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            TranslateItem ti = new TranslateItem();
            ti.VerticalAlignment = VerticalAlignment.Top;
            ti.HorizontalAlignment = HorizontalAlignment.Stretch;

            ti.ColumnId.Width = colId.Width;
            ti.ColumnBaseTranslation.Width = colBase.Width;
            ti.ColumnTranslation.Width = colTitem.Width;

            ti.Id = name;
            ti.BaseString = baseStr;
            ti.TranslatedString = translated;

            ti.TextChanged += TranslateItem_TextChanged;
            ti.DragStart += Ti_DragStart;
            ti.DragEnd += Ti_DragEnd;
            ti.ItemDrop += Ti_ItemDrop;
            
            if (index == -1)
            {
                stkTranslations.Children.Add(ti);
            }
            else
            {
                try
                {
                    stkTranslations.Children.Insert(index, ti);
                }
                catch (ArgumentOutOfRangeException)
                {
                    stkTranslations.Children.Add(ti);
                }
            }

            return true;
        }

        private void Ti_ItemDrop(object sender, DropEventArgs e)
        {
            string[] item = e.Data.Split('\n');
            string id = item[0];
            string baseStr = item[1];
            string tranStr = item[2];

            bool above = e.DropAbove;
            int index = stkTranslations.Children.IndexOf(e.TargetItem);

            category.RemoveTranslation(id);

            List<TranslateItem> lti = new List<TranslateItem>();

            foreach (TranslateItem titem in stkTranslations.Children)
            {
                if (titem.Id == id)
                {
                    lti.Add(titem);
                }
            }

            foreach (TranslateItem titem in lti)
            {
                stkTranslations.Children.Remove(titem);
            }

            if (!above)
            {
                index++;
            }

            if (RegisterEntry(id, baseStr, tranStr, index))
            {
                category.InsertTranslation(index, new Translation(id, tranStr));
            }

            MarkDirty(true);
        }

        private void Ti_DragEnd(object sender, EventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item != sender)
                {
                    item.HideDropGrids();
                }
            }
        }

        private void Ti_DragStart(object sender, EventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item != sender)
                {
                    item.ShowDropGrids();
                }
            }
        }

        private void TranslateItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (category == null || setup)
            {
                return;
            }

            foreach (Translation item in category.Translations)
            {
                if (item.Id == (sender as TranslateItem).Id)
                {
                    item.TranslatedItem = (sender as TranslateItem).TranslatedString;
                }
            }
        }

        #endregion

        #region UI

        private void spltId_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                item.ColumnId.Width = colId.Width;
                item.ColumnBaseTranslation.Width = colBase.Width;
                item.ColumnTranslation.Width = colTitem.Width;
            }
        }

        private void spltId_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                item.ColumnId.Width = colId.Width;
                item.ColumnBaseTranslation.Width = colBase.Width;
                item.ColumnTranslation.Width = colTitem.Width;
            }
        }

        private void mnuEntryAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog aid = new AddItemDialog("entry");

            aid.ShowDialog();

            if (aid.DialogResult)
            {
                AddEntry(aid.ItemName);
            }
        }

        private void mnuEntryDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!confirmDelete || MessageBox.Show(this, "Are you sure you want to delete the selected entries?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                List<TranslateItem> lti = new List<TranslateItem>();

                foreach (TranslateItem item in stkTranslations.Children)
                {
                    if (item.IsSelected)
                    {
                        lti.Add(item);
                    }
                }

                foreach (TranslateItem item in lti)
                {
                    category.RemoveTranslation(item.Id);
                    stkTranslations.Children.Remove(item);
                }

                MarkDirty(true);
            }
        }

        private void mnuConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (mnuConfirmDelete.IsChecked)
            {
                mnuConfirmDelete.IsChecked = false;
                confirmDelete = false;
            }
            else
            {
                mnuConfirmDelete.IsChecked = true;
                confirmDelete = true;
            }
        }

        #endregion

        #region Right-Click

        private void stkTranslations_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {

                // Background="#FFF0F0F0" MenuBackground="White" CheckedBrush="DarkGray" BorderBrush="Gray" DisabledBrush="Gray" HighlightBrush="LightGray" HighlightSubitemBrush="LightGray"
                ContextMenu mnu = FindResource("EntryStackMenu") as ContextMenu;
                mnu.MenuBackground = new SolidColorBrush(Colors.White);
                mnu.HighlightBrush = new SolidColorBrush(Colors.LightGray);
                mnu.HighlightSubitemBrush = new SolidColorBrush(Colors.LightGray);
                mnu.BorderBrush = new SolidColorBrush(Colors.Gray);
                mnu.CheckedBrush = new SolidColorBrush(Colors.DarkGray);

                mnu.PlacementTarget = stkTranslations;
                mnu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                mnu.VerticalOffset = 1;
                mnu.HorizontalOffset = 1;

                foreach (FrameworkElement item in mnu.Items)
                {
                    if (item.Name == "cmnuCategoryDelete" && catName == "(None)")
                    {
                        item.IsEnabled = false;
                    }
                    else
                    {
                        item.IsEnabled = true;
                    }
                }

                mnu.IsOpen = true;
            }
        }

        #endregion

        #endregion

        #region Edit Menu

        #region Copy/Cut/Paste

        private void mnuCopy_Click(object sender, RoutedEventArgs e)
        {
            List<TranslateItem> lti = new List<TranslateItem>();

            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item.IsSelected)
                {
                    lti.Add(item);
                }
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder st = new StringBuilder();

            foreach (TranslateItem item in lti)
            {
                sb.AppendLine(item.Id + ",\"" + item.BaseString + "\",\"" + item.TranslatedString + "\"");
                st.AppendLine(item.Id + "\t\"" + item.BaseString + "\"\t\"" + item.TranslatedString + "\"");
            }

            var dataObject = new DataObject();
            dataObject.SetText(st.ToString());

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var stream = new System.IO.MemoryStream(bytes);
            dataObject.SetData(DataFormats.CommaSeparatedValue, stream);

            Clipboard.SetDataObject(dataObject, true);
        }

        private void mnuCut_Click(object sender, RoutedEventArgs e)
        {
            List<TranslateItem> lti = new List<TranslateItem>();

            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item.IsSelected)
                {
                    lti.Add(item);
                }
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder st = new StringBuilder();

            foreach (TranslateItem item in lti)
            {
                sb.AppendLine(item.Id + ",\"" + item.BaseString + "\",\"" + item.TranslatedString + "\"");
                st.AppendLine(item.Id + "\t\"" + item.BaseString + "\"\t\"" + item.TranslatedString + "\"");
                category.RemoveTranslation(item.Id);
                stkTranslations.Children.Remove(item);
            }

            var dataObject = new DataObject();
            dataObject.SetText(st.ToString());

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var stream = new System.IO.MemoryStream(bytes);
            dataObject.SetData(DataFormats.CommaSeparatedValue, stream);

            Clipboard.SetDataObject(dataObject, true);

            MarkDirty(true);
        }

        private void mnuPaste_Click(object sender, RoutedEventArgs e)
        {
            string fmt_csv = DataFormats.CommaSeparatedValue;

            //read the CSV
            IDataObject dataobject = Clipboard.GetDataObject();
            //System.IO.Stream stream = (System.IO.Stream) dataobject.GetData(fmt_csv) as string).GetBytes();
            //Encoding enc = Encoding.UTF8;
            //System.IO.StreamReader reader = new System.IO.StreamReader(stream, enc);
            //string data_csv = reader.ReadToEnd();

            string data_csv = dataobject.GetData(fmt_csv) as string;

            string[] lines = data_csv.Split('\n');

            List<List<string>> matrix = new List<List<string>>();

            foreach (string item in lines)
            {
                bool quoteMode = false;
                bool escapeMode = false;

                List<string> stres = new List<string>();
                string curStr = "";

                foreach (char ch in item)
                {
                    if (escapeMode)
                    {
                        if (ch == '"')
                        {
                            curStr += "\"";
                        }
                        if (ch == ',')
                        {
                            curStr += ",";
                        }
                        if (ch == '\\')
                        {
                            curStr += "\\";
                        }
                    }
                    if (ch == '"')
                    {
                        quoteMode = !quoteMode;
                    }
                    else if (ch == '\\' && escapeMode == false)
                    {
                        escapeMode = true;
                    }
                    else if (ch == ',')
                    {
                        if (!quoteMode)
                        {
                            stres.Add(curStr);
                            curStr = "";
                        }
                    }
                    else if (ch == '\r')
                    {
                        // consume
                    }
                    else
                    {
                        curStr += ch;
                    }
                }

                stres.Add(curStr);

                matrix.Add(stres);
            }

            foreach (List<string> item in matrix)
            {
                if (item.Count >= 3)
                {
                    AddEntry(item[0], item[1], item[2]);
                }
            }

            MarkDirty(true);
        }

        #endregion

        #region Select

        private void mnuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    item.IsSelected = true;
                }
            }
        }

        private void mnuDeselect_Click(object sender, RoutedEventArgs e)
        {
            foreach (TranslateItem item in stkTranslations.Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    item.IsSelected = false;
                }
            }
        }

        #endregion

        #region Filter Bar

        private void mnuFilter_Click(object sender, RoutedEventArgs e)
        {
            rowFilter.Height = new GridLength(32);
        }

        private void btnCloseFilterBar_Click(object sender, RoutedEventArgs e)
        {
            rowFilter.Height = new GridLength(0);
            txtFilter.Text = "";
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtFilter.Text == "")
            {
                foreach (TranslateItem item in stkTranslations.Children)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
            else
            {
                string filterText = txtFilter.Text;

                foreach (TranslateItem item in stkTranslations.Children)
                {
                    if (chkCase.IsChecked.Value)
                    {
                        if (!item.Id.Contains(filterText) && !item.BaseString.Contains(filterText) && !item.TranslatedString.Contains(filterText))
                        {
                            item.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (!item.Id.ToLowerInvariant().Contains(filterText) && !item.BaseString.ToLowerInvariant().Contains(filterText) && !item.TranslatedString.ToLowerInvariant().Contains(filterText))
                        {
                            item.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region View Menu / Status Bar

        #region Path in Title Bar

        private void mnuFilePath_Click(object sender, RoutedEventArgs e)
        {
            mnuFilePath.IsChecked = !(mnuFilePath.IsChecked);
        }

        private void mnuFilePath_Checked(object sender, RoutedEventArgs e)
        {
            UpdateLocation(fileLocation);
        }

        private void mnuFilePath_Unchecked(object sender, RoutedEventArgs e)
        {
            Title = "QuickTrex";
        }

        #endregion

        #region Status Bar

        private void mnuStatusBar_Click(object sender, RoutedEventArgs e)
        {
            if (mnuStatusBar.IsChecked)
            {
                mnuStatusBar.IsChecked = false;
            }
            else
            {
                mnuStatusBar.IsChecked = true;
            }
        }

        private void mnuStatusBar_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rowStatusBar.Height = new GridLength(28);
                ResizeMode = ResizeMode.CanResizeWithGrip;
            }
            catch (NullReferenceException)
            {

            }
        }

        private void mnuStatusBar_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                rowStatusBar.Height = new GridLength(0);
                ResizeMode = ResizeMode.CanResize;
            }
            catch (NullReferenceException)
            {

            }
        }

        private void MarkDirty(bool dirty)
        {
            this.dirty = dirty;

            if (dirty)
            {
                lblUnsavedChanges.Visibility = Visibility.Visible;
            }
            else
            {
                lblUnsavedChanges.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #endregion

        #region File I/O (Open, Save, Export)

        /// <summary>
        /// The location as to which the file was loaded from or saved to. Used to continue referencing the file.
        /// </summary>
        string fileLocation = "";
        bool dirty = false;
        Document doc = new Document();
        Document baseDoc = new Document();

        #region UI

        void UpdateLocation(string loc)
        {
            fileLocation = loc;

            if (mnuFilePath.IsChecked)
            {
                if (loc != "")
                {
                    Title = "QuickTrex - " + loc;
                }
                else
                {
                    Title = "QuickTrex - (New File)";
                }
            }
        }

        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            if (dirty)
            {
                MessageBoxResult res = MessageBox.Show("Do you want to save your changes first?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);

                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (res == MessageBoxResult.Yes)
                {
                    mnuSave_Click(sender, e);
                }
            }

            ClearAllCategories();

            AddCategory("(None)");
            SelectCategory("(None)");

            UpdateLocation("");
            MarkDirty(false);
        }

        private void mnuOpen_Click(object sender, RoutedEventArgs e)
        {
            if (dirty)
            {
                MessageBoxResult res = MessageBox.Show("Do you want to save your changes first?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);

                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (res == MessageBoxResult.Yes)
                {
                    mnuSave_Click(sender, e);
                }
            }

            Document d = Open("Open");

            if (d == null || d.Categories == null)
            {
                return;
            }

            doc = d;

            ClearAllCategories();

            foreach (Category cat in d.Categories)
            {
                AddCategory(cat);
            }

            SelectCategory("(None)");
            MarkDirty(false);
        }

        private void mnuOpenAsBase_Click(object sender, RoutedEventArgs e)
        {
            Document d = Open("Open as Base");

            if (d == null || d.Categories == null)
            {
                return;
            }

            baseDoc = d;

            baseCategories = d.Categories;

            SelectCategory(catName);
        }

        private void mnuSave_Click(object sender, RoutedEventArgs e)
        {
            if (fileLocation == "")
            {
                mnuSaveAs_Click(sender, e);
            }
            else
            {
                doc.Categories = categories;
                XmlIo.SaveXmlFile(fileLocation, doc);
            }

            txtLastSaved.Text = DateTime.Now.ToString("g");
            MarkDirty(false);
        }

        private void mnuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            doc.Categories = categories;
            Save("Save", doc);

            txtLastSaved.Text = DateTime.Now.ToString("g");
            MarkDirty(false);
        }

        private void mnuExport_Click(object sender, RoutedEventArgs e)
        {
            Export("Export", categories);
        }

        private void mnuProperties_Click(object sender, RoutedEventArgs e)
        {
            PropertiesDialog pd = new PropertiesDialog(doc.Properties, baseDoc.Properties);

            pd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pd.Owner = this;
            pd.ShowDialog();

            if (pd.DialogResult == true)
            {
                doc.Properties = pd.Properties;
            }
        }

        #endregion

        public Document Open(string dialogTitle)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = dialogTitle;
            ofd.Filter = "XML Document|*.xml|All Files|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? res = ofd.ShowDialog(this);
            if (res.HasValue && res.Value)
            {
                UpdateLocation(ofd.FileName);

                return XmlIo.LoadXmlFile(ofd.FileName);
            }
            else
            {
                return null;
            }
        }

        public bool Save(string dialogTitle, Document item)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = dialogTitle;
            sfd.Filter = "XML Document|*.xml";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? res = sfd.ShowDialog(this);

            if (res.HasValue && res.Value)
            {
                XmlIo.SaveXmlFile(sfd.FileName, item);

                item.Filename = sfd.FileName;
                UpdateLocation(sfd.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Export(string dialogTitle, List<Category> items)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = dialogTitle;
            sfd.Filter = "Basic JSON List|*.json|Categorized JSON List|*.json|XAML ResourceDictionary|*.xaml|C# Dictionary|*.cs|Android Strings XML|*.xml|" +
                "Comma-Separated Values (CSV)|*.csv|Resources XML File (resx)|*.resx";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? res = sfd.ShowDialog(this);

            if (res.HasValue && res.Value)
            {
                int i = sfd.FilterIndex;

                switch (i)
                {
                    case 1:
                        JsonExport.ExportBasicJsonFile(sfd.FileName, items);
                        break;
                    case 2:
                        JsonExport.ExportCategorizedJsonFile(sfd.FileName, items);
                        break;
                    case 3:
                        XamlResExport.ExportXamlResDictionary(sfd.FileName, items);
                        break;
                    case 4:
                        ExportCsDialog ecd = new ExportCsDialog();
                        ecd.Owner = this;
                        ecd.ShowDialog();

                        if (ecd.DialogResult)
                        {
                            CsExport.ExportCsharpFile(sfd.FileName, items, ecd.NamespaceName, ecd.ClassName, ecd.UseReadOnlyDictionary);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 5:
                        AxmlExport.ExportAndroidStringFile(sfd.FileName, items);
                        break;
                    case 6:
                        ExportCsvDialog esd = new ExportCsvDialog();
                        esd.Owner = this;
                        esd.ShowDialog();

                        if (esd.DialogResult)
                        {
                            CsvExport.ExportCsvFile(sfd.FileName, esd.UseColumns, items);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 7:
                        ResxExport.ExportToResx(sfd.FileName, items);
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Keyboard Shortcuts

        #region Constants/Core Variables

        bool CtrlPressed = false;
        bool AltPressed = false;
        bool ShiftPressed = false;

        DispatcherTimer keyCheck;

        Dictionary<Key, RoutedEventHandler> ksr_None = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_Ctrl = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_Alt = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_Shift = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_AltShift = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_CtrlAlt = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_CtrlShift = new Dictionary<Key, RoutedEventHandler>();
        Dictionary<Key, RoutedEventHandler> ksr_CtrlAltShift = new Dictionary<Key, RoutedEventHandler>();

        public enum KeyboardCombination
        {
            None = 0,
            Ctrl = 1,
            Alt = 2,
            Shift = 3,
            AltShift = 4,
            CtrlAlt = 5,
            CtrlShift = 6,
            CtrlAltShift = 7,
        }

        public static Key[] allowedKeys = { Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12, Key.F13,
            Key.F14, Key.F15, Key.F16, Key.F17, Key.F18, Key.F19, Key.F20, Key.F21, Key.F22, Key.F23, Key.F24, Key.Delete, Key.Home, Key.PageDown,
            Key.PageUp, Key.End, Key.Insert, Key.Next, Key.Prior, Key.BrowserBack, Key.BrowserFavorites, Key.BrowserForward, Key.BrowserHome,
            Key.BrowserRefresh, Key.BrowserSearch, Key.BrowserStop, Key.MediaNextTrack, Key.MediaPlayPause, Key.MediaPreviousTrack, Key.MediaStop,
            Key.Pause, Key.Play, Key.Print, Key.Apps, Key.Help, Key.LaunchApplication1, Key.LaunchApplication2, Key.LaunchMail, Key.Zoom };

        private void KeyModifierCheck(object sender, EventArgs e)
        {
            if (CtrlPressed)
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    CtrlPressed = false;
                    txtCtrl.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }

            if (ShiftPressed)
            {
                if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                {
                    ShiftPressed = false;
                    txtShift.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }

            if (AltPressed)
            {
                if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt) && !Keyboard.IsKeyDown(Key.System))
                {
                    AltPressed = false;
                    txtAlt.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }

            if (!CtrlPressed && !ShiftPressed && !AltPressed)
            {
                keyCheck.Stop();
            }
        }

        #endregion

        private void InitialKeyboardShortcutSetup()
        {
            // set up keycheck timer
            // this timer is used to check that Ctrl, Alt, or Shift are still pressed
            // due to issues occurring if child dialogs open while modifiers are pressed
            keyCheck = new DispatcherTimer(new TimeSpan(400), DispatcherPriority.Input, KeyModifierCheck, this.Dispatcher);

            // set up default keyboard shortcuts
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.Insert, "EntryAdd");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.I, "EntryAdd");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.U, "CategoryAdd");
            RegisterKeyboardShortcut(KeyboardCombination.None, Key.Delete, "EntryDelete");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.Delete, "CategoryDelete");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.A, "SelectAll");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.D, "Deselect");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.F, "Filter");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.R, "Categories");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.S, "Save");
            RegisterKeyboardShortcut(KeyboardCombination.CtrlShift, Key.S, "SaveAs");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.E, "Export");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.O, "Open");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.C, "Copy");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.X, "Cut");
            RegisterKeyboardShortcut(KeyboardCombination.Ctrl, Key.V, "Paste");
        }

        #region Register/Unregister

        public void RegisterKeyboardShortcut(KeyboardCombination kc, Key key, RoutedEventHandler method)
        {
            switch (kc)
            {
                case KeyboardCombination.None:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_None.Add(key, method);
                    }
                    break;
                case KeyboardCombination.Ctrl:
                    ksr_Ctrl.Add(key, method);
                    break;
                case KeyboardCombination.Alt:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_Alt.Add(key, method);
                    }
                    break;
                case KeyboardCombination.Shift:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_Shift.Add(key, method);
                    }
                    break;
                case KeyboardCombination.AltShift:
                    ksr_AltShift.Add(key, method);
                    break;
                case KeyboardCombination.CtrlAlt:
                    ksr_CtrlAlt.Add(key, method);
                    break;
                case KeyboardCombination.CtrlShift:
                    ksr_CtrlShift.Add(key, method);
                    break;
                case KeyboardCombination.CtrlAltShift:
                    ksr_CtrlAltShift.Add(key, method);
                    break;
            }
        }

        public void RegisterKeyboardShortcut(KeyboardCombination kc, Key key, string methodId)
        {
            RoutedEventHandler method = GetMethodForCommand(methodId);
            MenuItem mi = GetCommandMenuItem(methodId);
            //ToolTip tt = new ToolTip();

            switch (kc)
            {
                case KeyboardCombination.None:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_None.Add(key, method);
                        //tt.Content = 
                        mi.InputGestureText = key.ToString("G");
                    }
                    break;
                case KeyboardCombination.Ctrl:
                    ksr_Ctrl.Add(key, method);
                    //tt.Content =
                    mi.InputGestureText = "Ctrl + " + key.ToString("G");
                    break;
                case KeyboardCombination.Alt:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_Alt.Add(key, method);
                        //tt.Content =
                        mi.InputGestureText = "Alt + " + key.ToString("G");
                    }
                    break;
                case KeyboardCombination.Shift:
                    if (allowedKeys.Contains(key))
                    {
                        ksr_Shift.Add(key, method);
                        //tt.Content =
                        mi.InputGestureText = "Shift + " + key.ToString("G");
                    }
                    break;
                case KeyboardCombination.AltShift:
                    ksr_AltShift.Add(key, method);
                    //tt.Content =
                    mi.InputGestureText = "Alt + Shift + " + key.ToString("G");
                    break;
                case KeyboardCombination.CtrlAlt:
                    ksr_CtrlAlt.Add(key, method);
                    //tt.Content =
                    mi.InputGestureText = "Ctrl + Alt + " + key.ToString("G");
                    break;
                case KeyboardCombination.CtrlShift:
                    ksr_CtrlShift.Add(key, method);
                    //tt.Content =
                    mi.InputGestureText = "Ctrl + Shift + " + key.ToString("G");
                    break;
                case KeyboardCombination.CtrlAltShift:
                    ksr_CtrlAltShift.Add(key, method);
                    //tt.Content =
                    mi.InputGestureText = "Ctrl + Alt + Shift + " + key.ToString("G");
                    break;
            }

            //if (mi != null)
            //{
            //    mi.ToolTip = tt;
            //}
        }

        public bool UnregisterKeyboardShortcut(KeyboardCombination kc, Key key)
        {
            bool success = true;

            switch (kc)
            {
                case KeyboardCombination.None:
                    try
                    {
                        return ksr_None.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.Ctrl:
                    try
                    {
                        return ksr_Ctrl.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.Alt:
                    try
                    {
                        return ksr_Alt.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.Shift:
                    try
                    {
                        return ksr_Shift.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.AltShift:
                    try
                    {
                        return ksr_AltShift.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.CtrlAlt:
                    try
                    {
                        return ksr_CtrlAlt.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.CtrlShift:
                    try
                    {
                        return ksr_CtrlShift.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
                case KeyboardCombination.CtrlAltShift:
                    try
                    {
                        return ksr_CtrlAltShift.Remove(key);
                    }
                    catch (ArgumentNullException)
                    {
                        success = false;
                    }
                    break;
            }

            return success;
        }

        #endregion

        #region KeyDown/KeyUp

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            // first, figure out what keys are pressed
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                CtrlPressed = true;
                keyCheck.Start();
                txtCtrl.Foreground = new SolidColorBrush(Colors.Black);

                return;
            }

            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                ShiftPressed = true;
                keyCheck.Start();
                txtShift.Foreground = new SolidColorBrush(Colors.Black);

                return;
            }

            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.System)
            {
                AltPressed = true;
                keyCheck.Start();
                txtAlt.Foreground = new SolidColorBrush(Colors.Black);

                return;
            }

            // secondly, check for keyboard shortcuts!

            if (CtrlPressed)
            {
                if (ShiftPressed)
                {
                    if (AltPressed)
                    {
                        // Ctrl + Shift + Alt + whatever
                        if (ksr_CtrlAltShift.ContainsKey(e.Key))
                        {
                            (ksr_CtrlAltShift[e.Key]).Invoke(this, new RoutedEventArgs());
                        }

                        return;
                    }

                    // Ctrl + Shift + whatever
                    if (ksr_CtrlShift.ContainsKey(e.Key))
                    {
                        (ksr_CtrlShift[e.Key]).Invoke(this, new RoutedEventArgs());
                    }

                    return;
                }

                if (AltPressed)
                {
                    // Ctrl + Alt + whatever
                    if (ksr_CtrlAlt.ContainsKey(e.Key))
                    {
                        (ksr_CtrlAlt[e.Key]).Invoke(this, new RoutedEventArgs());
                    }

                    return;
                }

                // Ctrl + whatever
                if (ksr_Ctrl.ContainsKey(e.Key))
                {
                    (ksr_Ctrl[e.Key]).Invoke(this, new RoutedEventArgs());
                }

                return;
            }

            if (AltPressed)
            {
                if (ShiftPressed)
                {
                    // Alt + Shift + whatever
                    if (ksr_AltShift.ContainsKey(e.Key))
                    {
                        (ksr_AltShift[e.Key]).Invoke(this, new RoutedEventArgs());
                    }

                    return;
                }

                // Alt + whatever
                // (Note: only some keys are allowed for Alt + key shortcuts)
                if (ksr_Alt.ContainsKey(e.Key))
                {
                    (ksr_Alt[e.Key]).Invoke(this, new RoutedEventArgs());
                }

                return;
            }

            if (ShiftPressed)
            {
                // Alt + whatever
                // (Note: only some keys are allowed for Shift + key shortcuts)
                if (ksr_Shift.ContainsKey(e.Key))
                {
                    (ksr_Shift[e.Key]).Invoke(this, new RoutedEventArgs());
                }

                return;
            }

            // finally, just keys with no modifiers
            // (Note: only some keys are allowed for unmodified shortcuts)
            if (ksr_None.ContainsKey(e.Key))
            {
                (ksr_None[e.Key]).Invoke(this, new RoutedEventArgs());
            }

            return;

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // use to monitor modifier key changes

            CtrlPressed &= (e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl);

            ShiftPressed &= (e.Key != Key.LeftShift && e.Key != Key.RightShift);

            AltPressed &= (e.Key != Key.LeftAlt && e.Key != Key.RightAlt && e.Key != Key.System);

            if (!CtrlPressed)
            {
                txtCtrl.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            if (!ShiftPressed)
            {
                txtShift.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            if (!AltPressed)
            {
                txtAlt.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        #endregion

        #region Menu Register

        /// <summary>
        /// Returns a RoutedEventMethod for the given item. This is primarily used for keyboard shortcuts and toolbar customizations.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <exception cref="ArgumentOutOfRangeException">There is no command by this name.</exception>
        /// <returns>A RoutedEventMethod, which can be used to invoke the given command.</returns>
        public RoutedEventHandler GetMethodForCommand(string command)
        {
            if (command.StartsWith("mnu", StringComparison.Ordinal))
            {
                command = command.Substring(2);
            }

            switch (command)
            {
                // File menu
                case "New":
                    return mnuNew_Click;
                case "Open":
                    return mnuOpen_Click;
                case "OpenAsBase":
                    return mnuOpenAsBase_Click;
                case "Save":
                    return mnuSave_Click;
                case "SaveAs":
                    return mnuSaveAs_Click;
                case "Export":
                    return mnuExport_Click;
                case "Exit":
                    return mnuExit_Click;
                // Edit menu
                case "Copy":
                    return mnuCopy_Click;
                case "Cut":
                    return mnuCut_Click;
                case "Paste":
                    return mnuPaste_Click;
                case "SelectAll":
                    return mnuSelectAll_Click;
                case "Deselect":
                    return mnuDeselect_Click;
                case "Filter":
                    return mnuFilter_Click;
                // View menu
                case "Categories":
                    return mnuCategories_Click;
                case "StatusBar":
                    return mnuStatusBar_Click;
                // Modify menu
                case "CategoryAdd":
                    return mnuCategoryAdd_Click;
                case "CategoryDelete":
                    return mnuCategoryDelete_Click;
                case "EntryAdd":
                    return mnuEntryAdd_Click;
                case "EntryDelete":
                    return mnuEntryDelete_Click;
                case "ConfirmDelete":
                    return mnuConfirmDelete_Click;
                // Help menu
                case "About":
                    return mnuAbout_Click;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), "There is no command that goes by this name.");
            }
        }

        /// <summary>
        /// Returns a MenuItem for the name provided. This is primarily used for keyboard shortcuts and toolbar customizations.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <exception cref="ArgumentOutOfRangeException">There is no command with this name.</exception>
        /// <returns>A MenuItem that, when clicked, activates that command.</returns>
        public MenuItem GetCommandMenuItem(string command)
        {
            if (command.StartsWith("mnu", StringComparison.Ordinal))
            {
                command = command.Substring(2);
            }

            switch (command)
            {
                // File menu
                case "New":
                    return mnuNew;
                case "Open":
                    return mnuOpen;
                case "OpenAsBase":
                    return mnuOpenAsBase;
                case "Save":
                    return mnuSave;
                case "SaveAs":
                    return mnuSaveAs;
                case "Export":
                    return mnuExport;
                case "Exit":
                    return mnuExit;
                // Edit menu
                case "Copy":
                    return mnuCopy;
                case "Cut":
                    return mnuCut;
                case "Paste":
                    return mnuPaste;
                case "SelectAll":
                    return mnuSelectAll;
                case "Deselect":
                    return mnuDeselect;
                case "Filter":
                    return mnuFilter;
                // View menu
                case "Categories":
                    return mnuCategories;
                case "StatusBar":
                    return mnuStatusBar;
                // Modify menu
                case "CategoryAdd":
                    return mnuCategoryAdd;
                case "CategoryDelete":
                    return mnuCategoryDelete;
                case "EntryAdd":
                    return mnuEntryAdd;
                case "EntryDelete":
                    return mnuEntryDelete;
                case "ConfirmDelete":
                    return mnuConfirmDelete;
                // Help menu
                case "About":
                    return mnuAbout;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), "There is no command that goes by this name.");
            }
        }



        #endregion

        #endregion


    }
}
