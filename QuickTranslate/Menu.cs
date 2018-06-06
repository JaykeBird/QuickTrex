using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace QuickTranslate
{
    /// <summary>
    /// A dropdown menu with a modified style.
    /// </summary>
    public class Menu : System.Windows.Controls.Menu
    {
        static Menu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(typeof(Menu)));
        }

        /// <summary>
        /// The background of the dropdown menu.
        /// </summary>
        public Brush MenuBackground
        {
            get
            {
                return (Brush)GetValue(MenuBackgroundProperty);
            }
            set
            {
                SetValue(MenuBackgroundProperty, value);
            }
        }

        /// <summary>
        /// The brush to use for disabled menu items.
        /// </summary>
        public Brush DisabledBrush
        {
            get
            {
                return (Brush)GetValue(DisabledBrushProperty);
            }
            set
            {
                SetValue(DisabledBrushProperty, value);
            }
        }

        /// <summary>
        /// The brush to use for the border of the dropdown menu.
        /// </summary>
        public new Brush BorderBrush
        {
            get
            {
                return (Brush)GetValue(BorderBrushProperty);
            }
            set
            {
                SetValue(BorderBrushProperty, value);
            }
        }

        /// <summary>
        /// The brush to use when the main menu item is highlighted (mouseover, keyboard focus, etc.)
        /// </summary>
        public Brush HighlightBrush
        {
            get
            {
                return (Brush)GetValue(HighlightBrushProperty);
            }
            set
            {
                SetValue(HighlightBrushProperty, value);
            }
        }

        /// <summary>
        /// The brush to use when a menu item in the dropdown menu is highlighted (mouseover, keyboard focus, etc.)s
        /// </summary>
        public Brush HighlightSubitemBrush
        {
            get
            {
                return (Brush)GetValue(HighlightSubitemBrushProperty);
            }
            set
            {
                SetValue(HighlightSubitemBrushProperty, value);
            }
        }

        /// <summary>
        /// The brush to use when a menu item is checked.
        /// </summary>
        public Brush CheckedBrush
        {
            get
            {
                return (Brush)GetValue(CheckedBrushProperty);
            }
            set
            {
                SetValue(CheckedBrushProperty, value);
            }
        }

        public static DependencyProperty MenuBackgroundProperty = DependencyProperty.Register(
            "MenuBackground", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public static DependencyProperty DisabledBrushProperty = DependencyProperty.Register(
            "DisabledBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static new DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public static DependencyProperty HighlightSubitemBrushProperty = DependencyProperty.Register(
            "HighlightSubitemBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static DependencyProperty HighlightBrushProperty = DependencyProperty.Register(
            "HighlightBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static DependencyProperty CheckedBrushProperty = DependencyProperty.Register(
            "CheckedBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(new SolidColorBrush(Colors.Gainsboro)));

    }
}
