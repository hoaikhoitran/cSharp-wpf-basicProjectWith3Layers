using BusinessLayer.Services;
using RepositoryLayer;
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

namespace TranHoaiKhoiWpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserAccountService _service = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Login
            UserAccount? account = _service.Authenticate(EmailTextBox.Text, PasswordTextBox.Text);
            if(account == null)
            {
                MessageBox.Show("Invalid Email or Password", "Wrong credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Xuống đây là Account 1 record nào đó có role 1 2 3 4
            if(account.Role == 4)
            {
                MessageBox.Show("You have no permission to access this function!", "Access denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MainWindow mainWindow = new();
            //Role 1 và 2 đi tiếp sang main
            mainWindow.CurrentAccount = account; //đẩy account từ login thành công sang main window
            mainWindow.Show(); //Render màng hình chính
            this.Hide(); // Ẩn đi cửa sổ login
        }
    }
}
