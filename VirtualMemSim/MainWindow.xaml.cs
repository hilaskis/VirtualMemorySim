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
            _Kernel.NextLine();
            pageTableGrid.Visibility = System.Windows.Visibility.Visible;
            procInfoGrid.Visibility = System.Windows.Visibility.Visible;
            splitter0.Visibility = System.Windows.Visibility.Visible;
            splitter1.Visibility = System.Windows.Visibility.Visible;
        }


    }
}
