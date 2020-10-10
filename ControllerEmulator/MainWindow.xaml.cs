namespace ControllerEmulator
{
    using ControllerEmulatorAPI.UserInterface.Pages;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainPage mainPage = new MainPage(Application.Current);

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(mainPage);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainPage.Dispose();
        }
    }
}