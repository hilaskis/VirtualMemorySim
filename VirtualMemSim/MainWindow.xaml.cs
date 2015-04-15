﻿using System;
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
using VirtualMemLib;

namespace VirtualMemSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OSKernel _Kernel;
        public MainWindow()
        {
            InitializeComponent();
            _Kernel = new OSKernel(@"C:\Programs\VirtualMemSim\input3a.data");
            this.DataContext = _Kernel;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _Kernel.NextLine();
        }


    }
}
