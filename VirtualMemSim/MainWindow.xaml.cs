using System;
using System.Windows;
using VirtualMemLib;

namespace VirtualMemSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string LaptopInputFile = @"C:\Programs\VirtualMemorySim\input3a.data";
        private static string DesktopInputFile = @"C:\Programs\VirtualMemSim\input3a.data";
        private OSKernel _Kernel;

        public MainWindow()
        {
            InitializeComponent();
            _Kernel = new OSKernel(DesktopInputFile);
            this.DataContext = _Kernel;
        }

        /// <summary>
        /// Button used to traverse through the input file line by line.
        /// </summary>
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            pageTableGrid.Visibility = System.Windows.Visibility.Visible;
            procInfoGrid.Visibility = System.Windows.Visibility.Visible;
            splitter0.Visibility = System.Windows.Visibility.Visible;
            splitter1.Visibility = System.Windows.Visibility.Visible;
            if (_Kernel.NextLine())
            {
                _Kernel.PrintFrameTable();
                _Kernel.PrintPageTables();
                Console.WriteLine("-------------------------------------------");
            }
            else
            {
                _Kernel.PrintCurrentState();
                DisableButtons();
            }

            pageDataGrid.Focus();
        }

        /// <summary>
        /// Button used to traverse through input file until the next fault.
        /// </summary>
        private void runFaultButton_Click(object sender, RoutedEventArgs e)
        {
            pageTableGrid.Visibility = System.Windows.Visibility.Visible;
            procInfoGrid.Visibility = System.Windows.Visibility.Visible;
            splitter0.Visibility = System.Windows.Visibility.Visible;
            splitter1.Visibility = System.Windows.Visibility.Visible;
            pageDataGrid.Focus();
            if (_Kernel.NextFault())
            {
                _Kernel.PrintFrameTable();
                _Kernel.PrintPageTables();
            }
            else
            {
                _Kernel.PrintCurrentState();
                DisableButtons();
            }
        }

        /// <summary>
        /// Button used to traverse through the input file until completion.
        /// </summary>
        private void runCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            pageTableGrid.Visibility = System.Windows.Visibility.Visible;
            procInfoGrid.Visibility = System.Windows.Visibility.Visible;
            splitter0.Visibility = System.Windows.Visibility.Visible;
            splitter1.Visibility = System.Windows.Visibility.Visible;
            while (_Kernel.NextFault());
            _Kernel.PrintFrameTable();
            _Kernel.PrintPageTables();
            _Kernel.PrintCurrentState();
            DisableButtons();
            pageDataGrid.Focus();
        }

        /// <summary>
        /// Disables all buttons.
        /// </summary>
        private void DisableButtons()
        {
            runCompleteButton.IsEnabled = false;
            runFaultButton.IsEnabled = false;
            nextButton.IsEnabled = false;
        }


    }
}
