﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void textBlock_Copy5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://twitter.com/JaykeBird");
        }

        private void textBlock_Copy5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Process.Start("http://twitter.com/JaykeBird");
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textBlock_Copy8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/JaykeBird/QuickTrex");
        }

        private void textBlock_Copy8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Process.Start("https://github.com/JaykeBird/QuickTrex");
            }
        }

    }
}
